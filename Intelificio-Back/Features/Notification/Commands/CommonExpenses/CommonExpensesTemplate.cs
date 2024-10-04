namespace Backend.Features.Notification.Commands.CommonExpenses
{
    public class CommonExpensesTemplate
    {
        public required string CommunityName { get; set; } // Nombre de la comunidad
        public required string Name { get; set; } // Nombre del usuario 
        public required string MesAno { get; set; } // Mes + año | Ejemplo: Abril 2024
        public required string UniteName { get; set; } // Nombre Unidad | Ejemplo: 101-B
        public required string Total { get; set; } // Monto total a pagar | Ejemplo: $80.000
        public required string EndDate { get; set; } // Fecha limite de pago | Ejemplo: 06-10-2024
    }
}
