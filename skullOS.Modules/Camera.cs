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

        public Camera()
        {
            FileManager.CreateSubDirectory("Captures");
            CameraService = new CameraService();
            MicrophoneService = new MicrophoneService();
        }

        public void TakePicture()
        {
            // Console.WriteLine($"({DateTime.Now}) Picture taken!"); //Maybe think about a better logging solution?
            CameraService.Camera.Capture($"{FileManager.GetSkullDirectory()}/Captures/{DateTime.Now:yyyyMMddHHmmss}.jpg");
        }

        public void RecordShortVideo()
        {
            string audioLocation = $"{FileManager.GetSkullDirectory()}/Captures/{DateTime.Now:yyyyMMddHHmmss}.mp3";
            string videoLocation = $"{FileManager.GetSkullDirectory()}/Captures/{DateTime.Now:yyyyMMddHHmmss}";
            CameraService.RecordVideoAsync(FileManager.GetSkullDirectory() + "/Captures", 30);
            MicrophoneService.Microphone.Record(30, audioLocation);
        }
    }
}
