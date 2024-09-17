using Backend.Features.Building.Commands.Create;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IntelificioBackTest.Fixtures
{
    public class BuildingFixture 
    {
        public static CreateBuildingCommand GetCreateBuildingCommandTest()
        {
            return new()
            {
                Name = "A",
                Floors = 10,
                CommunityId = 1
            };
        }
    }
}
