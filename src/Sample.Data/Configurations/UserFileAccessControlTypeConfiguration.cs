
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

using Sample.Entities;

namespace Sample.Data.Configurations
{
    public class UserFileAccessControlTypeConfiguration : IEntityTypeConfiguration<UserFileAccessControl>
    {
        public void Configure(EntityTypeBuilder<UserFileAccessControl> builder)
        {
            builder.HasKey(x => x.Id);

            builder.Property(x => x.Id)
                .IsRequired()
                .HasColumnType("char(36)")
                .ValueGeneratedOnAdd()
                .HasComment("Identifier");

            builder.Property(x => x.UserId)
                .IsRequired()
                .HasColumnType("char(36)")
                .HasComment("User identifier");

            builder.Property(x => x.FileId)
                .IsRequired()
                .HasColumnType("char(36)")
                .HasComment("File identifier");

            builder.Property(x => x.Token)
                .IsRequired()
                .HasMaxLength(1000)
                .HasComment("Access token '/api/file/{token}'");

            builder.Property(x => x.ExpiresOn)
                .HasComment("Expires access");

            builder.HasIndex(x => x.Token).IsUnique();

            builder.HasOne(x => x.User)
                .WithMany(x => x.FileAccess)
                .HasForeignKey(x => x.UserId);

            builder.HasOne(x => x.File)
                .WithMany(x => x.FileAccess)
                .HasForeignKey(x => x.FileId);
        }
    }
}
