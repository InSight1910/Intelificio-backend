using Backend.Features.Community.Commands.Create;

namespace IntelificioBackTest.Fixtures
{
    public class CommunityFixture
    {
        public static CreateCommunityCommand GetCommunityCommandTest()
        {
            return new()
            {
                Address = "Calle 123",
                MunicipalityId = 1,
                RUT = "",
                Name = "Comunidad Test"
            };
        }
    }
}
