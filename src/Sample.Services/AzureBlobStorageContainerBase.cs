using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sample.Services
{
    public abstract class AzureBlobStorageContainerBase
    {
        public string ContainerName { get => GetContainerName(); }

        public abstract string GetContainerName();
    }
}
