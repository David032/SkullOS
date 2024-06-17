using Iot.Device.ServoMotor;
using skullOS.Core;
using skullOS.HardwareServices;
using skullOS.HardwareServices.Interfaces;
using skullOS.Modules.Exceptions;
using System.Device.Pwm.Drivers;
using Timer = System.Timers.Timer;

namespace skullOS.Modules
{
    public class Prop : Module, IPropModule
    {
        ISpeakerService SpeakerService;
        ILedService LedService;

        static Timer? PlayIdleSound;
        double interval = 30000;
        string[] sounds;
        int numberOfIdles;

        ServoMotor leftFlap;
        ServoMotor rightFlap;

        Dictionary<string, string> propSettings;
        public Prop(ISpeakerService speaker = null, ILedService leds = null,
            string pathToSettings = @"Data/PropSettings.txt")
        {
            propSettings = SettingsLoader.LoadConfig(pathToSettings);

            propSettings.TryGetValue("Sounds", out string soundsState);
            bool useSounds = bool.Parse(soundsState);
            if (propSettings.ContainsKey("Sounds") && useSounds)
            {
                if (speaker == null)
                {
                    SpeakerService = new SpeakerService();
                }
                else
                {
                    SpeakerService = speaker;
                }

                SpeakerService.PlayAudio(@"Resources/computer-startup-music.mp3"); //This one won't await :(
                sounds = Directory.GetFiles(@"Resources/Haro/Idles"); //This shouldn't be hardcoded like this at all >:(
                numberOfIdles = sounds.Length;

                PlayIdleSound = new Timer(interval)
                {
                    AutoReset = true
                };
                PlayIdleSound.Elapsed += PlayIdleSound_Elapsed;
                PlayIdleSound.Start();
            }

            propSettings.TryGetValue("Lights", out string lightsState);
            bool useLights = bool.Parse(lightsState);
            if (propSettings.ContainsKey("Lights") && useLights)
            {
                //LEDs should be read from the file as well
                Dictionary<string, int> pins = new()
                {
                        { "LeftEye", 26 },
                        {"RightEye", 26 }
                    };

                if (leds == null)
                {
                    LedService = new LedService(pins);
                }
                else
                {
                    LedService = leds;
                    LedService.SetLeds(pins);
                }

                foreach (var item in LedService.GetLeds())
                {
                    string pin = item.Key;
                    LedService.TurnOn(pin);
                }
            }

            propSettings.TryGetValue("Servos", out string servosState);
            bool useServos = bool.Parse(servosState);
            if (propSettings.ContainsKey("Servos") && useServos)
            {
                SoftwarePwmChannel leftPWM = new(5, 50);
                leftFlap = new ServoMotor(leftPWM);
                leftFlap.Start();
                SoftwarePwmChannel rightPWM = new(6, 50);
                rightFlap = new ServoMotor(rightPWM);
                rightFlap.Start();
            }
        }

        public Prop()
        {
            propSettings = SettingsLoader.LoadConfig(@"Data/PropSettings.txt");

            propSettings.TryGetValue("Sounds", out string soundsState);
            bool useSounds = bool.Parse(soundsState);
            if (propSettings.ContainsKey("Sounds") && useSounds)
            {
                SpeakerService = new SpeakerService();

                SpeakerService.PlayAudio(@"Resources/computer-startup-music.mp3"); //This one won't await :(
                sounds = Directory.GetFiles(@"Resources/Haro/Idles"); //This shouldn't be hardcoded like this at all >:(
                numberOfIdles = sounds.Length;

                PlayIdleSound = new Timer(interval)
                {
                    AutoReset = true
                };
                PlayIdleSound.Elapsed += PlayIdleSound_Elapsed;
                PlayIdleSound.Start();
            }

            propSettings.TryGetValue("Lights", out string lightsState);
            bool useLights = bool.Parse(lightsState);
            if (propSettings.ContainsKey("Lights") && useLights)
            {
                //LEDs should be read from the file as well
                Dictionary<string, int> pins = new()
                {
                        { "LeftEye", 26 },
                        {"RightEye", 26 }
                    };

                LedService = new LedService(pins);

                foreach (var item in LedService.GetLeds())
                {
                    string pin = item.Key;
                    LedService.TurnOn(pin);
                }
            }

            propSettings.TryGetValue("Servos", out string servosState);
            bool useServos = bool.Parse(servosState);
            if (propSettings.ContainsKey("Servos") && useServos)
            {
                SoftwarePwmChannel leftPWM = new(5, 50);
                leftFlap = new ServoMotor(leftPWM);
                leftFlap.Start();
                SoftwarePwmChannel rightPWM = new(6, 50);
                rightFlap = new ServoMotor(rightPWM);
                rightFlap.Start();
            }
        }

        private void PlayIdleSound_Elapsed(object? sender, System.Timers.ElapsedEventArgs e)
        {
            Random random = new Random();
            int selection = random.Next(0, numberOfIdles + 1);
            SpeakerService.PlayAudio(sounds[selection]);

            propSettings.TryGetValue("Servos", out string servosState);
            bool useServos = bool.Parse(servosState);
            if (propSettings.ContainsKey("Servos") && useServos)
            {
                if (random.NextSingle() <= 0.5)
                {
                    FlapEar(true);
                }
                else
                {
                    FlapEar(false);
                }
            }
        }

        private async void FlapEar(bool useLeft = true)
        {
            if (useLeft)
            {
                leftFlap.WritePulseWidth(1500);
                await Task.Delay(1500);
                leftFlap.WritePulseWidth(0);
            }
            else
            {
                rightFlap.WritePulseWidth(1500);
                await Task.Delay(1500);
                rightFlap.WritePulseWidth(0);
            }

        }

        public override void OnAction(object? sender, EventArgs e)
        {
            throw new OnActionException("Prop doesn't support OnAction");
        }

        public override void OnEnable(string[] args)
        {
            throw new OnEnableException("Prop doesn't support OnEnable");
        }
        public override string ToString()
        {
            return "Prop";
        }

        public override void Create()
        {
            throw new NotImplementedException();
        }
    }
}
