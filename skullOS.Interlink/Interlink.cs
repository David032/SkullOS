using Iot.Device.Buzzer;
using skullOS.Core;
using skullOS.Core.Interfaces;
using System.Device.Gpio;

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

        public override bool Setup(GpioController controller)
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

            cameraModule.GetButton().Press += PlayBuzzerWhenActivated;
            cameraModule.GetButton().Press += FlashLightWhenActivatedAsync;

            Console.WriteLine(cameraModule.GetCamera().Settings);
        }



        #region Cross Module functions

        public void PlayBuzzerWhenActivated(object? sender, EventArgs e)
        {
            var buzzer = (Buzzer)outputModule.outputDevices.Select(x => x).FirstOrDefault(x => x.Name == "Buzzer").Device;
            buzzer.PlayTone(1500, 1500);
        }

        private async void FlashLightWhenActivatedAsync(object? sender, EventArgs e)
        {
            //Output.SkullLed? led = (Output.SkullLed)outputModule.outputDevices.Select(x => x).FirstOrDefault(x => x.Name == "Camera Light");
            //led.TurnOn();
            //await Task.Delay(15000);
            //led.TurnOff();
        }

        #endregion
    }
}