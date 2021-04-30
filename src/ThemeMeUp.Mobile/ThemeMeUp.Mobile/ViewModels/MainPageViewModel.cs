using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using ThemeMeUp.Core.Boundaries.GetLatestWallpapers;
using ThemeMeUp.Core.Entities;
using Xamarin.CommunityToolkit.ObjectModel;

namespace ThemeMeUp.Mobile.ViewModels
{
    public class MainPageViewModel : BaseViewModel
    {

        private readonly IGetLatestWallpapersUseCase _useCase;
        private readonly LatestWallpaperPresenter _presenter;

        #region Commands

        public IAsyncCommand RefreshCommand { get; }

        #endregion

        public MainPageViewModel(IGetLatestWallpapersUseCase useCase, LatestWallpaperPresenter presenter)
        {
            _useCase = useCase;
            _presenter = presenter;

            Title = "Theme Me Up";

            RefreshCommand = new AsyncCommand(RefreshAsync, CanExecute);

            Wallpapers = new ObservableCollection<Wallpaper>();
        }

        private async Task RefreshAsync()
        {
            try
            {
                IsRefreshing = true;

                Wallpapers.Clear();
                await GetLatestWallpapersAsync();
            }
            finally
            {
                IsRefreshing = false;
            }
           
        }

        public async Task GetLatestWallpapersAsync()
        {
            try
            {
                IsBusy = true;

                await Task.Run(async () =>
                {
                    await _useCase.Execute(new GetLatestWallpapersInput());
                });

                foreach (var wallpaper in _presenter.Wallpapers)
                    Wallpapers.Add(wallpaper);
            }
            finally
            {
                IsBusy = false;
            }
        }

        private bool CanExecute(object arg)
        {
            return !IsBusy && !IsRefreshing;
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