namespace MedicalSuiteNova.Domain.Dto.Appointment
{
    public class AppointmentInfoDto
    {
        public long Id { get; set; }
        public required int CustomerId { get; set; }
        public required int DoctorId { get; set; }
        public int? ResourceId { get; set; }
        public required byte AppointmentTypeId { get; set; }
        public byte StatusId { get; set; }
        public required string PatientName { get; set; }
        public required string DoctorName { get; set; }
        public required string TypeName { get; set; }
        public string? ResourceName { get; set; }
        public string? StatusName { get; set; }
        public required DateOnly Date { get; set; }
        public required TimeSpan StartTime { get; set; }
        public TimeSpan EndTime { get; set; }
        public string? Notes { get; set; }
    }
}
