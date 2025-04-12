using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace VideoCourse.Backend.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class added_unique_details : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_videos_title",
                table: "videos",
                column: "title",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_users_email",
                table: "users",
                column: "email",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_courses_title",
                table: "courses",
                column: "title",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_videos_title",
                table: "videos");

            migrationBuilder.DropIndex(
                name: "IX_users_email",
                table: "users");

            migrationBuilder.DropIndex(
                name: "IX_courses_title",
                table: "courses");
        }
    }
}
