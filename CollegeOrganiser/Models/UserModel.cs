using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CollegeOrganiser.Models
{
    public class UserModel
    {
        public ApplicationUser user { get; set; }
        public string role { get; set; }

        public UserModel(ApplicationUser user)
        {
            this.user = user;
        }
    }
}
