using Monitoring.Application.Interfaces;
using Domain.DTO;
using Domain.Interfaces.Repositories;

namespace Monitoring.Application.Services
{
    public class MonitoringAppService : ServiceBase, IMonitoringAppService
    {
        private readonly IMonitoringRepository _monitoringRepository;

        public MonitoringAppService(IMonitoringRepository MonitoringRepository)
		{
            _monitoringRepository = MonitoringRepository;
        }

        public async Task<IEnumerable<CovidCaseDto>> GetCasesByCityCodeAsync(string city, bool useCache)
        {
            var result = await _monitoringRepository.GetCasesByCityAsync(city, useCache);
            if (result == null || !result.Any())
            {
                return new List<CovidCaseDto>();
            }
            return result;
        }

        public async Task<IEnumerable<IbgeCityDto>> GetCitiesIbgeAsync(bool useCache)
        {
            return await _monitoringRepository.GetCitiesIbgeAsync(useCache);
        }
    }
}

