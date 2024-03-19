using Iot.Device.Camera.Settings;
using Iot.Device.Common;
using skullOS.HardwareServices.Exceptions;
using skullOS.HardwareServices.Interfaces;
using System.Diagnostics;

namespace skullOS.HardwareServices
{
    public class CameraService : ICameraService
    {
        private ProcessSettings _processSettings;
        int xResolution;
        int yResolution;


        public CameraService(int x = 2592, int y = 1944)
        {
            _processSettings = ProcessSettingsFactory.CreateForLibcamerastill();
            xResolution = x;
            yResolution = y;
        }

        public async Task<string> RecordShortVideoAsync(string fileLocation, bool useMic)
        {
            try
            {
                using Process videoRecording = new();
                string args = string.Empty;
                if (useMic)
                {
                    args = " --codec libav --hflip --vflip --libav-audio --width 1920 --height 1080 -t 30000 -o " + $"{fileLocation}"
                        + DateTime.Now.ToString("yyyyMMddHHmmss") + ".mp4";
                }
                else
                {
                    args = "--codec libav --hflip --vflip --width 1920 --height 1080 -t 30000 -o " + $"{fileLocation}"
                        + DateTime.Now.ToString("yyyyMMddHHmmss") + ".mp4";
                }
                videoRecording.StartInfo.UseShellExecute = false;
                videoRecording.StartInfo.FileName = "libcamera-vid";
                videoRecording.EnableRaisingEvents = true;
#if DEBUG
                await Console.Out.WriteLineAsync(videoRecording.StartInfo.Arguments);
#endif
                videoRecording.Start();
                await Task.WhenAny(Task.Delay(30000));
            }
            catch (CameraErrorException e)
            {
                await Console.Out.WriteLineAsync(e.Message);
                return "Camera errored when taking picture!";
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
            catch (CameraErrorException e)
            {
                await Console.Out.WriteLineAsync(e.Message);
                return "Camera errored when taking picture!";
            }

            return filename;
        }
    }
}
