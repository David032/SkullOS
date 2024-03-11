namespace skullOS.HardwareServices.Exceptions
{
    public enum CameraMode
    {
        Picture,
        ShortVideo,
        LongVideo
    }

    public class CameraErrorException(string message, CameraMode modeWhenFailed) : Exception(message)
    {
        public CameraMode Mode;
    }
}
