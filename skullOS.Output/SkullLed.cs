using skullOS.Core.LED_Elements;
using System.Device.Gpio;

namespace skullOS.Output
{
    public class SkullLed : SkullOutputDevice
    {
        /// <summary>
        /// SOMETHING IN HERE ISN'T WORKING!
        /// </summary>
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

        public void ToggleState()
        {
            gpioController.Write(pin, ((ledOn) ? PinValue.High : PinValue.Low));
            ledOn = !ledOn;
        }
    }
}
