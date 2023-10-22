using System.Device.Gpio;

namespace skullOS.Core.LED_Elements
{
    public enum LedBehaviour
    {
        On,
        Off,
        OnEvent
    }

    public class SkullLed
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

            gpioController.OpenPin(pin, PinMode.Output);
        }

        public void ToggleState()
        {
            gpioController.Write(pin, ((ledOn) ? PinValue.High : PinValue.Low));
        }
    }
}
