using System;
using System.Collections.Generic;

namespace Sample.Entities
{
    /// <summary>
    /// User
    /// </summary>
    public class User
    {
        public Guid Id { get; set; }

        public string UserName { get; set; }

        public string DisplayName { get; set; }

        public string Email { get; set; }

        public virtual ICollection<UserFileAccessControl> FileAccess { get; set; }
    }
}
