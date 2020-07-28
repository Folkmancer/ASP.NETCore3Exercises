namespace LanguageFeatures.Models
{
    public class Product
    {
        public string Name { get; set; }
        public string Category { get; set; } = "Watersports";
        public decimal? Price { get; set; }
        public Product Related { get; set; }
        public bool InStock { get; } = true;
        public bool NameBeginsWithS => Name?[0] == 'S';

        public Product(bool inStock = true)
        {
            InStock = inStock;
        }

        public static Product[] GetProducts()
        {
            var kayak = new Product()
            {
                Name = "Kayak",
                Category = "Water Craft",
                Price = 275m
            };

            var lifejacket = new Product(false)
            {
                Name = "Lifejacket",
                Price = 48.95m
            };

            kayak.Related = lifejacket;

            return new Product[] { kayak, lifejacket, null };
        }
    }
}