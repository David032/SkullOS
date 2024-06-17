using skullOS.HardwareServices;
using skullOS.HardwareServices.Interfaces;
using skullOS.Modules.Exceptions;
using skullOS.Modules.Interfaces;
using static skullOS.Modules.BuzzerLibrary;
using static skullOS.Modules.BuzzerStructures;
using DeviceBuzzer = Iot.Device.Buzzer.Buzzer;
using Duration = skullOS.Modules.BuzzerStructures.Duration;

namespace skullOS.Modules
{
    public class Buzzer : Module, IBuzzerModule
    {
        IBuzzerService PwmBuzzer;
        IMelodyPlayer Player;
        public Buzzer(IBuzzerService buzzerService = null, int pwmPin = 13, IMelodyPlayer testPlayer = null)
        {
            if (buzzerService == null)
            {
                PwmBuzzer = new BuzzerService(pwmPin);
            }
            else
            {
                PwmBuzzer = buzzerService;
            }

            if (testPlayer == null)
            {
                Player = new MelodyPlayer(PwmBuzzer.Buzzer);
            }
            else
            {
                Player = testPlayer;
            }
        }

        public Buzzer()
        {
            PwmBuzzer = new BuzzerService(13); //This should really be read from a settings file
            Player = new MelodyPlayer(PwmBuzzer.Buzzer);
        }

        public override void Create()
        {
            throw new NotImplementedException();
        }

        public void PlayTune(Tunes tuneToPlay)
        {
            switch (tuneToPlay)
            {
                case Tunes.HappyBirthday:
                    LogMessage("Playing Happy Birthday on the buzzer");
                    Player.Play(happyBirthday, 100);
                    break;
                case Tunes.AlphabetSong:
                    LogMessage("Playing the alphabet song on the buzzer");
                    Player.Play(alphabetSong, 100);
                    break;
            }
        }

        public override void OnAction(object? sender, EventArgs e)
        {
            throw new OnActionException("Buzzer does not support OnAction");
        }
        public override void OnEnable(string[] args)
        {
            throw new OnEnableException("Buzzer does not support OnEnable");
        }
        public override string ToString()
        {
            return "Buzzer";
        }
    }

    #region Buzzer Media Classes
    /// <summary>
    /// 3rd party class for playing media via a buzzer
    /// Sourced from teh iot samples
    /// </summary>
    public class MelodyPlayer : IMelodyPlayer
    {
        private readonly DeviceBuzzer _buzzer;
        private int _wholeNoteDurationInMilliseconds;

        /// <summary>
        /// Create MelodyPlayer.
        /// </summary>
        /// <param name="buzzer">Buzzer instance to be played on.</param>
        public MelodyPlayer(DeviceBuzzer buzzer)
        {
            _buzzer = buzzer;
        }

        /// <summary>
        /// Play melody elements sequence.
        /// </summary>
        /// <param name="sequence">Sequence of pauses and notes elements to be played.</param>
        /// <param name="tempo">Tempo of melody playing.</param>
        /// <param name="tonesToTranspose">Tones to transpose</param>
        public void Play(IList<MelodyElement> sequence, int tempo, int tonesToTranspose = 0)
        {
            _wholeNoteDurationInMilliseconds = GetWholeNoteDurationInMilliseconds(tempo);
            sequence = TransposeSequence(sequence, tonesToTranspose);
            foreach (var element in sequence)
            {
                PlayElement(element);
            }
        }

        /// <summary>
        /// Play melody elements sequence. 0 set transposing for testing
        /// </summary>
        /// <param name="sequence">Sequence of pauses and notes elements to be played.</param>
        /// <param name="tempo">Tempo of melody playing.</param>
        public void Play(IList<MelodyElement> sequence, int tempo)
        {
            _wholeNoteDurationInMilliseconds = GetWholeNoteDurationInMilliseconds(tempo);
            sequence = TransposeSequence(sequence, 0);
            foreach (var element in sequence)
            {
                PlayElement(element);
            }
        }

        private static IList<MelodyElement> TransposeSequence(IList<MelodyElement> sequence, int tonesToTranspose)
        {
            if (tonesToTranspose == 0)
            {
                return sequence;
            }

            return sequence
                .Select(element => TransposeElement(element, tonesToTranspose))
                .ToList();
        }

        private static MelodyElement TransposeElement(MelodyElement element, int tonesToTransponse)
        {
            if (element is not NoteElement noteElement)
            {
                return element;
            }

            // Every octave consists of 12 notes numberd from 1 to 12. Each octave numbered from 1 to 8.
            // To transpose over octaves we will get absolute index of note like (octave index - 1) * 12 + (note index - 1).
            // As far as octave and note numbered starting from 1 we decrease it's values by 1 for farher calculation.
            var absoluteNoteNumber = ((int)noteElement.Octave - 1) * 12 + ((int)noteElement.Note - 1);

            // Then we transpose absolute number. In case gotten value exceeds maximum value of 96 (12 notes * 8 octave)
            // we calculate remainder of dividing by 96. In case gotten value is below 0 we add 96.
            absoluteNoteNumber = (absoluteNoteNumber + tonesToTransponse) % 96;
            absoluteNoteNumber = absoluteNoteNumber >= 0 ? absoluteNoteNumber : absoluteNoteNumber + 96;

            // Then to get transposed octave index we divide transposed absolute index
            // value by 12 and increase it by 1 due to we decreased it by 1 before.
            // To get transposed note index we will get remainder of dividing by 12
            // and increase it by 1 due to we decreased it by 1 before.
            noteElement.Octave = (Octave)(absoluteNoteNumber / 12 + 1);
            noteElement.Note = (Note)(absoluteNoteNumber % 12 + 1);
            return noteElement;
        }

        private void PlayElement(MelodyElement element)
        {
            int durationInMilliseconds = _wholeNoteDurationInMilliseconds / (int)element.Duration;

            if (element is not NoteElement noteElement)
            {
                // In case it's a pause element we have only just wait desired time.
                Thread.Sleep(durationInMilliseconds);
            }
            else
            {
                // In case it's a note element we play it.
                var frequency = GetFrequency(noteElement.Note, noteElement.Octave);
                _buzzer.PlayTone(frequency, (int)(durationInMilliseconds * 0.7));
                Thread.Sleep((int)(durationInMilliseconds * 0.3));
            }
        }

        private static readonly Dictionary<Note, double> notesOfEightOctaveToFrequenciesMap
            = new Dictionary<Note, double>
                {
                    { Note.C,  4186.01 },
                    { Note.Db, 4434.92 },
                    { Note.D,  4698.63 },
                    { Note.Eb, 4978.03 },
                    { Note.E,  5274.04 },
                    { Note.F,  5587.65 },
                    { Note.Gb, 5919.91 },
                    { Note.G,  6271.93 },
                    { Note.Ab, 6644.88 },
                    { Note.A,  7040.00 },
                    { Note.Bb, 7458.62 },
                    { Note.B,  7902.13 }
                };

        // In music tempo defines amount of quarter notes per minute.
        // Dividing minute (60 * 1000) by tempo we get duration of quarter note.
        // Whole note duration equals to four quarters.
        private static int GetWholeNoteDurationInMilliseconds(int tempo) => 4 * 60 * 1000 / tempo;

        private static double GetFrequency(Note note, Octave octave)
        {
            // We could decrease octave of every note by 1 by dividing it's frequency by 2.
            // We have predefined frequency of every note of eighth octave rounded to 2 decimals.
            // To get note frequency of any other octave we have to divide note frequency of eighth octave by 2 n times,
            // where n is a difference between eight octave and desired octave.
            var eightOctaveNoteFrequency = GetNoteFrequencyOfEightOctave(note);
            var frequencyDivider = Math.Pow(2, 8 - (int)octave);
            return Math.Round(eightOctaveNoteFrequency / frequencyDivider, 2);
        }

        private static double GetNoteFrequencyOfEightOctave(Note note)
        {
            if (notesOfEightOctaveToFrequenciesMap.TryGetValue(note, out double result))
            {
                return result;
            }

            return 0;
        }

        public void Dispose() => _buzzer?.Dispose();
    }

    /// <summary>
    /// Musical data for the buzzer media player
    /// </summary>
    public static class BuzzerLibrary
    {
        public enum Tunes
        {
            HappyBirthday,
            AlphabetSong,
        }

        public static IList<MelodyElement> alphabetSong = new List<MelodyElement>()
        {
            new NoteElement(Note.C, Octave.Fourth, Duration.Quarter),   // A
            new NoteElement(Note.C, Octave.Fourth, Duration.Quarter),   // B
            new NoteElement(Note.G, Octave.Fourth, Duration.Quarter),   // C
            new NoteElement(Note.G, Octave.Fourth, Duration.Quarter),   // D
            new NoteElement(Note.A, Octave.Fourth, Duration.Quarter),   // E
            new NoteElement(Note.A, Octave.Fourth, Duration.Quarter),   // F
            new NoteElement(Note.G, Octave.Fourth, Duration.Half),      // G
            new NoteElement(Note.F, Octave.Fourth, Duration.Quarter),   // H
            new NoteElement(Note.F, Octave.Fourth, Duration.Quarter),   // I
            new NoteElement(Note.E, Octave.Fourth, Duration.Quarter),   // J
            new NoteElement(Note.E, Octave.Fourth, Duration.Quarter),   // K
            new NoteElement(Note.D, Octave.Fourth, Duration.Eighth),    // L
            new NoteElement(Note.D, Octave.Fourth, Duration.Eighth),    // M
            new NoteElement(Note.D, Octave.Fourth, Duration.Eighth),    // N
            new NoteElement(Note.D, Octave.Fourth, Duration.Eighth),    // O
            new NoteElement(Note.C, Octave.Fourth, Duration.Half),      // P
            new NoteElement(Note.G, Octave.Fourth, Duration.Quarter),   // Q
            new NoteElement(Note.G, Octave.Fourth, Duration.Quarter),   // R
            new NoteElement(Note.F, Octave.Fourth, Duration.Half),      // S
            new NoteElement(Note.E, Octave.Fourth, Duration.Quarter),   // T
            new NoteElement(Note.E, Octave.Fourth, Duration.Quarter),   // U
            new NoteElement(Note.D, Octave.Fourth, Duration.Half),      // V
            new NoteElement(Note.G, Octave.Fourth, Duration.Eighth),    // Dou-
            new NoteElement(Note.G, Octave.Fourth, Duration.Eighth),    // ble
            new NoteElement(Note.G, Octave.Fourth, Duration.Quarter),   // U
            new NoteElement(Note.F, Octave.Fourth, Duration.Half),      // X
            new NoteElement(Note.E, Octave.Fourth, Duration.Quarter),   // Y
            new NoteElement(Note.E, Octave.Fourth, Duration.Quarter),   // and
            new NoteElement(Note.D, Octave.Fourth, Duration.Half),      // Z
        };
        public static IList<MelodyElement> happyBirthday = new List<MelodyElement>()
        {
            new NoteElement(Note.C, Octave.Fourth, Duration.Quarter), //Hap
            new NoteElement(Note.C, Octave.Fourth, Duration.Quarter), //-py
            new NoteElement(Note.D, Octave.Fourth, Duration.Quarter), //birth
            new NoteElement(Note.C, Octave.Fourth, Duration.Quarter), //day
            new NoteElement(Note.F, Octave.Fourth, Duration.Quarter), //to
            new NoteElement(Note.E, Octave.Fourth, Duration.Half),    //you
            new NoteElement(Note.C, Octave.Fourth, Duration.Quarter), //Hap
            new NoteElement(Note.C, Octave.Fourth, Duration.Quarter), //-py
            new NoteElement(Note.D, Octave.Fourth, Duration.Quarter), //birth
            new NoteElement(Note.C, Octave.Fourth, Duration.Quarter), //day
            new NoteElement(Note.G, Octave.Fourth, Duration.Quarter), //to
            new NoteElement(Note.F, Octave.Fourth, Duration.Half), //you
            new NoteElement(Note.C, Octave.Fourth, Duration.Quarter), //Hap
            new NoteElement(Note.C, Octave.Fourth, Duration.Quarter), //-py
            new NoteElement(Note.C, Octave.Fourth, Duration.Quarter), //birth
            new NoteElement(Note.A, Octave.Fourth, Duration.Quarter), //day
            new NoteElement(Note.F, Octave.Fourth, Duration.Quarter), //dear
            new NoteElement(Note.E, Octave.Fourth, Duration.Quarter), //mak
            new NoteElement(Note.D, Octave.Fourth, Duration.Quarter), //-er
            new NoteElement(Note.B, Octave.Fourth, Duration.Quarter), //Hap
            new NoteElement(Note.B, Octave.Fourth, Duration.Quarter), //-py
            new NoteElement(Note.A, Octave.Fourth, Duration.Quarter), //birth
            new NoteElement(Note.F, Octave.Fourth, Duration.Quarter), //-day
            new NoteElement(Note.G, Octave.Fourth, Duration.Quarter), //to
            new NoteElement(Note.F, Octave.Fourth, Duration.Half), //you
        };
    }

    /// <summary>
    /// Musical structures used by the buzzer media player
    /// </summary>
    public static class BuzzerStructures
    {
        /// <summary>
        /// Represents all twelve notes.
        /// </summary>
        public enum Note
        {
            C = 1,
            Db = 2,
            D = 3,
            Eb = 4,
            E = 5,
            F = 6,
            Gb = 7,
            G = 8,
            Ab = 9,
            A = 10,
            Bb = 11,
            B = 12,
        }

        /// <summary>
        /// Represents music octave.
        /// </summary>
        public enum Octave
        {
            First = 1,
            Second = 2,
            Third = 3,
            Fourth = 4,
            Fifth = 5,
            Sixth = 6,
            Seventh = 7,
            Eighth = 8
        }

        /// <summary>
        /// Represents music note duration.
        /// </summary>
        public enum Duration
        {
            Whole = 1,
            Half = 2,
            Quarter = 4,
            Eighth = 8,
            Sixteenth = 16,
        }
    }

    /// <summary>
    /// A base class for melody sequence elements.
    /// </summary>
    public abstract class MelodyElement
    {
        /// <summary>
        /// Duration which defines how long should element take on melody sequence timeline.
        /// </summary>
        public Duration Duration { get; set; }

        public MelodyElement(Duration duration) => Duration = duration;
    }

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

    /// <summary>
    /// A pause
    /// </summary>
    internal class PauseElement : MelodyElement
    {
        public PauseElement(Duration duration) : base(duration)
        {

        }
    }
    #endregion
}
