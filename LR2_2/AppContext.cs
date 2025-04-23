using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;
using ЛР_2_2.Entities;

namespace LR1_2_1
{
    public class ApplicationContext : DbContext
    {
        public DbSet<Student> Students { get; set; } = null!;
        public DbSet<Course> Courses { get; set; } = null!;

        public DbSet<Grade> Grades { get; set; } = null!;

        public DbSet<Enrollment> Enrollments { get; set; } = null!;

        public ApplicationContext() { Database.EnsureDeleted(); Database.EnsureCreated(); }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlite("Data Source=sqlite.db");
            var config = new ConfigurationBuilder()
                        .SetBasePath(Directory.GetCurrentDirectory())
                        .Build();

            optionsBuilder.LogTo(Console.WriteLine, Microsoft.Extensions.Logging.LogLevel.Warning);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Enrollment>()
               .HasKey(e => new { e.StudentId, e.CourseId });

            modelBuilder.Entity<Enrollment>()
                .HasOne(e => e.Student)
                .WithMany(s => s.Enrollments)
                .HasForeignKey(e => e.StudentId);

            modelBuilder.Entity<Enrollment>()
                .HasOne(e => e.Course)
                .WithMany(c => c.Enrollments)
                .HasForeignKey(e => e.CourseId);

        }

        public void SeedData()
        {
            if (!Students.Any() && !Courses.Any() && !Grades.Any())
            {
                Student s1 = new Student { FirstName = "Кирилл", LastName = "Федорков", Age = 21, Address = "Братск" };
                Student s2 = new Student { FirstName = "Иван", LastName = "Прудников", Age = 20, Address = "Братск" };
                Student s3 = new Student { FirstName = "Ярослав", LastName = "Маркидонов", Age = 24, Address = "Братск" };

                Students.Add(s1);
                Students.Add(s2);
                Students.Add(s3);

                Course course1 = new Course { CourseName = "Математика" };
                Course course2 = new Course { CourseName = "Физика" };
                Course course3 = new Course { CourseName = "Химия" };

                Courses.Add(course1);
                Courses.Add(course2);

                Grades.Add(new Grade { Student = s1, Course = course1, Score = 5 });
                Grades.Add(new Grade { Student = s2, Course = course2, Score = 5 });

                SaveChanges();
            }
        }

        public void UpdateStudent(int studentId, string newFirstName, string newLastName, int newAge, string newAddress)
        {
            var student = Students.Find(studentId);
            if (student != null)
            {
                student.FirstName = newFirstName;
                student.LastName = newLastName;
                student.Age = newAge;
                student.Address = newAddress;
                SaveChanges();
            }
        }

        public void DeleteStudent(int studentId)
        {
            var student = Students.Find(studentId);
            if (student != null)
            {
                Students.Remove(student);
                SaveChanges();
            }
        }
    }
}
