using System.Threading.Tasks;
using ThemeMeUp.Core.Boundaries.Infrastructure;
using Xamarin.CommunityToolkit.ObjectModel;
using Xamarin.Forms;

namespace ThemeMeUp.Mobile.ViewModels
{
    public class SettingsPageViewModel : BaseViewModel
    {
        private readonly IAuthentication _authentication;

        #region Commands

        public Command SetTokenCommand { get; }

        #endregion

        public SettingsPageViewModel(IAuthentication authentication)
        {
            _authentication = authentication;

            Title = "Settings";

            SetTokenCommand = new Command(SetToken, CanExecute);
        }

        private void SetToken()
        {
            _authentication.SetApiKey(Token);
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

        #endregion
    }
}