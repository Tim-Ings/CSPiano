using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSPiano
{
    class Program
    {
        static int tempo = 120;
        static int noteDuration = 60000 / tempo;
        static List<int> final_freqs;
        static List<int> final_durations;

        static string odeToJoy_notes = "eefg gfed ccde edd eefg gfed ccde dcc dec defec defed cdg eefg gfed ccde dcc";
        static string odeToJoy_durations = "cccc cccc cccc dqm cccc cccc cccc dqm mcc cqqcc cqqcc ccm cccc cccc cccc dqm";
        static string notes = odeToJoy_notes;
        static string durations = odeToJoy_durations;

        static void ParseStaff()
        {
            final_freqs = new List<int>();
            final_durations = new List<int>();
            foreach (char note in notes)
            {
                switch (note)
                {
                    // c major
                    case 'c':
                        final_freqs.Add(262);
                        break;
                    case 'd':
                        final_freqs.Add(294);
                        break;
                    case 'e':
                        final_freqs.Add(330);
                        break;
                    case 'f':
                        final_freqs.Add(349);
                        break;
                    case 'g':
                        final_freqs.Add(392);
                        break;
                    case 'a':
                        final_freqs.Add(440);
                        break;
                    case 'b':
                        final_freqs.Add(493);
                        break;
                    default:
                        break;
                }
            }
            foreach (char mod in durations)
            {
                switch (mod)
                {
                    case 'c': // crotchet
                        final_durations.Add(noteDuration);
                        break;
                    case 'd': // dot
                        final_durations.Add((int)Math.Round(noteDuration * 1.5));
                        break;
                    case 'q': // quaver
                        final_durations.Add(noteDuration / 2);
                        break;
                    case 'm': // minim
                        final_durations.Add(noteDuration * 2);
                        break;
                }
            }
        }

        static void Play()
        {
            for (int i = 0; i < final_freqs.Count; i++)
            {
                Console.Beep(final_freqs[i], final_durations[i]);
            }
        }

        static void Main(string[] args)
        {
            ParseStaff();
            Play();
        }
    }
}
