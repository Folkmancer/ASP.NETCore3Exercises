using System.Collections.Generic;
using System.Linq;

namespace LanguageFeatures.Models
{
    public interface IProductSelection
    {
        public IEnumerable<Product> Products { get; }
        public IEnumerable<string> Names => Products.Select(product => product.Name);
    }
}