using Microsoft.EntityFrameworkCore;
using SurveySystem.Domain.Entities;

namespace SurveySystem.Infrastructure.Persistence
{
    public class ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : DbContext(options)
    {
        public DbSet<Survey> Surveys { get; set; }
        public DbSet<Question> Questions { get; set; }
        public DbSet<Answer> Answers { get; set; }
        public DbSet<Interview> Interviews { get; set; }
        public DbSet<Result> Results { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Question>()
                .HasIndex(q => new { q.SurveyId, q.Order })
                .IsUnique();

            modelBuilder.Entity<Result>()
                .HasIndex(r => r.InterviewId);

            modelBuilder.Entity<Result>()
                .HasIndex(r => r.QuestionId);

            base.OnModelCreating(modelBuilder);
        }
    }
}
