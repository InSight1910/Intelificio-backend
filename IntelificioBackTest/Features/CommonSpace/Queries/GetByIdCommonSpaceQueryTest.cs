using Backend.Features.CommonSpaces.Common;
using Backend.Features.CommonSpaces.Queries.GetById;
using Backend.Models;
using FluentAssertions;
using IntelificioBackTest.Fixtures;

namespace IntelificioBackTest.Features.CommonSpace.Queries;

public class GetByIdCommonSpaceQueryTest
{
    private readonly IntelificioDbContext _context;
    private readonly GetByIdCommonSpaceQueryHandler _handler;

    public GetByIdCommonSpaceQueryTest()
    {
        _context = DbContextFixture.GetDbContext();
        _handler = new GetByIdCommonSpaceQueryHandler(_context);
    }

    public void Dispose()
    {
        _context.Database.EnsureDeleted();
        _context.Dispose();
    }

    [Fact]
    public async Task GetByID_Should_ReturnError_WhenCommonSpaceIsNotFound()
    {
        // Arrange
        var query = new GetByIdCommonSpaceQuery { Id = 1 };

        // Act
        var result = await _handler.Handle(query, default);

        // Assert
        result.Error.Should().Be(CommonSpacesErrors.CommonSpaceNotFoundOnQuery);
    }

    [Fact]
    public async Task GetByID_Should_ReturnCommonSpace_WhenCommonSpaceIsFound()
    {
        // Arrange
        var query = new GetByIdCommonSpaceQuery { Id = 1 };
        await _context.CommonSpaces.AddAsync(new Backend.Models.CommonSpace
        {
            Location = "Roof",
            Name = "Roof",
            CommunityId = 1,
            Capacity = 10
        });
        await _context.SaveChangesAsync();

        // Act
        var result = await _handler.Handle(query, default);

        //Assert
        (result.Response.Data as GetByIdCommonSpaceQueryResponse).ID.Should().Be(1);
    }
}