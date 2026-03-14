using SharedKernel.Commons.Bases;

namespace Identity.Infrastructure.Services;

public static class PaginateQuery
{
    public static IQueryable<T> Paginate<T>(this IQueryable<T> queryable, BasePagination request)
    {
        return queryable.Skip((request.NumPage - 1) * request.Records).Take(request.Records);
    }
}
