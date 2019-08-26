using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System.Text;
using CRMSystemFunctions.Entities.Implementations;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using CRMSystemFunctions.Models;

namespace CRMSystemFunctions
{
    public static class CustomerInfoProviderFunctions
    {
        [FunctionName("CustomerInfoProviderFunctions")]
        public static async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Function, "get", Route = null)] HttpRequest req,
            ILogger log)
        {
            log.LogInformation("Customer Info Provider Service Started ... " + DateTime.Now.ToString());

            var result = default(IActionResult);

            try
            {
                var customerId = int.Parse(req.Query["customerId"]);

                var encodedConnectionString = Environment.GetEnvironmentVariable("CustomersDbConnectionString");
                var connectionString = Encoding.ASCII.GetString(
                    Convert.FromBase64String(encodedConnectionString));

                var contextOptionsBuilder = new DbContextOptionsBuilder<CustomersContext>();

                contextOptionsBuilder.UseSqlServer(connectionString);

                using (var context = new CustomersContext(contextOptionsBuilder.Options))
                {
                    var filteredCustomer =
                        context
                        .Customers
                        .Where(customer => customer.CustomerId.Equals(customerId))
                        .FirstOrDefault();

                    if (filteredCustomer == default(Customer))
                        result = new NotFoundResult();
                    else
                        result = new OkObjectResult(filteredCustomer);
                }
            }
            catch (Exception exceptionObject)
            {
                log.LogError(exceptionObject, exceptionObject.Message);

                result = new BadRequestResult();
            }

            log.LogInformation("Customer Info Provider Service Completed ... " + DateTime.Now.ToString());

            return result;
        }
    }
}
