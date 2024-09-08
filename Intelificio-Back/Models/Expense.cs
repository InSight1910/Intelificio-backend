using Intelificio_Back.Models.Base;

namespace Intelificio_Back.Models
{
    public class Expense : BaseEntity
    {
        // Falta ID_Comunidad
        // Falta ID_Tipo_Gasto
        public int Amount {  get; set; }
        public DateTime Date { get; set; }

        public required ExpenseType Type { get; set; } 
        
        public required Community Community { get; set; }
    }
}
