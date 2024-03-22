namespace skullOS.Modules.Interfaces
{
    public interface ICameraModule
    {
        Task RecordShortVideo();
        Task TakePicture();
    }
}