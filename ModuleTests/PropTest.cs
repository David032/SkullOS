using Moq;
using skullOS.HardwareServices.Interfaces;
using skullOS.Modules;
using skullOS.Modules.Exceptions;

namespace ModuleTests
{
    public class PropTest
    {
        Prop sut;
        string pathToTestData = @"TestData/PropSettings.txt";

        public PropTest()
        {
            Mock<ISpeakerService> speakerMock = new();
            speakerMock.Setup(speaker => speaker.PlayAudio(It.IsAny<string>())).Returns(Task.CompletedTask);
            Mock<ILedService> ledMock = new();

            sut = new Prop(speakerMock.Object, ledMock.Object, pathToTestData);
        }

        [Fact]
        public void CanCreateProp()
        {
            Assert.NotNull(sut);
        }

        [Fact]
        public void NameReturnsCorrect()
        {
            Assert.Equal("Prop", sut.ToString());
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
    }
}
