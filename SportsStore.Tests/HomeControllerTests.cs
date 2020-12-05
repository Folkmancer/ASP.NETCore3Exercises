using Microsoft.AspNetCore.Mvc;
using Moq;
using SportsStore.Controllers;
using SportsStore.Models;
using SportsStore.Models.ViewModels;
using System.Linq;
using Xunit;

namespace SportsStore.Tests
{
    public class HomeControllerTests
    {
        [Fact]
        public void CanUseRepository()
        {
            //Arrange
            var mock = new Mock<IStoreRepository>();
            var expectedProducts = new[]
            {
                new Product {ProductId = 1, Name = "P1"},
                new Product {ProductId = 2, Name = "P2"}
            };

            mock.Setup(m => m.Products)
                .Returns(expectedProducts.AsQueryable());

            var homeController = new HomeController(mock.Object);

            //Act
            var result = (homeController.Index() as ViewResult)
                .ViewData
                .Model
                as ProductsListViewModel;

            //Assert
            var resultProducts = result.Products.ToArray();
            Assert.True(resultProducts.Length == 2);
            Assert.Equal(expectedProducts[0].Name, resultProducts[0].Name);
            Assert.Equal(expectedProducts[1].Name, resultProducts[1].Name);
        }

        [Fact]
        public void CanPaginate()
        {
            //Arrange
            var mock = new Mock<IStoreRepository>();
            var expectedProducts = new[]
            {
                new Product {ProductId = 1, Name = "P1"},
                new Product {ProductId = 2, Name = "P2"},
                new Product {ProductId = 3, Name = "P3"},
                new Product {ProductId = 4, Name = "P4"},
                new Product {ProductId = 5, Name = "P5"}
            };

            mock.Setup(m => m.Products)
                .Returns(expectedProducts.AsQueryable());

            var homeController = new HomeController(mock.Object)
            {
                PageSize = 3
            };

            //Act
            var result = (homeController.Index(2) as ViewResult)
                .ViewData
                .Model
                as ProductsListViewModel;

            //Assert
            var resultProducts = result.Products.ToArray();
            Assert.True(resultProducts.Length == 2);
            Assert.Equal(expectedProducts[3].Name, resultProducts[0].Name);
            Assert.Equal(expectedProducts[4].Name, resultProducts[1].Name);
        }

        [Fact]
        public void CanSendPaginationViewModel()
        {
            //Arrange
            var mock = new Mock<IStoreRepository>();
            var expectedProducts = new[]
            {
                new Product {ProductId = 1, Name = "P1"},
                new Product {ProductId = 2, Name = "P2"},
                new Product {ProductId = 3, Name = "P3"},
                new Product {ProductId = 4, Name = "P4"},
                new Product {ProductId = 5, Name = "P5"}
            };

            mock.Setup(m => m.Products)
                .Returns(expectedProducts.AsQueryable());

            var homeController = new HomeController(mock.Object)
            {
                PageSize = 3
            };

            //Act
            var result = (homeController.Index(2) as ViewResult)
                .ViewData
                .Model
                as ProductsListViewModel;

            //Assert
            var pagingInfo = result.PagingInfo;
            Assert.Equal(2, pagingInfo.CurrentPage);
            Assert.Equal(3, pagingInfo.ItemsPerPage);
            Assert.Equal(5, pagingInfo.TotalItems);
            Assert.Equal(2, pagingInfo.TotalPages);
        }
    }
}