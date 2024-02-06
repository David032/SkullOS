using Iot.Device.Media;
using skullOS.HardwareServices.Interfaces;

namespace skullOS.HardwareServices
{
    public class MicrophoneService : IMicrophoneService
    {
        public SoundDevice Microphone { get; private set; }

        public MicrophoneService(SoundConnectionSettings? micSettings = null)
        {
            micSettings ??= new SoundConnectionSettings();
            Microphone = SoundDevice.Create(micSettings); //Can't create a microphone if there's no mic!
        }
    }
}
