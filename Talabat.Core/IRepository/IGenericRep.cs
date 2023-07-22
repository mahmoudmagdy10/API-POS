using Talabat.Core.Entities;
using Talabat.Core.ISpecification;

namespace Talabat.Core.IRepository
{
    public interface IGenericRep<T> where T : BaseEntity
    {
        Task<IReadOnlyList<T>> GetAllAsync();
        Task<T> GetByIdAsync(int id);

        Task<IReadOnlyList<T>> GetAllWithSpecAsync(ISpecification<T> spec);
        Task<T> GetEntityWithSpecAsync(ISpecification<T> spec);

        Task<int> GetCountWithSpecAsync(ISpecification<T> spec);

        Task Add(T entity);

        void Update(T entity);
        void Delete(T entity);


    }
}
