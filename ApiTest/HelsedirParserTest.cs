using System.Threading.Tasks;
using Api;
using Api.Models.Workplace;
using FakeItEasy;
using Newtonsoft.Json;
using Xunit;

namespace ApiTest
{
    public class HelsedirParserTest
    {
        private readonly IHelsedirMenuFetcher _helsedirMenuFetcherMock;


        public HelsedirParserTest()
        {
            _helsedirMenuFetcherMock = A.Fake<IHelsedirMenuFetcher>();

            var testData = JsonConvert.DeserializeObject<WorkplaceResponse>(TestData.HelseDirData);

            A.CallTo(() => _helsedirMenuFetcherMock.ReadMenu()).Returns(testData);
        }

        [Fact]
        public async Task FetchMenu_Finds5Days()
        {
            // Arrange
            var parser = new HelsedirMenuService(_helsedirMenuFetcherMock);

            // Act
            var menu = await parser.FetchMenu();

            // Assert
            Assert.Equal(5, menu.Count);
        }
    }
}
