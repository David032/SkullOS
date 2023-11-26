using Microsoft.AspNetCore.Mvc;
using skullOS.Modules.Interfaces;

namespace skullOS.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CameraController : ControllerBase
    {
        private readonly ILogger<CameraController> _logger;
        private ICameraModule _module;

        public CameraController(ILogger<CameraController> logger, ICameraModule camera)
        {
            _logger = logger;
            _module = camera;
        }

        [HttpGet]
        public string TakePicture()
        {
            _module.TakePicture();
            return "Picture Taken!";
        }

        [HttpGet]
        public string RecordVideo()
        {
            _module.RecordShortVideo();
            return "Video Recorded!";
        }
    }
}
