﻿using Microsoft.AspNetCore.Identity;

namespace Backend.Models
{
    public class User : IdentityUser<int>
    {
        public required string FirstName { get; set; }
        public required string LastName { get; set; }
        public required new string Email { get; set; }
        public required new string PhoneNumber { get; set; }
        public required string Password { get; set; }
        public required string Rut { get; set; }
        public DateTime BirthDate { get; set; }
        public DateTime Admission { get; set; }
        public required Role Role { get; set; }
        public string RefreshToken { get; set; } = string.Empty;
        public DateTime? RefreshTokenExpiry { get; set; }

        public IEnumerable<Community> Communities { get; set; } = Enumerable.Empty<Community>();

        public IEnumerable<Attendance> Attendances { get; set; } = Enumerable.Empty<Attendance>();

        public IEnumerable<AssignedShift> AssignedShifts { get; set; } = Enumerable.Empty<AssignedShift>();

        public IEnumerable<Guest> Guests { get; set; } = Enumerable.Empty<Guest>();

        public IEnumerable<Reservation> Reservations { get; set; } = Enumerable.Empty<Reservation>();

        public IEnumerable<Unit> Units { get; set; } = Enumerable.Empty<Unit>();

        public IEnumerable<Package> Packages { get; set; } = Enumerable.Empty<Package>();

        public IEnumerable<Charge> Charges { get; set; } = Enumerable.Empty<Charge>();

        public IEnumerable<Pet> Pets { get; set; } = Enumerable.Empty<Pet>();

    }
}
