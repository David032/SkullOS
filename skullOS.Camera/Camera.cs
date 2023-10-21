using Iot.Device.Media;
using skullOS.Core;

namespace skullOS.Camera
{
    public class Camera : Controller
    {
        VideoConnectionSettings settings = new(busId: 0, captureSize: (720, 720), pixelFormat: VideoPixelFormat.JPEG);

        public Camera()
        {

        }

        public override void Run()
        {
            throw new NotImplementedException();
        }

        public override bool Setup()
        {
            throw new NotImplementedException();
        }

        public override void Stop()
        {
            throw new NotImplementedException();
        }
    }
}
