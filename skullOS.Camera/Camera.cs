using Iot.Device.Button;
using Iot.Device.Media;
using skullOS.Core;

namespace skullOS.Camera
{
    public class Camera : Controller
    {
        //capture size is set to QHD, which is supported by the zero cam
        VideoDevice device;
        VideoConnectionSettings deviceSettings = new(busId: 0, captureSize: (2560, 1440), pixelFormat: VideoPixelFormat.JPEG);

        public Camera()
        {

        }

        public override void Run()
        {
        }

        public override bool Setup()
        {
            var settings = SettingsLoader.LoadConfig(@"Data/Settings.txt");

            var cameraMode = settings
                .Select(x => x)
                .Where(x => x.Key == "Mode")
                .FirstOrDefault();
            var pinToActOn = settings
                .Select(x => x)
                .Where(x => x.Key == "Pin")
                .FirstOrDefault();

            switch (cameraMode.Value)
            {
                case "Image":
                    device = VideoDevice.Create(deviceSettings);
                    GpioButton button = new(int.Parse(pinToActOn.Value));
                    button.Press += (sender, e) =>
                    {
                        Console.WriteLine($"({DateTime.Now}) Picture taken!");
                        device.Capture($"{DateTime.Now:yyyyMMddHHmmss}.jpg");
                    };

                    break;
                case "Video":
                    break;
                case "Both":
                    break;

                default:
                    throw new Exception("Camera mode not recognised!");
            }

            return true;
        }

        public override void Stop()
        {
            throw new NotImplementedException();
        }
    }
}
