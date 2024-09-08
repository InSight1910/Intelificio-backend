using AutoMapper;
using AZ_204.Common.Profilers;
using Intelificio_Back.Common.Security;
using Intelificio_Back.Features.Authentication.Commands.Login;
using Intelificio_Back.Features.Authentication.Common;
using Intelificio_Back.Models;
using IntelificioBackTest.Fixtures;
using IntelificioBackTest.Mocks;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Moq;

namespace IntelificioBackTest.Features.Authentication.Commands
{
    public class LogInCommandHandlerTest
    {
        private readonly IMapper _mapper;
        private readonly Mock<UserManager<User>> _userManager;
        private readonly TokenProvider _tokenProvider;
        private readonly LoginCommandHandler _handler;
        private readonly Mock<IConfiguration> _config;


        private const int TokenExpirationInMinutes = 60;
        private const int RefreshTokenExpireInMinutes = 120;
        private const string Secret = "this-is.a.secret-123.that-has-to-be-of-256-bits";
        private const string Audience = "http://localhost";
        private const string Issuer = "http://localhost";


        public LogInCommandHandlerTest()
        {
            var mapperConfig = new MapperConfiguration(config =>
            {
                config.AddProfile<UserProfile>();
            });
            _mapper = new Mapper(mapperConfig);
            _userManager = UserManagerMock.CreateUserManager();
            _config = ConfigMock.CreateConfigMock();
            ConfigureConfiguration(_config);
            _tokenProvider = new TokenProvider(_config.Object);
            _handler = new LoginCommandHandler(_userManager.Object, _tokenProvider, _config.Object);
        }

        [Fact]
        public async Task Handle_FailedLogIn_NotFoundUser()
        {
            //Arrange
            LoginCommand command = new() { Email = "", Password = "" };

            _ = _userManager
                    .Setup(x => x.FindByNameAsync(It.IsAny<string>()))
                    .ReturnsAsync((User)null);

            //Act
            var result = await _handler.Handle(command, default);

            //Assert
            Assert.True(result.IsFailure);
            Assert.NotEmpty(result.Error.Errors);
            Assert.Equal(AuthenticationErrors.UserNotFound.Message, result.Error.Message);
        }

        [Fact]
        public async Task Handle_FailedLogIn_EmailNotConfirmed()
        {
            //Arrange
            LoginCommand command = new() { Email = "", Password = "" };
            User user = UserFixture.GetUserTest();
            user.EmailConfirmed = false;

            _ = _userManager
                    .Setup(x => x.FindByNameAsync(It.IsAny<string>()))
                    .ReturnsAsync(user);

            //Act
            var result = await _handler.Handle(command, default);

            //Assert
            Assert.True(result.IsFailure);
            Assert.NotEmpty(result.Error.Errors);
            Assert.Equal(AuthenticationErrors.EmailNotConfirmed.Message, result.Error.Message);
        }

        [Fact]
        public async Task Handle_FailedLogIn_UserBlocked()
        {
            //Arrange
            LoginCommand command = new() { Email = "", Password = "" };
            User user = UserFixture.GetUserTest();
            user.EmailConfirmed = true;
            user.LockoutEnabled = true;

            _ = _userManager
                    .Setup(x => x.FindByNameAsync(It.IsAny<string>()))
                    .ReturnsAsync(user);

            //Act
            var result = await _handler.Handle(command, default);

            //Assert
            Assert.True(result.IsFailure);
            Assert.NotEmpty(result.Error.Errors);
            Assert.Equal(AuthenticationErrors.UserBlocked.Message, result.Error.Message);
        }
        [Fact]
        public async Task Handle_FailedLogIn_InvalidPassword()
        {
            //Arrange
            LoginCommand command = new() { Email = "", Password = "" };
            User user = UserFixture.GetUserTest();
            user.EmailConfirmed = true;

            _ = _userManager
                    .Setup(x => x.CheckPasswordAsync(user, It.IsAny<string>()))
                    .ReturnsAsync(false);

            //Act
            var result = await _handler.Handle(command, default);

            //Assert
            Assert.True(result.IsFailure);
            Assert.NotEmpty(result.Error.Errors);
        }
        [Fact]
        public async Task Handle_SuccessLogIn()
        {
            //Arrange
            LoginCommand command = new() { Email = "", Password = "" };

            var user = UserFixture.GetUserTest();

            _ = _userManager
                .Setup(x => x.FindByNameAsync(It.IsAny<string>()))
                .ReturnsAsync(user);

            _ = _userManager
                    .Setup(x => x.CheckPasswordAsync(user, It.IsAny<string>()))
                    .ReturnsAsync(true);
            _ = _userManager
                    .Setup(x => x.UpdateAsync(It.IsAny<User>()))
                    .ReturnsAsync(IdentityResult.Success);

            //Act
            var result = await _handler.Handle(command, default);

            //Assert
            Assert.True(result.IsSuccess);
            _ = Assert.IsType<LoginCommandResponse>(result.Response.Data);
            var data = (LoginCommandResponse)result.Response.Data;
            Assert.NotEmpty(data.Token);
            Assert.NotEmpty(data.RefreshToken);
        }

        private void ConfigureConfiguration(Mock<IConfiguration> _configuration)
        {
            var tokenExpirationSectionMock = new Mock<IConfigurationSection>();
            _ = tokenExpirationSectionMock.SetupGet(m => m.Value).Returns(TokenExpirationInMinutes.ToString());

            var refreshTokenExpirationSectionMock = new Mock<IConfigurationSection>();
            _ = refreshTokenExpirationSectionMock.SetupGet(m => m.Value).Returns(RefreshTokenExpireInMinutes.ToString());

            var secretSectionMock = new Mock<IConfigurationSection>();
            _ = secretSectionMock.SetupGet(m => m.Value).Returns(Secret);

            var audienceSectionMock = new Mock<IConfigurationSection>();
            _ = audienceSectionMock.SetupGet(m => m.Value).Returns(Audience);

            var issuerSectionMock = new Mock<IConfigurationSection>();
            _ = issuerSectionMock.SetupGet(m => m.Value).Returns(Issuer);

            // Setup IConfiguration to return the mocked sections
            _ = _configuration.Setup(c => c.GetSection("Jwt:TokenExpirationInMinutes")).Returns(tokenExpirationSectionMock.Object);
            _ = _configuration.Setup(c => c.GetSection("Jwt:RefreshTokenExpireInMinutes")).Returns(refreshTokenExpirationSectionMock.Object);
            _ = _configuration.Setup(c => c.GetSection("Jwt:Secret")).Returns(secretSectionMock.Object);
            _ = _configuration.Setup(c => c.GetSection("Jwt:Audience")).Returns(audienceSectionMock.Object);
            _ = _configuration.Setup(c => c.GetSection("Jwt:Issuer")).Returns(issuerSectionMock.Object);

        }
    }
}
