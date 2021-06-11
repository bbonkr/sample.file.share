using System;

using Microsoft.EntityFrameworkCore;

using Sample.Entities;

namespace Sample.Data
{
    public class DefaultDbContext : DbContext
    {
        public DefaultDbContext(DbContextOptions<DefaultDbContext> options):base(options)
        {

        }

        public DbSet<User> Users { get; set; }

        public DbSet<FileInformation> Files { get; set; }

        public DbSet<UserFileAccessControl> Access { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.ApplyConfigurationsFromAssembly(this.GetType().Assembly);
        }
    }
}
