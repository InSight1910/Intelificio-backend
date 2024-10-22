using Backend.Models.Extensions;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Backend.Models;

public class IntelificioDbContext : IdentityDbContext<User, Role, int>
{
    public IntelificioDbContext(DbContextOptions options) : base(options)
    {
    }

    public DbSet<AssignedShift> AssignedShifts { get; set; }

    public DbSet<Attendance> Attendances { get; set; }

    public DbSet<UnitType> UnitTypes { get; set; }

    public DbSet<Unit> Units { get; set; }

    public DbSet<ShiftType> ShiftTypes { get; set; }

    public DbSet<Shift> Shifts { get; set; }

    public DbSet<Reservation> Reservations { get; set; }
    public DbSet<Attendee> Attendees { get; set; }
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
    public DbSet<TemplateNotification> TemplateNotifications { get; set; }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);
        _ = builder.ApplySoftDeleteQueryFilter();

        _ = builder.Entity<Guest>(entity =>
        {
            _ = entity.HasKey(p => p.ID);
            _ = entity.HasOne(p => p.Unit)
                .WithMany(p => p.Guests)
                .HasForeignKey(p => p.UnitId);
        });

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
            _ = entity.HasOne(p => p.Recipient)
                .WithMany(p => p.Packages)
                .HasForeignKey(p => p.RecipientId);

            _ = entity.HasOne(p => p.Concierge)
                .WithMany()
                .HasForeignKey(p => p.ConciergeId);

            _ = entity
                .HasOne(p => p.DeliveredTo)
                .WithMany()
                .HasForeignKey(x => x.DeliveredToId);
            _ = entity
                .HasOne(p => p.CanRetire)
                .WithMany()
                .HasForeignKey(i => i.CanRetireId)
                .IsRequired(false);
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
                    .WithMany(p => p.Communities)
                ;

            _ = entity.HasMany(p => p.AssignedShifts)
                .WithMany(p => p.Communities);

            _ = entity.HasMany(p => p.Pets)
                .WithOne(p => p.Community);

            _ = entity.HasMany(p => p.Packages)
                .WithOne(p => p.Community);

            _ = entity.HasMany(p => p.Charges)
                .WithOne(p => p.Community);

            _ = entity.HasMany(p => p.Fines)
                .WithOne(p => p.Community);

            _ = entity.HasMany(p => p.Fines)
                .WithOne(p => p.Community);

            _ = entity.HasMany(p => p.Spaces)
                .WithOne(p => p.Community)
                .HasForeignKey(p => p.CommunityId);
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
        });

        _ = builder.Entity<Unit>(entity =>
        {
            _ = entity.HasKey(p => p.ID);

            _ = entity.HasOne(p => p.UnitType)
                .WithMany(p => p.Units)
                .HasForeignKey(p => p.UnitTypeId);

            _ = entity.HasOne(p => p.Building)
                .WithMany(p => p.Units)
                .HasForeignKey(p => p.BuildingId);

            _ = entity.HasMany(p => p.Guests)
                .WithOne(p => p.Unit);

            _ = entity.HasMany(p => p.Users)
                .WithMany(p => p.Units);  // Relación muchos a muchos

            _ = entity.HasOne(p => p.User)
                .WithMany()
                .HasForeignKey(p => p.UserId)
                .OnDelete(DeleteBehavior.Restrict);  // Relación uno a muchos para el usuario principal
        });

        _ = builder.Entity<User>(entity =>
        {
            _ = entity.HasMany(p => p.Attendances)
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
                .WithMany(p => p.Reservations)
                .HasForeignKey(f => f.UserId);

            _ = entity.HasOne(p => p.Spaces)
                .WithMany(p => p.Reservations)
                .HasForeignKey(f => f.SpaceId);
        });

        _ = builder.Entity<Attendee>(entity =>
        {
            _ = entity.HasKey(p => p.ID);
            _ = entity.HasOne(p => p.Reservation)
                .WithMany(p => p.Attendees)
                .HasForeignKey(f => f.ReservationId);
        });

        _ = builder.Entity<CommonSpace>(entity =>
        {
            _ = entity.HasKey(p => p.ID);

            _ = entity.HasMany(p => p.Maintenances)
                .WithOne(p => p.CommonSpace)
                .HasForeignKey(p => p.CommonSpaceID);
        });
    }
}