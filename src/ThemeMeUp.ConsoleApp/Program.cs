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
using ThemeMeUp.Core.Entities.Sorting;

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
                c.For<IWallpaperSetter>().UseIfNone<WallpaperSetter>();
                c.For<Configuration>().UseIfNone<Configuration>();
                c.For<Random>().UseIfNone<Random>();
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
                Console.WriteLine("Content Options:");
                Console.WriteLine("  If no content option is provided,");
                Console.WriteLine("  all options are included.\n");
                Console.WriteLine("  -g | --general      Include General wallpapers");
                Console.WriteLine("  -a | --anime        Include Anime wallpapers");
                Console.WriteLine("  -p | --people       Include People wallpapers");
                Console.WriteLine(string.Empty);
                Console.WriteLine("Sort Options:");
                Console.WriteLine("  If no sort option is provided,");
                Console.WriteLine("  newest wallpaper is picked.\n");
                Console.WriteLine("  --top-today         Picks the most popular wallpaper today");
                Console.WriteLine("  --top-3days         Picks the most popular wallpaper last 3 days");
                Console.WriteLine("  --top-week          Picks the most popular wallpaper last week");
                Console.WriteLine("  --top-month         Picks the most popular wallpaper last month");
                Console.WriteLine("  --top-3months       Picks the most popular wallpaper last 3 months");
                Console.WriteLine("  --top-halfYear      Picks the most popular wallpaper last 6 months");
                Console.WriteLine("  --top-year          Picks the most popular wallpaper last year");
                Console.WriteLine("Selection Method:");
                Console.WriteLine("  If no selection option is provided,");
                Console.WriteLine("  the first wallpaper is picked.\n");
                Console.WriteLine("  --select-random         Picks a random wallpaper from result page");
                Console.WriteLine("  --select-newOrRandom      Picks the first wallpaper, if not new, picks random");
                Console.WriteLine(string.Empty);
                Console.WriteLine("GNU/Linux wallpaper utilities:");
                Console.WriteLine("  --feh          Sets the wallpaper using feh");
                Console.WriteLine("  --nitrogen     Sets the wallpaper using nitrogen");
                Console.WriteLine("  --gnome        Sets the wallpaper through gsettings (default)");
                Console.WriteLine(string.Empty);
                Console.WriteLine("For custom utilities please change the ");
                Console.WriteLine("in ~/.config/theme-me-up/config.json file.");
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

            IWallpaperSort sort;
            var sortArg = args.FirstOrDefault(arg => arg.StartsWith("--top-"));
            if(sortArg is null)
            {
                sort = new LatestSort();
            }
            else
            {
                sort = sortArg switch
                {
                    "--top-today" => new TopSort(TopSortRange.Day),
                    "--top-3days" => new TopSort(TopSortRange.ThreeDays),
                    "--top-week" => new TopSort(TopSortRange.Week),
                    "--top-month" => new TopSort(TopSortRange.Month),
                    "--top-3months" => new TopSort(TopSortRange.ThreeMonths),
                    "--top-halfYear" => new TopSort(TopSortRange.HalfYear),
                    "--top-year" => new TopSort(TopSortRange.Year),
                    _ => new LatestSort()
                };
            }

            LatestWallpapersPresenter.TrueRandom = args.Any(arg => arg == "--select-random");
            LatestWallpapersPresenter.NewOrRandom = args.Any(arg => arg == "--select-newOrRandom");

            var wallSetter = container.GetInstance<IWallpaperSetter>();
            wallSetter.ApplyArgs(args);

            var useCase = container.GetInstance<IGetLatestWallpapersUseCase>();

            var searchQuery = GetQueryArgValue(args.FirstOrDefault(arg => arg.StartsWith("-q=") || arg.StartsWith("--query=")));

            await useCase.Execute(new GetLatestWallpapersInput
            {
                Nsfw = args.Any(arg => arg == "-n" || arg == "--nsfw"),
                Sfw = args.Any(arg => arg == "-s" || arg == "--sfw"),
                Sketchy = args.Any(arg => arg == "-k" || arg == "--sketchy"),
                General = args.Any(arg => arg == "-g" || arg == "--general"),
                Anime = args.Any(arg => arg == "-a" || arg == "--anime"),
                People = args.Any(arg => arg == "-p" || arg == "--people"),
                SearchTerm = searchQuery,
                Sort = sort,
                RandomPage = LatestWallpapersPresenter.TrueRandom || LatestWallpapersPresenter.NewOrRandom
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
