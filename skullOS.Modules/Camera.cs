using skullOS.Core;
using skullOS.HardwareServices;
using skullOS.Modules.Interfaces;

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
        public MicrophoneService MicrophoneService;
        public CameraMode CameraMode = CameraMode.Image;

        bool useMic = false;

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
        }

        public void TakePicture()
        {
            logger.LogMessage($"({DateTime.Now}) Picture taken!"); //Maybe think about a better logging solution?
            CameraService.Camera.Capture($"{FileManager.GetSkullDirectory()}/Captures/{DateTime.Now:yyyyMMddHHmmss}.jpg");
        }

        public void RecordShortVideo()
        {
            string audioLocation = $"{FileManager.GetSkullDirectory()}/Captures/{DateTime.Now:yyyyMMddHHmmss}.mp3";
            string videoLocation = $"{FileManager.GetSkullDirectory()}/Captures/{DateTime.Now:yyyyMMddHHmmss}";
            CameraService.RecordVideoAsync(FileManager.GetSkullDirectory() + "/Captures", 30);
            if (useMic)
            {
                MicrophoneService.Microphone.Record(30, audioLocation);
            }
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
