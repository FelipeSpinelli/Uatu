using System;
using System.Collections.Generic;

namespace Uatu.API.Models.Response
{
    public class SearchLogResponseViewModel
    {
        public string Id { get; set; }
        public string Application { get; set; }
        public string Message { get; set; }
        public string Level { get; set; }
        public string RequestKey { get; set; }
        public IDictionary<string,object> Metadata { get; set; }
        public DateTime Timestamp { get; set; }
    }
}
