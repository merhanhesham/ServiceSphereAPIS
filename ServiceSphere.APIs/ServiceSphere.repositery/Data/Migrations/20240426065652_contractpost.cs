using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ServiceSphere.repositery.Data.Migrations
{
    public partial class contractpost : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "PostContracts",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Terms = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ServiceDetails = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Status = table.Column<int>(type: "int", nullable: false),
                    Price = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    ProposalId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PostContracts", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PostContracts_Proposals_ProposalId",
                        column: x => x.ProposalId,
                        principalTable: "Proposals",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_PostContracts_ProposalId",
                table: "PostContracts",
                column: "ProposalId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "PostContracts");
        }
    }
}
