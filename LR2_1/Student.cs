using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LR2_1_6
{
    class Student
    {
        public string LastName { get; }
        public List<double> Grades { get; }

        public Student(string lastName, List<double> grades)
        {
            LastName = lastName;
            Grades = grades;
        }
    }
}
