using skullOS.Core;
using skullOS.HardwareServices;
using skullOS.HardwareServices.Interfaces;
using skullOS.Modules.Interfaces;

namespace skullOS.Modules
{
    public enum CameraMode
    {
        Image,
        ShortVideo,
        ContinuousVideo
    }

    public class Camera : Module, ICameraModule
    {
        public ICameraService CameraService;
        public IMicrophoneService? MicrophoneService = null;
        public ISpeakerService? SpeakerService = null;
        public ILedService? LedService = null;
        public IBuzzerService? BuzzerService = null;


        public CameraMode CameraMode = CameraMode.Image;

        bool useMic = false;
        bool useSpeaker = false;
        bool useBuzzer = false;
        bool isActive = false;

        public Camera(ICameraService camService = null, IMicrophoneService micService = null,
                        ISpeakerService spkService = null, ILedService ledService = null, IBuzzerService buzService = null,
                        string configFile = @"Data/CameraSettings.txt")
        {
            FileManager.CreateSubDirectory("Captures");
            var cameraSettings = SettingsLoader.LoadConfig(configFile);

            if (camService == null)
            {
                CameraService = new CameraService();
            }
            else
            {
                CameraService = camService;
            }

            if (cameraSettings.ContainsKey("UseMic"))
            {
                if (cameraSettings.TryGetValue("UseMic", out string shouldUseMic))
                {
                    if (bool.Parse(shouldUseMic))
                    {
                        //Might this be null coalescable?
                        if (micService == null)
                        {
                            MicrophoneService = new MicrophoneService();
                        }
                        else
                        {
                            MicrophoneService = micService;
                        }
                        useMic = true;
                    }
                    else
                    {
                        //No Mic desired
                    }
                }
            }
            if (cameraSettings.ContainsKey("CameraLight"))
            {
                if (cameraSettings.TryGetValue("CameraLight", out string lightPin))
                {
                    Dictionary<string, int> pins = new Dictionary<string, int>
                    {
                        { "CameraLight", int.Parse(lightPin) }
                    };

                    if (ledService == null)
                    {
                        LedService = new LedService(pins);
                    }
                    else
                    {
                        LedService = ledService;
                        LedService.SetLeds(pins);
                    }

                }
            }
            if (cameraSettings.ContainsKey("UseBuzzer"))
            {
                if (cameraSettings.TryGetValue("UseBuzzer", out string shouldUseBuzzer))
                {
                    if (bool.Parse(shouldUseBuzzer))
                    {
                        //This should be reading from the file!
                        if (buzService == null)
                        {
                            BuzzerService = new BuzzerService(12);
                        }
                        else
                        {
                            BuzzerService = buzService;
                            BuzzerService.SetBuzzer(12);
                        }
                        useBuzzer = true;
                    }
                    else
                    {
                        //No buzzer desired
                    }
                }
            }
            if (cameraSettings.ContainsKey("UseSpeaker"))
            {
                if (cameraSettings.TryGetValue("UseSpeaker", out string shouldUseSpeaker))
                {
                    if (bool.Parse(shouldUseSpeaker))
                    {
                        if (spkService == null)
                        {
                            SpeakerService = new SpeakerService();
                        }
                        else
                        {
                            SpeakerService = spkService;
                        }
                        useSpeaker = true;
                    }
                    else
                    {
                        //No Speaker desired
                    }
                }
            }
        }

        public Camera()
        {
            FileManager.CreateSubDirectory("Captures");
            var cameraSettings = SettingsLoader.LoadConfig(@"Data/CameraSettings.txt");

            CameraService = new CameraService();


            if (cameraSettings.ContainsKey("UseMic"))
            {
                if (cameraSettings.TryGetValue("UseMic", out string shouldUseMic))
                {
                    if (bool.Parse(shouldUseMic))
                    {
                        //Might this be null coalescable?
                        MicrophoneService = new MicrophoneService();
                        useMic = true;
                    }
                    else
                    {
                        //No Mic desired
                    }
                }
            }
            if (cameraSettings.ContainsKey("CameraLight"))
            {
                if (cameraSettings.TryGetValue("CameraLight", out string lightPin))
                {
                    Dictionary<string, int> pins = new Dictionary<string, int>
                    {
                        { "CameraLight", int.Parse(lightPin) }
                    };

                    LedService = new LedService(pins);
                }
            }
            if (cameraSettings.ContainsKey("UseBuzzer"))
            {
                if (cameraSettings.TryGetValue("UseBuzzer", out string shouldUseBuzzer))
                {
                    if (bool.Parse(shouldUseBuzzer))
                    {
                        //This should be reading from the file!
                        BuzzerService = new BuzzerService(13);
                        useBuzzer = true;
                    }
                    else
                    {
                        //No buzzer desired
                    }
                }
            }
            if (cameraSettings.ContainsKey("UseSpeaker"))
            {
                if (cameraSettings.TryGetValue("UseSpeaker", out string shouldUseSpeaker))
                {
                    if (bool.Parse(shouldUseSpeaker))
                    {
                        SpeakerService = new SpeakerService();
                        useSpeaker = true;
                    }
                    else
                    {
                        //No Speaker desired
                    }
                }
            }
        }

        public async Task TakePicture()
        {
            if (!isActive)
            {
                isActive = true;
                if (LedService != null && LedService.GetLeds().ContainsKey("CameraLight"))
                {
                    LedService.BlinkLight("CameraLight");
                }
                if (useBuzzer)
                {
                    BuzzerService.Buzzer.PlayTone(1500, 500);
                }
                if (useSpeaker)
                {
                    _ = SpeakerService.PlayAudio(@"Resources\51360__thecheeseman__camera_snap1.mp3");
                }
                string result = await CameraService.TakePictureAsync($"{FileManager.GetSkullDirectory()}/Captures/");
                LogMessage(result);
                isActive = false;
            }

        }

        public async Task RecordShortVideo()
        {
            if (!isActive)
            {
                isActive = true;
                if (LedService != null && LedService.GetLeds().ContainsKey("CameraLight"))
                {
                    LedService.TurnOn("CameraLight");
                }
                if (useBuzzer)
                {
                    BuzzerService.Buzzer.PlayTone(1500, 500);
                }
                if (useSpeaker)
                {
                    _ = SpeakerService.PlayAudio(@"Resources\195912__acpascal__start-beep.mp3");
                }
                string result = await CameraService.RecordShortVideoAsync($"{FileManager.GetSkullDirectory()}/Captures/", false);
                LogMessage(result);
                if (LedService != null && LedService.GetLeds().ContainsKey("CameraLight"))
                {
                    LedService.TurnOff("CameraLight");
                }
                if (useSpeaker)
                {
                    //Play camera stop sound
                }
                isActive = false;
            }

        }

        public override void OnEnable(string[] args)
        {
            if (Enum.TryParse(args[0], out CameraMode mode))
            {
                CameraMode = mode;
            }
            Console.WriteLine("Camera mode set to " + CameraMode);
        }

        public override string ToString()
        {
            return "Camera";
        }

        public override async void OnAction(object? sender, EventArgs e)
        {
            switch (CameraMode)
            {
                case CameraMode.Image:
                    await TakePicture();
                    break;
                case CameraMode.ShortVideo:
                    await RecordShortVideo();
                    break;
                case CameraMode.ContinuousVideo:
                    break;
                default:
                    break;
            }
        }

        public override void Create()
        {
            throw new NotImplementedException();
        }
    }
}
