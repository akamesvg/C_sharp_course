namespace WebApplication1.Model
{
    public class Student
    {
        public int Id { get; set; }
        public required string FirstName { get; set; }
        public required string LastName { get; set; }
        public required string Email { get; set; }
        public DateTime EnrollmentDate { get; set; } = DateTime.UtcNow;
        public List<Grade> Grades { get; set; } = new();
    }

    public class Course
    {
        public int Id { get; set; }
        public required string Title { get; set; }
        public string? Description { get; set; }
        public List<Grade> Grades { get; set; } = new();
    }

    public class Grade
    {
        public int Id { get; set; }
        public int StudentId { get; set; }
        public int CourseId { get; set; }
        public int Score { get; set; }
        public DateTime AssignedDate { get; set; } = DateTime.UtcNow;

        public Student? Student { get; set; }
        public Course? Course { get; set; }
    }
}
