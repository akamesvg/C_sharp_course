using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace UniversityGrades
{
    internal class EntryPoint
    {
        static async Task Main05(string[] args)
        {
            var gradeList = new List<Mark>
            {
                new Mark { Name = "Ярополк", Course = "Математика", Points = 90 },
                new Mark { Name = "Святослав", Course = "Физика", Points = 80 },
                new Mark { Name = "Иван", Course = "Математика", Points = 95 },
                new Mark { Name = "Илья", Course = "Физика", Points = 85 },
                new Mark { Name = "Павел", Course = "Математика", Points = 75 },
                new Mark { Name = "Александр", Course = "Физика", Points = 90 }
            };

            var names = new[] { "Ярополк", "Святослав", "Иван", "Илья", "Павел", "Александр" };
            var analyzer = new ScoreAnalyzer();

            var timer = System.Diagnostics.Stopwatch.StartNew();

            var tasks = names.Select(n => analyzer.GetAverageAsync(gradeList, n));
            var averages = await Task.WhenAll(tasks);

            for (int j = 0; j < names.Length; j++)
            {
                Console.WriteLine($"Средний балл для {names[j]}: {averages[j]:F2}");
            }

            timer.Stop();
            Console.WriteLine($"Подсчёты завершены за {timer.ElapsedMilliseconds} мс.");
        }
    }

    public class Mark
    {
        public string Name { get; set; }
        public string Course { get; set; }
        public int Points { get; set; }
    }

    public class ScoreAnalyzer
    {
        public async Task<double> GetAverageAsync(List<Mark> allMarks, string student)
        {
            await Task.Delay(100); // Имитация работы

            var relevantMarks = allMarks.Where(m => m.Name == student).ToList();

            if (!relevantMarks.Any())
                return 0;

            var sum = relevantMarks.Sum(m => m.Points);
            return (double)sum / relevantMarks.Count;
        }
    }
}
