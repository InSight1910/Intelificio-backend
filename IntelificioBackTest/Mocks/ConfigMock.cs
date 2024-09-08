

using Microsoft.Extensions.Configuration;
using Moq;

namespace IntelificioBackTest.Mocks
{
    public class ConfigMock
    {
        public static Mock<IConfiguration> CreateConfigMock()
        {
            return new Mock<IConfiguration>();
        }
    }
}
