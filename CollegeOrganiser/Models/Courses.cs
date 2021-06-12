using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace CollegeOrganiser.Models
{
    public class Courses
    {
        [Key]
        public int Id { get; set; }
        public string CourseName { get; set; }
        public string CourseOwner { get; set; }

    }
}
