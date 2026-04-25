using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RF.WebApi.Api.Migrations
{
    /// <inheritdoc />
    public partial class AddVoiceTypingToAccount : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "EnableVoiceTyping",
                table: "Account",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.UpdateData(
                table: "SystemConfiguration",
                keyColumn: "Id",
                keyValue: 2,
                column: "PropertyValue",
                value: "65vh");

            migrationBuilder.UpdateData(
                table: "User",
                keyColumn: "Id",
                keyValue: -1,
                column: "Password",
                value: "$2a$11$CHZ4Etpgaprj4m5we7L6tOS5.9dHNJ1ePOeDzElV3i9mqTDAhcz8a");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "EnableVoiceTyping",
                table: "Account");

            migrationBuilder.UpdateData(
                table: "SystemConfiguration",
                keyColumn: "Id",
                keyValue: 2,
                column: "PropertyValue",
                value: "425px");

            migrationBuilder.UpdateData(
                table: "User",
                keyColumn: "Id",
                keyValue: -1,
                column: "Password",
                value: "$2a$11$KTUA98WbpKAHHJg5addDDONo8dU.29B4VZ.T14PCFavm/wTANJNn2");
        }
    }
}
