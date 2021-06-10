using CollegeOrganiser.Data;
using CollegeOrganiser.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CollegeOrganiser.Controllers
{
    [Controller]
    public class AnnouncementsController : Controller
    {
        private readonly ApplicationDbContext _context;
        
        public AnnouncementsController(ApplicationDbContext context)
        {
            _context = context;
        }
        public IActionResult AnnouncementCreation()
        {
            return View();
        }

        public async Task<IActionResult> GetAllAnnouncements()
        {
            

            var anuntList = _context.AnuntModel.ToList();
            

            return View(anuntList);
        }
        [HttpPost]
        public async Task<IActionResult> CreateAnnouncement(DateTimeOffset CreatedOn, string Title, string Description, string Author)
        {
            var anuntNou = new AnuntModel
            {
                CreatedOn = CreatedOn,
                Title = Title,
                Description= Description,
                Author= Author
            };


                  _context.AnuntModel.Add(anuntNou);
          await  _context.SaveChangesAsync();
            return RedirectToAction("GetAllAnnouncements");
        }

        public async Task<IActionResult> DeleteAnnouncement( int Id)
        {

            var postToDelete = _context.AnuntModel.FirstOrDefault(p => p.Id == Id);
            var anunt = _context.AnuntModel.Remove(postToDelete);

            _context.SaveChanges();

            return RedirectToAction("GetAllAnnouncements");
        }

        //get announcement by 
        //get all announcements

        //post ann

    
    }
}
