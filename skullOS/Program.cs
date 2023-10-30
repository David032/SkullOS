using Iot.Device.Bmxx80;
using skullOS.Core.Interfaces;
using System.Device.Gpio;
using System.Device.I2c;
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

            const int busId = 1;
            I2cConnectionSettings i2cSettings = new(busId, Bme280.DefaultI2cAddress);
            I2cDevice i2cDevice = I2cDevice.Create(i2cSettings);

            List<ISubSystem> systemsLoaded = LoadModules(modulesToLoad);

            SetupModules(systemsLoaded, controller, i2cDevice);

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

        public static bool SetupModules(List<ISubSystem> systemsLoaded, GpioController controller, I2cDevice i2CDevice)
        {
            foreach (var system in systemsLoaded)
            {


                Console.WriteLine("Setting up " + system.ToString());
                if (system.ToString().Equals("skullOS.Interlink.Interlink"))
                {
                    Console.WriteLine("Giving linker data!");
                    Interlink.Interlink linker = (Interlink.Interlink)systemsLoaded.Select(x => x).FirstOrDefault(x => x.ToString() == "skullOS.Interlink.Interlink");
                    linker.subSystems = systemsLoaded;
                }



                if (!system.Setup(controller, i2CDevice))
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