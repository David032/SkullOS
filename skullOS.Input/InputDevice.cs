using skullOS.Input.Interfaces;

namespace skullOS.Input
{
    internal abstract class InputDevice : ISkullInput
    {
        public string Name = "NameOfInput";
        public int pin = 0;
    }
}
