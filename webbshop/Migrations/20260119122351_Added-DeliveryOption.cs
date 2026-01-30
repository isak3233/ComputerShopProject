using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace webbshop.Migrations
{
    /// <inheritdoc />
    public partial class AddedDeliveryOption : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "DeliveryOptionId",
                table: "PaymentHistories",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "DeliveryOption",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<int>(type: "int", nullable: false),
                    Price = table.Column<decimal>(type: "decimal(18,2)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DeliveryOption", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_PaymentHistories_DeliveryOptionId",
                table: "PaymentHistories",
                column: "DeliveryOptionId");

            migrationBuilder.AddForeignKey(
                name: "FK_PaymentHistories_DeliveryOption_DeliveryOptionId",
                table: "PaymentHistories",
                column: "DeliveryOptionId",
                principalTable: "DeliveryOption",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PaymentHistories_DeliveryOption_DeliveryOptionId",
                table: "PaymentHistories");

            migrationBuilder.DropTable(
                name: "DeliveryOption");

            migrationBuilder.DropIndex(
                name: "IX_PaymentHistories_DeliveryOptionId",
                table: "PaymentHistories");

            migrationBuilder.DropColumn(
                name: "DeliveryOptionId",
                table: "PaymentHistories");
        }
    }
}
