using Moq;
using skullOS.Core;
using skullOS.HardwareServices.Interfaces;
using skullOS.Modules;
using skullOS.Modules.Exceptions;

namespace ModuleTests
{
    public class AdventureTest
    {
        Mock<ICameraService> cameraMock;
        Adventure sut;
        public AdventureTest()
        {
            FileManager.CreateSkullDirectory(false, true);

            cameraMock = new();
            cameraMock.Setup(camera => camera.TakePictureAsync(It.IsAny<string>())).Returns(Task.FromResult("Pass"));
            sut = new(cameraMock.Object);
        }

        [Fact]
        public void CanCreateAdventureModule()
        {
            Assert.NotNull(sut);
        }
        [Fact]
        public void OnEnableThrowsException()
        {
            Assert.Throws<OnEnableException>(() => sut.OnEnable(It.IsAny<string[]>()));
        }
        [Fact]
        public void OnActionThrowsException()
        {
            Assert.Throws<OnActionException>(() => sut.OnAction(It.IsAny<object>(), It.IsAny<EventArgs>()));
        }
        [Fact]
        public void NameReturnsCorrect()
        {
            Assert.Equal("Adventure", sut.ToString());
        }

        //Timelapse settings should probably be moved to read from file?
        [Fact]
        public void TimerHasCorrectDuration()
        {
            var timer = sut.GetTimelapseController();
            Assert.Equal(30000, timer.Interval);
        }
    }
}
