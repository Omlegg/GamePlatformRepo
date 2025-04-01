using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GamePlatformRepo.Models
{
    public class EntryLog
    {
        public int RequestId { get; set; }
        public string Url { get; set; }
        public string RequestBody { get; set; }
        public string RequestHeaders { get; set; }
        public string MethodType { get; set; }
        public string ResponseBody { get; set; }
        public string ResponseHeaders { get; set; }
        public int StatusCode { get; set; }
        public DateTime CreationDateTime { get; set; } = DateTime.UtcNow;
        public DateTime? EndDateTime { get; set; }
        public string ClientIp { get; set; }
    }
}