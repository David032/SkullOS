using Iot.Device.Media;
using System.Device.Gpio;

const int Pin = 25; //What pin is the button plugged in on

using GpioController controller = new();
VideoConnectionSettings settings = new(busId: 0, captureSize: (720, 720), pixelFormat: VideoPixelFormat.JPEG);
using VideoDevice device = VideoDevice.Create(settings);

controller.OpenPin(Pin, PinMode.InputPullUp);

Console.WriteLine("Ready!");

controller.RegisterCallbackForPinValueChangedEvent(
    Pin,
    PinEventTypes.Falling,
    OnPinEvent);

await Task.Delay(Timeout.Infinite);

void OnPinEvent(object sender, PinValueChangedEventArgs args)
{
    Console.WriteLine($"({DateTime.Now}) Picture taken!");
    device.Capture($"{DateTime.Now:yyyyMMddHHmmss}.jpg");
}