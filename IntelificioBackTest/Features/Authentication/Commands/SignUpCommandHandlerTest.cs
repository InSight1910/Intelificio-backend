using AutoMapper;
using Backend.Common.Profiles;
using Backend.Common.Response;
using Backend.Features.Authentication.Commands.Signup;
using Backend.Models;
using IntelificioBackTest.Fixtures;
using IntelificioBackTest.Mocks;
using Microsoft.AspNetCore.Identity;
using Moq;

namespace IntelificioBackTest.Features.Authentication.Commands
{
    public class SignUpCommandHandlerTest
    {
        private readonly IMapper _mapper;
        private readonly Mock<UserManager<User>> _userManager;
        private readonly Mock<RoleManager<Role>> _roleManager;
        private readonly SignUpCommandHandler _handler;
        public SignUpCommandHandlerTest()
        {
            var mapperConfig = new MapperConfiguration(config =>
            {
                config.AddProfile<UserProfile>();
            });
            _mapper = new Mapper(mapperConfig);
            _userManager = UserManagerMock.CreateUserManager();
            _roleManager = new Mock<RoleManager<Role>>();
            _handler = new SignUpCommandHandler(_userManager.Object, _roleManager.Object, _mapper);
        }

        [Fact]
        public async Task Handle_FailedSignUp_UserAlreadyExist()
        {
            //Arrange
            var command = new SignUpCommand
            {
                Email = "test@test.com",
                FirstName = "Test",
                LastName = "LastTest",
                Password = "Test",
                PhoneNumber = "123",
                Rut = "123",

            };
            _ = _userManager.Setup(x => x.FindByEmailAsync(It.IsAny<string>()))
                        .ReturnsAsync(UserFixture.GetUserTest());

            //Act
            var result = await _handler.Handle(command, default);

            //Assest
            Assert.True(result.IsFailure);
            _ = Assert.IsType<Error>(result.Error);
            Assert.NotEmpty(result.Error.Errors);
        }

        [Fact]
        public async Task Handle_FailedSigUp_CreateFailed()
        {
            //Arrange
            var command = new SignUpCommand
            {
                Email = "test@test.com",
                FirstName = "Test",
                LastName = "LastTest",
                Password = "Test",
                PhoneNumber = "123",
                Rut = "123",

            };

            _ = _userManager.Setup(x => x.FindByEmailAsync(It.IsAny<string>()))
                            .ReturnsAsync((User)null);
            _ = _userManager.Setup(x => x.CreateAsync(It.IsAny<User>(), It.IsAny<string>()))
                            .ReturnsAsync(IdentityResult.Failed(new IdentityError { Description = "Password too weak" }));

            //Act
            var result = await _handler.Handle(command, default);

            //Assest
            Assert.True(result.IsFailure);
            _ = Assert.IsType<Error>(result.Error);
            Assert.NotEmpty(result.Error.Errors);
        }

        [Fact]
        public async Task Handle_Success()
        {
            //Arrange
            var command = new SignUpCommand
            {
                Email = "test@test.com",
                FirstName = "Test",
                LastName = "LastTest",
                Password = "Test",
                PhoneNumber = "123",
                Rut = "123"
            };

            _ = _userManager.Setup(x => x.FindByEmailAsync(It.IsAny<string>()))
                            .ReturnsAsync((User)null);
            _ = _userManager.Setup(x => x.CreateAsync(It.IsAny<User>(), It.IsAny<string>()))
                            .ReturnsAsync(IdentityResult.Success);

            //Act
            var result = await _handler.Handle(command, default);

            //Assest
            Assert.True(result.IsSuccess);
            _ = Assert.IsType<Result>(result);
        }
    }
}
