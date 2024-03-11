
namespace skullOS.HardwareServices.Interfaces
{
    public interface ICameraService
    {
        Task<string> TakePictureAsync(string fileLocation, int x = 2592, int y = 1944);
    }
}