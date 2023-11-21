using Iot.Device.Ws28xx;
using skullOS.HardwareServices.Interfaces;
using System.Device.Spi;

namespace skullOS.HardwareServices
{
    public class ProgrammableLedService : IProgrammableLedService
    {
        public Ws2812b LedArray { get; private set; }

        public ProgrammableLedService(SpiDevice spi, int width = 9)
        {
            LedArray = new Ws2812b(spi, width);
        }
    }
}
