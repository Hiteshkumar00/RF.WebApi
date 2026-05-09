using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RF.WebApi.Api.Migrations
{
    /// <inheritdoc />
    public partial class RemoveBuyingBillExpenceTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "BuyingBillExpence");

            migrationBuilder.UpdateData(
                table: "User",
                keyColumn: "Id",
                keyValue: -1,
                column: "Password",
                value: "$2a$11$I6Az0ZSN76nXb6S//5PPxeBHN.ErMYV6D7oq4AU5NXNIDzL/SCZxu");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "BuyingBillExpence",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Amount = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: true),
                    BillId = table.Column<int>(type: "int", nullable: false),
                    ExpenceType = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: true),
                    PaymentAccountId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BuyingBillExpence", x => x.Id);
                    table.ForeignKey(
                        name: "FK_BuyingBillExpence_BuyingBill_BillId",
                        column: x => x.BillId,
                        principalTable: "BuyingBill",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_BuyingBillExpence_PaymentAccount_PaymentAccountId",
                        column: x => x.PaymentAccountId,
                        principalTable: "PaymentAccount",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.UpdateData(
                table: "User",
                keyColumn: "Id",
                keyValue: -1,
                column: "Password",
                value: "$2a$11$eZzzdrc8DtUSe1kxEzjnI.0LmYHErrCk8RfN0PP6/WMtgAwDtoVJq");

            migrationBuilder.CreateIndex(
                name: "IX_BuyingBillExpence_BillId",
                table: "BuyingBillExpence",
                column: "BillId");

            migrationBuilder.CreateIndex(
                name: "IX_BuyingBillExpence_PaymentAccountId",
                table: "BuyingBillExpence",
                column: "PaymentAccountId");
        }
    }
}
