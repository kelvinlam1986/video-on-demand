using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using VideoOnDemand.Data.Data.Entities;

namespace VideoOnDemand.Data.Data
{
    public class VODContext : IdentityDbContext<User>
    {
        public VODContext(DbContextOptions<VODContext> options) : base(options)
        {
        }

        public DbSet<Course> Courses { get; set; }
        public DbSet<Download> Downloads { get; set; }
        public DbSet<Instructor> Instructors { get; set; }
        public DbSet<Module> Modules { get; set; }
        public DbSet<UserCourse> UserCourses { get; set; }
        public DbSet<Video> Videos { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<UserCourse>().HasKey(uc => new { uc.UserId, uc.CourseId });
            foreach (var relationship in 
                builder.Model.GetEntityTypes().SelectMany(e => 
                    e.GetForeignKeys()))
            {
                relationship.DeleteBehavior = DeleteBehavior.Restrict;
            }
        }
    }
}
