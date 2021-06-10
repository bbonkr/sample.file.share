using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

using MediatR;

using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

using Sample.Data;
using Sample.Services;

namespace Sample.Mediator.FileShareDomain.Queries
{

    public class FileByTokenQueryHandler : IRequestHandler<FileByTokenQuery, FileByTokenQueryResult>
    {
        public FileByTokenQueryHandler(
            DefaultDbContext dbContext, 
            AzureBlobStorageService<SampleAzureBlobStorageContainer> azureBlobStorageService,
            ILogger<FileByTokenQueryHandler> logger)
        {
            this.dbContext = dbContext;
            this.azureBlobStorageService = azureBlobStorageService;
            this.logger = logger;
        }

        public async Task<FileByTokenQueryResult> Handle(FileByTokenQuery request, CancellationToken cancellationToken)
        {
            var user = await dbContext.Users
                .Where(x => x.Id == request.UserId)
                .AsNoTracking()
                .FirstOrDefaultAsync(cancellationToken);

            if(user == null)
            {
                throw new Exception("Access deny");
            }

            var file = await dbContext.Access
                .Include(x => x.File)
                .Where(x => x.UserId == request.UserId)
                .Where(x => x.Token == request.FileToken)
                .Where(x => !x.ExpiresOn.HasValue || (x.ExpiresOn.HasValue && x.ExpiresOn.Value > DateTimeOffset.UtcNow))
                .Select(x => x.File)
                .AsNoTracking()
                .FirstOrDefaultAsync(cancellationToken);
                
            if(file == null)
            {
                throw new Exception("Link has been expired");
            }

            var blobStream = await azureBlobStorageService.DownloadAsync(file.Name);
            byte[] buffer = null;
            using (var outStream = new MemoryStream())
            {
                await blobStream.CopyToAsync(outStream, cancellationToken);

                buffer = outStream.ToArray();
            }

            return new FileByTokenQueryResult
            {
                ContentType = file.ContentType,
                FileName = file.Name,
                FileSize = file.Size,
                Buffer = buffer,
            };
        }

        private readonly DefaultDbContext dbContext;
        AzureBlobStorageService<SampleAzureBlobStorageContainer> azureBlobStorageService;
        private readonly ILogger logger;
    }
}
