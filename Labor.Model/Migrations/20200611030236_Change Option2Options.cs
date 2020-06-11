using Microsoft.EntityFrameworkCore.Migrations;

namespace Labor.Model.Migrations
{
    public partial class ChangeOption2Options : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Option",
                table: "LaborHead");

            migrationBuilder.AddColumn<string>(
                name: "Options",
                table: "LaborHead",
                nullable: false,
                defaultValue: "");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Options",
                table: "LaborHead");

            migrationBuilder.AddColumn<string>(
                name: "Option",
                table: "LaborHead",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }
    }
}
