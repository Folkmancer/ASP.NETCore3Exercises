using System.Collections.Generic;
using System.Linq;

namespace SportsStore.Models
{
    public class Cart
    {
        public List<CartLine> Lines { get; set; } = new List<CartLine>();

        public void AddItem(Product product, int quantity)
        {
            var line = Lines.Where(line => line.Product.ProductId == product.ProductId)
                .FirstOrDefault();

            if (line == null)
            {
                Lines.Add(new CartLine()
                {
                    Product = product,
                    Quantity = quantity
                });
            }
            else
            {
                line.Quantity += quantity;
            }
        }

        public void RemoveLine(Product product)
        {
            Lines.RemoveAll(line => line.Product.ProductId == product.ProductId);
        }

        public decimal ComputeTotalValue()
        {
            return Lines.Sum(line => line.Product.Price * line.Quantity);
        }

        public void Clear()
        {
            Lines.Clear();
        }

        public class CartLine
        {
            public int CartLineId { get; set; }
            public Product Product { get; set; }
            public int Quantity { get; set; }
        }
    }
}