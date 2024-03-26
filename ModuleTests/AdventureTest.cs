using Moq;
using skullOS.Core;
using skullOS.HardwareServices.Interfaces;
using skullOS.Modules;

namespace ModuleTests
{
    public class AdventureTest
    {
        public AdventureTest()
        {
            FileManager.CreateSkullDirectory(false, true);
        }

        [Fact]
        public void CanCreateAdventureModule()
        {
            //string now = DateTime.Now.ToString("M");
            //var directory = FileManager.GetSkullDirectory() + @"/Timelapse - " + now + @"/";
            Mock<ICameraService> cameraMock = new();
            cameraMock.Setup(camera => camera.TakePictureAsync(It.IsAny<string>())).Returns(Task.FromResult("Pass"));

            Adventure sut = new(cameraMock.Object);

            Assert.NotNull(sut);
        }
    }
}
