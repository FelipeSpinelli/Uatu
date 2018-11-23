using System.Collections.Generic;

namespace Uatu.API.Models.Request
{
    public class CreateLogRequestViewModel
    {
        public string Application { get; set; }
        public string Message { get; set; }
        public string Level { get; set; }
        public string RequestKey { get; set; }
        public IDictionary<string, object> Metadata { get; set; }
    }
}
