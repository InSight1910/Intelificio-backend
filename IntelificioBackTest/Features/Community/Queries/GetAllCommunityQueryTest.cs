using Backend.Features.Community.Queries.GetAll;
using Backend.Models;
using IntelificioBackTest.Fixtures;
using Microsoft.Extensions.Logging;
using Moq;

namespace IntelificioBackTest.Features.Community.Queries
{
    public class GetAllCommunityQueryTest
    {
        private readonly GetAllCommunitiesQueryHandler _handler;
        private readonly Mock<ILogger<GetAllCommunitiesQueryHandler>> _logger;
        private readonly IntelificioDbContext _context;

        public GetAllCommunityQueryTest()
        {
            _logger = new Mock<ILogger<GetAllCommunitiesQueryHandler>>();
            _context = DbContextFixture.GetDbContext();
            _handler = new GetAllCommunitiesQueryHandler(_logger.Object, _context);
        }

        [Fact]
        public async Task GetAllCommunitiesQueryHandler_Success()
        {
            await DbContextFixture.SeedData(_context);
            var result = await _handler.Handle(new GetAllCommunitiesQuery(), CancellationToken.None);
            Assert.True(result.IsSuccess);

            var data = result.Response.Data as ICollection<GetAllCommunitiesResponse>;
            Assert.True(data!.Any());
            Assert.Equal(2, data.Count);
        }

        [Fact]
        public async Task GetAllCommunitiesQueryHandler_NoData()
        {
            var result = await _handler.Handle(new GetAllCommunitiesQuery(), CancellationToken.None);
            Assert.True(result.IsSuccess);

            var data = result.Response.Data as ICollection<GetAllCommunitiesResponse>;
            Assert.Empty(data);
        }
    }
}
