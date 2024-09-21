using Backend.Features.Unit.Commands.Create;
using Backend.Features.Unit.Commands.Update;

namespace IntelificioBackTest.Fixtures
{
    public class UnitFixture
    {
        public static CreateUnitCommand GetUnitCommandTest()
        {
            return new()
            {
                Number = "1",
                Floor = 1,
                Surface = 50.1F,
                UnitTypeId = 1,
                BuildingId = 1
            };
        }

        public static UpdateUnitCommand GetUpdateUnitCommandTest()
        {
            return new()
            {
                Id = 1,
                Number = "2",
                Floor = 2,
                Surface = 50.2F,
                UnitTypeId = 1,
                BuildingId = 1
            };
        }
    }
}
