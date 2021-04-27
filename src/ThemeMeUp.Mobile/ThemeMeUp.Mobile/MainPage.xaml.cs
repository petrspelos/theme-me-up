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
    }
}
