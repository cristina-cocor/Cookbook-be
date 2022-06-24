using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CookbookBE.DataLayer.Entities
{
    public class Cookbook
    {
        public int IdE { get; set; }

        public String RecipeName { get; set; }

        public String Ingredients { get; set; }

        public String Instructions { get; set; }

        public String TotalTime { get; set; }

    }

    public class CookbookConfiguration : IEntityTypeConfiguration<Cookbook>
    {
        public void Configure(EntityTypeBuilder<Cookbook> builder)
        {
            builder.ToTable("Cookbook");
            builder.HasKey(c => c.IdE);
            builder.Property(c => c.RecipeName).HasMaxLength(128).IsRequired();
            builder.Property(c => c.Ingredients).HasMaxLength(384).IsRequired();
            builder.Property(c => c.Instructions).HasMaxLength(384).IsRequired();
            builder.Property(c => c.TotalTime).HasMaxLength(50).IsRequired();
        }
    }
}
