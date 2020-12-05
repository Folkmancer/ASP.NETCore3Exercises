using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Routing;
using Microsoft.AspNetCore.Razor.TagHelpers;
using Moq;
using SportsStore.Infrastructure;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;

namespace SportsStore.Tests
{
    public class PageLinkTagHelperTests
    {
        [Fact]
        public void CanGeneratePageLinks()
        {
            //Arrange
            var urlHelper = new Mock<IUrlHelper>();
            urlHelper.SetupSequence(x => x.Action(It.IsAny<UrlActionContext>()))
                .Returns("Test/Page1")
                .Returns("Test/Page2")
                .Returns("Test/Page3");

            var urlHelperFactory = new Mock<IUrlHelperFactory>();
            urlHelperFactory.Setup(f => f.GetUrlHelper(It.IsAny<ActionContext>()))
                .Returns(urlHelper.Object);

            var PageLinkTagHelper = new PageLinkTagHelper(urlHelperFactory.Object)
            {
                PageModel = new Models.ViewModels.PagingInfo()
                {
                    CurrentPage = 2,
                    TotalItems = 28,
                    ItemsPerPage = 10
                },
                PageAction = "Test"
            };

            var context = new TagHelperContext(new TagHelperAttributeList(), new Dictionary<object, object>(), "");
            var content = new Mock<TagHelperContent>();
            var output = new TagHelperOutput("div", new TagHelperAttributeList(), (cache, encode) => Task.FromResult(content.Object));

            //Act
            PageLinkTagHelper.Process(context, output);

            // Assert
            Assert.Equal(@"<a href=""Test/Page1"">1</a>"
                + @"<a href=""Test/Page2"">2</a>"
                + @"<a href=""Test/Page3"">3</a>",
                output.Content.GetContent());
        }
    }
}