using SportsStore.Models;
using System.Linq;
using Xunit;

namespace SportsStore.Tests
{
    public class CartTests
    {
        [Fact]
        public void CanAddNewLines()
        {
            //Arrange
            var product1 = new Product() { ProductId = 1, Name = "P1" };
            var product2 = new Product() { ProductId = 2, Name = "P2" };
            var cart = new Cart();

            //Act
            cart.AddItem(product1, 1);
            cart.AddItem(product2, 1);

            var result = cart.Lines.ToArray();

            //Assert
            Assert.Equal(2, result.Length);
            Assert.Equal(product1, result[0].Product);
            Assert.Equal(product2, result[1].Product);
        }

        [Fact]
        public void CanAddQuantityForExistingLines()
        {
            //Arrange
            var product1 = new Product() { ProductId = 1, Name = "P1" };
            var product2 = new Product() { ProductId = 2, Name = "P2" };
            var cart = new Cart();

            //Act
            cart.AddItem(product1, 1);
            cart.AddItem(product2, 1);
            cart.AddItem(product1, 10);

            var result = cart.Lines
                .OrderBy(line => line.Product.ProductId)
                .ToArray();

            //Assert
            Assert.Equal(2, result.Length);
            Assert.Equal(11, result[0].Quantity);
            Assert.Equal(1, result[1].Quantity);
        }
        
        [Fact]
        public void CanRemoveLine()
        {
            //Arrange
            var product1 = new Product() { ProductId = 1, Name = "P1" };
            var product2 = new Product() { ProductId = 2, Name = "P2" };
            var product3 = new Product() { ProductId = 3, Name = "P3" };
            var cart = new Cart();

            //Act
            cart.AddItem(product1, 1);
            cart.AddItem(product2, 3);
            cart.AddItem(product3, 5);
            cart.AddItem(product2, 1);
            cart.RemoveLine(product2);

            var result = cart.Lines
                .OrderBy(line => line.Product.ProductId)
                .ToArray();

            //Assert
            Assert.Empty(result.Where(line => line.Product == product2));
            Assert.Equal(2, result.Length);
        }

        [Fact]
        public void CalculateCartTotal()
        {
            //Arrange
            var product1 = new Product() { ProductId = 1, Name = "P1", Price = 100m };
            var product2 = new Product() { ProductId = 2, Name = "P2", Price = 50m };
            var cart = new Cart();

            cart.AddItem(product1, 1);
            cart.AddItem(product2, 1);
            cart.AddItem(product1, 3);

            //Act
            var result = cart.ComputeTotalValue();

            //Assert
            Assert.Equal(450m, result);
        }
        
        [Fact]
        public void CanClearContents()
        {
            //Arrange
            var product1 = new Product() { ProductId = 1, Name = "P1", Price = 100m };
            var product2 = new Product() { ProductId = 2, Name = "P2", Price = 50m };
            var cart = new Cart();

            cart.AddItem(product1, 1);
            cart.AddItem(product2, 1);
            cart.AddItem(product1, 3);
            
            //Act
            cart.Clear();

            //Assert
            Assert.Empty(cart.Lines);
        }
    }
}