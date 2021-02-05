using Microsoft.AspNetCore.Mvc;
using Moq;
using SportsStore.Controllers;
using SportsStore.Models;
using SportsStore.Models.ViewModels;
using System;
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
            var mockRepository = new Mock<IStoreRepository>();
            var expectedProducts = new[]
            {
                new Product {ProductId = 1, Name = "P1"},
                new Product {ProductId = 2, Name = "P2"}
            };

            mockRepository.Setup(repository => repository.Products)
                .Returns(expectedProducts.AsQueryable());

            var homeController = new HomeController(mockRepository.Object);

            //Act
            var result = homeController.Index(null)
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
            var mockRepository = new Mock<IStoreRepository>();
            var expectedProducts = new[]
            {
                new Product {ProductId = 1, Name = "P1"},
                new Product {ProductId = 2, Name = "P2"},
                new Product {ProductId = 3, Name = "P3"},
                new Product {ProductId = 4, Name = "P4"},
                new Product {ProductId = 5, Name = "P5"}
            };

            mockRepository.Setup(repository => repository.Products)
                .Returns(expectedProducts.AsQueryable());

            var homeController = new HomeController(mockRepository.Object)
            {
                PageSize = 3
            };

            //Act
            var result = homeController.Index(null, 2)
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
            var mockRepository = new Mock<IStoreRepository>();
            var expectedProducts = new[]
            {
                new Product {ProductId = 1, Name = "P1"},
                new Product {ProductId = 2, Name = "P2"},
                new Product {ProductId = 3, Name = "P3"},
                new Product {ProductId = 4, Name = "P4"},
                new Product {ProductId = 5, Name = "P5"}
            };

            mockRepository.Setup(repository => repository.Products)
                .Returns(expectedProducts.AsQueryable());

            var homeController = new HomeController(mockRepository.Object)
            {
                PageSize = 3
            };

            //Act
            var result = homeController.Index(null, 2)
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

        [Fact]
        public void CanFilterProducts()
        {
            //Arrange
            var mockRepository = new Mock<IStoreRepository>();
            var expectedCategory = "Cat2";
            var expectedProducts = new[]
            {
                new Product {ProductId = 1, Name = "P1", Category = "Cat1"},
                new Product {ProductId = 2, Name = "P2", Category = "Cat2"},
                new Product {ProductId = 3, Name = "P3", Category = "Cat1"},
                new Product {ProductId = 4, Name = "P4", Category = "Cat2"},
                new Product {ProductId = 5, Name = "P5", Category = "Cat3"}
            };

            mockRepository.Setup(repository => repository.Products)
                .Returns(expectedProducts.AsQueryable());

            var homeController = new HomeController(mockRepository.Object)
            {
                PageSize = 3
            };

            //Act
            var result = (homeController.Index(expectedCategory, 1)
                .ViewData.Model as ProductsListViewModel)
                .Products.ToArray();

            //Assert
            Assert.Equal(2, result.Length);
            Assert.True(result[0].Name == expectedProducts[1].Name && result[0].Category == expectedCategory);
            Assert.True(result[1].Name == expectedProducts[3].Name && result[1].Category == expectedCategory);
        }

        [Fact]
        public void GenerateCategorySpecificProductCount()
        {
            //Arrange
            var mockRepository = new Mock<IStoreRepository>();
            var expectedProducts = new[]
            {
                new Product {ProductId = 1, Name = "P1", Category = "Cat1"},
                new Product {ProductId = 2, Name = "P2", Category = "Cat2"},
                new Product {ProductId = 3, Name = "P3", Category = "Cat1"},
                new Product {ProductId = 4, Name = "P4", Category = "Cat2"},
                new Product {ProductId = 5, Name = "P5", Category = "Cat3"}
            };

            mockRepository.Setup(repository => repository.Products)
                .Returns(expectedProducts.AsQueryable());

            var homeController = new HomeController(mockRepository.Object)
            {
                PageSize = 3
            };

            Func<ViewResult, int?> GetModel = 
                result => (result?.ViewData?.Model as ProductsListViewModel)
                    ?.PagingInfo
                    ?.TotalItems; 

            //Act
            var resultCat1 = GetModel(homeController.Index("Cat1"));
            var resultCat2 = GetModel(homeController.Index("Cat2"));
            var resultCat3 = GetModel(homeController.Index("Cat3"));
            var resultAll = GetModel(homeController.Index(null));

            //Assert
            Assert.Equal(2, resultCat1);
            Assert.Equal(2, resultCat2);
            Assert.Equal(1, resultCat3);
            Assert.Equal(5, resultAll);
        }
    }
}