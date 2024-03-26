using Moq;
using skullOS.HardwareServices.Interfaces;
using skullOS.Modules;
using skullOS.Tests;
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
            Mock<MockableGpioDriver> gpioMock = new();
            //gpioMock.Setup(gpio => gpio.OpenPinEx(It.IsAny<int>()));
            var ctrlr = new GpioController(PinNumberingScheme.Logical, gpioMock.Object);

            var SupportModule = new Support(ctrlr, speakerMock.Object, 4);

            Assert.NotNull(SupportModule);
        }
    }
}
