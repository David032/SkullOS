using Iot.Device.Buzzer;
using skullOS.HardwareServices.Interfaces;

namespace skullOS.HardwareServices
{
    public class BuzzerService : IBuzzerService
    {
        public Buzzer Buzzer { get; private set; }
        public BuzzerService(int pinNumber, Buzzer buzz = null)
        {
            if (buzz == null)
            {
                Buzzer = new Buzzer(pinNumber);
            }
            else
            {
                Buzzer = buzz;
            }
        }

        public void SetBuzzer(int pinNumber)
        {
            Buzzer = new Buzzer(pinNumber);
        }
    }
}
