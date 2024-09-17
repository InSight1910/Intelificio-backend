using AutoMapper;
using Backend.Common.Profiles;
using Backend.Features.Building.Commands.AddUnit;
using Backend.Models;
using IntelificioBackTest.Fixtures;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
        public async void Handle_Success()
        {
            //Arrange
            var command = new AddUnitBuildingCommand
            {
                BuildingId = 1,
                UnitId = 1
            };

            // Act
            var result = await _handler.Handle(command, default);

            // Assert
            Assert.True(result.IsSuccess);
            Assert.Null(result.Response);
            Assert.Null(result.Errors);
        }

        [Fact]
        public async void Failure_Handle_Building_Not_Found()
        {
            // Arrange
            var command = new AddUnitBuildingCommand
            { 
                BuildingId = 0,
                UnitId = 1
            };

            // Act
            var result = await _handler.Handle(command, default);

            // Assert

            Assert.True(result.IsFailure);
            Assert.Null(result.Response);
            Assert.Null(result.Errors);
        }

        [Fact]
        public async void Failure_Handle_Unit_not_Found()
        {
            // Arrange
            var command = new AddUnitBuildingCommand
            {
                BuildingId = 1,
                UnitId = 0
            };

            // Act
            var result = await _handler.Handle(command, default);

            // Assert

            Assert.True(result.IsFailure);
            Assert.Null(result.Response);
            Assert.Null(result.Errors);
        }

        [Fact]
        public async void Failure_Handle_Unit_Already_Exist()
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

            building.Units.Add(unit);

            // Act
            var result = await _handler.Handle(command, default);

            // Assert

            Assert.True(result.IsFailure);
            Assert.Null(result.Response);
            Assert.Null(result.Errors);
        }
    }
}
