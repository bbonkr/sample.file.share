using System;

namespace Sample.Entities
{
    /// <summary>
    /// User file access control
    /// </summary>
    public class UserFileAccessControl
    {
        public Guid Id { get; set; }

        public Guid UserId { get; set; }

        public Guid FileId { get; set; }

        public string Token { get; set; }

        public DateTimeOffset? ExpiresOn { get; set; }

        public virtual User User { get; set; }

        public virtual FileInformation File { get; set; }
    }
}
