using System;
using System.Threading;

namespace ThreadSequence
{
    class ThreadRunner
    {
        private static ManualResetEventSlim waitHandle = new ManualResetEventSlim(false);
        private static readonly object syncObj = new object();

        static void Main02(string[] args)
        {
            Thread first = new Thread(() =>
            {
                for (int number = 1; number <= 100; number++)
                {
                    lock (syncObj)
                    {
                        Console.WriteLine($"Первый поток: {number}");
                    }
                    Thread.Sleep(20);
                }
                waitHandle.Set();
            });

            Thread second = new Thread(() =>
            {
                waitHandle.Wait();
                for (int number = 101; number <= 150; number++)
                {
                    lock (syncObj)
                    {
                        Console.WriteLine($"Второй поток: {number}");
                    }
                    Thread.Sleep(20);
                }
            });

            first.Start();
            Thread.Sleep(2000); // Искусственная задержка перед запуском второго потока
            second.Start();

            first.Join();
            second.Join();
        }
    }
}
