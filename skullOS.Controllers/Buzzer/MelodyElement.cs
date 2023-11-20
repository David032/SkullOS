using static skullOS.Controllers.Buzzer.Enums;

namespace skullOS.Controllers.Buzzer
{
    /// <summary>
    /// A base class for melody sequence elements.
    /// </summary>
    internal abstract class MelodyElement
    {
        /// <summary>
        /// Duration which defines how long should element take on melody sequence timeline.
        /// </summary>
        public Duration Duration { get; set; }

        public MelodyElement(Duration duration) => Duration = duration;
    }
}
