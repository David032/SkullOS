using Iot.Device.Bmp180;
using Iot.Device.Bmxx80;
using Iot.Device.Buzzer;
using Iot.Device.Ws28xx;
using skullOS.Core;
using System.Device.Gpio;
using System.Device.I2c;
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
            var operationLED = (OutputLED)outputDevices.Select(x => x).FirstOrDefault(x => x.Name == "Life Light");
            operationLED.TurnOn();
            var cameraLED = (OutputLED)outputDevices.Select(x => x).FirstOrDefault(x => x.Name == "Camera Light");
            cameraLED.TurnOn();

            Ws2812b? pixelDisplay = (Ws2812b)outputDevices.Select(x => x).FirstOrDefault(x => x.Name == "NeoPixel").Device;
            pixelDisplay?.Image.SetPixel(1, 1, Color.Green);
            pixelDisplay?.Image.SetPixel(0, 1, Color.Blue);
            pixelDisplay.Update();

            var buzzer = (Buzzer)outputDevices.Select(x => x).FirstOrDefault(x => x.Name == "Buzzer").Device;
            buzzer.PlayTone(1000, 3000);

            var sensor = (Bmp180)outputDevices.Select(x => x).FirstOrDefault(x => x.Name == "Env Sensor").Device;
            Console.WriteLine(sensor.ReadTemperature());

            cameraLED.TurnOff();
            pixelDisplay.Image.Clear();
            pixelDisplay.Update();
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


            if (settings.ContainsKey("LedPin"))
            {
                if (settings.TryGetValue("LedPin", out string pin))
                {
                    int lifeLedPinNumber = Convert.ToInt32(pin);
                    OutputLED operationalLED = new("Life Light", lifeLedPinNumber, controller);
                    outputDevices.Add(operationalLED);
                }
            }

            if (settings.ContainsKey("CameraLed"))
            {
                if (settings.TryGetValue("CameraLed", out string pin))
                {
                    int cameraLedPinNumber = Convert.ToInt32(pin);
                    OutputLED cameraLED = new("Camera Light", cameraLedPinNumber, controller);
                    outputDevices.Add(cameraLED);
                }
            }

            if (settings.ContainsKey("BMP180"))
            {
                const int busId = 1;
                I2cConnectionSettings i2cSettings = new(busId, Bme280.DefaultI2cAddress);
                using I2cDevice i2cDevice = I2cDevice.Create(i2cSettings);

                var sensor = new Bmp180(i2cDevice);
                outputDevices.Add(new OutputDevice("Env Sensor", sensor));
            }

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

    public class OutputLED : OutputDevice
    {
        public int Pin = 0;
        public GpioController Controller;

        public OutputLED(string name, int pin, GpioController gpio, object device = null) : base(name, device)
        {
            Pin = pin;
            Controller = gpio;
        }

        public void TurnOn()
        {
            if (!Controller.IsPinOpen(Pin))
            {
                Controller.OpenPin(Pin, PinMode.Output);
            }
            Controller.Write(Pin, PinValue.High);
        }

        public void TurnOff()
        {
            if (!Controller.IsPinOpen(Pin))
            {
                Controller.OpenPin(Pin, PinMode.Output);
            }
            Controller.Write(Pin, PinValue.Low);
        }
    }
}