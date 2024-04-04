
namespace skullOS.Modules.Interfaces
{
    public interface IMelodyPlayer
    {
        void Play(IList<MelodyElement> sequence, int tempo);
        void Play(IList<MelodyElement> sequence, int tempo, int tonesToTranspose = 0);
    }
}