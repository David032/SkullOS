using Iot.Device.Bmp180;
using Iot.Device.Buzzer;
using Iot.Device.Ws28xx;
using skullOS.Core;
using System.Device.Gpio;
using System.Device.I2c;
using System.Device.Spi;

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

            //Ws2812b? pixelDisplay = (Ws2812b)outputDevices.Select(x => x).FirstOrDefault(x => x.Name == "NeoPixel").Device;


            //pixelDisplay?.Image.SetPixel(1, 1, Color.Red); //D1
            //pixelDisplay?.Image.SetPixel(0, 1, Color.Green); //D2
            //pixelDisplay?.Image.SetPixel(2, 1, Color.Blue); //D3
            //pixelDisplay?.Image.SetPixel(6, 0, Color.White); //D4
            //pixelDisplay?.Image.SetPixel(4, 0, Color.Pink); //D5

            //pixelDisplay.Update();

            var buzzer = (Buzzer)outputDevices.Select(x => x).FirstOrDefault(x => x.Name == "Buzzer").Device;
            buzzer.PlayTone(1000, 1500);

            //var sensor = (Bmp180)outputDevices.Select(x => x).FirstOrDefault(x => x.Name == "Env Sensor").Device;
            //Console.WriteLine(sensor.ReadTemperature());

            cameraLED.TurnOff();
            //pixelDisplay.Image.Clear();
            //pixelDisplay.Update();
            //pixelDisplay?.Image.SetPixel(2, 1, Color.Blue); //D3

        }

        public override bool Setup(GpioController controller, I2cDevice i2cDevice)
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

            if (settings.ContainsKey("DangerLed"))
            {
                if (settings.TryGetValue("DangerLed", out string pin))
                {
                    int dangerLedPinNumber = Convert.ToInt32(pin);
                    OutputLED dangerLED = new("Danger Light", dangerLedPinNumber, controller);
                    outputDevices.Add(dangerLED);
                }
            }

            if (settings.ContainsKey("AlertLed"))
            {
                if (settings.TryGetValue("AlertLed", out string pin))
                {
                    int alertLedPinNumber = Convert.ToInt32(pin);
                    OutputLED alertLED = new("Alert Light", alertLedPinNumber, controller);
                    outputDevices.Add(alertLED);
                }
            }

            if (settings.ContainsKey("BMP180"))
            {
                var sensor = new Bmp180(i2cDevice);
                outputDevices.Add(new OutputDevice("Env Sensor", sensor));
            }

            AppDomain.CurrentDomain.ProcessExit += OnStopEvent;

            return true;
        }

        private void OnStopEvent(object? sender, EventArgs e)
        {
            foreach (var item in outputDevices)
            {
                if (item.Device is OutputLED ledDevice)
                {
                    ledDevice.TurnOff();
                }
                if (item.Device is Ws2812b neopixel)
                {
                    neopixel.Image.Clear();
                    neopixel.Update();
                }
            }
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