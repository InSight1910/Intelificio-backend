using AutoMapper;
using Backend.Common.Profiles;
using Backend.Features.Buildings.Queries.GetById;
using Backend.Models;
using IntelificioBackTest.Fixtures;
using Microsoft.Extensions.Logging;
using Moq;

namespace IntelificioBackTest.Features.Building.Queries
{
    public class GetByIdQueryTest
    {
        private readonly IntelificioDbContext _context;
        private readonly Mock<ILogger<GetByIDQueryHandler>> _logger;
        private readonly IMapper _mapper;

        public GetByIdQueryTest()
        {
            var mapperConfig = new MapperConfiguration(config =>
            {
                config.AddProfile<BuildingProfile>();
            });

            _mapper = new Mapper(mapperConfig);
            _context = DbContextFixture.GetDbContext();
            _logger = new Mock<ILogger<GetByIDQueryHandler>>();

        }

        public void Dispose()
        {
            _ = _context.Database.EnsureDeleted();
            _context.Dispose();
        }

        [Fact]
        public async Task GetByID_BuildingNotFound()
        {
            // Arrange
            var query = new GetByIDQuery { BuildingId = 0 };
            var handler = new GetByIDQueryHandler(_context, _logger.Object, _mapper);

            // Act
            var result = await handler.Handle(query, CancellationToken.None);


            // Assert
            Assert.True(result.IsFailure);
            Assert.False(result.IsSuccess);
            Assert.Equal("Edificio no fue encontrado.", result.Error.Message);
        }

        [Fact]
        public async Task GetByID_Success()
        {
            // Arrange
            await DbContextFixture.SeedData(_context);
            var bulding = _context.Buildings.First(x => x.ID == 1);
            var query = new GetByIDQuery { BuildingId = 1 };
            var handler = new GetByIDQueryHandler(_context, _logger.Object, _mapper);

            // Act
            var result = await handler.Handle(query, CancellationToken.None);
            var data = result.Response.Data as GetByIDQueryResponse;

            // Assert
            Assert.True(result.IsSuccess);
            Assert.NotNull(result.Response);
            Assert.NotNull(result.Response.Data);
            Assert.Equal(bulding.Name, data.Name);
            Assert.Equal(bulding.Floors, data.Floors);
            Assert.Equal(bulding.Community.Name, data.CommunityName);


        }
    }
}
