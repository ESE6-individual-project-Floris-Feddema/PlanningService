using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using PlanningService.Helpers;
using PlanningService.Models;

namespace PlanningService.Functions
{
    public static class CreatePlanning
    {
        [FunctionName("CreatePlanning")]
        public static async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = "planning")] 
            HttpRequest req,
            [CosmosDB(
                databaseName: "PlanningStore",
                collectionName: "CompanyPlannings",
                ConnectionStringSetting = "PlanningDB")]IAsyncCollector<Planning> plannings,
            ILogger log)
        {
            try
            {
                var requestBody = await new StreamReader(req.Body).ReadToEndAsync();
                var data = JsonConvert.DeserializeObject<Planning>(requestBody);
                data.id = Guid.NewGuid();
                foreach (var day in data.WorkDays)
                {
                    day.id = Guid.NewGuid();
                }
                data.Validate();
                await plannings.AddAsync(data);
                return new OkObjectResult(data);
            }
            catch (Exception e)
            {
               log.LogError(e.Message);
               return new BadRequestObjectResult(e.Message);

            } 
        }
    }
}
