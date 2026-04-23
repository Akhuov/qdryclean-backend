using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace QDryClean.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class RemovePaymentIdDuplication : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Payment_OrderInvoices_InvoiceId",
                table: "Payment");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Payment",
                table: "Payment");

            migrationBuilder.DropColumn(
                name: "PaidAmount",
                table: "OrderInvoices");

            migrationBuilder.RenameTable(
                name: "Payment",
                newName: "Payments");

            migrationBuilder.RenameIndex(
                name: "IX_Payment_InvoiceId",
                table: "Payments",
                newName: "IX_Payments_InvoiceId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Payments",
                table: "Payments",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Payments_OrderInvoices_InvoiceId",
                table: "Payments",
                column: "InvoiceId",
                principalTable: "OrderInvoices",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Payments_OrderInvoices_InvoiceId",
                table: "Payments");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Payments",
                table: "Payments");

            migrationBuilder.RenameTable(
                name: "Payments",
                newName: "Payment");

            migrationBuilder.RenameIndex(
                name: "IX_Payments_InvoiceId",
                table: "Payment",
                newName: "IX_Payment_InvoiceId");

            migrationBuilder.AddColumn<decimal>(
                name: "PaidAmount",
                table: "OrderInvoices",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddPrimaryKey(
                name: "PK_Payment",
                table: "Payment",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Payment_OrderInvoices_InvoiceId",
                table: "Payment",
                column: "InvoiceId",
                principalTable: "OrderInvoices",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
