using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace FitAndFresh.Data.Migrations
{
    public partial class AddOrderInfoAndOrderProcessingInfoToDatabase : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "OrderProcessingInfo",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<string>(nullable: false),
                    DateOfOrder = table.Column<DateTime>(nullable: false),
                    OrderTotal = table.Column<double>(nullable: false),
                    CollectionTime = table.Column<DateTime>(nullable: false),
                    CollectionName = table.Column<string>(nullable: false),
                    PhoneNumber = table.Column<string>(nullable: false),
                    OrderNumber = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OrderProcessingInfo", x => x.Id);
                    table.ForeignKey(
                        name: "FK_OrderProcessingInfo_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "OrderInfo",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    OrderProcessingId = table.Column<int>(nullable: false),
                    ItemInMenuId = table.Column<int>(nullable: false),
                    Quantity = table.Column<int>(nullable: false),
                    ItemName = table.Column<int>(nullable: false),
                    ItemPrice = table.Column<double>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OrderInfo", x => x.Id);
                    table.ForeignKey(
                        name: "FK_OrderInfo_ItemInMenu_ItemInMenuId",
                        column: x => x.ItemInMenuId,
                        principalTable: "ItemInMenu",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_OrderInfo_OrderProcessingInfo_OrderProcessingId",
                        column: x => x.OrderProcessingId,
                        principalTable: "OrderProcessingInfo",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_OrderInfo_ItemInMenuId",
                table: "OrderInfo",
                column: "ItemInMenuId");

            migrationBuilder.CreateIndex(
                name: "IX_OrderInfo_OrderProcessingId",
                table: "OrderInfo",
                column: "OrderProcessingId");

            migrationBuilder.CreateIndex(
                name: "IX_OrderProcessingInfo_UserId",
                table: "OrderProcessingInfo",
                column: "UserId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "OrderInfo");

            migrationBuilder.DropTable(
                name: "OrderProcessingInfo");
        }
    }
}
