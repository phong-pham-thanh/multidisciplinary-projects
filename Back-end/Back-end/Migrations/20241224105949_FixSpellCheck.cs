using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Back_end.Migrations
{
    public partial class FixSpellCheck : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "warningWhenOverHeat",
                table: "Users",
                newName: "WarningWhenOverHeat");

            migrationBuilder.RenameColumn(
                name: "temperatureWarning",
                table: "Users",
                newName: "TemperatureWarning");

            migrationBuilder.RenameColumn(
                name: "autoRunFanWhenOverHeat",
                table: "Users",
                newName: "AutoRunFanWhenOverHeat");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "WarningWhenOverHeat",
                table: "Users",
                newName: "warningWhenOverHeat");

            migrationBuilder.RenameColumn(
                name: "TemperatureWarning",
                table: "Users",
                newName: "temperatureWarning");

            migrationBuilder.RenameColumn(
                name: "AutoRunFanWhenOverHeat",
                table: "Users",
                newName: "autoRunFanWhenOverHeat");
        }
    }
}
