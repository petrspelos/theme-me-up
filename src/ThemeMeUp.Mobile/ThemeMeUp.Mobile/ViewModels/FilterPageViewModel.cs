using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ThemeMeUp.Core.Boundaries.GetLatestWallpapers;
using ThemeMeUp.Core.Boundaries.Infrastructure;
using ThemeMeUp.Core.Entities.Sorting;
using ThemeMeUp.Mobile.Resx;
using Xamarin.CommunityToolkit.ObjectModel;
using Xamarin.Forms;

namespace ThemeMeUp.Mobile.ViewModels
{
    public class FilterPageViewModel : BaseViewModel
    {
        private readonly IAuthentication _authentication;
        private readonly MainPageViewModel _viewModel;

        #region Commands

        public IAsyncCommand ApplyFilterCommand { get; }

        #endregion

        public FilterPageViewModel(MainPageViewModel viewModel, IAuthentication authentication)
        {
            _viewModel = viewModel;
            _authentication = authentication;

            Title = AppResources.FiltersLabel;

            ApplyFilterCommand = new AsyncCommand(ApplyFiltersAsync, CanExecute);

            SortItems = new[]
            {
                AppResources.LatestLabel,
                AppResources.TopOneDayLabel,
                AppResources.TopThreeDayLabel,
                AppResources.TopOneWeekLabel,
                AppResources.TopOneMonthLabel,
                AppResources.TopThreeMonthLabel,
                AppResources.TopSixMonthLabel,
                AppResources.TopOneYearLabel
            };
            SelectedSortIndex = 0;
        }

        private async Task ApplyFiltersAsync()
        {
            try
            {
                IsBusy = true;

                if (IncludeNsfw)
                {
                    if (string.IsNullOrEmpty(_authentication.GetApiKey()))
                    {
                        await Application.Current.MainPage.DisplayAlert(AppResources.NoTokenFoundLabel, AppResources.TokenNeedsToBeAddedLabel, AppResources.OkLabel);
                        IncludeNsfw = false;
                        return;
                    }
                }

                _viewModel.WallpapersInput = new GetLatestWallpapersInput
                {
                    SearchTerm = SearchTerm,
                    Sfw = IncludeSfw,
                    Sketchy = IncludeSketchy,
                    Nsfw = IncludeNsfw,
                    General = IncludeGeneral,
                    Anime = IncludeAnime,
                    People = IncludePeople,
                    Sort = SortFromInt(SelectedSortIndex)
                };

                await Application.Current.MainPage.Navigation.PopAsync();

                _viewModel.Wallpapers.Clear();
            }
            finally
            {
                IsBusy = false;
            }
        }

        private IWallpaperSort SortFromInt(int val)
        {
            return val switch
            {
                1 => new TopSort(TopSortRange.Day),
                2 => new TopSort(TopSortRange.ThreeDays),
                3 => new TopSort(TopSortRange.Week),
                4 => new TopSort(TopSortRange.Month),
                5 => new TopSort(TopSortRange.ThreeMonths),
                6 => new TopSort(TopSortRange.HalfYear),
                7 => new TopSort(TopSortRange.Year),
                _ => new LatestSort()
            };
        }

        private bool CanExecute(object arg)
        {
            return !IsBusy;
        }

        #region Properties

        private string _searchTerm;
        public string SearchTerm
        {
            get => _searchTerm;
            set
            {
                _searchTerm = value;
                OnPropertyChanged();
            }
        }

        private bool _includeGeneral;
        public bool IncludeGeneral
        {
            get => _includeGeneral;
            set
            {
                _includeGeneral = value;
                OnPropertyChanged();
            }
        }

        private bool _includeAnime;
        public bool IncludeAnime
        {
            get => _includeAnime;
            set
            {
                _includeAnime = value;
                OnPropertyChanged();
            }
        }

        private bool _includePeople;
        public bool IncludePeople
        {
            get => _includePeople;
            set
            {
                _includePeople = value;
                OnPropertyChanged();
            }
        }

        private bool _includeSfw;
        public bool IncludeSfw
        {
            get => _includeSfw;
            set
            {
                _includeSfw = value;
                OnPropertyChanged();
            }
        }

        private bool _includeSketchy;
        public bool IncludeSketchy
        {
            get => _includeSketchy;
            set
            {
                _includeSketchy = value;
                OnPropertyChanged();
            }
        }

        private bool _includeNsfw;
        public bool IncludeNsfw
        {
            get => _includeNsfw;
            set
            {
                _includeNsfw = value;
                OnPropertyChanged();
            }
        }

        private IEnumerable<string> _sortItems;
        public IEnumerable<string> SortItems
        {
            get => _sortItems;
            set
            {
                _sortItems = value;
                OnPropertyChanged();
            }
        }

        private int _selectedSortIndex;
        public int SelectedSortIndex
        {
            get => _selectedSortIndex;
            set
            {
                _selectedSortIndex = value;
                OnPropertyChanged();
            }
        }

        #endregion
    }
}