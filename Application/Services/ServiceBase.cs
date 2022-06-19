using System;
using AutoMapper;

namespace Monitoring.Application.Services
{
    public class ServiceBase
    {
        public IMapper Mapper { get; private set; }

        public ServiceBase()
        {
            var mapperConfig = new MapperConfiguration(cfg =>
            {

            });
            this.Mapper = mapperConfig.CreateMapper();

        }
    }
}

