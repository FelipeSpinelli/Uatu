using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Uatu.Core.Entities;
using Uatu.Core.Seedwork;

namespace Uatu.Core.Interfaces
{
    public interface IDbConnector
    {
        Task<Result> Write(Log log);
        Task<Result<IEnumerable<Log>>> Read(string application, string message, string requestKey, string level, DateTime since, DateTime until);
    }
}
