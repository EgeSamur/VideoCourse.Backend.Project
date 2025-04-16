using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace VideoCourse.Backend.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class namesChanged : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "course_section_relations");

            migrationBuilder.DropTable(
                name: "section_videos");

            migrationBuilder.CreateTable(
                name: "course_course_sections",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    course_id = table.Column<int>(type: "integer", nullable: false),
                    course_section_id = table.Column<int>(type: "integer", nullable: false),
                    order_index = table.Column<int>(type: "integer", nullable: false),
                    is_deleted = table.Column<bool>(type: "boolean", nullable: false),
                    created_date = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    updated_date = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true),
                    created_user_id = table.Column<int>(type: "integer", nullable: true),
                    updated_user_id = table.Column<int>(type: "integer", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_course_course_sections", x => x.id);
                    table.ForeignKey(
                        name: "FK_course_course_sections_course_sections_course_section_id",
                        column: x => x.course_section_id,
                        principalTable: "course_sections",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_course_course_sections_courses_course_id",
                        column: x => x.course_id,
                        principalTable: "courses",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "course_section_videos",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    course_section_id = table.Column<int>(type: "integer", nullable: false),
                    video_id = table.Column<int>(type: "integer", nullable: false),
                    order_index = table.Column<int>(type: "integer", nullable: false),
                    is_deleted = table.Column<bool>(type: "boolean", nullable: false),
                    created_date = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    updated_date = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true),
                    created_user_id = table.Column<int>(type: "integer", nullable: true),
                    updated_user_id = table.Column<int>(type: "integer", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_course_section_videos", x => x.id);
                    table.ForeignKey(
                        name: "FK_course_section_videos_course_sections_course_section_id",
                        column: x => x.course_section_id,
                        principalTable: "course_sections",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_course_section_videos_videos_video_id",
                        column: x => x.video_id,
                        principalTable: "videos",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

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

            migrationBuilder.CreateIndex(
                name: "IX_course_course_sections_course_section_id",
                table: "course_course_sections",
                column: "course_section_id");

            migrationBuilder.CreateIndex(
                name: "IX_course_section_videos_course_section_id",
                table: "course_section_videos",
                column: "course_section_id");

            migrationBuilder.CreateIndex(
                name: "IX_course_section_videos_video_id",
                table: "course_section_videos",
                column: "video_id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "course_course_sections");

            migrationBuilder.DropTable(
                name: "course_section_videos");

            migrationBuilder.CreateTable(
                name: "course_section_relations",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    course_id = table.Column<int>(type: "integer", nullable: false),
                    section_id = table.Column<int>(type: "integer", nullable: false),
                    created_date = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    created_user_id = table.Column<int>(type: "integer", nullable: true),
                    is_deleted = table.Column<bool>(type: "boolean", nullable: false),
                    order_index = table.Column<int>(type: "integer", nullable: false),
                    updated_date = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true),
                    updated_user_id = table.Column<int>(type: "integer", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_course_section_relations", x => x.id);
                    table.ForeignKey(
                        name: "FK_course_section_relations_course_sections_section_id",
                        column: x => x.section_id,
                        principalTable: "course_sections",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_course_section_relations_courses_course_id",
                        column: x => x.course_id,
                        principalTable: "courses",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "section_videos",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    course_section_id = table.Column<int>(type: "integer", nullable: false),
                    video_id = table.Column<int>(type: "integer", nullable: false),
                    created_date = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    created_user_id = table.Column<int>(type: "integer", nullable: true),
                    is_deleted = table.Column<bool>(type: "boolean", nullable: false),
                    order_index = table.Column<int>(type: "integer", nullable: false),
                    updated_date = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true),
                    updated_user_id = table.Column<int>(type: "integer", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_section_videos", x => x.id);
                    table.ForeignKey(
                        name: "FK_section_videos_course_sections_course_section_id",
                        column: x => x.course_section_id,
                        principalTable: "course_sections",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_section_videos_videos_video_id",
                        column: x => x.video_id,
                        principalTable: "videos",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_course_section_relations_course_id_order_index",
                table: "course_section_relations",
                columns: new[] { "course_id", "order_index" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_course_section_relations_course_id_section_id",
                table: "course_section_relations",
                columns: new[] { "course_id", "section_id" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_course_section_relations_section_id",
                table: "course_section_relations",
                column: "section_id");

            migrationBuilder.CreateIndex(
                name: "IX_section_videos_course_section_id",
                table: "section_videos",
                column: "course_section_id");

            migrationBuilder.CreateIndex(
                name: "IX_section_videos_video_id",
                table: "section_videos",
                column: "video_id");
        }
    }
}
