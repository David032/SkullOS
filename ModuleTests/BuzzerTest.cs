using Moq;
using skullOS.HardwareServices.Interfaces;
using skullOS.Modules;
using skullOS.Modules.Interfaces;

namespace ModuleTests
{
    public class BuzzerTest
    {
        Buzzer sut;
        Mock<IMelodyPlayer> buzzerPlayer;

        public BuzzerTest()
        {
            Mock<IBuzzerService> buzzerHardware = new();
            buzzerPlayer = new();
            buzzerPlayer.Setup(x => x.Play(It.IsAny<IList<MelodyElement>>(), It.IsAny<int>())).Verifiable();

            sut = new Buzzer(buzzerHardware.Object, 13, buzzerPlayer.Object);
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

            Mock.Verify([buzzerPlayer]);
        }
    }
}
