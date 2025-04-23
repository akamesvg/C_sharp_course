using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LR2_1
{
    class Program3
    {
        private static double value = 0.5;
        private static object synchronization = new object();
        private static bool isCosinus = true;

        static void Main03()
        {
            Thread cosinusThread = new Thread(CalculateCos);
            Thread arccosinusThread = new Thread(CalculateAcos);

            cosinusThread.Start();
            arccosinusThread.Start();

            cosinusThread.Join();
            arccosinusThread.Join();
        }
        static void CalculateCos()
        {
            while (true)
            {
                lock (synchronization)
                {
                    if (!isCosinus)
                    {
                        Monitor.Wait(synchronization);
                    }
                    value = Math.Cos(value);
                    Console.WriteLine($"Косинус: {value}");
                    isCosinus = false;
                    Monitor.Pulse(synchronization);
                }
                Thread.Sleep(500);
            }
        }

        static void CalculateAcos()
        {
            while (true)
            {
                lock (synchronization)
                {
                    if (isCosinus)
                    {
                        Monitor.Wait(synchronization);
                    }
                    double arccosValue = Math.Max(-1, Math.Min(1, value));
                    value = Math.Acos(arccosValue);
                    Console.WriteLine($"Арккосинус: {value}");
                    isCosinus = true;
                    Monitor.Pulse(synchronization);
                }
                Thread.Sleep(500);
            }
        }
    }
}
