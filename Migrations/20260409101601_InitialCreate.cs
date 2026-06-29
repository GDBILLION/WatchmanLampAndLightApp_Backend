using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WatchmanDevotional.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Devotionals",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Title = table.Column<string>(type: "text", nullable: false),
                    ScriptureReference = table.Column<string>(type: "text", nullable: false),
                    MemoryVerse = table.Column<string>(type: "text", nullable: false),
                    Content = table.Column<string>(type: "text", nullable: false),
                    YoutubeVideoId = table.Column<string>(type: "text", nullable: true),
                    PublishDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Devotionals", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "QuizEntries",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    FullName = table.Column<string>(type: "text", nullable: false),
                    PhoneNumber = table.Column<string>(type: "text", nullable: false),
                    Answer = table.Column<string>(type: "text", nullable: false),
                    SubmittedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    IsCorrect = table.Column<bool>(type: "boolean", nullable: false),
                    WasSelelectedAsWinner = table.Column<bool>(type: "boolean", nullable: false),
                    WinningDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_QuizEntries", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_QuizEntries_PhoneNumber_SubmittedAt",
                table: "QuizEntries",
                columns: new[] { "PhoneNumber", "SubmittedAt" },
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Devotionals");

            migrationBuilder.DropTable(
                name: "QuizEntries");
        }
    }
}
