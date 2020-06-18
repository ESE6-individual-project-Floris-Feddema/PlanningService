using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Documents.Client;
using Microsoft.Azure.Documents.Linq;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Extensions.Logging;
using PlanningService.Models;

namespace PlanningService.Functions
{
    public class GetAllCompanyPlanning
    {
        [FunctionName("GetAllCompanyPlanning")]
        public static async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "planning/company/{companyId}")]
            HttpRequest req,
            [CosmosDB(
                databaseName: "PlanningStore",
                collectionName: "CompanyPlannings",
                ConnectionStringSetting = "PlanningDB",
                PartitionKey = "{companyId}")] DocumentClient client,
            ILogger log, string companyId)
        {
            try
            {
                if (string.IsNullOrEmpty(companyId)) throw new Exception("companyId parameter is empty");

                var collectionUri = UriFactory.CreateDocumentCollectionUri("PlanningStore", "CompanyPlannings");

                var query = client.CreateDocumentQuery<Planning>(collectionUri)
                    .Where(p => p.CompanyId == companyId).AsDocumentQuery();

                var list = new List<Planning>();
                while (query.HasMoreResults)
                {
                    foreach (Planning item in await query.ExecuteNextAsync())
                    {
                        list.Add(item);
                    }
                }

                return new OkObjectResult(list);
            }
            catch (Exception e)
            {
                log.LogError(e.Message);
                return new BadRequestObjectResult(e.Message);
            }
        }
    }
}
