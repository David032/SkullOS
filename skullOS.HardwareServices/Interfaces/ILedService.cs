namespace skullOS.HardwareServices.Interfaces
{
    public interface ILedService
    {
        void BlinkLight(string light);
        void TurnOff(string Pin);
        void TurnOn(string Pin);
        Dictionary<string, int> GetLeds();
    }
}