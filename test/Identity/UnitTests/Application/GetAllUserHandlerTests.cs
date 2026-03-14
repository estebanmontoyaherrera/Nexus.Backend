using FluentAssertions;
using Identity.Application.Interfaces.Persistence;
using Identity.Application.Interfaces.Services;
using Identity.Application.UseCases.Users.Queries.GetAllQuery;
using Identity.Domain.Entities;
using Moq;

namespace Identity.Tests.UnitTests.Application;

public class GetAllUserHandlerTests
{
    private readonly Mock<IUnitOfWork> _unitOfWork = new();
    private readonly Mock<IUserRepository> _userRepository = new();
    private readonly Mock<IOrderingQuery> _orderingQuery = new();

    public GetAllUserHandlerTests()
    {
        _unitOfWork.SetupGet(x => x.User).Returns(_userRepository.Object);
    }

    [Fact]
    public async Task Handle_WhenUsersExist_ReturnsPagedResult()
    {
        var users = new List<User>
        {
            new() { Id = 1, FirstName = "A", LastName = "L", Email = "a@test.com", Password = "x", State = "1", AuditCreateDate = DateTime.UtcNow },
            new() { Id = 2, FirstName = "B", LastName = "K", Email = "b@test.com", Password = "x", State = "1", AuditCreateDate = DateTime.UtcNow }
        }.AsQueryable();

        _userRepository.Setup(x => x.GetAllQueryable()).Returns(users);
        _orderingQuery.Setup(x => x.Ordering(It.IsAny<GetAllUserQuery>(), It.IsAny<IQueryable<User>>()))
            .Returns<GetAllUserQuery, IQueryable<User>>((_, q) => q);

        var sut = new GetAllUserHandler(_unitOfWork.Object, _orderingQuery.Object);
        var result = await sut.Handle(new GetAllUserQuery { NumPage = 1, NumRecordsPage = 10 }, CancellationToken.None);

        result.IsSuccess.Should().BeTrue();
        result.TotalRecords.Should().Be(2);
        result.Data.Should().HaveCount(2);
    }

    [Fact]
    public async Task Handle_WhenStateFilterApplied_ReturnsFilteredUsers()
    {
        var users = new List<User>
        {
            new() { Id = 1, FirstName = "A", LastName = "L", Email = "a@test.com", Password = "x", State = "1", AuditCreateDate = DateTime.UtcNow },
            new() { Id = 2, FirstName = "B", LastName = "K", Email = "b@test.com", Password = "x", State = "0", AuditCreateDate = DateTime.UtcNow }
        }.AsQueryable();

        _userRepository.Setup(x => x.GetAllQueryable()).Returns(users);
        _orderingQuery.Setup(x => x.Ordering(It.IsAny<GetAllUserQuery>(), It.IsAny<IQueryable<User>>()))
            .Returns<GetAllUserQuery, IQueryable<User>>((_, q) => q);

        var sut = new GetAllUserHandler(_unitOfWork.Object, _orderingQuery.Object);
        var result = await sut.Handle(new GetAllUserQuery { StateFilter = "1", NumPage = 1, NumRecordsPage = 10 }, CancellationToken.None);

        result.Data.Should().HaveCount(1);
    }

    [Fact]
    public async Task Handle_WhenRepositoryThrows_ReturnsErrorMessage()
    {
        _userRepository.Setup(x => x.GetAllQueryable()).Throws(new Exception("query-failed"));

        var sut = new GetAllUserHandler(_unitOfWork.Object, _orderingQuery.Object);
        var result = await sut.Handle(new GetAllUserQuery(), CancellationToken.None);

        result.IsSuccess.Should().NotBeTrue();
        result.Message.Should().Be("query-failed");
    }
}
