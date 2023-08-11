using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ShareSquare.Migrations
{
    public partial class AddRatingToUserNew : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Reviews_AspNetUsers_ReviewedUserId",
                table: "Reviews");

            migrationBuilder.DropIndex(
                name: "IX_Reviews_ReviewedUserId",
                table: "Reviews");

            migrationBuilder.AlterColumn<string>(
                name: "ReviewedUserId",
                table: "Reviews",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(450)",
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ReviewerUserId",
                table: "Reviews",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Reviews_ReviewerUserId",
                table: "Reviews",
                column: "ReviewerUserId");

            migrationBuilder.AddForeignKey(
                name: "FK_Reviews_AspNetUsers_ReviewerUserId",
                table: "Reviews",
                column: "ReviewerUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Reviews_AspNetUsers_ReviewerUserId",
                table: "Reviews");

            migrationBuilder.DropIndex(
                name: "IX_Reviews_ReviewerUserId",
                table: "Reviews");

            migrationBuilder.DropColumn(
                name: "ReviewerUserId",
                table: "Reviews");

            migrationBuilder.AlterColumn<string>(
                name: "ReviewedUserId",
                table: "Reviews",
                type: "nvarchar(450)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.CreateIndex(
                name: "IX_Reviews_ReviewedUserId",
                table: "Reviews",
                column: "ReviewedUserId");

            migrationBuilder.AddForeignKey(
                name: "FK_Reviews_AspNetUsers_ReviewedUserId",
                table: "Reviews",
                column: "ReviewedUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");
        }
    }
}
