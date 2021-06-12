using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CollegeOrganiser.Models
{
    public class CoursesForUser
    {
        public int Id { get; set; }
        public bool AssignedToCourse { get; set; }

        public ApplicationUser User { get; set; }
 
        public Courses CoursesAssigned { get; set; }
    }
}
