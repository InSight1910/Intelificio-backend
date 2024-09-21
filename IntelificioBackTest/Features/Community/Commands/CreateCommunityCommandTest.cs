using AutoMapper;
using Backend.Common.Profiles;
using Backend.Features.Community.Commands.Create;
using Backend.Models;
using IntelificioBackTest.Fixtures;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Moq;

namespace IntelificioBackTest.Features.Community.Commands
{
    public class CreateCommunityCommandTest
    {
        private readonly CreateCommunityCommandHandler _handler;
        private readonly Mock<ILogger<CreateCommunityCommandHandler>> _logger;
        private readonly IMapper _mapper;
        private readonly IntelificioDbContext _context;

        public CreateCommunityCommandTest()
        {
            var mapperConfig = new MapperConfiguration(config =>
            {
                config.AddProfile<CommunityProfile>();
            });
            _mapper = new Mapper(mapperConfig);

            _context = DbContextFixture.GetDbContext();
            _logger = new Mock<ILogger<CreateCommunityCommandHandler>>();

            _handler = new CreateCommunityCommandHandler(_context, _logger.Object, _mapper);
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
            var command = CommunityFixture.GetCommunityCommandTest();
            await DbContextFixture.SeedData(_context);

            // Act
            var result = await _handler.Handle(command, default);


            // Assert
            Assert.True(result.IsSuccess);
            Assert.Null(result.Response);
            Assert.Null(result.Errors);

        }


        [Fact]
        public async Task Failure_Handle_Community_Already_Exists()
        {
            // Arrange
            var command = CommunityFixture.GetCommunityCommandTest();
            await DbContextFixture.SeedData(_context);

            command.Name = await _context.Community.Select(x => x.Name).FirstOrDefaultAsync();

            // Act
            var result = await _handler.Handle(command, default);

            // Assert
            Assert.False(result.IsSuccess);
            Assert.True(result.IsFailure);

            Assert.Null(result.Response);

            Assert.Null(result.Errors);

            Assert.Equal("La comunidad ingresada ya se encuentra registrada.", result.Error.Message);
        }

        [Fact]
        public async Task Failure_Handle_Municipality_Not_Found()
        {
            // Arrange
            var command = CommunityFixture.GetCommunityCommandTest();
            await DbContextFixture.SeedData(_context);

            command.MunicipalityId = 0;

            // Act
            var result = await _handler.Handle(command, default);

            // Assert
            Assert.False(result.IsSuccess);
            Assert.True(result.IsFailure);

            Assert.Null(result.Response);
            Assert.Null(result.Errors);

            Assert.Equal("La comuna ingresa no es valida.", result.Error.Message);
        }
    }
}
