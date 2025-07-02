using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace SurveySystem.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Interviews",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    SurveyId = table.Column<Guid>(type: "uuid", nullable: false),
                    StartedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    CompletedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Interviews", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Surveys",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Title = table.Column<string>(type: "text", nullable: false),
                    Description = table.Column<string>(type: "text", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Surveys", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Results",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    InterviewId = table.Column<Guid>(type: "uuid", nullable: false),
                    QuestionId = table.Column<Guid>(type: "uuid", nullable: false),
                    SelectedAnswerId = table.Column<Guid>(type: "uuid", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Results", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Results_Interviews_InterviewId",
                        column: x => x.InterviewId,
                        principalTable: "Interviews",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Questions",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    SurveyId = table.Column<Guid>(type: "uuid", nullable: false),
                    Text = table.Column<string>(type: "text", nullable: false),
                    Order = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Questions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Questions_Surveys_SurveyId",
                        column: x => x.SurveyId,
                        principalTable: "Surveys",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Answers",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    QuestionId = table.Column<Guid>(type: "uuid", nullable: false),
                    Text = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Answers", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Answers_Questions_QuestionId",
                        column: x => x.QuestionId,
                        principalTable: "Questions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "Interviews",
                columns: new[] { "Id", "CompletedAt", "StartedAt", "SurveyId" },
                values: new object[] { new Guid("eeeeeeee-eeee-eeee-eeee-eeeeeeeeeeee"), new DateTime(2024, 3, 1, 10, 5, 0, 0, DateTimeKind.Utc), new DateTime(2024, 3, 1, 10, 0, 0, 0, DateTimeKind.Utc), new Guid("1f9a3e42-1c7b-4b8f-8e4a-3e8a1d7f2b1c") });

            migrationBuilder.InsertData(
                table: "Surveys",
                columns: new[] { "Id", "CreatedAt", "Description", "Title" },
                values: new object[,]
                {
                    { new Guid("1f9a3e42-1c7b-4b8f-8e4a-3e8a1d7f2b1c"), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, "Test Survey About Regions" },
                    { new Guid("c5e8b9f0-3a2d-4b1c-8e6f-0a9d8c7b6a5e"), new DateTime(2024, 2, 1, 0, 0, 0, 0, DateTimeKind.Utc), "A short survey about your job.", "Work Satisfaction Survey" }
                });

            migrationBuilder.InsertData(
                table: "Questions",
                columns: new[] { "Id", "Order", "SurveyId", "Text" },
                values: new object[,]
                {
                    { new Guid("a1b2c3d4-e5f6-7788-9900-aabbccddeeff"), 1, new Guid("1f9a3e42-1c7b-4b8f-8e4a-3e8a1d7f2b1c"), "Which region do you live in?" },
                    { new Guid("b2c3d4e5-f6a7-8899-0011-aabbccddeeff"), 1, new Guid("c5e8b9f0-3a2d-4b1c-8e6f-0a9d8c7b6a5e"), "What is your role?" },
                    { new Guid("d3e4f5a6-b7c8-9900-1122-ccddeeff0011"), 2, new Guid("c5e8b9f0-3a2d-4b1c-8e6f-0a9d8c7b6a5e"), "How would you rate your work-life balance (1 to 5)?" },
                    { new Guid("ffeeddcc-bbaa-0099-8877-f6e5d4c3b2a1"), 2, new Guid("1f9a3e42-1c7b-4b8f-8e4a-3e8a1d7f2b1c"), "Are you satisfied with your region?" }
                });

            migrationBuilder.InsertData(
                table: "Results",
                columns: new[] { "Id", "CreatedAt", "InterviewId", "QuestionId", "SelectedAnswerId" },
                values: new object[,]
                {
                    { new Guid("f1f1f1f1-f1f1-f1f1-f1f1-f1f1f1f1f1f1"), new DateTime(2024, 3, 1, 10, 2, 0, 0, DateTimeKind.Utc), new Guid("eeeeeeee-eeee-eeee-eeee-eeeeeeeeeeee"), new Guid("a1b2c3d4-e5f6-7788-9900-aabbccddeeff"), new Guid("11111111-1111-1111-1111-111111111111") },
                    { new Guid("f2f2f2f2-f2f2-f2f2-f2f2-f2f2f2f2f2f2"), new DateTime(2024, 3, 1, 10, 4, 0, 0, DateTimeKind.Utc), new Guid("eeeeeeee-eeee-eeee-eeee-eeeeeeeeeeee"), new Guid("ffeeddcc-bbaa-0099-8877-f6e5d4c3b2a1"), new Guid("44444444-4444-4444-4444-444444444444") }
                });

            migrationBuilder.InsertData(
                table: "Answers",
                columns: new[] { "Id", "QuestionId", "Text" },
                values: new object[,]
                {
                    { new Guid("11111111-1111-1111-1111-111111111111"), new Guid("a1b2c3d4-e5f6-7788-9900-aabbccddeeff"), "Moscow" },
                    { new Guid("22222222-2222-2222-2222-222222222222"), new Guid("a1b2c3d4-e5f6-7788-9900-aabbccddeeff"), "Moscow Region" },
                    { new Guid("33333333-3333-3333-3333-333333333333"), new Guid("a1b2c3d4-e5f6-7788-9900-aabbccddeeff"), "Other region" },
                    { new Guid("44444444-4444-4444-4444-444444444444"), new Guid("ffeeddcc-bbaa-0099-8877-f6e5d4c3b2a1"), "Yes" },
                    { new Guid("55555555-5555-5555-5555-555555555555"), new Guid("ffeeddcc-bbaa-0099-8877-f6e5d4c3b2a1"), "No" },
                    { new Guid("66666666-6666-6666-6666-666666666666"), new Guid("b2c3d4e5-f6a7-8899-0011-aabbccddeeff"), "Developer" },
                    { new Guid("77777777-7777-7777-7777-777777777777"), new Guid("b2c3d4e5-f6a7-8899-0011-aabbccddeeff"), "QA Engineer" },
                    { new Guid("88888888-8888-8888-8888-888888888888"), new Guid("b2c3d4e5-f6a7-8899-0011-aabbccddeeff"), "Manager" },
                    { new Guid("99999999-9999-9999-9999-999999999999"), new Guid("d3e4f5a6-b7c8-9900-1122-ccddeeff0011"), "1 (Poor)" },
                    { new Guid("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaaaa"), new Guid("d3e4f5a6-b7c8-9900-1122-ccddeeff0011"), "3 (Average)" },
                    { new Guid("bbbbbbbb-bbbb-bbbb-bbbb-bbbbbbbbbbbb"), new Guid("d3e4f5a6-b7c8-9900-1122-ccddeeff0011"), "5 (Excellent)" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_Answers_QuestionId",
                table: "Answers",
                column: "QuestionId");

            migrationBuilder.CreateIndex(
                name: "IX_Questions_SurveyId_Order",
                table: "Questions",
                columns: new[] { "SurveyId", "Order" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Results_InterviewId",
                table: "Results",
                column: "InterviewId");

            migrationBuilder.CreateIndex(
                name: "IX_Results_QuestionId",
                table: "Results",
                column: "QuestionId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Answers");

            migrationBuilder.DropTable(
                name: "Results");

            migrationBuilder.DropTable(
                name: "Questions");

            migrationBuilder.DropTable(
                name: "Interviews");

            migrationBuilder.DropTable(
                name: "Surveys");
        }
    }
}
