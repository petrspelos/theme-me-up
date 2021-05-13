using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Threading.Tasks;
using ThemeMeUp.Core.Boundaries;
using ThemeMeUp.Core.Boundaries.GetLatestWallpapers;
using ThemeMeUp.Core.Entities;
using ThemeMeUp.Mobile.Resx;
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
        public IAsyncCommand LoadMoreWallpapersCommand { get; }

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
            LoadMoreWallpapersCommand = new AsyncCommand(LoadMoreWallpapersAsync, CanExecute);

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
                    await Application.Current.MainPage.DisplayAlert(AppResources.ProcessingLabel, AppResources.WallpaperInstallationLabel, AppResources.OkLabel);
                    return;
                }

                IsSettingWallpaper = true;

                await Application.Current.MainPage.DisplayAlert(AppResources.ProcessingLabel, AppResources.WallpaperInstallationInfoLabel, AppResources.OkLabel);
                await Task.Run(() =>
                {
                    _wallpaperSetter.SetFromUrlAsync(wallpaperUrl);
                }).ContinueWith(t =>
                {
                    IsSettingWallpaper = false;
                });

                await Application.Current.MainPage.DisplayAlert(AppResources.SuccessLabel, AppResources.WallpaperSetLabel, AppResources.OkLabel);
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
                await LoadWallpapersAsync();
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

        private const int PageSize = 24;
        private int _currentFlightIndex = 0;

        private async Task LoadMoreWallpapersAsync()
        {
            try
            {
                IsBusy = true;

                var page = Wallpapers.Count / PageSize;

                var filters = new GetLatestWallpapersInput
                {
                    SearchTerm = WallpapersInput.SearchTerm,
                    Sfw = WallpapersInput.Sfw,
                    Sketchy = WallpapersInput.Sketchy,
                    Nsfw = WallpapersInput.Nsfw,
                    General = WallpapersInput.General,
                    Anime = WallpapersInput.Anime,
                    People = WallpapersInput.People,
                    Sort = WallpapersInput.Sort,
                    Page = (ulong)++page
                };

                await _useCase.Execute(filters);

                foreach (var wallpaper in _presenter.Wallpapers)
                    Wallpapers.Add(wallpaper);

                _currentFlightIndex += PageSize;

            }
            finally
            {
                IsBusy = false;
            }
        }

        public async Task LoadWallpapersAsync()
        {
            try
            {
                IsBusy = true;
                await LoadMoreWallpapersAsync();
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

        public GetLatestWallpapersInput WallpapersInput = new GetLatestWallpapersInput();

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