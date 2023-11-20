using skullOS.Controllers.Buzzer;
using skullOS.Core;
using skullOS.Services;

namespace skullOS
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            string input = args[0].ToLower();
            switch (input)
            {
                case "run":
                    Run();
                    break;

                case "setup":
                    // setup(enable/disable subsystems),
                    break;

                case "install":
                    // install/validate(Check prerequisites are installed)
                    break;

                default:
                    Console.WriteLine("Argument not recognised! Exiting");
                    Environment.Exit(-1);
                    break;
            }

            await Task.Delay(Timeout.Infinite);
        }

        static async void Run(Modules modulesToLoad = null, bool shouldCreateDirectory = true)
        {
            if (shouldCreateDirectory)
            {
                FileManager.CreateSkullDirectory();
            }
            var settings = SettingsLoader.LoadConfig(@"Data/CoreSettings.txt");
            List<string> controllersToLoad = new List<string>();

            //First load all the hardware services
            BuzzerService hardwareBuzzer = new(13);
            //Then the software controllers
            BuzzerController buzzerController = new(hardwareBuzzer);
            controllersToLoad.Add("BuzzerController");
            //API Should be the last thing to be loaded
            //            if (settings.TryGetValue("API", out string useAPI))
            //            {
            //                if (bool.Parse(useAPI))
            //                {
            //                    string[] arguments = null;
            //#if DEBUG
            //                    arguments[0] = "enviroment=development";
            //#endif
            //                    API.ApiApp api = new(arguments, controllersToLoad);
            //                }
            //            }

            //Temp -----
            buzzerController.PlayTune(0);
            await Task.Delay(Timeout.Infinite);

        }


    }
}