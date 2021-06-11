using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using MediatR;

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

using Sample.Mediator;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddDomainServices(this IServiceCollection services, IConfiguration configuration)
        {
            services.ConfigureAzureStorageAccountOptions(configuration);

            services.AddSampleAzureBlobStorageService();

            services.AddMediatR(typeof(PlaceHolder).Assembly);

            return services;
        }
    }
}
