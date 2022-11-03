using System;
using System.Collections.Generic;

namespace SBPLibrary.Models.Sber
{
    public class CreatePaymentRequest
    {
        public string RqUid { get; set; }
        public DateTime RqTm { get; set; }
        public string MemberId { get; set; }

        /// <summary>
        /// For Sberbank is always a constant, you shouldn't touch it.
        /// </summary>
        public string SbpMemberId { get; set; } = "100000000111";
        public string OrderNumber { get; set; }
        public DateTime OrderCreateDate { get; set; }

        public List<OrderParamItem> OrderParamsType { get; set; }
        public string IdQr { get; set; }
        
        /// <summary>
        /// Сумма указывается в минимальных единицах валюты. Например, в копейках для рубля.
        /// </summary>
        public int OrderSum { get; set; }
        public string Currency { get; set; }
        public string Description { get; set; }
    }
    
    public class OrderParamItem
    {
        public string PositionName { get; set; }
        public int PositionCount { get; set; }
        public int PositionSum { get; set; }
        public string PositionDescription { get; set; }
    }
}
