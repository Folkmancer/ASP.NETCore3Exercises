using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using SportsStore.Models;
using System.Linq;

namespace SportsStore.Pages
{
    public class CartModel : PageModel
    {
        private readonly IStoreRepository repository;

        public CartModel(IStoreRepository repository, Cart cartService)
        {
            this.repository = repository;
            Cart = cartService;
        }

        public Cart Cart { get; set; }
        public string ReturnUrl { get; set; }

        public void OnGet(string returnUrl)
        {
            ReturnUrl = returnUrl ?? "/";
        }

        public IActionResult OnPost(long productId, string returnUrl)
        {
            var product = repository.Products
                .Where(product => product.ProductId == productId)
                .FirstOrDefault();

            Cart.AddItem(product, 1);

            return RedirectToPage(new { returnUrl = returnUrl });
        }

        public IActionResult OnPostRemove(long productId, string returnUrl)
        {
            var product = Cart.Lines
                .First(line => line.Product.ProductId == productId)
                .Product;

            Cart.RemoveLine(product);

            return RedirectToPage(new { returnUrl = returnUrl });
        }
    }
}