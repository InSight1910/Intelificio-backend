using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Internal;
using Org.BouncyCastle.Crypto.Prng;

namespace Backend.Models
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

                entity.HasMany(p => p.Payments)
                      .WithOne(p => p.Charge);

                entity.HasMany(p => p.Fines)
                      .WithOne(p => p.Charge);

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

                entity.HasMany(p => p.Users)
                      .WithMany(p => p.Communities);

                entity.HasMany(p => p.AssignedShifts)
                      .WithMany(p => p.Communities);

                entity.HasMany(p => p.Pets)
                      .WithOne(p => p.Community);

                entity.HasMany(p => p.Packages)
                      .WithOne(p => p.Community);

                entity.HasMany(p => p.Charges)
                      .WithOne(p => p.Community);

                entity.HasOne(p => p.Municipality)
                      .WithOne(p => p.Community);

            });

            builder.Entity<Municipality>(entity =>
            {
                entity.HasOne(p => p.Province)
                      .WithOne(p => p.Municipality);
            });

            builder.Entity<Province>(entity =>
            {
                entity.HasOne(p => p.Region)
                      .WithOne(p => p.Province);
            });

            builder.Entity<Building>(entity =>
            {
                entity.HasKey(p => p.ID);
                entity.HasMany(p => p.Units)
                      .WithOne(p => p.Building);

                entity.HasMany(p => p.Maintenances)
                      .WithOne(p => p.Building);
            });

            builder.Entity<Unit>(entity =>
            {
                entity.HasKey(p => p.ID);
                entity.HasOne(p => p.Type)
                      .WithMany(p => p.Units);
            });

            builder.Entity<User>(entity =>
            {

                entity.HasMany(p => p.Attendances)
                      .WithOne(p => p.User);

                entity.HasOne(p => p.Role)
                      .WithMany(p => p.Users);

                entity.HasMany(p => p.Visits)
                      .WithOne(p => p.User);

                entity.HasMany(p => p.Reservations)
                      .WithOne(p => p.User);

                entity.HasMany(p => p.Units)
                       .WithMany(p => p.users);

                entity.HasMany(p => p.Packages)
                    .WithOne(p => p.Owner);

                entity.HasMany(p => p.Packages)
                    .WithOne(p => p.Staff);

                entity.HasMany(p => p.Charges)
                    .WithOne(p => p.User);

                entity.HasMany(p => p.Pets)
                    .WithOne(p => p.User);


            });

            builder.Entity<AssignedShift>(entity =>
            {
                entity.HasKey(p => p.ID);
                entity.HasOne(p => p.Shift)
                    .WithMany(p => p.AssignedShifts);
                entity.HasMany(p => p.Users)
                    .WithMany(p => p.AssignedShifts);

            });

            builder.Entity<Shift>(entity =>
            {
                entity.HasKey(p => p.ID);
                entity.HasMany(p => p.AssignedShifts)
                    .WithOne(p => p.Shift);
                entity.HasOne(p => p.Type)
                    .WithMany(p => p.Shifts);

            });

            builder.Entity<Reservation>(entity =>
            {

                entity.HasKey(p => p.ID);
                entity.HasOne(p => p.User)
                        .WithMany(p => p.Reservations);
                entity.HasOne(p => p.Spaces)
                        .WithMany(p => p.Reservations);
                entity.HasMany(p => p.Invitees)
                         .WithMany(p => p.Reservations);

            });

        }

    }
}
