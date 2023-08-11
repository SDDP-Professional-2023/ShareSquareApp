using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ShareSquare.Migrations
{
    public partial class updateItemModel : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Items_AspNetUsers_ReviewedUserId",
                table: "Items");

            migrationBuilder.RenameColumn(
                name: "ReviewedUserId",
                table: "Items",
                newName: "UserId");

            migrationBuilder.RenameIndex(
                name: "IX_Items_ReviewedUserId",
                table: "Items",
                newName: "IX_Items_UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_Items_AspNetUsers_UserId",
                table: "Items",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Items_AspNetUsers_UserId",
                table: "Items");

            migrationBuilder.RenameColumn(
                name: "UserId",
                table: "Items",
                newName: "ReviewedUserId");

            migrationBuilder.RenameIndex(
                name: "IX_Items_UserId",
                table: "Items",
                newName: "IX_Items_ReviewedUserId");

            migrationBuilder.AddForeignKey(
                name: "FK_Items_AspNetUsers_ReviewedUserId",
                table: "Items",
                column: "ReviewedUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");
        }
    }
}
