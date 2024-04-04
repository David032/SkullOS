using skullOS.Core.Interfaces;
using System.Device.Gpio;
using System.Device.I2c;

namespace skullOS.Core
{
    public abstract class Controller : ISubSystem
    {
        public abstract void Run(GpioController controller);

        public abstract bool Setup(GpioController controller, I2cDevice i2CDevice);

        public abstract void Stop();
    }
}
