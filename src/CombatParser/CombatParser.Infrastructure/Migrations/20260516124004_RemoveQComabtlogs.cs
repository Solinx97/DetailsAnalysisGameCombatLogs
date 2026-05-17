using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CombatParser.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class RemoveQComabtlogs : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CombatsInQueue",
                table: "CombatLog");

            migrationBuilder.DropColumn(
                name: "IsReady",
                table: "CombatLog");

            migrationBuilder.DropColumn(
                name: "NumberReadyCombats",
                table: "CombatLog");

            migrationBuilder.CreateIndex(
                name: "IX_Combat_BossId",
                table: "Combat",
                column: "BossId");

            migrationBuilder.AddForeignKey(
                name: "FK_Combat_Boss_BossId",
                table: "Combat",
                column: "BossId",
                principalTable: "Boss",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Combat_Boss_BossId",
                table: "Combat");

            migrationBuilder.DropIndex(
                name: "IX_Combat_BossId",
                table: "Combat");

            migrationBuilder.AddColumn<int>(
                name: "CombatsInQueue",
                table: "CombatLog",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<bool>(
                name: "IsReady",
                table: "CombatLog",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<int>(
                name: "NumberReadyCombats",
                table: "CombatLog",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }
    }
}
