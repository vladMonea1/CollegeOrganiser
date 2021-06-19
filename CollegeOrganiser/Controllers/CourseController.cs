   using CollegeOrganiser.Data;
using CollegeOrganiser.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CollegeOrganiser.Controllers
{
    public class CourseController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public CourseController(ApplicationDbContext context,
                                UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }
        public IActionResult Index()
        {
            var courses = _context.Courses.ToList();
            return View(courses);
        }
        public async Task<IActionResult> JoinCourse(int ?id)
        {
            if (id != null)
            {
                var user = _userManager.Users.FirstOrDefault(u => u.UserName == User.Identity.Name);
                var courseToJoin = _context.Courses.FirstOrDefault(c => c.Id == id);
                var course = new CoursesForUser()
                {
                    User = user,
                    CoursesAssigned = courseToJoin,
                    AssignedToCourse = true
                };
                if (!_context.CoursesForUsers.Any(c => c.CoursesAssigned.CourseName==courseToJoin.CourseName))
                {
                    _context.CoursesForUsers.Add(course);
                    await _context.SaveChangesAsync();
                }
                return RedirectToAction("MyCourses");
            }
            return RedirectToAction("Index");
        }

        public async Task<IActionResult> MyCourses()
        {
            var user = _userManager.Users.FirstOrDefault(u => u.UserName == User.Identity.Name);
            var myCourses = _context.CoursesForUsers.Include(u => u.User).Include(c => c.CoursesAssigned).Where(u => u.User == user).ToList();

            return View(myCourses);
        }


        public IActionResult CreateCourse()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> CreateCourse(/*string NumeProfesor ,*/string NumeCurs)
        {
            var course = new Courses()
            {
                CourseOwner = _userManager.GetUserAsync(User).Result.NumeUtilizator,
                CourseName = NumeCurs
            };

            if(!_context.Courses.Any(c=>c==course))
            {
                _context.Courses.Add(course);
                await _context.SaveChangesAsync();
            }
            return RedirectToAction("Index");

           // _context.CoursesHeld.Include(c=>c.Course).Where(c=>c.CourseId==id)
        }

        
        [HttpGet]
        public async Task<IActionResult>SearchCourseResult(String SearchPhrase)
        {
            var rezultatCautare = await _context.Courses.Where(j => j.CourseName.Contains(SearchPhrase)).ToListAsync();

            return View(rezultatCautare);

        }
    }
}
