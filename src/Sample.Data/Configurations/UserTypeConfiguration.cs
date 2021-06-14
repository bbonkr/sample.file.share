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
                .ValueGeneratedOnAdd()
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
}
