using skullOS.Core.Interfaces;
using System.Device.Gpio;

namespace skullOS.Core
{
    public abstract class Controller : ISubSystem
    {
        public abstract void Run(GpioController controller);

        public abstract bool Setup(GpioController controller);

        public abstract void Stop();
    }
}
