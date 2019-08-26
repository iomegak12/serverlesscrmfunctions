using CRMSystemFunctions.Entities.Interfaces;
using CRMSystemFunctions.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace CRMSystemFunctions.Entities.Implementations
{
    public class CustomersContext : DbContext, ICustomersContext
    {
        public CustomersContext(DbContextOptions<CustomersContext> dbContextOptions) : base(dbContextOptions) { }

        public DbSet<Customer> Customers { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.ApplyConfiguration<Customer>(
                new CustomerEntityTypeConfiguration());
        }
    }
}
