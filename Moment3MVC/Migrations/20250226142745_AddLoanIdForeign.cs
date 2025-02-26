using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Moment3MVC.Migrations
{
    /// <inheritdoc />
    public partial class AddLoanIdForeign : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Id",
                table: "Loans",
                newName: "LoanId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "LoanId",
                table: "Loans",
                newName: "Id");
        }
    }
}
