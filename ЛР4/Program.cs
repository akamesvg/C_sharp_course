using System;
using System.Collections.Generic;

namespace StudentGrades
{
    enum Subjects
    {
        PE,
        BLS,
        Philosophy,
        Physics,
        Russian
    }

    interface IGradeManager
    {
        void AddMark();
        void ShowMarks();
        void DeleteMark();
    }

    class Mark
    {
        public Subjects Subject { get; set; }
        public int Value { get; set; }
        public DateTime DateReceived { get; set; }

        public Mark(Subjects subject, int value)
        {
            Subject = subject;
            Value = value;
            DateReceived = DateTime.Now;
        }
    }

    class GradeBook
    {
        protected List<Mark> marksList = new List<Mark>();
    }

    class GradeSystem : GradeBook, IGradeManager
    {
        public void AddMark()
        {
            Console.WriteLine("Выберите предмет\n0 - PE;\n1 - BLS;\n2 - Philosophy;\n3 - Physics;\n4 - Russian.");
            if (int.TryParse(Console.ReadLine(), out int subjectIndex) && Enum.IsDefined(typeof(Subjects), subjectIndex))
            {
                Subjects chosenSubject = (Subjects)subjectIndex;
                Console.Write("Введите оценку (0-5): ");
                if (int.TryParse(Console.ReadLine(), out int markValue) && markValue >= 0 && markValue <= 5)
                {
                    marksList.Add(new Mark(chosenSubject, markValue));
                    Console.WriteLine("Оценка добавлена успешно!");
                }
                else
                {
                    Console.WriteLine("Ошибка: некорректная оценка!");
                }
            }
            else
            {
                Console.WriteLine("Ошибка: некорректный предмет!");
            }
        }

        public void ShowMarks()
        {
            if (marksList.Count == 0)
            {
                Console.WriteLine("Оценок пока нет.");
                return;
            }
            Console.WriteLine("Ваши оценки:");
            foreach (var mark in marksList)
            {
                Console.WriteLine($"Предмет: {mark.Subject}, Оценка: {mark.Value}, Дата: {mark.DateReceived}");
            }
        }

        public void DeleteMark()
        {
            Console.Write("Введите номер оценки для удаления: ");
            if (int.TryParse(Console.ReadLine(), out int markIndex) && markIndex > 0 && markIndex <= marksList.Count)
            {
                marksList.RemoveAt(markIndex - 1);
                Console.WriteLine("Оценка удалена!");
            }
            else
            {
                Console.WriteLine("Ошибка: некорректный номер оценки!");
            }
        }
    }

    class Program
    {
        static void Main()
        {
            IGradeManager gradeSystem = new GradeSystem();

            while (true)
            {
                Console.WriteLine("\nМеню:");
                Console.WriteLine("1. Добавить оценку");
                Console.WriteLine("2. Посмотреть оценки");
                Console.WriteLine("3. Удалить оценку");
                Console.WriteLine("4. Выход");
                Console.Write("Выберите действие: ");
                string userChoice = Console.ReadLine();

                switch (userChoice)
                {
                    case "1":
                        gradeSystem.AddMark();
                        break;
                    case "2":
                        gradeSystem.ShowMarks();
                        break;
                    case "3":
                        gradeSystem.DeleteMark();
                        break;
                    case "4":
                        return;
                    default:
                        Console.WriteLine("Ошибка: некорректный ввод!");
                        break;
                }
            }
        }
    }
}
