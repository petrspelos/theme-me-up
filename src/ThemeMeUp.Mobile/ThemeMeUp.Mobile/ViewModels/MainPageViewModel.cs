using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using ThemeMeUp.Core.Boundaries.GetLatestWallpapers;
using ThemeMeUp.Core.Entities;

namespace ThemeMeUp.Mobile.ViewModels
{
    public class MainPageViewModel : BaseViewModel
    {

        private readonly IGetLatestWallpapersUseCase _useCase;
        private readonly LatestWallpaperPresenter _presenter;

        public MainPageViewModel(IGetLatestWallpapersUseCase useCase, LatestWallpaperPresenter presenter)
        {
            _useCase = useCase;
            _presenter = presenter;

            Title = "Theme Me Up";

            Wallpapers = new ObservableCollection<Wallpaper>();
        }

        public async Task GetLatestWallpapersAsync()
        {
            try
            {
                IsBusy = true;

                var task = Task.Run(async () =>
                {
                    await _useCase.Execute(new GetLatestWallpapersInput());
                });

                Task.WaitAll(task);

                foreach (var wallpaper in _presenter.Wallpapers)
                    Wallpapers.Add(wallpaper);
            }
            finally
            {
                IsBusy = false;
            }
        }

        #region Properties

        private ObservableCollection<Wallpaper> _wallpapers;

        public ObservableCollection<Wallpaper> Wallpapers
        {
            get => _wallpapers;
            set
            {
                _wallpapers = value;
                OnPropertyChanged();
            }
        }

        #endregion
    }
}

/*
   _presenter = new LatestWallpaperPresenter();
            _useCase = UseCaseFactory.CreateGetLatestWallpapersUseCase(_presenter);
*/