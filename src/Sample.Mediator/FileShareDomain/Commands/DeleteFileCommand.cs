using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

using MediatR;

using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

using Sample.Data;
using Sample.Services;

namespace Sample.Mediator.FileShareDomain.Commands
{
    public class DeleteFileCommand: IRequest
    {
        /// <summary>
        /// User authentication
        /// 
        /// <para>Use your email address Temporarily. ;)</para>
        /// </summary>
        public string UserImpersonate { get; set; }

        public Guid FileId { get; set; }
    }

    public class DeleteFileCommandHandler : IRequestHandler<DeleteFileCommand>
    {
        public DeleteFileCommandHandler(
            AzureBlobStorageService<SampleAzureBlobStorageContainer> azureBlobStorageService,
            DefaultDbContext dbContext,
            ILogger<UploadFileCommandHander> logger)
        {
            this.dbContext = dbContext;
            this.azureBlobStorageService = azureBlobStorageService;
            this.logger = logger;
        }

        public async Task<Unit> Handle(DeleteFileCommand request, CancellationToken cancellationToken)
        {
            var user = await dbContext.Users
                .Where(x => x.Email == request.UserImpersonate)
                .FirstOrDefaultAsync(cancellationToken);

            if (user == null)
            {
                throw new Exception("Access deny");
            }

            using (var transaction = dbContext.Database.BeginTransaction())
            {
                try
                {
                    var willDelete = await dbContext.Files
                        .Where(x => x.Id == request.FileId && x.CreatedBy == user.Id)
                        .FirstOrDefaultAsync(cancellationToken);

                    if (willDelete == null)
                    {
                        logger.LogInformation($"File did not find. UserId={user.Id} | FileId={willDelete.Id}");
                        throw new Exception("Does not allow to delete this file.");
                    }

                    var result = await azureBlobStorageService.DeleteAsync(willDelete.Name, cancellationToken);

                    if (result)
                    {
                        logger.LogInformation($"Azure Blob Storage Deletion Failed. UserId={user.Id} | FileId={willDelete.Id}");
                        throw new Exception("Could not delete this file.");
                    }

                    // Remove access
                    var deleteCandidate = dbContext.Access.Where(x => x.FileId == willDelete.Id);
                    dbContext.Access.RemoveRange(deleteCandidate);
                    await dbContext.SaveChangesAsync(cancellationToken);

                    dbContext.Files.Remove(willDelete);
                    await dbContext.SaveChangesAsync(cancellationToken);

                    await transaction.CommitAsync(cancellationToken);
                }
                catch (Exception ex)
                {
                    logger.LogError(ex.Message, ex);

                    await transaction.RollbackAsync(cancellationToken);

                    throw;
                }

                return Unit.Value;
            }

        }

        private readonly AzureBlobStorageService<SampleAzureBlobStorageContainer> azureBlobStorageService;
        private readonly DefaultDbContext dbContext;
        private readonly ILogger logger;
    }
}
