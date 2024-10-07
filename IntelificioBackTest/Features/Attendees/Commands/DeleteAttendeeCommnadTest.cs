using Backend.Features.Attendees.Commands.Delete;
using Backend.Features.Attendees.Common;
using Backend.Models;
using Backend.Models.Enums;
using FluentAssertions;
using IntelificioBackTest.Fixtures;

namespace IntelificioBackTest.Features.Attendees.Commands;

public class DeleteAttendeeCommnadTest
{
    private readonly IntelificioDbContext _context;
    private readonly DeleteAttendeeCommandHandler _handler;

    public DeleteAttendeeCommnadTest()
    {
        _context = DbContextFixture.GetDbContext();
        _handler = new DeleteAttendeeCommandHandler(_context);
    }

    [Fact]
    public async Task DeleteAttendee_ReturnsError_WhenAttendeeNotFound()
    {
        // Arrange
        var command = new DeleteAttendeeCommand { AttendeeId = 1 };

        // Act
        var result = await _handler.Handle(command, default);

        // Assert
        result.IsFailure.Should().BeTrue();
        result.Error.Should().Be(AttendeesErrors.AttendeeNotFoundOnDelete);
    }

    [Fact]
    public async Task DeleteAttendee_ReturnsSuccess_WhenAttendeeFound()
    {
        // Assert
        var command = new DeleteAttendeeCommand { AttendeeId = 1 };
        await DbContextFixture.SeedData(_context);
        await _context.Reservations.AddAsync(new Reservation()
        {
            Date = DateTime.Now, Status = ReservationStatus.PENDING, EndTime = DateTime.Now.AddHours(1).TimeOfDay,
            StartTime = DateTime.Now.AddHours(-1).TimeOfDay, SpaceId = 1, UserId = 1
        });
        await _context.SaveChangesAsync();
        await _context.Attendees.AddAsync(new Attendee()
            { Name = "test", Rut = "test", Email = "test@test.com", ReservationId = 1 });
        await _context.SaveChangesAsync();

        // Act
        var result = await _handler.Handle(command, default);

        // Assert
        result.IsSuccess.Should().BeTrue();
    }
}