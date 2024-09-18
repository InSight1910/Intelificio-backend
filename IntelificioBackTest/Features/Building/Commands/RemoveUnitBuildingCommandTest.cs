using AutoMapper;
using Backend.Common.Profiles;
using Backend.Features.Building.Commands.RemoveUnit;
using Backend.Features.Building.Common;
using Backend.Models;
using IntelificioBackTest.Fixtures;
using Microsoft.Extensions.Logging;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IntelificioBackTest.Features.Building.Commands
{
    public class RemoveUnitBuildingCommandTest
    {
        private readonly RemoveUnitBuildingCommandHandler _handler;
        private readonly IntelificioDbContext _context;
        private readonly Mock<ILogger<RemoveUnitBuildingCommandHandler>> _logger;
        private IMapper _mapper;

        public RemoveUnitBuildingCommandTest()
        {
            var mapperConfig = new MapperConfiguration(config =>
            {
                config.AddProfile<BuildingProfile>();
            });

            _mapper = new Mapper(mapperConfig);
            _logger = new Mock<ILogger<RemoveUnitBuildingCommandHandler>>();
            _context = DbContextFixture.GetDbContext();
            _handler = new RemoveUnitBuildingCommandHandler(_context, _logger.Object, _mapper);
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
            var command = new RemoveUnitBuildingCommand
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
            var command = new RemoveUnitBuildingCommand
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
        public async Task Failure_Handle_Unit_Not_Found()
        {
            // Arrange
            var command = new RemoveUnitBuildingCommand
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
        public async Task Failure_Handle_Unit_Does_Not_Exist_In_Building()
        {
            // Arrange
            var command = new RemoveUnitBuildingCommand
            {
                BuildingId = 2,
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
            Assert.Equal("La Unidad no existe en edificio indicado.", result.Error.Message);
        }

    }
}
