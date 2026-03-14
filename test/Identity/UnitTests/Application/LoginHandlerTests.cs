using FluentAssertions;
using Identity.Application.Interfaces.Authentication;
using Identity.Application.Interfaces.Persistence;
using Identity.Application.Interfaces.Services;
using Identity.Application.UseCases.Users.Queries.LoginQuery;
using Identity.Domain.Entities;
using Moq;

namespace Identity.Tests.UnitTests.Application;

public class LoginHandlerTests
{
    private readonly Mock<IUnitOfWork> _unitOfWork = new();
    private readonly Mock<IUserRepository> _userRepository = new();
    private readonly Mock<IRefreshTokenRepository> _refreshTokenRepository = new();
    private readonly Mock<IJwtTokenGenerator> _jwtGenerator = new();

    public LoginHandlerTests()
    {
        _unitOfWork.SetupGet(x => x.User).Returns(_userRepository.Object);
        _unitOfWork.SetupGet(x => x.RefreshToken).Returns(_refreshTokenRepository.Object);
        _jwtGenerator.Setup(x => x.GenerateToken(It.IsAny<User>())).Returns("jwt-token");
        _jwtGenerator.Setup(x => x.GenerateRefreshToken()).Returns("refresh-token");
    }

    [Fact]
    public async Task Handle_WithValidCredentials_ReturnsJwtAndRefreshToken()
    {
        var user = new User
        {
            Id = 5,
            FirstName = "Valid",
            LastName = "User",
            Email = "valid@test.com",
            Password = BCrypt.Net.BCrypt.HashPassword("P@ssw0rd!"),
            State = "1"
        };
        _userRepository.Setup(x => x.UserByEmailAsync(user.Email)).ReturnsAsync(user);

        var sut = new LoginHandler(_unitOfWork.Object, _jwtGenerator.Object);
        var result = await sut.Handle(new LoginQuery { Email = user.Email, Password = "P@ssw0rd!" }, CancellationToken.None);

        result.IsSuccess.Should().BeTrue();
        result.AccessToken.Should().Be("jwt-token");
        result.RefreshToken.Should().Be("refresh-token");
        _refreshTokenRepository.Verify(x => x.CreateToken(It.IsAny<RefreshToken>()), Times.Once);
    }

    [Fact]
    public async Task Handle_WithInvalidCredentials_ReturnsFailure()
    {
        var user = new User
        {
            Email = "valid@test.com",
            FirstName = "Invalid",
            LastName = "User",
            Password = BCrypt.Net.BCrypt.HashPassword("right"),
            State = "1"
        };
        _userRepository.Setup(x => x.UserByEmailAsync(user.Email)).ReturnsAsync(user);

        var sut = new LoginHandler(_unitOfWork.Object, _jwtGenerator.Object);
        var result = await sut.Handle(new LoginQuery { Email = user.Email, Password = "wrong" }, CancellationToken.None);

        result.IsSuccess.Should().BeFalse();
        result.AccessToken.Should().BeNull();
    }

    [Fact]
    public async Task Handle_WhenRepositoryThrows_ReturnsFailureMessage()
    {
        _userRepository.Setup(x => x.UserByEmailAsync(It.IsAny<string>())).ThrowsAsync(new Exception("db failure"));
        var sut = new LoginHandler(_unitOfWork.Object, _jwtGenerator.Object);

        var result = await sut.Handle(new LoginQuery { Email = "a@a.com", Password = "x" }, CancellationToken.None);

        result.IsSuccess.Should().NotBeTrue();
        result.Message.Should().Be("db failure");
    }
}
