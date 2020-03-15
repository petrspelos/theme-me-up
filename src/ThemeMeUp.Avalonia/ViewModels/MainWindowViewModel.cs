using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Avalonia.Media.Imaging;
using ReactiveUI;
using ThemeMeUp.Avalonia.Models;
using ThemeMeUp.Core.Boundaries.GetLatestWallpapers;
using ThemeMeUp.Core.Entities;

namespace ThemeMeUp.Avalonia.ViewModels
{
    public class MainWindowViewModel : ViewModelBase
    {
        private readonly IGetLatestWallpapersUseCase _useCase;
        private readonly IGetLatestWallpapersOutputPort _output;
        private readonly UrlImageConverter _imgConverter;
        private Bitmap _firstWallpaper;
        private IEnumerable<WallpaperModel> _wallpapers;

        public MainWindowViewModel(IGetLatestWallpapersUseCase useCase, IGetLatestWallpapersOutputPort output, UrlImageConverter imgConverter)
        {
            _useCase = useCase;
            _output = output;
            _imgConverter = imgConverter;
        }

        public string Greeting => "Hello World!";

        public Bitmap FirstWallpaper
        {
            get => _firstWallpaper;
            set => _ = this.RaiseAndSetIfChanged(ref _firstWallpaper, value);
        }

        public IEnumerable<WallpaperModel> Wallpapers
        {
            get => _wallpapers;
            set => _ = this.RaiseAndSetIfChanged(ref _wallpapers, value);
        }

        public async void OnGetWallpapers()
        {
            await _useCase.Execute(new GetLatestWallpapersInput());

            // foreach(var wallpaperUrl in ((LatestWallpapersPresenter)_output)._wallpapers.Select(w => w.SmallThumbnailUrl))
            // {
            //     FirstWallpaper = await _imgConverter.UrlToBitmapAsync(wallpaperUrl);
            //     await Task.Delay(TimeSpan.FromSeconds(2));
            // }

            var result = new List<WallpaperModel>();

            foreach(var wallpaper in ((LatestWallpapersPresenter)_output)._wallpapers)
            {
                result.Add(await ToModel(wallpaper));
            }

            Wallpapers = result;
        }

        private async Task<WallpaperModel> ToModel(Wallpaper w)
        {
            var model = new WallpaperModel();

            model.ThumbnailBitmap = await _imgConverter.UrlToBitmapAsync(w.SmallThumbnailUrl);
            
            return model;
        }
    }
}
