﻿using skullOS.HardwareServices.Interfaces;
using System.Device.Gpio;

namespace skullOS.HardwareServices
{
    public class LedService : ILedService
    {
        public Dictionary<string, int> LEDs { get; private set; }
        GpioController controller;

        public LedService(Dictionary<string, int> leds, GpioController controller = null)
        {
            LEDs = leds;
            if (controller == null)
            {
                this.controller = new GpioController();
            }
            else
            {
                this.controller = controller;
            }
        }

        public async void BlinkLight(string light)
        {
            TurnOn(light);
            await Task.Delay(750);
            TurnOff(light);
        }

        public void TurnOn(string light)
        {
            int pin = LEDs[light];
            if (!controller.IsPinOpen(pin))
            {
                controller.OpenPin(pin, PinMode.Output);
            }
            controller.Write(pin, PinValue.High);
        }

        public void TurnOff(string light)
        {
            int pin = LEDs[light];
            if (!controller.IsPinOpen(pin))
            {
                controller.OpenPin(pin, PinMode.Output);
            }
            controller.Write(pin, PinValue.Low);
        }

        public Dictionary<string, int> GetLeds()
        {
            return LEDs;
        }

        public void SetLeds(Dictionary<string, int> ledsToControl)
        {
            LEDs = ledsToControl;
        }
    }
}
