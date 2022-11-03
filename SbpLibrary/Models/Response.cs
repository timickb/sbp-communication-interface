namespace SBPLibrary.Models
{
    public class Response
    {
        public bool IsError { get; set; } = false;
        public string ErrMessage { get; set; } = "";
        public string QrUrl { get; set; } = "";
        public string OrderId { get; set; } = "";
        public string OrderStatus { get; set; } = "";
    }
}
