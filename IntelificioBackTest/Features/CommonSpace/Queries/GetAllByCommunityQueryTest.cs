using Backend.Features.CommonSpaces.Common;
using Backend.Features.CommonSpaces.Queries.GetAllByCommunity;
using Backend.Models;
using FluentAssertions;
using IntelificioBackTest.Fixtures;
using Microsoft.EntityFrameworkCore.Query.Internal;
using Quartz.Util;

namespace IntelificioBackTest.Features.CommonSpace.Queries;

public class GetAllByCommunityQueryTest
{
    private readonly IntelificioDbContext _context;
    private readonly GetAllByCommunityQueryHandler _handler;

    public GetAllByCommunityQueryTest()
    {
        _context = DbContextFixture.GetDbContext();
        _handler = new GetAllByCommunityQueryHandler(_context);
    }

    [Fact]
    public async Task GetAllByCommunity_Should_ReturnError_CommunityNotFound()
    {
        // Arrange
        var query = new GetAllByCommunityQuery
        {
            CommunityId = 1
        };

        // Act
        var result = await _handler.Handle(query, default);

        // Assert
        result.Error.Should().Be(CommonSpacesErrors.CommunityNotFoundOnQuery);
    }

    [Fact]
    public async Task GetAllByCommunity_Should_ReturnSuccess_ListOfAllCommunities()
    {
        // Arrange
        var query = new GetAllByCommunityQuery
        {
            CommunityId = 1
        };
        await DbContextFixture.SeedData(_context);
        await _context.CommonSpaces.AddAsync(new Backend.Models.CommonSpace
        {
            Location = "Roof",
            Name = "Roof",
            CommunityId = 1,
            IsInMaintenance = false,
            Capacity = 10
        });
        await _context.SaveChangesAsync();

        // Act
        var result = await _handler.Handle(query, default);

        // Assert
        var data = result.Response.Data as List<GetAllByCommunityQueryResponse>;
        data.Count().Should().Be(1);
        data.First().Location.Should().Be("Roof");
    }
}