using Android.App;
using Android.Graphics;
using System.Net;
using System.Threading.Tasks;
using ThemeMeUp.Core.Boundaries;

namespace ThemeMeUp.Mobile.Droid
{
    class AndroidWallpaperSetter : IWallpaperSetter
    {
        public void ApplyArgs(string[] args)
        {
        }

        public bool IsCached(string url) => false;

        public Task SetFromUrlAsync(string url)
        {
            var request = WebRequest.Create(url);
            var response = request.GetResponse();
            var responseStream = response.GetResponseStream();
            
            var wallpaperBitmap = BitmapFactory.DecodeStream(responseStream);

            var manager = WallpaperManager.GetInstance(Application.Context);

            manager.SetBitmap(wallpaperBitmap);
            return Task.CompletedTask;
        }
    }
}
