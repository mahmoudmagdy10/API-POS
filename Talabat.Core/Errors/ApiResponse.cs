using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Talabat.Core.Errors
{
    public class ApiResponse
    {
        public int StatusCode { get; set; }
        public string Message { get; set; }
        public string Details { get; set; }

        public ApiResponse(int statusCode, string? message = null, string? details = null)
        {
            StatusCode = statusCode;
            Message = GetErrorMessage(statusCode);
            Details = details;
        }

        private static string? GetErrorMessage(int code)
        {
            return code switch
            {
                400 => "Bad Request",
                401 => "UnAuthorized",
                404 => "Not Found Request",
                500 => "Internal Server Error",
                _ => null
            };
        }
    }
}
