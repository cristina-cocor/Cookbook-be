using CookbookBE.DataLayer.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace CookbookBE.DataLayer
{
    public class ApplicationContext : DbContext
    {
        public ApplicationContext(DbContextOptions opts): base(opts)
        {

        }

        public virtual DbSet<User> Users { get; set; }
        
       public virtual DbSet<Cookbook> Cookbook { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());

        }
    }
}
