using skullOS.HardwareServices;
using skullOS.HardwareServices.Interfaces;
using skullOS.Modules.Exceptions;
using skullOS.Modules.Interfaces;
using System.Device.Gpio;
using Timer = System.Timers.Timer;


namespace skullOS.Modules
{
    public class Support : Module, ISupport
    {
        GpioController controller;
        ISpeakerService speakerService;
        static Timer? LowBatteryAlert;
        double interval = 60000;
        int pin = 4;

        public Support()
        {
            LowBatteryAlert = new Timer(interval)
            {
                AutoReset = true,
            };
            LowBatteryAlert.Elapsed += LowBatteryAlert_Elapsed;

            speakerService = new SpeakerService();
            controller = new GpioController();
            controller.OpenPin(pin);
            controller.RegisterCallbackForPinValueChangedEvent(pin, PinEventTypes.Rising, OnLowBattery);
        }

        void OnLowBattery(object sender, PinValueChangedEventArgs args)
        {
            //Activate warning led - needs to be very dim to prevent it bleeding out of the casing
            LowBatteryAlert?.Start();
        }

        private void LowBatteryAlert_Elapsed(object? sender, System.Timers.ElapsedEventArgs e)
        {
            speakerService.PlayAudio(@"Resources\710280__dan2008__no-battery.mp3");
        }

        public override void OnAction(object? sender, EventArgs e)
        {
            throw new OnActionException("Support doesn't support OnAction");
        }

        public override void OnEnable(string[] args)
        {
            throw new OnEnableException("Support doesn't support OnEnable");
        }

        public override string ToString()
        {
            return "Support";
        }
    }
}
