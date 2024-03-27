using Moq;
using skullOS.HardwareServices.Interfaces;
using skullOS.Modules;
using skullOS.Modules.Exceptions;
using skullOS.Tests;
using System.Device.Gpio;

namespace ModuleTests
{
    public class SupportTest
    {
        Support sut;
        public SupportTest()
        {
            Mock<ISpeakerService> speakerMock = new();
            speakerMock.Setup(speaker => speaker.PlayAudio(It.IsAny<string>())).Returns(Task.CompletedTask);
            Mock<MockableGpioDriver> gpioMock = new();
            var ctrlr = new GpioController(PinNumberingScheme.Logical, gpioMock.Object);

            sut = new Support(ctrlr, speakerMock.Object, 4);
        }

        [Fact]
        public void CanCreateSupportModule()
        {
            Assert.NotNull(sut);
        }
        [Fact]
        public void SupportOnEnableThrowsException()
        {
            Assert.Throws<OnEnableException>(() => sut.OnEnable(It.IsAny<string[]>()));
        }
        [Fact]
        public void SupportOnActionThrowsException()
        {
            Assert.Throws<OnActionException>(() => sut.OnAction(It.IsAny<object>(), It.IsAny<EventArgs>()));
        }
        [Fact]
        public void SupportReturnsCorrectName()
        {
            Assert.Equal("Support", sut.ToString());
        }
    }
}
