using DeepSpeechClient.Interfaces;
using NAudio.Wave;
using skullOS.Core;
using skullOS.HardwareServices;
using skullOS.HardwareServices.Interfaces;
using skullOS.Modules.Interfaces;
using Timer = System.Timers.Timer;

namespace skullOS.Modules
{
    public class AiCamera : Module, ICameraModule
    {
        ICameraService CameraService { get; set; }
        IMicrophoneService MicrophoneService { get; set; }
        ISpeakerService SpeakerService { get; set; }

        string CameraSound = @"Resources/51360__thecheeseman__camera_snap1.mp3";

        static Timer CommandCheck;
        double interval = 10000;

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
        }

        private void CommandCheck_Elapsed(object? sender, System.Timers.ElapsedEventArgs e)
        {
            string file = FileManager.GetSkullDirectory() + "/Temp/Audio.mp3";
            if (File.Exists(file))
            {
                using IDeepSpeech sttClient = new DeepSpeechClient.DeepSpeech("output_graph.pbmm");
                var buffer = new WaveBuffer(File.ReadAllBytes(file));
                var info = new Mp3FileReader(file);

                var speechResult = sttClient.SpeechToText(buffer.ShortBuffer, Convert.ToUInt32(buffer.MaxSize / 2)).ToLower();
                var wordsSpoken = speechResult.Split();

                if (wordsSpoken.Contains("haro") && wordsSpoken.Contains("take") && wordsSpoken.Contains("picture"))
                {
                    TakePicture();
                }
            }
            MicrophoneService.GetMicrophone().Record(10, file);
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
    }
}
