using CollegeOrganiser.Data;
using CollegeOrganiser.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CollegeOrganiser.Controllers
{   [Controller]
    [Authorize]
    public class AttendanceController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        public AttendanceController(ApplicationDbContext context,
                                                        UserManager<ApplicationUser> userManager,
                                                        RoleManager<IdentityRole> roleManager)
        {
            _context = context;
            _userManager = userManager;
            _roleManager = roleManager;
        }


        public IActionResult Index()
        {

            var attendance = _context.CourseAttendances.Include(u => u.User)
                 .Include(c => c.CourseAttended)
                 .Include(ch => ch.CourseAttended.Course).ToList();

            return View(attendance);
        }

   



    }
}
