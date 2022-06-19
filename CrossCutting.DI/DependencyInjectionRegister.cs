using Monitoring.Application.Interfaces;
using Monitoring.Application.Services;
using Data.Repositories;
using Domain.Interfaces.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Monitoring.CrossCutting;
using static Monitoring.CrossCutting.Configuration.ConfigurationEnum;

namespace CrossCutting.DI
{
    public class DependencyInjectionRegister
	{

		public static void RegisterServices(IServiceCollection services)
		{
			services.AddScoped<IMonitoringAppService, MonitoringAppService>();
			services.AddScoped<IMonitoringRepository, MonitoramentoRepository>();
			services.AddDbContext<DbContext>(options => options.UseMySql(AppConfiguration.GetConfiguration(ConfigurationName.ConnectionStringDataBase)));
		}
	}
}

