using Backend.Features.Building.Commands.Create;

namespace IntelificioBackTest.Fixtures
{
    public class BuildingFixture 
    {
        public static CreateBuildingCommand GetCreateBuildingCommandTest()
        {
            return new()
            {
                Name = "As",
                Floors = 10,
                CommunityId = 1
            };
        }
    }
}
