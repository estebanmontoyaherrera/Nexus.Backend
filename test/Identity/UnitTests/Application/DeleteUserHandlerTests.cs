using FluentAssertions;
using Identity.Application.Interfaces.Persistence;
using Identity.Application.Interfaces.Services;
using Identity.Application.UseCases.Users.Commands.DeleteCommand;
using Identity.Domain.Entities;
using Moq;

namespace Identity.Tests.UnitTests.Application;

public class DeleteUserHandlerTests
{
    private readonly Mock<IUnitOfWork> _unitOfWork = new();
    private readonly Mock<IUserRepository> _userRepository = new();

    public DeleteUserHandlerTests()
    {
        _unitOfWork.SetupGet(x => x.User).Returns(_userRepository.Object);
    }

    [Fact]
    public async Task Handle_WhenUserExists_DeletesAndReturnsSuccess()
    {
        _userRepository.Setup(x => x.GetByIdAsync(7)).ReturnsAsync(new User { FirstName = "A", LastName = "B", Email = "a@a.com", Password = "x" });
        var sut = new DeleteUserHandler(_unitOfWork.Object);

        var result = await sut.Handle(new DeleteUserCommand { UserId = 7 }, CancellationToken.None);

        result.IsSuccess.Should().BeTrue();
        _userRepository.Verify(x => x.DeleteAsync(7), Times.Once);
        _unitOfWork.Verify(x => x.SaveChangesAsync(), Times.Once);
    }

    [Fact]
    public async Task Handle_WhenUserDoesNotExist_ReturnsFailure()
    {
        _userRepository.Setup(x => x.GetByIdAsync(99)).ReturnsAsync((User)null!);
        var sut = new DeleteUserHandler(_unitOfWork.Object);

        var result = await sut.Handle(new DeleteUserCommand { UserId = 99 }, CancellationToken.None);

        result.IsSuccess.Should().BeFalse();
        _userRepository.Verify(x => x.DeleteAsync(It.IsAny<int>()), Times.Never);
    }

    [Fact]
    public async Task Handle_WhenDependencyThrows_ReturnsErrorMessage()
    {
        _userRepository.Setup(x => x.GetByIdAsync(It.IsAny<int>())).ThrowsAsync(new Exception("unexpected"));
        var sut = new DeleteUserHandler(_unitOfWork.Object);

        var result = await sut.Handle(new DeleteUserCommand { UserId = 2 }, CancellationToken.None);

        result.Message.Should().Be("unexpected");
    }
}
