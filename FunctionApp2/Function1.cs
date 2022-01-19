using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Linq;

namespace FunctionApp1
{
    public static class Products
    {
        //    [FunctionName("GetProduct")]
        //    public static async Task<IActionResult> Run(
        //        [HttpTrigger(AuthorizationLevel.Function, "get", "post", Route = null)] HttpRequest req,
        //        ILogger log)
        //    {
        //        log.LogInformation("C# HTTP trigger function processed a request.");

        //        string productId = req.Query["productId"];            
        //        var products = new List<Product>() 
        //        { 
        //            new Product() { Id="1", Name="Prod1" },
        //            new Product() { Id="2", Name="Prod2"
        //        }};
        //        string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
        //        dynamic data = JsonConvert.DeserializeObject(requestBody);
        //        productId = productId ?? data?.productId;
        //        string responseMessage = string.Empty;
        //        var product = products.Where(x => x.Id == productId).FirstOrDefault();
        //        if (product != null)
        //        {                
        //            responseMessage = product.Name;
        //        }
        //        return new OkObjectResult(responseMessage);
        //    }
        //}

        [FunctionName("CreateUser")]
        public static async Task<IActionResult> CreateUser(
                [HttpTrigger(AuthorizationLevel.Function, "post", Route = null)] HttpRequest req,
                [CosmosDB(databaseName: "UserProduct",
                collectionName: "Users",
                CreateIfNotExists = true,
                ConnectionStringSetting = "AzureWebJobsStorage")] IAsyncCollector<User> productItemsOut,
                ILogger log)
        {
            string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
            var users = JsonConvert.DeserializeObject<IEnumerable<User>>(requestBody);
            log.LogInformation($"C# Queue trigger function processed {users?.Count()} items");

            foreach (var user in users)
            {
                await productItemsOut.AddAsync(user);
            }
            return new OkObjectResult(true);
        }

        [FunctionName("CreateProduct")]
        public static async Task<IActionResult> CreateProduct(
                [HttpTrigger(AuthorizationLevel.Function, "post", Route = null)] HttpRequest req,
                [CosmosDB(databaseName: "UserProduct",
                collectionName: "Products",
                CreateIfNotExists = true,
                ConnectionStringSetting = "AzureWebJobsStorage")] IAsyncCollector<Product> productItemsOut,
                ILogger log)
        {
            string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
            var products = JsonConvert.DeserializeObject<IEnumerable<Product>>(requestBody);
            log.LogInformation($"C# Queue trigger function processed {products?.Count()} items");

            foreach (var product in products)
            {
                log.LogInformation($"Description={product.ProductName}");
                await productItemsOut.AddAsync(product);
            }
            return new OkObjectResult(true);
        }

        [FunctionName("GetProducts")]
        public static async Task<IActionResult> GetProducts(
            [HttpTrigger(AuthorizationLevel.Function, "get", Route = "products")] HttpRequest req,
            [CosmosDB(databaseName: "UserProduct", collectionName: "Products", SqlQuery = "SELECT * FROM Products",
            ConnectionStringSetting = "AzureWebJobsStorage")] IEnumerable<Product> products,
            ILogger log)
        {
            log.LogInformation("C# HTTP trigger function processed a request.");

            if (products is null)
            {
                return new NotFoundResult();
            }

            return new OkObjectResult(products);
        }

        [FunctionName("GetProduct")]
        public static async Task<IActionResult> GetProduct(
            [HttpTrigger(AuthorizationLevel.Function, "get", Route = "product/{id}")] HttpRequest req,
            [CosmosDB(databaseName: "UserProduct", collectionName: "Products", SqlQuery = "SELECT * FROM Products p where p.ProductId = {id}",
            ConnectionStringSetting = "AzureWebJobsStorage")] IEnumerable<Product> products,
            ILogger log)
        {
            log.LogInformation("C# HTTP trigger function processed a request.");

            if (products is null)
            {
                return new NotFoundResult();
            }

            return new OkObjectResult(products);
        }

        [FunctionName("GetUsers")]
        public static async Task<IActionResult> GetUsers(
            [HttpTrigger(AuthorizationLevel.Function, "get", Route = "users")] HttpRequest req,
            [CosmosDB(databaseName: "UserProduct", collectionName: "Users", SqlQuery = "SELECT * FROM Users",
            ConnectionStringSetting = "AzureWebJobsStorage")] IEnumerable<User> users,
            ILogger log)
        {
            log.LogInformation("C# HTTP trigger function processed a request.");

            if (users is null)
            {
                return new NotFoundResult();
            }

            return new OkObjectResult(users);
        }

        [FunctionName("GetUser")]
        public static async Task<IActionResult> GetUser(
                [HttpTrigger(AuthorizationLevel.Function, "get", Route = "user/{id}")] HttpRequest req,
                [CosmosDB(databaseName: "UserProduct", collectionName: "Users", SqlQuery = "SELECT * FROM Users p where p.UserId = {id}",
            ConnectionStringSetting = "AzureWebJobsStorage")] IEnumerable<User> users,
                ILogger log)
        {
            log.LogInformation("C# HTTP trigger function processed a request.");

            if (users is null)
            {
                return new NotFoundResult();
            }

            return new OkObjectResult(users);
        }
    }
}

