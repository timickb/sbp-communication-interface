using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SBPLibrary.Models.Sber;

namespace SBPLibrary.Models
{

    public class EventModel
    {
        public bool IsError { get; set; } = false;
        public string ErrorMessage { get; set; } = "";
        public string OrderId { get; set; } = "";
        public string OperationId { get; set; } = "";
        public string QrUrl { get; set; } = "";
        public OrderStatus OrderStatus { get; set; }
    }
}
