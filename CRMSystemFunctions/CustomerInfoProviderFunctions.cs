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
using CRMSystemFunctions.Entities.Interfaces;

namespace CRMSystemFunctions
{
    public class CustomerInfoProviderFunctions
    {
        private ICustomersContext customersContext = default(ICustomersContext);

        public CustomerInfoProviderFunctions(ICustomersContext customersContext)
        {
            if (customersContext == default(ICustomersContext))
                throw new ArgumentNullException(nameof(customersContext));

            this.customersContext = customersContext;
        }

        [FunctionName("CustomerInfoProviderFunctions")]
        public async Task<IActionResult> Run(
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

                var filteredCustomer = this.customersContext
                    .Customers
                    .Where(customer => customer.CustomerId.Equals(customerId))
                    .FirstOrDefault();

                if (filteredCustomer == default(Customer))
                    result = new NotFoundResult();
                else
                    result = new OkObjectResult(filteredCustomer);
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
