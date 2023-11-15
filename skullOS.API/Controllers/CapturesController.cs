using Microsoft.AspNetCore.Mvc;
using skullOS.API.Data_Objects;
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
            FileManager.CreateSkullDirectory();
            capturesDirectory = @FileManager.GetSkullDirectory() + @"/Captures";
            capturesDirectoryInfo = new DirectoryInfo(capturesDirectory);
        }

        [HttpGet("Directory")]
        public string GetDirectory()
        {
            return capturesDirectoryInfo.FullName;
        }

        [HttpGet("MostRecent")]
        public IActionResult GetNewest()
        {
            var mostRecentImage = capturesDirectoryInfo.GetFiles().Where(f => f.Extension == ".jpg").OrderByDescending(f => f.LastAccessTime).FirstOrDefault();
            var image = System.IO.File.OpenRead(mostRecentImage.FullName);
            return File(image, "image/jpeg");
        }

        [HttpGet("All")]
        public List<Capture> GetAll()
        {
            List<Capture> Files = new();
            foreach (var item in capturesDirectoryInfo.GetFiles())
            {
                Capture file = new(item.FullName, item.CreationTime.ToShortTimeString());
                Files.Add(file);
            }
            return Files;
        }

        [HttpGet("AllPictures")]
        public List<Capture> GetAllPictures()
        {
            List<Capture> Files = new();
            foreach (var item in capturesDirectoryInfo.GetFiles().Where(f => f.Extension == ".jpg"))
            {
                Capture file = new(item.FullName, item.CreationTime.ToShortTimeString());
                Files.Add(file);
            }
            return Files;
        }

        [HttpGet("AllVideos")]
        public List<Capture> GetAllVideos()
        {
            List<Capture> Files = new();
            foreach (var item in capturesDirectoryInfo.GetFiles().Where(f => f.Extension == ".mp4"))
            {
                Capture file = new(item.FullName, item.CreationTime.ToShortTimeString());
                Files.Add(file);
            }
            return Files;
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
