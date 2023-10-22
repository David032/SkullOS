using Iot.Device.Buzzer;

namespace skullOS.Output
{
    internal class SkullBuzzer : SkullOutputDevice
    {
        int Pin = 0;
        public Buzzer device
        {
            get { return buzzer; }
        }
        Buzzer buzzer;


        public SkullBuzzer(string name, int pin)
        {
            this.Name = name;
            this.Pin = pin;
            buzzer = new Buzzer(pin);
        }
    }
}
