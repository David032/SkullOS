using Iot.Device.Button;
using Iot.Device.Media;
using skullOS.Core;
using System.Device.Gpio;

namespace skullOS.Camera
{
    public class Camera : Controller
    {
        //capture size is set to QHD, which is supported by the zero cam
        VideoDevice device;
        VideoConnectionSettings deviceSettings = new(busId: 0, captureSize: (2560, 1440), pixelFormat: VideoPixelFormat.JPEG);

        GpioButton actionButton;

        public Camera()
        {

        }

        public override void Run(GpioController controller)
        {
        }

        public override bool Setup(GpioController controller)
        {
            var settings = SettingsLoader.LoadConfig(@"Data/Settings.txt");
            var defaultValue = new KeyValuePair<string, string>("", "");

            var cameraMode = settings
                .Select(x => x)
                .Where(x => x.Key == "Mode")
                .FirstOrDefault(defaultValue);
            var pinToActOn = settings
                .Select(x => x)
                .Where(x => x.Key == "Pin")
                .FirstOrDefault(defaultValue);
            var ledPin = settings
                .Select(x => x)
                .Where(x => x.Key == "LedPin")
                .FirstOrDefault(defaultValue);

            switch (cameraMode.Value)
            {
                case "Image":
                    device = VideoDevice.Create(deviceSettings);
                    actionButton = new(int.Parse(pinToActOn.Value));
                    actionButton.Press += TakePicture;

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

        private void TakePicture(object? sender, EventArgs e)
        {
            Console.WriteLine($"({DateTime.Now}) Picture taken!");
            device.Capture($"{DateTime.Now:yyyyMMddHHmmss}.jpg");
        }

        public override void Stop()
        {
            throw new NotImplementedException();
        }

        public VideoDevice GetCamera()
        {
            return device;
        }
        public GpioButton GetButton()
        {
            return actionButton;
        }
    }
}
