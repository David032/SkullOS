using Iot.Device.Button;
using skullOS.Core;
using skullOS.Modules;
using System.Text.RegularExpressions;

namespace skullOS
{
    internal class InputManager
    {
        GpioButton ActionButton;
        int actionButtonPin = 25;
        private List<GpioButton>? Buttons;
        private Module? activeModule;

        public InputManager()
        {
            ActionButton = new GpioButton(actionButtonPin);
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
                string[] args = Array.Empty<string>();
                var match = Regex.Match(inputs.Keys.First(), @"[(]\w*[)]");
                if (match.Success)
                {
                    moduleToLoad = inputs.Keys.First().Split('(')[0];
                    args.SetValue(match.Groups[0].Value, 0);
                }
                else
                {
                    moduleToLoad = inputs.Keys.First();
                    args = null;
                }

                var module = Modules.Select(x => x).Where(x => x.ToString() == moduleToLoad).FirstOrDefault();
                SetActiveModule(module, args);
            }
            else
            {
                //If there's more than one, parse each one in the following pattern: module(args)=pin
            }
        }

        public void SetActiveModule(Module moduleToLoad, string[] args = null)
        {
            moduleToLoad.OnEnable(args);
            ActionButton.Press += moduleToLoad.OnAction;
            activeModule = moduleToLoad;
        }
    }
}
