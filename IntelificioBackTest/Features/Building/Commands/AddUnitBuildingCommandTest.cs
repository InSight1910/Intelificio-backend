using AutoMapper;
using Backend.Common.Profiles;
using Backend.Features.Building.Commands.AddUnit;
using Backend.Features.Building.Common;
using Backend.Models;
using IntelificioBackTest.Fixtures;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Moq;

namespace IntelificioBackTest.Features.Building.Commands
{
    public class AddUnitBuildingCommandTest
    {
        private readonly AddUnitBuildingCommandHandler _handler;
        private readonly IntelificioDbContext _context;
        private readonly IMapper _mapper;
        private readonly Mock<ILogger<AddUnitBuildingCommandHandler>> _logger;

        public AddUnitBuildingCommandTest()
        {
            var mapperConfig = new MapperConfiguration(config =>
            {
                config.AddProfile<BuildingProfile>();
            });

            _mapper = new Mapper(mapperConfig);
            _logger = new Mock<ILogger<AddUnitBuildingCommandHandler>>();
            _context = DbContextFixture.GetDbContext();
            _handler = new AddUnitBuildingCommandHandler(_context, _logger.Object, _mapper);
        }

        
        public void Dispose()
        {
            _ = _context.Database.EnsureDeleted();
            _context.Dispose();
        }

        [Fact]
        public async Task Handle_Success()
        {
            //Arrange
            // Crear una nueva unidad agregarla al arrange
            var command = new AddUnitBuildingCommand
            {
                BuildingId = 1,
                UnitId = 1
            };
            await DbContextFixture.SeedData(_context);

            // Act
            var result = await _handler.Handle(command, default);

            // Assert
            Assert.True(result.IsSuccess);
            Assert.False(result.IsFailure);
            Assert.Null(result.Response);
            Assert.Null(result.Errors);
        }

        [Fact]
        public async Task Failure_Handle_Building_Not_Found()
        {
            // Arrange
            var command = new AddUnitBuildingCommand
            { 
                BuildingId = 0,
                UnitId = 1
            };
            await DbContextFixture.SeedData(_context);

            // Act
            var result = await _handler.Handle(command, default);

            // Assert
            Assert.False(result.IsSuccess);
            Assert.True(result.IsFailure);
            Assert.Null(result.Response);
            Assert.Null(result.Errors);
            Assert.Equal("Edificio no fue encontrado.", result.Error.Message);
        }

        [Fact]
        public async Task Failure_Handle_Unit_not_Found()
        {
            // Arrange
            var command = new AddUnitBuildingCommand
            {
                BuildingId = 1,
                UnitId = 0
            };
            await DbContextFixture.SeedData(_context);

            // Act
            var result = await _handler.Handle(command, default);

            // Assert
            Assert.False(result.IsSuccess);
            Assert.True(result.IsFailure);
            Assert.Null(result.Response);
            Assert.Null(result.Errors);
            Assert.Equal("Unidad no fue encontrada.", result.Error.Message);
        }

        [Fact]
        public async Task Failure_Handle_Unit_Already_Exist()
        {
            // Arrange
            var command = new AddUnitBuildingCommand
            {
                BuildingId = 1,
                UnitId = 1
            };
            await DbContextFixture.SeedData(_context);

            var unit = await _context.Units.FirstOrDefaultAsync(x => x.ID == command.UnitId);
            var building = await _context.Buildings.FirstOrDefaultAsync(x => x.ID == command.BuildingId);

            building?.Units.Add(unit); 

            // Act
            var result = await _handler.Handle(command, default);

            // Assert
            Assert.False(result.IsSuccess);
            Assert.True(result.IsFailure);
            Assert.Null(result.Response);
            Assert.Null(result.Errors);
            Assert.Equal("Unidad ya pertenece al Edificio indicado.", result.Error.Message);
        }
    }
}
