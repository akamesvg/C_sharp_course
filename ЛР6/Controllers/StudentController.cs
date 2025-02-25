using Microsoft.AspNetCore.Mvc;
using WebApplication1.Model;

namespace WebApplication1.Controllers
{
    [ApiController]
    [Route("api/courses")]
    public class CoursesController : ControllerBase
    {
        private static readonly List<Course> CourseStorage = new();

        public static void SeedData(List<Course> courses)
        {
            CourseStorage.Clear();
            CourseStorage.AddRange(courses);
        }

        [HttpGet]
        public IActionResult FetchAllCourses()
        {
            return CourseStorage.Any() ? Ok(CourseStorage) : NoContent();
        }

        [HttpGet("{courseId}")]
        public IActionResult FetchCourseById(int courseId)
        {
            var foundCourse = CourseStorage.SingleOrDefault(c => c.Id == courseId);
            return foundCourse is not null ? Ok(foundCourse) : NotFound();
        }
    }
}

