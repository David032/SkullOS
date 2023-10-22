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

        skullOS.Core.LED_Elements.SkullLed cameraLight;
        bool hasCameraLed = false;
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

            if (!ledPin.Equals(defaultValue))
            {
                cameraLight = new Core.LED_Elements.SkullLed("Camera Light", int.Parse(ledPin.Value), controller);
                hasCameraLed = true;
            }

            switch (cameraMode.Value)
            {
                case "Image":
                    device = VideoDevice.Create(deviceSettings);
                    GpioButton button = new(int.Parse(pinToActOn.Value));
                    button.Press += (sender, e) =>
                    {
                        if (hasCameraLed)
                        {
                            cameraLight.ToggleState();
                        }
                        Console.WriteLine($"({DateTime.Now}) Picture taken!");
                        device.Capture($"{DateTime.Now:yyyyMMddHHmmss}.jpg");
                        if (hasCameraLed)
                        {
                            cameraLight.ToggleState();
                        }
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
