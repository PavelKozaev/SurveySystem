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
            base.OnModelCreating(modelBuilder);

            #region Конфигурация индексов

            modelBuilder.Entity<Question>()
                .HasIndex(q => new { q.SurveyId, q.Order })
                .IsUnique();

            modelBuilder.Entity<Result>()
                .HasIndex(r => r.InterviewId);

            modelBuilder.Entity<Result>()
                .HasIndex(r => r.QuestionId);

            #endregion            

            #region Начальные данные (Data Seeding)

            var survey1Id = Guid.Parse("1f9a3e42-1c7b-4b8f-8e4a-3e8a1d7f2b1c");
            var survey2Id = Guid.Parse("c5e8b9f0-3a2d-4b1c-8e6f-0a9d8c7b6a5e");

            var q1_1Id = Guid.Parse("a1b2c3d4-e5f6-7788-9900-aabbccddeeff");
            var q1_2Id = Guid.Parse("ffeeddcc-bbaa-0099-8877-f6e5d4c3b2a1");

            var ans1_1Id_Moscow = Guid.Parse("11111111-1111-1111-1111-111111111111");
            var ans1_1Id_MoscowRegion = Guid.Parse("22222222-2222-2222-2222-222222222222");
            var ans1_1Id_Other = Guid.Parse("33333333-3333-3333-3333-333333333333");
            var ans1_2Id_Yes = Guid.Parse("44444444-4444-4444-4444-444444444444");
            var ans1_2Id_No = Guid.Parse("55555555-5555-5555-5555-555555555555");

            var q2_1Id = Guid.Parse("b2c3d4e5-f6a7-8899-0011-aabbccddeeff");
            var q2_2Id = Guid.Parse("d3e4f5a6-b7c8-9900-1122-ccddeeff0011");

            var ans2_1Id_Dev = Guid.Parse("66666666-6666-6666-6666-666666666666");
            var ans2_1Id_QA = Guid.Parse("77777777-7777-7777-7777-777777777777");
            var ans2_1Id_Manager = Guid.Parse("88888888-8888-8888-8888-888888888888");
            var ans2_2Id_Poor = Guid.Parse("99999999-9999-9999-9999-999999999999");
            var ans2_2Id_Avg = Guid.Parse("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaaaa");
            var ans2_2Id_Exc = Guid.Parse("bbbbbbbb-bbbb-bbbb-bbbb-bbbbbbbbbbbb");

            var completedInterviewId = Guid.Parse("eeeeeeee-eeee-eeee-eeee-eeeeeeeeeeee");
            var result1Id = Guid.Parse("f1f1f1f1-f1f1-f1f1-f1f1-f1f1f1f1f1f1");
            var result2Id = Guid.Parse("f2f2f2f2-f2f2-f2f2-f2f2-f2f2f2f2f2f2");


            modelBuilder.Entity<Survey>().HasData(
                new Survey { Id = survey1Id, Title = "Test Survey About Regions", CreatedAt = new DateTime(2024, 1, 1, 0, 0, 0, DateTimeKind.Utc) },
                new Survey { Id = survey2Id, Title = "Work Satisfaction Survey", Description = "A short survey about your job.", CreatedAt = new DateTime(2024, 2, 1, 0, 0, 0, DateTimeKind.Utc) }
            );

            modelBuilder.Entity<Question>().HasData(
                new Question { Id = q1_1Id, SurveyId = survey1Id, Text = "Which region do you live in?", Order = 1 },
                new Question { Id = q1_2Id, SurveyId = survey1Id, Text = "Are you satisfied with your region?", Order = 2 },
                new Question { Id = q2_1Id, SurveyId = survey2Id, Text = "What is your role?", Order = 1 },
                new Question { Id = q2_2Id, SurveyId = survey2Id, Text = "How would you rate your work-life balance (1 to 5)?", Order = 2 }
            );

            modelBuilder.Entity<Answer>().HasData(
                new Answer { Id = ans1_1Id_Moscow, QuestionId = q1_1Id, Text = "Moscow" },
                new Answer { Id = ans1_1Id_MoscowRegion, QuestionId = q1_1Id, Text = "Moscow Region" },
                new Answer { Id = ans1_1Id_Other, QuestionId = q1_1Id, Text = "Other region" },
                new Answer { Id = ans1_2Id_Yes, QuestionId = q1_2Id, Text = "Yes" },
                new Answer { Id = ans1_2Id_No, QuestionId = q1_2Id, Text = "No" },
                
                new Answer { Id = ans2_1Id_Dev, QuestionId = q2_1Id, Text = "Developer" },
                new Answer { Id = ans2_1Id_QA, QuestionId = q2_1Id, Text = "QA Engineer" },
                new Answer { Id = ans2_1Id_Manager, QuestionId = q2_1Id, Text = "Manager" },
                new Answer { Id = ans2_2Id_Poor, QuestionId = q2_2Id, Text = "1 (Poor)" },
                new Answer { Id = ans2_2Id_Avg, QuestionId = q2_2Id, Text = "3 (Average)" },
                new Answer { Id = ans2_2Id_Exc, QuestionId = q2_2Id, Text = "5 (Excellent)" }
            );

            modelBuilder.Entity<Interview>().HasData(
                new Interview
                {
                    Id = completedInterviewId,
                    SurveyId = survey1Id,
                    StartedAt = new DateTime(2024, 3, 1, 10, 0, 0, DateTimeKind.Utc),
                    CompletedAt = new DateTime(2024, 3, 1, 10, 5, 0, DateTimeKind.Utc)
                }
            );

            modelBuilder.Entity<Result>().HasData(
                new Result { Id = result1Id, InterviewId = completedInterviewId, QuestionId = q1_1Id, SelectedAnswerId = ans1_1Id_Moscow, CreatedAt = new DateTime(2024, 3, 1, 10, 2, 0, DateTimeKind.Utc) },
                new Result { Id = result2Id, InterviewId = completedInterviewId, QuestionId = q1_2Id, SelectedAnswerId = ans1_2Id_Yes, CreatedAt = new DateTime(2024, 3, 1, 10, 4, 0, DateTimeKind.Utc) }
            );

            #endregion
        }
    }
}
