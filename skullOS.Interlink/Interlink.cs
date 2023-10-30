using Iot.Device.Bmp180;
using Iot.Device.Buzzer;
using skullOS.Core;
using skullOS.Core.Interfaces;
using skullOS.Output;
using System.Device.Gpio;
using System.Device.I2c;
using System.Net.NetworkInformation;

namespace skullOS.Interlink
{
    public class Interlink : Controller
    {
        public List<ISubSystem> subSystems = new();
        Camera.Camera cameraModule;
        Output.Output outputModule;


        public override void Run(GpioController controller)
        {
            Console.WriteLine("Interlink ran successfully!");
        }

        public override bool Setup(GpioController controller, I2cDevice i2CDevice)
        {
            Link();
            return true;
        }

        public override void Stop()
        {
            throw new NotImplementedException();
        }

        public void Link()
        {
            Console.WriteLine("Linkable modules!");
            foreach (var item in subSystems)
            {
                Console.WriteLine(item.ToString());
            }

            cameraModule = (Camera.Camera)subSystems.Select(x => x).Where(x => x.ToString() == "skullOS.Camera.Camera").FirstOrDefault();
            outputModule = (Output.Output)subSystems.Select(x => x).Where(x => x.ToString() == "skullOS.Output.Output").FirstOrDefault();

            cameraModule.GetButton().Press += FlashLightWhenActivatedAsync;
            cameraModule.GetButton().Press += PlayBuzzerWhenActivated;

            var autoEvent = new AutoResetEvent(false);
            //var temperatureCheck = new Timer(CheckTemperature, autoEvent, 0, 300000);
            var connectionCheck = new Timer(CheckForNetwork, autoEvent, 0, 30000);
        }



        #region Cross Module functions

        public void PlayBuzzerWhenActivated(object? sender, EventArgs e)
        {
            var buzzer = (Buzzer)outputModule.outputDevices.Select(x => x).FirstOrDefault(x => x.Name == "Buzzer").Device;
            buzzer.PlayTone(1500, 750);
        }

        private async void FlashLightWhenActivatedAsync(object? sender, EventArgs e)
        {
            var cameraLED = (OutputLED)outputModule.outputDevices.Select(x => x).FirstOrDefault(x => x.Name == "Camera Light");
            cameraLED.TurnOn();
            await Task.Delay(750);
            cameraLED.TurnOff();
        }

        public void CheckTemperature(object? state)
        {
            var warningLight = (OutputLED)outputModule.outputDevices.Select(x => x).FirstOrDefault(x => x.Name == "Danger Light");
            var temperatureDevice = (Bmp180)outputModule.outputDevices.Select(x => x).FirstOrDefault(x => x.Name == "Env Sensor").Device;

            var temp = temperatureDevice.ReadTemperature();
            if (temp.DegreesCelsius >= 30)
            {
                warningLight.TurnOn();
            }
            else
            {
                warningLight.TurnOff();
            }
        }

        public void CheckForNetwork(object? state)
        {
            var connectionLed = (OutputLED)outputModule.outputDevices.Select(x => x).FirstOrDefault(x => x.Name == "Alert Light");

            if (NetworkInterface.GetIsNetworkAvailable())
            {
                connectionLed.TurnOn();
            }
            else
            {
                connectionLed.TurnOff();
            }

        }

        #endregion
    }
}