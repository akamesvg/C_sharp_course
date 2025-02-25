using System;
using System.Collections.Generic;

// Перечисление предметов
public enum Subject
{
    Math,
    Physics,
    Chemistry
}

// Структура Grade
public struct Grade
{
    public Subject Subject { get; }
    public int Score { get; }
    public DateTime Date { get; }

    public Grade(Subject subject, int score, DateTime date)
    {
        Subject = subject;
        Score = score;
        Date = date;
    }

    public override string ToString()
    {
        return $"{Subject}: {Score}, {Date.ToShortDateString()}";
    }
}

// Класс для управления оценками
public class GradeManager
{
    private List<Grade> grades = new List<Grade>();

    // Метод для добавления оценки
    public void AddGrade(Subject subject, int score)
    {
        grades.Add(new Grade(subject, score, DateTime.Now));
    }

    // Метод для удаления оценки
    public bool RemoveGrade(Subject subject, int score)
    {
        Grade? gradeToRemove = grades.Find(g => g.Subject == subject && g.Score == score);
        if (gradeToRemove.HasValue)
        {
            grades.Remove(gradeToRemove.Value);
            return true;
        }
        return false;
    }

    // Метод для поиска оценок по предмету
    public List<Grade> GetGradesBySubject(Subject subject)
    {
        return grades.FindAll(g => g.Subject == subject);
    }

    // Метод для вывода всех оценок
    public void DisplayGrades()
    {
        if (grades.Count == 0)
        {
            Console.WriteLine("Оценок пока нет.");
            return;
        }

        foreach (var grade in grades)
        {
            Console.WriteLine(grade);
        }
    }
}

// Тестирование программы
class Program
{
    static void Main()
    {
        GradeManager manager = new GradeManager();

        manager.AddGrade(Subject.Math, 85);
        manager.AddGrade(Subject.Physics, 90);
        manager.AddGrade(Subject.Chemistry, 78);

        Console.WriteLine("Все оценки:");
        manager.DisplayGrades();

        Console.WriteLine("\nОценки по предмету Math:");
        foreach (var grade in manager.GetGradesBySubject(Subject.Math))
        {
            Console.WriteLine(grade);
        }

        Console.WriteLine("\nУдаляем оценку по физике...");
        if (manager.RemoveGrade(Subject.Physics, 90))
            Console.WriteLine("Оценка удалена.");
        else
            Console.WriteLine("Оценка не найдена.");

        Console.WriteLine("\nОбновленный список оценок:");
        manager.DisplayGrades();
    }
}
