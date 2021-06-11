using System;
using System.Collections.Generic;

namespace Sample.Entities
{
    /// <summary>
    /// File information
    /// </summary>
    public class FileInformation
    {
        public Guid Id { get; set; }

        public string Name { get; set; }

        public string ContentType { get; set; }

        public long Size { get; set; }

        public string Uri { get; set; }

        public DateTimeOffset CreatedAt { get; set; }

        public virtual ICollection<UserFileAccessControl> FileAccess { get; set; }
    }
}
