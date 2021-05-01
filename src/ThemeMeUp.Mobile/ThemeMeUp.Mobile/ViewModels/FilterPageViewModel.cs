using System.Collections.ObjectModel;
using ThemeMeUp.Mobile.Models;

namespace ThemeMeUp.Mobile.ViewModels
{
    public class FilterPageViewModel : BaseViewModel
    {
        public FilterPageViewModel()
        {
            Title = "Filters";

            SortingOptions = new ObservableCollection<WallpaperSortingOption>
            {
                new WallpaperSortingOption
                {
                    Title = "Relevance"
                },
                new WallpaperSortingOption
                {
                    Title = "Random"
                },
                new WallpaperSortingOption
                {
                    Title = "Date Added"
                },
                new WallpaperSortingOption
                {
                    Title = "Views"
                },
                new WallpaperSortingOption
                {
                    Title = "Favorites"
                },
                new WallpaperSortingOption
                {
                    Title = "Toplist"
                },
                new WallpaperSortingOption
                {
                    Title = "Hot"
                },
            };
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

        private bool _isGeneral;
        public bool IsGeneral
        {
            get => _isGeneral;
            set
            {
                _isGeneral = value;
                OnPropertyChanged();
            }
        }

        private bool _isAnime;
        public bool IsAnime
        {
            get => _isAnime;
            set
            {
                _isAnime = value;
                OnPropertyChanged();
            }
        }

        private bool _isPeople;
        public bool IsPeople
        {
            get => _isPeople;
            set
            {
                _isPeople = value;
                OnPropertyChanged();
            }
        }

        private bool _isSfw;
        public bool IsSfw
        {
            get => _isSfw;
            set
            {
                _isSfw = value;
                OnPropertyChanged();
            }
        }

        private bool _isSketchy;
        public bool IsSketchy
        {
            get => _isSketchy;
            set
            {
                _isSketchy = value;
                OnPropertyChanged();
            }
        }

        private bool _isNsfw;
        public bool IsNsfw
        {
            get => _isNsfw;
            set
            {
                _isNsfw = value;
                OnPropertyChanged();
            }
        }

        private ObservableCollection<WallpaperSortingOption> _sortingOptions;
        public ObservableCollection<WallpaperSortingOption> SortingOptions
        {
            get => _sortingOptions;
            set
            {
                _sortingOptions = value;
                OnPropertyChanged();
            }
        }

        #endregion
    }
}