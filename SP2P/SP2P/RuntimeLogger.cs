using System;
using System.IO;

namespace SP2P
{
    class RuntimeLogger
    {
        public static void Open(string fpath)
        {
#if !DEBUG
            Close();
            f = new StreamWriter(fpath, true);
#endif
        }
        public static void Close()
        {
            f?.Close();
        }
#pragma warning disable 0649  
        static StreamWriter f;
#pragma warning restore 0649  

        static object locker = new object();

        public static void WriteLine()
        {
            DateTime dt = DateTime.Now;
#if !DEBUG
            lock (locker)
            {
                f.Write($"{dt} | ");
                f.WriteLine();
            }
#else
            Console.Write($@"{dt} | ");
            Console.WriteLine();
#endif
        }
        public static void WriteLine(object value)
        {
            DateTime dt = DateTime.Now;
#if !DEBUG
            lock (locker)
            {
                f.Write($"{dt} | ");
                f.WriteLine(value);
            }
#else
            Console.Write($@"{dt} | ");
            Console.WriteLine(value);
#endif
        }
        public static void WriteLine(char[] buffer)
        {
            DateTime dt = DateTime.Now;
#if !DEBUG
            lock (locker)
            {
                f.Write($"{dt} | ");
                f.WriteLine(buffer);
            }
#else
            Console.Write($@"{dt} | ");
            Console.WriteLine(buffer);
#endif
        }
        public static void WriteLine(char[] buffer, int index, int count)
        {
            DateTime dt = DateTime.Now;
#if !DEBUG
            lock (locker)
            {
                f.Write($"{dt} | ");
                f.WriteLine(buffer, index, count);
            }
#else
            Console.Write($@"{dt} | ");
            Console.WriteLine(buffer, index, count);
#endif
        }
        public static void WriteLine(string format, params object[] arg)
        {
            DateTime dt = DateTime.Now;
#if !DEBUG
            lock (locker)
            {
                f.Write($"{dt} | ");
                f.WriteLine(format, arg);
            }
#else
            Console.Write($@"{dt} | ");
            Console.WriteLine(format, arg);
#endif
        }
        public static void Write(object value)
        {
            DateTime dt = DateTime.Now;
#if !DEBUG
            lock (locker)
            {
                f.Write(value);
            }
#else
            Console.Write(value);
#endif
        }
        public static void Write(char[] buffer)
        {
            DateTime dt = DateTime.Now;
#if !DEBUG
            lock (locker)
            {
                f.Write(buffer);
            }
#else
            Console.Write(buffer);
#endif
        }
        public static void Write(char[] buffer, int index, int count)
        {
            DateTime dt = DateTime.Now;
#if !DEBUG
            lock (locker)
            {
                f.Write(buffer, index, count);
            }
#else
            Console.Write(buffer, index, count);
#endif
        }
        public static void Write(string format, params object[] arg)
        {
            DateTime dt = DateTime.Now;
#if !DEBUG
            lock (locker)
            {
                f.Write(format, arg);
            }
#else
            Console.Write(format, arg);
#endif
        }
    }
}
