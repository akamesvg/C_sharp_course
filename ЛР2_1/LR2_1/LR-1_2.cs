using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LR2_1
{
    class Program2
    {
        private static ManualResetEventSlim end_first = new ManualResetEventSlim(false);
        private static object Lock = new object();

        static void Main02()
        {
            Thread thread1 = new Thread(() =>
            {
                for (int i = 1; i <= 100; i++)
                {
                    lock (Lock)
                    {
                        Console.WriteLine($"Поток 1: {i}");
                    }
                    Thread.Sleep(20);
                }
                end_first.Set();
            });
            Thread thread2 = new Thread(() =>
            {
                end_first.Wait();
                for (int i = 101; i <= 150; i++)
                {
                    lock (Lock)
                    {
                        Console.WriteLine($"Поток 2: {i}");
                    }
                    Thread.Sleep(20);
                }
            });
            thread1.Start();
            Thread.Sleep(2000);
            thread2.Start();
            thread1.Join();
            thread2.Join();
        }
    }
}
