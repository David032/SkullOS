using skullOS.Core.LED_Elements;
using System.Device.Gpio;

namespace skullOS.Output
{
    public class SkullLed : SkullOutputDevice
    {
        public string name = "";
        public int pin = 0;
        public LedBehaviour state = LedBehaviour.Off;

        private GpioController gpioController;
        bool ledOn = false;

        public SkullLed(string LedName, int ledPin, GpioController controller)
        {
            name = LedName;
            pin = ledPin;
            gpioController = controller;
        }

        public void ToggleState()
        {
            gpioController.Write(pin, ((ledOn) ? PinValue.High : PinValue.Low));
        }
    }
}
