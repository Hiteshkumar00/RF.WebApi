using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RF.WebApi.Api.Migrations
{
    /// <inheritdoc />
    public partial class AddTotalAmountAndBuyingBillIdToBusinessExpence : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "BuyingBillId",
                table: "BusinessExpence",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "TotalAmount",
                table: "BusinessExpence",
                type: "decimal(18,2)",
                precision: 18,
                scale: 2,
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.UpdateData(
                table: "User",
                keyColumn: "Id",
                keyValue: -1,
                column: "Password",
                value: "$2a$11$eZzzdrc8DtUSe1kxEzjnI.0LmYHErrCk8RfN0PP6/WMtgAwDtoVJq");

            migrationBuilder.CreateIndex(
                name: "IX_BusinessExpence_BuyingBillId",
                table: "BusinessExpence",
                column: "BuyingBillId");

            migrationBuilder.AddForeignKey(
                name: "FK_BusinessExpence_BuyingBill_BuyingBillId",
                table: "BusinessExpence",
                column: "BuyingBillId",
                principalTable: "BuyingBill",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_BusinessExpence_BuyingBill_BuyingBillId",
                table: "BusinessExpence");

            migrationBuilder.DropIndex(
                name: "IX_BusinessExpence_BuyingBillId",
                table: "BusinessExpence");

            migrationBuilder.DropColumn(
                name: "BuyingBillId",
                table: "BusinessExpence");

            migrationBuilder.DropColumn(
                name: "TotalAmount",
                table: "BusinessExpence");

            migrationBuilder.UpdateData(
                table: "User",
                keyColumn: "Id",
                keyValue: -1,
                column: "Password",
                value: "$2a$11$CHZ4Etpgaprj4m5we7L6tOS5.9dHNJ1ePOeDzElV3i9mqTDAhcz8a");
        }
    }
}
