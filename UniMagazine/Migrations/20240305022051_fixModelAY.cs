using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace UniMagazine.Migrations
{
    /// <inheritdoc />
    public partial class fixModelAY : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "academicYearDate",
                table: "AcademicYears",
                newName: "openDate");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "openDate",
                table: "AcademicYears",
                newName: "academicYearDate");
        }
    }
}
