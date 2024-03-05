using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace UniMagazine.Migrations
{
    /// <inheritdoc />
    public partial class fixModelAY_01 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "status",
                table: "AcademicYears",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "status",
                table: "AcademicYears");
        }
    }
}
