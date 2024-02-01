using skullOS.HardwareServices;
using skullOS.HardwareServices.Interfaces;
using Timer = System.Timers.Timer;

namespace skullOS.Modules
{
    public class Prop : Module, IPropModule
    {
        public ISpeakerService SpeakerService { get; set; }
        public ILedService LedService { get; set; }

        static Timer PlayIdleSound;
        double interval = 30000;

        string[] sounds;
        int numberOfIdles;

        public Prop()
        {
            SpeakerService = new SpeakerService();
            SpeakerService.PlayAudio(@"Resources/computer-startup-music.mp3"); //This one won't await :(
            sounds = Directory.GetFiles(@"Resources/Haro/Idles");
            numberOfIdles = sounds.Length;

            PlayIdleSound = new Timer(interval);
            PlayIdleSound.AutoReset = true;
            PlayIdleSound.Elapsed += PlayIdleSound_Elapsed;
            PlayIdleSound.Start();

            //Left and right eye, these are next to each other so it should be easy to tell
            Dictionary<string, int> pins = new Dictionary<string, int>
                    {
                        { "LeftEye", 22 },
                        {"RightEye", 23 }
                    };
            LedService = new LedService(pins);
            foreach (var item in LedService.GetLeds())
            {
                int pin = item.Value;
                LedService.TurnOn(pin);
            }
        }

        private void PlayIdleSound_Elapsed(object? sender, System.Timers.ElapsedEventArgs e)
        {
            var random = new Random();
            int selection = random.Next(0, numberOfIdles + 1);
            SpeakerService.PlayAudio(sounds[selection]);
        }

        public override void OnAction(object? sender, EventArgs e)
        {
            throw new NotImplementedException();
        }

        public override void OnEnable(string[] args)
        {
            throw new NotImplementedException();
        }
        public override string ToString()
        {
            return "Prop";
        }
    }
}
