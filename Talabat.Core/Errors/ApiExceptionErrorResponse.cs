using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Talabat.Core.Errors
{
    public class ApiExceptionErrorResponse : ApiResponse
    {
        public string Details { get; set; }

        public ApiExceptionErrorResponse(string? details = null): base(500)
        {
            Details = details;
        }
    }
}
