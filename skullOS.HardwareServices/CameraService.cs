﻿using Iot.Device.Media;
using skullOS.HardwareServices.Interfaces;
using System.Diagnostics;

namespace skullOS.HardwareServices
{
    public class CameraService : ICameraService
    {
        public VideoDevice Camera { get; private set; }
        private Process? cameraCommand;
        private TaskCompletionSource<bool> eventHandled;

        public CameraService(VideoConnectionSettings cameraSettings = null)
        {
            cameraSettings ??= new(busId: 0, captureSize: (2592, 1944), pixelFormat: VideoPixelFormat.JPEG);
            Camera = VideoDevice.Create(cameraSettings);
            Camera.Settings.HorizontalFlip = true;
            Camera.Settings.VerticalFlip = true;
        }

        #region SharpCamera code(https://github.com/David032/sharpCamera) for recording video
        public void RecordVideo(string outputDirectory, int duration)
        {
            try
            {
                using (cameraCommand = new Process())
                {
                    string args = "-o " + outputDirectory + "/" + DateTime.Now.ToString("yyyyMMddHHmmss") + ".h264 -t " + (duration * 1000);
                    cameraCommand.StartInfo.UseShellExecute = false;
                    cameraCommand.StartInfo.FileName = "raspivid";
                    cameraCommand.StartInfo.Arguments = args;

#if DEBUG
                    Console.Out.WriteLine(cameraCommand.StartInfo.Arguments);
#endif

                    cameraCommand.Start();
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }

        public async Task RecordVideoAsync(string outputDirectory, int duration)
        {
            eventHandled = new TaskCompletionSource<bool>();
            using (cameraCommand = new Process())
            {
                try
                {
                    string args = "-o " + outputDirectory + "/" + DateTime.Now.ToString("yyyyMMddHHmmss") + ".h264 -t " + (duration * 1000);
                    cameraCommand.StartInfo.UseShellExecute = false;
                    cameraCommand.StartInfo.FileName = "raspivid";
                    cameraCommand.StartInfo.Arguments = args;
                    cameraCommand.EnableRaisingEvents = true;
                    cameraCommand.Exited += new EventHandler(VideoRecorded);
#if DEBUG
                    Console.Out.WriteLine(cameraCommand.StartInfo.Arguments);
#endif
                    cameraCommand.Start();
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                    return;
                }

                await Task.WhenAny(eventHandled.Task, Task.Delay(duration * 1000));
            }
        }

        private void VideoRecorded(object sender, EventArgs e)
        {
            eventHandled.TrySetResult(true);
        }
        #endregion
    }
}