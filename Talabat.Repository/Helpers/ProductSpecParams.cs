using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Talabat.Repository.Helpers
{
    public class ProductSpecParams
    {
        private const int MAXPAGESIZE = 10;

        private int pageSize = 5;

        public int PageSize
        {
            get { return pageSize; }
            set { pageSize = value > MAXPAGESIZE ? MAXPAGESIZE : value; }
        }
        public int PageIndex { get; set; } = 1;
        public string? Sort { get; set; }

    }
}
