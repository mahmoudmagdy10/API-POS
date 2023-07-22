 using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Talabat.Core.Entities;
using Talabat.Core.IRepository;
using Talabat.Core.ISpecification;
using Talabat.Repository.Data;
using Talabat.Repository.Specification;

namespace Talabat.Repository.Repositories
{
    public class GenericRep<T> : IGenericRep<T> where T : BaseEntity
    {
        private readonly StoreContext db;

        public GenericRep(StoreContext db)
        {
            this.db = db;
        }
        public async Task<IReadOnlyList<T>> GetAllAsync()
            => await db.Set<T>().ToListAsync();

        public async Task<T> GetByIdAsync(int id) =>
            //return await db.Set<T>().Where(p => p.Id == id).FirstOrDefaultAsync();
            await db.Set<T>().FindAsync(id);


        public async Task<IReadOnlyList<T>> GetAllWithSpecAsync(ISpecification<T> spec)
        {
            return await ApplySpecifications(spec).ToListAsync();
        }

        public async Task<T> GetEntityWithSpecAsync(ISpecification<T> spec)
        {
            return await ApplySpecifications(spec).FirstOrDefaultAsync();
        }

        public async Task<int> GetCountWithSpecAsync(ISpecification<T> spec)
        {
            return await ApplySpecifications(spec).CountAsync();
        }


        private IQueryable<T> ApplySpecifications(ISpecification<T> spec)
        {
            return SpecificationEvaluator<T>.GetQuery(db.Set<T>(), spec);
        }

        public async Task Add(T entity)
            => await db.Set<T>().AddAsync(entity);

        public void Update(T entity)
            => db.Set<T>().Update(entity);

        public void Delete(T entity)
            => db.Set<T>().Remove(entity);
    }
}
