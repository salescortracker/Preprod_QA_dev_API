using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.Common
{
    public class ApiResponse<T>
    {
        public bool Success { get; set; } = true;
        public string Message { get; set; } = string.Empty;
        public T? Data { get; set; }
        public ApiResponse() { }
        public ApiResponse(T? data, string message = "", bool success = true)
        {
            Success = success;
            Message = message;
            Data = data;
        }
    }
}
