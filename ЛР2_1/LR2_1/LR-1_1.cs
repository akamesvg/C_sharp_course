using System;
using System.Threading;

class Program1
{
    static void Main01()
    {
        Thread thread1 = new Thread(() => Print(1, 5));
        thread1.Start();

        Thread thread2 = new Thread(() => Print(10, 15));
        thread2.Start();

        thread1.Join();
        thread2.Join();

        Console.WriteLine("Потоки выполнились");
    }

    static void Print(int start, int end)
    {
        for (int i = start; i <= end; i++)
        {
            Console.WriteLine($"Поток {Thread.CurrentThread.ManagedThreadId}: {i}");
            Thread.Sleep(200);
        }
    }
}