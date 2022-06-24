using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CookbookBE.DataLayer.Entities
{
    public class User
    {
        public int Id { get; set; }

        public String FirstName { get; set; }

        public String ProfilePictureURL { get; set; }

        public String LastName { get; set; }

        public String Email { get; set; }

        public String PasswordHash { get; set; }
    }

    public class UserConfiguration : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> builder)
        {
            builder.ToTable("Users");
            builder.HasKey(u => u.Id);
            builder.Property(u => u.FirstName).HasMaxLength(128).IsRequired();
            builder.Property(u => u.LastName).HasMaxLength(128).IsRequired();
            builder.Property(u => u.Email).HasMaxLength(128).IsRequired();
            builder.Property(u => u.PasswordHash).HasMaxLength(512).IsRequired();

        }
    }
}
