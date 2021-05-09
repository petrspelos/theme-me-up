using System.Threading.Tasks;
using ThemeMeUp.Core.Boundaries.Infrastructure;
using ThemeMeUp.Mobile.Resx;
using ThemeMeUp.Mobile.Services;
using Xamarin.CommunityToolkit.ObjectModel;
using Xamarin.Forms;

namespace ThemeMeUp.Mobile.ViewModels
{
    public class SettingsPageViewModel : BaseViewModel
    {
        private readonly IAuthentication _authentication;
        private readonly IUserSettingsService _settings;

        #region Commands

        public IAsyncCommand SetTokenCommand { get; }

        #endregion

        public SettingsPageViewModel(IAuthentication authentication, IUserSettingsService settings)
        {
            _authentication = authentication;
            _settings = settings;

            Title = AppResources.SettingsLabel;
            LoadSettings();

            SetTokenCommand = new AsyncCommand(SetToken, CanExecute);
        }

        private void LoadSettings()
        {
            Token = _authentication.GetApiKey();
            LoadFullImageInPreview = _settings.LoadFullImageInPreview;
        }

        private async Task SetToken()
        {
            if (string.IsNullOrWhiteSpace(Token))
            {
                await Application.Current.MainPage.DisplayAlert(AppResources.NoTokenFoundLabel, "Token cannot be empty.", "OK");
                return;
            }

            _authentication.SetApiKey(Token);

            await Application.Current.MainPage.DisplayAlert(AppResources.TokenAddedLabel, "Your token was successfully added.", "OK");
        }

        private bool CanExecute()
        {
            return !IsBusy && !IsRefreshing;
        }

        #region Properties

        private string _token;
        public string Token
        {
            get => _token;
            set
            {
                _token = value;
                OnPropertyChanged();
            }
        }

        private bool _loadFullImageInPreview;
        public bool LoadFullImageInPreview
        {
            get => _loadFullImageInPreview;
            set
            {
                _settings.LoadFullImageInPreview = value;
                _loadFullImageInPreview = value;
                OnPropertyChanged();
            }
        }

        #endregion
    }
}