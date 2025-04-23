using System;
using System.Threading;

namespace ThreadExample
{
    class ThreadDemo
    {
        static void Main01(string[] args)
        {
            Thread firstThread = new Thread(() => DisplayNumbers(1, 5));
            Thread secondThread = new Thread(() => DisplayNumbers(10, 15));

            firstThread.Start();
            secondThread.Start();

            firstThread.Join();
            secondThread.Join();

            Console.WriteLine("Все потоки завершены");
        }

        static void DisplayNumbers(int from, int to)
        {
            for (int num = from; num <= to; num++)
            {
                Console.WriteLine($"ID потока {Thread.CurrentThread.ManagedThreadId}: значение {num}");
                Thread.Sleep(200); // Пауза для наглядности
            }
        }
    }
}
