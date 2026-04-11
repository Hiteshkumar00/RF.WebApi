using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RF.WebApi.Api.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate1234 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_SellingBillItem_SellingItemWarrenty_WarrentyId",
                table: "SellingBillItem");

            migrationBuilder.DropIndex(
                name: "IX_SellingItemWarrenty_ItemId",
                table: "SellingItemWarrenty");

            migrationBuilder.DropIndex(
                name: "IX_SellingBillItem_WarrentyId",
                table: "SellingBillItem");

            migrationBuilder.DropColumn(
                name: "WarrentyId",
                table: "SellingBillItem");

            migrationBuilder.UpdateData(
                table: "User",
                keyColumn: "Id",
                keyValue: -1,
                column: "Password",
                value: "$2a$11$YIvTmKr2Y3xT/F.6NeNDC.ach6or650JmXof8wMG/TcNYdHGxRGBW");

            migrationBuilder.CreateIndex(
                name: "IX_SellingItemWarrenty_ItemId",
                table: "SellingItemWarrenty",
                column: "ItemId",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_SellingItemWarrenty_ItemId",
                table: "SellingItemWarrenty");

            migrationBuilder.AddColumn<int>(
                name: "WarrentyId",
                table: "SellingBillItem",
                type: "int",
                nullable: true);

            migrationBuilder.UpdateData(
                table: "User",
                keyColumn: "Id",
                keyValue: -1,
                column: "Password",
                value: "$2a$11$/Hu.ql.8Dfl7TQMDgBnxtu40fGgeMvX3kVv1Oijb/IdQ94.44qFK6");

            migrationBuilder.CreateIndex(
                name: "IX_SellingItemWarrenty_ItemId",
                table: "SellingItemWarrenty",
                column: "ItemId");

            migrationBuilder.CreateIndex(
                name: "IX_SellingBillItem_WarrentyId",
                table: "SellingBillItem",
                column: "WarrentyId");

            migrationBuilder.AddForeignKey(
                name: "FK_SellingBillItem_SellingItemWarrenty_WarrentyId",
                table: "SellingBillItem",
                column: "WarrentyId",
                principalTable: "SellingItemWarrenty",
                principalColumn: "Id");
        }
    }
}
