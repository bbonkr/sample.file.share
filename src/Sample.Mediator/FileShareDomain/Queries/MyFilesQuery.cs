using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

using kr.bbon.EntityFrameworkCore.Extensions;

using MediatR;

using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

using Sample.Data;

namespace Sample.Mediator.FileShareDomain.Queries
{
    public class MyFilesQuery : IRequest<IPagedModel<FileItemModel>>
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

    

    public class FileItemModel
    {
        public string Id { get; set; }

        public string Name { get; set; }

        public string ContentType { get; set; }

        public long Size { get; set; }

        public string Uri { get; set; }

        public long CreatedAt { get; set; } 
    }

    public class MyFilesQueryHandler : IRequestHandler<MyFilesQuery, IPagedModel<FileItemModel>>
    {
        public MyFilesQueryHandler(
            DefaultDbContext dbContext, 
            ILogger<MyFilesQueryHandler> logger)
        {
            this.dbContext = dbContext;
            this.logger = logger;
        }

        public async Task<IPagedModel<FileItemModel>> Handle(MyFilesQuery request, CancellationToken cancellationToken)
        {

            var user = await dbContext.Users
                .Where(x => x.Email == request.UserImpersonate)
                .FirstOrDefaultAsync(cancellationToken);

            if (user == null)
            {
                throw new Exception("Access deny");
            }

            var query = dbContext.Files
                 .Where(x => x.CreatedBy == user.Id);

            if (!string.IsNullOrWhiteSpace(request.Keyword))
            {
                query = query.Where(x => EF.Functions.Like(x.Name, $"%{request.Keyword}%"));
            }

            var result = await query.OrderBy(x => x.Name)
                 .Select(x => new FileItemModel
                 {
                     Id = x.Id.ToString(),
                     ContentType = x.ContentType,
                     Name = x.Name,
                     Size = x.Size,
                     Uri = x.Uri.ToString(),
                     CreatedAt = x.CreatedAt.Ticks,
                 })
                 .ToPagedModelAsync(request.Page, request.Limit, cancellationToken);

            return result;
        }


        private readonly DefaultDbContext dbContext;
        
        private readonly ILogger logger;
    }
}
