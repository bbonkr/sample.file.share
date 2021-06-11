using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

using Sample.Entities;

namespace Sample.Data.Configurations
{
    public class UserTypeConfiguration : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> builder)
        {
            builder.ToTable("AppUsers");

            builder.HasKey(x => x.Id);

            builder.Property(x => x.Id)
                .IsRequired()
                .HasColumnType("char(36)")
                .HasComment("Identifier");

            builder.Property(x => x.UserName)
                .IsRequired()
                .HasMaxLength(100)
                .HasComment("User account name");

            builder.Property(x => x.Email)
                .IsRequired()
                .HasMaxLength(500)
                .HasComment("Email address");

            builder.Property(x => x.DisplayName)
                .IsRequired()
                .HasMaxLength(100)
                .HasComment("display name");
        }
    }

    public class FileInformationTypeConfiguration : IEntityTypeConfiguration<FileInformation>
    {
        public void Configure(EntityTypeBuilder<FileInformation> builder)
        {
            builder.HasKey(x => x.Id);

            builder.Property(x => x.Id)
                .IsRequired()
                .HasColumnType("char(36)")
                .HasComment("Identifier");

            builder.Property(x => x.Name)
                .IsRequired()
                .HasMaxLength(1000)
                .HasComment("File name");

            builder.Property(x => x.ContentType)
                .IsRequired()
                .HasMaxLength(100)
                .HasComment("Content type");

            builder.Property(x => x.Size)
                .IsRequired()
                .HasDefaultValue(0)
                .HasComment("File size");

            builder.Property(x => x.Uri)
                .IsRequired()
                .HasMaxLength(2000)
                .HasComment("File origin access uri;");

            builder.Property(x => x.CreatedAt)
                .IsRequired()
                .HasDefaultValue(DateTimeOffset.UtcNow)
                .HasComment("File uploaded at");

        }
    }

    public class UserFileAccessControlTypeConfiguration : IEntityTypeConfiguration<UserFileAccessControl>
    {
        public void Configure(EntityTypeBuilder<UserFileAccessControl> builder)
        {
            builder.HasKey(x => new { x.UserId, x.FileId });

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

            builder.HasOne(x => x.User)
                .WithMany(x => x.FileAccess)
                .HasForeignKey(x => x.UserId);

            builder.HasOne(x => x.File)
                .WithMany(x => x.FileAccess)
                .HasForeignKey(x => x.FileId);
        }
    }
}
