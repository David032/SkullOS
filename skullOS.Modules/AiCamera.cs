using skullOS.Core;
using skullOS.HardwareServices;
using skullOS.HardwareServices.Interfaces;
using skullOS.Modules.Interfaces;
using System.Diagnostics;
using Timer = System.Timers.Timer;

namespace skullOS.Modules
{
    public class AiCamera : Module, ICameraModule
    {
        ICameraService CameraService { get; set; }
        IMicrophoneService MicrophoneService { get; set; }
        ISpeakerService SpeakerService { get; set; }

        string CameraSound = @"Resources/cameraSnap.mp3";

        static Timer CommandCheck;
        double interval = 10500;

        string tempFile;
        public AiCamera()
        {
            CameraService = new CameraService();
            MicrophoneService = new MicrophoneService();
            SpeakerService = new SpeakerService();

            FileManager.CreateSubDirectory("Captures");
            FileManager.CreateSubDirectory("Temp");

            CommandCheck = new Timer(interval);
            CommandCheck.AutoReset = true;
            CommandCheck.Elapsed += CommandCheck_Elapsed;
            CommandCheck.Start();

            tempFile = FileManager.GetSkullDirectory() + "/Temp/Audio.wav";
        }

        private void CommandCheck_Elapsed(object? sender, System.Timers.ElapsedEventArgs e)
        {
#if DEBUG
            logger.LogMessage("Checking for audio prompt...");
#endif
            RecordAudio();
        }

        public override void OnAction(object? sender, EventArgs e)
        {
            throw new NotImplementedException();
        }

        public override void OnEnable(string[] args)
        {
            throw new NotImplementedException();
        }

        public override string ToString()
        {
            return "Smart Camera";
        }

        public void RecordShortVideo()
        {
            throw new NotImplementedException();
        }

        public void TakePicture()
        {
            CameraService.TakePictureAsync($"{FileManager.GetSkullDirectory()}/Captures/");
            LogMessage($"({DateTime.Now}) Picture taken!");
            SpeakerService.PlayAudio(CameraSound);
        }

        public void RecordAudio()
        {
            using (Process audioRecording = new Process())
            {
                string args = " -D plughw:1,0 --duration=10 " + tempFile;

                audioRecording.StartInfo.FileName = "arecord";
                audioRecording.StartInfo.Arguments = args;
                audioRecording.Start();
            }
        }
    }
}
