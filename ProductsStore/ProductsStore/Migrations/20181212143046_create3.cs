using Microsoft.EntityFrameworkCore.Migrations;

namespace ProductsStore.Migrations
{
    public partial class create3 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "single",
                table: "User");

            migrationBuilder.RenameColumn(
                name: "numchildren",
                table: "User",
                newName: "isAdmin");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "isAdmin",
                table: "User",
                newName: "numchildren");

            migrationBuilder.AddColumn<bool>(
                name: "single",
                table: "User",
                nullable: false,
                defaultValue: false);
        }
    }
}
