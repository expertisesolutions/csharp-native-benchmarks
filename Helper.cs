using System;
using System.Diagnostics;
using System.Runtime.InteropServices;

namespace helpers
{
    public static class Helper
    {
        public static TimeSpan Measure(Action action, int n = 1)
        {
            var stopwatch = new Stopwatch();
            stopwatch.Start();
            for (int i = 0; i < n; i++)
            {
                action();
            }
            stopwatch.Stop();

            return stopwatch.Elapsed;
        }

        public static void WriteHeader()
        {
            Console.WriteLine("domain,name,iteration,size,span,ns,us");
        }

        public static void Inspect(string domain, int iteration, string title, TimeSpan span, int n)
        {
            var ms = span.TotalMilliseconds + span.Ticks / 10000;
            var ms_per_run = ms / n ;
            Console.WriteLine($"{domain},{title},{iteration},{n},{span},{1000000 * ms_per_run},{1000 * ms_per_run}");
        }


        [DllImport("libc")]
        private static extern IntPtr fopen(string name, string mode);

        public static IntPtr Open(string name, string mode = "r")
        {
            return fopen(name, mode);
        }
    }
}
