using System;
using System.Collections.Generic;
using Microsoft.Azure.Documents;
using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace FunctionApp1
{
    public static class Function2
    {
        [FunctionName("Function2")]
        public static void Run([CosmosDBTrigger(
            databaseName: "ToDoList",
            collectionName: "Items",
            StartFromBeginning = true,
            ConnectionStringSetting = "CosmosDB:ConnectionString",
            LeaseCollectionName = "FunctionApp2-leases",
            CreateLeaseCollectionIfNotExists = true)]IReadOnlyList<Document> documents, ILogger log)
        {
            // https://anthonychu.ca/post/scaling-azure-functions-http/

            var instanceId =
                Environment.GetEnvironmentVariable(
                    "WEBSITE_INSTANCE_ID",
                    EnvironmentVariableTarget.Process);

            log.LogInformation($"Running on instance {instanceId}.");

            if (documents != null)
            {
                log.LogInformation($"{nameof(Function2)} received {documents.Count} document(s).");

                foreach (var document in documents)
                {
                    log.LogDebug(JsonConvert.SerializeObject(document, Formatting.Indented));
                }
            }
            else
            {
                log.LogInformation($"{nameof(Function1)} received 0 document(s).");
            }
        }
    }
}
