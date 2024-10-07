using AutoMapper;
using Backend.Common.Profiles;
using Backend.Features.CommonSpaces.Commands.Create;
using Backend.Models;
using IntelificioBackTest.Fixtures;

namespace IntelificioBackTest.Features.CommonSpace.Commands;

public class CreateCommonSpaceCommandTest
{
    private readonly CreateCommonSpaceCommandHandler _handler;
    private readonly IntelificioDbContext _context;
    private readonly IMapper _mapper;

    public CreateCommonSpaceCommandTest()
    {
        var mapperConfig = new MapperConfiguration(
            config => { config.AddProfile<CommonSpaceProfile>(); });

        _mapper = new Mapper(mapperConfig);
        _context = DbContextFixture.GetDbContext();
        _handler = new CreateCommonSpaceCommandHandler(_context, _mapper);
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
        var command = CommonSpaceFixture.CreateCommonSpaceCommand();
        await DbContextFixture.SeedData(_context);

        // Act
        var result = await _handler.Handle(command, default);

        // Assert
        Assert.True(result.IsSuccess);
        Assert.NotNull(result.Response);
        Assert.Null(result.Error);
    }

    [Fact]
    public async Task Handler_CommunityNotExist()
    {
        // Arrange
        var command = CommonSpaceFixture.CreateCommonSpaceCommand();
        await DbContextFixture.SeedData(_context);
        command.CommunityId = 100;

        // Act
        var result = await _handler.Handle(command, default);

        // Assert
        Assert.True(result.IsFailure);
        Assert.NotNull(result.Error);
        Assert.Equal("Comunidad no fue encontrada.", result.Error.Message);
        Assert.Equal("CommonSpace.Create.CommunityNotFoundOnCreate", result.Error.Code);
    }

    [Fact]
    public async Task Handle_AlreadyExist()
    {
        // Arrange
        var command = CommonSpaceFixture.CreateCommonSpaceCommand();
        await DbContextFixture.SeedData(_context);

        _context.CommonSpaces.AddAsync(new Backend.Models.CommonSpace
        {
            Name = command.Name,
            Capacity = command.Capacity,
            CommunityId = command.CommunityId,
            Location = command.Location
        });
        await _context.SaveChangesAsync();

        // Act
        var result = await _handler.Handle(command, default);

        // Assert
        Assert.True(result.IsFailure);
        Assert.NotNull(result.Error);
        Assert.Equal("El espacio comun ya se encuentra registrado.", result.Error.Message);
        Assert.Equal("CommonSpace.Create.CommonSpaceAlreadyExist", result.Error.Code);
    }
}