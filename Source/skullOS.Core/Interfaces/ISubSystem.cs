using System.Device.Gpio;
using System.Device.I2c;

namespace skullOS.Core.Interfaces
{
    public interface ISubSystem
    {
        bool Setup(GpioController controller, I2cDevice i2CDevice);

        void Run(GpioController controller);

        void Stop();
    }
}
