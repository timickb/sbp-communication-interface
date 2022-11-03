using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace SBPLibrary.Models
{
    public class ErrorModel<T>
    {
        public bool IsError { get; set; } = false;
        public string ErrorMsg { get; set; } = "";
        public HttpStatusCode RespCode { get; set; } = HttpStatusCode.InternalServerError;
        public T RespObject { get; set; } = default;
    }
}
