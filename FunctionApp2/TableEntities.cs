using System;
using System.Collections.Generic;
using System.Text;

namespace FunctionApp1
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

    public class ProductRating
    {
        public Guid Id { get; set; }
        public string UserId { get; set; }
        public string ProductId { get; set; }
        public string LocationName { get; set; }
        public int Rating { get; set; }
        public string UserNotes { get; set; }
        public DateTime Timestamp { get; set; }
    }
}
