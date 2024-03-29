﻿using skullOS.Core;
using System.Device.Gpio;
using System.Reflection;
using Module = skullOS.Modules.Module;

namespace skullOS
{
    public class Program
    {
        static bool usePersonalDir = false;
        public static async Task Main(string[] args)
        {
            if (args.Length == 0)
            {
                Console.WriteLine("No arguments provided!");
                Environment.Exit(-1);
            }
            string input = args[0].ToLower();
            if (args.Length == 2)
            {
                bool.TryParse(args[1], out usePersonalDir);
            }
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

            await Console.Out.WriteLineAsync("Online and running!");
            await Task.Delay(Timeout.Infinite);
        }

        static void Run(bool shouldCreateDirectory = true)
        {
            if (shouldCreateDirectory)
            {
                Console.WriteLine("Use personal dir? " + usePersonalDir);
                FileManager.CreateSkullDirectory(usePersonalDir);
            }

            SkullLogger logger = new();
            GpioController controller = new();
            DeviceManager deviceManager = new(controller);
            InputManager inputManager = new();
            inputManager.attachLogger(logger);

            var settings = SettingsLoader.LoadConfig(@"Data/CoreSettings.txt");

            //Enable API
            if (settings.TryGetValue("API", out string useAPI))
            {
                logger.LogMessage("Enabling API...");
                if (bool.Parse(useAPI))
                {
                    string[] arguments = new string[1];
#if DEBUG
                    arguments[0] = "environment=development";
                    logger.LogMessage("Set first argument to " + arguments[0]);
#endif
                    //Replace this with something that calls the api as a seperate program


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
                    logger.LogMessage("Attempting to load " + item.Key);
                    Type moduleClass = ModulesLibrary.DefinedTypes.FirstOrDefault(x => x.Name == item.Key);
                    if (moduleClass != null)
                    {
                        object? moduleObj = Activator.CreateInstance(moduleClass);
                        Module? module = moduleObj as Module ?? throw new Exception("Failed to load module: " + item.Key);
                        module.AttachLogger(logger);
                        modules.Add(module);
                    }
                    else
                    {
                        logger.LogMessage("Something went wrong!");
                    }

                }
            }
            deviceManager.AttachModules(modules);

            //Setup input options
            inputManager.SetupSelector(deviceManager.GetModules());
        }
    }
}