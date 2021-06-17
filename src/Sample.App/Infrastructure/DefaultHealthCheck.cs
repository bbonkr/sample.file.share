using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

using kr.bbon.AspNetCore;

using Microsoft.Extensions.Diagnostics.HealthChecks;

using Sample.Data;

namespace Sample.App
{
    public class DefaultHealthCheck : HealthCheckBase
    {
        public DefaultHealthCheck(DefaultDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public override Task<HealthCheckResult> CheckAsync(CancellationToken cancellationToken = default)
        {
            try
            {
                var exists = dbContext.Users.Any();
            }
            catch (Exception ex)
            {
                return Task.FromResult(HealthCheckResult.Unhealthy("Api is sick. :("));
            }

            return Task.FromResult(HealthCheckResult.Healthy("Api is running. :)"));
        }

        private readonly DefaultDbContext dbContext;
    }
}
