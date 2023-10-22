using Iot.Device.Ws28xx;
using System.Device.Spi;

namespace skullOS.Output
{
    public class SkullNeoPixel : SkullOutputDevice
    {
        public Ws2812b device
        {
            get { return neoPixel; }
        }
        Ws2812b neoPixel;

        public SkullNeoPixel(string name, int count)
        {
            this.Name = name;

            SpiConnectionSettings spiSettings = new(0, 0)
            {
                ClockFrequency = 2_400_000,
                Mode = SpiMode.Mode0,
                DataBitLength = 8
            };
            using SpiDevice spi = SpiDevice.Create(spiSettings);

            neoPixel = new Ws2812b(spi, count);
        }
    }
}
