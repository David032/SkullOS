﻿using Iot.Device.Ws28xx;
using System.Device.Spi;

namespace skullOS.Output
{
    internal class SkullNeoPixel : SkullOutputDevice
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

            Ws2812b neopixel = new(spi, count);

        }
    }
}