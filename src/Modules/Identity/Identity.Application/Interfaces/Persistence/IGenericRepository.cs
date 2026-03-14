using Identity.Domain.Entities;

namespace Identity.Application.Interfaces.Persistence;

public interface IGenericRepository<T> where T : BaseEntity
{
    IQueryable<T> GetAllQueryable();
    Task<IEnumerable<T>> GetAllAsync();
    Task<T> GetByIdAsync(int id);
    Task CreateAsync(T entity);
    void UpdateAsync(T entity);
    Task DeleteAsync(int id);
}
