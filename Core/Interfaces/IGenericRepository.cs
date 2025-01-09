using Core.Entities;
using Core.Specifications;

namespace Core.Interfaces
{
    public interface IGenericRepository<T> where T : BaseEntity
    {
        Task<T> GetByIdAsync(int? id);
        Task<IReadOnlyList<T>> GetAllAsync();
        Task<T> GetEntityWithSpecificationsAsync(ISpecifications<T> specs);
        Task<IReadOnlyList<T>> GetAllWithSpecificationsAsync(ISpecifications<T> specs);
        Task<int> CountAsync(ISpecifications<T> specifications);
        Task Add(T entity);
        void Update(T entity);
        void Delete(T entity);
    }
}
