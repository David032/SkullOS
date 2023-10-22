using skullOS.Core.Interfaces;
using System.Device.Gpio;
using System.Reflection;

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

        static void Run(Modules modulesToLoad = null)
        {
            GpioController controller = new();
            List<ISubSystem> systemsLoaded = LoadModules(modulesToLoad);

            SetupModules(systemsLoaded, controller);

            RunModules(systemsLoaded, controller);
        }

        public static bool RunModules(List<ISubSystem> systemsLoaded, GpioController controller)
        {
            foreach (ISubSystem system in systemsLoaded)
            {
                system.Run(controller);
            }
            return true;
        }

        public static bool SetupModules(List<ISubSystem> systemsLoaded, GpioController controller)
        {
            foreach (var system in systemsLoaded)
            {
                if (!system.Setup(controller))
                {
                    throw new Exception($"{system} failed to load");
                }
            }
            return true;
        }

        public static List<ISubSystem> LoadModules(Modules modulesToLoad)
        {
            Modules modules;
            List<ISubSystem> subSystems = new();
            if (modulesToLoad == null)
            {
                modules = new();
            }
            else
            {
                modules = modulesToLoad;
            }

            foreach (var item in modules.Get())
            {

                try
                {
                    Assembly system = Assembly.Load("skullOS." + item.ModuleName);
                    Type systemType = system.DefinedTypes.Where(x => x.Name == item.ModuleName).FirstOrDefault();
                    object obj = Activator.CreateInstance(systemType, false);
                    subSystems.Add((ISubSystem)obj);
                }
                catch (Exception e)
                {

                    throw;
                }
            }

            return subSystems;
        }

        void Setup()
        {

        }
    }
}