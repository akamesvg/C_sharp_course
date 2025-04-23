﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ЛР_2_2.Entities
{
    public class Student
    {
        [Key]
        public int StudentId { get; set; }
        [Required]
        public string? FirstName { get; set; } 
        [Required]
        public string? LastName { get; set; }  
        public int Age { get; set; }          
        public string? Address { get; set; }   

        public virtual ICollection<Grade>? Grades { get; set; }

        public virtual ICollection<Enrollment> Enrollments { get; set; } = new List<Enrollment>();

    }
}
