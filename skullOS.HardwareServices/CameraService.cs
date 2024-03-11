using Iot.Device.Camera.Settings;
using Iot.Device.Common;
using skullOS.HardwareServices.Interfaces;

namespace skullOS.HardwareServices
{
    public class CameraService : ICameraService
    {
        public VideoDevice Camera { get; private set; }

        private ProcessSettings _processSettings;


        public CameraService(VideoConnectionSettings? cameraSettings = null)
        {
            xResolution = x;
            yResolution = y;
        }

        public async Task<string> TakePictureAsync(string fileLocation, int x = 2592, int y = 1944)
        {
            var builder = new CommandOptionsBuilder()
                .WithTimeout(1)
                .WithVflip()
                .WithHflip()
                .WithPictureOptions(quality: 100)
                .WithResolution(x, y);
            var args = builder.GetArguments();

            _processSettings = ProcessSettingsFactory.CreateForLibcamerastill();
            using var proc = new ProcessRunner(_processSettings);

            var timestamp = DateTime.Now.ToString("yyyyMMddHHmmss");
            string? filename = fileLocation + timestamp + ".jpg";

            try
            {
                using var file = File.OpenWrite(filename);
                await proc.ExecuteAsync(args, file);
            }
            catch (Exception e)
            {
                await Console.Out.WriteLineAsync(e.Message);
                return "Cam errored!";
            }

            return $"({DateTime.Now}) Picture taken!";
        }
    }
}
