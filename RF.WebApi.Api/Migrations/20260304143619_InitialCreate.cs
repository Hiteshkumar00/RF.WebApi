using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RF.WebApi.Api.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Account",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ProfileName = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: false),
                    ProfileLogoLink = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CurrencyType = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false, defaultValue: "INR")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Account", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Entity",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    EntityName = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: false),
                    DisplayName = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Entity", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AccountPerson",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    AccountId = table.Column<int>(type: "int", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: false),
                    PhoneNo = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    Email = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: true),
                    PersonOccupation = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: true),
                    Address = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AccountPerson", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AccountPerson_Account_AccountId",
                        column: x => x.AccountId,
                        principalTable: "Account",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Agency",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    AccountId = table.Column<int>(type: "int", nullable: false),
                    AgencyName = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: false),
                    Address = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Agency", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Agency_Account_AccountId",
                        column: x => x.AccountId,
                        principalTable: "Account",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "BusinessExpence",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    AccountId = table.Column<int>(type: "int", nullable: false),
                    ExpenceType = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: true),
                    Date = table.Column<DateOnly>(type: "date", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BusinessExpence", x => x.Id);
                    table.ForeignKey(
                        name: "FK_BusinessExpence_Account_AccountId",
                        column: x => x.AccountId,
                        principalTable: "Account",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "BusinessYear",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    AccountId = table.Column<int>(type: "int", nullable: false),
                    YearName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Date = table.Column<DateOnly>(type: "date", maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BusinessYear", x => x.Id);
                    table.ForeignKey(
                        name: "FK_BusinessYear_Account_AccountId",
                        column: x => x.AccountId,
                        principalTable: "Account",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "SellingBill",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    AccountId = table.Column<int>(type: "int", nullable: false),
                    BillNo = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    CustomerName = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: false),
                    Email = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: true),
                    PhoneNo = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Address = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Date = table.Column<DateOnly>(type: "date", nullable: false),
                    Discount = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SellingBill", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SellingBill_Account_AccountId",
                        column: x => x.AccountId,
                        principalTable: "Account",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "User",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    AccountId = table.Column<int>(type: "int", nullable: true),
                    FirstName = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: false),
                    MiddleName = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: true),
                    Surname = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: false),
                    Email = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: false),
                    PhoneNo = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    Role = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Password = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false, defaultValue: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_User", x => x.Id);
                    table.ForeignKey(
                        name: "FK_User_Account_AccountId",
                        column: x => x.AccountId,
                        principalTable: "Account",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                });

            migrationBuilder.CreateTable(
                name: "RelatedEntity",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RelatedEntityName = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: false),
                    RelatedDisplayName = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: true),
                    EntityId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RelatedEntity", x => x.Id);
                    table.ForeignKey(
                        name: "FK_RelatedEntity_Entity_EntityId",
                        column: x => x.EntityId,
                        principalTable: "Entity",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "AddContribution",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    AccountPersonId = table.Column<int>(type: "int", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    Date = table.Column<DateOnly>(type: "date", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AddContribution", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AddContribution_AccountPerson_AccountPersonId",
                        column: x => x.AccountPersonId,
                        principalTable: "AccountPerson",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "PaymentAccount",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    MethodName = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: false),
                    AccountId = table.Column<int>(type: "int", nullable: false),
                    AccountPersonId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PaymentAccount", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PaymentAccount_AccountPerson_AccountPersonId",
                        column: x => x.AccountPersonId,
                        principalTable: "AccountPerson",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_PaymentAccount_Account_AccountId",
                        column: x => x.AccountId,
                        principalTable: "Account",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "RemoveContribution",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    AccountPersonId = table.Column<int>(type: "int", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    Date = table.Column<DateOnly>(type: "date", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RemoveContribution", x => x.Id);
                    table.ForeignKey(
                        name: "FK_RemoveContribution_AccountPerson_AccountPersonId",
                        column: x => x.AccountPersonId,
                        principalTable: "AccountPerson",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AgencyPerson",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    AgencyId = table.Column<int>(type: "int", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: false),
                    PhoneNo = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    Email = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: true),
                    PersonOccupation = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: true),
                    Address = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AgencyPerson", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AgencyPerson_Agency_AgencyId",
                        column: x => x.AgencyId,
                        principalTable: "Agency",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "BuyingBill",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    AccountId = table.Column<int>(type: "int", nullable: false),
                    AgencyId = table.Column<int>(type: "int", nullable: false),
                    BillNo = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Date = table.Column<DateOnly>(type: "date", nullable: false),
                    Discount = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BuyingBill", x => x.Id);
                    table.ForeignKey(
                        name: "FK_BuyingBill_Account_AccountId",
                        column: x => x.AccountId,
                        principalTable: "Account",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_BuyingBill_Agency_AgencyId",
                        column: x => x.AgencyId,
                        principalTable: "Agency",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "SellingBillItem",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ItemName = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    Quntity = table.Column<int>(type: "int", nullable: false),
                    Price = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: true),
                    BillId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SellingBillItem", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SellingBillItem_SellingBill_BillId",
                        column: x => x.BillId,
                        principalTable: "SellingBill",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "UserSelectedYearMapping",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<int>(type: "int", nullable: false),
                    AccountId = table.Column<int>(type: "int", nullable: false),
                    BusinessYearId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserSelectedYearMapping", x => x.Id);
                    table.ForeignKey(
                        name: "FK_UserSelectedYearMapping_Account_AccountId",
                        column: x => x.AccountId,
                        principalTable: "Account",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_UserSelectedYearMapping_BusinessYear_BusinessYearId",
                        column: x => x.BusinessYearId,
                        principalTable: "BusinessYear",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_UserSelectedYearMapping_User_UserId",
                        column: x => x.UserId,
                        principalTable: "User",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AddContributionPayment",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Amount = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: true),
                    AddContributionId = table.Column<int>(type: "int", nullable: false),
                    PaymentAccountId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AddContributionPayment", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AddContributionPayment_AddContribution_AddContributionId",
                        column: x => x.AddContributionId,
                        principalTable: "AddContribution",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AddContributionPayment_PaymentAccount_PaymentAccountId",
                        column: x => x.PaymentAccountId,
                        principalTable: "PaymentAccount",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "BusinessExpencePayment",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Amount = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: true),
                    BusinessExpenceId = table.Column<int>(type: "int", nullable: false),
                    PaymentAccountId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BusinessExpencePayment", x => x.Id);
                    table.ForeignKey(
                        name: "FK_BusinessExpencePayment_BusinessExpence_BusinessExpenceId",
                        column: x => x.BusinessExpenceId,
                        principalTable: "BusinessExpence",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_BusinessExpencePayment_PaymentAccount_PaymentAccountId",
                        column: x => x.PaymentAccountId,
                        principalTable: "PaymentAccount",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "SellingBillPayment",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Amount = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: true),
                    BillId = table.Column<int>(type: "int", nullable: false),
                    PaymentAccountId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SellingBillPayment", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SellingBillPayment_PaymentAccount_PaymentAccountId",
                        column: x => x.PaymentAccountId,
                        principalTable: "PaymentAccount",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_SellingBillPayment_SellingBill_BillId",
                        column: x => x.BillId,
                        principalTable: "SellingBill",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "RemoveContributionPayment",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Amount = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: true),
                    RemoveContributionId = table.Column<int>(type: "int", nullable: false),
                    PaymentAccountId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RemoveContributionPayment", x => x.Id);
                    table.ForeignKey(
                        name: "FK_RemoveContributionPayment_PaymentAccount_PaymentAccountId",
                        column: x => x.PaymentAccountId,
                        principalTable: "PaymentAccount",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_RemoveContributionPayment_RemoveContribution_RemoveContributionId",
                        column: x => x.RemoveContributionId,
                        principalTable: "RemoveContribution",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "BuyingBillExpence",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Amount = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: true),
                    BillId = table.Column<int>(type: "int", nullable: false),
                    PaymentAccountId = table.Column<int>(type: "int", nullable: false),
                    ExpenceType = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BuyingBillExpence", x => x.Id);
                    table.ForeignKey(
                        name: "FK_BuyingBillExpence_BuyingBill_BillId",
                        column: x => x.BillId,
                        principalTable: "BuyingBill",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_BuyingBillExpence_PaymentAccount_PaymentAccountId",
                        column: x => x.PaymentAccountId,
                        principalTable: "PaymentAccount",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "BuyingBillItem",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ItemName = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    Quntity = table.Column<int>(type: "int", nullable: true),
                    Price = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: true),
                    BillId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BuyingBillItem", x => x.Id);
                    table.ForeignKey(
                        name: "FK_BuyingBillItem_BuyingBill_BillId",
                        column: x => x.BillId,
                        principalTable: "BuyingBill",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "BuyingBillPayment",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Amount = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: true),
                    BillId = table.Column<int>(type: "int", nullable: false),
                    PaymentAccountId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BuyingBillPayment", x => x.Id);
                    table.ForeignKey(
                        name: "FK_BuyingBillPayment_BuyingBill_BillId",
                        column: x => x.BillId,
                        principalTable: "BuyingBill",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_BuyingBillPayment_PaymentAccount_PaymentAccountId",
                        column: x => x.PaymentAccountId,
                        principalTable: "PaymentAccount",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "SellingItemWarrenty",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ItemId = table.Column<int>(type: "int", nullable: false),
                    BillId = table.Column<int>(type: "int", nullable: false),
                    Year = table.Column<int>(type: "int", nullable: true),
                    Month = table.Column<int>(type: "int", nullable: true),
                    Day = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SellingItemWarrenty", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SellingItemWarrenty_SellingBillItem_ItemId",
                        column: x => x.ItemId,
                        principalTable: "SellingBillItem",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_SellingItemWarrenty_SellingBill_BillId",
                        column: x => x.BillId,
                        principalTable: "SellingBill",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "BuyingBillExpencePayment",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Amount = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: true),
                    BuyingBillExpenceId = table.Column<int>(type: "int", nullable: false),
                    PaymentAccountId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BuyingBillExpencePayment", x => x.Id);
                    table.ForeignKey(
                        name: "FK_BuyingBillExpencePayment_BuyingBillExpence_BuyingBillExpenceId",
                        column: x => x.BuyingBillExpenceId,
                        principalTable: "BuyingBillExpence",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_BuyingBillExpencePayment_PaymentAccount_PaymentAccountId",
                        column: x => x.PaymentAccountId,
                        principalTable: "PaymentAccount",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.InsertData(
                table: "User",
                columns: new[] { "Id", "AccountId", "Email", "FirstName", "MiddleName", "Password", "PhoneNo", "Role", "Surname" },
                values: new object[] { -1, null, "hiteshkumar252020@gmail.com", "System", null, "$2a$11$kZAWToKm.Cs0A59eSpW6AOMOH8sWPWoYeDgzhgmicCvmfZa.f7P7e", null, "SuperAdmin", "User" });

            migrationBuilder.CreateIndex(
                name: "IX_AccountPerson_AccountId",
                table: "AccountPerson",
                column: "AccountId");

            migrationBuilder.CreateIndex(
                name: "IX_AddContribution_AccountPersonId",
                table: "AddContribution",
                column: "AccountPersonId");

            migrationBuilder.CreateIndex(
                name: "IX_AddContributionPayment_AddContributionId",
                table: "AddContributionPayment",
                column: "AddContributionId");

            migrationBuilder.CreateIndex(
                name: "IX_AddContributionPayment_PaymentAccountId",
                table: "AddContributionPayment",
                column: "PaymentAccountId");

            migrationBuilder.CreateIndex(
                name: "IX_Agency_AccountId",
                table: "Agency",
                column: "AccountId");

            migrationBuilder.CreateIndex(
                name: "IX_AgencyPerson_AgencyId",
                table: "AgencyPerson",
                column: "AgencyId");

            migrationBuilder.CreateIndex(
                name: "IX_BusinessExpence_AccountId",
                table: "BusinessExpence",
                column: "AccountId");

            migrationBuilder.CreateIndex(
                name: "IX_BusinessExpencePayment_BusinessExpenceId",
                table: "BusinessExpencePayment",
                column: "BusinessExpenceId");

            migrationBuilder.CreateIndex(
                name: "IX_BusinessExpencePayment_PaymentAccountId",
                table: "BusinessExpencePayment",
                column: "PaymentAccountId");

            migrationBuilder.CreateIndex(
                name: "IX_BusinessYear_AccountId",
                table: "BusinessYear",
                column: "AccountId");

            migrationBuilder.CreateIndex(
                name: "IX_BuyingBill_AccountId",
                table: "BuyingBill",
                column: "AccountId");

            migrationBuilder.CreateIndex(
                name: "IX_BuyingBill_AgencyId",
                table: "BuyingBill",
                column: "AgencyId");

            migrationBuilder.CreateIndex(
                name: "IX_BuyingBillExpence_BillId",
                table: "BuyingBillExpence",
                column: "BillId");

            migrationBuilder.CreateIndex(
                name: "IX_BuyingBillExpence_PaymentAccountId",
                table: "BuyingBillExpence",
                column: "PaymentAccountId");

            migrationBuilder.CreateIndex(
                name: "IX_BuyingBillExpencePayment_BuyingBillExpenceId",
                table: "BuyingBillExpencePayment",
                column: "BuyingBillExpenceId");

            migrationBuilder.CreateIndex(
                name: "IX_BuyingBillExpencePayment_PaymentAccountId",
                table: "BuyingBillExpencePayment",
                column: "PaymentAccountId");

            migrationBuilder.CreateIndex(
                name: "IX_BuyingBillItem_BillId",
                table: "BuyingBillItem",
                column: "BillId");

            migrationBuilder.CreateIndex(
                name: "IX_BuyingBillPayment_BillId",
                table: "BuyingBillPayment",
                column: "BillId");

            migrationBuilder.CreateIndex(
                name: "IX_BuyingBillPayment_PaymentAccountId",
                table: "BuyingBillPayment",
                column: "PaymentAccountId");

            migrationBuilder.CreateIndex(
                name: "IX_Entity_EntityName",
                table: "Entity",
                column: "EntityName",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_PaymentAccount_AccountId",
                table: "PaymentAccount",
                column: "AccountId");

            migrationBuilder.CreateIndex(
                name: "IX_PaymentAccount_AccountPersonId",
                table: "PaymentAccount",
                column: "AccountPersonId");

            migrationBuilder.CreateIndex(
                name: "IX_RelatedEntity_EntityId",
                table: "RelatedEntity",
                column: "EntityId");

            migrationBuilder.CreateIndex(
                name: "IX_RemoveContribution_AccountPersonId",
                table: "RemoveContribution",
                column: "AccountPersonId");

            migrationBuilder.CreateIndex(
                name: "IX_RemoveContributionPayment_PaymentAccountId",
                table: "RemoveContributionPayment",
                column: "PaymentAccountId");

            migrationBuilder.CreateIndex(
                name: "IX_RemoveContributionPayment_RemoveContributionId",
                table: "RemoveContributionPayment",
                column: "RemoveContributionId");

            migrationBuilder.CreateIndex(
                name: "IX_SellingBill_AccountId",
                table: "SellingBill",
                column: "AccountId");

            migrationBuilder.CreateIndex(
                name: "IX_SellingBillItem_BillId",
                table: "SellingBillItem",
                column: "BillId");

            migrationBuilder.CreateIndex(
                name: "IX_SellingBillPayment_BillId",
                table: "SellingBillPayment",
                column: "BillId");

            migrationBuilder.CreateIndex(
                name: "IX_SellingBillPayment_PaymentAccountId",
                table: "SellingBillPayment",
                column: "PaymentAccountId");

            migrationBuilder.CreateIndex(
                name: "IX_SellingItemWarrenty_BillId",
                table: "SellingItemWarrenty",
                column: "BillId");

            migrationBuilder.CreateIndex(
                name: "IX_SellingItemWarrenty_ItemId",
                table: "SellingItemWarrenty",
                column: "ItemId");

            migrationBuilder.CreateIndex(
                name: "IX_User_AccountId",
                table: "User",
                column: "AccountId");

            migrationBuilder.CreateIndex(
                name: "IX_User_Email",
                table: "User",
                column: "Email",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_UserSelectedYearMapping_AccountId",
                table: "UserSelectedYearMapping",
                column: "AccountId");

            migrationBuilder.CreateIndex(
                name: "IX_UserSelectedYearMapping_BusinessYearId",
                table: "UserSelectedYearMapping",
                column: "BusinessYearId");

            migrationBuilder.CreateIndex(
                name: "IX_UserSelectedYearMapping_UserId",
                table: "UserSelectedYearMapping",
                column: "UserId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AddContributionPayment");

            migrationBuilder.DropTable(
                name: "AgencyPerson");

            migrationBuilder.DropTable(
                name: "BusinessExpencePayment");

            migrationBuilder.DropTable(
                name: "BuyingBillExpencePayment");

            migrationBuilder.DropTable(
                name: "BuyingBillItem");

            migrationBuilder.DropTable(
                name: "BuyingBillPayment");

            migrationBuilder.DropTable(
                name: "RelatedEntity");

            migrationBuilder.DropTable(
                name: "RemoveContributionPayment");

            migrationBuilder.DropTable(
                name: "SellingBillPayment");

            migrationBuilder.DropTable(
                name: "SellingItemWarrenty");

            migrationBuilder.DropTable(
                name: "UserSelectedYearMapping");

            migrationBuilder.DropTable(
                name: "AddContribution");

            migrationBuilder.DropTable(
                name: "BusinessExpence");

            migrationBuilder.DropTable(
                name: "BuyingBillExpence");

            migrationBuilder.DropTable(
                name: "Entity");

            migrationBuilder.DropTable(
                name: "RemoveContribution");

            migrationBuilder.DropTable(
                name: "SellingBillItem");

            migrationBuilder.DropTable(
                name: "BusinessYear");

            migrationBuilder.DropTable(
                name: "User");

            migrationBuilder.DropTable(
                name: "BuyingBill");

            migrationBuilder.DropTable(
                name: "PaymentAccount");

            migrationBuilder.DropTable(
                name: "SellingBill");

            migrationBuilder.DropTable(
                name: "Agency");

            migrationBuilder.DropTable(
                name: "AccountPerson");

            migrationBuilder.DropTable(
                name: "Account");
        }
    }
}
