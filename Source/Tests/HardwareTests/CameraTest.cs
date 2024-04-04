using skullOS.HardwareServices;

namespace HardwareTests
{
    public class CameraTest : IHardwareServiceTest
    {
        CameraService sut;

        [Fact]
        public void CanCreate()
        {
            CameraService service = new CameraService();

            Assert.NotNull(service);
        }
    }
}
