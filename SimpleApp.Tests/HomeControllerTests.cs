using Moq;
using SimpleApp.Controllers;
using SimpleApp.Models;
using System.Collections.Generic;
using Xunit;

namespace SimpleApp.Tests
{
    public class HomeControllerTests
    {
        [Fact]
        public void IndexActionModelIsComplete()
        {
            var testData = new[]
            {
                new Product { Name = "P1", Price = 75.10M },
                new Product { Name = "P2", Price = 120M },
                new Product { Name = "P3", Price = 110M }
            };
            
            var controller = new HomeController();
            var mock = new Mock<IDataSource>();
            mock.SetupGet(data => data.Products).Returns(testData);
            controller.DataSource = mock.Object;
            
            var model = controller.Index()?.ViewData.Model as IEnumerable<Product>;

            Assert.Equal(testData, model, Comparer.Get<Product>((p1, p2) => p1.Name == p2.Name && p1.Price == p2.Price));
            mock.VerifyGet(data => data.Products, Times.Once);
        }
    }
}