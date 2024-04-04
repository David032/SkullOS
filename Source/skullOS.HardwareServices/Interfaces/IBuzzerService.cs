using Iot.Device.Buzzer;

namespace skullOS.HardwareServices.Interfaces
{
    public interface IBuzzerService
    {
        Buzzer Buzzer { get; }

        void SetBuzzer(int pinNumber);
    }
}