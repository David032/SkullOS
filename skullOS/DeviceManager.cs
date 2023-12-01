using skullOS.Modules;
using System.Device.Gpio;
using System.Net.NetworkInformation;

namespace skullOS
{
    public class DeviceManager
    {
        Task apiStatus;
        List<Module> Modules;
        GpioController Controller;
        int powerLed = 23;
        int networkLED = 12;

        public DeviceManager(GpioController gpio)
        {
            Controller = gpio;

            if (!Controller.IsPinOpen(powerLed))
            {
                Controller.OpenPin(powerLed, PinMode.Output);
            }
            Controller.Write(powerLed, PinValue.High);

            var autoEvent = new AutoResetEvent(false);
            var connectionCheck = new Timer(CheckForNetwork, autoEvent, 0, 30000);
        }

        public void AttachApi(Task apiTask)
        {
            apiStatus = apiTask;
        }

        public void AttachModules(List<Module> modules)
        {
            Modules = modules;
        }

        public List<Module> GetModules()
        {
            return Modules;
        }

        public void CheckForNetwork(object? state)
        {
            if (NetworkInterface.GetIsNetworkAvailable())
            {
                if (!Controller.IsPinOpen(networkLED))
                {
                    Controller.OpenPin(networkLED, PinMode.Output);
                }
                Controller.Write(networkLED, PinValue.High);
            }
            else
            {
                if (!Controller.IsPinOpen(networkLED))
                {
                    Controller.OpenPin(networkLED, PinMode.Output);
                }
                Controller.Write(networkLED, PinValue.High);
            }

        }

    }
}
