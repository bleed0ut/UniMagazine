using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace UniMagazine.Migrations
{
    /// <inheritdoc />
    public partial class fixModelAY_02 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "status",
                table: "AcademicYears",
                newName: "Status");

            migrationBuilder.RenameColumn(
                name: "openDate",
                table: "AcademicYears",
                newName: "OpenDate");

            migrationBuilder.RenameColumn(
                name: "closeDate",
                table: "AcademicYears",
                newName: "CloseDate");

            migrationBuilder.RenameColumn(
                name: "createdDate",
                table: "AcademicYears",
                newName: "YearDate");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Status",
                table: "AcademicYears",
                newName: "status");

            migrationBuilder.RenameColumn(
                name: "OpenDate",
                table: "AcademicYears",
                newName: "openDate");

            migrationBuilder.RenameColumn(
                name: "CloseDate",
                table: "AcademicYears",
                newName: "closeDate");

            migrationBuilder.RenameColumn(
                name: "YearDate",
                table: "AcademicYears",
                newName: "createdDate");
        }
    }
}
