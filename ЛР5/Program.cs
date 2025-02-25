using System;
using System.Collections.Generic;
using System.IO;
using System.Xml.Serialization;

namespace StudentGradesApp
{
    public enum Course
    {
        PhysicalEducation,
        FirstAid,
        Ethics,
        QuantumMechanics,
        Literature
    }

    public interface IGradeManager
    {
        void InsertGrade();
        void DisplayGrades();
        void DeleteGrade();
        void ExportReport();
    }

    public class Assessment
    {
        public Course Course { get; set; }
        public int Mark { get; set; }
        public DateTime Timestamp { get; set; }

        public Assessment() { }

        public Assessment(Course course, int mark)
        {
            Course = course;
            Mark = mark;
            Timestamp = DateTime.Now;
        }
    }

    public class Learner
    {
        public string FullName { get; set; }
        public List<Assessment> ScoreList { get; set; } = new List<Assessment>();
    }

    public class GradeService : IGradeManager
    {
        private List<Learner> learners = new List<Learner>();
        public delegate void NotifyHandler(string message);
        public event NotifyHandler Alert;

        public GradeService()
        {
            Alert += msg => Console.WriteLine($"[INFO]: {msg}");
        }

        public void InsertGrade()
        {
            Console.Write("Введите имя студента: ");
            string fullName = Console.ReadLine();
            Learner learner = learners.Find(l => l.FullName == fullName) ?? new Learner { FullName = fullName };
            if (!learners.Contains(learner)) learners.Add(learner);

            Console.WriteLine("Выберите предмет:\n0 - Физкультура\n1 - Первая помощь\n2 - Этика\n3 - Квантовая механика\n4 - Литература");
            if (int.TryParse(Console.ReadLine(), out int courseIndex) && Enum.IsDefined(typeof(Course), courseIndex))
            {
                Console.Write("Введите оценку (0-5): ");
                if (int.TryParse(Console.ReadLine(), out int mark) && mark >= 0 && mark <= 5)
                {
                    learner.ScoreList.Add(new Assessment((Course)courseIndex, mark));
                    Alert?.Invoke($"Добавлена оценка {mark} студенту {learner.FullName}");
                }
                else
                {
                    Console.WriteLine("Ошибка: некорректное значение оценки.");
                }
            }
            else
            {
                Console.WriteLine("Ошибка: некорректный предмет.");
            }
        }

        public void DisplayGrades()
        {
            if (learners.Count == 0)
            {
                Console.WriteLine("Нет данных об оценках.");
                return;
            }
            foreach (var learner in learners)
            {
                Console.WriteLine($"Студент: {learner.FullName}");
                foreach (var assessment in learner.ScoreList)
                {
                    Console.WriteLine($"  Дисциплина: {assessment.Course}, Оценка: {assessment.Mark}, Дата: {assessment.Timestamp}");
                }
            }
        }

        public void DeleteGrade()
        {
            Console.Write("Введите имя студента: ");
            string fullName = Console.ReadLine();
            Learner learner = learners.Find(l => l.FullName == fullName);
            if (learner == null || learner.ScoreList.Count == 0)
            {
                Console.WriteLine("Нет доступных оценок для удаления.");
                return;
            }
            Console.Write("Введите номер оценки для удаления: ");
            if (int.TryParse(Console.ReadLine(), out int idx) && idx >= 0 && idx < learner.ScoreList.Count)
            {
                learner.ScoreList.RemoveAt(idx);
                Alert?.Invoke($"Удалена оценка у {learner.FullName}");
            }
            else
            {
                Console.WriteLine("Ошибка: некорректный индекс.");
            }
        }

        public void ExportReport()
        {
            string outputFile = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "grades_report.xml");

            XmlSerializer serializer = new XmlSerializer(typeof(List<Learner>));
            using (FileStream fs = new FileStream(outputFile, FileMode.Create))
            {
                serializer.Serialize(fs, learners);
            }

            Alert?.Invoke($"Файл сохранен: {outputFile}");
        }
    }

    class Application
    {
        static void Main()
        {
            GradeService gradeSystem = new GradeService();

            while (true)
            {
                Console.WriteLine("\nМеню:");
                Console.WriteLine("1. Добавить оценку");
                Console.WriteLine("2. Посмотреть оценки");
                Console.WriteLine("3. Удалить оценку");
                Console.WriteLine("4. Сохранить отчет");
                Console.WriteLine("5. Выход");
                Console.Write("Выберите действие: ");
                string option = Console.ReadLine();

                switch (option)
                {
                    case "1":
                        gradeSystem.InsertGrade();
                        break;
                    case "2":
                        gradeSystem.DisplayGrades();
                        break;
                    case "3":
                        gradeSystem.DeleteGrade();
                        break;
                    case "4":
                        gradeSystem.ExportReport();
                        break;
                    case "5":
                        return;
                    default:
                        Console.WriteLine("Ошибка: некорректный ввод.");
                        break;
                }
            }
        }
    }
}
