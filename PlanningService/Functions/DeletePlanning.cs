using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Azure.Documents;
using Microsoft.Azure.Documents.Client;
using Microsoft.Azure.Documents.Linq;
using Microsoft.Extensions.Logging;
using PlanningService.Models;

namespace PlanningService.Functions
{
    public static class DeletePlanning
    {
        [FunctionName("DeletePlanning")]
        public static async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "delete", Route = "planning/{companyId}/{id}")] 
            HttpRequest req,
            [CosmosDB(
                databaseName: "PlanningStore",
                collectionName: "CompanyPlannings",
                ConnectionStringSetting = "PlanningDB",
                PartitionKey = "{companyId}")] DocumentClient client,
            ILogger log, string companyId, Guid? id)
        {
            try
            {
                if (id == null) throw new Exception("id parameter is empty");
                if (string.IsNullOrEmpty(companyId)) throw new Exception("companyId parameter is empty");

                var collectionUri = UriFactory.CreateDocumentCollectionUri("PlanningStore", "CompanyPlannings");
                var option = new FeedOptions {PartitionKey = new PartitionKey(companyId.ToString())};

                var query = client.CreateDocumentQuery<Planning>(collectionUri, option)
                    .Where(p => p.id == id).AsDocumentQuery();

                while (query.HasMoreResults)
                {
                    foreach (var item in await query.ExecuteNextAsync())
                    {
                        var i = (Document)item;
                        var requestOptions = new RequestOptions()
                        {
                            PartitionKey = new PartitionKey(companyId.ToString())
                        };
                        await client.DeleteDocumentAsync(i.SelfLink, requestOptions);
                    }
                }

                return new NoContentResult();
            }
            catch (Exception e)
            {
                log.LogError(e.Message);
                return new BadRequestObjectResult(e.Message);
            }
        }
    }
}
