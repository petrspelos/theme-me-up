using System;
using System.IO;
using System.Diagnostics;
using System.Net;
using System.Linq;
using System.Net.Http;
using System.Text.RegularExpressions;
using System.Web;
using System.Runtime.InteropServices;

namespace theme_me_up
{
    class Program
    {
        private static readonly HttpClient _client = new HttpClient();
        const string sketchyRandom = "https://wallhaven.cc/search?purity=010&sorting=random";
        const string normalAndSketchyRandom = "https://wallhaven.cc/search?purity=110&sorting=random";
        const string sfwRandom = "https://wallhaven.cc/search?purity=100&sorting=random";

        [DllImport("User32", CharSet = CharSet.Auto)]
        public static extern int SystemParametersInfo(int uiAction, int uiParam, string pvParam, uint fWinIni);

        static void Main(string[] args)
        {
            var picturesDir = Environment.GetFolderPath(Environment.SpecialFolder.MyPictures);
            var wallhavenCacheDir = Path.Combine(picturesDir, "wallhaven");

            Directory.CreateDirectory(wallhavenCacheDir);

            if(args.Any(arg => arg == "--help" || arg == "-h")) {
                Console.WriteLine("Theme Me Up");
                Console.WriteLine("Random wallpaper utility using wallhaven.cc");
                Console.WriteLine("");
                Console.WriteLine("OPTIONS");
                Console.WriteLine("SKETCHY-NESS");
                Console.WriteLine("-n | --nsfw   Picks a random sketchy wallpaper.");
                Console.WriteLine("-m | --mixed  Picks a random sketchy or normal wallpaper.");
                Console.WriteLine("Without any of the options, the default always picks a non-sketchy wallpaper");
                Console.WriteLine("");
                Console.WriteLine("CONTENT");
                Console.WriteLine("-a | --anime           Picks only anime wallpapers.");
                Console.WriteLine("-g | --general         No anime, no people.");
                Console.WriteLine("-w | --wide            Picks only 16x9 wallpapers.");
                Console.WriteLine("--search=[SEARCH TERM] Searches for a specified term.");
                Console.WriteLine("");
                Console.WriteLine("META");
                Console.WriteLine("--url                  Display search url. (used for debugging)");
                return;
            }

            var targetUrl = sfwRandom;
            if(args.Any(arg => arg.ToLower() == "--nsfw" || arg.ToLower() == "-n"))
            {
                targetUrl = sketchyRandom;
            }
            else if(args.Any(arg => arg.ToLower() == "--mixed" || arg.ToLower() == "-m"))
            {
                targetUrl = normalAndSketchyRandom;
            }

            if(args.Any(arg => arg.ToLower() == "--anime" || arg.ToLower() == "-a"))
            {
                targetUrl += "&categories=010";
            }
            else if(args.Any(arg => arg.ToLower() == "--general" || arg.ToLower() == "-g"))
            {
                targetUrl += "&categories=100";
            }
            else
            {
                targetUrl += "&categories=111";
            }

            if(args.Any(arg => arg.ToLower() == "--wide" || arg.ToLower() == "-w"))
            {
                targetUrl += "&ratios=16x9";
            }

            var searchTerm = args.Where(arg => arg.StartsWith("--search=")).Select(s => s.Substring(9)).FirstOrDefault();
            if(!string.IsNullOrEmpty(searchTerm))
            {
                targetUrl += "&q=" + HttpUtility.UrlEncode(searchTerm);
            }

            if(args.Any(arg => arg.ToLower() == "--url"))
            {
                Console.WriteLine(targetUrl);
            }

            var url = FetchRandomWallpaper(targetUrl);

            if(url is null) {
                Console.WriteLine("Wallhaven.cc was unable to provide wallpapers for your options.");
                Console.WriteLine("If you used a search term, it is possible there are not results for it.");
                return;
            }

            var wallpaperFileName = Path.GetFileName(url);
            var wallpaperFilePath = Path.Combine(wallhavenCacheDir, wallpaperFileName);

            if(!File.Exists(wallpaperFilePath))
                DownloadImage(url, wallpaperFilePath);

            if(RuntimeInformation.IsOSPlatform(OSPlatform.Linux)) {
                SetWallpaperGnome(wallpaperFilePath);
            } else if(RuntimeInformation.IsOSPlatform(OSPlatform.Windows)) {
                SetWallpaperWindows(wallpaperFilePath);
            } else {
                Console.WriteLine("Sorry, ThemeMeUp doesn't know how to set the wallpaper on your OS.");
                Console.WriteLine("However, we downloaded the wallpaper to your Pictures directory. =)");
            }
        }

        private static string FetchRandomWallpaper(string baseUrl)
        {
            using (WebClient client = new WebClient ())
            {
                string htmlCode = client.DownloadString(baseUrl);
                var regex = new Regex(@"wallhaven\.cc\/small\/.*?href=""(.*?)""");
                var match = regex.Match(htmlCode);
                if(!match.Success) { return null; }
                var wallpaperPageUrl = match.Groups[1].Value;
                var finalHtml = client.DownloadString(wallpaperPageUrl);
                var wallpaperRegex = new Regex(@"<img id=""wallpaper"" src=""(.*?\/full\/.*?)""");
                var finalMatch = wallpaperRegex.Match(finalHtml);
                return finalMatch.Groups[1].Value;
            }
        }

        private static void DownloadImage(string url, string file)
        {
            using(WebClient client = new WebClient())
            {
                client.DownloadFile(url, file);
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
