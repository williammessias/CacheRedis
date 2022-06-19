using Dapper;
using Domain.DTO;
using Domain.Exceptions;
using Domain.Interfaces.Repositories;
using KissLog;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace Data.Repositories
{
	public class MonitoramentoRepository : ConnectionBase, IMonitoringRepository
	{

        private readonly IDistributedCache _cache;
        private const string KEY_GET_CASES = "Get_Cases";
        private const string KEY_ALL_CITIES = "All_Cities";
        private readonly ILogger<MonitoramentoRepository> _logger;

        public MonitoramentoRepository(IDistributedCache cache, ILogger<MonitoramentoRepository> logger)
        {
            _cache = cache;
            _logger = logger;
        }

        public async Task<IEnumerable<IbgeCityDto>> GetCitiesIbgeAsync(bool useCache)
        {
            var dataCache = "";

            var logger = new Logger();
            if (useCache)
            {
                dataCache = await _cache.GetStringAsync(KEY_ALL_CITIES);
            }

            if (string.IsNullOrWhiteSpace(dataCache))
            { 
                var cacheSettings = new DistributedCacheEntryOptions();
                cacheSettings.SetAbsoluteExpiration(TimeSpan.FromMinutes(2));

                var sql = @"select distinct city_ibge_code, city from Monitoramento.casos_covid where city != ''";

                    try
                    {
                        using (var conexao = this.Connection)
                        {
                            var cidadesIbge =  conexao.Query<dynamic>(sql);

                            var itemsJson = JsonConvert.SerializeObject(cidadesIbge);

                            await _cache.SetStringAsync(KEY_ALL_CITIES, itemsJson, cacheSettings);

                            logger.Trace("Get Cities - database was used");
                            Logger.NotifyListeners(logger);

                        return Mapper.Map<IEnumerable<IbgeCityDto>>(cidadesIbge);
                        }
                    }
                    catch (Exception e)
                    {
                        throw new ObterCidadeIbgeException(e.Message);
                    }
            }
            logger.Trace("RequestGet Cities - cache redis was used");
            Logger.NotifyListeners(logger);

            var items = JsonConvert.DeserializeObject(dataCache);
            return Mapper.Map<IEnumerable<IbgeCityDto>>(items);
        }

        public async Task<IEnumerable<CovidCaseDto>> GetCasesByCityAsync(string codigoIbgeCidade, bool useCache)
        {
            var logger = new Logger();
            var dataCache = "";

            if (useCache)
            {
                dataCache = await _cache.GetStringAsync(KEY_GET_CASES + codigoIbgeCidade);
            }

            if (string.IsNullOrWhiteSpace(dataCache))
            {
                var cacheSettings = new DistributedCacheEntryOptions();
                cacheSettings.SetAbsoluteExpiration(TimeSpan.FromMinutes(2));

                var sql = @"select distinct
                    city,
                    city_ibge_code,
                    date,
                    epidemiological_week,
                    estimated_population_2019,
                    is_last,
                    is_repeated,
                    last_available_confirmed,
                    last_available_confirmed_per_100k_inhabitants,
                    last_available_date,
                    last_available_death_rate,
                    last_available_deaths,
                    order_for_place,
                    place_type,
                    state,
                    new_confirmed,
                    new_deaths 
                    from Monitoramento.casos_covid where city_ibge_code = @codigoIbgeCidade";

                try
                {
                    using (var conexao = this.Connection)
                    {
                        var param = new Dictionary<string, object>();
                        param.Add(@"codigoIbgeCidade", codigoIbgeCidade);
                        var casosPorCidade = this.Get(sql, param);

                        var itemsJson = JsonConvert.SerializeObject(casosPorCidade);

                        _cache.SetString(KEY_GET_CASES + codigoIbgeCidade, itemsJson);
                        logger.Trace("Get cases - database was used");
                        Logger.NotifyListeners(logger);

                        return Mapper.Map<IEnumerable<CovidCaseDto>>(casosPorCidade); ;
                    }
                }
                catch (Exception e)
                {
                    throw new GetCasesByCityException(e.Message);
                }
            }
            _logger.LogTrace("Get cases - cache redis was used");

            var items = JsonConvert.DeserializeObject(dataCache);
            return Mapper.Map<IEnumerable<CovidCaseDto>>(items);
        }
         
    }
}

