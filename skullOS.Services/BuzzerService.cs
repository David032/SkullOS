using Iot.Device.Buzzer;
using skullOS.Services.Interfaces;

namespace skullOS.Services
{
    public class BuzzerService : IBuzzerService
    {
        public Buzzer Buzzer { get; private set; }

        public BuzzerService(int pinNumber)
        {
            Buzzer = new Buzzer(pinNumber);
        }
    }
}
