using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace QDryClean.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class RefactorOrderInvoiceToInvoice : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_OrderInvoices_Orders_OrderId",
                table: "OrderInvoices");

            migrationBuilder.DropForeignKey(
                name: "FK_Payments_OrderInvoices_InvoiceId",
                table: "Payments");

            migrationBuilder.DropPrimaryKey(
                name: "PK_OrderInvoices",
                table: "OrderInvoices");

            migrationBuilder.RenameTable(
                name: "OrderInvoices",
                newName: "Invoices");

            migrationBuilder.RenameIndex(
                name: "IX_OrderInvoices_OrderId",
                table: "Invoices",
                newName: "IX_Invoices_OrderId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Invoices",
                table: "Invoices",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Invoices_Orders_OrderId",
                table: "Invoices",
                column: "OrderId",
                principalTable: "Orders",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Payments_Invoices_InvoiceId",
                table: "Payments",
                column: "InvoiceId",
                principalTable: "Invoices",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Invoices_Orders_OrderId",
                table: "Invoices");

            migrationBuilder.DropForeignKey(
                name: "FK_Payments_Invoices_InvoiceId",
                table: "Payments");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Invoices",
                table: "Invoices");

            migrationBuilder.RenameTable(
                name: "Invoices",
                newName: "OrderInvoices");

            migrationBuilder.RenameIndex(
                name: "IX_Invoices_OrderId",
                table: "OrderInvoices",
                newName: "IX_OrderInvoices_OrderId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_OrderInvoices",
                table: "OrderInvoices",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_OrderInvoices_Orders_OrderId",
                table: "OrderInvoices",
                column: "OrderId",
                principalTable: "Orders",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Payments_OrderInvoices_InvoiceId",
                table: "Payments",
                column: "InvoiceId",
                principalTable: "OrderInvoices",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
