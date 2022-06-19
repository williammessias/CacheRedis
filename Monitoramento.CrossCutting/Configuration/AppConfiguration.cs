using System;
using Microsoft.Extensions.Configuration;
using System.IO;
using static Monitoring.CrossCutting.Configuration.ConfigurationEnum;

namespace Monitoring.CrossCutting
{
    public static class AppConfiguration
    {
        public static string GetConfiguration(ConfigurationName configName)
        {
            var config = CreateConfig();

            switch (configName)
            {
                case (ConfigurationName.ConnectionStringDataBase):
                    return config.GetConnectionString("ConnectionStringDataBase");
                case (ConfigurationName.ConnectionStringCache):
                    return config.GetConnectionString("ConnectionStringCache");
                case (ConfigurationName.OrganizationId):
                    return config.GetSection("KissLog")["OrganizationId"];
                case (ConfigurationName.ApplicationId):
                    return config.GetSection("KissLog")["ApplicationId"];
                case (ConfigurationName.KissUrl):
                    return config.GetSection("KissLog")["KissUrl"];
                default:
                    throw new Exception("Configuração solicitada não existe");
            }

        }

        private static IConfigurationRoot CreateConfig()
        {
            var config = new ConfigurationBuilder()
                           .SetBasePath(Directory.GetCurrentDirectory())
                           .AddJsonFile("appsettings.json")
                           .Build();

            return config;
        }
    }
}

