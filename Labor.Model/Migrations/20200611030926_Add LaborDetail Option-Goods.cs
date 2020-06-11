using Microsoft.EntityFrameworkCore.Migrations;

namespace Labor.Model.Migrations
{
    public partial class AddLaborDetailOptionGoods : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Goods",
                table: "LaborDetail",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Option",
                table: "LaborDetail",
                nullable: false,
                defaultValue: "");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Goods",
                table: "LaborDetail");

            migrationBuilder.DropColumn(
                name: "Option",
                table: "LaborDetail");
        }
    }
}
