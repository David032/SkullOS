using Moq;
using skullOS.HardwareServices;
using skullOS.Tests;
using System.Device.Gpio;

namespace HardwareTests
{
    public class LEDTest : IHardwareServiceTest
    {
        [Fact]
        public void CanCreate()
        {
            Mock<MockableGpioDriver> gpioMock = new();
            var ctrlr = new GpioController(PinNumberingScheme.Logical, gpioMock.Object);
            Dictionary<string, int> testPins = new()
            {
                    { "An Led", 6 },
                    {"Another led", 26 }
            };
            LedService sut = new LedService(testPins, ctrlr);

            Assert.NotNull(sut);
        }
    }
}
