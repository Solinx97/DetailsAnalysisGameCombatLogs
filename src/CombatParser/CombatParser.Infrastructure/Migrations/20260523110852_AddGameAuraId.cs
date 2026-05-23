using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CombatParser.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddGameAuraId : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "GameAuraId",
                table: "CombatAura",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "GameAuraId",
                table: "CombatAura");
        }
    }
}
