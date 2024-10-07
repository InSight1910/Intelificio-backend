using AutoMapper;
using Backend.Common.Profiles;
using Backend.Features.Attendees.Commands.Create;
using Backend.Features.Attendees.Common;
using Backend.Models;
using Backend.Models.Enums;
using FluentAssertions;
using IntelificioBackTest.Fixtures;

namespace IntelificioBackTest.Features.Attendees.Commands;

public class CreateAttendeeCommandTest
{
    private readonly IntelificioDbContext _context;
    private readonly IMapper _mapper;
    private readonly CreateAttendeeCommandHandler _handler;

    public CreateAttendeeCommandTest()
    {
        _context = DbContextFixture.GetDbContext();
        var mapperConfig = new MapperConfiguration(config => config.AddProfile<AttendeeProfile>());
        _mapper = new Mapper(mapperConfig);
        _handler = new CreateAttendeeCommandHandler(_context, _mapper);
    }

    public void Dispose()
    {
        _context.Database.EnsureDeleted();
        _context.Dispose();
    }

    [Fact]
    public async Task CreateAttendee_ReturnsSuccess()
    {
        // Arrange
        await DbContextFixture.SeedData(_context);
        var reservation = new Reservation
        {
            Date = DateTime.Now,
            Status = ReservationStatus.PENDING,
            StartTime = DateTime.Now.AddHours(-1).TimeOfDay,
            EndTime = DateTime.Now.AddHours(1).TimeOfDay,
            SpaceId = 1,
            UserId = 1
        };
        reservation = (await _context.Reservations.AddAsync(reservation)).Entity;
        await _context.SaveChangesAsync();
        var command = new CreateAttendeeCommand
        {
            ReservationId = reservation.ID,
            Email = "test",
            Name = "test",
            RUT = "test"
        };
        // Act
        var result = await _handler.Handle(command, default);

        // Assert
        result.IsSuccess.Should().BeTrue();
    }

    [Fact]
    public async Task CreateAttendee_ReturnsError_WhenReservationNotExists()
    {
        // Assert
        var command = new CreateAttendeeCommand
        {
            ReservationId = 1,
            Email = "test",
            Name = "test",
            RUT = "test"
        };

        // Act
        var result = await _handler.Handle(command, default);

        // Assert
        result.Error.Should().Be(AttendeesErrors.ReservationNotFoundOnCreate);
    }

    [Fact]
    public async Task CreateAttendee_ReturnsError_WhenAttendeeAlreadyExists()
    {
        // Arrange
        await DbContextFixture.SeedData(_context);
        var reservation = new Reservation
        {
            Date = DateTime.Now,
            Status = ReservationStatus.PENDING,
            StartTime = DateTime.Now.AddHours(-1).TimeOfDay,
            EndTime = DateTime.Now.AddHours(1).TimeOfDay,
            SpaceId = 1,
            UserId = 1
        };
        reservation = (await _context.Reservations.AddAsync(reservation)).Entity;
        await _context.SaveChangesAsync();
        var command = new CreateAttendeeCommand
        {
            ReservationId = reservation.ID,
            Email = "test",
            Name = "test",
            RUT = "test"
        };
        await _handler.Handle(command, default);
        // Act
        var result = await _handler.Handle(command, default);

        // Assert
        result.Error.Should().Be(AttendeesErrors.AttendeeAlreadyExist);
    }
}