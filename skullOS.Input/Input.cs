using skullOS.Core;

namespace skullOS.Input
{
    internal class Input : Controller
    {
        private static readonly Lazy<Input> lazy = new(() => new Input());

        public static Input Instance { get { return lazy.Value; } }

        private List<InputDevice> inputDevices = new();

        public List<InputDevice> InputDevices
        {
            get { return inputDevices; }
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
