using System;
using System.Diagnostics;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Runtime.InteropServices;
using System.Threading.Tasks;

namespace ThemeMeUp.ConsoleApp.Utilities
{
    public class WallpaperSetter
    {
        private readonly HttpClient _client;
        private readonly string _wallpapersDirectoryPath;

        [DllImport("User32", CharSet = CharSet.Auto)]
        public static extern int SystemParametersInfo(int uiAction, int uiParam, string pvParam, uint fWinIni);

        public WallpaperSetter(HttpClient client)
        {
            _client = client;

            var picturesDir = Environment.GetFolderPath(Environment.SpecialFolder.MyPictures);
            _wallpapersDirectoryPath = Path.Combine(picturesDir, "wallhaven");
            Directory.CreateDirectory(_wallpapersDirectoryPath);
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
                SetWallpaperGnome(filePath);
            } else if(RuntimeInformation.IsOSPlatform(OSPlatform.Windows)) {
                SetWallpaperWindows(filePath);
            } else {
                Console.WriteLine("Unsupported platform. Cannot set wallpaper.");
            }
        }

        private static void SetWallpaperGnome(string file)
        {
            using var process = Process.Start(
            new ProcessStartInfo
            {
                FileName = "gsettings",
                ArgumentList = { "set", "org.gnome.desktop.background", "picture-uri", file }
            });
            process.WaitForExit();
        }

        private static void SetWallpaperWindows(string file) => SystemParametersInfo(0x0014, 0, file, 0x0001);
    }
}
