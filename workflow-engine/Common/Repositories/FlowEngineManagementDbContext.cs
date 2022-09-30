using Common.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace Common.Repositories
{
    public class FlowEngineManagementDbContext : DbContext
    {
        public DbSet<User> Users { get; set; }
        public DbSet<FlowConfigurationHistory> FlowConfigurationHistory { get; set; }
        public DbSet<FlowExecutions> FlowExecutions { get; set; }
        public DbSet<Token> Tokens { get; set; }

        public FlowEngineManagementDbContext()
        {

        }

        public FlowEngineManagementDbContext(DbContextOptions<FlowEngineManagementDbContext> options) :base(options)
        {

        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder
                .UseSqlServer("Server=(localdb)\\kris-local;Database=FlowEngineManagementDbContext;User Id = kris;Password=kris;"
                ,builder => builder.EnableRetryOnFailure())
                .UseLazyLoadingProxies();
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>().HasData(new User
            {
                Id = 1,
                Username = "kris",
                Password = "krispass",
                FirstName = "Kris",
                LastName = "Dimov"
            });
        }


    }
}
