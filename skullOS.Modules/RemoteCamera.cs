using skullOS.Core;
using skullOS.HardwareServices;
using skullOS.HardwareServices.Interfaces;
using skullOS.Modules.Interfaces;
using System.Diagnostics;

namespace skullOS.Modules
{
    public class RemoteCamera : Module, ICameraModule
    {
        ICameraService CameraService { get; set; }
        ISpeakerService SpeakerService { get; set; }
        string CameraSound = @"Resources/cameraSnap.mp3";
        Process remoteControl;

        public RemoteCamera()
        {
            CameraService = new CameraService();
            SpeakerService = new SpeakerService();

            FileManager.CreateSubDirectory("Captures");

            remoteControl = new Process();

            remoteControl.StartInfo.UseShellExecute = false;
            remoteControl.StartInfo.FileName = "python3";
            remoteControl.StartInfo.Arguments = " Python/RemoteCamera.py";


            remoteControl.Start();
            Console.WriteLine("Remote Cam setup done");
        }

        void ICameraModule.RecordShortVideo()
        {
            throw new NotImplementedException();
        }

        void ICameraModule.TakePicture()
        {
            throw new NotImplementedException();
        }

        public override void OnEnable(string[] args)
        {
            throw new NotImplementedException();
        }

        public override void OnAction(object? sender, EventArgs e)
        {
            throw new NotImplementedException();
        }

        public override string ToString()
        {
            return "Remote Camera";
        }
    }
}
