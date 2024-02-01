using skullOS.HardwareServices.Interfaces;
using System.Device.Gpio;

namespace skullOS.HardwareServices
{
    public class LedService : ILedService
    {
        public Dictionary<string, int> LEDs { get; private set; }
        GpioController controller;

        public LedService(Dictionary<string, int> leds)
        {
            LEDs = leds;
            controller = new GpioController();
        }

        public async void BlinkLight(string light)
        {
            int pin = LEDs[light];
            TurnOn(pin);
            await Task.Delay(750);
            TurnOff(pin);
        }

        public void TurnOn(int Pin)
        {
            if (!controller.IsPinOpen(Pin))
            {
                controller.OpenPin(Pin, PinMode.Output);
            }
            controller.Write(Pin, PinValue.High);
        }

        public void TurnOff(int Pin)
        {
            if (!controller.IsPinOpen(Pin))
            {
                controller.OpenPin(Pin, PinMode.Output);
            }
            controller.Write(Pin, PinValue.Low);
        }

        public Dictionary<string, int> GetLeds()
        {
            return LEDs;
        }
    }
}
