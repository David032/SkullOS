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
            TurnOn(light);
            await Task.Delay(750);
            TurnOff(light);
        }

        public void TurnOn(string light)
        {
            int pin = LEDs[light];
            if (!controller.IsPinOpen(pin))
            {
                controller.OpenPin(pin, PinMode.Output);
            }
            controller.Write(pin, PinValue.High);
        }

        public void TurnOff(string light)
        {
            int pin = LEDs[light];
            if (!controller.IsPinOpen(pin))
            {
                controller.OpenPin(pin, PinMode.Output);
            }
            controller.Write(pin, PinValue.Low);
        }
    }
}
