using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ReactiveUI;
using ThemeMeUp.Avalonia.Models;
using ThemeMeUp.Avalonia.Utilities;
using ThemeMeUp.Core.Boundaries;
using ThemeMeUp.Core.Boundaries.GetLatestWallpapers;
using ThemeMeUp.Core.Entities;
using ThemeMeUp.Core.Entities.Sorting;

namespace ThemeMeUp.Avalonia.ViewModels
{
    public class MainWindowViewModel : ViewModelBase
    {
        private readonly IGetLatestWallpapersUseCase _useCase;
        private readonly LatestWallpapersPresenter _output;
        private readonly UrlImageConverter _imgConverter;
        private readonly IWallpaperSetter _wallpaperSetter;
        private string _searchTerm;
        private bool _includeSfw;
        private bool _includeSketchy;
        private bool _includeNsfw;
        private bool _includeGeneral = true;
        private bool _includeAnime = true;
        private bool _includePeople = true;
        private bool _fetchButtonEnabled = true;
        private IEnumerable<WallpaperModel> _wallpapers;
        private IEnumerable<string> _sorts;
        private string _selectedSort;
        private WallpaperModel _selectedWallpaper;

        public MainWindowViewModel(IGetLatestWallpapersUseCase useCase, IGetLatestWallpapersOutputPort output, UrlImageConverter imgConverter, IWallpaperSetter wallpaperSetter)
        {
            _useCase = useCase;
            _output = (LatestWallpapersPresenter)output;
            _imgConverter = imgConverter;
            _wallpaperSetter = wallpaperSetter;
            _sorts = new string[]
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
            _selectedSort = _sorts.First();
        }

        public string SearchTerm
        {
            get => _searchTerm;
            set => _ = this.RaiseAndSetIfChanged(ref _searchTerm, value);
        }

        public bool IncludeSfw
        {
            get => _includeSfw;
            set => _ = this.RaiseAndSetIfChanged(ref _includeSfw, value);
        }

        public bool IncludeSketchy
        {
            get => _includeSketchy;
            set => _ = this.RaiseAndSetIfChanged(ref _includeSketchy, value);
        }

        public bool IncludeNsfw
        {
            get => _includeNsfw;
            set => _ = this.RaiseAndSetIfChanged(ref _includeNsfw, value);
        }

        public bool IncludeGeneral
        {
            get => _includeGeneral;
            set => _ = this.RaiseAndSetIfChanged(ref _includeGeneral, value);
        }

        public bool IncludeAnime
        {
            get => _includeAnime;
            set => _ = this.RaiseAndSetIfChanged(ref _includeAnime, value);
        }

        public bool IncludePeople
        {
            get => _includePeople;
            set => _ = this.RaiseAndSetIfChanged(ref _includePeople, value);
        }

        public bool FetchButtonEnabled
        {
            get => _fetchButtonEnabled;
            set => _ = this.RaiseAndSetIfChanged(ref _fetchButtonEnabled, value);
        }

        public IEnumerable<WallpaperModel> Wallpapers
        {
            get => _wallpapers;
            set => _ = this.RaiseAndSetIfChanged(ref _wallpapers, value);
        }

        public IEnumerable<string> SortItems
        {
            get => _sorts;
            set => _ = this.RaiseAndSetIfChanged(ref _sorts, value);
        }

        public string SelectedSort
        {
            get => _selectedSort;
            set => _ = this.RaiseAndSetIfChanged(ref _selectedSort, value);
        }

        public WallpaperModel SelectedWallpaper
        {
            get => _selectedWallpaper;
            set => _ = this.RaiseAndSetIfChanged(ref _selectedWallpaper, value);
        }

        public async void OnGetWallpapers()
        {
            Wallpapers = Array.Empty<WallpaperModel>();
            _output.Clear();

            FetchButtonEnabled = false;

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

            if (_output.noApiKey || _output.noConnection)
            {
                FetchButtonEnabled = true;
                return;
            }

            var result = new List<WallpaperModel>();

            foreach (var wallpaper in _output.wallpapers)
            {
                result.Add(await ToModel(wallpaper));
            }

            Wallpapers = result;

            FetchButtonEnabled = true;
        }

        public async void SetSelectedWallpaper()
        {
            if (SelectedWallpaper is null) { return; }

            await _wallpaperSetter.SetFromUrlAsync(SelectedWallpaper.FullImageUrl);
        }

        private async Task<WallpaperModel> ToModel(Wallpaper w)
        {
            var model = new WallpaperModel
            {
                FullImageUrl = w.FullImageUrl
            };

            model.ThumbnailBitmap = await _imgConverter.UrlToBitmapAsync(w.SmallThumbnailUrl);

            return model;
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
    }
}
