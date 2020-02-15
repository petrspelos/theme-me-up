using ThemeMeUp.ConsoleApp;
using Xunit;

namespace ThemeMeUp.Tests
{
    public class QueryBuilderTests
    {
        private readonly WallhavenQueryBuilder _builder;

        private const string BaseSearch = Constants.WallhavenRootUrl + Constants.SearchOptionsRoot + Constants.SortingRandomOption;

        public QueryBuilderTests()
        {
            _builder = new WallhavenQueryBuilder();
        }

        [Fact]
        public void NoAdditionalOptions_ShouldReturnBaseSearch()
        {
            Assert.Equal(BaseSearch, _builder.BuildUrl());
        }

        [Theory]
        [InlineData(1280, 720)]
        public void MinimumResolution_ShouldAppendResolutionTerm(int width, int height)
        {
            var expected = BaseSearch + Constants.OptionSeparator + string.Format(Constants.MinResolutionOptionTemplate, width, height);
            
            var actual = _builder.MinimumResolution(width, height).BuildUrl();

            Assert.Equal(expected, actual);
        }
    }
}
