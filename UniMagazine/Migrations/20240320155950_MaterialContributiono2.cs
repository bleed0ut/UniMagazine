using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace UniMagazine.Migrations
{
    /// <inheritdoc />
    public partial class MaterialContributiono2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
               name: "MaterialContributions",
               columns: table => new
               {
                   Id = table.Column<int>(type: "int", nullable: false)
                       .Annotation("SqlServer:Identity", "1, 1"),
                   ContributionId = table.Column<int>(type: "int", nullable: false),
                   CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                   ImageUrl = table.Column<string>(type: "nvarchar(max)", nullable: true)
               },
               constraints: table =>
               {
                   table.PrimaryKey("PK_MaterialContributions", x => x.Id);
                   table.ForeignKey(
                       name: "FK_MaterialContributions_Contributions_ContributionId",
                       column: x => x.ContributionId,
                       principalTable: "Contributions",
                       principalColumn: "Id",
                       onDelete: ReferentialAction.Cascade);
               });

            migrationBuilder.CreateIndex(
                name: "IX_MaterialContributions_ContributionId",
                table: "MaterialContributions",
                column: "ContributionId");
           
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "MaterialContributions");
        }
    }
}
