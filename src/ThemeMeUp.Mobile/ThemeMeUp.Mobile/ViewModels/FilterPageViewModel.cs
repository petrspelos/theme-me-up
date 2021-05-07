using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ThemeMeUp.Core.Boundaries.GetLatestWallpapers;
using ThemeMeUp.Core.Boundaries.Infrastructure;
using ThemeMeUp.Core.Entities.Sorting;
using Xamarin.CommunityToolkit.ObjectModel;
using Xamarin.Forms;

namespace ThemeMeUp.Mobile.ViewModels
{
    public class FilterPageViewModel : BaseViewModel
    {
        private readonly IGetLatestWallpapersUseCase _useCase;
        private readonly IAuthentication _authentication;
        private readonly LatestWallpapersPresenter _presenter;
        private readonly MainPageViewModel _viewModel;

        #region Commands

        public IAsyncCommand ApplyFilterCommand { get; }

        #endregion

        public FilterPageViewModel(IGetLatestWallpapersUseCase useCase, IGetLatestWallpapersOutputPort presenter, MainPageViewModel viewModel, IAuthentication authentication)
        {
            _useCase = useCase;
            _presenter = (LatestWallpapersPresenter)presenter;
            _viewModel = viewModel;
            _authentication = authentication;

            Title = "Filters";

            ApplyFilterCommand = new AsyncCommand(ApplyFiltersAsync, CanExecute);

            SortItems = new[]
            {
                "Latest",
                "Top (1 day)",
                "Top (3 days)",
                "Top (1 week)",
                "Top (1 month)",
                "Top (3 months)",
                "Top (6 months)",
                "Top (1 year)"
            };
            SelectedSort = SortItems.First();
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
                        await Application.Current.MainPage.DisplayAlert("No Token found", "You need an API key to search for NSFW wallpapers.", "Okay");
                        IncludeNsfw = false;
                        return;
                    }
                }

                await _useCase.Execute(new GetLatestWallpapersInput
                {
                    SearchTerm = SearchTerm,
                    Sfw = IncludeSfw,
                    Sketchy = IncludeSketchy,
                    Nsfw = IncludeNsfw,
                    General = IncludeGeneral,
                    Anime = IncludeAnime,
                    People = IncludePeople,
                    Sort = SortFromString(_selectedSort)
                });
                _viewModel.Wallpapers.Clear();

                foreach (var wallpaper in _presenter.Wallpapers)
                    _viewModel.Wallpapers.Add(wallpaper);

                await Application.Current.MainPage.Navigation.PopAsync();
            }
            finally
            {
                IsBusy = false;
            }
        }

        private IWallpaperSort SortFromString(string val)
        {
            return val switch
            {
                "Top (1 day)" => new TopSort(TopSortRange.Day),
                "Top (3 days)" => new TopSort(TopSortRange.ThreeDays),
                "Top (1 week)" => new TopSort(TopSortRange.Week),
                "Top (1 month)" => new TopSort(TopSortRange.Month),
                "Top (3 months)" => new TopSort(TopSortRange.ThreeMonths),
                "Top (6 months)" => new TopSort(TopSortRange.HalfYear),
                "Top (1 year)" => new TopSort(TopSortRange.Year),
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

        private string _selectedSort;

        public string SelectedSort
        {
            get => _selectedSort;
            set
            {
                _selectedSort = value;
                OnPropertyChanged();
            }
        }

        #endregion
    }
}