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


        /// <summary>
        /// Add sample azure storage blob service dependency
        /// <para>
        /// See <see cref="AddSampleAzureBlobStorageService"/> method how to use.
        /// </para>
        /// </summary>
        /// <typeparam name="TAzureBlobStorageService"></typeparam>
        /// <param name="services"></param>
        /// <returns></returns>
        public static IServiceCollection AddAzureBlobStorageService<TAzureBlobStorageContainer>(this IServiceCollection services) where TAzureBlobStorageContainer : AzureBlobStorageContainerBase
        {
            services.AddTransient<TAzureBlobStorageContainer>();
            services.AddTransient<AzureBlobStorageService<TAzureBlobStorageContainer>>();

            return services;
        }

        /// <summary>
        /// Add sample azure storage blob service dependency for 'file-share-sample' container
        /// </summary>
        /// <param name="services"></param>
        /// <returns></returns>
        public static IServiceCollection AddSampleAzureBlobStorageService(this IServiceCollection services)
        {
            services.AddTransient<AzureBlobStorageService<SampleAzureBlobStorageContainer>>();

            return services;
        }
    }
}
