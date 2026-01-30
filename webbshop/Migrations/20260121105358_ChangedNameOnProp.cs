using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace webbshop.Migrations
{
    /// <inheritdoc />
    public partial class ChangedNameOnProp : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PaymentHistories_DeliveryOption_DeliveryOptionId",
                table: "PaymentHistories");

            migrationBuilder.DropPrimaryKey(
                name: "PK_DeliveryOption",
                table: "DeliveryOption");

            migrationBuilder.RenameTable(
                name: "DeliveryOption",
                newName: "DeliveryOptions");

            migrationBuilder.AddPrimaryKey(
                name: "PK_DeliveryOptions",
                table: "DeliveryOptions",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_PaymentHistories_DeliveryOptions_DeliveryOptionId",
                table: "PaymentHistories",
                column: "DeliveryOptionId",
                principalTable: "DeliveryOptions",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PaymentHistories_DeliveryOptions_DeliveryOptionId",
                table: "PaymentHistories");

            migrationBuilder.DropPrimaryKey(
                name: "PK_DeliveryOptions",
                table: "DeliveryOptions");

            migrationBuilder.RenameTable(
                name: "DeliveryOptions",
                newName: "DeliveryOption");

            migrationBuilder.AddPrimaryKey(
                name: "PK_DeliveryOption",
                table: "DeliveryOption",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_PaymentHistories_DeliveryOption_DeliveryOptionId",
                table: "PaymentHistories",
                column: "DeliveryOptionId",
                principalTable: "DeliveryOption",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
