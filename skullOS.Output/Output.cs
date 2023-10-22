using skullOS.Core;
using System.Device.Gpio;
using System.Drawing;

namespace skullOS.Output
{
    public class Output : Controller
    {
        public List<SkullOutputDevice> outputDevices = new();

        public override void Run(GpioController controller)
        {
            SkullNeoPixel? pixelDisplay = (SkullNeoPixel)outputDevices.Select(x => x).FirstOrDefault(x => x.Name == "NeoPixel");
            pixelDisplay?.device.Image.SetPixel(0, 0, Color.AliceBlue);
            SkullLed? lifeLed = (SkullLed)outputDevices.Select(x => x).FirstOrDefault(x => x.Name == "Life Light");
            lifeLed.TurnOn();
        }

        public override bool Setup(GpioController controller)
        {
            var settings = SettingsLoader.LoadConfig(@"Data/Output.txt");
            var defaultValue = new KeyValuePair<string, string>("", "");
#if DEBUG
            Console.WriteLine("Output device settings contain:");
            foreach (var item in settings)
            {
                Console.WriteLine(item.Key + " : " + item.Value);
            }
#endif
            if (settings.ContainsKey("BuzzerPin"))
            {
                if (settings.TryGetValue("BuzzerPin", out string BuzzerPin))
                {
                    int buzzerPinNumber = Convert.ToInt32(BuzzerPin);
                    SkullBuzzer deviceBuzzer = new("Buzzer", buzzerPinNumber);
                    outputDevices.Add(deviceBuzzer);
                }
            }

            if (settings.ContainsKey("NeoPixel"))
            {
                if (settings.TryGetValue("NeoPixelCount", out string count))
                {
                    int neoPixelPinNumber = Convert.ToInt32(count);
                    SkullNeoPixel deviceNeoPixel = new("NeoPixel", neoPixelPinNumber);
                    outputDevices.Add(deviceNeoPixel);
                }
            }


            if (settings.ContainsKey("LedPin"))
            {
                if (settings.TryGetValue("LedPin", out string pin))
                {
                    int lifeLedPinNumber = Convert.ToInt32(pin);
                    SkullLed deviceLed = new("Life Light", lifeLedPinNumber, controller);

                    outputDevices.Add(deviceLed);
                }
            }


#if DEBUG
            Console.WriteLine("The following output devices have been registered:");
            foreach (var item in outputDevices)
            {
                Console.WriteLine(item.Name);
            }
#endif
            return true;
        }

        public override void Stop()
        {
            throw new NotImplementedException();
        }
    }
}