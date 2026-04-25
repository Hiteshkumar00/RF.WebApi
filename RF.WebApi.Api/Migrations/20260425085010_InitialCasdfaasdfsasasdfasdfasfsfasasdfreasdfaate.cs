using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RF.WebApi.Api.Migrations
{
    /// <inheritdoc />
    public partial class InitialCasdfaasdfsasasdfasdfasfsfasasdfreasdfaate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "User",
                keyColumn: "Id",
                keyValue: -1,
                column: "Password",
                value: "$2a$11$xW2.WIDfctwGnsWkt6DOWeI59iu6zeNMOmjnFCKAbed4T8ml0kgl2");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "User",
                keyColumn: "Id",
                keyValue: -1,
                column: "Password",
                value: "$2a$11$bFNtUehM.YJz1hEOaRf61uEZDHWYa3GwppVgSq6FSbXBQn3VxWvjy");
        }
    }
}
