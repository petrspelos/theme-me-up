using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Moq;
using ThemeMeUp.ApiWrapper;
using ThemeMeUp.ApiWrapper.Entities.Api;
using ThemeMeUp.ApiWrapper.Entities.Requests;
using ThemeMeUp.Core.Boundaries;
using ThemeMeUp.Core.Boundaries.GetLatestWallpapers;
using ThemeMeUp.Core.Entities;
using ThemeMeUp.Core.UseCases;
using Xunit;

namespace ThemeMeUp.Tests.UseCases
{
    public class GetLatestWallpapersTests
    {
        private readonly Mock<IGetLatestWallpapersOutputPort> _output;
        private readonly Mock<IWallhavenClient> _clientMock;
        private readonly IWallpaperProvider _wallpaperProvider;
        private readonly IGetLatestWallpapersUseCase _useCase;

        public GetLatestWallpapersTests()
        {
            _output = new Mock<IGetLatestWallpapersOutputPort>();
            _clientMock = new Mock<IWallhavenClient>();
            _wallpaperProvider = new WallpaperProvider(_clientMock.Object);
            _useCase = new GetLatestWallpapersUseCase(_wallpaperProvider, _output.Object);
        }

        [Fact]
        public async Task NoConnection_ShouldOutputError()
        {
            SetNoConnection();

            await _useCase.Execute(new GetLatestWallpapersInput());

            AssertNoConnectionOutput();
            AssertNoOtherOutput();
        }

        [Fact]
        public async Task SimpleResponse_ShouldOutputWallpapers()
        {
            SetSimpleWallpaperResponse();

            await _useCase.Execute(new GetLatestWallpapersInput());

            AssertSimpleDefaultOutput();
            AssertNoOtherOutput();
        }

        [Fact]
        public async Task UnauthenticatedNSFW_ShouldOutputUnauthorized()
        {
            await _useCase.Execute(new GetLatestWallpapersInput
            {
                Nsfw = true
            });

            AssertUnauthenticatedOutput();
            AssertNoOtherOutput();
        }

        private void AssertUnauthenticatedOutput()
        {
            _output.Verify(o => o.Unauthenticated(), Times.Once());
        }

        private void AssertSimpleDefaultOutput()
        {
            _output.Verify(o => o.Default(It.Is<IEnumerable<Wallpaper>>(arg => SimpleWallpaperSet(arg))), Times.Once());
        }

        private bool SimpleWallpaperSet(IEnumerable<Wallpaper> arg)
        {
            if (arg.Count() != 2) { return false; }

            for (var i = 0; i < arg.Count(); i++)
            {
                var item = arg.ElementAt(i);
                if (item.FullImageUrl != $"FULL WALLPAPER URL {i + 1}" || item.SmallThumbnailUrl != $"SMALL THUMBNAIL URL {i + 1}")
                {
                    return false;
                }
            }

            return true;
        }

        private void SetSimpleWallpaperResponse()
        {
            IEnumerable<WallpaperResponse> wallpapers = new WallpaperResponse[] {
                new WallpaperResponse
                {
                    Path = "FULL WALLPAPER URL 1",
                    Thumbs = new Thumbnails
                    {
                        Small = "SMALL THUMBNAIL URL 1"
                    }
                },
                new WallpaperResponse
                {
                    Path = "FULL WALLPAPER URL 2",
                    Thumbs = new Thumbnails
                    {
                        Small = "SMALL THUMBNAIL URL 2"
                    }
                }
            };

            _clientMock.Setup(c => c.GetLatestWallpapersAsync(It.IsAny<QueryOptions>(), false)).Returns(Task.FromResult(wallpapers));
        }

        private void AssertNoOtherOutput() => _output.VerifyNoOtherCalls();

        private void AssertNoConnectionOutput()
            => _output.Verify(o => o.NoConnection(), Times.Once());

        private void SetNoConnection()
            => _clientMock.Setup(c => c.GetLatestWallpapersAsync(It.IsAny<QueryOptions>(), false)).Throws<HttpRequestException>();
    }
}
