using Iot.Device.Camera.Settings;
using Iot.Device.Common;
using Iot.Device.Media;
using skullOS.HardwareServices.Interfaces;

namespace skullOS.HardwareServices
{
    public class CameraService : ICameraService
    {
        public VideoDevice Camera { get; private set; }

        private readonly ProcessSettings _processSettings;


        public CameraService(VideoConnectionSettings? cameraSettings = null)
        {
            cameraSettings ??= new(busId: 0, captureSize: (2592, 1944), pixelFormat: VideoPixelFormat.JPEG);
            Camera = VideoDevice.Create(cameraSettings);
            Camera.Settings.HorizontalFlip = true;
            Camera.Settings.VerticalFlip = true;

            _processSettings = ProcessSettingsFactory.CreateForLibcamerastill();
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

            using var proc = new ProcessRunner(_processSettings);
            Console.WriteLine("Using the following command line:");
            Console.WriteLine(proc.GetFullCommandLine(args));
            Console.WriteLine();

            var timestamp = DateTime.Now.ToString("yyyyMMddHHmmss");
            string? filename = fileLocation + timestamp + ".jpg";
            //string? filename = $"{DateTime.Now:yyyyMMddHHmmss}.jpg"; //Fakename
            try
            {
                using var file = File.OpenWrite(filename);
                await proc.ExecuteAsync(args, file);
            }
            catch (Exception)
            {
                await Console.Out.WriteLineAsync("Cam errored!");
            }

            return filename;
        }
    }
}
