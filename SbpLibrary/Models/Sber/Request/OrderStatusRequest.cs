using System;

namespace SBPLibrary.Models.Sber
{
    public class OrderStatusRequest
    {
        public string RqUid { get; set; }
        public DateTime RqTm { get; set; }
        
        public string OrderId { get; set; }
        public string Tid { get; set; }
        public string PartnerOrderNumber { get; set; }
    }
}
