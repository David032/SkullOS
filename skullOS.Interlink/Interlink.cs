using skullOS.Core;
using skullOS.Core.Interfaces;
using System.Device.Gpio;

namespace skullOS.Interlink
{
    public class Interlink : Controller
    {
        Camera.Camera cameraModule;
        Output.Output outputModule;


        public override void Run(GpioController controller)
        {
            throw new NotImplementedException();
        }

        public override bool Setup(GpioController controller)
        {
            throw new NotImplementedException();
        }

        public override void Stop()
        {
            throw new NotImplementedException();
        }

        public void Link(List<ISubSystem> activeModules)
        {
            cameraModule = (Camera.Camera)activeModules.Select(x => x).Where(x => x.ToString() == "Camera").FirstOrDefault();
            outputModule = (Output.Output)activeModules.Select(x => x).Where(x => x.ToString() == "Output").FirstOrDefault();

            cameraModule.GetButton().Press += PlayBuzzerWhenActivated;
            //Need to do picture led
        }



        #region Cross Module functions

        private void PlayBuzzerWhenActivated(object? sender, EventArgs e)
        {
            Output.SkullBuzzer? buzzer = (Output.SkullBuzzer)outputModule.outputDevices.Select(x => x).FirstOrDefault(x => x.Name == "Buzzer");
            buzzer?.device.PlayTone(300, 1);
        }

        #endregion
    }
}