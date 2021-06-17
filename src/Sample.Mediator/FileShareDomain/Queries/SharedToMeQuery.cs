using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

using kr.bbon.Core;
using kr.bbon.EntityFrameworkCore.Extensions;

using MediatR;

using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

using Sample.Data;
using Sample.Mediator.FileShareDomain.Models;
using Sample.Services;

namespace Sample.Mediator.FileShareDomain.Queries
{
    public class SharedToMeQuery : IRequest<IPagedModel<SharedFileModel>>
    {
        /// <summary>
        /// User authentication
        /// 
        /// <para>Use your email address Temporarily. ;)</para>
        /// </summary>
        public string UserImpersonate { get; set; }

        public int Page { get; set; }

        public int Limit { get; set; }

        public string Keyword { get; set; }
    }

    public class SharedToMeQueryHandler : IRequestHandler<SharedToMeQuery, IPagedModel<SharedFileModel>>
    {
        public SharedToMeQueryHandler(
            DefaultDbContext dbContext,
            DateTimeConvertService dateTimeConvertService,
            ILogger<SharedToMeQueryHandler> logger)
        {
            this.dbContext = dbContext;
            this.dateTimeConvertService = dateTimeConvertService;
            this.logger = logger;
        }

        public async Task<IPagedModel<SharedFileModel>> Handle(SharedToMeQuery request, CancellationToken cancellationToken)
        {
            var user = await dbContext.Users
                .Where(x => x.Email == request.UserImpersonate)
                .AsNoTracking()
                .FirstOrDefaultAsync(cancellationToken);

            if (user == null)
            {
                throw new HttpStatusException<object>(System.Net.HttpStatusCode.Unauthorized, "Access deny", null);
            }

            var result = await dbContext.Access
                .Include(x => x.File)
                .Where(x => x.UserId == user.Id)
                .WhereDependOn(
                    !string.IsNullOrEmpty(request.Keyword?.Trim()),
                    x => EF.Functions.Like(x.File.Name, $"%{request.Keyword.Trim()}%"))
                .OrderBy(x => x.File.Name).ThenBy(x => x.ExpiresOn)
                .Select(x => new SharedFileModel
                {
                    Id = x.Id.ToString(),
                    Name = x.File.Name,
                    ContentType = x.File.ContentType,
                    ExpiresOn = (long?)dateTimeConvertService.ToJavascriptDate(x.ExpiresOn),
                    Size = x.File.Size,
                    Token = x.Token,
                })
                .ToPagedModelAsync(request.Page, request.Limit, cancellationToken);

            return result;
        }

        private readonly DefaultDbContext dbContext;
        private readonly DateTimeConvertService dateTimeConvertService;
        private readonly ILogger logger;
    }
}
