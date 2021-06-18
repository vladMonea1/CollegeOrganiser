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
    [Controller]
    public class AnnouncementsController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;
        public AnnouncementsController(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
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
        public async Task<IActionResult> CreateAnnouncement(DateTime CreatedOn, string Title, string Description/*, string Author*/)
        {
            CreatedOn = DateTime.Now;
            

            var anuntNou = new AnuntModel
            {
                CreatedOn = CreatedOn,
                Title = Title,
                Description= Description,
                Author= _userManager.GetUserAsync(User).Result.NumeUtilizator
            };


                  _context.AnuntModel.Add(anuntNou);
          await  _context.SaveChangesAsync();
            return RedirectToAction("GetAllAnnouncements");
        }

       
        [HttpGet]
        public async Task<IActionResult>SearchAnnouncements(String SearchPhrase)
        {
            var rezultatCautare= await _context.AnuntModel.Where(j => j.Author.Contains(SearchPhrase)).ToListAsync();
         
            return View(rezultatCautare);
            
        }



        public async Task<IActionResult> DeleteAnnouncement( int Id)
        {
           
            var postToDelete = _context.AnuntModel.FirstOrDefault(p => p.Id == Id);

            if(_userManager.GetUserAsync(User).Result.NumeUtilizator==postToDelete.Author)
            {
            var anunt = _context.AnuntModel.Remove(postToDelete);

            _context.SaveChanges();
            }
            return RedirectToAction("GetAllAnnouncements");
        }

       

    
    }
}
