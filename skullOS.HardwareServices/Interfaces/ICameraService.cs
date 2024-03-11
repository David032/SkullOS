
namespace skullOS.HardwareServices.Interfaces
{
    public interface ICameraService
    {
        Task<string> TakePictureAsync(string fileLocation);
        Task<string> RecordShortVideoAsync(string fileLocation, bool useMic);
    }
}