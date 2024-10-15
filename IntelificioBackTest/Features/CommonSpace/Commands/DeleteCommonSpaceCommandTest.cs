using Backend.Features.CommonSpaces.Commands.Delete;
using Backend.Models;
using Backend.Models.Enums;
using IntelificioBackTest.Fixtures;
using Quartz.Xml.JobSchedulingData20;

namespace IntelificioBackTest.Features.CommonSpace.Commands;

public class DeleteCommonSpaceCommandTest
{
    private readonly IntelificioDbContext _context;
    private readonly DeleteCommonSpaceCommandHandler _handler;

    public DeleteCommonSpaceCommandTest()
    {
        _context = DbContextFixture.GetDbContext();
        _handler = new DeleteCommonSpaceCommandHandler(_context);
    }

    [Fact]
    public async Task Handle_Success()
    {
        // Arrange
        await DbContextFixture.SeedData(_context);
        var commonSpace = CommonSpaceFixture.CreateCommonSpaceCommand();
        await _context.CommonSpaces.AddAsync(new Backend.Models.CommonSpace
        {
            Location = commonSpace.Location,
            Name = commonSpace.Name,
            CommunityId = commonSpace.CommunityId,
            Capacity = commonSpace.Capacity
        });
        await _context.SaveChangesAsync();

        var space = new DeleteCommonSpaceCommand
        {
            Id = 1
        };

        // Act
        var result = await _handler.Handle(space, CancellationToken.None);
        // Assest

        Assert.Null(result.Error);
        Assert.True(result.IsSuccess);
    }

    [Fact]
    public async Task Handle_Failure_NotFound()
    {
        // Arrange
        var space = new DeleteCommonSpaceCommand
        {
            Id = 1
        };

        // Act
        var result = await _handler.Handle(space, CancellationToken.None);
        // Assest

        Assert.NotNull(result.Error);
        Assert.True(result.IsFailure);
        Assert.Equal("No fue posible encontrar el espacio comun indicado.", result.Error.Message);
        Assert.Equal("CommonSpace.Delete.CommonSpaceNotFound", result.Error.Code);
    }

    [Fact]
    public async Task Handle_Failure_PendingReservations()
    {
        // Arrange
        await DbContextFixture.SeedData(_context);
        var commonSpace = CommonSpaceFixture.CreateCommonSpaceCommand();
        var resultSpace = await _context.CommonSpaces.AddAsync(new Backend.Models.CommonSpace
        {
            Location = commonSpace.Location,
            Name = commonSpace.Name,
            CommunityId = commonSpace.CommunityId,
            Capacity = commonSpace.Capacity
        });
        await _context.SaveChangesAsync();
        Reservation reservations =
            new()
            {
                Date = DateTime.Now,
                Status = ReservationStatus.PENDING,
                StartTime = DateTime.Now.AddHours(1).TimeOfDay,
                EndTime = DateTime.Now.AddHours(1).TimeOfDay,
                SpaceId = resultSpace.Entity.ID
            };
        await _context.Reservations.AddAsync(reservations);
        await _context.SaveChangesAsync();
        var command = new DeleteCommonSpaceCommand { Id = 1 };
        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        //Assert
        Assert.NotNull(result.Error);
        Assert.True(result.IsFailure);
        Assert.Equal("Existen reservas activas.", result.Error.Message);
        Assert.Equal("CommonSpace.Delete.HasPendingReservations", result.Error.Code);
    }
}