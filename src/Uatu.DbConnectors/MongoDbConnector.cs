using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Uatu.Core.Entities;
using Uatu.Core.Interfaces;
using Uatu.Core.Seedwork;

namespace Uatu.DbConnectors
{
    public class MongoDbConnector : IDbConnector
    {
        private readonly MongoClient _mongoClient;
        private readonly IMongoDatabase _database;
        private readonly IMongoCollection<Log> _collection;

        public MongoDbConnector(string url, string databaseName = "Uatu")
        {
            var settings = MongoClientSettings.FromUrl(new MongoUrl(url));
            settings.MaxConnectionIdleTime = TimeSpan.FromSeconds(5);

            _mongoClient = new MongoClient(settings);
            _database = _mongoClient.GetDatabase(databaseName);
            _collection = _database.GetCollection<Log>(nameof(Log));
        }

        private string GetQuery(string application, string message, string requestKey, string level, DateTime since, DateTime until)
        {
            var filters = new List<string>();
            if (!string.IsNullOrEmpty(application))
                filters.Add($"Application : '{application}'");
            if (!string.IsNullOrEmpty(message))
                filters.Add($"Message : /{message}/");
            if (!string.IsNullOrEmpty(requestKey))
                filters.Add($"RequestKey : '{requestKey.Replace("-", "")}'");
            if (!string.IsNullOrEmpty(level))
                filters.Add($"Level : '{level}'");

            filters.Add($"Timestamp : {{ $gte: ISODate(\"{since.ToString("yyyy-MM-ddTHH:mm:ss.fffZ")}\"), $lte: ISODate(\"{until.ToString("yyyy-MM-ddTHH:mm:ss.fffZ")}\") }}");

            var query = $"{{ {string.Join(',', filters)} }}";
            return query;
        }

        public async Task<Result<IEnumerable<Log>>> Read(string application, string message, string requestKey, string level, DateTime since, DateTime until)
        {
            return await Task<Result<IEnumerable<Log>>>.Factory.StartNew(() =>
            {
                var result = new Result<IEnumerable<Log>>();
                try
                {
                    var query = GetQuery(application, message, requestKey, level, since, until);
                    var items = _collection.FindAsync(query).Result.ToList();
                    result.SetContent(items as IEnumerable<Log>);
                }
                catch (Exception ex)
                {
                    result.AddError(ex.Message);
                }
                return result;
            });
        }

        public async Task<Result> Write(Log log)
        {
            return await Task<Result>.Factory.StartNew(() =>
            {
                var result = new Result();
                try
                {                    
                    _collection.InsertOneAsync(log);
                }
                catch (Exception ex)
                {
                    result.AddError(ex.Message);
                }
                return result;
            });
        }
    }
}
