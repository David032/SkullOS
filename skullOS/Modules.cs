namespace skullOS
{

    public class Module
    {
        private string? _moduleName;
        private bool _enabled;

        public bool ModuleEnabled
        {
            get { return _enabled; }
            set { _enabled = value; }
        }

        public string ModuleName
        {
            get { return _moduleName; }
            set { _moduleName = value; }
        }

        public Module(string name, bool state)
        {
            _moduleName = name;
            _enabled = state;
        }

    }

    public class Modules
    {
        List<Module> _modules;

        public Modules()
        {
            _modules = new List<Module>();
            string[] entries = File.ReadAllLines(@"Data/Modules.txt");
            foreach (string entry in entries)
            {
                string[] split = entry.Split(" = ");
                Module newModule = new(split[0], bool.Parse(split[1]));
                Console.WriteLine("Adding module: " + newModule.ModuleName);
                _modules.Add(newModule);
            }
        }

        public Modules(string pathToConfig)
        {
            _modules = new List<Module>();
            string[] entries = File.ReadAllLines(pathToConfig);
            foreach (string entry in entries)
            {
                string[] split = entry.Split(" = ");
                Module newModule = new(split[0], bool.Parse(split[1]));
                Console.WriteLine("Adding module: " + newModule.ModuleName);
                _modules.Add(newModule);
            }
        }

        public List<Module> Get()
        {
            return _modules;
        }
    }
}
