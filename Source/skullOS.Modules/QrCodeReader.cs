using skullOS.Modules.Exceptions;

namespace skullOS.Modules
{
    public class QrCodeReader : Module, IQrCodeReaderModule
    {
        public override void Create()
        {
            throw new NotImplementedException();
        }

        public override void OnAction(object? sender, EventArgs e)
        {
            throw new OnActionException("Not yet implemented");
        }

        public override void OnEnable(string[] args)
        {
            throw new OnEnableException("Not yet implemented");
        }
        public override string ToString()
        {
            throw new NotImplementedException();
        }
    }
}
