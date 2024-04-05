using Iot.Device.Ws28xx;
using Moq;
using skullOS.HardwareServices;
using skullOS.Tests;

namespace HardwareTests
{
    public class ProgrammableLedTest : IHardwareServiceTest
    {
        [Fact(Skip = "SPI issue")]
        public void CanCreate()
        {
            Mock<MockableSpiDevice> spiDevice = new Mock<MockableSpiDevice>();
            Mock<Ws2812b> mockLed = new Mock<Ws2812b>();

            var sut = new ProgrammableLedService(spiDevice.Object, 9, mockLed.Object);

            Assert.NotNull(sut);
        }
    }
}
