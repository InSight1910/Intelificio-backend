using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Internal;

namespace Intelificio_Back.Models
{
    public class IntelificioDbContext : IdentityDbContext<User, Role, int>
    {
        public IntelificioDbContext(DbContextOptions options) : base(options) { }

        public DbSet<AssignedShift> AssignedShifts { get; set; }

        public DbSet<Attendance> Attendances { get; set; }

        public DbSet<Visit> Visits { get; set; }

        public DbSet<UnitType> UnitTypes { get; set; }

        public DbSet<Unit> Units { get; set; }

        public DbSet<ShiftType> ShiftTypes { get; set; }

        public DbSet<Shift> Shifts { get; set; }

        public DbSet<Reservation> Reservations { get; set; }

        public DbSet<Region> Regions { get; set; }

        public DbSet<Province> Provinces { get; set; }

        public DbSet<Pet> Pets { get; set; }
        
        public DbSet<Building> Buildings { get; set; }

        public DbSet<Charge> Charges { get; set; }

        public DbSet<ChargeType> ChargeTypes { get; set; }

        public DbSet<CommonSpace> CommonSpaces { get; set; }

        public DbSet<Community> Community { get; set; }

        public DbSet<Contact> Contacts { get; set; }

        public DbSet<Expense> Expense { get; set; }

        public DbSet<ExpenseType> ExpenseTypes { get; set; }

        public DbSet<Fine> Fine { get; set; }

        public DbSet<Guest> Guests { get; set; }

        public DbSet<Municipality> Municipality { get; set; }

        public DbSet<Package> Package { get; set; }

        public DbSet<Payment> Payment { get; set; }

        public DbSet<Maintenance> Maintenances { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<Charge>(entity =>
            {
                entity.HasKey(p => p.ID);
                entity.HasOne(p => p.Type)
                      .WithMany(p => p.Charges);
            });

            builder.Entity<Expense>(entity => 
            {  
                entity.HasKey(p => p.ID);
                entity.HasOne(p => p.Type)
                      .WithMany(p => p.Expenses); 
            });

            builder.Entity<Community>(entity =>
            {
                entity.HasKey(p => p.ID);
                entity.HasMany(p => p.Spaces)
                      .WithOne(p => p.Community);
                entity.HasMany(p => p.Contacts)
                      .WithOne(p => p.Community);
                entity.HasMany(p => p.Expenses)
                      .WithOne(p => p.Community);
                entity.HasMany(p => p.Buildings)
                      .WithOne(p => p.Community);
                entity.HasMany(P => P.Maintenances)
                      .WithOne(p => p.Community);
  
            });

        }

    }
}
