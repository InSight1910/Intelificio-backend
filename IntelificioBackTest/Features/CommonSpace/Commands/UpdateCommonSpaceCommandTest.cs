using AutoMapper;
using Backend.Common.Profiles;
using Backend.Features.CommonSpaces.Commands.Update;
using Backend.Models;
using IntelificioBackTest.Fixtures;
using Microsoft.EntityFrameworkCore.InMemory.Storage.Internal;

namespace IntelificioBackTest.Features.CommonSpace.Commands;

public class UpdateCommonSpaceCommandTest
{
    private readonly UpdateCommonSpaceCommandHandler _handler;
    private readonly IntelificioDbContext _context;
    private readonly IMapper _mapper;

    public UpdateCommonSpaceCommandTest()
    {
        var mapperConfig = new MapperConfiguration(
            config => { config.AddProfile<CommonSpaceProfile>(); });
        _mapper = new Mapper(mapperConfig);
        _context = DbContextFixture.GetDbContext();
        _handler = new UpdateCommonSpaceCommandHandler(_context, _mapper);
    }

    public void Dispose()
    {
        _context.Database.EnsureDeleted();
        _context.Dispose();
    }

    [Fact]
    public async Task Handle_Success()
    {
        // Arrange
        var space = CommonSpaceFixture.CreateCommonSpaceCommand();
        await DbContextFixture.SeedData(_context);
        await _context.CommonSpaces.AddAsync(_mapper.Map<Backend.Models.CommonSpace>(space));
        await _context.SaveChangesAsync();

        var updateSpace = CommonSpaceFixture.UpdateCommonSpaceCommand();
        // Act

        var result = await _handler.Handle(updateSpace, CancellationToken.None);

        // Assert
        Assert.True(result.IsSuccess);
        Assert.Null(result.Error);
    }

    [Fact]
    public async Task Handle_Failure_CommonSpaceNotFound()
    {
        var space = CommonSpaceFixture.UpdateCommonSpaceCommand();

        var result = await _handler.Handle(space, default);

        Assert.True(result.IsFailure);
        Assert.NotNull(result.Error);
        Assert.Equal("No fue posible encontrar el espacio comun indicado.", result.Error.Message);
        Assert.Equal("CommonSpace.Update.CommonSpaceNotFound", result.Error.Code);
    }

    [Fact]
    public async Task Handle_Failure_NameExist()
    {
        // Arrange
        var space = CommonSpaceFixture.CreateCommonSpaceCommand();
        await DbContextFixture.SeedData(_context);
        await _context.CommonSpaces.AddAsync(_mapper.Map<Backend.Models.CommonSpace>(space));
        space.Name = "test";
        await _context.CommonSpaces.AddAsync(_mapper.Map<Backend.Models.CommonSpace>(space));
        await _context.SaveChangesAsync();

        var updateSpace = CommonSpaceFixture.UpdateCommonSpaceCommand();
        updateSpace.Name = space.Name;
        // Act

        var result = await _handler.Handle(updateSpace, CancellationToken.None);

        // Assert
        Assert.True(result.IsFailure);
        Assert.NotNull(result.Error);
        Assert.Equal("Ya existe un espacio comun registrado con ese nombre.", result.Error.Message);
        Assert.Equal("CommonSpace.Update.CommonSpaceNameAlreadyExist", result.Error.Code);
    }
}