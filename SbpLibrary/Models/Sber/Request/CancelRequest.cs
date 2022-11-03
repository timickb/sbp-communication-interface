using System;

namespace SBPLibrary.Models.Sber
{
    public class CancelRequest
    {
        public string RqUid { get; set; }
        public DateTime RqTm { get; set; }
        public string OrderId { get; set; }
        public string OperationType { get; set; }
        public string OperationId { get; set; }
        
        // TODO: Выяснить, для чего нужен этот параметр
        public string AuthCode { get; set; } = "000000";
        public string Tid { get; set; }
        public string IdQr { get; set; }
        
        /// <summary>
        /// Сумма указывается в минимальных единицах валюты. Например, в копейках для рубля.
        /// </summary>
        public int CancelOperationSum { get; set; }
        public string OperationCurrency { get; set; }
    }
}