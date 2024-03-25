using Moq;
using skullOS.HardwareServices.Interfaces;
using skullOS.Modules;
using System.Device.Gpio;

namespace ModuleTests
{
    public class SupportTest
    {
        [Fact]
        public void CanCreateSupportModule()
        {
            Mock<ISpeakerService> speakerMock = new();
            speakerMock.Setup(speaker => speaker.PlayAudio(It.IsAny<string>())).Returns(Task.CompletedTask);
            Mock<GpioController> gpioMock = new();
            gpioMock.Setup(gpio => gpio.OpenPin(It.IsAny<int>()));
            gpioMock.Setup(gpio => gpio.RegisterCallbackForPinValueChangedEvent(It.IsAny<int>(), PinEventTypes.Rising, It.IsNotNull<PinChangeEventHandler>()));

            var SupportModule = new Support(gpioMock.Object, speakerMock.Object, 4);

            Assert.NotNull(SupportModule);
        }
    }
}
