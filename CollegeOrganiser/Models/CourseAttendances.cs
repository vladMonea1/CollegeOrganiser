using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace CollegeOrganiser.Models
{
    public class CourseAttendances
    {
        [Key]
        public int Id { get; set; }

        public bool Attended { get; set; }

        public CoursesHeld CourseAttended { get; set; }
        public ApplicationUser User{ get; set; }

    }
}
