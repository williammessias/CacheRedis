using CrossCutting.DI;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;
using Monitoring.CrossCutting;
using Monitoring.CrossCutting.Configuration;
using KissLog;
using KissLog.AspNetCore;
using KissLog.CloudListeners.Auth;
using KissLog.CloudListeners.RequestLogsListener;
using System;
using KissLog.Formatters;

namespace Services.API
{
    public class Startup
	{
		public Startup(IConfiguration configuration)
		{
			Configuration = configuration;
		}
		public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddHttpContextAccessor();

            services.AddScoped<IKLogger>((provider) => Logger.Factory.Get());

            services.AddLogging(logging =>
            {
                logging.AddKissLog(options =>
                {
                    options.Formatter = (FormatterArgs args) =>
                    {
                        if (args.Exception == null)
                            return args.DefaultValue;

                        string exceptionStr = new ExceptionFormatter().Format(args.Exception, args.Logger);

                        return string.Join(Environment.NewLine, new[] { args.DefaultValue, exceptionStr });
                    };
                });
            });

            services.AddDistributedRedisCache(options =>
            {
                options.Configuration = AppConfiguration.GetConfiguration(ConfigurationEnum.ConfigurationName.ConnectionStringCache);
                options.InstanceName = "Covid";
            });


            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo
                {
                    Version = "v1",
                    Title = "Monitoramento de Covid",
                    Description = "API para monitoramento de casos de covid",
                    
                });
            });

            services.AddMvc(options => options.EnableEndpointRouting = false);
            InitializeContainer(services);

        }

        public void Configure(IApplicationBuilder app)
        {
            app.UseMvc();

            app.UseRouting();

            app.UseKissLogMiddleware(options => ConfigureKissLog(options));
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapDefaultControllerRoute();
                endpoints.MapControllerRoute("default", "{controller=Home}/{action=Index}/{id?}");
            });

            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json",
                    "V1");
            });

        }

        private void ConfigureKissLog(IOptionsBuilder options)
        {
            KissLogConfiguration.Listeners
                .Add(new RequestLogsApiListener(new Application(AppConfiguration.GetConfiguration(ConfigurationEnum.ConfigurationName.OrganizationId), AppConfiguration.GetConfiguration(ConfigurationEnum.ConfigurationName.ApplicationId)))
                    {
                        ApiUrl = AppConfiguration.GetConfiguration(ConfigurationEnum.ConfigurationName.KissUrl)
                    }); 
        }

            private void InitializeContainer(IServiceCollection services)
        {
            DependencyInjectionRegister.RegisterServices(services);
        }

    }
}

