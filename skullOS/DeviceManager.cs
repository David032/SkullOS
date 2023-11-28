using skullOS.Modules;
using System.Device.Gpio;

namespace skullOS
{
    public class DeviceManager
    {
        Task apiStatus;
        List<Module> Modules;
        GpioController Controller;

        public DeviceManager(GpioController gpio)
        {
            Controller = gpio;
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

    }
}
