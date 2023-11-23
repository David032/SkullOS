using skullOS.API;
using skullOS.Core;
using System.Device.Gpio;
using System.Reflection;
using Module = skullOS.Modules.Module;

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
            DeviceManager manager = new();
            if (shouldCreateDirectory)
            {
                FileManager.CreateSkullDirectory();
            }
            var settings = SettingsLoader.LoadConfig(@"Data/CoreSettings.txt");
            GpioController controller = new();

            //Enable API
            if (settings.TryGetValue("API", out string useAPI))
            {
                if (bool.Parse(useAPI))
                {
                    Runner apiRunner = new();
                    Task apiStatus = apiRunner.StartWebAPI(null);
                    manager.AttachApi(apiStatus);
                }
            }

            //Load Modules
            List<Module> modules = new();
            Assembly ModulesLibrary = Assembly.Load("skullOS.Modules");
            var modulesToLoad = SettingsLoader.LoadConfig(@"Data/Modules.txt");
            foreach (var item in modulesToLoad)
            {
                if (bool.Parse(item.Value))
                {
                    Type moduleClass = ModulesLibrary.GetType(item.Key);
                    Module? module = Activator.CreateInstance(moduleClass) as Module;
                    modules.Add(module);
                }
            }
            manager.AttachModules(modules);

        }
    }
}