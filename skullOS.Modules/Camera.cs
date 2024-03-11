using skullOS.Core;
using skullOS.HardwareServices;
using skullOS.Modules.Interfaces;
using System.Diagnostics;

namespace skullOS.Modules
{
    public enum CameraMode
    {
        Image,
        ShortVideo,
        ContinuousVideo
    }

    public class Camera : Module, ICameraModule
    {
        public CameraService CameraService;
        public MicrophoneService? MicrophoneService = null;
        public LedService? LedService = null;
        public CameraMode CameraMode = CameraMode.Image;
        public BuzzerService BuzzerService;

        bool useMic = false;
        private TaskCompletionSource<bool> eventHandled;

        public Camera()
        {
            FileManager.CreateSubDirectory("Captures");
            var cameraSettings = SettingsLoader.LoadConfig(@"Data/CameraSettings.txt");
            CameraService = new CameraService();
            if (cameraSettings.ContainsKey("UseMic"))
            {
                if (cameraSettings.TryGetValue("UseMic", out string shouldUseMic))
                {
                    if (bool.Parse(shouldUseMic))
                    {
                        MicrophoneService = new MicrophoneService();
                        useMic = true;
                    }
                    else
                    {
                        //No Mic desired
                    }
                }
            }
            if (cameraSettings.ContainsKey("CameraLight"))
            {
                if (cameraSettings.TryGetValue("CameraLight", out string lightPin))
                {
                    Dictionary<string, int> pins = new Dictionary<string, int>
                    {
                        { "CameraLight", int.Parse(lightPin) }
                    };
                    LedService = new LedService(pins);
                }
            }

            BuzzerService = new BuzzerService(13);
        }

        public async Task TakePicture()
        {
            if (LedService != null && LedService.LEDs.ContainsKey("CameraLight"))
            {
                LedService.BlinkLight("CameraLight");
            }
            BuzzerService.Buzzer.PlayTone(1500, 500);
            var result = await CameraService.TakePictureAsync($"{FileManager.GetSkullDirectory()}/Captures/");
            LogMessage(result);

        }

        //Why are we back to hard-calling libcamera again?! :(
        //Remember that this needs rpicam-apps full to be installed(lite os doesn't come with it)
        //TODO: Still needs quality setting
        public async Task RecordShortVideo()
        {
            eventHandled = new TaskCompletionSource<bool>();

            using Process videoRecording = new();
            string args = string.Empty;
            if (useMic)
            {
                args = " --codec libav --hflip --vflip --libav-audio --width 1920 --height 1080 -t 30000 -o " + $"{FileManager.GetSkullDirectory()}/Captures/"
                    + DateTime.Now.ToString("yyyyMMddHHmmss") + ".mp4";
            }
            else
            {
                args = "--codec libav --hflip --vflip --width 1920 --height 1080 -t 30000 -o " + $"{FileManager.GetSkullDirectory()}/Captures/"
                    + DateTime.Now.ToString("yyyyMMddHHmmss") + ".mp4";
            }
            videoRecording.StartInfo.UseShellExecute = false;
            videoRecording.StartInfo.FileName = "libcamera-vid";
            videoRecording.EnableRaisingEvents = true;
            videoRecording.Exited += VideoRecording_Exited;
#if DEBUG
            await Console.Out.WriteLineAsync(videoRecording.StartInfo.Arguments);
#endif
            videoRecording.Start();
            await Task.WhenAny(eventHandled.Task, Task.Delay(30000));
        }

        private void VideoRecording_Exited(object? sender, EventArgs e)
        {
            eventHandled.TrySetResult(true);
        }

        public override void OnEnable(string[] args)
        {
            if (Enum.TryParse(args[0], out CameraMode mode))
            {
                CameraMode = mode;
            }
            Console.WriteLine("Camera mode set to " + CameraMode);
        }

        public override string ToString()
        {
            return "Camera";
        }

        public override void OnAction(object? sender, EventArgs e)
        {
            switch (CameraMode)
            {
                case CameraMode.Image:
                    TakePicture();
                    break;
                case CameraMode.ShortVideo:
                    RecordShortVideo();
                    break;
                case CameraMode.ContinuousVideo:
                    break;
                default:
                    break;
            }
        }
    }
}
