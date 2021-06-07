using CollegeOrganiser.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CollegeOrganiser.Models
{
    public class AccountsModel
    {
        public List<UserModel> users { get; set; }
        public List<string> usersRoles { get; set; }
      
        public AccountsModel()
        {
            users = new List<UserModel>();
            usersRoles = new List<string>();
        }

        public async Task GetUsersAndRoles(UserManager<ApplicationUser> _userManager)
        {
            var users = await _userManager.Users.ToListAsync();
            foreach(var user in users)
            {
                UserModel model = new UserModel(user);
                var roles = await _userManager.GetRolesAsync(user);
                model.role = roles.FirstOrDefault();
                this.users.Add(model);
            }
        }

    }
}
