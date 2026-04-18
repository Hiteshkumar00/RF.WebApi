using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RF.WebApi.Api.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate23543262543 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "EmailSmtpEnableSsl",
                table: "Account",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "EmailSmtpHost",
                table: "Account",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "EmailSmtpPassword",
                table: "Account",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "EmailSmtpPort",
                table: "Account",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "EmailSmtpUsername",
                table: "Account",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "EnableEmail",
                table: "Account",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.UpdateData(
                table: "User",
                keyColumn: "Id",
                keyValue: -1,
                column: "Password",
                value: "$2a$11$skTKQnJZJjyyvxgFPm/1K.bfeyl6pTHe3FeXwkSpzzcWs9GqWIJmq");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "EmailSmtpEnableSsl",
                table: "Account");

            migrationBuilder.DropColumn(
                name: "EmailSmtpHost",
                table: "Account");

            migrationBuilder.DropColumn(
                name: "EmailSmtpPassword",
                table: "Account");

            migrationBuilder.DropColumn(
                name: "EmailSmtpPort",
                table: "Account");

            migrationBuilder.DropColumn(
                name: "EmailSmtpUsername",
                table: "Account");

            migrationBuilder.DropColumn(
                name: "EnableEmail",
                table: "Account");

            migrationBuilder.UpdateData(
                table: "User",
                keyColumn: "Id",
                keyValue: -1,
                column: "Password",
                value: "$2a$11$3nNwjFIgAXkMMcKsh2rONuq0GqXPSuwVSMLRWk5IJxQ3zYgTQfxWG");
        }
    }
}
