using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace CollegeOrganiser.Models
{
    public class CoursesHeld
    {   
        [Key]
        public int Id { get; set; }
        public DateTime DateHeld { get; set; }

        public Courses Course { get; set; }
      

    }
}
