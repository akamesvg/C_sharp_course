using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LR2_1
{
    class Program05
    {
        static async Task Main05(string[] args)
        {
            List<Grade> grades = new List<Grade>
        {
            new Grade { StudentName = "Кирилл", Subject = "Математика", Score = 90 },
            new Grade { StudentName = "Кирилл", Subject = "Физика", Score = 80 },
            new Grade { StudentName = "Ярослав", Subject = "Математика", Score = 95 },
            new Grade { StudentName = "Ярослав", Subject = "Физика", Score = 85 },
            new Grade { StudentName = "Иван", Subject = "Математика", Score = 75 },
            new Grade { StudentName = "Иван", Subject = "Физика", Score = 90 }
        };

            
            List<string> students = new List<string> { "Кирилл", "Ярослав", "Иван" };

            var gradeService = new GradeServiceAsync();

            var watch = System.Diagnostics.Stopwatch.StartNew();

            var calculationTasks = students.Select(student => gradeService.CalculateAverageScoreAsync(grades, student));

            var results = await Task.WhenAll(calculationTasks);

            for (int i = 0; i < students.Count; i++)
            {
                Console.WriteLine($"Для студента {students[i]} средняя оценка: {results[i]}");
            }

            watch.Stop();
            Console.WriteLine($"Все вычисления завершены. Время выполнения: {watch.ElapsedMilliseconds} мс");
        }
    }

    public class GradeServiceAsync
    {
        public async Task<double> CalculateAverageScoreAsync(List<Grade> grades, string studentName)
        {
            await Task.Delay(100); 
            var studentGrades = grades.Where(g => g.StudentName == studentName).ToList();

            if (studentGrades.Count == 0)
                return 0;

            double totalScore = studentGrades.Sum(g => g.Score);
            return totalScore / studentGrades.Count;
        }
    }
}
