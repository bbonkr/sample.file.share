using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sample.Services.Models
{
    public class AzureBlobUploadResponseModel
    {
        public string BlobName { get; set; }

        public string ContainerName { get; set; }

        public Uri Uri { get; set; }
    }
}
