using ЛР_2_2.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ЛР_2_2;
using LR1_2_1;

namespace ЛР_2_2
{
    class Programm
    {
        public static void Main()
        {
            using (ApplicationContext db = new())
            {
                db.SeedData();

                var coursesWithStudents = db.Courses
                    .Include(c => c.Enrollments)
                    .ThenInclude(e => e.Student)
                    .ToList();

                Console.WriteLine("Курсы и студенты:");
                foreach (var course in coursesWithStudents)
                {
                    Console.WriteLine($"Курс: {course.CourseName}");
                    foreach (var enrollment in course.Enrollments)
                    {
                        Console.WriteLine($"  Студент: {enrollment.Student.FirstName} {enrollment.Student.LastName}");
                    }
                }

                var studentsWithCourses = db.Students
                    .Include(s => s.Enrollments)
                    .ThenInclude(e => e.Course)
                    .ToList();

                Console.WriteLine("\nСтуденты и курсы:");
                foreach (var student in studentsWithCourses)
                {
                    Console.WriteLine($"Студент: {student.FirstName} {student.LastName}");
                    foreach (var enrollment in student.Enrollments)
                    {
                        Console.WriteLine($"  Курс: {enrollment.Course.CourseName}");
                    }
                }

                var studentToUpdate = studentsWithCourses.FirstOrDefault();
                if (studentToUpdate != null)
                {
                    db.UpdateStudent(studentToUpdate.StudentId, "Михаил", "Звёздочкин", 20, "Братск");
                }

                var studentToDelete = studentsWithCourses.LastOrDefault();
                if (studentToDelete != null)
                {
                    db.DeleteStudent(studentToDelete.StudentId);
                }

                studentsWithCourses = db.Students
                    .Include(s => s.Enrollments)
                    .ThenInclude(e => e.Course)
                    .ToList();
                Console.WriteLine("\nСписок студентов после обновления и удаления:");
                foreach (var student in studentsWithCourses)
                {
                    Console.WriteLine($"Студент: {student.FirstName} {student.LastName}");
                    foreach (var enrollment in student.Enrollments)
                    {
                        Console.WriteLine($" Курс: {enrollment.Course.CourseName}");
                    }
                }

                var kirillGrades = db.Grades
                    .Where(g => g.Student.FirstName == "Кирилл")
                    .Select(g => new
                    {
                        CourseName = g.Course.CourseName,
                        Score = g.Score
                    })
                    .ToList();

                Console.WriteLine("\nОценки Кирилла:");
                foreach (var grade in kirillGrades)
                {
                    Console.WriteLine($"{grade.CourseName}: {grade.Score}");
                }

                var averageScores = db.Grades
                    .GroupBy(g => g.Course.CourseName)
                    .Select(g => new
                    {
                        CourseName = g.Key,
                        AverageScore = g.Average(gr => gr.Score)
                    })
                    .ToList();

                Console.WriteLine("\nСредний балл по курсам:");
                foreach (var item in averageScores)
                {
                    Console.WriteLine($"{item.CourseName}: {item.AverageScore:F2}");
                }

                Console.WriteLine();
            }


            using (ApplicationContext db = new())
            {
                db.SeedData();

                Console.WriteLine("--- ЛЕНИВАЯ ЗАГРУЗКА ---");

                var studentLazy = db.Students.OrderBy(s => s.StudentId).FirstOrDefault();
                if (studentLazy != null)
                {
                    Console.WriteLine($"Студент: {studentLazy.FirstName} {studentLazy.LastName}");
                    foreach (var enrollment in studentLazy.Enrollments)
                    {
                        Console.WriteLine($"  Курс: {enrollment.Course.CourseName}");
                    }
                }

                Console.WriteLine();
            }

            using (ApplicationContext db = new())
            {

                db.SeedData();

                Console.WriteLine("--- ЯВНАЯ ЗАГРУЗКА ---");

                var course = db.Courses.OrderBy(c => c.CourseId).FirstOrDefault();
                if (course != null)
                {
                    Console.WriteLine($"Курс: {course.CourseName}");

                    db.Entry(course).Collection(c => c.Enrollments).Load();
                    foreach (var enrollment in course.Enrollments)
                    {
                        db.Entry(enrollment).Reference(e => e.Student).Load();
                        Console.WriteLine($"  Студент: {enrollment.Student.FirstName} {enrollment.Student.LastName}");
                    }
                }

                var student = db.Students.OrderBy(s => s.StudentId).FirstOrDefault();
                if (student != null)
                {
                    Console.WriteLine($"\nСтудент: {student.FirstName} {student.LastName}");

                    db.Entry(student).Collection(s => s.Enrollments).Load();
                    foreach (var enrollment in student.Enrollments)
                    {
                        db.Entry(enrollment).Reference(e => e.Course).Load();
                        Console.WriteLine($" Курс: {enrollment.Course.CourseName}");
                    }
                }
            }
        }
    }
}
