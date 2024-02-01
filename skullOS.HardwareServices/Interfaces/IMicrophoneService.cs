using Iot.Device.Media;

namespace skullOS.HardwareServices.Interfaces
{
    public interface IMicrophoneService
    {
        SoundDevice GetMicrophone();
    }
}