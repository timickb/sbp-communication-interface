using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Serialization;

namespace SBPLibrary.Models.Sber.Response
{
    public class OrderStatusResponse
    {
        public string rq_uid { get; set; }
        public string rq_tm { get; set; }
        public string mid { get; set; }
        public string tid { get; set; }
        public string id_qr { get; set; }
        public string order_id { get; set; }

        [JsonConverter(typeof(StringEnumConverter), converterParameters:typeof(CamelCaseNamingStrategy))]
        public OrderStatus order_state { get; set; }

        public string error_code { get; set; }

        [JsonProperty(Required = Required.Default)]
        public List<OrderOperationParamsStatus> order_operation_params { get; set; } =
            new List<OrderOperationParamsStatus>();
    }

    public class OrderOperationParamsStatus
    {
        public string operation_id { get; set; }
        public string operation_type { get; set; }
        public DateTime operation_date_time { get; set; }
        public string rrn { get; set; }
        public int operation_sum { get; set; }
        public string operation_currency { get; set; }
        public string auth_code { get; set; }
        public string response_code { get; set; }
        public string response_desc { get; set; }
        public string client_name { get; set; }
    }
}
