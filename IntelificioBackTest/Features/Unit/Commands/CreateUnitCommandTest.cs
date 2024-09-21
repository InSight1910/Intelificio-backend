using AutoMapper;
using Backend.Common.Profiles;
using Backend.Features.Unit.Commands.Create;
using Backend.Models;
using IntelificioBackTest.Fixtures;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Moq;

namespace IntelificioBackTest.Features.Unit.Commands
{
    public class CreateUnitCommandTest
    {
        private readonly CreateUnitCommandHandler _handler;
        private readonly Mock<ILogger<CreateUnitCommandHandler>> _logger;
        private readonly IMapper _mapper;
        private readonly IntelificioDbContext _context;

        public CreateUnitCommandTest()
        {
            var mapperConfig = new MapperConfiguration(static config =>
            {
                config.AddProfile<UnitProfile>();
            });

            _context = DbContextFixture.GetDbContext();
            _mapper = new Mapper(mapperConfig);
            _logger = new Mock<ILogger<CreateUnitCommandHandler>>();
            _handler = new CreateUnitCommandHandler(_context, _logger.Object, _mapper);
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
            var command = UnitFixture.GetUnitCommandTest();
            await DbContextFixture.SeedData(_context);

            // Act
            var result = await _handler.Handle(command, default);

            // Assert
            Assert.True(result.IsSuccess);
            Assert.Null(result.Response);
            Assert.Null(result.Errors);
        }

        [Fact]
        public async Task Failure_Handle_UnitAlreadyExists()
        {
            // Arrange
            var command = UnitFixture.GetUnitCommandTest();
            await DbContextFixture.SeedData(_context);

            command.Number = await _context.Units.Select(x => x.Number).FirstOrDefaultAsync();

            // Act
            var result = await _handler.Handle(command, default);

            // Assert
            Assert.False(result.IsSuccess);
            Assert.True(result.IsFailure);
            Assert.Null(result.Response);
            Assert.Null(result.Errors);
            Assert.Equal("La unidad ya ha sido registrada", result.Error.Message);
        }

        [Fact]
        public async Task Failure_Handle_UnitTypeNotFound()
        {
            // Arrange
            var command = UnitFixture.GetUnitCommandTest();
            await DbContextFixture.SeedData(_context);

            command.UnitTypeId = 0;

            // Act
            var result = await _handler.Handle(command, default);

            // Assert
            Assert.False(result.IsSuccess);
            Assert.True(result.IsFailure);
            Assert.Null(result.Response);
            Assert.Null(result.Errors);
            Assert.Equal("El tipo de unidad no fue encontrado", result.Error.Message);
        }

        [Fact]
        public async Task Failure_Handle_BuildingNotFound()
        {
            //Arrange
            var command = UnitFixture.GetUnitCommandTest();
            await DbContextFixture.SeedData(_context);

            command.BuildingId = 0;

            //Act
            var result = await _handler.Handle(command, default);

            //Assert
            Assert.False(result.IsSuccess);
            Assert.True(result.IsFailure);
            Assert.Null(result.Response);
            Assert.Null(result.Errors);
            Assert.Equal("El edificio no fue encontrado", result.Error.Message);
        }
    }
}