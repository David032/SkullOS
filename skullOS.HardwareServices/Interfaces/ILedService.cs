namespace skullOS.HardwareServices.Interfaces
{
    public interface ILedService
    {
        void BlinkLight(string light);
        void TurnOff(int Pin);
        void TurnOn(int Pin);
        Dictionary<string, int> GetLeds();
    }
}