namespace skullOS.Core.Interfaces
{
    public interface ISubSystem
    {
        bool Setup();

        void Run();

        void Stop();
    }
}
