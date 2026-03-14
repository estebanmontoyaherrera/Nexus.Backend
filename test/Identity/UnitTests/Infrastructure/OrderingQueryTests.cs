using FluentAssertions;
using Identity.Infrastructure.Services;
using SharedKernel.Commons.Bases;

namespace Identity.Tests.UnitTests.Infrastructure;

public class OrderingQueryTests
{
    private sealed class TestRow
    {
        public int Id { get; init; }
        public string Name { get; init; } = string.Empty;
    }

    [Fact]
    public void Ordering_WithAscendingSort_ReturnsOrderedPage()
    {
        var rows = new List<TestRow>
        {
            new() { Id = 3, Name = "C" },
            new() { Id = 1, Name = "A" },
            new() { Id = 2, Name = "B" }
        }.AsQueryable();

        var sut = new OrderingQuery();
        var request = new BasePagination { Sort = "Id", Order = "asc", NumPage = 1, NumRecordsPage = 2 };

        var result = sut.Ordering(request, rows).ToList();

        result.Select(x => x.Id).Should().Equal(1, 2);
    }

    [Fact]
    public void Ordering_WithDescendingSort_ReturnsOrderedPage()
    {
        var rows = new List<TestRow>
        {
            new() { Id = 1, Name = "A" },
            new() { Id = 2, Name = "B" },
            new() { Id = 3, Name = "C" }
        }.AsQueryable();

        var sut = new OrderingQuery();
        var request = new BasePagination { Sort = "Id", Order = "desc", NumPage = 1, NumRecordsPage = 2 };

        var result = sut.Ordering(request, rows).ToList();

        result.Select(x => x.Id).Should().Equal(3, 2);
    }

    [Fact]
    public void Ordering_WithUnknownColumn_ReturnsOriginalOrdering()
    {
        var rows = new List<TestRow>
        {
            new() { Id = 2, Name = "B" },
            new() { Id = 1, Name = "A" }
        }.AsQueryable();

        var sut = new OrderingQuery();
        var request = new BasePagination { Sort = "Unknown", Order = "asc", NumPage = 1, NumRecordsPage = 10 };

        var result = sut.Ordering(request, rows).ToList();

        result.Select(x => x.Id).Should().Equal(2, 1);
    }
}
