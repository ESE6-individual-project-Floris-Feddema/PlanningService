using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using PlanningService.Models;

namespace PlanningService
{
    public static class CreatePlanning
    {
        [FunctionName("CreatePlanning")]
        public static async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = "Planning")] 
            HttpRequest req,
            [CosmosDB(
                databaseName: "PlanningStore",
                collectionName: "Plannings",
                ConnectionStringSetting = "PlanningDB")]IAsyncCollector<Planning> plannings,
            ILogger log)
        {
            try
            {
                var requestBody = await new StreamReader(req.Body).ReadToEndAsync();
                var data = JsonConvert.DeserializeObject<Planning>(requestBody);
                data.id = Guid.NewGuid();
                await plannings.AddAsync(data);
                return new OkObjectResult(data);
            }
            catch (Exception e)
            {
               log.LogError(e.Message);
            }
            return new OkResult();
        }
    }
}
