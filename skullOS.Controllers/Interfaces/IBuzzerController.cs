using static skullOS.Controllers.Buzzer.BuzzerController;

namespace skullOS.Controllers.Interfaces
{
    internal interface IBuzzerController
    {
        public void PlayTune(Tunes tune);
    }
}
