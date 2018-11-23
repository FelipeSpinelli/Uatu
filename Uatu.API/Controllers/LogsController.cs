using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;
using Uatu.API.Models.Request;
using Uatu.Core.Entities;
using Uatu.Core.Interfaces;
using Uatu.Core.Seedwork;

namespace Uatu.API.Controllers
{
    [Route("api/logs")]
    [ApiController]
    public class LogsController : ControllerBase
    {
        private readonly IDbConnector _dbConnector;

        public LogsController(IDbConnector dbConnector)
        {
            _dbConnector = dbConnector;
        }

        [HttpGet]
        public async Task<IActionResult> Get([FromQuery]SearchLogRequestViewModel requestQuery)
        {
            try
            {                
                if (!ModelState.IsValid)
                    return BadRequest(requestQuery);

                var result = await _dbConnector.Read(requestQuery.Application, requestQuery.Message, requestQuery.RequestKey, requestQuery.Level, requestQuery.Since.Value, requestQuery.Until.Value);
                return Ok();
            }
            catch
            {
                throw;
            }
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] CreateLogRequestViewModel requestBody)
        {
            try
            {
                Result<Log> result = Log.Create(requestBody.Application, requestBody.Message, (ELogLevel)Enum.Parse(typeof(ELogLevel), requestBody.Level), Guid.Parse(requestBody.RequestKey), requestBody.Metadata);

                if (!result.IsSuccess)
                    return BadRequest(result);

                await _dbConnector.Write(result.Content);
                return Ok();
            }
            catch
            {
                throw;
            }
        }
    }
}
