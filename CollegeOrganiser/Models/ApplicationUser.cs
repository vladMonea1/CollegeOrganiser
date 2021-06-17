using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CollegeOrganiser.Models
{
    public class ApplicationUser: IdentityUser
    {

        public String NumeUtilizator { get; set; }
        //just in case you want to add more data
    }
}
