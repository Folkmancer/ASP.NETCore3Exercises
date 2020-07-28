using System.Collections.Generic;

namespace LanguageFeatures.Models
{
    public class ShoppingCart : IProductSelection
    {
        private List<Product> products = new List<Product>();

        public IEnumerable<Product> Products { get => products; }

        public ShoppingCart(params Product[] products)
        {
            this.products.AddRange(products);
        }
    }
}