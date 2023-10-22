using skullOS.Core.LED_Elements;
using System.Device.Gpio;

namespace skullOS.Output
{
    public class SkullLed : SkullOutputDevice
    {
        public int pin = 0;
        public LedBehaviour state = LedBehaviour.Off;

        private GpioController gpioController;
        bool ledOn = false;

        public SkullLed(string LedName, int ledPin, GpioController controller)
        {
            Name = LedName;
            pin = ledPin;
            gpioController = controller;

            gpioController.OpenPin(pin, PinMode.Output);
        }

        [Obsolete]
        public void ToggleState()
        {
            gpioController.Write(pin, ((ledOn) ? PinValue.High : PinValue.Low));
            ledOn = !ledOn;
        }

        public void TurnOn()
        {
            if (!gpioController.IsPinOpen(pin))
            {
                gpioController.OpenPin(pin, PinMode.Output);
            }
            gpioController.Write(pin, PinValue.High);
        }

        public void TurnOff()
        {
            if (!gpioController.IsPinOpen(pin))
            {
                gpioController.OpenPin(pin, PinMode.Output);
            }
            gpioController.Write(pin, PinValue.Low);
        }
    }
}
