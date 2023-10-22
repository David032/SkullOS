using System.Device.Gpio;

namespace skullOS.Core.Interfaces
{
    public interface ISubSystem
    {
        bool Setup(GpioController controller);

        void Run(GpioController controller);

        void Stop();
    }
}
