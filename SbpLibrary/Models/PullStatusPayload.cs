using System.Threading;

namespace SBPLibrary.Models
{
    public class PullStatusPayload
    {
        public string OrderId { get; set; }
        public string OrderNumber { get; set; }
        public CancellationToken Token { get; set; }
    }
}