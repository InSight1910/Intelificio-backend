using Microsoft.Extensions.Logging;
using Moq;

namespace IntelificioBackTest.Helpers
{
    public static class LoggerHelper
    {
        public static void AssertLog<T>(Mock<ILogger<T>> logger, LogLevel logLevel, string message)
        {
            logger.Verify(
                logger => logger.Log(
                    It.Is<LogLevel>(logLevel => logLevel == logLevel),
                    It.IsAny<EventId>(),
                    It.Is<It.IsAnyType>((v, t) => v.ToString().Contains(message)),
                    It.IsAny<Exception>(),
                    It.IsAny<Func<It.IsAnyType, Exception, string>>()), // Add formatter
                Times.Once);
        }

    }
}
