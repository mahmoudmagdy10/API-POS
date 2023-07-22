using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Talabat.Core.Entities;

namespace Talabat.Core.ISpecification
{
    public interface ISpecification<T> where T : BaseEntity
    {
        // This Interface is just signiture of properity
        public Expression<Func<T,bool>> Criteria { get; set; } // Condition (Where)
        public List<Expression<Func<T,object>>> Includes { get; set; } // List Of Includes
        public Expression<Func<T,object>> OrderBy { get; set; } 
        public Expression<Func<T,object>> OrderByDesc { get; set; }
        public int Skip { get; set; }
        public int Take { get; set; }
        public bool IsPaginationEnable { get; set; }
    }
}
