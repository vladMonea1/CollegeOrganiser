   using CollegeOrganiser.Data;
using CollegeOrganiser.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.IO;
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
        public async Task<IActionResult> JoinCourse(int? id)
        {
            //if (id != null)
            //{
                var user = _userManager.Users.FirstOrDefault(u => u.UserName == User.Identity.Name);
                var courseToJoin = _context.Courses.FirstOrDefault(c => c.Id == id);
                var course = new CoursesForUser()
                {
                    User = user,
                    CoursesAssigned = courseToJoin,
                    AssignedToCourse = true
                };
                //if (!_context.CoursesForUsers.Any(c => c.CoursesAssigned.CourseName == courseToJoin.CourseName))
                //{
                    _context.CoursesForUsers.Add(course);
                    await _context.SaveChangesAsync();
                //}
                return RedirectToAction("MyCourses");
            //}
            //return RedirectToAction("Index");
        }

        public async Task<IActionResult> MyCourses()
        {
            var user = _userManager.Users.FirstOrDefault(u => u.UserName == User.Identity.Name);
            var myCourses = _context.CoursesForUsers.Include(u => u.User).Include(c => c.CoursesAssigned).Where(u => u.User == user).ToList();

            return View(myCourses);
        }

        public async Task<IActionResult> MyCoursesTeacher()
        {
            var user = _userManager.Users.FirstOrDefault(u => u.UserName == User.Identity.Name);
            var myCoursesTeacher = _context.Courses.Where(p => p.CourseOwner == user.NumeUtilizator).ToList();

            return View(myCoursesTeacher);
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

            if (!_context.Courses.Any(c => c == course))
            {
                _context.Courses.Add(course);
                await _context.SaveChangesAsync();
            }
            return RedirectToAction("Index");


        }


        [HttpGet]
        public async Task<IActionResult> SearchCourseResult(String SearchPhrase)
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

            model.AnunturileCursului = await _context.AnunturiCurs.Where(p => p.forCourseWithId == id).ToListAsync();

            model.TemeleCursului = await _context.FileCurs.Where(p => p.fileForCourseWithId == id).ToListAsync();

            model.ContorPrezente =  _context.CoursesHeld.Include(c => c.Course).Where(course => course.Course.Id == id).Count();

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
        [HttpGet]
        public IActionResult AnuntCursNou(int CourseId)
        {
            return View(CourseId);
        }

        [HttpPost]
        public async Task<IActionResult> AnuntCursNou(DateTime CreatedOn, string Title, string Description, int CourseId)
        {

            #region testing
            /* var testid = CourseId;
             if (Description == null || Title == null || CreatedOn == null)
             {
                 TempData["CompletionError"] = "All fields are required";
                 return RedirectToAction("AnuntCursNou", new { id = CourseId });
             } */
            #endregion
            CreatedOn = DateTime.Now;
            var anuntNou = new AnuntModelCurs
            {
                CreatedOn = CreatedOn,
                Title = Title,
                Description = Description,
                forCourseWithId = CourseId,
                Author = _userManager.GetUserAsync(User).Result.NumeUtilizator,

            };


            _context.AnunturiCurs.Add(anuntNou);
            await _context.SaveChangesAsync();
            return RedirectToAction("ViewCourseFeed", new { id = CourseId });
        }



        public async Task<IActionResult> DeleteAnnouncementCurs(int Id, int CourseId)
        {

            var postToDelete = _context.AnunturiCurs.FirstOrDefault(p => p.Id == Id);

            if (_userManager.GetUserAsync(User).Result.NumeUtilizator == postToDelete.Author)
            {
                var anunt = _context.AnunturiCurs.Remove(postToDelete);

                _context.SaveChanges();
            }
            return RedirectToAction("ViewCourseFeed", new { id = CourseId });
        }


        [HttpGet]
        public IActionResult TemaCursNoua(int CourseId)
        {
            return View(CourseId);
        }


        [HttpPost]
        public async Task<IActionResult> TemaCursNoua(List<IFormFile> files, string description, int CourseId)

        {
            foreach (var file in files)
            {
                var fileName = Path.GetFileNameWithoutExtension(file.FileName);
                var extension = Path.GetExtension(file.FileName);
                var fileModel = new FileModelCurs
                {
                    CreatedOn = DateTime.UtcNow,
                    FileType = file.ContentType,
                    Extension = extension,
                    Name = fileName,
                    Description = description,
                    UploadedBy = _userManager.GetUserAsync(User).Result.NumeUtilizator,
                    fileForCourseWithId = CourseId


                };
                using (var dataStream = new MemoryStream())
                {
                    await file.CopyToAsync(dataStream);
                    fileModel.Data = dataStream.ToArray();
                }
                _context.FileCurs.Add(fileModel);
                _context.SaveChanges();
            }


            return RedirectToAction("ViewCourseFeed", new { id = CourseId });
        }



        public async Task<IActionResult> DownloadFileCurs(int id)
        {
            var file = await _context.FileCurs.Where(x => x.Id == id).FirstOrDefaultAsync();
            if (file == null) return null;
            return File(file.Data, file.FileType, file.Name + file.Extension);
        }

        public async Task<IActionResult> DeleteFileFromDatabase(int id, int CourseId)
        {
            var file = await _context.FileCurs.Where(x => x.Id == id).FirstOrDefaultAsync();
            _context.FileCurs.Remove(file);
            _context.SaveChanges();

            return RedirectToAction("ViewCourseFeed", new { id = CourseId });
        }

        public async Task<IActionResult> FeedbackPrezente(int CourseId)
        {
            List<EvidentaPrezenteModel> model = new List<EvidentaPrezenteModel>();

            var curs = _context.Courses.FirstOrDefault(c => c.Id == CourseId);
            var ceva = _context.CoursesForUsers.Include(u => u.User).Where(z => z.CoursesAssigned == curs);
            var useriLaCurs = ceva.Select(u => u.User);

            var toateCursurileTinute = _context.CoursesHeld.Where(x => x.Course.Id == CourseId);

            foreach (var user in useriLaCurs)
            {
                EvidentaPrezenteModel evidenta = new EvidentaPrezenteModel();
                evidenta.user = user;
                evidenta.controPrezente = _context.CourseAttendances.Where(c => c.User == user && toateCursurileTinute.Contains(c.CourseAttended)).Count();
                model.Add(evidenta);
            }
            
            return View(model);
        }
    }
}
