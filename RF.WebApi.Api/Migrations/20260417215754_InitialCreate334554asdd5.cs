using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RF.WebApi.Api.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate334554asdd5 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "EnableAdvancedWhatsApp",
                table: "Account",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "WhatsAppAccessToken",
                table: "Account",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "WhatsAppBusinessId",
                table: "Account",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "WhatsAppPhoneNumberId",
                table: "Account",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.UpdateData(
                table: "User",
                keyColumn: "Id",
                keyValue: -1,
                column: "Password",
                value: "$2a$11$3nNwjFIgAXkMMcKsh2rONuq0GqXPSuwVSMLRWk5IJxQ3zYgTQfxWG");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "EnableAdvancedWhatsApp",
                table: "Account");

            migrationBuilder.DropColumn(
                name: "WhatsAppAccessToken",
                table: "Account");

            migrationBuilder.DropColumn(
                name: "WhatsAppBusinessId",
                table: "Account");

            migrationBuilder.DropColumn(
                name: "WhatsAppPhoneNumberId",
                table: "Account");

            migrationBuilder.UpdateData(
                table: "User",
                keyColumn: "Id",
                keyValue: -1,
                column: "Password",
                value: "$2a$11$9fIMxnfZLQviOzSRtSNMH.BYG9H6/3IDc7wiBRR2RtksYZ5ST4pQ2");
        }
    }
}
