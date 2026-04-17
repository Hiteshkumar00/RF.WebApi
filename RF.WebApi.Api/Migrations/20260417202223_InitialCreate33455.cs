using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace RF.WebApi.Api.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate33455 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "RelatedEntity",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "RelatedDisplayName", "RelatedEntityName" },
                values: new object[] { "INR (₹)", "en-IN" });

            migrationBuilder.UpdateData(
                table: "RelatedEntity",
                keyColumn: "Id",
                keyValue: 4,
                columns: new[] { "RelatedDisplayName", "RelatedEntityName" },
                values: new object[] { "USD ($)", "en-US" });

            migrationBuilder.InsertData(
                table: "RelatedEntity",
                columns: new[] { "Id", "EntityId", "RelatedDisplayName", "RelatedEntityName" },
                values: new object[,]
                {
                    { 5, 2, "GBP (£)", "en-GB" },
                    { 6, 2, "EUR (€)", "en-IE" }
                });

            migrationBuilder.UpdateData(
                table: "User",
                keyColumn: "Id",
                keyValue: -1,
                column: "Password",
                value: "$2a$11$3o4o2qyamZ8zexfzEqpdw.0DRDLokze6MGE7ZFMUgxTR6mmvKyQAO");
        }

        /// <inheritdoc />
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
                columns: new[] { "RelatedDisplayName", "RelatedEntityName" },
                values: new object[] { "$", "Dollar" });

            migrationBuilder.UpdateData(
                table: "RelatedEntity",
                keyColumn: "Id",
                keyValue: 4,
                columns: new[] { "RelatedDisplayName", "RelatedEntityName" },
                values: new object[] { "₹", "INR" });

            migrationBuilder.UpdateData(
                table: "User",
                keyColumn: "Id",
                keyValue: -1,
                column: "Password",
                value: "$2a$11$br1X6b3m8593M11CxyC3seVpPwb9.C.vAnJb4mR8cC2XVv4v/kc.q");
        }
    }
}
