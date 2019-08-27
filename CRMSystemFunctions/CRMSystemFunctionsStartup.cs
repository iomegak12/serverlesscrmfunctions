using CRMSystemFunctions.Entities.Implementations;
using CRMSystemFunctions.Entities.Interfaces;
using Microsoft.Azure.Functions.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;

[assembly: FunctionsStartup(typeof(CRMSystemFunctions.CRMSystemFunctionsStartup))]

namespace CRMSystemFunctions
{
    public class CRMSystemFunctionsStartup : FunctionsStartup
    {
        private const string INVALID_CONNECTION_STRING = "Invalid Connection String Specified!";
        public override void Configure(IFunctionsHostBuilder builder)
        {
            builder.Services.AddScoped<ICustomersContext>(
                serviceProvider =>
                {
                    var encodedConnectionString = Environment.GetEnvironmentVariable("CustomersDbConnectionString");
                    var connectionString = Encoding.ASCII.GetString(Convert.FromBase64String(encodedConnectionString));

                    if (string.IsNullOrEmpty(connectionString))
                        throw new ApplicationException(INVALID_CONNECTION_STRING);

                    var dbContextOptionsBuilder = new DbContextOptionsBuilder<CustomersContext>();

                    dbContextOptionsBuilder.UseSqlServer(connectionString);

                    var customersInfoContext = new CustomersContext(dbContextOptionsBuilder.Options);

                    return customersInfoContext;
                });
        }
    }
}
