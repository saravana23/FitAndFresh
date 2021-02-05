using Microsoft.EntityFrameworkCore.Migrations;

namespace FitAndFresh.Data.Migrations
{
    public partial class AddItemMenuToDb : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ItemInMenu",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ItemName = table.Column<string>(nullable: false),
                    ItemDescription = table.Column<string>(nullable: true),
                    ProteinContent = table.Column<string>(nullable: true),
                    ItemPrice = table.Column<double>(nullable: false),
                    ItemPicture = table.Column<string>(nullable: true),
                    CatId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ItemInMenu", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ItemInMenu_Category_CatId",
                        column: x => x.CatId,
                        principalTable: "Category",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.NoAction);
                        //onDelete: ReferentialAction.Cascade);
        });

            migrationBuilder.CreateIndex(
                name: "IX_ItemInMenu_CatId",
                table: "ItemInMenu",
                column: "CatId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ItemInMenu");
        }
    }
}
