using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace zad6
{
    class Zadatak6
    {
        public static void LongOperation(string taskName)
        {
            Thread.Sleep(1000);
            Console.WriteLine("{0} Finished . Executing Thread : {1}", taskName,
                Thread.CurrentThread.ManagedThreadId);
        }

        public static void Main(string[] args)
        {
            ConcurrentBag<int> iterations = new ConcurrentBag<int>();
            Parallel.For(0, 100, (i) =>
            {
                Thread.Sleep(1);
                iterations.Add(i);
            });
            Console.WriteLine("Bag length should be 100. Length is {0}",
                iterations.Count);
            Console.Read();
        }
    }
}