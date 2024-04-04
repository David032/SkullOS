using Iot.Device.Ws28xx;
using skullOS.HardwareServices.Interfaces;
using System.Device.Spi;

namespace skullOS.HardwareServices
{
    public class ProgrammableLedService : IProgrammableLedService
    {
        public Ws2812b LedArray { get; private set; }

        public ProgrammableLedService(SpiDevice spi, int width = 9, Ws2812b LedArray = null)
        {
            if (LedArray == null)
            {
                this.LedArray = new Ws2812b(spi, width);
            }
            else
            {
                this.LedArray = LedArray;
            }
        }
    }
}
