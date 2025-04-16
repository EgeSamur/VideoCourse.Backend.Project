using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace VideoCourse.Backend.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class namesChangedv2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_course_course_sections_course_id_course_section_id",
                table: "course_course_sections");

            migrationBuilder.DropIndex(
                name: "IX_course_course_sections_course_id_order_index",
                table: "course_course_sections");

            migrationBuilder.CreateIndex(
                name: "IX_course_course_sections_course_id",
                table: "course_course_sections",
                column: "course_id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_course_course_sections_course_id",
                table: "course_course_sections");

            migrationBuilder.CreateIndex(
                name: "IX_course_course_sections_course_id_course_section_id",
                table: "course_course_sections",
                columns: new[] { "course_id", "course_section_id" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_course_course_sections_course_id_order_index",
                table: "course_course_sections",
                columns: new[] { "course_id", "order_index" },
                unique: true);
        }
    }
}
