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

            var task = Task.Run(async () => { await _useCase.Execute(new GetLatestWallpapersInput()); });
            Task.WaitAll(task);

            collectionViewDebug.ItemsSource = _presenter.Wallpapers;

            labelDebug.Text = "Finished.";
        }
    }
}
