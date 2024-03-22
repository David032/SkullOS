using skullOS.Core;
using skullOS.HardwareServices;
using skullOS.HardwareServices.Interfaces;
using skullOS.Modules.Exceptions;
using skullOS.Modules.Interfaces;
using Timer = System.Timers.Timer;

namespace skullOS.Modules
{
    public class Adventure : Module, IAdventure
    {
        string directory = string.Empty;
        static Timer? takePicture;
        double interval = 30000;
        ICameraService cameraService;

        public Adventure()
        {
            var now = DateTime.Now.ToString("M");
            FileManager.CreateSubDirectory("Timelapse - " + now);
            directory = FileManager.GetSkullDirectory() + @"/Timelapse - " + now + @"/";
            cameraService = new CameraService();


            takePicture = new Timer(interval);
            takePicture.AutoReset = true;
            takePicture.Elapsed += TakePicture_Elapsed;
            takePicture.Start();
        }

        private void TakePicture_Elapsed(object? sender, System.Timers.ElapsedEventArgs e)
        {
            cameraService.TakePictureAsync(directory);
        }

        public override void OnAction(object? sender, EventArgs e)
        {
            throw new OnActionException("Adventure does not support OnAction");
        }

        public override void OnEnable(string[] args)
        {
            throw new OnEnableException("Adventure does not support OnEnable");
        }

        public override string ToString()
        {
            return "Adventure";
        }
    }
}
