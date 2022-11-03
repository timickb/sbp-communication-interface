using System;

namespace SBPLibrary.Models.Sber
{
    public class RevokeRequest
    {
        public string RqUid { get; set; }
        public DateTime RqTm { get; set; }
        public string OrderId { get; set; }
    }
}
