using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Serialization;

namespace SBPLibrary.Models.Sber
{
    public class CreatePaymentResponse
    {
        public string rq_uid { get; set; }
        public string rq_tm { get; set; }
        public string order_number { get; set; }
        public string order_id { get; set; }

        [JsonConverter(typeof(StringEnumConverter), converterParameters:typeof(CamelCaseNamingStrategy))]
        public OrderStatus order_state { get; set; }
        public string order_form_url { get; set; }
        public string error_code { get; set; }
        public string error_description { get; set; }

    }
}
