using Microsoft.EntityFrameworkCore.Migrations;

namespace Shop.Migrations
{
    public partial class iio : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Category",
                table: "Product");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Category",
                table: "Product",
                nullable: true);
        }
    }
}
