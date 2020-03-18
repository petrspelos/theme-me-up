
using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using ThemeMeUp.Core.Boundaries;

namespace ThemeMeUp.Infrastructure
{
    public class WallpaperSetter : IWallpaperSetter
    {
        private readonly Configuration _config;

        public WallpaperSetter(Configuration config, HttpClient client)
        {
            _config = config;
            _client = client;

            var picturesDir = Environment.GetFolderPath(Environment.SpecialFolder.MyPictures);
            _wallpapersDirectoryPath = Path.Combine(picturesDir, "wallhaven");
            Directory.CreateDirectory(_wallpapersDirectoryPath);
        }

        private readonly HttpClient _client;
        private readonly string _wallpapersDirectoryPath;

        [DllImport("User32", CharSet = CharSet.Auto)]
        public static extern int SystemParametersInfo(int uiAction, int uiParam, string pvParam, uint fWinIni);

        public void ApplyArgs(string[] args)
        {
            var cfg = _config.GetConfig();

            if(args.Contains("--nitrogen"))
            {
                cfg.WallpaperApp = "nitrogen";
                cfg.WallpaperAppArgs = "{0} --set-scaled";
            }
            
            if(args.Contains("--feh"))
            {
                cfg.WallpaperApp = "feh";
                cfg.WallpaperAppArgs = "--bg-scale {0}";
            }

            if(args.Contains("--gnome"))
            {
                cfg.WallpaperApp = "gsettings";
                cfg.WallpaperAppArgs = "set org.gnome.desktop.background picture-uri {0}";
            }

            _config.StoreConfig(cfg);
        }

        public Task SetFromUrlAsync(string url)
        {
            var filePath = Path.Combine(_wallpapersDirectoryPath, Path.GetFileName(url));
            DownloadImage(url, filePath);
            SetWallpaper(filePath);
            return Task.CompletedTask;
        }

        private void DownloadImage(string url, string file)
        {
            if(File.Exists(file)) { return; }

            using(WebClient client = new WebClient())
            {
                client.DownloadFile(url, file);
            }
        }

        private void SetWallpaper(string filePath)
        {
            if(!File.Exists(filePath)) { return; }
            
            if(RuntimeInformation.IsOSPlatform(OSPlatform.Linux)) {
                SetWallpaperGnuLinux(filePath);
            } else if(RuntimeInformation.IsOSPlatform(OSPlatform.Windows)) {
                SetWallpaperWindows(filePath);
            } else {
                Console.WriteLine("Unsupported platform. Cannot set wallpaper.");
            }
        }

        private void SetWallpaperGnuLinux(string file)
        {
            var cfg = _config.GetConfig();

            using var process = Process.Start(
            new ProcessStartInfo
            {
                FileName = cfg.WallpaperApp,
                Arguments = string.Format(cfg.WallpaperAppArgs, file)
            });
            process.WaitForExit();
        }

        private static void SetWallpaperWindows(string file) => SystemParametersInfo(0x0014, 0, file, 0x0001);
    }
}
