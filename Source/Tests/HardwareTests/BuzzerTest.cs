using Iot.Device.Buzzer;
using Moq;
using skullOS.HardwareServices;

namespace HardwareTests
{
    public class BuzzerTest : IHardwareServiceTest
    {
        BuzzerService sut;

        [Fact(Skip = "Buzzer hard creates gpio driver")]
        public void CanCreate()
        {
            Mock<Buzzer> mockedBuzzer = new Mock<Buzzer>([0]);

            sut = new BuzzerService(0, mockedBuzzer.Object);

            Assert.NotNull(sut);
        }
    }
}
