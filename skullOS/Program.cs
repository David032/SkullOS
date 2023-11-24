using skullOS.Core;
using System.Device.Gpio;
using System.Reflection;
using Module = skullOS.Modules.Module;
using Runner = skullOS.API.Runner;

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

        static void Run(bool shouldCreateDirectory = true)
        {
            SkullLogger logger = new();
            GpioController controller = new();
            DeviceManager deviceManager = new(controller);
            InputManager inputManager = new();

            if (shouldCreateDirectory)
            {
                FileManager.CreateSkullDirectory();
            }
            var settings = SettingsLoader.LoadConfig(@"Data/CoreSettings.txt");

            //Enable API
            if (settings.TryGetValue("API", out string useAPI))
            {
                logger.LogMessage("Enabling API...");
                if (bool.Parse(useAPI))
                {
                    Runner apiRunner = new();
                    Task apiStatus = apiRunner.StartWebAPI(null);
                    deviceManager.AttachApi(apiStatus);
                    logger.LogMessage("API enabled");
                }
            }

            //Load Modules
            List<Module> modules = new();
            Assembly ModulesLibrary = Assembly.Load("skullOS.Modules");
            var modulesToLoad = SettingsLoader.LoadConfig(@"Data/Modules.txt");
            foreach (var item in ModulesLibrary.DefinedTypes)
            {
                Console.WriteLine("Modules library contains: " + item.Name);
            }
            foreach (var item in modulesToLoad)
            {
                logger.LogMessage("Checking: " + item.Key);
                if (bool.Parse(item.Value))
                {
                    logger.LogMessage("Attempting to load " + item.Key); //Can't seem to find the camera
                    Type moduleClass = ModulesLibrary.GetType(item.Key);
                    Module? module = Activator.CreateInstance(moduleClass) as Module;
                    modules.Add(module);
                }
            }
            deviceManager.AttachModules(modules);

            //Setup input options
            inputManager.SetupSelector(deviceManager.GetModules());
        }
    }
}