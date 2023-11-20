using static skullOS.Controllers.Buzzer.Enums;
using Duration = skullOS.Controllers.Buzzer.Enums.Duration;

namespace skullOS.Controllers.Buzzer
{
    /// <summary>
    /// Note element to define Note and Octave of sound in melody.
    /// </summary>
    internal class NoteElement : MelodyElement
    {
        /// <summary>
        /// Note of sound in melody sequence.
        /// </summary>
        public Note Note { get; set; }

        /// <summary>
        /// Octave of sound in melody sequence.
        /// </summary>
        public Octave Octave { get; set; }

        /// <summary>
        /// Create Note element.
        /// </summary>
        /// <param name="note">Note of sound.</param>
        /// <param name="octave">Octave of sound.</param>
        /// <param name="duration">Duration of sound in melody sequence timeline.</param>
        public NoteElement(Note note, Octave octave, Duration duration)
            : base(duration)
        {
            Note = note;
            Octave = octave;
        }
    }
}
