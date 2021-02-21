using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace FitAndFresh.Data.Migrations
{
    public partial class AddOrderInformationAndOrderProcessingInformationToDatabase : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "OrderProcessingInformation",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<string>(nullable: false),
                    DateOfOrder = table.Column<DateTime>(nullable: false),
                    OrderTotal = table.Column<double>(nullable: false),
                    CollectionTime = table.Column<DateTime>(nullable: false),
                    StatusOfPayment = table.Column<string>(nullable: false),
                    Status = table.Column<string>(nullable: false),
                    CollectionName = table.Column<string>(nullable: false),
                    PhoneNumber = table.Column<string>(nullable: false),
                    OrderNumber = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OrderProcessingInformation", x => x.Id);
                    table.ForeignKey(
                        name: "FK_OrderProcessingInformation_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "OrderInformation",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    OrderProcessingId = table.Column<int>(nullable: false),
                    ItemInMenuId = table.Column<int>(nullable: false),
                    Quantity = table.Column<int>(nullable: false),
                    ItemName = table.Column<string>(nullable: false),
                    ItemPrice = table.Column<double>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OrderInformation", x => x.Id);
                    table.ForeignKey(
                        name: "FK_OrderInformation_ItemInMenu_ItemInMenuId",
                        column: x => x.ItemInMenuId,
                        principalTable: "ItemInMenu",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_OrderInformation_OrderProcessingInformation_OrderProcessingId",
                        column: x => x.OrderProcessingId,
                        principalTable: "OrderProcessingInformation",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_OrderInformation_ItemInMenuId",
                table: "OrderInformation",
                column: "ItemInMenuId");

            migrationBuilder.CreateIndex(
                name: "IX_OrderInformation_OrderProcessingId",
                table: "OrderInformation",
                column: "OrderProcessingId");

            migrationBuilder.CreateIndex(
                name: "IX_OrderProcessingInformation_UserId",
                table: "OrderProcessingInformation",
                column: "UserId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "OrderInformation");

            migrationBuilder.DropTable(
                name: "OrderProcessingInformation");
        }
    }
}
