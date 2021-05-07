using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using ThemeMeUp.Core.Boundaries;
using ThemeMeUp.Core.Boundaries.GetLatestWallpapers;
using ThemeMeUp.Core.Entities;
using ThemeMeUp.Mobile.Services;
using Xamarin.CommunityToolkit.ObjectModel;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace ThemeMeUp.Mobile.ViewModels
{
    public class MainPageViewModel : BaseViewModel
    {
        private readonly INavigationService _navigationService;
        private readonly IGetLatestWallpapersUseCase _useCase;
        private readonly LatestWallpapersPresenter _presenter;
        private readonly IWallpaperSetter _wallpaperSetter;
        private readonly IUserSettingsService _settings;

        #region Commands

        public IAsyncCommand RefreshCommand { get; }
        public IAsyncCommand OpenFilterPageCommand { get; }
        public IAsyncCommand<string> ShareWallpaperCommand { get; }
        public IAsyncCommand<string> SetWallpaperCommand { get; }
        public IAsyncCommand OpenSettingsPageCommand { get; }
        public IAsyncCommand<Wallpaper> OpenWallpaperDetailPageCommand { get; }

        #endregion

        public MainPageViewModel(INavigationService navigationService, IGetLatestWallpapersUseCase useCase, IGetLatestWallpapersOutputPort presenter, IWallpaperSetter wallpaperSetter, IUserSettingsService settings)
        {
            _navigationService = navigationService;
            _useCase = useCase;
            _presenter = (LatestWallpapersPresenter)presenter;
            _wallpaperSetter = wallpaperSetter;
            _settings = settings;

            Title = "Theme Me Up";
            LoadSettings();

            RefreshCommand = new AsyncCommand(RefreshAsync, CanExecute);
            OpenFilterPageCommand = new AsyncCommand(OpenFilterPageAsync, CanExecute);
            ShareWallpaperCommand = new AsyncCommand<string>(OpenSharePromptAsync, CanExecute);
            SetWallpaperCommand = new AsyncCommand<string>(SetWallpaperAsync, CanExecute);
            OpenSettingsPageCommand = new AsyncCommand(OpenSettingsPageAsync, CanExecute);
            OpenWallpaperDetailPageCommand = new AsyncCommand<Wallpaper>(OpenWallpaperDetailPageAsync, CanExecute);

            Wallpapers = new ObservableCollection<Wallpaper>();
        }

        private void LoadSettings()
        {
            LoadFullImageInPreviewSetting = _settings.LoadFullImageInPreview;
        }

        private async Task SetWallpaperAsync(string wallpaperUrl)
        {
            try
            {
                IsBusy = true;

                if (IsSettingWallpaper)
                {
                    await Application.Current.MainPage.DisplayAlert("Processing", "Wallpaper setting in process, please wait.", "OK");
                    return;
                }

                IsSettingWallpaper = true;

                await Application.Current.MainPage.DisplayAlert("Setting a wallpaper", "We will let you know as soon as your wallpaper is downloaded and set.", "OK");
                await Task.Run(() =>
                {
                    _wallpaperSetter.SetFromUrlAsync(wallpaperUrl);
                }).ContinueWith(t =>
                {
                    IsSettingWallpaper = false;
                });

                await Application.Current.MainPage.DisplayAlert("Success", "Your wallpaper was set.", "OK");
            }
            finally
            {
                IsBusy = false;
            }
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

        private async Task OpenWallpaperDetailPageAsync(Wallpaper wallpaper)
        {
            try
            {
                IsBusy = true;
                await _navigationService.OpenWallpaperDetailPageAsync(wallpaper);
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

        private bool _isSettingWallpaper;
        public bool IsSettingWallpaper
        {
            get => _isSettingWallpaper;
            set
            {
                _isSettingWallpaper = value;
                OnPropertyChanged();
            }
        }

        private bool _loadFullImageInPreviewSetting;
        public bool LoadFullImageInPreviewSetting
        {
            get => _loadFullImageInPreviewSetting;
            set
            {
                _loadFullImageInPreviewSetting = value;
                OnPropertyChanged();
            }
        }

        #endregion
    }
}