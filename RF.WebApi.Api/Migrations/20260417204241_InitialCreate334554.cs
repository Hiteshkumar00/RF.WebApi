using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RF.WebApi.Api.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate334554 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "EnableWhatsApp",
                table: "Account",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "WhatsAppNumber",
                table: "Account",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: true);

            migrationBuilder.UpdateData(
                table: "User",
                keyColumn: "Id",
                keyValue: -1,
                column: "Password",
                value: "$2a$11$9fIMxnfZLQviOzSRtSNMH.BYG9H6/3IDc7wiBRR2RtksYZ5ST4pQ2");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "EnableWhatsApp",
                table: "Account");

            migrationBuilder.DropColumn(
                name: "WhatsAppNumber",
                table: "Account");

            migrationBuilder.UpdateData(
                table: "User",
                keyColumn: "Id",
                keyValue: -1,
                column: "Password",
                value: "$2a$11$3o4o2qyamZ8zexfzEqpdw.0DRDLokze6MGE7ZFMUgxTR6mmvKyQAO");
        }
    }
}
