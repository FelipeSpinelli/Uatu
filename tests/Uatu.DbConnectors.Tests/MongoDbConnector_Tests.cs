using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Uatu.Core.Entities;
using Uatu.Core.Seedwork;
using Xunit;

namespace Uatu.DbConnectors.Tests
{
    public class MongoDbConnector_Tests
    {
        [Fact]
        public async Task Write_Test_Success()
        {
            var mongoDbConnector = new MongoDbConnector("mongodb://localhost:27017");
            Result<Log> log = Log.Create(
                "Test", 
                "Log test successfully", 
                ELogLevel.Debug, 
                Guid.NewGuid(), 
                new Dictionary<string, object>
                {
                    { "Key1", "Value1" },
                    { "Key2", "Value2" },
                }
            );

            var result = await mongoDbConnector.Write(log.Content);
            Assert.True(result.IsSuccess);
        }
    }
}
