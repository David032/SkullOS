using Moq;
using skullOS.HardwareServices.Interfaces;
using skullOS.Modules;

namespace ModuleTests
{
    public class BuzzerTest
    {
        Buzzer sut;
        Mock<MelodyPlayer> melodyPlayer;

        public BuzzerTest()
        {
            Mock<IBuzzerService> buzzerHardware = new();
            melodyPlayer = new();
            melodyPlayer.Setup(player => player.Play(It.IsAny<IList<MelodyElement>>(), It.IsAny<int>())).Verifiable();

            sut = new Buzzer(buzzerHardware.Object, 13, melodyPlayer.Object);
        }

        [Fact]
        public void CanCreateBuzzer()
        {
            Assert.NotNull(sut);
        }

        [Fact]
        public void CanPlayTune()
        {
            sut.PlayTune(BuzzerLibrary.Tunes.AlphabetSong);

            Mock.Verify([melodyPlayer]);
        }
    }
}
