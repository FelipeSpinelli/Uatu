using System;
using System.Collections.Generic;
using Uatu.Core.Seedwork;

namespace Uatu.Core.Entities
{
    public class Log
    {
        public string Id { get; protected set; }
        public string Application { get; protected set; }
        public string Message { get; protected set; }
        public ELogLevel Level { get; protected set; }
        public string RequestKey { get; protected set; }
        public IDictionary<string, object> Metadata { get; protected set; }
        public DateTime Timestamp { get; set; }

        private Log(string application, string message, ELogLevel level, string requestKey, IDictionary<string,object> metadata)
        {
            Id = Guid.NewGuid().ToString("N");
            Application = application;
            Message = message;
            Level = level;
            RequestKey = requestKey;
            Metadata = metadata;
            Timestamp = DateTime.Now;
        }

        public static Result<Log> Create(string application, string message, ELogLevel level, Guid requestKey, IDictionary<string, object> metadata)
        {
            var result = new Result<Log>();

            if (string.IsNullOrEmpty(application))
                result.AddError("Application cannot be null or empty!");

            if (string.IsNullOrEmpty(message))
                result.AddError("Message cannot be null or empty!");

            if (!result.IsSuccess)
                return result;

            if (requestKey == null)
                requestKey = Guid.NewGuid();

            result.SetContent(new Log(application, message, level, requestKey.ToString("N"), metadata));

            return result;
        }
    }
}
