using System;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Serialization;

namespace SBPLibrary.Models.Sber
{
    public class RevokeResponse
    {
        public string rq_uid { get; set; }
        public DateTime rq_tm { get; set; }
        public string order_id { get; set; }
        
        [JsonConverter(typeof(StringEnumConverter), converterParameters:typeof(CamelCaseNamingStrategy))]
        public OrderStatus order_state { get; set; }
        public string error_code { get; set; }
        public string error_description { get; set; }
    }
}
