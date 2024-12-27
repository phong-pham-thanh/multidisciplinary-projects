using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Back_end.Migrations
{
    public partial class AddSettingColumnToUser : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "autoRunFanWhenOverHeat",
                table: "Users",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "temperatureWarning",
                table: "Users",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "warningWhenOverHeat",
                table: "Users",
                type: "bit",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "autoRunFanWhenOverHeat",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "temperatureWarning",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "warningWhenOverHeat",
                table: "Users");
        }
    }
}
