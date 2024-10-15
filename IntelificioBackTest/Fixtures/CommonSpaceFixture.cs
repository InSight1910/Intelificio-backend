using Backend.Features.CommonSpaces.Commands.Create;
using Backend.Features.CommonSpaces.Commands.Update;

namespace IntelificioBackTest.Fixtures;

public class CommonSpaceFixture
{
    public static CreateCommonSpaceCommand CreateCommonSpaceCommand()
    {
        return new CreateCommonSpaceCommand
        {
            Name = "CommonSpace",
            CommunityId = 1,
            Capacity = 100,
            Location = "Roof"
        };
    }

    public static UpdateCommonSpaceCommand UpdateCommonSpaceCommand()
    {
        return new UpdateCommonSpaceCommand
        {
            Name = "CommonSpace",
            Capacity = 10,
            Location = "Roof",
            IsInMaintenance = false,
            Id = 1
        };
    }
}