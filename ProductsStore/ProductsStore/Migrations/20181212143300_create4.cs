using Microsoft.EntityFrameworkCore.Migrations;

namespace ProductsStore.Migrations
{
    public partial class create4 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<bool>(
                name: "isAdmin",
                table: "User",
                nullable: false,
                oldClrType: typeof(int));
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<int>(
                name: "isAdmin",
                table: "User",
                nullable: false,
                oldClrType: typeof(bool));
        }
    }
}
