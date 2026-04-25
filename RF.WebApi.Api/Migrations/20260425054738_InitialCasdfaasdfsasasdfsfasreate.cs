using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RF.WebApi.Api.Migrations
{
    /// <inheritdoc />
    public partial class InitialCasdfaasdfsasasdfsfasreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AddContribution_AccountPerson_AccountPersonId",
                table: "AddContribution");

            migrationBuilder.DropForeignKey(
                name: "FK_Agency_Account_AccountId",
                table: "Agency");

            migrationBuilder.DropForeignKey(
                name: "FK_BusinessExpence_Account_AccountId",
                table: "BusinessExpence");

            migrationBuilder.DropForeignKey(
                name: "FK_BuyingBill_Agency_AgencyId",
                table: "BuyingBill");

            migrationBuilder.DropForeignKey(
                name: "FK_RelatedEntity_Entity_EntityId",
                table: "RelatedEntity");

            migrationBuilder.DropForeignKey(
                name: "FK_RemoveContribution_AccountPerson_AccountPersonId",
                table: "RemoveContribution");

            migrationBuilder.DropForeignKey(
                name: "FK_SellingBill_Account_AccountId",
                table: "SellingBill");

            migrationBuilder.DropForeignKey(
                name: "FK_UserSelectedYearMapping_Account_AccountId",
                table: "UserSelectedYearMapping");

            migrationBuilder.DropForeignKey(
                name: "FK_UserSelectedYearMapping_BusinessYear_BusinessYearId",
                table: "UserSelectedYearMapping");

            migrationBuilder.UpdateData(
                table: "User",
                keyColumn: "Id",
                keyValue: -1,
                column: "Password",
                value: "$2a$11$ZffPtZZ8FeJ16T3wR9paVud6yO7yBS1M3bY8rYxs2MZMbfdqa8ePK");

            migrationBuilder.CreateIndex(
                name: "IX_RemoveContribution_AccountId",
                table: "RemoveContribution",
                column: "AccountId");

            migrationBuilder.CreateIndex(
                name: "IX_AddContribution_AccountId",
                table: "AddContribution",
                column: "AccountId");

            migrationBuilder.AddForeignKey(
                name: "FK_AddContribution_AccountPerson_AccountPersonId",
                table: "AddContribution",
                column: "AccountPersonId",
                principalTable: "AccountPerson",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_AddContribution_Account_AccountId",
                table: "AddContribution",
                column: "AccountId",
                principalTable: "Account",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Agency_Account_AccountId",
                table: "Agency",
                column: "AccountId",
                principalTable: "Account",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_BusinessExpence_Account_AccountId",
                table: "BusinessExpence",
                column: "AccountId",
                principalTable: "Account",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_BuyingBill_Agency_AgencyId",
                table: "BuyingBill",
                column: "AgencyId",
                principalTable: "Agency",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_RelatedEntity_Entity_EntityId",
                table: "RelatedEntity",
                column: "EntityId",
                principalTable: "Entity",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_RemoveContribution_AccountPerson_AccountPersonId",
                table: "RemoveContribution",
                column: "AccountPersonId",
                principalTable: "AccountPerson",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_RemoveContribution_Account_AccountId",
                table: "RemoveContribution",
                column: "AccountId",
                principalTable: "Account",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_SellingBill_Account_AccountId",
                table: "SellingBill",
                column: "AccountId",
                principalTable: "Account",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_UserSelectedYearMapping_Account_AccountId",
                table: "UserSelectedYearMapping",
                column: "AccountId",
                principalTable: "Account",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_UserSelectedYearMapping_BusinessYear_BusinessYearId",
                table: "UserSelectedYearMapping",
                column: "BusinessYearId",
                principalTable: "BusinessYear",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AddContribution_AccountPerson_AccountPersonId",
                table: "AddContribution");

            migrationBuilder.DropForeignKey(
                name: "FK_AddContribution_Account_AccountId",
                table: "AddContribution");

            migrationBuilder.DropForeignKey(
                name: "FK_Agency_Account_AccountId",
                table: "Agency");

            migrationBuilder.DropForeignKey(
                name: "FK_BusinessExpence_Account_AccountId",
                table: "BusinessExpence");

            migrationBuilder.DropForeignKey(
                name: "FK_BuyingBill_Agency_AgencyId",
                table: "BuyingBill");

            migrationBuilder.DropForeignKey(
                name: "FK_RelatedEntity_Entity_EntityId",
                table: "RelatedEntity");

            migrationBuilder.DropForeignKey(
                name: "FK_RemoveContribution_AccountPerson_AccountPersonId",
                table: "RemoveContribution");

            migrationBuilder.DropForeignKey(
                name: "FK_RemoveContribution_Account_AccountId",
                table: "RemoveContribution");

            migrationBuilder.DropForeignKey(
                name: "FK_SellingBill_Account_AccountId",
                table: "SellingBill");

            migrationBuilder.DropForeignKey(
                name: "FK_UserSelectedYearMapping_Account_AccountId",
                table: "UserSelectedYearMapping");

            migrationBuilder.DropForeignKey(
                name: "FK_UserSelectedYearMapping_BusinessYear_BusinessYearId",
                table: "UserSelectedYearMapping");

            migrationBuilder.DropIndex(
                name: "IX_RemoveContribution_AccountId",
                table: "RemoveContribution");

            migrationBuilder.DropIndex(
                name: "IX_AddContribution_AccountId",
                table: "AddContribution");

            migrationBuilder.UpdateData(
                table: "User",
                keyColumn: "Id",
                keyValue: -1,
                column: "Password",
                value: "$2a$11$skTKQnJZJjyyvxgFPm/1K.bfeyl6pTHe3FeXwkSpzzcWs9GqWIJmq");

            migrationBuilder.AddForeignKey(
                name: "FK_AddContribution_AccountPerson_AccountPersonId",
                table: "AddContribution",
                column: "AccountPersonId",
                principalTable: "AccountPerson",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Agency_Account_AccountId",
                table: "Agency",
                column: "AccountId",
                principalTable: "Account",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_BusinessExpence_Account_AccountId",
                table: "BusinessExpence",
                column: "AccountId",
                principalTable: "Account",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_BuyingBill_Agency_AgencyId",
                table: "BuyingBill",
                column: "AgencyId",
                principalTable: "Agency",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_RelatedEntity_Entity_EntityId",
                table: "RelatedEntity",
                column: "EntityId",
                principalTable: "Entity",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_RemoveContribution_AccountPerson_AccountPersonId",
                table: "RemoveContribution",
                column: "AccountPersonId",
                principalTable: "AccountPerson",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_SellingBill_Account_AccountId",
                table: "SellingBill",
                column: "AccountId",
                principalTable: "Account",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_UserSelectedYearMapping_Account_AccountId",
                table: "UserSelectedYearMapping",
                column: "AccountId",
                principalTable: "Account",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_UserSelectedYearMapping_BusinessYear_BusinessYearId",
                table: "UserSelectedYearMapping",
                column: "BusinessYearId",
                principalTable: "BusinessYear",
                principalColumn: "Id");
        }
    }
}
