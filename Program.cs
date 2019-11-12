using System;
using System.Runtime.InteropServices;

namespace csharp_native_benchmarks
{
    class Program
    {
        static void Main(string[] args)
        {
            if (args.Length < 1)
            {
                Console.Error.WriteLine("Missing prefix");
                Environment.Exit(1);
            }

            var domain = args[0];
            helpers.Helper.WriteHeader();

            for (int i = 0; i < 20; i++)
            {
                Iterate(domain, i);
            }

        }

        static void Iterate(string domain, int iteration)
        {
            TimeSpan time;


            var file = helpers.Helper.Open("/dev/null", "w");
            for (int n = 10; n < 10000000; n *= 2)
            {
                time = helpers.Helper.Measure(() => benchmarks.DirectCsharpCall.StaticMethod("abc"), n);
                helpers.Helper.Inspect(domain, iteration, "ManagedStatic", time, n);

                var obj = new benchmarks.DirectCsharpCall();
                time = helpers.Helper.Measure(() => obj.VirtualMethod("abc"), n);
                helpers.Helper.Inspect(domain, iteration, "ManagedVirtual", time, n);

                time = helpers.Helper.Measure(() => benchmarks.DirectDllImportCall.fprintf(file, "abc\n"), n);
                helpers.Helper.Inspect(domain, iteration, "DllImportDirect", time, n);

                time = helpers.Helper.Measure(() => benchmarks.CustomMarshalDllImportCall.fprintf(file, "abc\n"), n);
                helpers.Helper.Inspect(domain, iteration, "DllImportCustomMarshaller", time, n);
            }


            var called = 0;
            benchmarks.MethodCalledFromC.CompareFunc func = (IntPtr a, IntPtr b) => {
                called += 1;
                return (int)(a.ToInt64() - b.ToInt64());
            };
            for (uint n = 10; n < 10000000; n *= 2)
            {
                called = 0;
                IntPtr data = benchmarks.MethodCalledFromC.PrepareData(n);
                time = helpers.Helper.Measure(() => benchmarks.MethodCalledFromC.qsort(data, n, (uint)Marshal.SizeOf<int>(), func), 1);
                helpers.Helper.Inspect(domain, iteration, "DllImportWithCallback", time, called);
                Marshal.FreeHGlobal(data);
            }
        }
    }
}
