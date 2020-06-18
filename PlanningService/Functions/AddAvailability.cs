using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Documents;
using Microsoft.Azure.Documents.Client;
using Microsoft.Azure.Documents.Linq;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using PlanningService.Models;
using PlanningService.Views;

namespace PlanningService.Functions
{
    public class AddAvailability
    {
        [FunctionName("AddAvailability")]
        public static async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "put", Route = "planning/availability/{companyId}/{planningId}")]
            HttpRequest req,
            [CosmosDB(
                databaseName: "PlanningStore",
                collectionName: "CompanyPlannings",
                ConnectionStringSetting = "PlanningDB",
                PartitionKey = "{companyId}")] DocumentClient client,
            ILogger log, string companyId, Guid? planningId)
        {
            try
            {
                var requestBody = await new StreamReader(req.Body).ReadToEndAsync();
                var data = JsonConvert.DeserializeObject<List<AvailabilitySlotView>>(requestBody);

                var collectionUri = UriFactory.CreateDocumentCollectionUri("PlanningStore", "CompanyPlannings");
                var option = new FeedOptions { PartitionKey = new PartitionKey(companyId) };

                var query = client.CreateDocumentQuery<Planning>(collectionUri, option).Where(p => p.id == planningId).AsDocumentQuery();

                if (query.HasMoreResults)
                {
                    foreach (var item in await query.ExecuteNextAsync())
                    {
                        var planning = (Planning) item;
                        var document = (Document) item;
                        var requestOptions = new RequestOptions()
                        {
                            PartitionKey = new PartitionKey(companyId)
                        };
                        foreach (var slots in data)
                        {
                            foreach (var day in planning.WorkDays.Where(day => slots.WorkDayId == day.id))
                            {
                                day.Slots.Add(new AvailabilitySlot()
                                {
                                    id = Guid.NewGuid(),
                                    User = slots.User
                                });
                            }
                        }
                        await client.ReplaceDocumentAsync(document.SelfLink, planning, requestOptions);
                    }
             
                }

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
