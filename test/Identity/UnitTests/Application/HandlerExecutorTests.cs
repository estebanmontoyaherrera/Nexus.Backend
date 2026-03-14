using FluentAssertions;
using Microsoft.Extensions.Logging;
using Moq;
using SharedKernel.Abstractions.Messaging;
using SharedKernel.Commons.Bases;
using SharedKernel.Commons.Behaviours;

namespace Identity.Tests.UnitTests.Application;

public class HandlerExecutorTests
{
    private readonly Mock<IValidationService> _validationService = new();
    private readonly Mock<ILogger<HandlerExecutor>> _logger = new();

    [Fact]
    public async Task ExecuteAsync_WhenValidationPasses_ReturnsActionResult()
    {
        var sut = new HandlerExecutor(_validationService.Object, _logger.Object);

        var result = await sut.ExecuteAsync<object, bool>(new object(), () => Task.FromResult(new BaseResponse<bool>
        {
            IsSuccess = true,
            Data = true
        }), CancellationToken.None);

        result.IsSuccess.Should().BeTrue();
        result.Data.Should().BeTrue();
    }

    [Fact]
    public async Task ExecuteAsync_WhenValidationFails_ReturnsValidationErrors()
    {
        _validationService
            .Setup(v => v.ValidateAsync(It.IsAny<object>(), It.IsAny<CancellationToken>()))
            .ThrowsAsync(new SharedKernel.Commons.Exceptions.ValidationException(new[]
            {
                new BaseError { PropertyName = "Email", ErrorMessage = "Required" }
            }));

        var sut = new HandlerExecutor(_validationService.Object, _logger.Object);
        var result = await sut.ExecuteAsync<object, bool>(new object(), () => Task.FromResult(new BaseResponse<bool>()), CancellationToken.None);

        result.IsSuccess.Should().BeFalse();
        result.Errors.Should().NotBeNull();
    }

    [Fact]
    public async Task ExecuteAsync_WhenActionThrows_ReturnsUnexpectedError()
    {
        var sut = new HandlerExecutor(_validationService.Object, _logger.Object);

        var result = await sut.ExecuteAsync<object, bool>(new object(), () => throw new InvalidOperationException("boom"), CancellationToken.None);

        result.IsSuccess.Should().BeFalse();
        result.Message.Should().Be("Ocurrió un error inesperado");
    }
}
