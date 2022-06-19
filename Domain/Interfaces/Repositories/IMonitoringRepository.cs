using System;
using Domain.DTO;

namespace Domain.Interfaces.Repositories
{
	public interface IMonitoringRepository
	{
		Task<IEnumerable<CovidCaseDto>> GetCasesByCityAsync(string cidade, bool useCache);
		Task<IEnumerable<IbgeCityDto>> GetCitiesIbgeAsync(bool useCache);
		
	}
}

