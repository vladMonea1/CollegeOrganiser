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
                .Include(c=>c.CourseAttended)
                .Include(ch=>ch.CourseAttended.Course).ToList();

            return View(attendance);
        }

        [Route("Course/ViewCourseFeed/{id}")]

        public async Task<IActionResult> ViewCourseFeed(int id)
        {
            var user = _userManager.GetUserAsync(User).Result;
            ModelFeedCurs model = new ModelFeedCurs();

            model.CursurileCareSeTin = await _context.CoursesHeld.Include(c => c.Course).Where(course => course.Course.Id == id).ToListAsync();

            model.Prezente = await _context.CourseAttendances.Where(c => c.User == user && model.CursurileCareSeTin.Contains(c.CourseAttended)).ToListAsync();

            model.CourseId = id;

            return View(model);
        }

        [Route("Course/PunePrezenta/{id}")]

        public async Task<IActionResult> PunePrezenta(int id)
        {
            var cursTinut = _context.CoursesHeld.Include(c => c.Course).FirstOrDefault(ch => ch.Id == id);

            CourseAttendances prezenta = new CourseAttendances
            {
                Attended = true,
                User = _userManager.GetUserAsync(User).Result,
                CourseAttended = cursTinut

            };

            _context.CourseAttendances.Add(prezenta);
            _context.SaveChanges();

            var curs = cursTinut.Course;

            return RedirectToAction("ViewCourseFeed", new { id = curs.Id });
        }

        public async Task<IActionResult> InitiazaPrezenta(int id)
        {
            var cursCurent = _context.Courses.FirstOrDefault(c => c.Id == id);



            CoursesHeld curS = new CoursesHeld()
            {
                DateHeld = DateTime.Now,
                Course = cursCurent,

            };
            _context.CoursesHeld.Add(curS);
            _context.SaveChanges();

            return RedirectToAction("ViewCourseFeed", new { id = cursCurent.Id });
        }


    }
}
