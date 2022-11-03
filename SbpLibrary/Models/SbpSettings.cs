using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SBPLibrary.Models
{
    public class SbpSettings
    {
        public string ClientId { get; set; }
        public string ClientSecret { get; set; }
        public string TokenBasePath { get; set; }
        public string BasePath { get; set; }
        public string Currency { get; set; }
        public string TerminalId { get; set; }
        public string MemberId { get; set; }
        public string CertPath { get; set; }
        public string CertPassword { get; set; }
        public int PaymentTimeoutSeconds { get; set; }
    }
}
