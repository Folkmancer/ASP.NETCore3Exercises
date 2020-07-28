using System;
using System.Collections.Generic;

namespace LanguageFeatures.Models
{
    public static class MyExtensionMethods
    {
        public static decimal TotalPrices(this IEnumerable<Product> products)
        {
            var totalPrice = 0m;

            foreach (var product in products)
            {
                totalPrice += product?.Price ?? 0m;
            }

            return totalPrice;
        }

        public static IEnumerable<Product> Filter(this IEnumerable<Product> products, Func<Product, bool> selector)
        {
            foreach (var product in products)
            {
                if (selector(product))
                {
                    yield return product;
                }
            }
        }
    }
}