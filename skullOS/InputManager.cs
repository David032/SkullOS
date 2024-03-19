using Iot.Device.Button;
using skullOS.Core;
using skullOS.Modules;
using System.Text.RegularExpressions;

namespace skullOS
{
    internal class InputManager
    {
        GpioButton ActionButton;
        GpioButton ToggleButton;
        int actionButtonPin = 24; //25
        int toggleButtonPin = 23; //26
        private Module? activeModule;
        List<(int, Module, string)> inputModules = [];
        int index = 0;
        private SkullLogger? logger;

        public InputManager()
        {
            ActionButton = new GpioButton(actionButtonPin);
            ToggleButton = new GpioButton(toggleButtonPin);
        }

        public void SetupSelector(List<Module> Modules)
        {
            var inputs = SettingsLoader.LoadConfig(@"Data/InputSettings.txt");
            if (inputs.Count == 0)
            {
                //No inputs? So must be remote only
            }
            if (inputs.Count == 1)
            {
                //One input means no selector
                string moduleToLoad;
                string[]? args = new string[1];
                var match = Regex.Match(inputs.Keys.First(), @"[(]\w*[)]");
                if (match.Success)
                {
                    moduleToLoad = inputs.Keys.First().Split('(')[0];
                    char[] charsToRemove = { '(', ')' };
                    args.SetValue(match.Groups[0].Value.Trim(charsToRemove), 0);
                    LogMessage("Set argument to " + match.Groups[0].Value.Trim(charsToRemove));
                }
                else
                {
                    moduleToLoad = inputs.Keys.First();
                    args = null;
                }

                var module = Modules.Select(x => x).Where(x => x.ToString() == moduleToLoad).FirstOrDefault() ?? throw new Exception("Failed to load module for input binding!");
                SetActiveModule(module, args);
            }
            else
            {
                int count = 0;
                foreach (var item in inputs)
                {
                    string moduleToLoad;
                    string[]? args = new string[1];
                    string moduleArguments = string.Empty;

                    var match = Regex.Match(item.Key, @"[(]\w*[)]");
                    if (match.Success)
                    {
                        moduleToLoad = item.Key.Split('(')[0];
                        LogMessage("Identifed " + moduleToLoad + " as an input source");
                        char[] charsToRemove = { '(', ')' };
                        args.SetValue(match.Groups[0].Value.Trim(charsToRemove), 0);
                        moduleArguments = match.Groups[0].Value.Trim(charsToRemove);
                        LogMessage("Set argument to " + moduleArguments);
                    }
                    else
                    {
                        moduleToLoad = item.Key;
                        args = null;
                    }

                    var module = Modules.Select(x => x).Where(x => x.ToString() == moduleToLoad).FirstOrDefault() ?? throw new Exception("Failed to load module for input binding!");
                    inputModules.Add((count, module, moduleArguments));
                    count++;
                }
                Console.WriteLine(count + " options have been registered for input");


                SetActiveModule(inputModules.FirstOrDefault().Item2, [inputModules.FirstOrDefault().Item3]);
                Console.WriteLine("Adding toggle button event");
                ToggleButton.Press += ToggleButton_Press;
                Console.WriteLine("toggle button event added!");
            }
        }

        private void ToggleButton_Press(object? sender, EventArgs e)
        {
            Console.WriteLine("Toggle button pressed!");
            if (index < inputModules.Count)
            {
                index += 1;
            }
            else
            {
                index = 0;
            }

            var moduleToSetActive = inputModules[index];
            Console.WriteLine("Attempting to load " + moduleToSetActive.Item2 + " with the args " + moduleToSetActive.Item3);
            SetActiveModule(moduleToSetActive.Item2, [moduleToSetActive.Item3]);
        }

        public void SetActiveModule(Module moduleToLoad, string[]? args = null)
        {
            if (args == null)
            {
                LogMessage("No modules provided, so nothing to set as active");
                return;
            }
            LogMessage("Setting " + moduleToLoad.ToString() + " as the active module");
            moduleToLoad.OnEnable(args);
            ActionButton.Press += moduleToLoad.OnAction;
            activeModule = moduleToLoad;
        }

        public void attachLogger(SkullLogger skullLogger)
        {
            logger = skullLogger;
        }

        public void LogMessage(string message)
        {
            if (logger == null)
            {
                throw new Exception("Logger not attached to input manager!");
            }
            logger.LogMessage(message);
        }
    }
}
