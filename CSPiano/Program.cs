using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSPiano
{
    enum Accidental
    {
        Flat,
        Sharp,
        Natural,
        DoubleFlat,
        DoubleSharp,
    }

    enum Note
    {
        Large,
        Long,
        Breve,
        Semibreve,
        Minum,
        Crotchet,
        Quaver,
        Semiquaver,
        Demisemiquaver,
        Hemidemisemiquaver,
        Semihemidemisemiquaver,
        Demisemihemidemisemiquaver,
    }

    enum Pitch
    {
        C0 = 37,// 16,
        D0 = 37,// 18,
        E0 = 37,// 21,
        F0 = 37,// 22,
        G0 = 37,// 25,
        A0 = 37,// 28,
        B0 = 37,// 31,

        C1 = 37,// 33,
        D1 = 37,
        E1 = 41,
        F1 = 44,
        G1 = 49,
        A1 = 55,
        B1 = 62,

        C2 = 65,
        D2 = 73,
        E2 = 82,
        F2 = 87,
        G2 = 98,
        A2 = 110,
        B2 = 123,

        C3 = 131,
        D3 = 147,
        E3 = 165,
        F3 = 175,
        G3 = 196,
        A3 = 220,
        B3 = 247,

        C4 = 262,
        D4 = 294,
        E4 = 330,
        F4 = 349,
        G4 = 392,
        A4 = 440,
        B4 = 494,

        C5 = 523,
        D5 = 587,
        E5 = 659,
        F5 = 698,
        G5 = 784,
        A5 = 880,
        B5 = 988,

        C6 = 1047,
        D6 = 1175,
        E6 = 1319,
        F6 = 1397,
        G6 = 1568,
        A6 = 1760,
        B6 = 1975,

        C7 = 2093,
        D7 = 2349,
        E7 = 2637,
        F7 = 2794,
        G7 = 3136,
        A7 = 3520,
        B7 = 3951,

        C8 = 4186,
        D8 = 4699,
        E8 = 5274,
        F8 = 5588,
        G8 = 6272,
        A8 = 7040,
        B8 = 7902,
    }

    class NoteInfo
    {
        Note note;
        List<Pitch> pitches = new List<Pitch>();
        List<Accidental> accidentals = new List<Accidental>();
        int dotCount;

        List<int> frequencies = new List<int>();
        int duration;

        public NoteInfo(int tempo, string line)
        {
            // pitch1-accidental1, pitch2-accidental2, ptich3-accidental3, ..., pitchX-accidentalX | noteType | dotCount |
            string[] parts = line.Split('|');

            string[] pitchAccidentals = parts[0].Split(',');
            foreach (string pitchAccidental in pitchAccidentals)
            {
                string[] split = pitchAccidental.Split('-');
                string pitch = split[0];
                string accidental = split[1];

                Pitch p;
                if (!Enum.TryParse(pitch, out p))
                    p = Pitch.A0;
                this.pitches.Add(p);

                switch (accidental)
                {
                    case "b":
                        this.accidentals.Add(Accidental.Flat);
                    break;
                    case "s":
                        this.accidentals.Add(Accidental.Sharp);
                    break;
                    case "ss":
                        this.accidentals.Add(Accidental.DoubleSharp);
                        break;
                    case "bb":
                        this.accidentals.Add(Accidental.DoubleFlat);
                        break;
                    case "n":
                    default:
                        this.accidentals.Add(Accidental.Natural);
                    break;
                }
            }

            string note = parts[1];
            Note n;
            if (!Enum.TryParse(note, out n))
                n = Note.Crotchet;
            this.note = n;

            string dotCount = parts[2];
            int dc;
            if (!int.TryParse(dotCount, out dc))
                dc = 0;
            this.dotCount = dc;

            int crotchetDuration = 60000 / tempo;
            switch (this.note)
            {
                case Note.Large:
                    this.duration = crotchetDuration * 32;
                    break;
                case Note.Long:
                    this.duration = crotchetDuration * 16;
                    break;
                case Note.Breve:
                    this.duration = crotchetDuration * 8;
                    break;
                case Note.Semibreve:
                    this.duration = crotchetDuration * 4;
                    break;
                case Note.Minum:
                    this.duration = crotchetDuration * 2;
                    break;
                case Note.Crotchet:
                    this.duration = crotchetDuration;
                    break;
                case Note.Quaver:
                    this.duration = crotchetDuration / 2;
                    break;
                case Note.Semiquaver:
                    this.duration = crotchetDuration / 4;
                    break;
                case Note.Demisemiquaver:
                    this.duration = crotchetDuration / 8;
                    break;
                case Note.Hemidemisemiquaver:
                    this.duration = crotchetDuration / 16;
                    break;
                case Note.Semihemidemisemiquaver:
                    this.duration = crotchetDuration / 32;
                    break;
                case Note.Demisemihemidemisemiquaver:
                    this.duration = crotchetDuration / 64;
                    break;
                default:
                    this.duration = crotchetDuration;
                    break;
            }

            foreach (Pitch pitch in pitches)
            {
                frequencies.Add((int)pitch);
            }
        }

        public override string ToString()
        {
            StringBuilder pitchesSB = new StringBuilder();
            for (int i = 0; i < pitches.Count; i++)
            {
                pitchesSB.Append(pitches[i]);
                pitchesSB.Append("-");
                pitchesSB.Append(accidentals[i]);
                if (i != pitches.Count - 1)
                    pitchesSB.Append(":");
            }

            return $"{pitchesSB.ToString()} {note}";
        }

        public void Play()
        {
            // TODO: this needs to be at the same time
            int finalFreq = 0;
            foreach (int freq in frequencies)
            {
                finalFreq += freq;
            }
            finalFreq /= frequencies.Count;
            Console.WriteLine("Playing: " + this.ToString());
            Console.Beep(finalFreq, duration);
        }
    }

    class Program
    {
        static int tempo = 120;
        static string fileName = "TurkishMarch.txt";

        static List<NoteInfo> LoadFile(string fileName)
        {
            string[] lines = File.ReadAllLines(fileName);
            List<NoteInfo> notes = new List<NoteInfo>();
            foreach (string line in lines)
            {
                if (line == "")
                    continue;
                notes.Add(new NoteInfo(tempo, line));
            }
            return notes;
        }

        static void Play(List<NoteInfo> notes)
        {
            foreach (NoteInfo note in notes)
            {
                note.Play();
            }
        }

        static void Main(string[] args)
        {
            List<NoteInfo> notes;
            if (args.Length > 1)
                notes = LoadFile(args[0]);
            else
                notes = LoadFile(fileName);
            Play(notes);
        }
    }
}
