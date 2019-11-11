using System;
using System.Runtime.InteropServices;

namespace benchmarks
{
    public class DirectCsharpCall
    {
        public static void StaticMethod(string data)
        {

        }

        public virtual void VirtualMethod(string data)
        {
        }
    }

    public static class DirectDllImportCall
    {
        [DllImport("libc")]
        public static extern int fprintf(IntPtr stream, string format);
    }

    public static class CustomMarshalDllImportCall
    {
        [DllImport("libc")]
        public static extern int fprintf(IntPtr stream, [MarshalAs(UnmanagedType.CustomMarshaler, MarshalTypeRef=typeof(StringCustomMarshaler))]string format);
    }

    public class StringCustomMarshaler : ICustomMarshaler
    {

        private static ICustomMarshaler _instance = null;
        public static ICustomMarshaler GetInstance(string cookie)
        {
            if (_instance == null) {
                _instance = new StringCustomMarshaler();
            }

            return _instance;
        }
        public void CleanUpManagedData(object ManagedObj)
        {
            throw new NotImplementedException();
        }

        public void CleanUpNativeData(IntPtr pNativeData)
        {
            throw new NotImplementedException();
        }

        public int GetNativeDataSize()
        {
            throw new NotImplementedException();
        }

        public IntPtr MarshalManagedToNative(object ManagedObj)
        {
            return IntPtr.Zero;
        }

        public object MarshalNativeToManaged(IntPtr pNativeData)
        {
            throw new NotImplementedException();
        }
    }

    public static class MethodCalledFromC
    {
        public delegate int CompareFunc(IntPtr a, IntPtr b);

        [DllImport("libc", CallingConvention=CallingConvention.Cdecl)]
        public static extern void qsort(IntPtr data, uint nmemb, uint size, CompareFunc compar);

        public static IntPtr PrepareData(uint n)
        {
            int[] data = new int[n];
            for (int i = 0; i < n; i++)
            {
                data[i] = i;
            }
            IntPtr native_data = Marshal.AllocHGlobal((int)(Marshal.SizeOf<int>()*n));
            Marshal.Copy(data, 0, native_data, (int)n);
            return native_data;
        }
    }
}