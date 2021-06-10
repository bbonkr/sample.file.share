using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;

using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;

using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

using Sample.Services.Models;

namespace Sample.Services
{
    public class AzureBlobStorageService<TAzureBlobStorageContainer> where TAzureBlobStorageContainer : AzureBlobStorageContainerBase
    {
        public AzureBlobStorageService(
            IOptionsMonitor<AzureStorageAccountOptions> azureStorageAccountOptionsMonitor,
            IOptionsMonitor<TAzureBlobStorageContainer> azureBlobStorageContainerMonitor,
            ILogger<AzureBlobStorageService<TAzureBlobStorageContainer>> logger)
        {
            this.logger = logger;

            azureStorageAccountOptions = azureStorageAccountOptionsMonitor.CurrentValue ?? throw new ArgumentException(AzureStorageAccountOptions.ExceptionMessage);
            azureBlobStorageContainer = azureBlobStorageContainerMonitor.CurrentValue ?? throw new ArgumentException(AzureBlobStorageContainerBase.ExceptionMessage);

            containerClient = new BlobContainerClient(azureStorageAccountOptions.ConnectionString, azureBlobStorageContainer.ContainerName);
        }

        public async Task<AzureBlobUploadResponseModel> UploadAsync(string name, Stream stream, string contentType = "", CancellationToken cancellationToken = default)
        {
            var actualName = name;
            try
            {
                await EnsureContainerCreated(cancellationToken);

                var blobClient = containerClient.GetBlobClient(actualName);

                

                var exists = await blobClient.ExistsAsync(cancellationToken);
                if (exists)
                {
                    do
                    {
                        // rename {originalName}-{ticks}.{extension}
                        actualName = Rename(actualName);

                        blobClient = containerClient.GetBlobClient(actualName);
                        exists = await blobClient.ExistsAsync(cancellationToken);
                    }
                    while (exists);
                }

                var uploadOptions = new BlobUploadOptions();

                if (!string.IsNullOrWhiteSpace(contentType))
                {
                    uploadOptions.HttpHeaders = new BlobHttpHeaders {
                        ContentType = contentType, 
                    };
                }

                var result = await blobClient.UploadAsync(stream, uploadOptions, cancellationToken);

                return new AzureBlobUploadResponseModel
                {
                    BlobName = actualName,
                    ContainerName = containerClient.Name,
                    Uri = blobClient.Uri,
                };

            }
            catch (Exception ex)
            {
                logger.LogError(ex.Message, ex);

                throw; 
            }
        }

        public async Task<Stream> DownloadAsync(string name, CancellationToken cancellationToken = default)
        {
            var message = "";
            try
            {
                await EnsureContainerCreated(cancellationToken);

                var blobClient = containerClient.GetBlobClient(name);

                var exists = await blobClient.ExistsAsync(cancellationToken);

                if (!exists)
                {
                    message = "Could not find a file.";
                    throw new Exception(message);
                }

                if (await blobClient.ExistsAsync())
                {
                    var result = await blobClient.DownloadAsync();
                    return result.Value.Content;
                }

                return null;
            }
            catch (Exception ex)
            {
                logger.LogError(ex.Message, ex);

                throw;
            }
        }

        public async Task<bool> DeleteAsync(string name, CancellationToken cancellationToken = default)
        {
            var message = "";
            try
            {
                await EnsureContainerCreated();

                var blobClient = containerClient.GetBlobClient(name);

                var exists = await blobClient.ExistsAsync(cancellationToken);

                if (!exists)
                {
                    message = "Could not find a file.";
                    throw new Exception(message);
                }

                var result = await blobClient.DeleteIfExistsAsync(DeleteSnapshotsOption.IncludeSnapshots, cancellationToken: cancellationToken);

                return result;

            }
            catch (Exception ex)
            {
                logger.LogError(ex.Message);

                throw;
            }
        }

        private async Task EnsureContainerCreated(CancellationToken cancellationToken = default)
        {
            await containerClient.CreateIfNotExistsAsync(PublicAccessType.BlobContainer, cancellationToken: cancellationToken);
        }

        private string Rename(string name)
        {
            var nameTokens = name.Split('.');

            var extension = nameTokens.Length > 1 ? nameTokens.Last() : string.Empty;
            var fileNameWithoutExtension = String.Join(".", nameTokens.Take(nameTokens.Length > 1 ? nameTokens.Length - 1 : nameTokens.Length));

            return $"{fileNameWithoutExtension}-{DateTimeOffset.UtcNow.Ticks}{(string.IsNullOrEmpty(extension) ? string.Empty : $".{extension}")}";
        }
     

        private readonly BlobContainerClient containerClient;
        private readonly AzureStorageAccountOptions azureStorageAccountOptions;
        private readonly AzureBlobStorageContainerBase azureBlobStorageContainer;
        private readonly ILogger logger;
    }
}
