﻿using skullOS.HardwareServices;
using skullOS.HardwareServices.Interfaces;
using skullOS.Modules.Exceptions;
using skullOS.Modules.Interfaces;
using System.Device.Gpio;
using Timer = System.Timers.Timer;


namespace skullOS.Modules
{
    public class Support : Module, ISupportModule
    {
        GpioController controller;
        ISpeakerService speakerService;
        static Timer? LowBatteryAlert;
        readonly double interval = 60000;
        int pin; //Defaults to Pimoroni's Lipo shim,

        public Support(GpioController gpioController = null, ISpeakerService speaker = null, int signalPin = 4)
        {
            pin = signalPin;

            speakerService = speaker ?? new SpeakerService();

            controller = gpioController ?? new GpioController();

            Create();
        }

        public Support()
        {
            pin = 4; //This should really be read from a file

            speakerService = new SpeakerService();

            controller = new GpioController();
            Create();
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

        public override void Create()
        {
            LowBatteryAlert = new Timer(interval)
            {
                AutoReset = true,
            };
            LowBatteryAlert.Elapsed += LowBatteryAlert_Elapsed;
            controller.OpenPin(pin);
            controller.RegisterCallbackForPinValueChangedEvent(pin, PinEventTypes.Rising, OnLowBattery);
        }
    }
}
