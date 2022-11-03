using System;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Serialization;

namespace SBPLibrary.Models.Sber
{
    public class CancelResponse
    {
        public string rq_uid { get; set; }
        public DateTime rq_tm { get; set; }
        public string order_id { get; set; }
        
        [JsonConverter(typeof(StringEnumConverter), converterParameters:typeof(CamelCaseNamingStrategy))]
        public OrderStatus order_status { get; set; }
        public string operation_id { get; set; }
        public DateTime operation_date_time { get; set; }
        public string operation_type { get; set; }
        public string operation_sum { get; set; }
        public string operation_currency { get; set; }
        public string auth_code { get; set; }
        public string rrn { get; set; }
        public string tid { get; set; }
        public string id_qr { get; set; }
        public string error_code { get; set; }
        
        /// <summary>
        /// Описание некоторых случаев:
        /// AMOUNT_TOO_LARGE: Возврат уже произведен, либо указана сумма, превышащая оплаченную.
        /// ORIGINAL_ORDER_NO_FOUND: Неверный orderId
        /// </summary>
        public string error_description { get; set; }
        public SbpOperationParams SbpOperationParams { get; set; }

    }

    public class SbpOperationParams
    {
        public string sbp_cancel_operation_id;
        public string sbp_merchant_name;
    }
}