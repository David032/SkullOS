using Microsoft.AspNetCore.Mvc;
using skullOS.Modules.Interfaces;
using static skullOS.Modules.BuzzerLibrary;

namespace skullOS.API.Controllers
{
    [Route("Buzzer")]
    [ApiController]
    public class BuzzerController : ControllerBase
    {
        private readonly ILogger<BuzzerController> _logger;
        private IBuzzerModule _module;

        public BuzzerController(ILogger<BuzzerController> logger, IBuzzerModule buzzer)
        {
            _logger = logger;
            _module = buzzer;
        }

        [HttpGet("PlayTune")]
        public string PlayTune(string tune)
        {
            Tunes tuneToPlay = (Tunes)Enum.Parse(typeof(Tunes), tune);
            _module.PlayTune(tuneToPlay);
            return "Playing " + tuneToPlay.ToString();
        }

        [HttpGet("Tunes")]
        public List<string> AvailableTunes()
        {
            List<string> tunes = Enum.GetNames(typeof(Tunes)).ToList();
            return tunes;
        }
    }
}
