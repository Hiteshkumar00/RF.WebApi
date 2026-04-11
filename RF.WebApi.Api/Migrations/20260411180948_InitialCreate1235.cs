using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RF.WebApi.Api.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate1235 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_BuyingBillExpence_BuyingBill_BuyingBillId",
                table: "BuyingBillExpence");

            migrationBuilder.DropForeignKey(
                name: "FK_BuyingBillItem_BuyingBill_BuyingBillId",
                table: "BuyingBillItem");

            migrationBuilder.DropForeignKey(
                name: "FK_BuyingBillPayment_BuyingBill_BuyingBillId",
                table: "BuyingBillPayment");

            migrationBuilder.DropForeignKey(
                name: "FK_SellingBillPayment_SellingBill_SellingBillId",
                table: "SellingBillPayment");

            migrationBuilder.DropIndex(
                name: "IX_SellingBillPayment_SellingBillId",
                table: "SellingBillPayment");

            migrationBuilder.DropIndex(
                name: "IX_BuyingBillPayment_BuyingBillId",
                table: "BuyingBillPayment");

            migrationBuilder.DropIndex(
                name: "IX_BuyingBillItem_BuyingBillId",
                table: "BuyingBillItem");

            migrationBuilder.DropIndex(
                name: "IX_BuyingBillExpence_BuyingBillId",
                table: "BuyingBillExpence");

            migrationBuilder.DropColumn(
                name: "SellingBillId",
                table: "SellingBillPayment");

            migrationBuilder.DropColumn(
                name: "BuyingBillId",
                table: "BuyingBillPayment");

            migrationBuilder.DropColumn(
                name: "BuyingBillId",
                table: "BuyingBillItem");

            migrationBuilder.DropColumn(
                name: "BuyingBillId",
                table: "BuyingBillExpence");

            migrationBuilder.UpdateData(
                table: "User",
                keyColumn: "Id",
                keyValue: -1,
                column: "Password",
                value: "$2a$11$FYNX0A6r7pyp1P..KEQVqeg6cA1WjR6QTCs0.MDRg27jjAjRBvcL6");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "SellingBillId",
                table: "SellingBillPayment",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "BuyingBillId",
                table: "BuyingBillPayment",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "BuyingBillId",
                table: "BuyingBillItem",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "BuyingBillId",
                table: "BuyingBillExpence",
                type: "int",
                nullable: true);

            migrationBuilder.UpdateData(
                table: "User",
                keyColumn: "Id",
                keyValue: -1,
                column: "Password",
                value: "$2a$11$YIvTmKr2Y3xT/F.6NeNDC.ach6or650JmXof8wMG/TcNYdHGxRGBW");

            migrationBuilder.CreateIndex(
                name: "IX_SellingBillPayment_SellingBillId",
                table: "SellingBillPayment",
                column: "SellingBillId");

            migrationBuilder.CreateIndex(
                name: "IX_BuyingBillPayment_BuyingBillId",
                table: "BuyingBillPayment",
                column: "BuyingBillId");

            migrationBuilder.CreateIndex(
                name: "IX_BuyingBillItem_BuyingBillId",
                table: "BuyingBillItem",
                column: "BuyingBillId");

            migrationBuilder.CreateIndex(
                name: "IX_BuyingBillExpence_BuyingBillId",
                table: "BuyingBillExpence",
                column: "BuyingBillId");

            migrationBuilder.AddForeignKey(
                name: "FK_BuyingBillExpence_BuyingBill_BuyingBillId",
                table: "BuyingBillExpence",
                column: "BuyingBillId",
                principalTable: "BuyingBill",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_BuyingBillItem_BuyingBill_BuyingBillId",
                table: "BuyingBillItem",
                column: "BuyingBillId",
                principalTable: "BuyingBill",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_BuyingBillPayment_BuyingBill_BuyingBillId",
                table: "BuyingBillPayment",
                column: "BuyingBillId",
                principalTable: "BuyingBill",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_SellingBillPayment_SellingBill_SellingBillId",
                table: "SellingBillPayment",
                column: "SellingBillId",
                principalTable: "SellingBill",
                principalColumn: "Id");
        }
    }
}
