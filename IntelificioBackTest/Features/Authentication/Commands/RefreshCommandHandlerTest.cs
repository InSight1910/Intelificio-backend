using Backend.Common.Security;
using Backend.Features.Authentication.Commands.Refresh;
using Backend.Models;
using IntelificioBackTest.Fixtures;
using IntelificioBackTest.Mocks;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.JsonWebTokens;
using Moq;

namespace IntelificioBackTest.Features.Authentication.Commands
{
    public class RefreshCommandHandlerTest
    {
        private readonly Mock<UserManager<User>> _userManager;
        private readonly Mock<IConfiguration> _configuration;
        private readonly TokenProvider _tokenProvider;
        private readonly RefreshCommandHandler _handler;

        private const int TokenExpirationInMinutes = 60;
        private const int RefreshTokenExpireInMinutes = 120;
        private const string Secret = "this-is.a.secret-123.that-has-to-be-of-256-bits";
        private const string Audience = "http://localhost";
        private const string Issuer = "http://localhost";

        public RefreshCommandHandlerTest()
        {
            _configuration = ConfigMock.CreateConfigMock();

            ConfigureConfiguration(_configuration);
            _userManager = UserManagerMock.CreateUserManager();
            _tokenProvider = new TokenProvider(_configuration.Object);
            _handler = new RefreshCommandHandler(_userManager.Object, _tokenProvider, _configuration.Object);
        }

        [Fact]
        public async Task Handle_OnSuccess()
        {
            //Arrange
            var user = UserFixture.GetUserTest();
            var token = _tokenProvider.CreateToken(user, user.Role.Name);
            var refreshToken = _tokenProvider.CreateRefreshToken();
            var command = new RefreshCommand
            {
                Token = token,
                RefreshToken = refreshToken
            };
            user.RefreshToken = refreshToken;
            user.RefreshTokenExpiry = DateTime.UtcNow.AddMinutes(TokenExpirationInMinutes);
            //Act

            _ = _userManager.Setup(x => x.FindByNameAsync(It.IsAny<string>())).ReturnsAsync(user);
            _ = _userManager.Setup(x => x.UpdateAsync(It.IsAny<User>())).ReturnsAsync(IdentityResult.Success);

            var result = await _handler.Handle(command, default);

            //Assest
            Assert.True(result.IsSuccess);
            Assert.NotNull(result.Response?.Data);

            var data = (RefreshCommandResponse)result.Response.Data;
            var jwtHandler = new JsonWebTokenHandler();
            var tokenResponse = jwtHandler.ReadJsonWebToken(data.Token);
            var expiration = tokenResponse.ValidTo;
            Assert.NotNull(token);
            Assert.True(expiration > DateTime.UtcNow);
            Assert.True(expiration <= DateTime.UtcNow.AddMinutes(_configuration.Object.GetValue<int>("Jwt:TokenExpirationInMinutes")));
        }

        [Fact]
        public async Task Handle_OnFailed_DifferentRefreshToken()
        {
            //Arrange
            var user = UserFixture.GetUserTest();
            var token = _tokenProvider.CreateToken(user, user.Role.Name);
            var refreshToken = _tokenProvider.CreateRefreshToken();
            var command = new RefreshCommand
            {
                Token = token,
                RefreshToken = _tokenProvider.CreateRefreshToken()
            };
            user.RefreshToken = refreshToken;
            user.RefreshTokenExpiry = DateTime.UtcNow.AddMinutes(TokenExpirationInMinutes);

            //Act

            _ = _userManager.Setup(x => x.FindByNameAsync(It.IsAny<string>())).ReturnsAsync(user);
            _ = _userManager.Setup(x => x.UpdateAsync(It.IsAny<User>())).ReturnsAsync(IdentityResult.Success);

            var result = await _handler.Handle(command, default);

            //Assest
            Assert.True(result.IsFailure);
            Assert.NotNull(result.Error.Errors);

            var errors = result.Error.Errors;

            Assert.NotNull(errors);
            Assert.Equal(errors.Count(), 1);
        }

        [Fact]
        public async Task Handle_OnFailed_ExpiredRefreshToken()
        {
            //Arrange
            var user = UserFixture.GetUserTest();
            var token = _tokenProvider.CreateToken(user, user.Role.Name);
            var refreshToken = _tokenProvider.CreateRefreshToken();
            var command = new RefreshCommand
            {
                Token = token,
                RefreshToken = refreshToken
            };
            user.RefreshToken = refreshToken;
            user.RefreshTokenExpiry = DateTime.UtcNow.AddMinutes(-TokenExpirationInMinutes);

            //Act

            _ = _userManager.Setup(x => x.FindByNameAsync(It.IsAny<string>())).ReturnsAsync(user);
            _ = _userManager.Setup(x => x.UpdateAsync(It.IsAny<User>())).ReturnsAsync(IdentityResult.Success);

            var result = await _handler.Handle(command, default);

            //Assest
            Assert.True(result.IsFailure);
            Assert.NotNull(result.Error.Errors);

            var errors = result.Error.Errors;

            Assert.NotNull(errors);
            Assert.Equal(errors.Count(), 1);
        }
        [Fact]
        public async Task Handle_OnFailed_ExpiredAndDifferentRefreshToken()
        {
            //Arrange
            var user = UserFixture.GetUserTest();
            var token = _tokenProvider.CreateToken(user, user.Role.Name);
            var refreshToken = _tokenProvider.CreateRefreshToken();
            var command = new RefreshCommand
            {
                Token = token,
                RefreshToken = _tokenProvider.CreateRefreshToken()
            };
            user.RefreshToken = refreshToken;
            user.RefreshTokenExpiry = DateTime.UtcNow.AddMinutes(-TokenExpirationInMinutes);

            //Act

            _ = _userManager.Setup(x => x.FindByNameAsync(It.IsAny<string>())).ReturnsAsync(user);
            _ = _userManager.Setup(x => x.UpdateAsync(It.IsAny<User>())).ReturnsAsync(IdentityResult.Success);

            var result = await _handler.Handle(command, default);

            //Assest
            Assert.True(result.IsFailure);
            Assert.NotNull(result.Error.Errors);

            var errors = result.Error.Errors;

            Assert.NotNull(errors);
            Assert.Equal(errors.Count(), 2);
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
