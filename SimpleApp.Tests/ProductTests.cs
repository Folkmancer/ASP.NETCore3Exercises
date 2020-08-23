using SimpleApp.Models;
using Xunit;

namespace SimpleApp.Tests
{
    public class ProductTests
    {
        [Fact]
        public void CanChangeProductName()
        {
            //Arrange
            var product = new Product() { Name = "Test", Price = 100m };

            //Act
            product.Name = "New Name";

            //Assert
            Assert.Equal("New Name", product.Name);
        }

        [Fact]
        public void CanChangeProductPrice()
        {
            //Arrange
            var product = new Product() { Name = "Test", Price = 100m };

            //Act
            product.Price = 200m;

            //Assert
            Assert.Equal(200m , product.Price);
        }
    }
}