using Microsoft.EntityFrameworkCore.Migrations;

namespace Labor.Model.Migrations
{
    public partial class AddEmpNo : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "EmpNo",
                table: "User",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "EmpNo",
                table: "User");
        }
    }
}
