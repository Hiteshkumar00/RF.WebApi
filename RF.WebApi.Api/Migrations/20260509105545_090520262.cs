using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RF.WebApi.Api.Migrations
{
    /// <inheritdoc />
    public partial class _090520262 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateOnly>(
                name: "Date",
                table: "SellingBillPayment",
                type: "date",
                nullable: true);

            migrationBuilder.AddColumn<DateOnly>(
                name: "Date",
                table: "RemoveContributionPayment",
                type: "date",
                nullable: true);

            migrationBuilder.AddColumn<DateOnly>(
                name: "Date",
                table: "BuyingBillPayment",
                type: "date",
                nullable: true);

            migrationBuilder.AddColumn<DateOnly>(
                name: "Date",
                table: "BusinessExpencePayment",
                type: "date",
                nullable: true);

            migrationBuilder.AddColumn<DateOnly>(
                name: "Date",
                table: "AddContributionPayment",
                type: "date",
                nullable: true);

            migrationBuilder.UpdateData(
                table: "User",
                keyColumn: "Id",
                keyValue: -1,
                column: "Password",
                value: "$2a$11$UXJ8FeNxG1SJ6gnnkF6lKeAVdYuKXlR2UdZkmMPF0m02H0wOd8XLS");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Date",
                table: "SellingBillPayment");

            migrationBuilder.DropColumn(
                name: "Date",
                table: "RemoveContributionPayment");

            migrationBuilder.DropColumn(
                name: "Date",
                table: "BuyingBillPayment");

            migrationBuilder.DropColumn(
                name: "Date",
                table: "BusinessExpencePayment");

            migrationBuilder.DropColumn(
                name: "Date",
                table: "AddContributionPayment");

            migrationBuilder.UpdateData(
                table: "User",
                keyColumn: "Id",
                keyValue: -1,
                column: "Password",
                value: "$2a$11$iLlcol8e7X6aPkCreK6XCO7t9RDfoBJ6kngrhb07qJwCedRf3wHI6");
        }
    }
}
