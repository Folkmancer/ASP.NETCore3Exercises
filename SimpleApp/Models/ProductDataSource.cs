using System.Collections.Generic;

namespace SimpleApp.Models
{
    public class ProductDataSource : IDataSource
    {
        public IEnumerable<Product> Products => new[]
        {
                new Product() { Name = "Kayak", Price = 275m },
                new Product() { Name = "Lifejacket", Price = 48.95m }
        };
    }
}