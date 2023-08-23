using EduHome.Identity;
using EduHome.Models;
using EduHome.Models.common;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace EduHome.Contexts
{
    public class AppDbContext :IdentityDbContext<AppUser>
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { 
        }

        public DbSet<Slider> Sliders { get; set; } = null!;
        public DbSet<Event> Events { get; set; } = null!;
        public DbSet<Speaker> Speakers { get; set; } = null!;
        public DbSet<EventSpeaker> EventSpeakers { get; set; } = null!;
        public DbSet<CourseCategory> courseCategories{ get; set; } = null!;
        public DbSet<Course> courses{ get; set; } = null!;
        public DbSet<Category> Categories { get; set; } = null!;
		public DbSet<Teacher> Teachers { get; set; } = null!;
		public DbSet<SocialMedia> SocialMedias { get; set; } = null!;
		public DbSet<Skill> Skills { get; set; } = null!;
        public DbSet<Subscriber> Subscriber { get; set; } = null!;
        public DbSet<TeacherSkill> TeacherSkills { get; set; } = null!;
        public DbSet<Blog> Blogs { get; set; } = null!;
        public DbSet<Student> Students { get; set; }=null!;
        public DbSet<StudentSkill> studentSkills { get; set; } = null!;
        public DbSet<StudentSocialMedia>   studentSocialMedia { get; set; }= null!;
        public DbSet<Skill2> skill2s { get; set; } = null!;




        public override  Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            var entries = ChangeTracker.Entries<BaseSectionEntity>();
            foreach (var entry in entries)
            {
              
               switch (entry.State)
                {
                    case EntityState.Added:
						entry.Entity.CreatedDate = DateTime.UtcNow;
						entry.Entity.CreatedBy = "Admin";
						entry.Entity.UpdatedDate = DateTime.UtcNow;
						entry.Entity.UpdatedBy = "Admin";
                        break;
                        case EntityState.Modified:
						entry.Entity.UpdatedDate = DateTime.UtcNow;
						entry.Entity.UpdatedBy = "Admin";
                        break;
				}
            }
            return base.SaveChangesAsync(cancellationToken);
              
        }

    }
}
