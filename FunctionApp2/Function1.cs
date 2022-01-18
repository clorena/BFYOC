using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace FunctionApp2
{
    public static class WriteDoc
    {
        [FunctionName("CreateUser")]       
        public static async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Function, "post", Route = null)] User[] users,
            [CosmosDB(
                databaseName: "UserProducts",
                collectionName: "Users",
                ConnectionStringSetting = "AzureWebJobsStorage")]IAsyncCollector<User> userItemsOut,
            ILogger log)
        {
            log.LogInformation($"C# Queue trigger function processed {users?.Length} items");

            foreach (User user in users)
            {
                log.LogInformation($"Description={user.UserName}");
                await userItemsOut.AddAsync(user);
            }
            return new OkObjectResult(true);
        }
        

        [FunctionName("CreateProduct")]
        public static async Task<IActionResult> CreateProduct(
            [HttpTrigger(AuthorizationLevel.Function, "post", Route = null)] HttpRequest req,
            [CosmosDB(
                databaseName: "UserProducts",
                collectionName: "Products",
                ConnectionStringSetting = "AzureWebJobsStorage")]IAsyncCollector<Product> productItemsOut,
            ILogger log)
        {
            var products = new Product[] { };
            log.LogInformation($"C# Queue trigger function processed {products?.Length} items");
            
            foreach (Product product in products)
            {
                log.LogInformation($"Description={product.ProductName}");
                await productItemsOut.AddAsync(product);
            }
            return new OkObjectResult(true);
        }
    }
}
