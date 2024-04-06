using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace UniMagazine.Migrations
{
    /// <inheritdoc />
    public partial class Extrafieldsforcontributionandmaterialcontribution : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Status",
                table: "MaterialContributions",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "TempContent",
                table: "Contributions",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Status",
                table: "MaterialContributions");

            migrationBuilder.DropColumn(
                name: "TempContent",
                table: "Contributions");
        }
    }
}
