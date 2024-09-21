using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Backend.Models
{
    public class IntelificioDbContext : IdentityDbContext<User, Role, int>
    {
        public IntelificioDbContext(DbContextOptions options) : base(options) { }

        public DbSet<AssignedShift> AssignedShifts { get; set; }

        public DbSet<Attendance> Attendances { get; set; }

        public DbSet<UnitType> UnitTypes { get; set; }

        public DbSet<Unit> Units { get; set; }

        public DbSet<ShiftType> ShiftTypes { get; set; }

        public DbSet<Shift> Shifts { get; set; }

        public DbSet<Reservation> Reservations { get; set; }

        public DbSet<Region> Regions { get; set; }

        public DbSet<City> City { get; set; }

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

        public DbSet<Guest> Guest { get; set; }

        public DbSet<Municipality> Municipality { get; set; }

        public DbSet<Package> Package { get; set; }

        public DbSet<Payment> Payment { get; set; }

        public DbSet<Maintenance> Maintenances { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            _ = builder.Entity<Guest>().HasKey(p => p.ID);

            _ = builder.Entity<Charge>(entity =>
            {
                _ = entity.HasKey(p => p.ID);

                _ = entity.HasOne(p => p.Type)
                      .WithMany(p => p.Charges);

                _ = entity.HasMany(p => p.Payments)
                      .WithOne(p => p.Charge);


            });

            _ = builder.Entity<Package>(entity =>
            {
                _ = entity.HasOne(p => p.Owner)
                    .WithMany(p => p.Packages)
                    .HasForeignKey(p => p.OwnerId);

                _ = entity.HasOne(p => p.Staff)
                    .WithMany()
                    .HasForeignKey(p => p.StaffId);
            });

            _ = builder.Entity<Expense>(entity =>
            {
                _ = entity.HasKey(p => p.ID);

                _ = entity.HasOne(p => p.Type)
                      .WithMany(p => p.Expenses);
            });

            _ = builder.Entity<Community>(entity =>
            {
                _ = entity.HasKey(p => p.ID);

                _ = entity.HasMany(p => p.Spaces)
                      .WithOne(p => p.Community);

                _ = entity.HasMany(p => p.Contacts)
                      .WithOne(p => p.Community);

                _ = entity.HasMany(p => p.Expenses)
                      .WithOne(p => p.Community);

                _ = entity.HasMany(p => p.Buildings)
                      .WithOne(p => p.Community);

                _ = entity.HasMany(P => P.Maintenances)
                      .WithOne(p => p.Community);

                _ = entity.HasMany(p => p.Users)
                      .WithMany(p => p.Communities);

                _ = entity.HasMany(p => p.AssignedShifts)
                      .WithMany(p => p.Communities);

                _ = entity.HasMany(p => p.Pets)
                      .WithOne(p => p.Community);

                _ = entity.HasMany(p => p.Packages)
                      .WithOne(p => p.Community);

                _ = entity.HasMany(p => p.Charges)
                      .WithOne(p => p.Community);

                _ = entity.HasOne(p => p.Municipality)
                      .WithMany(p => p.Community);

                _ = entity.HasMany(p => p.Fines)
                      .WithOne(p => p.Community);

                _ = entity.HasMany(p => p.Fines)
                      .WithOne(p => p.Community);

                _ = entity.HasMany(p => p.Fines)
                      .WithOne(p => p.Community);

            });

            _ = builder.Entity<Municipality>(entity =>
            {
                _ = entity.HasKey(p => p.ID);
                _ = entity.HasOne(p => p.City)
                      .WithMany(p => p.Municipalities);
            });

            _ = builder.Entity<City>(entity =>
            {
                _ = entity.HasKey(p => p.ID);

                _ = entity.HasOne(p => p.Region)
                      .WithMany(p => p.Cities);
            });

            _ = builder.Entity<Building>(entity =>
            {
                _ = entity.HasKey(p => p.ID);

                _ = entity.HasMany(p => p.Units)
                      .WithOne(p => p.Building);

                _ = entity.HasMany(p => p.Maintenances)
                      .WithOne(p => p.Building);
            });

            _ = builder.Entity<Unit>(entity =>
            {
                _ = entity.HasKey(p => p.ID);

                _ = entity.HasOne(p => p.UnitType)
                      .WithMany(p => p.Units);
            });

            _ = builder.Entity<User>(entity =>
            {

                _ = entity.HasMany(p => p.Attendances)
                      .WithOne(p => p.User);

                _ = entity.HasMany(p => p.Guests)
                      .WithOne(p => p.User);

                _ = entity.HasMany(p => p.Reservations)
                      .WithOne(p => p.User);

                _ = entity.HasMany(p => p.Units)
                       .WithMany(p => p.Users);

                _ = entity.HasMany(p => p.Charges)
                    .WithOne(p => p.User);

                _ = entity.HasMany(p => p.Pets)
                    .WithOne(p => p.User);


            });


            _ = builder.Entity<AssignedShift>(entity =>
            {
                _ = entity.HasKey(p => p.ID);

                _ = entity.HasOne(p => p.Shift)
                    .WithMany(p => p.AssignedShifts);
                _ = entity.HasMany(p => p.Users)
                    .WithMany(p => p.AssignedShifts);

            });

            _ = builder.Entity<Shift>(entity =>
            {
                _ = entity.HasKey(p => p.ID);

                _ = entity.HasMany(p => p.AssignedShifts)
                    .WithOne(p => p.Shift);
                _ = entity.HasOne(p => p.Type)
                    .WithMany(p => p.Shifts);

            });

            _ = builder.Entity<Reservation>(entity =>
            {

                _ = entity.HasKey(p => p.ID);

                _ = entity.HasOne(p => p.User)
                        .WithMany(p => p.Reservations);
                _ = entity.HasOne(p => p.Spaces)
                        .WithMany(p => p.Reservations);
                _ = entity.HasMany(p => p.Invites)
                         .WithMany(p => p.Reservations);

            });

        }

    }
}
