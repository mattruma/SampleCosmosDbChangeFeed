using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Microsoft.Azure.Cosmos;
using System.Collections.Generic;
using FunctionApp1.Data;
using NLipsum.Core;

namespace FunctionApp1
{
    public static class Function1
    {
        private static readonly Lazy<CosmosClient> _cosmosClient =
            new Lazy<CosmosClient>(InitializeCosmosClient);
        private static CosmosDatabase _cosmosDatabase =>
            _cosmosClient.Value.Databases["ToDoList"];

        private static CosmosClient InitializeCosmosClient()
        {
            var connectionString =
                Environment.GetEnvironmentVariable("CosmosDB:ConnectionString");

            var cosmosConfiguration =
                new CosmosConfiguration(connectionString);

            var cosmosClient =
                new CosmosClient(cosmosConfiguration);

            return cosmosClient;
        }

        // https://docs.microsoft.com/en-us/azure/azure-functions/functions-bindings-cosmosdb-v2
        // https://docs.microsoft.com/en-us/azure/cosmos-db/how-to-multi-master#netv3
        // https://jeremylindsayni.wordpress.com/2019/03/18/test-driving-the-cosmos-sdk-v3-with-net-core-getting-started-with-azure-cosmos-db-and-net-core-part-3/

        [FunctionName("Function1")]
        public static async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Function, "post")] HttpRequest req,
            ILogger log)
        {
            var toDoItemAdd =
                JsonConvert.DeserializeObject<ToDoItemAdd>(
                    await new StreamReader(req.Body).ReadToEndAsync());

            var toDoItemDocumentList =
                new List<ToDoItem>();

            var toDoItemContainer =
               _cosmosDatabase.Containers["Items"];

            var lipsumGenerator = 
                new LipsumGenerator(Lipsums.LoremIpsum, false);

            for (var index = 0; index < toDoItemAdd.Copies; index++)
            {
                var toDoItem = 
                    new ToDoItem
                    {
                        Description = lipsumGenerator.GenerateSentences(1)[0]
                    };

                var toDoItemDocument =
                    await toDoItemContainer.Items.CreateItemAsync<ToDoItem>(
                        toDoItem.Id.ToString(),
                        toDoItem);

                log.LogInformation(
                    JsonConvert.SerializeObject(toDoItemDocument, Formatting.Indented));

                toDoItemDocumentList.Add(toDoItemDocument.Resource);
            }

            return new OkObjectResult($"Created {toDoItemDocumentList.Count} documents.");
        }
    }
}
