using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

using Sample.Services;

namespace Microsoft.Extensions.DependencyInjection { 
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection ConfigureAzureStorageAccountOptions(this IServiceCollection services, IConfiguration configuration)
        {
            services.Configure<AzureStorageAccountOptions>(options =>
            {
                options.ConnectionString = configuration.GetConnectionString("AzureStorageAccount");
            });

            return services;
        }
    }
}
