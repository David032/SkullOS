using skullOS.HardwareServices.Interfaces;
using System.Diagnostics;

namespace skullOS.HardwareServices
{
    /// <summary>
    /// Service to communicate with audio output
    /// Requires `sudo apt install --no-install-recommends vlc-bin vlc-plugin-base` to have been ran first
    /// </summary>
    public class SpeakerService : ISpeakerService
    {
        public SpeakerService()
        {

        }

        public async Task PlayAudio(string filepath)
        {
            using Process soundPlayback = new Process();

            string args = " --play-and-exit " + filepath;
            soundPlayback.StartInfo.UseShellExecute = false;
            soundPlayback.StartInfo.FileName = "cvlc";
            soundPlayback.StartInfo.Arguments = args;
            soundPlayback.EnableRaisingEvents = true;
#if DEBUG
            await Console.Out.WriteLineAsync(soundPlayback.StartInfo.FileName + soundPlayback.StartInfo.Arguments);
#endif
            soundPlayback.Start();
            await soundPlayback.WaitForExitAsync();
        }
    }
}
