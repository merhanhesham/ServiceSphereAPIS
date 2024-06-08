using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ServiceSphere.repositery.Data.Migrations
{
    public partial class ProjectPostingEndpointUpdate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ProjectPostingSubCategory");

            migrationBuilder.AddColumn<int>(
                name: "SubCategoryId",
                table: "ProjectPostings",
                type: "int",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "ProjectSubCategory",
                columns: table => new
                {
                    ProjectPostingId = table.Column<int>(type: "int", nullable: false),
                    SubCategoryId = table.Column<int>(type: "int", nullable: false),
                    TeamMembersRequired = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProjectSubCategory", x => new { x.ProjectPostingId, x.SubCategoryId });
                    table.ForeignKey(
                        name: "FK_ProjectSubCategory_ProjectPostings_ProjectPostingId",
                        column: x => x.ProjectPostingId,
                        principalTable: "ProjectPostings",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ProjectSubCategory_SubCategories_SubCategoryId",
                        column: x => x.SubCategoryId,
                        principalTable: "SubCategories",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.NoAction);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ProjectPostings_SubCategoryId",
                table: "ProjectPostings",
                column: "SubCategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_ProjectSubCategory_SubCategoryId",
                table: "ProjectSubCategory",
                column: "SubCategoryId");

            migrationBuilder.AddForeignKey(
                name: "FK_ProjectPostings_SubCategories_SubCategoryId",
                table: "ProjectPostings",
                column: "SubCategoryId",
                principalTable: "SubCategories",
                principalColumn: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ProjectPostings_SubCategories_SubCategoryId",
                table: "ProjectPostings");

            migrationBuilder.DropTable(
                name: "ProjectSubCategory");

            migrationBuilder.DropIndex(
                name: "IX_ProjectPostings_SubCategoryId",
                table: "ProjectPostings");

            migrationBuilder.DropColumn(
                name: "SubCategoryId",
                table: "ProjectPostings");

            migrationBuilder.CreateTable(
                name: "ProjectPostingSubCategory",
                columns: table => new
                {
                    ProjectPostingsId = table.Column<int>(type: "int", nullable: false),
                    SubCategoriesId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProjectPostingSubCategory", x => new { x.ProjectPostingsId, x.SubCategoriesId });
                    table.ForeignKey(
                        name: "FK_ProjectPostingSubCategory_ProjectPostings_ProjectPostingsId",
                        column: x => x.ProjectPostingsId,
                        principalTable: "ProjectPostings",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ProjectPostingSubCategory_SubCategories_SubCategoriesId",
                        column: x => x.SubCategoriesId,
                        principalTable: "SubCategories",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.NoAction);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ProjectPostingSubCategory_SubCategoriesId",
                table: "ProjectPostingSubCategory",
                column: "SubCategoriesId");
        }
    }
}
