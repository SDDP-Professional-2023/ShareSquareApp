using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ShareSquare.Migrations
{
    public partial class updateEntities : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Items_AspNetUsers_ApplicationUserId",
                table: "Items");

            migrationBuilder.DropColumn(
                name: "ReviewerId",
                table: "Reviews");

            migrationBuilder.DropColumn(
                name: "Rating",
                table: "Items");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "Items");

            migrationBuilder.RenameColumn(
                name: "ApplicationUserId",
                table: "Items",
                newName: "ReviewedUserId");

            migrationBuilder.RenameIndex(
                name: "IX_Items_ApplicationUserId",
                table: "Items",
                newName: "IX_Items_ReviewedUserId");

            migrationBuilder.AddForeignKey(
                name: "FK_Items_AspNetUsers_ReviewedUserId",
                table: "Items",
                column: "ReviewedUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Items_AspNetUsers_ReviewedUserId",
                table: "Items");

            migrationBuilder.RenameColumn(
                name: "ReviewedUserId",
                table: "Items",
                newName: "ApplicationUserId");

            migrationBuilder.RenameIndex(
                name: "IX_Items_ReviewedUserId",
                table: "Items",
                newName: "IX_Items_ApplicationUserId");

            migrationBuilder.AddColumn<string>(
                name: "ReviewerId",
                table: "Reviews",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<decimal>(
                name: "Rating",
                table: "Items",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<string>(
                name: "UserId",
                table: "Items",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddForeignKey(
                name: "FK_Items_AspNetUsers_ApplicationUserId",
                table: "Items",
                column: "ApplicationUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");
        }
    }
}
