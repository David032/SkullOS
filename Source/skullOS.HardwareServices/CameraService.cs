using Iot.Device.Camera.Settings;
using Iot.Device.Common;
using skullOS.HardwareServices.Interfaces;

namespace skullOS.HardwareServices
{
    public class CameraService : ICameraService
    {
        private ProcessSettings _processSettings;
        int xResolution;
        int yResolution;


        public CameraService(int x = 2592, int y = 1944)
        {
            xResolution = x;
            yResolution = y;
        }

        //Why are we back to hard-calling libcamera again?! :(
        //Remember that this needs rpicam-apps full to be installed(lite os doesn't come with it)
        //TODO: Still needs quality setting
        public async Task<string> RecordShortVideoAsync(string fileLocation, bool useMic)
        {
            var processSettings = ProcessSettingsFactory.CreateForLibcameravid();
            var builder = new CommandOptionsBuilder()
                .WithContinuousStreaming()
                .WithVflip()
                .WithHflip()
                .WithResolution(1920, 1080);
            var args = builder.GetArguments();
            using var proc = new ProcessRunner(processSettings);

            var timestamp = DateTime.Now.ToString("yyyyMMddHHmmss");
            string? filename = fileLocation + timestamp + ".h264";
            using var file = File.OpenWrite(filename);

            var task = await proc.ContinuousRunAsync(args, file);
            await Task.Delay(30000);
            proc.Dispose();
            try
            {
                await task;
            }
            catch (Exception)
            {
            }

            return $"({DateTime.Now}) Short video recorded!";
        }

        public async Task<string> TakePictureAsync(string fileLocation)
        {
            var builder = new CommandOptionsBuilder()
                .WithTimeout(1)
                .WithVflip()
                .WithHflip()
                .WithPictureOptions(quality: 100)
                .WithResolution(xResolution, yResolution);
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
            catch (Exception)
            {
                await Console.Out.WriteLineAsync("Cam errored when taking picture!");
            }

            return $"({DateTime.Now}) Picture taken!";
        }
    }
}
