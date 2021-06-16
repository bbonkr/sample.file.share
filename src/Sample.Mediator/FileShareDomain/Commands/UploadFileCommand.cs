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
using Sample.Entities;
using Sample.Mediator.FileShareDomain.Models;
using Sample.Services;

namespace Sample.Mediator.FileShareDomain.Commands
{
    public class UploadFileCommand : IRequest<FileItemModel>
    {
        /// <summary>
        /// User authentication
        /// 
        /// <para>Use your email address Temporarily. ;)</para>
        /// </summary>
        public string UserImpersonate { get; set; }

        public string Name { get; set; }

        public string ContentType { get; set; }

        public long Size { get; set; }

        public Stream Stream { get; set; }
    }

    public class UploadFileCommandHander : IRequestHandler<UploadFileCommand, FileItemModel>
    {
        public UploadFileCommandHander(
            AzureBlobStorageService<SampleAzureBlobStorageContainer> azureBlobStorageService,
            DefaultDbContext dbContext,
            ILogger<UploadFileCommandHander> logger)
        {
            this.dbContext = dbContext;
            this.azureBlobStorageService = azureBlobStorageService;
            this.logger = logger;
        }

        public async Task<FileItemModel> Handle(UploadFileCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var now = DateTimeOffset.UtcNow;

                var user = await dbContext.Users
                    .Where(x => x.Email == request.UserImpersonate)
                    .FirstOrDefaultAsync(cancellationToken);

                if(user == null)
                {
                    throw new Exception("Access deny");
                }

                var fileName = $"{user.Id}/{now:yyyy}/{now:MM}/{request.Name}";

                var result = await azureBlobStorageService.UploadAsync(request.Name, request.Stream, request.ContentType, cancellationToken);
                
                logger.LogInformation($"Upload to azure blob storage: origin file name: {request.Name}, uploaded file name: {result.BlobName} uri: {result.Uri}");

                var fileInformation = new FileInformation
                {
                    Name = result.BlobName,
                    Size = request.Stream.Length,
                    ContentType = request.ContentType,
                    Uri = result.Uri.ToString(),
                    CreatedAt = DateTimeOffset.UtcNow,
                    CreatedBy = user.Id,
                };

               var added= dbContext.Files.Add(fileInformation);
                await dbContext.SaveChangesAsync(cancellationToken);

                return new FileItemModel
                {
                    Id = added.Entity.Id.ToString(),
                    ContentType = added.Entity.ContentType,
                    Name = added.Entity.Name,
                    Uri = added.Entity.Uri,
                    Size = added.Entity.Size,
                    CreatedAt = added.Entity.CreatedAt.Ticks,
                };
               
            }
            catch (Exception ex)
            {
                logger.LogError(ex.Message, ex);
                throw;
            }
        }

        private readonly AzureBlobStorageService<SampleAzureBlobStorageContainer> azureBlobStorageService;
        private readonly DefaultDbContext dbContext;
        private readonly ILogger logger;
    }
}
