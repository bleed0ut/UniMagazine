using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace UniMagazine.Migrations
{
    /// <inheritdoc />
    public partial class fixModelAY_03 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "OpenDate",
                table: "AcademicYears",
                newName: "OpenedDate");

            migrationBuilder.RenameColumn(
                name: "CloseDate",
                table: "AcademicYears",
                newName: "ClosedDate");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "OpenedDate",
                table: "AcademicYears",
                newName: "OpenDate");

            migrationBuilder.RenameColumn(
                name: "ClosedDate",
                table: "AcademicYears",
                newName: "CloseDate");
        }
    }
}
