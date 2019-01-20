using Microsoft.EntityFrameworkCore.Migrations;

namespace ProductsStore.Migrations
{
    public partial class ii1 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<double>(
                name: "lat",
                table: "Address",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "lng",
                table: "Address",
                nullable: false,
                defaultValue: 0.0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "lat",
                table: "Address");

            migrationBuilder.DropColumn(
                name: "lng",
                table: "Address");
        }
    }
}
