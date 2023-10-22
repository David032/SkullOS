using Iot.Device.Button;

namespace skullOS.Input
{
    internal class Button : InputDevice
    {
        public GpioButton GpioButton;

        public Button(string name, int pin = 0)
        {
            this.Name = name;
            this.pin = pin;
            this.GpioButton = new GpioButton(pin);
        }
    }
}
