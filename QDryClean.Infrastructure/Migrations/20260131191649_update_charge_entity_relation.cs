using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace QDryClean.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class update_charge_entity_relation : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_ItemTypes_ChargeId",
                table: "ItemTypes");

            migrationBuilder.CreateIndex(
                name: "IX_ItemTypes_ChargeId",
                table: "ItemTypes",
                column: "ChargeId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_ItemTypes_ChargeId",
                table: "ItemTypes");

            migrationBuilder.CreateIndex(
                name: "IX_ItemTypes_ChargeId",
                table: "ItemTypes",
                column: "ChargeId",
                unique: true,
                filter: "[ChargeId] IS NOT NULL");
        }
    }
}
