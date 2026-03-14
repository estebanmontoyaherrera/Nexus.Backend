using FluentAssertions;
using Identity.Application.Interfaces.Persistence;
using Identity.Application.Interfaces.Services;
using Identity.Application.UseCases.Users.Commands.CreateCommand;
using Identity.Domain.Entities;
using Identity.Tests.Builders;
using Moq;

namespace Identity.Tests.UnitTests.Application;

public class CreateUserHandlerTests
{
    private readonly Mock<IUnitOfWork> _unitOfWork = new();
    private readonly Mock<IUserRepository> _userRepository = new();

    public CreateUserHandlerTests()
    {
        _unitOfWork.SetupGet(x => x.User).Returns(_userRepository.Object);
    }

    [Fact]
    public async Task Handle_WithValidRequest_ReturnsSuccessfulResponse()
    {
        var command = new CreateUserCommandBuilder().Build();
        var sut = new CreateUserHandler(_unitOfWork.Object);

        var result = await sut.Handle(command, CancellationToken.None);

        result.IsSuccess.Should().BeTrue();
        result.Message.Should().NotBeNullOrWhiteSpace();
        _userRepository.Verify(x => x.CreateAsync(It.Is<User>(u => u.Email == command.Email)), Times.Once);
        _unitOfWork.Verify(x => x.SaveChangesAsync(), Times.Once);
    }

    [Fact]
    public async Task Handle_WhenRepositoryFails_ReturnsFailureMessage()
    {
        var command = new CreateUserCommandBuilder().Build();
        _userRepository.Setup(x => x.CreateAsync(It.IsAny<User>())).ThrowsAsync(new InvalidOperationException("db-error"));

        var sut = new CreateUserHandler(_unitOfWork.Object);
        var result = await sut.Handle(command, CancellationToken.None);

        result.IsSuccess.Should().NotBeTrue();
        result.Message.Should().Be("db-error");
    }

    [Fact]
    public async Task Handle_WhenPasswordProvided_StoresHashedPassword()
    {
        var command = new CreateUserCommandBuilder().WithPassword("MyStrongPassword").Build();
        var sut = new CreateUserHandler(_unitOfWork.Object);

        await sut.Handle(command, CancellationToken.None);

        _userRepository.Verify(x => x.CreateAsync(It.Is<User>(u => u.Password != command.Password)), Times.Once);
    }
}
