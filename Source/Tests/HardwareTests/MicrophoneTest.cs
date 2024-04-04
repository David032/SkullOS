using Iot.Device.Media;
using Moq;
using skullOS.HardwareServices;

namespace HardwareTests
{
    public class MicrophoneTest : IHardwareServiceTest
    {
        [Fact]
        public void CanCreate()
        {
            Mock<SoundDevice> SoundDevice = new Mock<SoundDevice>();
            Mock<SoundConnectionSettings> SoundConnectionSettings = new Mock<SoundConnectionSettings>();

            var sut = new MicrophoneService(mic: SoundDevice.Object);

            Assert.NotNull(sut);
        }
    }
}
