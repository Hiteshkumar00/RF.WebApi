using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RF.WebApi.Api.Migrations
{
    public partial class AddCurrenciesToSeed : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "RelatedEntity",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "RelatedDisplayName", "RelatedEntityName" },
                values: new object[] { "USD ($)", "en-US" });

            migrationBuilder.UpdateData(
                table: "RelatedEntity",
                keyColumn: "Id",
                keyValue: 4,
                columns: new[] { "RelatedDisplayName", "RelatedEntityName" },
                values: new object[] { "INR (₹)", "en-IN" });

            migrationBuilder.InsertData(
                table: "RelatedEntity",
                columns: new[] { "Id", "EntityId", "RelatedDisplayName", "RelatedEntityName" },
                values: new object[,]
                {
                    { 5, 2, "GBP (£)", "en-GB" },
                    { 6, 2, "EUR (€)", "en-IE" }
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "RelatedEntity",
                keyColumn: "Id",
                keyValue: 5);

            migrationBuilder.DeleteData(
                table: "RelatedEntity",
                keyColumn: "Id",
                keyValue: 6);

            migrationBuilder.UpdateData(
                table: "RelatedEntity",
                keyColumn: "Id",
                keyValue: 3,
                column: "RelatedEntityName",
                value: "Dollar");
        }
    }
}
