using Iot.Device.Button;
using Iot.Device.Media;
using skullOS.Core;
using System.Device.Gpio;
using System.Device.I2c;

namespace skullOS.Camera
{
    public class Camera : Controller
    {
        //capture size is set to QHD, which is supported by the zero cam
        VideoDevice device;
        VideoConnectionSettings deviceSettings = new(busId: 0, captureSize: (2592, 1944), pixelFormat: VideoPixelFormat.JPEG);

        GpioButton actionButton;

        public Camera()
        {

        }

        public override void Run(GpioController controller)
        {
        }

        public override bool Setup(GpioController controller, I2cDevice i2CDevice)
        {
            FileManager.CreateSubDirectory("Captures");
            Dictionary<string, string> settings = SettingsLoader.LoadConfig(@"Data/Settings.txt");
            KeyValuePair<string, string> defaultValue = new("", "");

            KeyValuePair<string, string> cameraMode = settings
                .Select(x => x)
                .Where(x => x.Key == "Mode")
                .FirstOrDefault(defaultValue);
            KeyValuePair<string, string> pinToActOn = settings
                .Select(x => x)
                .Where(x => x.Key == "Pin")
                .FirstOrDefault(defaultValue);
            KeyValuePair<string, string> ledPin = settings
                .Select(x => x)
                .Where(x => x.Key == "LedPin")
                .FirstOrDefault(defaultValue);

            switch (cameraMode.Value)
            {
                case "Image":
                    device = VideoDevice.Create(deviceSettings);
                    device.Settings.HorizontalFlip = true;
                    device.Settings.VerticalFlip = true;
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
            device.Capture($"{FileManager.GetSkullDirectory()}/Captures/{DateTime.Now:yyyyMMddHHmmss}.jpg");
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
