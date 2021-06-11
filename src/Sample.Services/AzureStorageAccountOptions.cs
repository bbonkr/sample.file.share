using System;

namespace Sample.Services
{
    public class AzureStorageAccountOptions
    {
        public const string Name = "AzureStorageAccount";

        public const string ExceptionMessage = @"Should add { ""ConnectionStrings"": { ""AzureStorageAccount"": ""<Azure Storage Account connection string>"" } } in appsettings.json file or environment variable.";

        public string ConnectionString { get; set; }
    }
}
