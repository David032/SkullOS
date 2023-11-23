using Iot.Device.Button;
using skullOS.Core;
using skullOS.Modules;

namespace skullOS
{
    internal class InputManager
    {
        GpioButton ActionButton;
        int actionButtonPin = 23;
        List<GpioButton> Buttons;

        public InputManager()
        {
            ActionButton = new GpioButton(23);
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
                var module = Modules.Select(x => x).Where(x => x.ToString() == inputs.Keys.First()).FirstOrDefault();
                ActionButton.Press += module.OnEnable;
            }
            //If there's one entry in the file, then there's no selection required -> set action button to call that modules OnAction function
            //If there's more than one, parse each one in the following pattern: module(args)=pin
        }
    }
}
