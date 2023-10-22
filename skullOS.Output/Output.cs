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
            var pixelDisplay = (SkullNeoPixel)outputDevices.Select(x => x).Where(x => x.Name == "NeoPixel").FirstOrDefault();
            pixelDisplay.device.Image.SetPixel(0, 0, Color.AliceBlue);
        }

        public override bool Setup(GpioController controller)
        {
            var settings = SettingsLoader.LoadConfig(@"Data/Settings.txt");
            var defaultValue = new KeyValuePair<string, string>("", "");

            if (settings.ContainsKey("Buzzer"))
            {
                settings.TryGetValue("Buzzer", out string BuzzerPin);
                var deviceBuzzer = new SkullBuzzer("Buzzer", int.Parse(BuzzerPin));
                outputDevices.Add(deviceBuzzer);
            }
            if (settings.ContainsKey("NeoPixel"))
            {
                settings.TryGetValue("NeoPixelCount", out string count);
                var deviceNeoPixel = new SkullNeoPixel("NeoPixel", int.Parse(count));
                outputDevices.Add(deviceNeoPixel);
            }

            if (settings.ContainsKey("LedPin"))
            {
                settings.TryGetValue("LedPin", out string pin);
                var deviceLed = new skullOS.Output.SkullLed("Life Light", int.Parse(pin), controller);
                outputDevices.Add(deviceLed);
            }

            return true;
        }

        public override void Stop()
        {
            throw new NotImplementedException();
        }
    }
}