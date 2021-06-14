using System;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

using Sample.Entities;

namespace Sample.Data.Configurations
{
    public class FileInformationTypeConfiguration : IEntityTypeConfiguration<FileInformation>
    {
        public void Configure(EntityTypeBuilder<FileInformation> builder)
        {
            builder.HasKey(x => x.Id);

            builder.Property(x => x.Id)
                .IsRequired()
                .HasColumnType("char(36)")
                .ValueGeneratedOnAdd()
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

            builder.Property(x => x.CreatedBy)
                .IsRequired()
                .HasComment("Creator");

            builder.HasOne(x => x.Creator)
                .WithMany(x => x.Files)
                .HasForeignKey(x => x.CreatedBy);

        }
    }
}
