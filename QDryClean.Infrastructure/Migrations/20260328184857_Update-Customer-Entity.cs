using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace QDryClean.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class UpdateCustomerEntity : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "LastName",
                table: "Customers");

            migrationBuilder.RenameColumn(
                name: "FirstName",
                table: "Customers",
                newName: "FullName");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "FullName",
                table: "Customers",
                newName: "FirstName");

            migrationBuilder.AddColumn<string>(
                name: "LastName",
                table: "Customers",
                type: "nvarchar(max)",
                nullable: true);
        }
    }
}
