using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Talabat.Core;
using Talabat.Core.Entities;
using Talabat.Core.Entities.Order_Aggregate;
using Talabat.Core.IRepository;
using Talabat.Repository.Data;
using Talabat.Repository.Repositories;

namespace Talabat.Repository
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly StoreContext _dbContext;
        private Hashtable _repositories;

        //public IGenericRep<Product> ProductRepo { get; set; }
        //public IGenericRep<ProductBrand> BrandRepo { get; set; }
        //public IGenericRep<ProductType> TypeRepo { get; set; }
        //public IGenericRep<DeliveryMethod> DeliveryMethodRepo { get; set; }
        //public IGenericRep<OrderItem> OrderItemsRepo { get; set; }
        //public IGenericRep<Order> OrderRepo { get; set; }

        public UnitOfWork(StoreContext dbContext)
        {
            _dbContext = dbContext;

            //ProductRepo = new GenericRep<Product>(_dbContext);
            //BrandRepo = new GenericRep<ProductBrand>(_dbContext);
            //TypeRepo = new GenericRep<ProductType>(_dbContext);
            //DeliveryMethodRepo = new GenericRep<DeliveryMethod>(_dbContext);
            //OrderItemsRepo = new GenericRep<OrderItem>(_dbContext);
            //OrderRepo = new GenericRep<Order>(_dbContext);
        }

        public IGenericRep<TEntity>? Repository<TEntity>() where TEntity : BaseEntity
        {
            if (_repositories is null)
                    _repositories = new Hashtable();

            var type = typeof(TEntity).Name; // refactor get the entity name at run time

            if(!_repositories.ContainsKey(type))
            {
                var repository = new GenericRep<TEntity>(_dbContext);

                _repositories.Add(type, repository);
            }

            return _repositories[type] as IGenericRep<TEntity>;
        }

        public async Task<int> Complete()
            => await _dbContext.SaveChangesAsync();

        public async ValueTask DisposeAsync()
            => await _dbContext.DisposeAsync();


    }
}
