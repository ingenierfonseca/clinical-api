using AutoMapper;
using ClosedXML.Excel;
using DocumentFormat.OpenXml.InkML;
using DocumentFormat.OpenXml.Wordprocessing;
using MedicalSuiteNova.Application.Interfaces;
using MedicalSuiteNova.Domain.Dto;
using MedicalSuiteNova.Domain.Dto.Request;
using MedicalSuiteNova.Domain.Dto.Responses;
using MedicalSuiteNova.Domain.Entities;
using MedicalSuiteNova.Util;
using MedicalSuiteNova.Utils;
using Microsoft.AspNetCore.Http;
using System.Diagnostics;

namespace MedicalSuiteNova.Application.Services
{
    public class CustomerService : BaseService<Customer>, ICustomerService
    {
        public CustomerService(IUnitOfWork uow, IMapper mapper) : base(uow, mapper, uow.Customers) { }

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

        public async Task<ResponseImportResult> BulkImport(List<CustomerImportDto> dtos)
        {
            var stopwatch = Stopwatch.StartNew();
            var response = new ResponseImportResult { TotalRows = dtos.Count };

            //var combinaciones = dtos.Select(x => (x.FirstName + x.LastName).ToLower()).ToList();
            //var fullNamesExists = await _uow.Customers.GetAllAsync(p => combinaciones.Contains((p.FirstName.Trim() + p.LastName.Trim()).ToLower()));
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

                if (d.Age > 120)
                {
                    response.Errors.Add(new RowError
                    {
                        RowNumber = index,
                        ErrorMessage = "Edad inválido"
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

                if (!string.IsNullOrWhiteSpace(phone) && !PhoneHelper.IsValidPhone(phone))
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
                        Age = d.Age,
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

        /*public async Task<ResponseImportResult> HandleImport(Stream file)
        {
            await _uow.BeginTransactionAsync();
            try
            {
                var patients = _excelService.Parse(file);
                await _uow.Customers.AddRangeAsync(patients);

                await _uow.CompleteAsync();
                await _uow.CommitTransactionAsync();

                return result;
            }
            catch
            {
                await _uow.RollbackTransactionAsync();
                throw;
            }
        }

        public async Task<ResponseImportResult> ImportPatients(Stream fileStream)
        {
            var results = new ResponseImportResult();
            using var workbook = new XLWorkbook(fileStream); // Usando ClosedXML
            var worksheet = workbook.Worksheet(1);
            var rows = worksheet.RowsUsed().Skip(1); // Saltar encabezados

            foreach (var row in rows)
            {
                try
                {
                    var dto = new PatientImportDto
                    {
                        FirstName = CleanString(row.Cell(1).Value.ToString()),
                        LastName = CleanString(row.Cell(2).Value.ToString()),
                        // ... resto de campos
                    };

                    // 1. Validar (Ej: ¿Ya existe el email?)
                    // 2. Mapear a Entidad
                    // 3. Guardar
                    results.SuccessCount++;
                }
                catch (Exception ex)
                {
                    results.Errors.Add(new RowError
                    {
                        RowNumber = row.RowNumber(),
                        PatientName = row.Cell(1).Value.ToString(),
                        ErrorMessage = ex.Message
                    });
                }
            }
            return results;
        }*/

        public async Task<Result<string>> UploadAvatarAsync(int id, IFormFile file)
        {
            if (file == null || file.Length == 0)
                return Result<string>.Failure("No se ha seleccionado ninguna imagen.");

            // 1. Validar extensión
            var allowedExtensions = new[] { ".jpg", ".jpeg", ".png", ".webp" };
            var extension = Path.GetExtension(file.FileName).ToLower();
            if (!allowedExtensions.Contains(extension))
                return Result<string>.Failure("Formato de imagen no permitido (solo JPG, PNG).");

            string fileName = $"avatar_{id}_{Guid.NewGuid()}{extension}";
            string path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "uploads", "avatars");

            if (!Directory.Exists(path))
                Directory.CreateDirectory(path);

            string fullPath = Path.Combine(path, fileName);

            try
            {
                var customer = await _uow.Customers.FindAsync(id);
                if (customer == null)
                {
                    return Result<string>.Failure("No se encontro el customer");
                }

                using (var stream = new FileStream(fullPath, FileMode.Create))
                {
                    await file.CopyToAsync(stream);
                }

                
                if (!string.IsNullOrEmpty(customer.Avatar))
                {
                    var oldPath = Path.Combine(path, customer.Avatar);
                    if (File.Exists(oldPath)) File.Delete(oldPath);
                }

                customer.Avatar = fileName;
                await _uow.Customers.UpdateAsync(customer);
                await _uow.CompleteAsync();

                return Result<string>.Success(fileName);
            }
            catch (Exception ex)
            {
                return Result<string>.Failure($"Error al guardar la imagen: {ex.Message}");
            }
        }
    }
}
