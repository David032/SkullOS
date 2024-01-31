using skullOS.HardwareServices;
using skullOS.HardwareServices.Interfaces;

namespace skullOS.Modules
{
    public class Prop : Module, IPropModule
    {
        public ISpeakerService SpeakerService { get; set; }

        public Prop()
        {
            SpeakerService = new SpeakerService();
            SpeakerService.PlayAudio(@"Resources/computer-startup-music.mp3"); //This one won't await :(
        }

        public override void OnAction(object? sender, EventArgs e)
        {
            throw new NotImplementedException();
        }

        public override void OnEnable(string[] args)
        {
            throw new NotImplementedException();
        }
        public override string ToString()
        {
            return "Prop";
        }
    }
}
