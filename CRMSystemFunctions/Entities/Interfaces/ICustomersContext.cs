using CRMSystemFunctions.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace CRMSystemFunctions.Entities.Interfaces
{
    public interface ICustomersContext : IDisposable
    {
        DbSet<Customer> Customers { get; set; }
    }
}
