using System;
using System.Linq;
using System.Threading.Tasks;
using ThemeMeUp.Core.Boundaries.GetLatestWallpapers;
using Xamarin.Forms;

namespace ThemeMeUp.Mobile
{
    public partial class MainPage : ContentPage
    {
        private readonly IGetLatestWallpapersUseCase _useCase;
        private readonly LatestWallpaperPresenter _presenter;

        public MainPage()
        {
            InitializeComponent();

            _presenter = new LatestWallpaperPresenter();
            _useCase = UseCaseFactory.CreateGetLatestWallpapersUseCase(_presenter);

            labelDebug.Text = "Presenter and a use case created.";
        }

        private void Execute_Clicked(object sender, System.EventArgs e)
        {
            labelDebug.Text = "Running UseCase...";

            _useCase.Execute(new GetLatestWallpapersInput());
        }

        private void Resolve_Clicked(object sender, System.EventArgs e)
        {
            labelDebug.Text = "Resolving...";

            if (_presenter.Wallpapers.Any())
            {
                labelDebug.Text = $"The UseCase execution finished. Fetched {_presenter.Wallpapers.Count} wallpaper(s). - {_presenter.Wallpapers.First().SmallThumbnailUrl}";
                imageDebug.Source = ImageSource.FromUri(new Uri(_presenter.Wallpapers.First().SmallThumbnailUrl));
            }
            else
            {
                labelDebug.Text = $"The UseCase execution finished. Fetched {_presenter.Wallpapers.Count} wallpaper(s).";
            }
        }
    }
}
