using System;
using Domain.DTO;

namespace Monitoring.Application.Interfaces
{
	public interface IMonitoringAppService
	{
		Task<IEnumerable<CovidCaseDto>> GetCasesByCityCodeAsync(string cidade, bool useCache);
		Task<IEnumerable<IbgeCityDto>> GetCitiesIbgeAsync(bool useCache);
	}
}

