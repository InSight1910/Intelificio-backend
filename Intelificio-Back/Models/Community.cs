using Backend.Models.Base;

namespace Backend.Models
{
    public class Community : BaseEntity
    {
        public required string Name { get; set; }
        public required string Address { get; set; }
        public int MunicipalityId { get; set; }
        public DateTime CreationDate { get; set; }
        public required Municipality Municipality { get; set; }
        public ICollection<CommonSpace> Spaces { get; set; } = new List<CommonSpace>();
        public ICollection<Contact> Contacts { get; set; } = new List<Contact>();
        public ICollection<Expense> Expenses { get; set; } = new List<Expense>();
        public ICollection<Charge> Charges { get; set; } = new List<Charge>();
        public ICollection<Building> Buildings { get; set; } = new List<Building>();
        public ICollection<Maintenance> Maintenances { get; set; } = new List<Maintenance>();
        public ICollection<Pet> Pets { get; set; } = new List<Pet>();
        public ICollection<Package> Packages { get; set; } = new List<Package>();
        public ICollection<User> Users { get; set; } = new List<User>();
        public ICollection<AssignedShift> AssignedShifts { get; set; } = new List<AssignedShift>();
        public ICollection<Fine> Fines { get; set; } = new List<Fine>();
    }
}
