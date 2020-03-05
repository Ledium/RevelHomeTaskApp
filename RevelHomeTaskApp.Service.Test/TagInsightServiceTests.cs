using RevelHomeTaskApp.Service.Utilities;
using Xunit;

namespace RevelHomeTaskApp.Service.Test
{
   public class TagInsightServiceTests
    {
        [Fact]
        public void LoadPage_ShouldLoadHtmlWebPageData()
        {
            //Act
            var actual = WebPageHelper.LoadPage("http://www.facebook.com");

            //Assert
            Assert.NotNull(actual);
        }

        [Fact]
        public void LoadPage_ShouldLoadXmlWebPageData()
        {
            //Act
            var actual = WebPageHelper.LoadPage("https://www.w3schools.com/xml/plant_catalog.xml");

            //Assert
            Assert.NotNull(actual);
        }

        [Theory]
        [InlineData("http://www.facebook.com")]
        [InlineData("https://www.w3schools.com/xml/plant_catalog.xml")]
        public void GetTags_ShouldGetACollectionOfTags(string url)
        {
            //Arrange
            var service = new TagInsightsService();

            //Act
            var actual = service.GetTags(url);

            //Assert
            Assert.NotEmpty(actual);
        }

        [Theory]
        [InlineData("http://www.facebook.com")]
        [InlineData("https://www.w3schools.com/xml/plant_catalog.xml")]
        public void FindMostPopularTag_ShouldReturnTagWithCount(string url)
        {
            //Arrange
            var service = new TagInsightsService();

            var tags = service.GetTags(url);

            //Act
            var actual = service.FindMostPopularTag(tags);

            //Assert
            Assert.NotNull(tags);
            Assert.True(actual.Value > 0 );
        }

        [Theory]
        [InlineData("http://www.facebook.com")]
        [InlineData("https://www.w3schools.com/xml/plant_catalog.xml")]
        public void FindPaths_ShoudldReturnValidCollectionOfPaths(string url)
        {
            //Arrange
            var service = new TagInsightsService();

            var tagss = service.GetTags(url);

            var maxTag = service.FindMostPopularTag(tagss);

            //Act
            var actual = service.FindPaths(tagss, maxTag);

            //Assert
            foreach(var item in actual)
            {
                Assert.True(!string.IsNullOrEmpty(item.XPath));
                Assert.True(item.LevelCount > 0 && item.TagOccurrences > 0);
            }
        }
    }
}
