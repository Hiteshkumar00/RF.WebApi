using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RF.WebApi.Api.Migrations
{
    /// <inheritdoc />
    public partial class InitialCasdfaasdfsasasdfasdfasfsfasreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "User",
                keyColumn: "Id",
                keyValue: -1,
                column: "Password",
                value: "$2a$11$X4MpA1Exzh.FdXqFl8BWRutH.NRYTPy3c8nxSmhysG2QxASvDrQAy");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "User",
                keyColumn: "Id",
                keyValue: -1,
                column: "Password",
                value: "$2a$11$ZffPtZZ8FeJ16T3wR9paVud6yO7yBS1M3bY8rYxs2MZMbfdqa8ePK");
        }
    }
}
