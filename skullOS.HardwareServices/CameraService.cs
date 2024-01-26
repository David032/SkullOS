using Iot.Device.Camera.Settings;
using Iot.Device.Common;
using skullOS.HardwareServices.Interfaces;
using System.Diagnostics;

namespace skullOS.HardwareServices
{
    public class CameraService : ICameraService
    {
        private Process? cameraCommand;
        private TaskCompletionSource<bool> eventHandled;
        private int xResolution = 0;
        private int yResolution = 0;

        private ProcessSettings _processSettings;


        public CameraService(int x = 2592, int y = 1944)
        {
            xResolution = x;
            yResolution = y;
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
            await Console.Out.WriteLineAsync("Picture file is at: " + filename);

            try
            {
                using var file = File.OpenWrite(filename);
                await proc.ExecuteAsync(args, file);
            }
            catch (Exception)
            {
                return "Cam errored!";
            }

            return $"({DateTime.Now}) Picture taken!";
        }

        //Adapted from the iot device sample for the camera
        public async Task<string> CaptureVideo(string fileLocation, int duration = 30)
        {
            var builder = new CommandOptionsBuilder()
                .WithContinuousStreaming(0)
                .WithVflip()
                .WithHflip()
                .WithResolution(xResolution, yResolution);
            var args = builder.GetArguments();

            _processSettings = ProcessSettingsFactory.CreateForLibcameravid();
            using var proc = new ProcessRunner(_processSettings);

            var timestamp = DateTime.Now.ToString("yyyyMMddHHmmss");
            var filename = fileLocation + timestamp + ".h264";
            await Console.Out.WriteLineAsync("Video file is at: " + filename);
            using var file = File.OpenWrite(filename);

            var task = await proc.ContinuousRunAsync(args, file);
            await Task.Delay(duration * 1000);
            proc.Dispose();
            // The following try/catch is needed to trash the OperationCanceledException triggered by the Dispose
            try
            {
                await task;
            }
            catch (Exception)
            {
            }

            return filename;
        }
    }
}
