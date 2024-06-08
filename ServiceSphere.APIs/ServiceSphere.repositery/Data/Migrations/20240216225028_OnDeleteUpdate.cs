using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ServiceSphere.repositery.Data.Migrations
{
    public partial class OnDeleteUpdate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ProjectSubCategory_SubCategories_SubCategoryId",
                table: "ProjectSubCategory");

            migrationBuilder.AddForeignKey(
                name: "FK_ProjectSubCategory_SubCategories_SubCategoryId",
                table: "ProjectSubCategory",
                column: "SubCategoryId",
                principalTable: "SubCategories",
                principalColumn: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ProjectSubCategory_SubCategories_SubCategoryId",
                table: "ProjectSubCategory");

            migrationBuilder.AddForeignKey(
                name: "FK_ProjectSubCategory_SubCategories_SubCategoryId",
                table: "ProjectSubCategory",
                column: "SubCategoryId",
                principalTable: "SubCategories",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
