using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace LabWork2
{
    internal class Program
    {
        static async Task Main06(string[] args)
        {
            var subject = new Subject("Математика");

            var group = new List<Pupil>
            {
                new Pupil("Иванов", new List<double>{ 4, 5, 3, 4 }),
                new Pupil("Петров", new List<double>{ 3, 4, 5, 4 }),
                new Pupil("Сидоров", new List<double>{ 5, 5, 5, 5 }),
                new Pupil("Кузнецов", new List<double>{ 3, 3, 4, 4 }),
                new Pupil("Смирнов", new List<double>{ 4, 4, 4, 4 })
            };

            subject.Enroll(group);

            await Benchmark("Обычный способ", subject.SyncAverage);
            await Benchmark("Parallel.ForEach", subject.ParallelAverage);
            await Benchmark("Async/Await", subject.AsyncAverage);
            await Benchmark("Через потоки", subject.ThreadAverage);
        }

        static async Task Benchmark(string title, Func<double> method)
        {
            var timer = Stopwatch.StartNew();
            var result = await Task.Run(method);
            timer.Stop();
            Console.WriteLine($"{title}: средний балл = {result:F2}, время = {timer.ElapsedMilliseconds} мс");
        }

        static async Task Benchmark(string title, Func<Task<double>> method)
        {
            var timer = Stopwatch.StartNew();
            var result = await method();
            timer.Stop();
            Console.WriteLine($"{title}: средний балл = {result:F2}, время = {timer.ElapsedMilliseconds} мс");
        }
    }

    class Subject
    {
        public string Title { get; }
        private List<Pupil> Pupils { get; set; } = new List<Pupil>();
        private readonly object locker = new object();

        public Subject(string title) => Title = title;

        public void Enroll(IEnumerable<Pupil> pupils)
        {
            Pupils.AddRange(pupils);
        }

        public double SyncAverage()
        {
            double sum = 0;
            int count = 0;

            foreach (var p in Pupils)
            {
                foreach (var g in p.Marks)
                {
                    sum += g;
                    count++;
                    Thread.Sleep(10);
                }
            }

            return count > 0 ? sum / count : 0;
        }

        public double ParallelAverage()
        {
            double total = 0;
            int totalCount = 0;

            Parallel.ForEach(Pupils, pupil =>
            {
                double localSum = 0;
                int localCount = 0;

                foreach (var mark in pupil.Marks)
                {
                    localSum += mark;
                    localCount++;
                    Thread.Sleep(10);
                }

                lock (locker)
                {
                    total += localSum;
                    totalCount += localCount;
                }
            });

            return totalCount > 0 ? total / totalCount : 0;
        }

        public async Task<double> AsyncAverage()
        {
            var tasks = Pupils.Select(p => Task.Run(() =>
            {
                double s = 0;
                int c = 0;
                foreach (var m in p.Marks)
                {
                    s += m;
                    c++;
                    Thread.Sleep(10);
                }
                return (s, c);
            }));

            var results = await Task.WhenAll(tasks);
            var totalSum = results.Sum(r => r.Item1);
            var totalCount = results.Sum(r => r.Item2);

            return totalCount > 0 ? totalSum / totalCount : 0;
        }

        public double ThreadAverage()
        {
            double finalSum = 0;
            int finalCount = 0;
            var threadList = new List<Thread>();

            foreach (var pupil in Pupils)
            {
                var t = new Thread(() =>
                {
                double s = 0;
                int c = 0;

                foreach (var m in pupil.Marks)
                {
                    s += m;
                    c++;
                    Thread.Sleep(10);
                }

                lock (locker)
                {
                    finalSum += s;
                    finalCount += c;
                }
            });

            threadList.Add(t);
            t.Start();
        }

            foreach (var t in threadList)
                t.Join();

            return finalCount > 0 ? finalSum / finalCount : 0;
        }
}

class Pupil
{
    public string FullName { get; }
    public List<double> Marks { get; }

    public Pupil(string name, List<double> marks)
    {
        FullName = name;
        Marks = marks;
    }
}
}
