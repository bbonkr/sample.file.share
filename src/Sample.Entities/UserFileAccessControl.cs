using System;

namespace Sample.Entities
{
    /// <summary>
    /// User file access control
    /// </summary>
    public class UserFileAccessControl
    {
        public Guid UserId { get; set; }

        public Guid FileId { get; set; }

        public DateTimeOffset? ExpiresOn { get; set; }

        public virtual User User { get; set; }

        public virtual FileInformation File { get; set; }
    }
}
