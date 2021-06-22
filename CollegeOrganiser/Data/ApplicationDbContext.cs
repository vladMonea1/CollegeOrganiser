using CollegeOrganiser.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CollegeOrganiser.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        private readonly IHttpContextAccessor _httpContextAccesor;
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
            
        }
        public DbSet<FileModel> Files { get; set; }
        public DbSet<AnuntModel> AnuntModel { get; set; }

        public DbSet<AnuntModelCurs> AnunturiCurs { get; set; }
        public DbSet<FileModelCurs> FileCurs { get; set; }

        public DbSet<Courses> Courses { get; set; }
        public DbSet<CoursesHeld> CoursesHeld { get; set; }
        public DbSet<CourseAttendances> CourseAttendances{ get; set; }
        public DbSet<CoursesForUser> CoursesForUsers{ get; set; }
        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            foreach (var foreignKey in builder.Model.GetEntityTypes().SelectMany(e => e.GetForeignKeys()))
            {
                foreignKey.DeleteBehavior = DeleteBehavior.Restrict;
            }
        }
        

    }
}
