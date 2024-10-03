using AutoMapper;
using Backend.Common.Profiles;
using Backend.Features.Community.Commands.Update;
using Backend.Models;
using IntelificioBackTest.Fixtures;
using IntelificioBackTest.Mocks;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Moq;

namespace IntelificioBackTest.Features.Community.Commands
{
    public class UpdateCommunityCommandTest
    {
        private readonly Mock<ILogger<UpdateCommunityCommandHandler>> _logger;
        private readonly Mock<UserManager<User>> _userManager;
        private readonly IMapper _mapper;
        private readonly IntelificioDbContext _context;
        private readonly UpdateCommunityCommandHandler _handler;

        public UpdateCommunityCommandTest()
        {
            _logger = new Mock<ILogger<UpdateCommunityCommandHandler>>();

            var mapperConfig = new MapperConfiguration(config =>
            {
                config.AddProfile<CommunityProfile>();
            });
            _mapper = new Mapper(mapperConfig);

            _context = DbContextFixture.GetDbContext();
            _userManager = UserManagerMock.CreateUserManager();

            _handler = new UpdateCommunityCommandHandler(_context, _userManager.Object, _logger.Object, _mapper);
        }

        
        public void Dispose()
        {
            _ = _context.Database.EnsureDeleted();
            _context.Dispose();
        }

        [Fact]
        public async void Handle_Success_Full_Update()
        {
            // Arrange
            var command = new UpdateCommunityCommand
            {
                Id = 1,
                Address = "Calle Nueva 123",
                Name = "New Name",
                MunicipalityId = 2
            };
            await DbContextFixture.SeedData(_context);

            // Act
            var result = await _handler.Handle(command, default);

            // Assert
            Assert.True(result.IsSuccess);
            Assert.Null(result.Response);
            Assert.Null(result.Errors);

            var community = await _context.Community.Include(x => x.Municipality).FirstOrDefaultAsync(x => x.ID == command.Id);

            Assert.NotNull(community);
            Assert.Equal(command.Address, community.Address);
            Assert.Equal(command.Name, community.Name);
            Assert.Equal(command.MunicipalityId, community.Municipality.ID);
        }

        [Fact]
        public async void Handle_Success_Partial_Update()
        {
            // Arrange
            var command = new UpdateCommunityCommand
            {
                Id = 1,
                Address = "Calle Nueva 123",
            };
            await DbContextFixture.SeedData(_context);

            var originalCommunity = _context.Community.Include(x => x.Municipality).FirstOrDefault(x => x.ID == command.Id);

            // Act
            var result = await _handler.Handle(command, default);

            // Assert
            Assert.True(result.IsSuccess);
            Assert.Null(result.Response);
            Assert.Null(result.Errors);

            var community = await _context.Community.Include(x => x.Municipality).FirstOrDefaultAsync(x => x.ID == command.Id);

            Assert.NotNull(community);
            Assert.NotNull(originalCommunity);

            Assert.Equal(command.Address, community.Address);
            Assert.Equal(originalCommunity.Name, community.Name);
            Assert.Equal(originalCommunity.Municipality.ID, community.Municipality.ID);
        }

        [Fact]
        public async void Failure_Handle_Community_Not_Found()
        {
            // Arrange
            var command = new UpdateCommunityCommand
            {
                Id = 0,
                Address = "Calle Nueva 123",
                Name = "New Name",
                MunicipalityId = 2
            };
            await DbContextFixture.SeedData(_context);

            // Act
            var result = await _handler.Handle(command, default);

            // Assert
            Assert.False(result.IsSuccess);
            Assert.True(result.IsFailure);

            Assert.Null(result.Response);

            Assert.Null(result.Errors);

            Assert.Equal("La comunidad indicada no se encuentra dentro de nuestros registros.", result.Error.Message);
        }

        [Fact]
        public async void Failure_Handle_Municipality_Not_Found()
        {
            // Arrange
            var command = new UpdateCommunityCommand
            {
                Id = 1,
                Address = "Calle Nueva 123",
                Name = "New Name",
                MunicipalityId = 0
            };
            await DbContextFixture.SeedData(_context);

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
