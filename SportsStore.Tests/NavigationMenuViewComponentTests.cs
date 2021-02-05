using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewComponents;
using Microsoft.AspNetCore.Routing;
using Moq;
using SportsStore.Components;
using SportsStore.Models;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace SportsStore.Tests
{
    public class NavigationMenuViewComponentTests
    {
        [Fact]
        public void CanSelectCategories()
        {
            //Arrange
            var mockRepository = new Mock<IStoreRepository>();
            var expectedCategories = new[] { "Apples", "Oranges", "Plums" };
            var products = new Product[]
            {
                new Product { ProductId = 1, Name = "P1", Category = "Apples"},
                new Product { ProductId = 2, Name = "P2", Category = "Apples"},
                new Product { ProductId = 3, Name = "P3", Category = "Plums" },
                new Product { ProductId = 4, Name = "P4", Category = "Oranges" },
            };

            mockRepository.Setup(repository => repository.Products)
                .Returns(products.AsQueryable());

            var viewComponent = new NavigationMenuViewComponent(mockRepository.Object);

            //Act
            var result = ((IEnumerable<string>)(viewComponent.Invoke() as ViewViewComponentResult).ViewData.Model)
                .ToArray();

            //Assert
            Assert.True(Enumerable.SequenceEqual(expectedCategories, result));
        }

        [Fact]
        public void IndicatesSelectedCategory()
        {
            //Arrange
            var categoryToSelect = "Apples";
            var mockRepository = new Mock<IStoreRepository>();
            var expectedCategories = new[] { "Apples", "Oranges", "Plums" };
            var expectedProducts = new Product[]
            {
                new Product { ProductId = 1, Name = "P1", Category = "Apples"},
                new Product { ProductId = 2, Name = "P2", Category = "Apples"},
                new Product { ProductId = 3, Name = "P3", Category = "Plums" },
                new Product { ProductId = 4, Name = "P4", Category = "Oranges" },
            };

            mockRepository.Setup(repository => repository.Products)
                .Returns(expectedProducts.AsQueryable());

            var viewComponent = new NavigationMenuViewComponent(mockRepository.Object)
            {
                ViewComponentContext = new ViewComponentContext()
                {
                    ViewContext = new ViewContext()
                    {
                        RouteData = new RouteData()
                    }
                }
            };
            viewComponent.RouteData.Values["category"] = categoryToSelect;

            //Act
            var result = (string)((viewComponent.Invoke() as ViewViewComponentResult).ViewData["SelectedCategory"]);

            //Assert
            Assert.Equal(categoryToSelect, result);
        }
    }
}