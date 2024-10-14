using Microsoft.AspNetCore.Identity;

namespace Backend.Models;

public class User : IdentityUser<int>
{
    public required string FirstName { get; set; }
    public required string LastName { get; set; }
    public required string Rut { get; set; }
    public DateTime BirthDate { get; set; }
    public DateTime Admission { get; set; }
    public string RefreshToken { get; set; } = string.Empty;
    public DateTime? RefreshTokenExpiry { get; set; }

    public ICollection<Community> Communities { get; set; } = new List<Community>();

    public ICollection<Attendance> Attendances { get; set; } = new List<Attendance>();

    public ICollection<AssignedShift> AssignedShifts { get; set; } = new List<AssignedShift>();

    public ICollection<Reservation> Reservations { get; set; } = new List<Reservation>();

    public ICollection<Unit> Units { get; set; } = new List<Unit>();

    public ICollection<Package> Packages { get; set; } = new List<Package>();

    public ICollection<Charge> Charges { get; set; } = new List<Charge>();

    public ICollection<Pet> Pets { get; set; } = new List<Pet>();

    public override string ToString()
    {
        return $"{FirstName} {LastName}";
    }
}