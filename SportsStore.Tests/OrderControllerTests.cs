using Microsoft.AspNetCore.Mvc;
using Moq;
using SportsStore.Controllers;
using SportsStore.Models;
using Xunit;

namespace SportsStore.Tests
{
    public class OrderControllerTests
    {
        [Fact]
        public void CannotCheckoutEmptyCart()
        {
            //Arrange - create a mock repository 
            var mockRepository = new Mock<IOrderRepository>();
            //Arrange - create an instance of the order controller
            var targetOrderController = new OrderController(mockRepository.Object, new Cart());

            //Act
            var result = targetOrderController.Checkout(new Order()) as ViewResult;

            //Assert - check that the order hasn't been stored
            mockRepository.Verify(m => m.SaveOrder(It.IsAny<Order>()), Times.Never);
            //Assert - check that method is returning the default view
            Assert.True(string.IsNullOrEmpty(result.ViewName));
            //Assert - check that I am passing an invalid model to the view
            Assert.False(result.ViewData.ModelState.IsValid);
        }

        [Fact]
        public void CannotCheckoutInvalidShippingDetails()
        {
            //Arrange - create a mock repository 
            var mockRepository = new Mock<IOrderRepository>();
            //Arrange - create a cart with one item
            var cart = new Cart();
            cart.AddItem(new Product(), 1);
            //Arrange - create an instance of the order controller
            var targetOrderController = new OrderController(mockRepository.Object, cart);
            //Arrange - add an error to the model
            targetOrderController.ModelState.AddModelError("error", "error");

            //Act - try to checkout
            var result = targetOrderController.Checkout(new Order()) as ViewResult;

            //Assert - check that the order hasn't been stored
            mockRepository.Verify(m => m.SaveOrder(It.IsAny<Order>()), Times.Never);
            //Assert - check that method is returning the default view
            Assert.True(string.IsNullOrEmpty(result.ViewName));
            //Assert - check that I am passing an invalid model to the view
            Assert.False(result.ViewData.ModelState.IsValid);
        }
        
        [Fact]
        public void CanCheckoutAndSubmitOrder()
        {
            //Arrange - create a mock repository 
            var mockRepository = new Mock<IOrderRepository>();
            //Arrange - create a cart with one item
            var cart = new Cart();
            cart.AddItem(new Product(), 1);
            //Arrange - create an instance of the order controller
            var targetOrderController = new OrderController(mockRepository.Object, cart);

            //Act - try to checkout
            var result = targetOrderController.Checkout(new Order()) as RedirectToPageResult;

            //Assert - check that the order has been stored
            mockRepository.Verify(m => m.SaveOrder(It.IsAny<Order>()), Times.Once);
            //Assert - check that I am passing an invalid model to the view
            Assert.Equal("/Completed", result.PageName);
        }
    }
}