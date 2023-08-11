using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ShareSquare.Migrations
{
    public partial class addSoftDeleteToMessage : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "Deleted",
                table: "Messages",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Deleted",
                table: "Messages");
        }
    }
}
