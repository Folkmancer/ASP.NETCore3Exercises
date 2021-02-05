using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Routing;
using Moq;
using SportsStore.Models;
using SportsStore.Pages;
using System.Linq;
using System.Text;
using System.Text.Json;
using Xunit;

namespace SportsStore.Tests
{
    public class CartPageTests
    {
        [Fact]
        public void CanLoadCart()
        {
            //Arrange
            var product1 = new Product { ProductId = 1, Name = "P1" };
            var product2 = new Product { ProductId = 2, Name = "P2" };
            var products = new[] { product1, product2 };
            var mockRepository = new Mock<IStoreRepository>();

            mockRepository.Setup(repository => repository.Products)
                .Returns(products.AsQueryable());

            var cart = new Cart();
            
            cart.AddItem(product1, 2);
            cart.AddItem(product2, 1);

            var mockSession = new Mock<ISession>();
            var sessionData = Encoding.UTF8.GetBytes(JsonSerializer.Serialize(cart));

            mockSession.Setup(session => session.TryGetValue(It.IsAny<string>(), out sessionData));
            
            var mockHttpContext = new Mock<HttpContext>();

            mockHttpContext.SetupGet(httpContext => httpContext.Session).Returns(mockSession.Object);

            //Act
            var cartModel = new CartModel(mockRepository.Object)
            {
                PageContext =
                {
                    HttpContext = mockHttpContext.Object,
                    RouteData = new RouteData(),
                    ActionDescriptor = new CompiledPageActionDescriptor()
                }
            };

            cartModel.OnGet("myUrl");

            //Assert
            Assert.Equal(2, cartModel.Cart.Lines.Count());
            Assert.Equal("myUrl", cartModel.ReturnUrl);
        }
        
        [Fact]
        public void CanUpdateCart()
        {
            //Arrange
            var products = new[] { new Product { ProductId = 1, Name = "P1" } };
            var mockRepository = new Mock<IStoreRepository>();

            mockRepository.Setup(repository => repository.Products)
                .Returns(products.AsQueryable());

            var cart = new Cart();
            var mockSession = new Mock<ISession>();

            mockSession.Setup(session => session.Set(It.IsAny<string>(), It.IsAny<byte[]>()))
                .Callback<string, byte[]>((key, val) => {
                    cart = JsonSerializer.Deserialize<Cart>(Encoding.UTF8.GetString(val));
                });

            var mockHttpContext = new Mock<HttpContext>();

            mockHttpContext.SetupGet(httpContext => httpContext.Session).Returns(mockSession.Object);

            //Act
            var cartModel = new CartModel(mockRepository.Object)
            {
                PageContext =
                {
                    HttpContext = mockHttpContext.Object,
                    RouteData = new RouteData(),
                    ActionDescriptor = new CompiledPageActionDescriptor()
                }
            };

            cartModel.OnPost(1, "myUrl");

            //Assert
            Assert.Single(cart.Lines);
            Assert.Equal("P1", cart.Lines.First().Product.Name);
            Assert.Equal(1, cart.Lines.First().Quantity);
        }
    }
}