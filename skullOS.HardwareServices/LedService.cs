using skullOS.HardwareServices.Interfaces;

namespace skullOS.HardwareServices
{
    public class LedService : ILedService
    {
        public Dictionary<string, int> LEDs { get; private set; }

        public LedService(Dictionary<string, int> leds)
        {
            LEDs = leds;
        }
    }
}
