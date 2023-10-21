using System.Device.Gpio;

namespace skullOS.Input
{
    public enum ButtonEvent
    {
        Press,
        Release
    }

    internal class Button
    {
        public int Pin = 25;
        public ButtonEvent EventType = ButtonEvent.Press;
        public string Name = "";

        public Button(string buttonName, int pin = 25, ButtonEvent action = ButtonEvent.Press)
        {
            Name = buttonName;
            Pin = pin;
            EventType = action;
        }

        public PinEventTypes getButtonEvent()
        {
            if (EventType == ButtonEvent.Press)
            {
                return PinEventTypes.Falling;
            }
            else
            {
                return PinEventTypes.Rising;
            }
        }
    }
}
