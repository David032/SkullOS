using Iot.Device.Buzzer;
using Iot.Device.Ws28xx;
using skullOS.Core;
using System.Device.Gpio;
using System.Device.Spi;
using System.Drawing;

namespace skullOS.Output
{
    public class Output : Controller
    {
        public List<OutputDevice> outputDevices = new();

        public override void Run(GpioController controller)
        {
            Console.WriteLine("### OUTPUT DEVICE WARMUP CHECKS ###");

            Ws2812b? pixelDisplay = (Ws2812b)outputDevices.Select(x => x).FirstOrDefault(x => x.Name == "NeoPixel").Device;
            pixelDisplay?.Image.SetPixel(0, 0, Color.AliceBlue);
            pixelDisplay.Update();

            //SkullLed? lifeLed = (SkullLed)outputDevices.Select(x => x).FirstOrDefault(x => x.Name == "Life Light");
            //lifeLed.TurnOn();

            var buzzer = (Buzzer)outputDevices.Select(x => x).FirstOrDefault(x => x.Name == "Buzzer").Device;
            buzzer.PlayTone(1000, 3000);
        }

        public override bool Setup(GpioController controller)
        {
            var settings = SettingsLoader.LoadConfig(@"Data/Output.txt");
            var defaultValue = new KeyValuePair<string, string>("", "");

            if (settings.ContainsKey("BuzzerPin"))
            {
                if (settings.TryGetValue("BuzzerPin", out string BuzzerPin))
                {
                    int buzzerPinNumber = Convert.ToInt32(BuzzerPin);
                    Buzzer buzzer = new(buzzerPinNumber);
                    outputDevices.Add(new OutputDevice("Buzzer", buzzer));

                }
            }

            if (settings.ContainsKey("NeoPixel"))
            {
                if (settings.TryGetValue("NeoPixelCount", out string count))
                {
                    int neoPixelLedCount = Convert.ToInt32(count);
                    SpiConnectionSettings spiSettings = new(0, 0)
                    {
                        ClockFrequency = 2_400_000,
                        Mode = SpiMode.Mode0,
                        DataBitLength = 8
                    };
                    using SpiDevice spi = SpiDevice.Create(spiSettings);

                    Ws2812b neoPixel = new Ws2812b(spi, neoPixelLedCount);
                    outputDevices.Add(new OutputDevice("NeoPixel", neoPixel));
                }
            }


            //if (settings.ContainsKey("LedPin"))
            //{
            //    if (settings.TryGetValue("LedPin", out string pin))
            //    {
            //        int lifeLedPinNumber = Convert.ToInt32(pin);
            //        SkullLed deviceLed = new("Life Light", lifeLedPinNumber, controller);

            //        outputDevices.Add(deviceLed);
            //    }
            //}

            //if (settings.ContainsKey("CameraLed"))
            //{
            //    if (settings.TryGetValue("CameraLed", out string pin))
            //    {
            //        int lifeLedPinNumber = Convert.ToInt32(pin);
            //        SkullLed deviceLed = new("Camera Light", lifeLedPinNumber, controller);

            //        outputDevices.Add(deviceLed);
            //    }
            //}

            return true;
        }

        public override void Stop()
        {
            throw new NotImplementedException();
        }
    }

    public class OutputDevice
    {
        public string Name = "";
        public Object Device;

        public OutputDevice(string name, Object device)
        {
            Name = name;
            Device = device;
        }
    }
}