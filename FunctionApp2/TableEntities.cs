using Microsoft.Azure.Cosmos.Table;
using System;

namespace FunctionApp2
{
    public class User
    {
        public string UserId { get; set; }
        public string UserName { get; set; }
        public string FullName { get; set; }        
    }

    public class Product
    {
        public string ProductId { get; set; }
        public string ProductName { get; set; }
        public string ProductDescription { get; set; }       
    }
}
