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
        public async Task<IActionResult> CreateCourse(string NumeCurs)
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

          
        }

        // _context.CoursesHeld.Include(c=>c.Course).Where(c=>c.CourseId==id)
        [HttpGet]
        public async Task<IActionResult>SearchCourseResult(String SearchPhrase)
        {
            var rezultatCautare = await _context.Courses.Where(j => j.CourseName.Contains(SearchPhrase)).ToListAsync();

            return View(rezultatCautare);

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
