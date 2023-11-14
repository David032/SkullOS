using Microsoft.AspNetCore.Mvc;
using skullOS.Core;

namespace skullOS.API.Controllers
{
    [Route("Captures")]
    [ApiController]
    public class CapturesController : ControllerBase
    {
        private readonly ILogger<CapturesController> _logger;
        string capturesDirectory;
        DirectoryInfo capturesDirectoryInfo;

        public CapturesController(ILogger<CapturesController> logger)
        {
            _logger = logger;
            capturesDirectory = @FileManager.GetSkullDirectory() + @"/Captures";
            capturesDirectoryInfo = new DirectoryInfo(capturesDirectory);
        }

        [HttpGet("MostRecent")]
        public IActionResult GetNewest()
        {
            var mostRecentImage = capturesDirectoryInfo.GetFiles().OrderByDescending(f => f.LastAccessTime).FirstOrDefault();
            var image = System.IO.File.OpenRead(mostRecentImage.FullName);
            return File(image, "image/jpeg");
        }

        [HttpGet("All")]
        public List<FileInfo> GetAll()
        {
            return capturesDirectoryInfo.GetFiles().ToList();
        }

        [HttpGet("AllPictures")]
        public List<FileInfo> GetAllPictures()
        {
            return capturesDirectoryInfo.GetFiles().Where(f => f.Extension == ".jpeg").ToList();
        }

        [HttpGet("AllVideos")]
        public List<FileInfo> GetAllVideos()
        {
            return capturesDirectoryInfo.GetFiles().Where(f => f.Extension == ".mp4").ToList();
        }

        [HttpGet("Image")]
        public IActionResult GetImage(string fileId)
        {
            var file = capturesDirectoryInfo.GetFiles().Where(f => f.Name == fileId).FirstOrDefault();
            var image = System.IO.File.OpenRead(file.FullName);
            return File(image, "image/jpeg");
        }

        [HttpGet("Video")]
        public IActionResult GetVideo(string fileId)
        {
            var file = capturesDirectoryInfo.GetFiles().Where(f => f.Name == fileId).FirstOrDefault();
            var image = System.IO.File.OpenRead(file.FullName);
            return File(image, "video/mp4");
        }
    }
}
