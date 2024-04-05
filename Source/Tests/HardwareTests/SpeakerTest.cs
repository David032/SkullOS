using skullOS.HardwareServices;

namespace HardwareTests
{
    public class SpeakerTest : IHardwareServiceTest
    {
        [Fact]
        public void CanCreate()
        {
            var sut = new SpeakerService();

            Assert.NotNull(sut);
        }
    }
}
