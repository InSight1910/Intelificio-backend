using Intelificio_Back.Models.Base;

namespace Intelificio_Back.Models
{
    public class Community : BaseEntity
    {
        public required string Name { get; set; }
        public required string Address { get; set; }
        public int MunicipalityId { get; set; }
        public DateTime CreationDate { get; set; }
        public bool IsActive { get; set; }
        public required IEnumerable<CommonSpace> Spaces { get; set; }
        public required IEnumerable<Contact> Contacts { get; set; }
        public required IEnumerable<Expense> Expenses { get; set; }
        public required IEnumerable<Charge> Charges { get; set; }
        public required IEnumerable<Building> Buildings { get; set; }
        public required IEnumerable<Maintenance> Maintenances { get; set; }
        public required IEnumerable<Pet> Pets { get; set; }
        public required IEnumerable<Package> Packages { get; set; }
        public required IEnumerable<User> Users { get; set; }
        public IEnumerable<AssignedShift> AssignedShifts { get; set; } = Enumerable.Empty<AssignedShift>();
    }
}
