using skullOS.Core.Interfaces;

namespace skullOS.Core
{
    public abstract class Controller : ISubSystem
    {
        public abstract void Run();

        public abstract bool Setup();

        public abstract void Stop();
    }
}
