using Microsoft.EntityFrameworkCore;
using SurveySystem.Application;
using SurveySystem.Application.Interfaces;
using SurveySystem.Application.Services;
using SurveySystem.Domain.Entities;
using SurveySystem.Infrastructure.Persistence;

var builder = WebApplication.CreateBuilder(args);

var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseNpgsql(connectionString));

builder.Services.AddScoped<IInterviewService, InterviewService>();
builder.Services.AddAutoMapper(typeof(MappingProfile));

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    var context = services.GetRequiredService<ApplicationDbContext>();
    context.Database.Migrate();
    await SeedData(context);
}

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseAuthorization();

app.MapControllers();

app.Run();

async Task SeedData(ApplicationDbContext context)
{
    if (!await context.Surveys.AnyAsync())
    {
        var survey = new Survey
        {
            Id = Guid.Parse("1f9a3e42-1c7b-4b8f-8e4a-3e8a1d7f2b1c"),
            Title = "Тестовый опрос о регионах",
            CreatedAt = DateTime.UtcNow
        };

        var question1 = new Question { Id = Guid.NewGuid(), SurveyId = survey.Id, Text = "В каком регионе Вы проживаете?", Order = 1 };
        var question2 = new Question { Id = Guid.NewGuid(), SurveyId = survey.Id, Text = "Довольны ли Вы своим регионом?", Order = 2 };

        var answers1 = new List<Answer>
        {
            new() { Id = Guid.NewGuid(), QuestionId = question1.Id, Text = "Москва" },
            new() { Id = Guid.NewGuid(), QuestionId = question1.Id, Text = "Московская область" },
            new() { Id = Guid.NewGuid(), QuestionId = question1.Id, Text = "Другой регион" }
        };
        var answers2 = new List<Answer>
        {
            new() { Id = Guid.NewGuid(), QuestionId = question2.Id, Text = "Да" },
            new() { Id = Guid.NewGuid(), QuestionId = question2.Id, Text = "Нет" }
        };

        await context.Surveys.AddAsync(survey);
        await context.Questions.AddRangeAsync(question1, question2);
        await context.Answers.AddRangeAsync(answers1);
        await context.Answers.AddRangeAsync(answers2);
        await context.SaveChangesAsync();
    }
}