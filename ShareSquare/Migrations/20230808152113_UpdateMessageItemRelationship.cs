using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ShareSquare.Migrations
{
    public partial class UpdateMessageItemRelationship : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Messages_Items_ItemId",
                table: "Messages");

            migrationBuilder.DropIndex(
                name: "IX_Messages_ItemId",
                table: "Messages");

            migrationBuilder.DropColumn(
                name: "ItemId",
                table: "Messages");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "ItemId",
                table: "Messages",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Messages_ItemId",
                table: "Messages",
                column: "ItemId");

            migrationBuilder.AddForeignKey(
                name: "FK_Messages_Items_ItemId",
                table: "Messages",
                column: "ItemId",
                principalTable: "Items",
                principalColumn: "ItemId",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
