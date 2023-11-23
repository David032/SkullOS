namespace skullOS.Modules
{
    public abstract class Module
    {
        public abstract void OnEnable(string[] args);
        public abstract void OnAction(object? sender, EventArgs e);

        public abstract override string ToString();
    }
}
