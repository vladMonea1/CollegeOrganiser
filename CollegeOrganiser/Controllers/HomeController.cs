using CollegeOrganiser.Data;
using CollegeOrganiser.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace CollegeOrganiser.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly ApplicationDbContext _dbContext;
        public HomeController(ILogger<HomeController> logger,
                                UserManager<ApplicationUser> userManager,
                                ApplicationDbContext dbContext, RoleManager<IdentityRole> roleManager )
        {
            _logger = logger;
            _userManager = userManager;
            _dbContext = dbContext;
            _roleManager = roleManager;
        }

        public IActionResult Index()
        {
            return View();
        }


        // [Authorize(Roles ="Admin")]
        public async Task<IActionResult> GetUsers()
        {
            AccountsModel model = new AccountsModel();
            await model.GetUsersAndRoles(_userManager);

            var rolesList = _roleManager.Roles.ToList();
            model.usersRoles = rolesList.Select(r=>r.Name).ToList();
            return View(model);
        }

        public async Task <IActionResult> ChangeRole(string email, string designatedrole)
        {
            var rolesList = _roleManager.Roles.Select(r => r.Name).ToList();
            var user = _userManager.Users.FirstOrDefault(u => u.Email == email);
            await _userManager.RemoveFromRolesAsync(user, rolesList);
           var result= await _userManager.AddToRoleAsync(user, designatedrole);
           
            return RedirectToAction("GetUsers");
        }
      

        public IActionResult Privacy()
        {
            if(User.IsInRole("Profesor"))
            {
                return View();
            }
            else
            return LocalRedirect("/Identity/Account/AccessDenied");
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
