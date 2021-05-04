using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using ThemeMeUp.Core.Boundaries.GetLatestWallpapers;
using ThemeMeUp.Core.Entities;
using ThemeMeUp.Mobile.Services;
using Xamarin.CommunityToolkit.ObjectModel;
using Xamarin.Essentials;

namespace ThemeMeUp.Mobile.ViewModels
{
    public class MainPageViewModel : BaseViewModel
    {
        private readonly INavigationService _navigationService;
        private readonly IGetLatestWallpapersUseCase _useCase;
        private readonly LatestWallpapersPresenter _presenter;

        #region Commands

        public IAsyncCommand RefreshCommand { get; }
        public IAsyncCommand OpenFilterPageCommand { get; }
        public IAsyncCommand<string> ShareWallpaperCommand { get; }
        public IAsyncCommand OpenSettingsPageCommand { get; }

        #endregion

        public MainPageViewModel(INavigationService navigationService, IGetLatestWallpapersUseCase useCase, IGetLatestWallpapersOutputPort presenter)
        {
            _navigationService = navigationService;
            _useCase = useCase;
            _presenter = (LatestWallpapersPresenter)presenter;

            Title = "Theme Me Up";

            RefreshCommand = new AsyncCommand(RefreshAsync, CanExecute);
            OpenFilterPageCommand = new AsyncCommand(OpenFilterPageAsync, CanExecute);
            ShareWallpaperCommand = new AsyncCommand<string>(OpenSharePromptAsync, CanExecute);
            OpenSettingsPageCommand = new AsyncCommand(OpenSettingsPageAsync, CanExecute);

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

        private async Task OpenFilterPageAsync()
        {
            try
            {
                IsBusy = true;
                await _navigationService.OpenFilterPageAsync();
            }
            finally
            {
                IsBusy = false;
            }
        }

        private async Task OpenSharePromptAsync(string wallpaperUrl)
        {
            try
            {
                IsBusy = true;

                await Share.RequestAsync(new ShareTextRequest
                {
                    Uri = wallpaperUrl,
                    Title = "Share Wallpaper"
                });
            }
            finally
            {
                IsBusy = false;
            }
        }

        private async Task OpenSettingsPageAsync()
        {
            try
            {
                IsBusy = true;
                await _navigationService.OpenSettingsPageAsync();
            }
            finally
            {
                IsBusy = false;
            }
        }

        public async Task GetLatestWallpapersAsync()
        {
            try
            {
                IsBusy = true;

                await _useCase.Execute(new GetLatestWallpapersInput());
            

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