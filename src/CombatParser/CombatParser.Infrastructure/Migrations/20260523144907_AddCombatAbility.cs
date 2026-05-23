using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace CombatParser.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddCombatAbility : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "CombatAbility",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    GameId = table.Column<int>(type: "int", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    AbilityType = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CombatAbility", x => x.Id);
                });

            migrationBuilder.InsertData(
                table: "CombatAbility",
                columns: new[] { "Id", "AbilityType", "GameId", "Name" },
                values: new object[,]
                {
                    { 1, 1, 105702, "Зелье Нефритовой Змеи" },
                    { 2, 1, 105697, "Укус гну-синя" },
                    { 3, 1, 105706, "Зелье силы могу" },
                    { 4, 1, 125282, "Бодрящая кафа" },
                    { 5, 0, 105696, "Настой кусачих морозов" },
                    { 6, 0, 105689, "Настой весенних цветов" },
                    { 7, 0, 105691, "Настой ласкового солнца" },
                    { 8, 9, 104277, "Сытость" },
                    { 9, 7, 80353, "Искажение времени" },
                    { 10, 7, 2825, "Жажда крови" },
                    { 11, 7, 114207, "Знамя с черепом" },
                    { 12, 7, 120676, "Тотем порыва бури" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CombatAbility");
        }
    }
}
