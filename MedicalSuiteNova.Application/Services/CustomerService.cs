using AutoMapper;
using ClosedXML.Excel;
using MedicalSuiteNova.Application.Constants;
using MedicalSuiteNova.Application.Enums;
using MedicalSuiteNova.Application.Interfaces;
using MedicalSuiteNova.Domain.Dto.Appointment;
using MedicalSuiteNova.Domain.Dto.Customer;
using MedicalSuiteNova.Domain.Dto.Request;
using MedicalSuiteNova.Domain.Dto.Responses;
using MedicalSuiteNova.Domain.Entities;
using MedicalSuiteNova.Domain.Interfaces;
using MedicalSuiteNova.Util;
using MedicalSuiteNova.Utils;
using Microsoft.AspNetCore.Http;
using System.Diagnostics;

namespace MedicalSuiteNova.Application.Services
{
    public class CustomerService(IFileStorageService _fileStorage, IUnitOfWork uow, IMapper mapper) : BaseService<Customer>(uow, mapper, uow.Customers), ICustomerService
    {
        public async Task<List<CustomerDashboardDto>> GetDashboard()
        {
            var dashboardList = new List<CustomerDashboardDto>();
            var now = DateTime.UtcNow;
            var firstDayCurrentMonth = new DateTime(now.Year, now.Month, 1);
            var firstDayLastMonth = firstDayCurrentMonth.AddMonths(-1);

            var allCustomers = await _uow.Customers.GetAllAsync();
            int totalCount = allCustomers.Count;
            int totalLastMonth = allCustomers.Count(c => c.CreatedAt < firstDayCurrentMonth);
            int totalNewThisMonth = allCustomers.Count(c => c.CreatedAt >= firstDayCurrentMonth);
            var change = StatHelper.CalculateTrend(totalLastMonth, totalNewThisMonth);
            dashboardList.Add(new CustomerDashboardDto
            {
                Title = "Total Pacientes",
                Value = totalCount.ToString(),
                Change = change.ToString(),
                Trend = change >= 0 ? "Up" : "Down",
                Description = "vs last month"
            });

            return dashboardList;
        }

        public async Task<List<CustomerRiskDashboard>> GetCustomerRiskDashboard(int customerId)
        {
            var customer = await _uow.Customers.FindAsync(customerId);
            if (customer == null)
                return [];

            List<CustomerRiskDashboard> dashboardList = [];
            dashboardList.Add(await GetAppointmentRisk(customerId));
            dashboardList.Add(await GetPaymentRisk(customer));

            return dashboardList;
        }

        public async Task<CustomerRiskDashboard> GetAppointmentRisk(int customerId)
        {
            var missedAppointment = await _uow.Appointments.GetAllAsync(
                a => a.CustomerId == customerId && (
                a.StatusId == (int)AppointmentStatusEnum.NoShow ||
                a.StatusId == (int)AppointmentStatusEnum.Cancelled));

            var appointmentTotal = missedAppointment.Count;
            RiskLevelEnum riskLevel = CalculateRisk(appointmentTotal);

            return new CustomerRiskDashboard
            {
                Title = "Riesgo de no presentarse a la cita",
                Description = $"{appointmentTotal} {(appointmentTotal > 1 ? "citas" : "cita")} perdidas",
                RiskLevel = riskLevel.ToString()
            };
        }

        public async Task<CustomerRiskDashboard> GetPaymentRisk(Customer customer)
        {
            var diff = 0;
            string description = "Sin deudas pendientes";

            if (customer.Balance > 0)
            {
                var payments = await _uow.Payments.GetAllAsync(p => p.CustomerId == customer.Id);
                if (payments != null && payments.Count > 0)
                {
                    var latestPayment = payments.OrderBy(p => p.Date).Last();
                    diff = DateTimeHelper.CalculateDiffMonthDate(latestPayment.Date, DateTime.UtcNow);
                    description = $"Ultimo pago hace {diff} {(diff > 1 ? "meses" : "mes")}";
                }
            }

            RiskLevelEnum riskLevel = CalculateRisk(diff);

            return new CustomerRiskDashboard
            {
                Title = "Riesgo de pago",
                Description = description,
                RiskLevel = riskLevel.ToString()
            };
        }

        private static RiskLevelEnum CalculateRisk(int qnty)
        {
            return qnty switch
            {
                0 => RiskLevelEnum.Low,
                1 => RiskLevelEnum.Medium,
                _ => RiskLevelEnum.High
            };
        }

        public async Task<AppointmentInfoDto?> GetCustomerNextAppointment(int customerId)
        {
            return await _uow.Appointments.GetNextByCustomerAsync(customerId);
        }

        public async Task<Result<CustomerDto>> AddAsync(CreateCustomerDto dto)
        {
            var existDNI = await _uow.Customers.FirstOrDefaultAsync(c => c.DNI == dto.DNI);
            if (existDNI != null)
                return Result<CustomerDto>.Failure("Ya existe la identificación");

            var validateResult = ValidateAsync(dto);
            if (!validateResult.IsSuccess)
                return Result<CustomerDto>.Failure(validateResult.ErrorMessage);

            var customer = await _uow.Customers.AddAsync(_mapper.Map<Customer>(dto));
            await _uow.CompleteAsync();

            return Result<CustomerDto>.Success(_mapper.Map<CustomerDto>(customer));
        }

        public async Task<Result<CustomerDto>> UpdateAsync(int id, UpdateCustomerDto dto)
        {
            var customer = await _uow.Customers.FindAsync(id);
            if (customer == null)
                return Result<CustomerDto>.Failure("Id no encontrado");

            var validateResult = ValidateAsync(dto);
            if (!validateResult.IsSuccess)
                return Result<CustomerDto>.Failure(validateResult.ErrorMessage);

            _mapper.Map(dto, customer);
            await _uow.Customers.UpdateAsync(customer);
            await _uow.CompleteAsync();

            return Result<CustomerDto>.Success(_mapper.Map<CustomerDto>(customer));
        }

        private static Result<string> ValidateAsync(ICustomerValidatable dto)
        {
            if (!PhoneHelper.ValidatePhoneNumber(dto.Phone!))
                return Result<string>.Failure("Teléfono inválido");

            if (!MailHelper.IsValidEmail(dto.Email!))
                return Result<string>.Failure("Email inválido");

            var (isValid, message) = DateTimeHelper.ValidateBirthDate(dto.BirthDate);
            if (!isValid)
                return Result<string>.Failure(message);

            return Result<string>.Success("");
        }

        public async Task<ResponseImportResult> BulkImport(List<CustomerImportDto> dtos)
        {
            var stopwatch = Stopwatch.StartNew();
            var response = new ResponseImportResult { TotalRows = dtos.Count };

            var dniList = dtos.Select(x => (x.DNI).ToLower()).ToList();
            var dniExists = await _uow.Customers.GetAllAsync(p => dniList.Contains(p.DNI.ToLower()));

            var setExists = dniExists
                .Select(p => (p.FirstName.Trim() + p.LastName.Trim()).ToLower())
                .ToHashSet();

            var newCustomers = new List<Customer>();

            int index = 0;
            foreach (var d in dtos)
            {
                index++;

                var DNI = d.DNI.Trim();
                var firstName = d.FirstName?.Trim();
                var lastName = d.LastName?.Trim();
                var email = d.Email?.Trim();
                var phone = d.Phone?.Trim();

                var llave = (firstName + lastName).ToLower();

                if (string.IsNullOrWhiteSpace(DNI))
                {
                    response.Errors.Add(new RowError
                    {
                        RowNumber = index,
                        ErrorMessage = "DNI vacío"
                    });
                    continue;
                }

                if (string.IsNullOrWhiteSpace(firstName) || string.IsNullOrWhiteSpace(lastName))
                {
                    response.Errors.Add(new RowError
                    {
                        RowNumber = index,
                        ErrorMessage = "Nombre o apellido vacío"
                    });
                    continue;
                }

                if (!string.IsNullOrWhiteSpace(email) && !MailHelper.IsValidEmail(email))
                {
                    response.Errors.Add(new RowError
                    {
                        RowNumber = index,
                        ErrorMessage = "Email inválido"
                    });
                    continue;
                }

                if (!string.IsNullOrWhiteSpace(phone) && !PhoneHelper.ValidatePhoneNumber(phone))
                {
                    response.Errors.Add(new RowError
                    {
                        RowNumber = index,
                        ErrorMessage = "Teléfono inválido"
                    });
                    continue;
                }

                if (setExists.Contains(llave))
                {
                    response.Errors.Add(new RowError
                    {
                        RowNumber = index,
                        ErrorMessage = $"El cliente {d.DNI} ya existe en el sistema."
                    });
                }
                else
                {
                    newCustomers.Add(new Customer
                    {
                        DNI = DNI,
                        FirstName = firstName,
                        LastName = lastName,
                        Phone = phone,
                        Email = email,
                        CreatedAt = DateTime.UtcNow
                    });
                    // Agregamos la llave al set temporal para evitar duplicados dentro del mismo Excel
                    setExists.Add(llave);
                }
            }

            if (newCustomers.Count > 0)
            {
                await _uow.Customers.AddRangeAsync(newCustomers);
                await _uow.CompleteAsync();
            }

            stopwatch.Stop();
            response.SuccessCount = newCustomers.Count;
            response.ProcessingTimeSeconds = stopwatch.Elapsed.TotalSeconds;

            return response;
        }
       

        public async Task<Result<string>>UploadAvatarAsync(int id, IFormFile file)
        {
            var customer = await _uow.Customers.FindAsync(id);

            if (customer == null)
                return Result<string>.Failure("Cliente no encontrado.");

            var result = await _fileStorage.SaveAsync(
                file,
                FolderName.AvatarFolder,
                [".jpg", ".jpeg", ".png", ".webp" ]);

            if (!result.IsSuccess)
                return Result<string>.Failure(result.ErrorMessage);

            if (!string.IsNullOrEmpty(customer.Avatar))
            {
                string oldFile = Path.Combine(
                    Directory.GetCurrentDirectory(),
                    FolderName.RootFolder,
                    FolderName.UploadFolder,
                    FolderName.AvatarFolder,
                    customer.Avatar);

                if (File.Exists(oldFile))
                    File.Delete(oldFile);
            }

            customer.Avatar = result.Value.StoredName;

            await _uow.Customers.UpdateAsync(customer);
            await _uow.CompleteAsync();

            return Result<string>.Success("");
        }
    }
}
