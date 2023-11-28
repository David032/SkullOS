using skullOS.Core;

namespace skullOS.Modules
{
    public abstract class Module
    {
        protected SkullLogger logger;
        public abstract void OnEnable(string[] args);
        public abstract void OnAction(object? sender, EventArgs e);
        public abstract override string ToString();
        public void AttachLogger(SkullLogger logger)
        {
            this.logger = logger;
        }

        public void LogMessage(string message)
        {
            if (logger != null)
            {
                logger.LogMessage(message);
            }
        }
    }
}
