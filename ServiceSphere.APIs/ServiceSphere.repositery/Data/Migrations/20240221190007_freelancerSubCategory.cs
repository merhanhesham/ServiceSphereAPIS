using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ServiceSphere.repositery.Data.Migrations
{
    public partial class freelancerSubCategory : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "FreelancerSubCategory",
                columns: table => new
                {
                    FreelancersId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    SubCategoriesId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FreelancerSubCategory", x => new { x.FreelancersId, x.SubCategoriesId });
                    table.ForeignKey(
                        name: "FK_FreelancerSubCategory_AppUser_FreelancersId",
                        column: x => x.FreelancersId,
                        principalTable: "AppUser",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_FreelancerSubCategory_SubCategories_SubCategoriesId",
                        column: x => x.SubCategoriesId,
                        principalTable: "SubCategories",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_FreelancerSubCategory_SubCategoriesId",
                table: "FreelancerSubCategory",
                column: "SubCategoriesId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "FreelancerSubCategory");
        }
    }
}
