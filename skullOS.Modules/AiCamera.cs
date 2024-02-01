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

        string CameraSound = @"Resources/cameraSnap.mp3";

        static Timer CommandCheck;
        double interval = 10000;

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
            File.Create(tempFile);
        }

        private void CommandCheck_Elapsed(object? sender, System.Timers.ElapsedEventArgs e)
        {
#if DEBUG
            logger.LogMessage("Checking for audio prompt...");
#endif
            if (File.Exists(tempFile))
            {
                using IDeepSpeech sttClient = new DeepSpeechClient.DeepSpeech("output_graph.pbmm");
                var buffer = new WaveBuffer(File.ReadAllBytes(tempFile));
                var info = new WaveFileReader(tempFile);
                Console.WriteLine("Length: " + info.Length + ". Total time: " + info.TotalTime);

                var speechResult = sttClient.SpeechToText(buffer.ShortBuffer, Convert.ToUInt32(buffer.MaxSize / 2)).ToLower();
                var wordsSpoken = speechResult.Split();


                if (wordsSpoken.Contains("haro") && wordsSpoken.Contains("take") && wordsSpoken.Contains("picture"))
                {
                    logger.LogMessage("Heard the command phase!");
                    TakePicture();
                }
                else if (wordsSpoken.Contains("take") && wordsSpoken.Contains("picture"))
                {
                    logger.LogMessage("Might've heard the command phase?");
                    TakePicture();
                }
                else if (wordsSpoken.Contains("haro"))
                {
                    logger.LogMessage("Heard my name?");
                    TakePicture();
                }
                else
                {
#if DEBUG
                    Console.WriteLine(speechResult);
#endif
                }
            }
            else
            {
                Console.WriteLine("No audio file?!");
            }
            MicrophoneService.GetMicrophone().Record(10, tempFile);
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
