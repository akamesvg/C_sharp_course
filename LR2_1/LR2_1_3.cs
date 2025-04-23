using System;
using System.Threading;

namespace TrigLoopApp
{
    class TrigProcessor
    {
        private static double currentValue = 0.5;
        private static readonly object locker = new object();
        private static bool turnCos = true;

        static void Main03(string[] args)
        {
            var threadCos = new Thread(RunCos);
            var threadAcos = new Thread(RunAcos);

            threadCos.Start();
            threadAcos.Start();

            threadCos.Join();
            threadAcos.Join();
        }

        static void RunCos()
        {
            while (true)
            {
                lock (locker)
                {
                    while (!turnCos)
                    {
                        Monitor.Wait(locker);
                    }

                    currentValue = Math.Cos(currentValue);
                    Console.WriteLine($"[cos] => {currentValue:F6}");

                    turnCos = false;
                    Monitor.Pulse(locker);
                }

                Thread.Sleep(500);
            }
        }

        static void RunAcos()
        {
            while (true)
            {
                lock (locker)
                {
                    while (turnCos)
                    {
                        Monitor.Wait(locker);
                    }

                    var input = Math.Clamp(currentValue, -1.0, 1.0);
                    currentValue = Math.Acos(input);
                    Console.WriteLine($"[acos] => {currentValue:F6}");

                    turnCos = true;
                    Monitor.Pulse(locker);
                }

                Thread.Sleep(500);
            }
        }
    }
}
