using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace LR2_1
{
    class Program6
    {
        static async Task Main06()
        {
            var discipline = new Discipline("Математика");
            var students = new List<Student>
            {
                new Student("Иванов", new List<double> { 4, 5, 3, 4 }),
                new Student("Петров", new List<double> { 3, 4, 5, 4 }),
                new Student("Сидоров", new List<double> { 5, 5, 5, 5 }),
                new Student("Кузнецов", new List<double> { 3, 3, 4, 4 }),
                new Student("Смирнов", new List<double> { 4, 4, 4, 4 })
            };
            discipline.AddStudents(students);
            await TestMethod("Синхронный метод", discipline.CalculateAverageSync);
            await TestMethod("Parallel.ForEach", discipline.CalculateAverageParallel);
            await TestMethod("Task async/await", discipline.CalculateAverageAsync);
            await TestMethod("Thread", discipline.CalculateAverageThread);
        }

        static async Task TestMethod(string methodName, Func<double> method)
        {
            var stopwatch = Stopwatch.StartNew();
            var result = await Task.Run(method);
            stopwatch.Stop();
            Console.WriteLine($"{methodName}: средняя = {result:F2}, время = {stopwatch.ElapsedMilliseconds} мс");
        }

        static async Task TestMethod(string methodName, Func<Task<double>> method)
        {
            var stopwatch = Stopwatch.StartNew();
            var result = await method();
            stopwatch.Stop();
            Console.WriteLine($"{methodName}: средняя = {result:F2}, время = {stopwatch.ElapsedMilliseconds} мс");
        }
    }

    class Discipline
    {
        public string Name { get; }
        private List<Student> Students { get; } = new List<Student>();
        private readonly object lockObj = new object();

        public Discipline(string name) => Name = name;

        public void AddStudents(IEnumerable<Student> students) => Students.AddRange(students);

        public double CalculateAverageSync()
        {
            double sum = 0;
            int count = 0;

            foreach (var student in Students)
            {
                foreach (var grade in student.Grades)
                {
                    sum += grade;
                    count++;
                    Thread.Sleep(10);
                }
            }

            return count > 0 ? sum / count : 0;
        }

        public double CalculateAverageParallel()
        {
            double totalSum = 0;
            int totalCount = 0;

            Parallel.ForEach(Students, student =>
            {
                double studentSum = 0;
                int studentCount = 0;

                foreach (var grade in student.Grades)
                {
                    studentSum += grade;
                    studentCount++;
                    Thread.Sleep(10);
                }

                lock (lockObj)
                {
                    totalSum += studentSum;
                    totalCount += studentCount;
                }
            });

            return totalCount > 0 ? totalSum / totalCount : 0;
        }

        public async Task<double> CalculateAverageAsync()
        {
            var tasks = Students.Select(student =>
                Task.Run(() =>
                {
                    double sum = 0;
                    int count = 0;

                    foreach (var grade in student.Grades)
                    {
                        sum += grade;
                        count++;
                        Thread.Sleep(10);
                    }

                    return (sum, count);
                }));

            var results = await Task.WhenAll(tasks);
            var totalSum = results.Sum(r => r.sum);
            var totalCount = results.Sum(r => r.count);

            return totalCount > 0 ? totalSum / totalCount : 0;
        }

        public double CalculateAverageThread()
        {
            double totalSum = 0;
            int totalCount = 0;
            var threads = new List<Thread>();

            foreach (var student in Students)
            {
                var thread = new Thread(() =>
                {
                    double sum = 0;
                    int count = 0;

                    foreach (var grade in student.Grades)
                    {
                        sum += grade;
                        count++;
                        Thread.Sleep(10);
                    }

                    lock (lockObj)
                    {
                        totalSum += sum;
                        totalCount += count;
                    }
                });

                threads.Add(thread);
                thread.Start();
            }

            foreach (var thread in threads)
            {
                thread.Join();
            }

            return totalCount > 0 ? totalSum / totalCount : 0;
        }
    }
}

