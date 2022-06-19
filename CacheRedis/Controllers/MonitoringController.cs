using System.Collections.Generic;
using System.Threading.Tasks;
using Domain.DTO;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using Microsoft.Extensions.Logging;
using Monitoring.Application.Interfaces;
using KissLog;

namespace Services.API.Controllers
{
    [Produces("application/json")]
    [Route("api/[controller]")]
    public class MonitoringController : Controller
    {
        private readonly IMonitoringAppService _monitoringAppService;

        public MonitoringController(IMonitoringAppService monitoringAppService)
        {
            _monitoringAppService = monitoringAppService;
        }

        [Route("cases/city/{id}"), HttpGet]
        [SwaggerResponse(StatusCodes.Status412PreconditionFailed, Type = typeof(IEnumerable<CovidCaseDto>))]
        [SwaggerResponse(StatusCodes.Status404NotFound)]
        [SwaggerResponse(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetCasesByCityCodeAsync(string id, [FromQuery] bool useCache)
        { 
            var logger = new Logger();
            logger.Trace("Request on Get Cases By City Code was received");
            Logger.NotifyListeners(logger);
            return new JsonResult(await _monitoringAppService.GetCasesByCityCodeAsync(id, useCache));
        }

        [Route("cities"), HttpGet]
        [SwaggerResponse(StatusCodes.Status200OK, Type = typeof(IEnumerable<IbgeCityDto>))]
        [SwaggerResponse(StatusCodes.Status404NotFound)]
        [SwaggerResponse(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetCitiesIbgeAsync([FromQuery] bool useCache)
        {
            var logger = new Logger();
            logger.Trace("Request on Get Cities was received");
            Logger.NotifyListeners(logger);
            return new JsonResult(await _monitoringAppService.GetCitiesIbgeAsync(useCache));
        }
    }
}
