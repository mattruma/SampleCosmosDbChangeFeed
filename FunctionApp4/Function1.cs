using System.Collections.Generic;
using Microsoft.Azure.Documents;
using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace FunctionApp4
{
    public static class Function1
    {
        [FunctionName("Function1")]
        public static void Run([CosmosDBTrigger(
            databaseName: "ToDoList",
            collectionName: "Items",
            StartFromBeginning = true,
            ConnectionStringSetting = "CosmosDB:ConnectionString",
            LeaseCollectionName = "FunctionApp4-Function1-leases",
            CreateLeaseCollectionIfNotExists = true)]IReadOnlyList<Document> documents, ILogger log)
        {
            log.LogInformation("Function1 function processed a request.");

            if (documents != null)
            {
                foreach (var document in documents)
                {
                    log.LogInformation(JsonConvert.SerializeObject(document, Formatting.Indented));
                }
            }
        }
    }
}
