using System;
using System.Linq;
using System.Net.Http;
using Lamar;
using ThemeMeUp.Core.Boundaries.Infrastructure;
using ThemeMeUp.Infrastructure;
using ThemeMeUp.Core.UseCases;
using ThemeMeUp.Core.Boundaries.GetLatestWallpapers;
using System.Threading.Tasks;
using ThemeMeUp.ApiWrapper;
using ThemeMeUp.Core.Boundaries;
using ThemeMeUp.ConsoleApp.Utilities;

namespace ThemeMeUp.ConsoleApp
{
    class Program
    {
        static async Task Main(string[] args)
        {
            var container = new Container(c =>
            {
                c.For<HttpClient>().UseIfNone<HttpClient>();
                c.For<INetwork>().UseIfNone<Network>();
                c.For<IWallhavenClient>().UseIfNone<WallhavenClient>();
                c.For<IWallpaperProvider>().UseIfNone<WallpaperProvider>();
                c.For<IGetLatestWallpapersUseCase>().Use<GetLatestWallpapersUseCase>();
                c.For<IGetLatestWallpapersOutputPort>().UseIfNone<LatestWallpapersPresenter>();
                c.For<IAuthentication>().UseIfNone<JsonFileAuthentication>();
                c.For<WallpaperSetter>().UseIfNone<WallpaperSetter>();
            });

            if(args.Any(arg => arg == "--help" || arg == "-h")) {
                Console.WriteLine("Theme Me Up");
                Console.WriteLine("Set a wallhaven.cc wallpaper from the comfort of your OS.\n");
                Console.WriteLine("Usage:");
                Console.WriteLine("  theme-me-up [one or more options]\n");
                Console.WriteLine("Examples:");
                Console.WriteLine("  theme-me-up             Sets the newest SFW wallpaper as desktop background");
                Console.WriteLine("  theme-me-up -n          Sets the newest NSFW wallpaper as desktop background");
                Console.WriteLine("  theme-me-up -n -k       Sets the newest NSFW or Sketchy wallpaper as desktop background");
                Console.WriteLine("  theme-me-up --query=car Sets the newest 'car' wallpaper as desktop background");
                Console.WriteLine(string.Empty);
                Console.WriteLine("General Options:");
                Console.WriteLine("  --api          Displays help about API keys");
                Console.WriteLine(string.Empty);
                Console.WriteLine("Purity Options:");
                Console.WriteLine("  If no purity option is provided,");
                Console.WriteLine("  only SFW wallpapers will be fetched.\n");
                Console.WriteLine("  -n | --nsfw    Include NSFW wallpapers (API key required)");
                Console.WriteLine("  -s | --sfw     Include SFW wallpapers");
                Console.WriteLine("  -k | --sketchy Include Sketchy wallpapers");
                Console.WriteLine(string.Empty);
                Console.WriteLine("Custom Search:");
                Console.WriteLine("  -q=<TERM> | --query=<TERM>    Search for a wallpaper");
                return;
            }

            if(args.Any(arg => arg == "--api"))
            {
                Console.WriteLine("Theme Me Up");
                Console.WriteLine("  API KEY HELP");
                Console.WriteLine(string.Empty);
                Console.WriteLine("In order to use NSFW wallpapers, you need to set an API key.");
                Console.WriteLine("You get an API key here: https://wallhaven.cc/settings/account");
                Console.WriteLine("A free account is required.");
                Console.WriteLine(string.Empty);
                Console.WriteLine("Use the following command to set your key:");
                Console.WriteLine("  theme-me-up --key=xxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxx");
                return;
            }

            var keyArg = args.FirstOrDefault(arg => arg.StartsWith("--key="));
            if(keyArg != null)
            {
                var auth = container.GetInstance<IAuthentication>();
                var key = keyArg.Substring(6);
                auth.SetApiKey(key);
                Console.WriteLine($"Your provided {key.Length} characters long API key was set.");
                return;
            }

            var useCase = container.GetInstance<IGetLatestWallpapersUseCase>();

            var searchQuery = GetQueryArgValue(args.FirstOrDefault(arg => arg.StartsWith("-q=") || arg.StartsWith("--query=")));

            await useCase.Execute(new GetLatestWallpapersInput
            {
                Nsfw = args.Any(arg => arg == "-n" || arg == "--nsfw"),
                Sfw = args.Any(arg => arg == "-s" || arg == "--sfw"),
                Sketchy = args.Any(arg => arg == "-k" || arg == "--sketchy"),
                SearchTerm = searchQuery
            });
        }

        private static string GetQueryArgValue(string arg)
        {
            if(arg is null) { return string.Empty; }
            if(arg.StartsWith("--query=")) { return arg.Substring(8); }
            if(arg.StartsWith("-q=")) { return arg.Substring(3); }
            return string.Empty;
        }
    }
}
