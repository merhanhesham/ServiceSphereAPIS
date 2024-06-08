using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ServiceSphere.repositery.Data.Migrations
{
    public partial class ProposalUpdate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ProjectSubCategory_SubCategories_SubCategoryId",
                table: "ProjectSubCategory");

            migrationBuilder.AddColumn<string>(
                name: "Availability",
                table: "Proposals",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "CoverLetter",
                table: "Proposals",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Inquiries",
                table: "Proposals",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Milestones",
                table: "Proposals",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "WorkPlan",
                table: "Proposals",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddForeignKey(
                name: "FK_ProjectSubCategory_SubCategories_SubCategoryId",
                table: "ProjectSubCategory",
                column: "SubCategoryId",
                principalTable: "SubCategories",
                principalColumn: "Id",
                onDelete: ReferentialAction.NoAction);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ProjectSubCategory_SubCategories_SubCategoryId",
                table: "ProjectSubCategory");

            migrationBuilder.DropColumn(
                name: "Availability",
                table: "Proposals");

            migrationBuilder.DropColumn(
                name: "CoverLetter",
                table: "Proposals");

            migrationBuilder.DropColumn(
                name: "Inquiries",
                table: "Proposals");

            migrationBuilder.DropColumn(
                name: "Milestones",
                table: "Proposals");

            migrationBuilder.DropColumn(
                name: "WorkPlan",
                table: "Proposals");

            migrationBuilder.AddForeignKey(
                name: "FK_ProjectSubCategory_SubCategories_SubCategoryId",
                table: "ProjectSubCategory",
                column: "SubCategoryId",
                principalTable: "SubCategories",
                principalColumn: "Id");
        }
    }
}
