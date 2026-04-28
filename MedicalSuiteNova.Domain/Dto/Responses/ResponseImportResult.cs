
namespace MedicalSuiteNova.Domain.Dto.Responses
{
    public class ResponseImportResult
    {
        public int TotalRows { get; set; }
        public int SuccessCount { get; set; }
        public int ErrorCount => Errors.Count;
        public List<RowError> Errors { get; set; } = new List<RowError>();
        public double ProcessingTimeSeconds { get; set; }
    }

    public class RowError
    {
        public int RowNumber { get; set; }     // Para que el usuario sepa qué fila de Excel arreglar
        //public required string PatientName { get; set; } // Opcional, para identificarlo rápido
        public required string ErrorMessage { get; set; } // Ej: "El correo electrónico ya existe"
    }
}
