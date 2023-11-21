using Iot.Device.Media;
using skullOS.HardwareServices.Interfaces;

namespace skullOS.HardwareServices
{
    public class SpeakerService : ISpeakerService
    {
        public SoundDevice Speaker { get; private set; }

        public SpeakerService(SoundConnectionSettings micSettings)
        {
            micSettings ??= new SoundConnectionSettings();
            Speaker = SoundDevice.Create(micSettings);
        }
    }
}
