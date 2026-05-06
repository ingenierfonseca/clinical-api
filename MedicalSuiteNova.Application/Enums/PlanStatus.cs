
namespace MedicalSuiteNova.Application.Enums
{
    public static class PlanStatus
    {
        public const string Pending = "Pendiente";      //El plan ha sido creado y el presupuesto entregado, pero el paciente no ha confirmado.
        public const string InProcess = "En Proceso";   //El paciente aceptó. Los ítems del detalle están listos para ser marcados como "Realizados".
        public const string Completed = "Completado";   //Todos los ítems del PatientPlanDetail han sido ejecutados y finalizados.
        public const string Suspended = "Suspendido";   //El tratamiento se pausó (ej. el paciente se fue de viaje o debe sanar una cirugía antes de seguir)
        public const string Cancelled = "Cancelado";    //El paciente decidió no continuar con ese plan específico.
    }
}