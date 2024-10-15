using Backend.Features.Community.Commands.Delete;
using Backend.Models;
using IntelificioBackTest.Fixtures;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Moq;

namespace IntelificioBackTest.Features.Community.Commands;

public class DeleteCommunityCommandTest
{
    private readonly DeleteCommunityCommandHandler _handler;
    private readonly Mock<ILogger<DeleteCommunityCommandHandler>> _logger;
    private readonly IntelificioDbContext _context;

    public DeleteCommunityCommandTest()
    {
        _logger = new Mock<ILogger<DeleteCommunityCommandHandler>>();

        _context = DbContextFixture.GetDbContext();
        _handler = new DeleteCommunityCommandHandler(_context, _logger.Object);
    }


    public void Dispose()
    {
        _ = _context.Database.EnsureDeleted();
        _context.Dispose();
    }

    [Fact]
    public async Task Handle_Success()
    {
        // Arrange
        var command = new DeleteCommunityCommand { Id = 1 };
        await DbContextFixture.SeedData(_context);
        var buildings = await _context.Buildings.Include(x => x.Community).Where(x => x.Community.ID == command.Id)
            .ToListAsync();

        foreach (var building in buildings) _context.Buildings.Remove(building);
        await _context.SaveChangesAsync();


        // Act
        var result = await _handler.Handle(command, default);

        // Assert
        Assert.True(result.IsSuccess);
    }

    [Fact]
    public async Task Failure_Handle_Community_Not_Found()
    {
        // Arrange
        var command = new DeleteCommunityCommand { Id = 1 };

        // Act
        var result = await _handler.Handle(command, default);

        // Assert
        Assert.False(result.IsSuccess);
        Assert.Null(result.Response);
        Assert.Null(result.Errors);
        Assert.Equal("La comunidad indicada no se encuentra dentro de nuestros registros.", result.Error.Message);
    }
}