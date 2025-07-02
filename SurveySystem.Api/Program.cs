using Microsoft.EntityFrameworkCore;
using SurveySystem.Application;
using SurveySystem.Application.Interfaces;
using SurveySystem.Application.Services;
using SurveySystem.Domain.Entities;
using SurveySystem.Infrastructure.Persistence;
using SurveySystem.Infrastructure.Repositories;

var builder = WebApplication.CreateBuilder(args);

var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseNpgsql(connectionString));

builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();

builder.Services.AddScoped<IInterviewRepository, InterviewRepository>();
builder.Services.AddScoped<IQuestionRepository, QuestionRepository>();
builder.Services.AddScoped<IResultRepository, ResultRepository>();

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
            Title = "Test Survey About Regions",
            CreatedAt = DateTime.UtcNow
        };

        var question1 = new Question { Id = Guid.NewGuid(), SurveyId = survey.Id, Text = "Which region do you live in?", Order = 1 };
        var question2 = new Question { Id = Guid.NewGuid(), SurveyId = survey.Id, Text = "Are you satisfied with your region?", Order = 2 };

        var answers1 = new List<Answer>
        {
            new() { Id = Guid.NewGuid(), QuestionId = question1.Id, Text = "Moscow" },
            new() { Id = Guid.NewGuid(), QuestionId = question1.Id, Text = "Moscow Region" },
            new() { Id = Guid.NewGuid(), QuestionId = question1.Id, Text = "Other region" }
        };
        var answers2 = new List<Answer>
        {
            new() { Id = Guid.NewGuid(), QuestionId = question2.Id, Text = "Yes" },
            new() { Id = Guid.NewGuid(), QuestionId = question2.Id, Text = "No" }
        };

        await context.Surveys.AddAsync(survey);
        await context.Questions.AddRangeAsync(question1, question2);
        await context.Answers.AddRangeAsync(answers1);
        await context.Answers.AddRangeAsync(answers2);
        await context.SaveChangesAsync();
    }
}