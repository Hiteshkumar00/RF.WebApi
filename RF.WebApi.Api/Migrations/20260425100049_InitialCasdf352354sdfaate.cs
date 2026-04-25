using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RF.WebApi.Api.Migrations
{
    /// <inheritdoc />
    public partial class InitialCasdf352354sdfaate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "SystemConfiguration",
                columns: new[] { "Id", "PropertyDisplayName", "PropertyName", "PropertyType", "PropertyValue" },
                values: new object[] { 2, "Table Scroll Height", "TableScrollHeight", "string", "425px" });

            migrationBuilder.UpdateData(
                table: "User",
                keyColumn: "Id",
                keyValue: -1,
                column: "Password",
                value: "$2a$11$KTUA98WbpKAHHJg5addDDONo8dU.29B4VZ.T14PCFavm/wTANJNn2");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "SystemConfiguration",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.UpdateData(
                table: "User",
                keyColumn: "Id",
                keyValue: -1,
                column: "Password",
                value: "$2a$11$xW2.WIDfctwGnsWkt6DOWeI59iu6zeNMOmjnFCKAbed4T8ml0kgl2");
        }
    }
}
