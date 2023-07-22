using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Talabat.Core.Entities;
using Talabat.Core.ISpecification;

namespace Talabat.Repository.Specification
{
    public static class SpecificationEvaluator<TEntity> where TEntity : BaseEntity
    {
        public static IQueryable<TEntity> GetQuery(IQueryable<TEntity> inputQuery, ISpecification<TEntity> spec)
        {
            var query = inputQuery;

            if(spec.Criteria is not null) 
                query = query.Where(spec.Criteria);

            if (spec.IsPaginationEnable)
                query = query.Skip(spec.Skip).Take(spec.Take);

            if (spec.OrderBy is not null)
                query = query.OrderBy(spec.OrderBy);

            if (spec.OrderByDesc is not null)
                query = query.OrderBy(spec.OrderByDesc);

            if(spec.Includes is not null)
            {
                query = spec.Includes.Aggregate(query, (CurrentQuery, IncludeExe) => CurrentQuery.Include(IncludeExe));
            }

            return query;
        }
        
    }
}
