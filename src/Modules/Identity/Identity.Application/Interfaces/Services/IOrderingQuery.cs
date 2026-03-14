using SharedKernel.Commons.Bases;

namespace Identity.Application.Interfaces.Services;

public interface IOrderingQuery
{
    IQueryable<T> Ordering<T>(BasePagination request, IQueryable<T> queryable) where T : class;
}
