using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace CombatParser.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class RemoveCombatStatus : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsReady",
                table: "Combat");

            migrationBuilder.UpdateData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 14,
                columns: new[] { "BossId", "SpecializationId" },
                values: new object[] { 1, 14 });

            migrationBuilder.UpdateData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 15,
                columns: new[] { "BossId", "SpecializationId" },
                values: new object[] { 1, 15 });

            migrationBuilder.UpdateData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 16,
                columns: new[] { "BossId", "SpecializationId" },
                values: new object[] { 1, 16 });

            migrationBuilder.UpdateData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 17,
                columns: new[] { "BossId", "SpecializationId" },
                values: new object[] { 1, 17 });

            migrationBuilder.UpdateData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 18,
                column: "SpecializationId",
                value: 1);

            migrationBuilder.UpdateData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 19,
                column: "SpecializationId",
                value: 2);

            migrationBuilder.UpdateData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 20,
                column: "SpecializationId",
                value: 3);

            migrationBuilder.UpdateData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 21,
                column: "SpecializationId",
                value: 4);

            migrationBuilder.UpdateData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 22,
                column: "SpecializationId",
                value: 5);

            migrationBuilder.UpdateData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 23,
                column: "SpecializationId",
                value: 6);

            migrationBuilder.UpdateData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 24,
                column: "SpecializationId",
                value: 7);

            migrationBuilder.UpdateData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 25,
                column: "SpecializationId",
                value: 8);

            migrationBuilder.UpdateData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 26,
                column: "SpecializationId",
                value: 9);

            migrationBuilder.UpdateData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 27,
                columns: new[] { "BossId", "SpecializationId" },
                values: new object[] { 2, 10 });

            migrationBuilder.UpdateData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 28,
                columns: new[] { "BossId", "SpecializationId" },
                values: new object[] { 2, 11 });

            migrationBuilder.UpdateData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 29,
                columns: new[] { "BossId", "SpecializationId" },
                values: new object[] { 2, 12 });

            migrationBuilder.UpdateData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 30,
                columns: new[] { "BossId", "SpecializationId" },
                values: new object[] { 2, 13 });

            migrationBuilder.UpdateData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 31,
                columns: new[] { "BossId", "SpecializationId" },
                values: new object[] { 2, 14 });

            migrationBuilder.UpdateData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 32,
                columns: new[] { "BossId", "SpecializationId" },
                values: new object[] { 2, 15 });

            migrationBuilder.UpdateData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 33,
                columns: new[] { "BossId", "SpecializationId" },
                values: new object[] { 2, 16 });

            migrationBuilder.UpdateData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 34,
                columns: new[] { "BossId", "SpecializationId" },
                values: new object[] { 2, 17 });

            migrationBuilder.UpdateData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 35,
                column: "SpecializationId",
                value: 1);

            migrationBuilder.UpdateData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 36,
                column: "SpecializationId",
                value: 2);

            migrationBuilder.UpdateData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 37,
                column: "SpecializationId",
                value: 3);

            migrationBuilder.UpdateData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 38,
                column: "SpecializationId",
                value: 4);

            migrationBuilder.UpdateData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 39,
                column: "SpecializationId",
                value: 5);

            migrationBuilder.UpdateData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 40,
                columns: new[] { "BossId", "SpecializationId" },
                values: new object[] { 3, 6 });

            migrationBuilder.UpdateData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 41,
                columns: new[] { "BossId", "SpecializationId" },
                values: new object[] { 3, 7 });

            migrationBuilder.UpdateData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 42,
                columns: new[] { "BossId", "SpecializationId" },
                values: new object[] { 3, 8 });

            migrationBuilder.UpdateData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 43,
                columns: new[] { "BossId", "SpecializationId" },
                values: new object[] { 3, 9 });

            migrationBuilder.UpdateData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 44,
                columns: new[] { "BossId", "SpecializationId" },
                values: new object[] { 3, 10 });

            migrationBuilder.UpdateData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 45,
                columns: new[] { "BossId", "SpecializationId" },
                values: new object[] { 3, 11 });

            migrationBuilder.UpdateData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 46,
                columns: new[] { "BossId", "SpecializationId" },
                values: new object[] { 3, 12 });

            migrationBuilder.UpdateData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 47,
                columns: new[] { "BossId", "SpecializationId" },
                values: new object[] { 3, 13 });

            migrationBuilder.UpdateData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 48,
                columns: new[] { "BossId", "SpecializationId" },
                values: new object[] { 3, 14 });

            migrationBuilder.UpdateData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 49,
                columns: new[] { "BossId", "SpecializationId" },
                values: new object[] { 3, 15 });

            migrationBuilder.UpdateData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 50,
                columns: new[] { "BossId", "SpecializationId" },
                values: new object[] { 3, 16 });

            migrationBuilder.UpdateData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 51,
                columns: new[] { "BossId", "SpecializationId" },
                values: new object[] { 3, 17 });

            migrationBuilder.UpdateData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 52,
                column: "SpecializationId",
                value: 1);

            migrationBuilder.UpdateData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 53,
                columns: new[] { "BossId", "SpecializationId" },
                values: new object[] { 4, 2 });

            migrationBuilder.UpdateData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 54,
                columns: new[] { "BossId", "SpecializationId" },
                values: new object[] { 4, 3 });

            migrationBuilder.UpdateData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 55,
                columns: new[] { "BossId", "SpecializationId" },
                values: new object[] { 4, 4 });

            migrationBuilder.UpdateData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 56,
                columns: new[] { "BossId", "SpecializationId" },
                values: new object[] { 4, 5 });

            migrationBuilder.UpdateData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 57,
                columns: new[] { "BossId", "SpecializationId" },
                values: new object[] { 4, 6 });

            migrationBuilder.UpdateData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 58,
                columns: new[] { "BossId", "SpecializationId" },
                values: new object[] { 4, 7 });

            migrationBuilder.UpdateData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 59,
                columns: new[] { "BossId", "SpecializationId" },
                values: new object[] { 4, 8 });

            migrationBuilder.UpdateData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 60,
                columns: new[] { "BossId", "SpecializationId" },
                values: new object[] { 4, 9 });

            migrationBuilder.UpdateData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 61,
                columns: new[] { "BossId", "SpecializationId" },
                values: new object[] { 4, 10 });

            migrationBuilder.UpdateData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 62,
                columns: new[] { "BossId", "SpecializationId" },
                values: new object[] { 4, 11 });

            migrationBuilder.UpdateData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 63,
                columns: new[] { "BossId", "SpecializationId" },
                values: new object[] { 4, 12 });

            migrationBuilder.UpdateData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 64,
                columns: new[] { "BossId", "SpecializationId" },
                values: new object[] { 4, 13 });

            migrationBuilder.UpdateData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 65,
                columns: new[] { "BossId", "SpecializationId" },
                values: new object[] { 4, 14 });

            migrationBuilder.UpdateData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 66,
                columns: new[] { "BossId", "SpecializationId" },
                values: new object[] { 4, 15 });

            migrationBuilder.UpdateData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 67,
                columns: new[] { "BossId", "SpecializationId" },
                values: new object[] { 4, 16 });

            migrationBuilder.UpdateData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 68,
                columns: new[] { "BossId", "SpecializationId" },
                values: new object[] { 4, 17 });

            migrationBuilder.UpdateData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 69,
                columns: new[] { "BossId", "SpecializationId" },
                values: new object[] { 5, 1 });

            migrationBuilder.UpdateData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 70,
                columns: new[] { "BossId", "SpecializationId" },
                values: new object[] { 5, 2 });

            migrationBuilder.UpdateData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 71,
                columns: new[] { "BossId", "SpecializationId" },
                values: new object[] { 5, 3 });

            migrationBuilder.UpdateData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 72,
                columns: new[] { "BossId", "SpecializationId" },
                values: new object[] { 5, 4 });

            migrationBuilder.UpdateData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 73,
                columns: new[] { "BossId", "SpecializationId" },
                values: new object[] { 5, 5 });

            migrationBuilder.UpdateData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 74,
                columns: new[] { "BossId", "SpecializationId" },
                values: new object[] { 5, 6 });

            migrationBuilder.UpdateData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 75,
                columns: new[] { "BossId", "SpecializationId" },
                values: new object[] { 5, 7 });

            migrationBuilder.UpdateData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 76,
                columns: new[] { "BossId", "SpecializationId" },
                values: new object[] { 5, 8 });

            migrationBuilder.UpdateData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 77,
                columns: new[] { "BossId", "SpecializationId" },
                values: new object[] { 5, 9 });

            migrationBuilder.UpdateData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 78,
                columns: new[] { "BossId", "SpecializationId" },
                values: new object[] { 5, 10 });

            migrationBuilder.UpdateData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 79,
                columns: new[] { "BossId", "SpecializationId" },
                values: new object[] { 5, 11 });

            migrationBuilder.UpdateData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 80,
                columns: new[] { "BossId", "SpecializationId" },
                values: new object[] { 5, 12 });

            migrationBuilder.UpdateData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 81,
                columns: new[] { "BossId", "SpecializationId" },
                values: new object[] { 5, 13 });

            migrationBuilder.UpdateData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 82,
                columns: new[] { "BossId", "SpecializationId" },
                values: new object[] { 5, 14 });

            migrationBuilder.UpdateData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 83,
                columns: new[] { "BossId", "SpecializationId" },
                values: new object[] { 5, 15 });

            migrationBuilder.UpdateData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 84,
                columns: new[] { "BossId", "SpecializationId" },
                values: new object[] { 5, 16 });

            migrationBuilder.UpdateData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 85,
                columns: new[] { "BossId", "SpecializationId" },
                values: new object[] { 5, 17 });

            migrationBuilder.UpdateData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 86,
                columns: new[] { "BossId", "SpecializationId" },
                values: new object[] { 6, 1 });

            migrationBuilder.UpdateData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 87,
                columns: new[] { "BossId", "SpecializationId" },
                values: new object[] { 6, 2 });

            migrationBuilder.UpdateData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 88,
                columns: new[] { "BossId", "SpecializationId" },
                values: new object[] { 6, 3 });

            migrationBuilder.UpdateData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 89,
                columns: new[] { "BossId", "SpecializationId" },
                values: new object[] { 6, 4 });

            migrationBuilder.UpdateData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 90,
                columns: new[] { "BossId", "SpecializationId" },
                values: new object[] { 6, 5 });

            migrationBuilder.UpdateData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 91,
                columns: new[] { "BossId", "SpecializationId" },
                values: new object[] { 6, 6 });

            migrationBuilder.UpdateData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 92,
                columns: new[] { "BossId", "SpecializationId" },
                values: new object[] { 6, 7 });

            migrationBuilder.UpdateData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 93,
                columns: new[] { "BossId", "SpecializationId" },
                values: new object[] { 6, 8 });

            migrationBuilder.UpdateData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 94,
                columns: new[] { "BossId", "SpecializationId" },
                values: new object[] { 6, 9 });

            migrationBuilder.UpdateData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 95,
                columns: new[] { "BossId", "SpecializationId" },
                values: new object[] { 6, 10 });

            migrationBuilder.UpdateData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 96,
                columns: new[] { "BossId", "SpecializationId" },
                values: new object[] { 6, 11 });

            migrationBuilder.UpdateData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 97,
                columns: new[] { "BossId", "SpecializationId" },
                values: new object[] { 6, 12 });

            migrationBuilder.UpdateData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 98,
                columns: new[] { "BossId", "SpecializationId" },
                values: new object[] { 6, 13 });

            migrationBuilder.UpdateData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 99,
                columns: new[] { "BossId", "SpecializationId" },
                values: new object[] { 6, 14 });

            migrationBuilder.UpdateData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 100,
                columns: new[] { "BossId", "SpecializationId" },
                values: new object[] { 6, 15 });

            migrationBuilder.UpdateData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 101,
                columns: new[] { "BossId", "SpecializationId" },
                values: new object[] { 6, 16 });

            migrationBuilder.UpdateData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 102,
                columns: new[] { "BossId", "SpecializationId" },
                values: new object[] { 6, 17 });

            migrationBuilder.UpdateData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 103,
                columns: new[] { "BossId", "SpecializationId" },
                values: new object[] { 7, 1 });

            migrationBuilder.UpdateData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 104,
                columns: new[] { "BossId", "SpecializationId" },
                values: new object[] { 7, 2 });

            migrationBuilder.UpdateData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 105,
                columns: new[] { "BossId", "SpecializationId" },
                values: new object[] { 7, 3 });

            migrationBuilder.UpdateData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 106,
                columns: new[] { "BossId", "SpecializationId" },
                values: new object[] { 7, 4 });

            migrationBuilder.UpdateData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 107,
                columns: new[] { "BossId", "SpecializationId" },
                values: new object[] { 7, 5 });

            migrationBuilder.UpdateData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 108,
                columns: new[] { "BossId", "SpecializationId" },
                values: new object[] { 7, 6 });

            migrationBuilder.UpdateData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 109,
                columns: new[] { "BossId", "SpecializationId" },
                values: new object[] { 7, 7 });

            migrationBuilder.UpdateData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 110,
                columns: new[] { "BossId", "SpecializationId" },
                values: new object[] { 7, 8 });

            migrationBuilder.UpdateData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 111,
                columns: new[] { "BossId", "SpecializationId" },
                values: new object[] { 7, 9 });

            migrationBuilder.UpdateData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 112,
                columns: new[] { "BossId", "SpecializationId" },
                values: new object[] { 7, 10 });

            migrationBuilder.UpdateData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 113,
                columns: new[] { "BossId", "SpecializationId" },
                values: new object[] { 7, 11 });

            migrationBuilder.UpdateData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 114,
                columns: new[] { "BossId", "SpecializationId" },
                values: new object[] { 7, 12 });

            migrationBuilder.UpdateData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 115,
                columns: new[] { "BossId", "SpecializationId" },
                values: new object[] { 7, 13 });

            migrationBuilder.UpdateData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 116,
                columns: new[] { "BossId", "SpecializationId" },
                values: new object[] { 7, 14 });

            migrationBuilder.UpdateData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 117,
                columns: new[] { "BossId", "SpecializationId" },
                values: new object[] { 7, 15 });

            migrationBuilder.UpdateData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 118,
                columns: new[] { "BossId", "SpecializationId" },
                values: new object[] { 7, 16 });

            migrationBuilder.UpdateData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 119,
                columns: new[] { "BossId", "SpecializationId" },
                values: new object[] { 7, 17 });

            migrationBuilder.UpdateData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 120,
                columns: new[] { "BossId", "SpecializationId" },
                values: new object[] { 8, 1 });

            migrationBuilder.UpdateData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 121,
                columns: new[] { "BossId", "SpecializationId" },
                values: new object[] { 8, 2 });

            migrationBuilder.UpdateData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 122,
                columns: new[] { "BossId", "SpecializationId" },
                values: new object[] { 8, 3 });

            migrationBuilder.UpdateData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 123,
                columns: new[] { "BossId", "SpecializationId" },
                values: new object[] { 8, 4 });

            migrationBuilder.UpdateData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 124,
                columns: new[] { "BossId", "SpecializationId" },
                values: new object[] { 8, 5 });

            migrationBuilder.UpdateData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 125,
                columns: new[] { "BossId", "SpecializationId" },
                values: new object[] { 8, 6 });

            migrationBuilder.UpdateData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 126,
                columns: new[] { "BossId", "SpecializationId" },
                values: new object[] { 8, 7 });

            migrationBuilder.UpdateData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 127,
                columns: new[] { "BossId", "SpecializationId" },
                values: new object[] { 8, 8 });

            migrationBuilder.UpdateData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 128,
                columns: new[] { "BossId", "SpecializationId" },
                values: new object[] { 8, 9 });

            migrationBuilder.UpdateData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 129,
                columns: new[] { "BossId", "SpecializationId" },
                values: new object[] { 8, 10 });

            migrationBuilder.UpdateData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 130,
                columns: new[] { "BossId", "SpecializationId" },
                values: new object[] { 8, 11 });

            migrationBuilder.UpdateData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 131,
                columns: new[] { "BossId", "SpecializationId" },
                values: new object[] { 8, 12 });

            migrationBuilder.UpdateData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 132,
                columns: new[] { "BossId", "SpecializationId" },
                values: new object[] { 8, 13 });

            migrationBuilder.UpdateData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 133,
                columns: new[] { "BossId", "SpecializationId" },
                values: new object[] { 8, 14 });

            migrationBuilder.UpdateData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 134,
                columns: new[] { "BossId", "SpecializationId" },
                values: new object[] { 8, 15 });

            migrationBuilder.UpdateData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 135,
                columns: new[] { "BossId", "SpecializationId" },
                values: new object[] { 8, 16 });

            migrationBuilder.UpdateData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 136,
                columns: new[] { "BossId", "SpecializationId" },
                values: new object[] { 8, 17 });

            migrationBuilder.UpdateData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 137,
                columns: new[] { "BossId", "SpecializationId" },
                values: new object[] { 9, 1 });

            migrationBuilder.UpdateData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 138,
                columns: new[] { "BossId", "SpecializationId" },
                values: new object[] { 9, 2 });

            migrationBuilder.UpdateData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 139,
                columns: new[] { "BossId", "SpecializationId" },
                values: new object[] { 9, 3 });

            migrationBuilder.UpdateData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 140,
                columns: new[] { "BossId", "SpecializationId" },
                values: new object[] { 9, 4 });

            migrationBuilder.UpdateData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 141,
                columns: new[] { "BossId", "SpecializationId" },
                values: new object[] { 9, 5 });

            migrationBuilder.UpdateData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 142,
                columns: new[] { "BossId", "SpecializationId" },
                values: new object[] { 9, 6 });

            migrationBuilder.UpdateData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 143,
                columns: new[] { "BossId", "SpecializationId" },
                values: new object[] { 9, 7 });

            migrationBuilder.UpdateData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 144,
                columns: new[] { "BossId", "SpecializationId" },
                values: new object[] { 9, 8 });

            migrationBuilder.UpdateData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 145,
                columns: new[] { "BossId", "SpecializationId" },
                values: new object[] { 9, 9 });

            migrationBuilder.UpdateData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 146,
                columns: new[] { "BossId", "SpecializationId" },
                values: new object[] { 9, 10 });

            migrationBuilder.UpdateData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 147,
                columns: new[] { "BossId", "SpecializationId" },
                values: new object[] { 9, 11 });

            migrationBuilder.UpdateData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 148,
                columns: new[] { "BossId", "SpecializationId" },
                values: new object[] { 9, 12 });

            migrationBuilder.UpdateData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 149,
                columns: new[] { "BossId", "SpecializationId" },
                values: new object[] { 9, 13 });

            migrationBuilder.UpdateData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 150,
                columns: new[] { "BossId", "SpecializationId" },
                values: new object[] { 9, 14 });

            migrationBuilder.UpdateData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 151,
                columns: new[] { "BossId", "SpecializationId" },
                values: new object[] { 9, 15 });

            migrationBuilder.UpdateData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 152,
                columns: new[] { "BossId", "SpecializationId" },
                values: new object[] { 9, 16 });

            migrationBuilder.UpdateData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 153,
                columns: new[] { "BossId", "SpecializationId" },
                values: new object[] { 9, 17 });

            migrationBuilder.UpdateData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 154,
                columns: new[] { "BossId", "SpecializationId" },
                values: new object[] { 10, 1 });

            migrationBuilder.UpdateData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 155,
                columns: new[] { "BossId", "SpecializationId" },
                values: new object[] { 10, 2 });

            migrationBuilder.UpdateData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 156,
                columns: new[] { "BossId", "SpecializationId" },
                values: new object[] { 10, 3 });

            migrationBuilder.UpdateData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 157,
                columns: new[] { "BossId", "SpecializationId" },
                values: new object[] { 10, 4 });

            migrationBuilder.UpdateData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 158,
                columns: new[] { "BossId", "SpecializationId" },
                values: new object[] { 10, 5 });

            migrationBuilder.UpdateData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 159,
                columns: new[] { "BossId", "SpecializationId" },
                values: new object[] { 10, 6 });

            migrationBuilder.UpdateData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 160,
                columns: new[] { "BossId", "SpecializationId" },
                values: new object[] { 10, 7 });

            migrationBuilder.UpdateData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 161,
                columns: new[] { "BossId", "SpecializationId" },
                values: new object[] { 10, 8 });

            migrationBuilder.UpdateData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 162,
                columns: new[] { "BossId", "SpecializationId" },
                values: new object[] { 10, 9 });

            migrationBuilder.UpdateData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 163,
                columns: new[] { "BossId", "SpecializationId" },
                values: new object[] { 10, 10 });

            migrationBuilder.UpdateData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 164,
                columns: new[] { "BossId", "SpecializationId" },
                values: new object[] { 10, 11 });

            migrationBuilder.UpdateData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 165,
                columns: new[] { "BossId", "SpecializationId" },
                values: new object[] { 10, 12 });

            migrationBuilder.UpdateData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 166,
                columns: new[] { "BossId", "SpecializationId" },
                values: new object[] { 10, 13 });

            migrationBuilder.UpdateData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 167,
                columns: new[] { "BossId", "SpecializationId" },
                values: new object[] { 10, 14 });

            migrationBuilder.UpdateData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 168,
                columns: new[] { "BossId", "SpecializationId" },
                values: new object[] { 10, 15 });

            migrationBuilder.UpdateData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 169,
                columns: new[] { "BossId", "SpecializationId" },
                values: new object[] { 10, 16 });

            migrationBuilder.UpdateData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 170,
                columns: new[] { "BossId", "SpecializationId" },
                values: new object[] { 10, 17 });

            migrationBuilder.UpdateData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 171,
                columns: new[] { "BossId", "SpecializationId" },
                values: new object[] { 11, 1 });

            migrationBuilder.UpdateData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 172,
                columns: new[] { "BossId", "SpecializationId" },
                values: new object[] { 11, 2 });

            migrationBuilder.UpdateData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 173,
                columns: new[] { "BossId", "SpecializationId" },
                values: new object[] { 11, 3 });

            migrationBuilder.UpdateData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 174,
                columns: new[] { "BossId", "SpecializationId" },
                values: new object[] { 11, 4 });

            migrationBuilder.UpdateData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 175,
                columns: new[] { "BossId", "SpecializationId" },
                values: new object[] { 11, 5 });

            migrationBuilder.UpdateData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 176,
                columns: new[] { "BossId", "SpecializationId" },
                values: new object[] { 11, 6 });

            migrationBuilder.UpdateData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 177,
                columns: new[] { "BossId", "SpecializationId" },
                values: new object[] { 11, 7 });

            migrationBuilder.UpdateData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 178,
                columns: new[] { "BossId", "SpecializationId" },
                values: new object[] { 11, 8 });

            migrationBuilder.UpdateData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 179,
                columns: new[] { "BossId", "SpecializationId" },
                values: new object[] { 11, 9 });

            migrationBuilder.UpdateData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 180,
                columns: new[] { "BossId", "SpecializationId" },
                values: new object[] { 11, 10 });

            migrationBuilder.UpdateData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 181,
                columns: new[] { "BossId", "SpecializationId" },
                values: new object[] { 11, 11 });

            migrationBuilder.UpdateData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 182,
                columns: new[] { "BossId", "SpecializationId" },
                values: new object[] { 11, 12 });

            migrationBuilder.UpdateData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 183,
                columns: new[] { "BossId", "SpecializationId" },
                values: new object[] { 11, 13 });

            migrationBuilder.UpdateData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 184,
                columns: new[] { "BossId", "SpecializationId" },
                values: new object[] { 11, 14 });

            migrationBuilder.UpdateData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 185,
                columns: new[] { "BossId", "SpecializationId" },
                values: new object[] { 11, 15 });

            migrationBuilder.UpdateData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 186,
                columns: new[] { "BossId", "SpecializationId" },
                values: new object[] { 11, 16 });

            migrationBuilder.UpdateData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 187,
                columns: new[] { "BossId", "SpecializationId" },
                values: new object[] { 11, 17 });

            migrationBuilder.UpdateData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 188,
                columns: new[] { "BossId", "SpecializationId" },
                values: new object[] { 12, 1 });

            migrationBuilder.UpdateData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 189,
                columns: new[] { "BossId", "SpecializationId" },
                values: new object[] { 12, 2 });

            migrationBuilder.UpdateData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 190,
                columns: new[] { "BossId", "SpecializationId" },
                values: new object[] { 12, 3 });

            migrationBuilder.UpdateData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 191,
                columns: new[] { "BossId", "SpecializationId" },
                values: new object[] { 12, 4 });

            migrationBuilder.UpdateData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 192,
                columns: new[] { "BossId", "SpecializationId" },
                values: new object[] { 12, 5 });

            migrationBuilder.UpdateData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 193,
                columns: new[] { "BossId", "SpecializationId" },
                values: new object[] { 12, 6 });

            migrationBuilder.UpdateData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 194,
                columns: new[] { "BossId", "SpecializationId" },
                values: new object[] { 12, 7 });

            migrationBuilder.UpdateData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 195,
                columns: new[] { "BossId", "SpecializationId" },
                values: new object[] { 12, 8 });

            migrationBuilder.UpdateData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 196,
                columns: new[] { "BossId", "SpecializationId" },
                values: new object[] { 12, 9 });

            migrationBuilder.UpdateData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 197,
                columns: new[] { "BossId", "SpecializationId" },
                values: new object[] { 12, 10 });

            migrationBuilder.UpdateData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 198,
                columns: new[] { "BossId", "SpecializationId" },
                values: new object[] { 12, 11 });

            migrationBuilder.UpdateData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 199,
                columns: new[] { "BossId", "SpecializationId" },
                values: new object[] { 12, 12 });

            migrationBuilder.UpdateData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 200,
                columns: new[] { "BossId", "SpecializationId" },
                values: new object[] { 12, 13 });

            migrationBuilder.UpdateData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 201,
                columns: new[] { "BossId", "SpecializationId" },
                values: new object[] { 12, 14 });

            migrationBuilder.UpdateData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 202,
                columns: new[] { "BossId", "SpecializationId" },
                values: new object[] { 12, 15 });

            migrationBuilder.UpdateData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 203,
                columns: new[] { "BossId", "SpecializationId" },
                values: new object[] { 12, 16 });

            migrationBuilder.UpdateData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 204,
                columns: new[] { "BossId", "SpecializationId" },
                values: new object[] { 12, 17 });

            migrationBuilder.UpdateData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 205,
                columns: new[] { "BossId", "SpecializationId" },
                values: new object[] { 13, 1 });

            migrationBuilder.UpdateData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 206,
                columns: new[] { "BossId", "SpecializationId" },
                values: new object[] { 13, 2 });

            migrationBuilder.UpdateData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 207,
                columns: new[] { "BossId", "SpecializationId" },
                values: new object[] { 13, 3 });

            migrationBuilder.UpdateData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 208,
                columns: new[] { "BossId", "SpecializationId" },
                values: new object[] { 13, 4 });

            migrationBuilder.UpdateData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 209,
                columns: new[] { "BossId", "SpecializationId" },
                values: new object[] { 13, 5 });

            migrationBuilder.UpdateData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 210,
                columns: new[] { "BossId", "SpecializationId" },
                values: new object[] { 13, 6 });

            migrationBuilder.UpdateData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 211,
                columns: new[] { "BossId", "SpecializationId" },
                values: new object[] { 13, 7 });

            migrationBuilder.UpdateData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 212,
                columns: new[] { "BossId", "SpecializationId" },
                values: new object[] { 13, 8 });

            migrationBuilder.UpdateData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 213,
                columns: new[] { "BossId", "SpecializationId" },
                values: new object[] { 13, 9 });

            migrationBuilder.UpdateData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 214,
                columns: new[] { "BossId", "SpecializationId" },
                values: new object[] { 13, 10 });

            migrationBuilder.UpdateData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 215,
                columns: new[] { "BossId", "SpecializationId" },
                values: new object[] { 13, 11 });

            migrationBuilder.UpdateData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 216,
                columns: new[] { "BossId", "SpecializationId" },
                values: new object[] { 13, 12 });

            migrationBuilder.UpdateData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 217,
                columns: new[] { "BossId", "SpecializationId" },
                values: new object[] { 13, 13 });

            migrationBuilder.UpdateData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 218,
                columns: new[] { "BossId", "SpecializationId" },
                values: new object[] { 13, 14 });

            migrationBuilder.UpdateData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 219,
                columns: new[] { "BossId", "SpecializationId" },
                values: new object[] { 13, 15 });

            migrationBuilder.UpdateData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 220,
                columns: new[] { "BossId", "SpecializationId" },
                values: new object[] { 13, 16 });

            migrationBuilder.UpdateData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 221,
                columns: new[] { "BossId", "SpecializationId" },
                values: new object[] { 13, 17 });

            migrationBuilder.UpdateData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 222,
                column: "BossId",
                value: 14);

            migrationBuilder.UpdateData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 223,
                column: "BossId",
                value: 14);

            migrationBuilder.UpdateData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 224,
                column: "BossId",
                value: 14);

            migrationBuilder.UpdateData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 225,
                column: "BossId",
                value: 14);

            migrationBuilder.UpdateData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 226,
                column: "BossId",
                value: 14);

            migrationBuilder.UpdateData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 227,
                column: "BossId",
                value: 14);

            migrationBuilder.UpdateData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 228,
                column: "BossId",
                value: 14);

            migrationBuilder.UpdateData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 229,
                column: "BossId",
                value: 14);

            migrationBuilder.UpdateData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 230,
                column: "BossId",
                value: 14);

            migrationBuilder.UpdateData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 231,
                column: "BossId",
                value: 14);

            migrationBuilder.UpdateData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 232,
                column: "BossId",
                value: 14);

            migrationBuilder.UpdateData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 233,
                column: "BossId",
                value: 14);

            migrationBuilder.UpdateData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 234,
                column: "BossId",
                value: 14);

            migrationBuilder.UpdateData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 235,
                columns: new[] { "BossId", "SpecializationId" },
                values: new object[] { 14, 14 });

            migrationBuilder.UpdateData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 236,
                columns: new[] { "BossId", "SpecializationId" },
                values: new object[] { 14, 15 });

            migrationBuilder.UpdateData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 237,
                columns: new[] { "BossId", "SpecializationId" },
                values: new object[] { 14, 16 });

            migrationBuilder.UpdateData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 238,
                columns: new[] { "BossId", "SpecializationId" },
                values: new object[] { 14, 17 });

            migrationBuilder.UpdateData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 239,
                columns: new[] { "BossId", "SpecializationId" },
                values: new object[] { 15, 1 });

            migrationBuilder.UpdateData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 240,
                columns: new[] { "BossId", "SpecializationId" },
                values: new object[] { 15, 2 });

            migrationBuilder.UpdateData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 241,
                columns: new[] { "BossId", "SpecializationId" },
                values: new object[] { 15, 3 });

            migrationBuilder.UpdateData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 242,
                columns: new[] { "BossId", "SpecializationId" },
                values: new object[] { 15, 4 });

            migrationBuilder.UpdateData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 243,
                columns: new[] { "BossId", "SpecializationId" },
                values: new object[] { 15, 5 });

            migrationBuilder.UpdateData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 244,
                columns: new[] { "BossId", "SpecializationId" },
                values: new object[] { 15, 6 });

            migrationBuilder.UpdateData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 245,
                columns: new[] { "BossId", "SpecializationId" },
                values: new object[] { 15, 7 });

            migrationBuilder.UpdateData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 246,
                columns: new[] { "BossId", "SpecializationId" },
                values: new object[] { 15, 8 });

            migrationBuilder.UpdateData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 247,
                columns: new[] { "BossId", "SpecializationId" },
                values: new object[] { 15, 9 });

            migrationBuilder.UpdateData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 248,
                columns: new[] { "BossId", "SpecializationId" },
                values: new object[] { 15, 10 });

            migrationBuilder.UpdateData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 249,
                columns: new[] { "BossId", "SpecializationId" },
                values: new object[] { 15, 11 });

            migrationBuilder.UpdateData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 250,
                columns: new[] { "BossId", "SpecializationId" },
                values: new object[] { 15, 12 });

            migrationBuilder.UpdateData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 251,
                columns: new[] { "BossId", "SpecializationId" },
                values: new object[] { 15, 13 });

            migrationBuilder.UpdateData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 252,
                columns: new[] { "BossId", "SpecializationId" },
                values: new object[] { 15, 14 });

            migrationBuilder.UpdateData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 253,
                columns: new[] { "BossId", "SpecializationId" },
                values: new object[] { 15, 15 });

            migrationBuilder.UpdateData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 254,
                columns: new[] { "BossId", "SpecializationId" },
                values: new object[] { 15, 16 });

            migrationBuilder.UpdateData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 255,
                columns: new[] { "BossId", "SpecializationId" },
                values: new object[] { 15, 17 });

            migrationBuilder.UpdateData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 256,
                columns: new[] { "BossId", "SpecializationId" },
                values: new object[] { 16, 1 });

            migrationBuilder.UpdateData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 257,
                columns: new[] { "BossId", "SpecializationId" },
                values: new object[] { 16, 2 });

            migrationBuilder.UpdateData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 258,
                columns: new[] { "BossId", "SpecializationId" },
                values: new object[] { 16, 3 });

            migrationBuilder.UpdateData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 259,
                columns: new[] { "BossId", "SpecializationId" },
                values: new object[] { 16, 4 });

            migrationBuilder.UpdateData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 260,
                columns: new[] { "BossId", "SpecializationId" },
                values: new object[] { 16, 5 });

            migrationBuilder.UpdateData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 261,
                columns: new[] { "BossId", "SpecializationId" },
                values: new object[] { 16, 6 });

            migrationBuilder.UpdateData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 262,
                columns: new[] { "BossId", "SpecializationId" },
                values: new object[] { 16, 7 });

            migrationBuilder.UpdateData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 263,
                columns: new[] { "BossId", "SpecializationId" },
                values: new object[] { 16, 8 });

            migrationBuilder.UpdateData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 264,
                columns: new[] { "BossId", "SpecializationId" },
                values: new object[] { 16, 9 });

            migrationBuilder.UpdateData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 265,
                columns: new[] { "BossId", "SpecializationId" },
                values: new object[] { 16, 10 });

            migrationBuilder.UpdateData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 266,
                columns: new[] { "BossId", "SpecializationId" },
                values: new object[] { 16, 11 });

            migrationBuilder.UpdateData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 267,
                columns: new[] { "BossId", "SpecializationId" },
                values: new object[] { 16, 12 });

            migrationBuilder.UpdateData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 268,
                columns: new[] { "BossId", "SpecializationId" },
                values: new object[] { 16, 13 });

            migrationBuilder.UpdateData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 269,
                columns: new[] { "BossId", "SpecializationId" },
                values: new object[] { 16, 14 });

            migrationBuilder.UpdateData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 270,
                columns: new[] { "BossId", "SpecializationId" },
                values: new object[] { 16, 15 });

            migrationBuilder.UpdateData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 271,
                columns: new[] { "BossId", "SpecializationId" },
                values: new object[] { 16, 16 });

            migrationBuilder.UpdateData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 272,
                columns: new[] { "BossId", "SpecializationId" },
                values: new object[] { 16, 17 });

            migrationBuilder.UpdateData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 273,
                columns: new[] { "BossId", "SpecializationId" },
                values: new object[] { 17, 1 });

            migrationBuilder.UpdateData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 274,
                columns: new[] { "BossId", "SpecializationId" },
                values: new object[] { 17, 2 });

            migrationBuilder.UpdateData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 275,
                columns: new[] { "BossId", "SpecializationId" },
                values: new object[] { 17, 3 });

            migrationBuilder.UpdateData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 276,
                columns: new[] { "BossId", "SpecializationId" },
                values: new object[] { 17, 4 });

            migrationBuilder.UpdateData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 277,
                columns: new[] { "BossId", "SpecializationId" },
                values: new object[] { 17, 5 });

            migrationBuilder.UpdateData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 278,
                columns: new[] { "BossId", "SpecializationId" },
                values: new object[] { 17, 6 });

            migrationBuilder.UpdateData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 279,
                columns: new[] { "BossId", "SpecializationId" },
                values: new object[] { 17, 7 });

            migrationBuilder.UpdateData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 280,
                columns: new[] { "BossId", "SpecializationId" },
                values: new object[] { 17, 8 });

            migrationBuilder.UpdateData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 281,
                columns: new[] { "BossId", "SpecializationId" },
                values: new object[] { 17, 9 });

            migrationBuilder.UpdateData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 282,
                columns: new[] { "BossId", "SpecializationId" },
                values: new object[] { 17, 10 });

            migrationBuilder.UpdateData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 283,
                columns: new[] { "BossId", "SpecializationId" },
                values: new object[] { 17, 11 });

            migrationBuilder.UpdateData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 284,
                columns: new[] { "BossId", "SpecializationId" },
                values: new object[] { 17, 12 });

            migrationBuilder.UpdateData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 285,
                columns: new[] { "BossId", "SpecializationId" },
                values: new object[] { 17, 13 });

            migrationBuilder.UpdateData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 286,
                columns: new[] { "BossId", "SpecializationId" },
                values: new object[] { 17, 14 });

            migrationBuilder.UpdateData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 287,
                columns: new[] { "BossId", "SpecializationId" },
                values: new object[] { 17, 15 });

            migrationBuilder.UpdateData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 288,
                columns: new[] { "BossId", "SpecializationId" },
                values: new object[] { 17, 16 });

            migrationBuilder.UpdateData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 289,
                columns: new[] { "BossId", "SpecializationId" },
                values: new object[] { 17, 17 });

            migrationBuilder.UpdateData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 290,
                columns: new[] { "BossId", "SpecializationId" },
                values: new object[] { 18, 1 });

            migrationBuilder.UpdateData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 291,
                columns: new[] { "BossId", "SpecializationId" },
                values: new object[] { 18, 2 });

            migrationBuilder.UpdateData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 292,
                columns: new[] { "BossId", "SpecializationId" },
                values: new object[] { 18, 3 });

            migrationBuilder.UpdateData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 293,
                columns: new[] { "BossId", "SpecializationId" },
                values: new object[] { 18, 4 });

            migrationBuilder.UpdateData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 294,
                columns: new[] { "BossId", "SpecializationId" },
                values: new object[] { 18, 5 });

            migrationBuilder.UpdateData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 295,
                columns: new[] { "BossId", "SpecializationId" },
                values: new object[] { 18, 6 });

            migrationBuilder.UpdateData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 296,
                columns: new[] { "BossId", "SpecializationId" },
                values: new object[] { 18, 7 });

            migrationBuilder.UpdateData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 297,
                columns: new[] { "BossId", "SpecializationId" },
                values: new object[] { 18, 8 });

            migrationBuilder.UpdateData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 298,
                columns: new[] { "BossId", "SpecializationId" },
                values: new object[] { 18, 9 });

            migrationBuilder.UpdateData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 299,
                columns: new[] { "BossId", "SpecializationId" },
                values: new object[] { 18, 10 });

            migrationBuilder.UpdateData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 300,
                columns: new[] { "BossId", "SpecializationId" },
                values: new object[] { 18, 11 });

            migrationBuilder.UpdateData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 301,
                columns: new[] { "BossId", "SpecializationId" },
                values: new object[] { 18, 12 });

            migrationBuilder.UpdateData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 302,
                columns: new[] { "BossId", "SpecializationId" },
                values: new object[] { 18, 13 });

            migrationBuilder.UpdateData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 303,
                columns: new[] { "BossId", "SpecializationId" },
                values: new object[] { 18, 14 });

            migrationBuilder.UpdateData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 304,
                columns: new[] { "BossId", "SpecializationId" },
                values: new object[] { 18, 15 });

            migrationBuilder.UpdateData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 305,
                columns: new[] { "BossId", "SpecializationId" },
                values: new object[] { 18, 16 });

            migrationBuilder.UpdateData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 306,
                columns: new[] { "BossId", "SpecializationId" },
                values: new object[] { 18, 17 });

            migrationBuilder.UpdateData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 307,
                columns: new[] { "BossId", "SpecializationId" },
                values: new object[] { 19, 1 });

            migrationBuilder.UpdateData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 308,
                columns: new[] { "BossId", "SpecializationId" },
                values: new object[] { 19, 2 });

            migrationBuilder.UpdateData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 309,
                columns: new[] { "BossId", "SpecializationId" },
                values: new object[] { 19, 3 });

            migrationBuilder.UpdateData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 310,
                columns: new[] { "BossId", "SpecializationId" },
                values: new object[] { 19, 4 });

            migrationBuilder.UpdateData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 311,
                columns: new[] { "BossId", "SpecializationId" },
                values: new object[] { 19, 5 });

            migrationBuilder.UpdateData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 312,
                columns: new[] { "BossId", "SpecializationId" },
                values: new object[] { 19, 6 });

            migrationBuilder.UpdateData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 313,
                columns: new[] { "BossId", "SpecializationId" },
                values: new object[] { 19, 7 });

            migrationBuilder.UpdateData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 314,
                columns: new[] { "BossId", "SpecializationId" },
                values: new object[] { 19, 8 });

            migrationBuilder.UpdateData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 315,
                columns: new[] { "BossId", "SpecializationId" },
                values: new object[] { 19, 9 });

            migrationBuilder.UpdateData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 316,
                columns: new[] { "BossId", "SpecializationId" },
                values: new object[] { 19, 10 });

            migrationBuilder.UpdateData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 317,
                columns: new[] { "BossId", "SpecializationId" },
                values: new object[] { 19, 11 });

            migrationBuilder.UpdateData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 318,
                columns: new[] { "BossId", "SpecializationId" },
                values: new object[] { 19, 12 });

            migrationBuilder.UpdateData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 319,
                columns: new[] { "BossId", "SpecializationId" },
                values: new object[] { 19, 13 });

            migrationBuilder.UpdateData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 320,
                columns: new[] { "BossId", "SpecializationId" },
                values: new object[] { 19, 14 });

            migrationBuilder.UpdateData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 321,
                columns: new[] { "BossId", "SpecializationId" },
                values: new object[] { 19, 15 });

            migrationBuilder.UpdateData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 322,
                columns: new[] { "BossId", "SpecializationId" },
                values: new object[] { 19, 16 });

            migrationBuilder.UpdateData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 323,
                columns: new[] { "BossId", "SpecializationId" },
                values: new object[] { 19, 17 });

            migrationBuilder.UpdateData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 324,
                columns: new[] { "BossId", "SpecializationId" },
                values: new object[] { 20, 1 });

            migrationBuilder.UpdateData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 325,
                columns: new[] { "BossId", "SpecializationId" },
                values: new object[] { 20, 2 });

            migrationBuilder.UpdateData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 326,
                columns: new[] { "BossId", "SpecializationId" },
                values: new object[] { 20, 3 });

            migrationBuilder.UpdateData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 327,
                columns: new[] { "BossId", "SpecializationId" },
                values: new object[] { 20, 4 });

            migrationBuilder.UpdateData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 328,
                columns: new[] { "BossId", "SpecializationId" },
                values: new object[] { 20, 5 });

            migrationBuilder.UpdateData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 329,
                columns: new[] { "BossId", "SpecializationId" },
                values: new object[] { 20, 6 });

            migrationBuilder.UpdateData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 330,
                columns: new[] { "BossId", "SpecializationId" },
                values: new object[] { 20, 7 });

            migrationBuilder.UpdateData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 331,
                columns: new[] { "BossId", "SpecializationId" },
                values: new object[] { 20, 8 });

            migrationBuilder.UpdateData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 332,
                columns: new[] { "BossId", "SpecializationId" },
                values: new object[] { 20, 9 });

            migrationBuilder.UpdateData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 333,
                columns: new[] { "BossId", "SpecializationId" },
                values: new object[] { 20, 10 });

            migrationBuilder.UpdateData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 334,
                columns: new[] { "BossId", "SpecializationId" },
                values: new object[] { 20, 11 });

            migrationBuilder.UpdateData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 335,
                columns: new[] { "BossId", "SpecializationId" },
                values: new object[] { 20, 12 });

            migrationBuilder.UpdateData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 336,
                columns: new[] { "BossId", "SpecializationId" },
                values: new object[] { 20, 13 });

            migrationBuilder.UpdateData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 337,
                columns: new[] { "BossId", "SpecializationId" },
                values: new object[] { 20, 14 });

            migrationBuilder.UpdateData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 338,
                columns: new[] { "BossId", "SpecializationId" },
                values: new object[] { 20, 15 });

            migrationBuilder.UpdateData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 339,
                columns: new[] { "BossId", "SpecializationId" },
                values: new object[] { 20, 16 });

            migrationBuilder.UpdateData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 340,
                columns: new[] { "BossId", "SpecializationId" },
                values: new object[] { 20, 17 });

            migrationBuilder.UpdateData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 341,
                columns: new[] { "BossId", "SpecializationId" },
                values: new object[] { 21, 1 });

            migrationBuilder.UpdateData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 342,
                columns: new[] { "BossId", "SpecializationId" },
                values: new object[] { 21, 2 });

            migrationBuilder.UpdateData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 343,
                columns: new[] { "BossId", "SpecializationId" },
                values: new object[] { 21, 3 });

            migrationBuilder.UpdateData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 344,
                columns: new[] { "BossId", "SpecializationId" },
                values: new object[] { 21, 4 });

            migrationBuilder.UpdateData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 345,
                columns: new[] { "BossId", "SpecializationId" },
                values: new object[] { 21, 5 });

            migrationBuilder.UpdateData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 346,
                columns: new[] { "BossId", "SpecializationId" },
                values: new object[] { 21, 6 });

            migrationBuilder.UpdateData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 347,
                columns: new[] { "BossId", "SpecializationId" },
                values: new object[] { 21, 7 });

            migrationBuilder.UpdateData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 348,
                columns: new[] { "BossId", "SpecializationId" },
                values: new object[] { 21, 8 });

            migrationBuilder.UpdateData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 349,
                columns: new[] { "BossId", "SpecializationId" },
                values: new object[] { 21, 9 });

            migrationBuilder.UpdateData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 350,
                columns: new[] { "BossId", "SpecializationId" },
                values: new object[] { 21, 10 });

            migrationBuilder.UpdateData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 351,
                columns: new[] { "BossId", "SpecializationId" },
                values: new object[] { 21, 11 });

            migrationBuilder.UpdateData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 352,
                columns: new[] { "BossId", "SpecializationId" },
                values: new object[] { 21, 12 });

            migrationBuilder.UpdateData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 353,
                columns: new[] { "BossId", "SpecializationId" },
                values: new object[] { 21, 13 });

            migrationBuilder.UpdateData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 354,
                columns: new[] { "BossId", "SpecializationId" },
                values: new object[] { 21, 14 });

            migrationBuilder.UpdateData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 355,
                columns: new[] { "BossId", "SpecializationId" },
                values: new object[] { 21, 15 });

            migrationBuilder.UpdateData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 356,
                columns: new[] { "BossId", "SpecializationId" },
                values: new object[] { 21, 16 });

            migrationBuilder.UpdateData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 357,
                columns: new[] { "BossId", "SpecializationId" },
                values: new object[] { 21, 17 });

            migrationBuilder.UpdateData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 358,
                columns: new[] { "BossId", "SpecializationId" },
                values: new object[] { 22, 1 });

            migrationBuilder.UpdateData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 359,
                columns: new[] { "BossId", "SpecializationId" },
                values: new object[] { 22, 2 });

            migrationBuilder.UpdateData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 360,
                columns: new[] { "BossId", "SpecializationId" },
                values: new object[] { 22, 3 });

            migrationBuilder.UpdateData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 361,
                columns: new[] { "BossId", "SpecializationId" },
                values: new object[] { 22, 4 });

            migrationBuilder.UpdateData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 362,
                columns: new[] { "BossId", "SpecializationId" },
                values: new object[] { 22, 5 });

            migrationBuilder.UpdateData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 363,
                columns: new[] { "BossId", "SpecializationId" },
                values: new object[] { 22, 6 });

            migrationBuilder.UpdateData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 364,
                columns: new[] { "BossId", "SpecializationId" },
                values: new object[] { 22, 7 });

            migrationBuilder.UpdateData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 365,
                columns: new[] { "BossId", "SpecializationId" },
                values: new object[] { 22, 8 });

            migrationBuilder.UpdateData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 366,
                columns: new[] { "BossId", "SpecializationId" },
                values: new object[] { 22, 9 });

            migrationBuilder.UpdateData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 367,
                columns: new[] { "BossId", "SpecializationId" },
                values: new object[] { 22, 10 });

            migrationBuilder.UpdateData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 368,
                columns: new[] { "BossId", "SpecializationId" },
                values: new object[] { 22, 11 });

            migrationBuilder.UpdateData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 369,
                columns: new[] { "BossId", "SpecializationId" },
                values: new object[] { 22, 12 });

            migrationBuilder.UpdateData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 370,
                columns: new[] { "BossId", "SpecializationId" },
                values: new object[] { 22, 13 });

            migrationBuilder.UpdateData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 371,
                columns: new[] { "BossId", "SpecializationId" },
                values: new object[] { 22, 14 });

            migrationBuilder.UpdateData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 372,
                columns: new[] { "BossId", "SpecializationId" },
                values: new object[] { 22, 15 });

            migrationBuilder.UpdateData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 373,
                columns: new[] { "BossId", "SpecializationId" },
                values: new object[] { 22, 16 });

            migrationBuilder.UpdateData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 374,
                columns: new[] { "BossId", "SpecializationId" },
                values: new object[] { 22, 17 });

            migrationBuilder.UpdateData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 375,
                columns: new[] { "BossId", "SpecializationId" },
                values: new object[] { 23, 1 });

            migrationBuilder.UpdateData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 376,
                columns: new[] { "BossId", "SpecializationId" },
                values: new object[] { 23, 2 });

            migrationBuilder.UpdateData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 377,
                columns: new[] { "BossId", "SpecializationId" },
                values: new object[] { 23, 3 });

            migrationBuilder.UpdateData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 378,
                columns: new[] { "BossId", "SpecializationId" },
                values: new object[] { 23, 4 });

            migrationBuilder.UpdateData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 379,
                columns: new[] { "BossId", "SpecializationId" },
                values: new object[] { 23, 5 });

            migrationBuilder.UpdateData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 380,
                columns: new[] { "BossId", "SpecializationId" },
                values: new object[] { 23, 6 });

            migrationBuilder.UpdateData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 381,
                columns: new[] { "BossId", "SpecializationId" },
                values: new object[] { 23, 7 });

            migrationBuilder.UpdateData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 382,
                columns: new[] { "BossId", "SpecializationId" },
                values: new object[] { 23, 8 });

            migrationBuilder.UpdateData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 383,
                columns: new[] { "BossId", "SpecializationId" },
                values: new object[] { 23, 9 });

            migrationBuilder.UpdateData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 384,
                columns: new[] { "BossId", "SpecializationId" },
                values: new object[] { 23, 10 });

            migrationBuilder.UpdateData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 385,
                columns: new[] { "BossId", "SpecializationId" },
                values: new object[] { 23, 11 });

            migrationBuilder.UpdateData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 386,
                columns: new[] { "BossId", "SpecializationId" },
                values: new object[] { 23, 12 });

            migrationBuilder.UpdateData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 387,
                columns: new[] { "BossId", "SpecializationId" },
                values: new object[] { 23, 13 });

            migrationBuilder.UpdateData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 388,
                columns: new[] { "BossId", "SpecializationId" },
                values: new object[] { 23, 14 });

            migrationBuilder.UpdateData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 389,
                columns: new[] { "BossId", "SpecializationId" },
                values: new object[] { 23, 15 });

            migrationBuilder.UpdateData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 390,
                columns: new[] { "BossId", "SpecializationId" },
                values: new object[] { 23, 16 });

            migrationBuilder.UpdateData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 391,
                columns: new[] { "BossId", "SpecializationId" },
                values: new object[] { 23, 17 });

            migrationBuilder.UpdateData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 392,
                columns: new[] { "BossId", "SpecializationId" },
                values: new object[] { 24, 1 });

            migrationBuilder.UpdateData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 393,
                columns: new[] { "BossId", "SpecializationId" },
                values: new object[] { 24, 2 });

            migrationBuilder.UpdateData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 394,
                columns: new[] { "BossId", "SpecializationId" },
                values: new object[] { 24, 3 });

            migrationBuilder.UpdateData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 395,
                columns: new[] { "BossId", "SpecializationId" },
                values: new object[] { 24, 4 });

            migrationBuilder.UpdateData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 396,
                columns: new[] { "BossId", "SpecializationId" },
                values: new object[] { 24, 5 });

            migrationBuilder.UpdateData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 397,
                columns: new[] { "BossId", "SpecializationId" },
                values: new object[] { 24, 6 });

            migrationBuilder.UpdateData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 398,
                columns: new[] { "BossId", "SpecializationId" },
                values: new object[] { 24, 7 });

            migrationBuilder.UpdateData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 399,
                columns: new[] { "BossId", "SpecializationId" },
                values: new object[] { 24, 8 });

            migrationBuilder.UpdateData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 400,
                columns: new[] { "BossId", "SpecializationId" },
                values: new object[] { 24, 9 });

            migrationBuilder.UpdateData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 401,
                columns: new[] { "BossId", "SpecializationId" },
                values: new object[] { 24, 10 });

            migrationBuilder.UpdateData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 402,
                columns: new[] { "BossId", "SpecializationId" },
                values: new object[] { 24, 11 });

            migrationBuilder.UpdateData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 403,
                columns: new[] { "BossId", "SpecializationId" },
                values: new object[] { 24, 12 });

            migrationBuilder.UpdateData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 404,
                columns: new[] { "BossId", "SpecializationId" },
                values: new object[] { 24, 13 });

            migrationBuilder.UpdateData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 405,
                columns: new[] { "BossId", "SpecializationId" },
                values: new object[] { 24, 14 });

            migrationBuilder.UpdateData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 406,
                columns: new[] { "BossId", "SpecializationId" },
                values: new object[] { 24, 15 });

            migrationBuilder.UpdateData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 407,
                columns: new[] { "BossId", "SpecializationId" },
                values: new object[] { 24, 16 });

            migrationBuilder.UpdateData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 408,
                columns: new[] { "BossId", "SpecializationId" },
                values: new object[] { 24, 17 });

            migrationBuilder.UpdateData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 409,
                columns: new[] { "BossId", "SpecializationId" },
                values: new object[] { 25, 1 });

            migrationBuilder.UpdateData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 410,
                columns: new[] { "BossId", "SpecializationId" },
                values: new object[] { 25, 2 });

            migrationBuilder.UpdateData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 411,
                columns: new[] { "BossId", "SpecializationId" },
                values: new object[] { 25, 3 });

            migrationBuilder.UpdateData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 412,
                columns: new[] { "BossId", "SpecializationId" },
                values: new object[] { 25, 4 });

            migrationBuilder.UpdateData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 413,
                columns: new[] { "BossId", "SpecializationId" },
                values: new object[] { 25, 5 });

            migrationBuilder.UpdateData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 414,
                columns: new[] { "BossId", "SpecializationId" },
                values: new object[] { 25, 6 });

            migrationBuilder.UpdateData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 415,
                columns: new[] { "BossId", "SpecializationId" },
                values: new object[] { 25, 7 });

            migrationBuilder.UpdateData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 416,
                columns: new[] { "BossId", "SpecializationId" },
                values: new object[] { 25, 8 });

            migrationBuilder.UpdateData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 417,
                columns: new[] { "BossId", "SpecializationId" },
                values: new object[] { 25, 9 });

            migrationBuilder.UpdateData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 418,
                columns: new[] { "BossId", "SpecializationId" },
                values: new object[] { 25, 10 });

            migrationBuilder.UpdateData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 419,
                columns: new[] { "BossId", "SpecializationId" },
                values: new object[] { 25, 11 });

            migrationBuilder.UpdateData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 420,
                columns: new[] { "BossId", "SpecializationId" },
                values: new object[] { 25, 12 });

            migrationBuilder.UpdateData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 421,
                columns: new[] { "BossId", "SpecializationId" },
                values: new object[] { 25, 13 });

            migrationBuilder.UpdateData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 422,
                columns: new[] { "BossId", "SpecializationId" },
                values: new object[] { 25, 14 });

            migrationBuilder.UpdateData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 423,
                columns: new[] { "BossId", "SpecializationId" },
                values: new object[] { 25, 15 });

            migrationBuilder.UpdateData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 424,
                columns: new[] { "BossId", "SpecializationId" },
                values: new object[] { 25, 16 });

            migrationBuilder.UpdateData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 425,
                columns: new[] { "BossId", "SpecializationId" },
                values: new object[] { 25, 17 });

            migrationBuilder.UpdateData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 426,
                columns: new[] { "BossId", "SpecializationId" },
                values: new object[] { 26, 1 });

            migrationBuilder.UpdateData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 427,
                columns: new[] { "BossId", "SpecializationId" },
                values: new object[] { 26, 2 });

            migrationBuilder.UpdateData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 428,
                columns: new[] { "BossId", "SpecializationId" },
                values: new object[] { 26, 3 });

            migrationBuilder.UpdateData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 429,
                columns: new[] { "BossId", "SpecializationId" },
                values: new object[] { 26, 4 });

            migrationBuilder.UpdateData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 430,
                columns: new[] { "BossId", "SpecializationId" },
                values: new object[] { 26, 5 });

            migrationBuilder.UpdateData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 431,
                columns: new[] { "BossId", "SpecializationId" },
                values: new object[] { 26, 6 });

            migrationBuilder.UpdateData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 432,
                columns: new[] { "BossId", "SpecializationId" },
                values: new object[] { 26, 7 });

            migrationBuilder.UpdateData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 433,
                columns: new[] { "BossId", "SpecializationId" },
                values: new object[] { 26, 8 });

            migrationBuilder.UpdateData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 434,
                columns: new[] { "BossId", "SpecializationId" },
                values: new object[] { 26, 9 });

            migrationBuilder.UpdateData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 435,
                columns: new[] { "BossId", "SpecializationId" },
                values: new object[] { 26, 10 });

            migrationBuilder.UpdateData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 436,
                columns: new[] { "BossId", "SpecializationId" },
                values: new object[] { 26, 11 });

            migrationBuilder.UpdateData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 437,
                columns: new[] { "BossId", "SpecializationId" },
                values: new object[] { 26, 12 });

            migrationBuilder.UpdateData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 438,
                columns: new[] { "BossId", "SpecializationId" },
                values: new object[] { 26, 13 });

            migrationBuilder.UpdateData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 439,
                columns: new[] { "BossId", "SpecializationId" },
                values: new object[] { 26, 14 });

            migrationBuilder.UpdateData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 440,
                columns: new[] { "BossId", "SpecializationId" },
                values: new object[] { 26, 15 });

            migrationBuilder.UpdateData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 441,
                columns: new[] { "BossId", "SpecializationId" },
                values: new object[] { 26, 16 });

            migrationBuilder.UpdateData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 442,
                columns: new[] { "BossId", "SpecializationId" },
                values: new object[] { 26, 17 });

            migrationBuilder.UpdateData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 443,
                column: "BossId",
                value: 27);

            migrationBuilder.UpdateData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 444,
                column: "BossId",
                value: 27);

            migrationBuilder.UpdateData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 445,
                column: "BossId",
                value: 27);

            migrationBuilder.UpdateData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 446,
                column: "BossId",
                value: 27);

            migrationBuilder.UpdateData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 447,
                column: "BossId",
                value: 27);

            migrationBuilder.UpdateData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 448,
                column: "BossId",
                value: 27);

            migrationBuilder.UpdateData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 449,
                column: "BossId",
                value: 27);

            migrationBuilder.UpdateData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 450,
                column: "BossId",
                value: 27);

            migrationBuilder.UpdateData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 451,
                column: "BossId",
                value: 27);

            migrationBuilder.UpdateData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 452,
                column: "BossId",
                value: 27);

            migrationBuilder.UpdateData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 453,
                column: "BossId",
                value: 27);

            migrationBuilder.UpdateData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 454,
                column: "BossId",
                value: 27);

            migrationBuilder.UpdateData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 455,
                column: "BossId",
                value: 27);

            migrationBuilder.UpdateData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 456,
                columns: new[] { "BossId", "SpecializationId" },
                values: new object[] { 27, 14 });

            migrationBuilder.UpdateData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 457,
                columns: new[] { "BossId", "SpecializationId" },
                values: new object[] { 27, 15 });

            migrationBuilder.UpdateData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 458,
                columns: new[] { "BossId", "SpecializationId" },
                values: new object[] { 27, 16 });

            migrationBuilder.UpdateData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 459,
                columns: new[] { "BossId", "SpecializationId" },
                values: new object[] { 27, 17 });

            migrationBuilder.UpdateData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 460,
                columns: new[] { "BossId", "SpecializationId" },
                values: new object[] { 28, 1 });

            migrationBuilder.UpdateData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 461,
                columns: new[] { "BossId", "SpecializationId" },
                values: new object[] { 28, 2 });

            migrationBuilder.UpdateData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 462,
                columns: new[] { "BossId", "SpecializationId" },
                values: new object[] { 28, 3 });

            migrationBuilder.UpdateData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 463,
                columns: new[] { "BossId", "SpecializationId" },
                values: new object[] { 28, 4 });

            migrationBuilder.UpdateData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 464,
                columns: new[] { "BossId", "SpecializationId" },
                values: new object[] { 28, 5 });

            migrationBuilder.UpdateData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 465,
                columns: new[] { "BossId", "SpecializationId" },
                values: new object[] { 28, 6 });

            migrationBuilder.UpdateData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 466,
                columns: new[] { "BossId", "SpecializationId" },
                values: new object[] { 28, 7 });

            migrationBuilder.UpdateData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 467,
                columns: new[] { "BossId", "SpecializationId" },
                values: new object[] { 28, 8 });

            migrationBuilder.UpdateData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 468,
                columns: new[] { "BossId", "SpecializationId" },
                values: new object[] { 28, 9 });

            migrationBuilder.UpdateData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 469,
                columns: new[] { "BossId", "SpecializationId" },
                values: new object[] { 28, 10 });

            migrationBuilder.UpdateData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 470,
                columns: new[] { "BossId", "SpecializationId" },
                values: new object[] { 28, 11 });

            migrationBuilder.UpdateData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 471,
                columns: new[] { "BossId", "SpecializationId" },
                values: new object[] { 28, 12 });

            migrationBuilder.UpdateData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 472,
                columns: new[] { "BossId", "SpecializationId" },
                values: new object[] { 28, 13 });

            migrationBuilder.UpdateData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 473,
                columns: new[] { "BossId", "SpecializationId" },
                values: new object[] { 28, 14 });

            migrationBuilder.UpdateData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 474,
                columns: new[] { "BossId", "SpecializationId" },
                values: new object[] { 28, 15 });

            migrationBuilder.UpdateData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 475,
                columns: new[] { "BossId", "SpecializationId" },
                values: new object[] { 28, 16 });

            migrationBuilder.UpdateData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 476,
                columns: new[] { "BossId", "SpecializationId" },
                values: new object[] { 28, 17 });

            migrationBuilder.UpdateData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 477,
                columns: new[] { "BossId", "SpecializationId" },
                values: new object[] { 29, 1 });

            migrationBuilder.UpdateData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 478,
                columns: new[] { "BossId", "SpecializationId" },
                values: new object[] { 29, 2 });

            migrationBuilder.UpdateData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 479,
                columns: new[] { "BossId", "SpecializationId" },
                values: new object[] { 29, 3 });

            migrationBuilder.UpdateData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 480,
                columns: new[] { "BossId", "SpecializationId" },
                values: new object[] { 29, 4 });

            migrationBuilder.UpdateData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 481,
                columns: new[] { "BossId", "SpecializationId" },
                values: new object[] { 29, 5 });

            migrationBuilder.UpdateData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 482,
                columns: new[] { "BossId", "SpecializationId" },
                values: new object[] { 29, 6 });

            migrationBuilder.UpdateData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 483,
                columns: new[] { "BossId", "SpecializationId" },
                values: new object[] { 29, 7 });

            migrationBuilder.UpdateData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 484,
                columns: new[] { "BossId", "SpecializationId" },
                values: new object[] { 29, 8 });

            migrationBuilder.UpdateData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 485,
                columns: new[] { "BossId", "SpecializationId" },
                values: new object[] { 29, 9 });

            migrationBuilder.UpdateData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 486,
                columns: new[] { "BossId", "SpecializationId" },
                values: new object[] { 29, 10 });

            migrationBuilder.UpdateData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 487,
                columns: new[] { "BossId", "SpecializationId" },
                values: new object[] { 29, 11 });

            migrationBuilder.UpdateData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 488,
                columns: new[] { "BossId", "SpecializationId" },
                values: new object[] { 29, 12 });

            migrationBuilder.UpdateData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 489,
                columns: new[] { "BossId", "SpecializationId" },
                values: new object[] { 29, 13 });

            migrationBuilder.UpdateData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 490,
                columns: new[] { "BossId", "SpecializationId" },
                values: new object[] { 29, 14 });

            migrationBuilder.UpdateData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 491,
                columns: new[] { "BossId", "SpecializationId" },
                values: new object[] { 29, 15 });

            migrationBuilder.UpdateData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 492,
                columns: new[] { "BossId", "SpecializationId" },
                values: new object[] { 29, 16 });

            migrationBuilder.UpdateData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 493,
                columns: new[] { "BossId", "SpecializationId" },
                values: new object[] { 29, 17 });

            migrationBuilder.UpdateData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 494,
                columns: new[] { "BossId", "SpecializationId" },
                values: new object[] { 30, 1 });

            migrationBuilder.UpdateData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 495,
                columns: new[] { "BossId", "SpecializationId" },
                values: new object[] { 30, 2 });

            migrationBuilder.UpdateData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 496,
                columns: new[] { "BossId", "SpecializationId" },
                values: new object[] { 30, 3 });

            migrationBuilder.UpdateData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 497,
                columns: new[] { "BossId", "SpecializationId" },
                values: new object[] { 30, 4 });

            migrationBuilder.UpdateData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 498,
                columns: new[] { "BossId", "SpecializationId" },
                values: new object[] { 30, 5 });

            migrationBuilder.UpdateData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 499,
                columns: new[] { "BossId", "SpecializationId" },
                values: new object[] { 30, 6 });

            migrationBuilder.UpdateData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 500,
                columns: new[] { "BossId", "SpecializationId" },
                values: new object[] { 30, 7 });

            migrationBuilder.UpdateData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 501,
                columns: new[] { "BossId", "SpecializationId" },
                values: new object[] { 30, 8 });

            migrationBuilder.UpdateData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 502,
                columns: new[] { "BossId", "SpecializationId" },
                values: new object[] { 30, 9 });

            migrationBuilder.UpdateData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 503,
                columns: new[] { "BossId", "SpecializationId" },
                values: new object[] { 30, 10 });

            migrationBuilder.UpdateData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 504,
                columns: new[] { "BossId", "SpecializationId" },
                values: new object[] { 30, 11 });

            migrationBuilder.UpdateData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 505,
                columns: new[] { "BossId", "SpecializationId" },
                values: new object[] { 30, 12 });

            migrationBuilder.UpdateData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 506,
                columns: new[] { "BossId", "SpecializationId" },
                values: new object[] { 30, 13 });

            migrationBuilder.UpdateData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 507,
                columns: new[] { "BossId", "SpecializationId" },
                values: new object[] { 30, 14 });

            migrationBuilder.UpdateData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 508,
                columns: new[] { "BossId", "SpecializationId" },
                values: new object[] { 30, 15 });

            migrationBuilder.UpdateData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 509,
                columns: new[] { "BossId", "SpecializationId" },
                values: new object[] { 30, 16 });

            migrationBuilder.UpdateData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 510,
                columns: new[] { "BossId", "SpecializationId" },
                values: new object[] { 30, 17 });

            migrationBuilder.UpdateData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 511,
                columns: new[] { "BossId", "SpecializationId" },
                values: new object[] { 31, 1 });

            migrationBuilder.UpdateData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 512,
                columns: new[] { "BossId", "SpecializationId" },
                values: new object[] { 31, 2 });

            migrationBuilder.UpdateData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 513,
                columns: new[] { "BossId", "SpecializationId" },
                values: new object[] { 31, 3 });

            migrationBuilder.UpdateData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 514,
                columns: new[] { "BossId", "SpecializationId" },
                values: new object[] { 31, 4 });

            migrationBuilder.UpdateData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 515,
                columns: new[] { "BossId", "SpecializationId" },
                values: new object[] { 31, 5 });

            migrationBuilder.UpdateData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 516,
                columns: new[] { "BossId", "SpecializationId" },
                values: new object[] { 31, 6 });

            migrationBuilder.UpdateData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 517,
                columns: new[] { "BossId", "SpecializationId" },
                values: new object[] { 31, 7 });

            migrationBuilder.UpdateData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 518,
                columns: new[] { "BossId", "SpecializationId" },
                values: new object[] { 31, 8 });

            migrationBuilder.UpdateData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 519,
                columns: new[] { "BossId", "SpecializationId" },
                values: new object[] { 31, 9 });

            migrationBuilder.UpdateData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 520,
                columns: new[] { "BossId", "SpecializationId" },
                values: new object[] { 31, 10 });

            migrationBuilder.UpdateData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 521,
                columns: new[] { "BossId", "SpecializationId" },
                values: new object[] { 31, 11 });

            migrationBuilder.UpdateData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 522,
                columns: new[] { "BossId", "SpecializationId" },
                values: new object[] { 31, 12 });

            migrationBuilder.UpdateData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 523,
                columns: new[] { "BossId", "SpecializationId" },
                values: new object[] { 31, 13 });

            migrationBuilder.UpdateData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 524,
                columns: new[] { "BossId", "SpecializationId" },
                values: new object[] { 31, 14 });

            migrationBuilder.UpdateData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 525,
                columns: new[] { "BossId", "SpecializationId" },
                values: new object[] { 31, 15 });

            migrationBuilder.UpdateData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 526,
                columns: new[] { "BossId", "SpecializationId" },
                values: new object[] { 31, 16 });

            migrationBuilder.UpdateData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 527,
                columns: new[] { "BossId", "SpecializationId" },
                values: new object[] { 31, 17 });

            migrationBuilder.UpdateData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 528,
                columns: new[] { "BossId", "SpecializationId" },
                values: new object[] { 32, 1 });

            migrationBuilder.UpdateData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 529,
                columns: new[] { "BossId", "SpecializationId" },
                values: new object[] { 32, 2 });

            migrationBuilder.UpdateData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 530,
                columns: new[] { "BossId", "SpecializationId" },
                values: new object[] { 32, 3 });

            migrationBuilder.UpdateData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 531,
                columns: new[] { "BossId", "SpecializationId" },
                values: new object[] { 32, 4 });

            migrationBuilder.UpdateData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 532,
                columns: new[] { "BossId", "SpecializationId" },
                values: new object[] { 32, 5 });

            migrationBuilder.UpdateData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 533,
                columns: new[] { "BossId", "SpecializationId" },
                values: new object[] { 32, 6 });

            migrationBuilder.UpdateData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 534,
                columns: new[] { "BossId", "SpecializationId" },
                values: new object[] { 32, 7 });

            migrationBuilder.UpdateData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 535,
                columns: new[] { "BossId", "SpecializationId" },
                values: new object[] { 32, 8 });

            migrationBuilder.UpdateData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 536,
                columns: new[] { "BossId", "SpecializationId" },
                values: new object[] { 32, 9 });

            migrationBuilder.UpdateData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 537,
                columns: new[] { "BossId", "SpecializationId" },
                values: new object[] { 32, 10 });

            migrationBuilder.UpdateData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 538,
                columns: new[] { "BossId", "SpecializationId" },
                values: new object[] { 32, 11 });

            migrationBuilder.UpdateData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 539,
                columns: new[] { "BossId", "SpecializationId" },
                values: new object[] { 32, 12 });

            migrationBuilder.UpdateData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 540,
                columns: new[] { "BossId", "SpecializationId" },
                values: new object[] { 32, 13 });

            migrationBuilder.UpdateData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 541,
                columns: new[] { "BossId", "SpecializationId" },
                values: new object[] { 32, 14 });

            migrationBuilder.UpdateData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 542,
                columns: new[] { "BossId", "SpecializationId" },
                values: new object[] { 32, 15 });

            migrationBuilder.UpdateData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 543,
                columns: new[] { "BossId", "SpecializationId" },
                values: new object[] { 32, 16 });

            migrationBuilder.UpdateData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 544,
                columns: new[] { "BossId", "SpecializationId" },
                values: new object[] { 32, 17 });

            migrationBuilder.UpdateData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 545,
                columns: new[] { "BossId", "SpecializationId" },
                values: new object[] { 33, 1 });

            migrationBuilder.UpdateData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 546,
                columns: new[] { "BossId", "SpecializationId" },
                values: new object[] { 33, 2 });

            migrationBuilder.UpdateData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 547,
                columns: new[] { "BossId", "SpecializationId" },
                values: new object[] { 33, 3 });

            migrationBuilder.UpdateData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 548,
                columns: new[] { "BossId", "SpecializationId" },
                values: new object[] { 33, 4 });

            migrationBuilder.UpdateData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 549,
                columns: new[] { "BossId", "SpecializationId" },
                values: new object[] { 33, 5 });

            migrationBuilder.UpdateData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 550,
                columns: new[] { "BossId", "SpecializationId" },
                values: new object[] { 33, 6 });

            migrationBuilder.UpdateData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 551,
                columns: new[] { "BossId", "SpecializationId" },
                values: new object[] { 33, 7 });

            migrationBuilder.UpdateData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 552,
                columns: new[] { "BossId", "SpecializationId" },
                values: new object[] { 33, 8 });

            migrationBuilder.UpdateData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 553,
                columns: new[] { "BossId", "SpecializationId" },
                values: new object[] { 33, 9 });

            migrationBuilder.UpdateData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 554,
                columns: new[] { "BossId", "SpecializationId" },
                values: new object[] { 33, 10 });

            migrationBuilder.UpdateData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 555,
                columns: new[] { "BossId", "SpecializationId" },
                values: new object[] { 33, 11 });

            migrationBuilder.UpdateData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 556,
                columns: new[] { "BossId", "SpecializationId" },
                values: new object[] { 33, 12 });

            migrationBuilder.UpdateData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 557,
                columns: new[] { "BossId", "SpecializationId" },
                values: new object[] { 33, 13 });

            migrationBuilder.UpdateData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 558,
                columns: new[] { "BossId", "SpecializationId" },
                values: new object[] { 33, 14 });

            migrationBuilder.UpdateData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 559,
                columns: new[] { "BossId", "SpecializationId" },
                values: new object[] { 33, 15 });

            migrationBuilder.UpdateData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 560,
                columns: new[] { "BossId", "SpecializationId" },
                values: new object[] { 33, 16 });

            migrationBuilder.UpdateData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 561,
                columns: new[] { "BossId", "SpecializationId" },
                values: new object[] { 33, 17 });

            migrationBuilder.UpdateData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 562,
                columns: new[] { "BossId", "SpecializationId" },
                values: new object[] { 34, 1 });

            migrationBuilder.UpdateData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 563,
                columns: new[] { "BossId", "SpecializationId" },
                values: new object[] { 34, 2 });

            migrationBuilder.UpdateData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 564,
                columns: new[] { "BossId", "SpecializationId" },
                values: new object[] { 34, 3 });

            migrationBuilder.UpdateData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 565,
                columns: new[] { "BossId", "SpecializationId" },
                values: new object[] { 34, 4 });

            migrationBuilder.UpdateData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 566,
                columns: new[] { "BossId", "SpecializationId" },
                values: new object[] { 34, 5 });

            migrationBuilder.UpdateData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 567,
                columns: new[] { "BossId", "SpecializationId" },
                values: new object[] { 34, 6 });

            migrationBuilder.UpdateData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 568,
                columns: new[] { "BossId", "SpecializationId" },
                values: new object[] { 34, 7 });

            migrationBuilder.UpdateData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 569,
                columns: new[] { "BossId", "SpecializationId" },
                values: new object[] { 34, 8 });

            migrationBuilder.UpdateData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 570,
                columns: new[] { "BossId", "SpecializationId" },
                values: new object[] { 34, 9 });

            migrationBuilder.UpdateData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 571,
                columns: new[] { "BossId", "SpecializationId" },
                values: new object[] { 34, 10 });

            migrationBuilder.UpdateData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 572,
                columns: new[] { "BossId", "SpecializationId" },
                values: new object[] { 34, 11 });

            migrationBuilder.UpdateData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 573,
                columns: new[] { "BossId", "SpecializationId" },
                values: new object[] { 34, 12 });

            migrationBuilder.UpdateData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 574,
                columns: new[] { "BossId", "SpecializationId" },
                values: new object[] { 34, 13 });

            migrationBuilder.UpdateData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 575,
                columns: new[] { "BossId", "SpecializationId" },
                values: new object[] { 34, 14 });

            migrationBuilder.UpdateData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 576,
                columns: new[] { "BossId", "SpecializationId" },
                values: new object[] { 34, 15 });

            migrationBuilder.UpdateData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 577,
                columns: new[] { "BossId", "SpecializationId" },
                values: new object[] { 34, 16 });

            migrationBuilder.UpdateData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 578,
                columns: new[] { "BossId", "SpecializationId" },
                values: new object[] { 34, 17 });

            migrationBuilder.UpdateData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 579,
                columns: new[] { "BossId", "SpecializationId" },
                values: new object[] { 35, 1 });

            migrationBuilder.UpdateData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 580,
                columns: new[] { "BossId", "SpecializationId" },
                values: new object[] { 35, 2 });

            migrationBuilder.UpdateData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 581,
                columns: new[] { "BossId", "SpecializationId" },
                values: new object[] { 35, 3 });

            migrationBuilder.UpdateData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 582,
                columns: new[] { "BossId", "SpecializationId" },
                values: new object[] { 35, 4 });

            migrationBuilder.UpdateData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 583,
                columns: new[] { "BossId", "SpecializationId" },
                values: new object[] { 35, 5 });

            migrationBuilder.UpdateData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 584,
                columns: new[] { "BossId", "SpecializationId" },
                values: new object[] { 35, 6 });

            migrationBuilder.UpdateData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 585,
                columns: new[] { "BossId", "SpecializationId" },
                values: new object[] { 35, 7 });

            migrationBuilder.UpdateData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 586,
                columns: new[] { "BossId", "SpecializationId" },
                values: new object[] { 35, 8 });

            migrationBuilder.UpdateData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 587,
                columns: new[] { "BossId", "SpecializationId" },
                values: new object[] { 35, 9 });

            migrationBuilder.UpdateData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 588,
                columns: new[] { "BossId", "SpecializationId" },
                values: new object[] { 35, 10 });

            migrationBuilder.UpdateData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 589,
                columns: new[] { "BossId", "SpecializationId" },
                values: new object[] { 35, 11 });

            migrationBuilder.UpdateData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 590,
                columns: new[] { "BossId", "SpecializationId" },
                values: new object[] { 35, 12 });

            migrationBuilder.UpdateData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 591,
                columns: new[] { "BossId", "SpecializationId" },
                values: new object[] { 35, 13 });

            migrationBuilder.UpdateData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 592,
                columns: new[] { "BossId", "SpecializationId" },
                values: new object[] { 35, 14 });

            migrationBuilder.UpdateData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 593,
                columns: new[] { "BossId", "SpecializationId" },
                values: new object[] { 35, 15 });

            migrationBuilder.UpdateData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 594,
                columns: new[] { "BossId", "SpecializationId" },
                values: new object[] { 35, 16 });

            migrationBuilder.UpdateData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 595,
                columns: new[] { "BossId", "SpecializationId" },
                values: new object[] { 35, 17 });

            migrationBuilder.UpdateData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 596,
                columns: new[] { "BossId", "SpecializationId" },
                values: new object[] { 36, 1 });

            migrationBuilder.UpdateData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 597,
                columns: new[] { "BossId", "SpecializationId" },
                values: new object[] { 36, 2 });

            migrationBuilder.UpdateData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 598,
                columns: new[] { "BossId", "SpecializationId" },
                values: new object[] { 36, 3 });

            migrationBuilder.UpdateData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 599,
                columns: new[] { "BossId", "SpecializationId" },
                values: new object[] { 36, 4 });

            migrationBuilder.UpdateData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 600,
                columns: new[] { "BossId", "SpecializationId" },
                values: new object[] { 36, 5 });

            migrationBuilder.UpdateData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 601,
                columns: new[] { "BossId", "SpecializationId" },
                values: new object[] { 36, 6 });

            migrationBuilder.UpdateData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 602,
                columns: new[] { "BossId", "SpecializationId" },
                values: new object[] { 36, 7 });

            migrationBuilder.UpdateData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 603,
                columns: new[] { "BossId", "SpecializationId" },
                values: new object[] { 36, 8 });

            migrationBuilder.UpdateData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 604,
                columns: new[] { "BossId", "SpecializationId" },
                values: new object[] { 36, 9 });

            migrationBuilder.UpdateData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 605,
                columns: new[] { "BossId", "SpecializationId" },
                values: new object[] { 36, 10 });

            migrationBuilder.UpdateData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 606,
                columns: new[] { "BossId", "SpecializationId" },
                values: new object[] { 36, 11 });

            migrationBuilder.UpdateData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 607,
                columns: new[] { "BossId", "SpecializationId" },
                values: new object[] { 36, 12 });

            migrationBuilder.UpdateData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 608,
                columns: new[] { "BossId", "SpecializationId" },
                values: new object[] { 36, 13 });

            migrationBuilder.UpdateData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 609,
                columns: new[] { "BossId", "SpecializationId" },
                values: new object[] { 36, 14 });

            migrationBuilder.UpdateData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 610,
                columns: new[] { "BossId", "SpecializationId" },
                values: new object[] { 36, 15 });

            migrationBuilder.UpdateData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 611,
                columns: new[] { "BossId", "SpecializationId" },
                values: new object[] { 36, 16 });

            migrationBuilder.UpdateData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 612,
                columns: new[] { "BossId", "SpecializationId" },
                values: new object[] { 36, 17 });

            migrationBuilder.UpdateData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 613,
                columns: new[] { "BossId", "SpecializationId" },
                values: new object[] { 37, 1 });

            migrationBuilder.UpdateData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 614,
                columns: new[] { "BossId", "SpecializationId" },
                values: new object[] { 37, 2 });

            migrationBuilder.UpdateData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 615,
                columns: new[] { "BossId", "SpecializationId" },
                values: new object[] { 37, 3 });

            migrationBuilder.UpdateData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 616,
                columns: new[] { "BossId", "SpecializationId" },
                values: new object[] { 37, 4 });

            migrationBuilder.UpdateData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 617,
                columns: new[] { "BossId", "SpecializationId" },
                values: new object[] { 37, 5 });

            migrationBuilder.UpdateData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 618,
                columns: new[] { "BossId", "SpecializationId" },
                values: new object[] { 37, 6 });

            migrationBuilder.UpdateData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 619,
                columns: new[] { "BossId", "SpecializationId" },
                values: new object[] { 37, 7 });

            migrationBuilder.UpdateData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 620,
                columns: new[] { "BossId", "SpecializationId" },
                values: new object[] { 37, 8 });

            migrationBuilder.UpdateData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 621,
                columns: new[] { "BossId", "SpecializationId" },
                values: new object[] { 37, 9 });

            migrationBuilder.UpdateData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 622,
                columns: new[] { "BossId", "SpecializationId" },
                values: new object[] { 37, 10 });

            migrationBuilder.UpdateData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 623,
                columns: new[] { "BossId", "SpecializationId" },
                values: new object[] { 37, 11 });

            migrationBuilder.UpdateData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 624,
                columns: new[] { "BossId", "SpecializationId" },
                values: new object[] { 37, 12 });

            migrationBuilder.UpdateData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 625,
                columns: new[] { "BossId", "SpecializationId" },
                values: new object[] { 37, 13 });

            migrationBuilder.UpdateData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 626,
                columns: new[] { "BossId", "SpecializationId" },
                values: new object[] { 37, 14 });

            migrationBuilder.UpdateData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 627,
                columns: new[] { "BossId", "SpecializationId" },
                values: new object[] { 37, 15 });

            migrationBuilder.UpdateData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 628,
                columns: new[] { "BossId", "SpecializationId" },
                values: new object[] { 37, 16 });

            migrationBuilder.UpdateData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 629,
                columns: new[] { "BossId", "SpecializationId" },
                values: new object[] { 37, 17 });

            migrationBuilder.UpdateData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 630,
                columns: new[] { "BossId", "SpecializationId" },
                values: new object[] { 38, 1 });

            migrationBuilder.UpdateData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 631,
                columns: new[] { "BossId", "SpecializationId" },
                values: new object[] { 38, 2 });

            migrationBuilder.UpdateData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 632,
                columns: new[] { "BossId", "SpecializationId" },
                values: new object[] { 38, 3 });

            migrationBuilder.UpdateData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 633,
                columns: new[] { "BossId", "SpecializationId" },
                values: new object[] { 38, 4 });

            migrationBuilder.UpdateData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 634,
                columns: new[] { "BossId", "SpecializationId" },
                values: new object[] { 38, 5 });

            migrationBuilder.UpdateData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 635,
                columns: new[] { "BossId", "SpecializationId" },
                values: new object[] { 38, 6 });

            migrationBuilder.UpdateData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 636,
                columns: new[] { "BossId", "SpecializationId" },
                values: new object[] { 38, 7 });

            migrationBuilder.UpdateData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 637,
                columns: new[] { "BossId", "SpecializationId" },
                values: new object[] { 38, 8 });

            migrationBuilder.UpdateData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 638,
                columns: new[] { "BossId", "SpecializationId" },
                values: new object[] { 38, 9 });

            migrationBuilder.UpdateData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 639,
                columns: new[] { "BossId", "SpecializationId" },
                values: new object[] { 38, 10 });

            migrationBuilder.UpdateData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 640,
                columns: new[] { "BossId", "SpecializationId" },
                values: new object[] { 38, 11 });

            migrationBuilder.UpdateData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 641,
                columns: new[] { "BossId", "SpecializationId" },
                values: new object[] { 38, 12 });

            migrationBuilder.UpdateData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 642,
                columns: new[] { "BossId", "SpecializationId" },
                values: new object[] { 38, 13 });

            migrationBuilder.UpdateData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 643,
                columns: new[] { "BossId", "SpecializationId" },
                values: new object[] { 38, 14 });

            migrationBuilder.UpdateData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 644,
                columns: new[] { "BossId", "SpecializationId" },
                values: new object[] { 38, 15 });

            migrationBuilder.UpdateData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 645,
                columns: new[] { "BossId", "SpecializationId" },
                values: new object[] { 38, 16 });

            migrationBuilder.UpdateData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 646,
                columns: new[] { "BossId", "SpecializationId" },
                values: new object[] { 38, 17 });

            migrationBuilder.UpdateData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 647,
                columns: new[] { "BossId", "SpecializationId" },
                values: new object[] { 39, 1 });

            migrationBuilder.UpdateData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 648,
                columns: new[] { "BossId", "SpecializationId" },
                values: new object[] { 39, 2 });

            migrationBuilder.UpdateData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 649,
                columns: new[] { "BossId", "SpecializationId" },
                values: new object[] { 39, 3 });

            migrationBuilder.UpdateData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 650,
                columns: new[] { "BossId", "SpecializationId" },
                values: new object[] { 39, 4 });

            migrationBuilder.UpdateData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 651,
                columns: new[] { "BossId", "SpecializationId" },
                values: new object[] { 39, 5 });

            migrationBuilder.UpdateData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 652,
                columns: new[] { "BossId", "SpecializationId" },
                values: new object[] { 39, 6 });

            migrationBuilder.UpdateData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 653,
                columns: new[] { "BossId", "SpecializationId" },
                values: new object[] { 39, 7 });

            migrationBuilder.UpdateData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 654,
                columns: new[] { "BossId", "SpecializationId" },
                values: new object[] { 39, 8 });

            migrationBuilder.UpdateData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 655,
                columns: new[] { "BossId", "SpecializationId" },
                values: new object[] { 39, 9 });

            migrationBuilder.UpdateData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 656,
                columns: new[] { "BossId", "SpecializationId" },
                values: new object[] { 39, 10 });

            migrationBuilder.UpdateData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 657,
                columns: new[] { "BossId", "SpecializationId" },
                values: new object[] { 39, 11 });

            migrationBuilder.UpdateData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 658,
                columns: new[] { "BossId", "SpecializationId" },
                values: new object[] { 39, 12 });

            migrationBuilder.UpdateData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 659,
                columns: new[] { "BossId", "SpecializationId" },
                values: new object[] { 39, 13 });

            migrationBuilder.UpdateData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 660,
                columns: new[] { "BossId", "SpecializationId" },
                values: new object[] { 39, 14 });

            migrationBuilder.UpdateData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 661,
                columns: new[] { "BossId", "SpecializationId" },
                values: new object[] { 39, 15 });

            migrationBuilder.UpdateData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 662,
                columns: new[] { "BossId", "SpecializationId" },
                values: new object[] { 39, 16 });

            migrationBuilder.UpdateData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 663,
                columns: new[] { "BossId", "SpecializationId" },
                values: new object[] { 39, 17 });

            migrationBuilder.UpdateData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 664,
                column: "BossId",
                value: 40);

            migrationBuilder.UpdateData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 665,
                column: "BossId",
                value: 40);

            migrationBuilder.UpdateData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 666,
                column: "BossId",
                value: 40);

            migrationBuilder.UpdateData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 667,
                column: "BossId",
                value: 40);

            migrationBuilder.UpdateData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 668,
                column: "BossId",
                value: 40);

            migrationBuilder.UpdateData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 669,
                column: "BossId",
                value: 40);

            migrationBuilder.UpdateData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 670,
                column: "BossId",
                value: 40);

            migrationBuilder.UpdateData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 671,
                column: "BossId",
                value: 40);

            migrationBuilder.UpdateData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 672,
                column: "BossId",
                value: 40);

            migrationBuilder.UpdateData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 673,
                column: "BossId",
                value: 40);

            migrationBuilder.UpdateData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 674,
                column: "BossId",
                value: 40);

            migrationBuilder.UpdateData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 675,
                column: "BossId",
                value: 40);

            migrationBuilder.UpdateData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 676,
                column: "BossId",
                value: 40);

            migrationBuilder.UpdateData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 677,
                columns: new[] { "BossId", "SpecializationId" },
                values: new object[] { 40, 14 });

            migrationBuilder.UpdateData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 678,
                columns: new[] { "BossId", "SpecializationId" },
                values: new object[] { 40, 15 });

            migrationBuilder.UpdateData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 679,
                columns: new[] { "BossId", "SpecializationId" },
                values: new object[] { 40, 16 });

            migrationBuilder.UpdateData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 680,
                columns: new[] { "BossId", "SpecializationId" },
                values: new object[] { 40, 17 });

            migrationBuilder.UpdateData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 681,
                columns: new[] { "BossId", "SpecializationId" },
                values: new object[] { 41, 1 });

            migrationBuilder.UpdateData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 682,
                columns: new[] { "BossId", "SpecializationId" },
                values: new object[] { 41, 2 });

            migrationBuilder.UpdateData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 683,
                columns: new[] { "BossId", "SpecializationId" },
                values: new object[] { 41, 3 });

            migrationBuilder.UpdateData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 684,
                columns: new[] { "BossId", "SpecializationId" },
                values: new object[] { 41, 4 });

            migrationBuilder.UpdateData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 685,
                columns: new[] { "BossId", "SpecializationId" },
                values: new object[] { 41, 5 });

            migrationBuilder.UpdateData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 686,
                columns: new[] { "BossId", "SpecializationId" },
                values: new object[] { 41, 6 });

            migrationBuilder.UpdateData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 687,
                columns: new[] { "BossId", "SpecializationId" },
                values: new object[] { 41, 7 });

            migrationBuilder.UpdateData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 688,
                columns: new[] { "BossId", "SpecializationId" },
                values: new object[] { 41, 8 });

            migrationBuilder.UpdateData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 689,
                columns: new[] { "BossId", "SpecializationId" },
                values: new object[] { 41, 9 });

            migrationBuilder.UpdateData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 690,
                columns: new[] { "BossId", "SpecializationId" },
                values: new object[] { 41, 10 });

            migrationBuilder.UpdateData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 691,
                columns: new[] { "BossId", "SpecializationId" },
                values: new object[] { 41, 11 });

            migrationBuilder.UpdateData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 692,
                columns: new[] { "BossId", "SpecializationId" },
                values: new object[] { 41, 12 });

            migrationBuilder.UpdateData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 693,
                columns: new[] { "BossId", "SpecializationId" },
                values: new object[] { 41, 13 });

            migrationBuilder.UpdateData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 694,
                columns: new[] { "BossId", "SpecializationId" },
                values: new object[] { 41, 14 });

            migrationBuilder.UpdateData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 695,
                columns: new[] { "BossId", "SpecializationId" },
                values: new object[] { 41, 15 });

            migrationBuilder.UpdateData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 696,
                columns: new[] { "BossId", "SpecializationId" },
                values: new object[] { 41, 16 });

            migrationBuilder.UpdateData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 697,
                columns: new[] { "BossId", "SpecializationId" },
                values: new object[] { 41, 17 });

            migrationBuilder.UpdateData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 698,
                columns: new[] { "BossId", "SpecializationId" },
                values: new object[] { 42, 1 });

            migrationBuilder.UpdateData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 699,
                columns: new[] { "BossId", "SpecializationId" },
                values: new object[] { 42, 2 });

            migrationBuilder.UpdateData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 700,
                columns: new[] { "BossId", "SpecializationId" },
                values: new object[] { 42, 3 });

            migrationBuilder.UpdateData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 701,
                columns: new[] { "BossId", "SpecializationId" },
                values: new object[] { 42, 4 });

            migrationBuilder.UpdateData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 702,
                columns: new[] { "BossId", "SpecializationId" },
                values: new object[] { 42, 5 });

            migrationBuilder.UpdateData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 703,
                columns: new[] { "BossId", "SpecializationId" },
                values: new object[] { 42, 6 });

            migrationBuilder.UpdateData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 704,
                columns: new[] { "BossId", "SpecializationId" },
                values: new object[] { 42, 7 });

            migrationBuilder.UpdateData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 705,
                columns: new[] { "BossId", "SpecializationId" },
                values: new object[] { 42, 8 });

            migrationBuilder.UpdateData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 706,
                columns: new[] { "BossId", "SpecializationId" },
                values: new object[] { 42, 9 });

            migrationBuilder.UpdateData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 707,
                columns: new[] { "BossId", "SpecializationId" },
                values: new object[] { 42, 10 });

            migrationBuilder.UpdateData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 708,
                columns: new[] { "BossId", "SpecializationId" },
                values: new object[] { 42, 11 });

            migrationBuilder.UpdateData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 709,
                columns: new[] { "BossId", "SpecializationId" },
                values: new object[] { 42, 12 });

            migrationBuilder.UpdateData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 710,
                columns: new[] { "BossId", "SpecializationId" },
                values: new object[] { 42, 13 });

            migrationBuilder.UpdateData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 711,
                columns: new[] { "BossId", "SpecializationId" },
                values: new object[] { 42, 14 });

            migrationBuilder.UpdateData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 712,
                columns: new[] { "BossId", "SpecializationId" },
                values: new object[] { 42, 15 });

            migrationBuilder.UpdateData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 713,
                columns: new[] { "BossId", "SpecializationId" },
                values: new object[] { 42, 16 });

            migrationBuilder.UpdateData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 714,
                columns: new[] { "BossId", "SpecializationId" },
                values: new object[] { 42, 17 });

            migrationBuilder.UpdateData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 715,
                columns: new[] { "BossId", "SpecializationId" },
                values: new object[] { 43, 1 });

            migrationBuilder.UpdateData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 716,
                columns: new[] { "BossId", "SpecializationId" },
                values: new object[] { 43, 2 });

            migrationBuilder.UpdateData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 717,
                columns: new[] { "BossId", "SpecializationId" },
                values: new object[] { 43, 3 });

            migrationBuilder.UpdateData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 718,
                columns: new[] { "BossId", "SpecializationId" },
                values: new object[] { 43, 4 });

            migrationBuilder.UpdateData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 719,
                columns: new[] { "BossId", "SpecializationId" },
                values: new object[] { 43, 5 });

            migrationBuilder.UpdateData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 720,
                columns: new[] { "BossId", "SpecializationId" },
                values: new object[] { 43, 6 });

            migrationBuilder.UpdateData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 721,
                columns: new[] { "BossId", "SpecializationId" },
                values: new object[] { 43, 7 });

            migrationBuilder.UpdateData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 722,
                columns: new[] { "BossId", "SpecializationId" },
                values: new object[] { 43, 8 });

            migrationBuilder.UpdateData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 723,
                columns: new[] { "BossId", "SpecializationId" },
                values: new object[] { 43, 9 });

            migrationBuilder.UpdateData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 724,
                columns: new[] { "BossId", "SpecializationId" },
                values: new object[] { 43, 10 });

            migrationBuilder.UpdateData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 725,
                columns: new[] { "BossId", "SpecializationId" },
                values: new object[] { 43, 11 });

            migrationBuilder.UpdateData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 726,
                columns: new[] { "BossId", "SpecializationId" },
                values: new object[] { 43, 12 });

            migrationBuilder.UpdateData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 727,
                columns: new[] { "BossId", "SpecializationId" },
                values: new object[] { 43, 13 });

            migrationBuilder.UpdateData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 728,
                columns: new[] { "BossId", "SpecializationId" },
                values: new object[] { 43, 14 });

            migrationBuilder.InsertData(
                table: "BestSpecializationScore",
                columns: new[] { "Id", "BossId", "DamageDone", "HealDone", "SpecializationId", "Updated" },
                values: new object[,]
                {
                    { 732, 44, 0, 0, 1, null },
                    { 733, 44, 0, 0, 2, null },
                    { 734, 44, 0, 0, 3, null },
                    { 735, 44, 0, 0, 4, null },
                    { 736, 44, 0, 0, 5, null },
                    { 737, 44, 0, 0, 6, null },
                    { 738, 44, 0, 0, 7, null },
                    { 739, 44, 0, 0, 8, null },
                    { 740, 44, 0, 0, 9, null },
                    { 741, 44, 0, 0, 10, null },
                    { 742, 44, 0, 0, 11, null },
                    { 743, 44, 0, 0, 12, null },
                    { 744, 44, 0, 0, 13, null },
                    { 749, 45, 0, 0, 1, null },
                    { 750, 45, 0, 0, 2, null },
                    { 751, 45, 0, 0, 3, null },
                    { 752, 45, 0, 0, 4, null },
                    { 753, 45, 0, 0, 5, null },
                    { 754, 45, 0, 0, 6, null },
                    { 755, 45, 0, 0, 7, null },
                    { 756, 45, 0, 0, 8, null },
                    { 757, 45, 0, 0, 9, null },
                    { 758, 45, 0, 0, 10, null },
                    { 759, 45, 0, 0, 11, null },
                    { 760, 45, 0, 0, 12, null },
                    { 761, 45, 0, 0, 13, null },
                    { 766, 46, 0, 0, 1, null },
                    { 767, 46, 0, 0, 2, null },
                    { 768, 46, 0, 0, 3, null },
                    { 769, 46, 0, 0, 4, null },
                    { 770, 46, 0, 0, 5, null },
                    { 771, 46, 0, 0, 6, null },
                    { 772, 46, 0, 0, 7, null },
                    { 773, 46, 0, 0, 8, null },
                    { 774, 46, 0, 0, 9, null },
                    { 775, 46, 0, 0, 10, null },
                    { 776, 46, 0, 0, 11, null },
                    { 777, 46, 0, 0, 12, null },
                    { 778, 46, 0, 0, 13, null },
                    { 783, 47, 0, 0, 1, null },
                    { 784, 47, 0, 0, 2, null },
                    { 785, 47, 0, 0, 3, null },
                    { 786, 47, 0, 0, 4, null },
                    { 787, 47, 0, 0, 5, null },
                    { 788, 47, 0, 0, 6, null },
                    { 789, 47, 0, 0, 7, null },
                    { 790, 47, 0, 0, 8, null },
                    { 791, 47, 0, 0, 9, null },
                    { 792, 47, 0, 0, 10, null },
                    { 793, 47, 0, 0, 11, null },
                    { 794, 47, 0, 0, 12, null },
                    { 795, 47, 0, 0, 13, null },
                    { 800, 48, 0, 0, 1, null },
                    { 801, 48, 0, 0, 2, null },
                    { 802, 48, 0, 0, 3, null },
                    { 803, 48, 0, 0, 4, null },
                    { 804, 48, 0, 0, 5, null },
                    { 805, 48, 0, 0, 6, null },
                    { 806, 48, 0, 0, 7, null },
                    { 807, 48, 0, 0, 8, null },
                    { 808, 48, 0, 0, 9, null },
                    { 809, 48, 0, 0, 10, null },
                    { 810, 48, 0, 0, 11, null },
                    { 811, 48, 0, 0, 12, null },
                    { 812, 48, 0, 0, 13, null },
                    { 817, 49, 0, 0, 1, null },
                    { 818, 49, 0, 0, 2, null },
                    { 819, 49, 0, 0, 3, null },
                    { 820, 49, 0, 0, 4, null },
                    { 821, 49, 0, 0, 5, null },
                    { 822, 49, 0, 0, 6, null },
                    { 823, 49, 0, 0, 7, null },
                    { 824, 49, 0, 0, 8, null },
                    { 825, 49, 0, 0, 9, null },
                    { 826, 49, 0, 0, 10, null },
                    { 827, 49, 0, 0, 11, null },
                    { 828, 49, 0, 0, 12, null },
                    { 829, 49, 0, 0, 13, null },
                    { 834, 50, 0, 0, 1, null },
                    { 835, 50, 0, 0, 2, null },
                    { 836, 50, 0, 0, 3, null },
                    { 837, 50, 0, 0, 4, null },
                    { 838, 50, 0, 0, 5, null },
                    { 839, 50, 0, 0, 6, null },
                    { 840, 50, 0, 0, 7, null },
                    { 841, 50, 0, 0, 8, null },
                    { 842, 50, 0, 0, 9, null },
                    { 843, 50, 0, 0, 10, null },
                    { 844, 50, 0, 0, 11, null },
                    { 845, 50, 0, 0, 12, null },
                    { 846, 50, 0, 0, 13, null },
                    { 851, 51, 0, 0, 1, null },
                    { 852, 51, 0, 0, 2, null },
                    { 853, 51, 0, 0, 3, null },
                    { 854, 51, 0, 0, 4, null },
                    { 855, 51, 0, 0, 5, null },
                    { 856, 51, 0, 0, 6, null },
                    { 857, 51, 0, 0, 7, null },
                    { 858, 51, 0, 0, 8, null },
                    { 859, 51, 0, 0, 9, null },
                    { 860, 51, 0, 0, 10, null },
                    { 861, 51, 0, 0, 11, null },
                    { 862, 51, 0, 0, 12, null },
                    { 863, 51, 0, 0, 13, null },
                    { 868, 52, 0, 0, 1, null },
                    { 869, 52, 0, 0, 2, null },
                    { 870, 52, 0, 0, 3, null },
                    { 871, 52, 0, 0, 4, null },
                    { 872, 52, 0, 0, 5, null },
                    { 873, 52, 0, 0, 6, null },
                    { 874, 52, 0, 0, 7, null },
                    { 875, 52, 0, 0, 8, null },
                    { 876, 52, 0, 0, 9, null },
                    { 877, 52, 0, 0, 10, null },
                    { 878, 52, 0, 0, 11, null },
                    { 879, 52, 0, 0, 12, null },
                    { 880, 52, 0, 0, 13, null },
                    { 885, 53, 0, 0, 1, null },
                    { 886, 53, 0, 0, 2, null },
                    { 887, 53, 0, 0, 3, null },
                    { 888, 53, 0, 0, 4, null },
                    { 889, 53, 0, 0, 5, null },
                    { 890, 53, 0, 0, 6, null },
                    { 891, 53, 0, 0, 7, null },
                    { 892, 53, 0, 0, 8, null },
                    { 893, 53, 0, 0, 9, null },
                    { 894, 53, 0, 0, 10, null },
                    { 895, 53, 0, 0, 11, null },
                    { 896, 53, 0, 0, 12, null },
                    { 897, 53, 0, 0, 13, null },
                    { 902, 54, 0, 0, 1, null },
                    { 903, 54, 0, 0, 2, null },
                    { 904, 54, 0, 0, 3, null },
                    { 905, 54, 0, 0, 4, null },
                    { 906, 54, 0, 0, 5, null },
                    { 907, 54, 0, 0, 6, null },
                    { 908, 54, 0, 0, 7, null },
                    { 909, 54, 0, 0, 8, null },
                    { 910, 54, 0, 0, 9, null },
                    { 911, 54, 0, 0, 10, null },
                    { 912, 54, 0, 0, 11, null },
                    { 913, 54, 0, 0, 12, null },
                    { 914, 54, 0, 0, 13, null },
                    { 919, 55, 0, 0, 1, null },
                    { 920, 55, 0, 0, 2, null },
                    { 921, 55, 0, 0, 3, null },
                    { 922, 55, 0, 0, 4, null },
                    { 923, 55, 0, 0, 5, null },
                    { 924, 55, 0, 0, 6, null },
                    { 925, 55, 0, 0, 7, null },
                    { 926, 55, 0, 0, 8, null },
                    { 927, 55, 0, 0, 9, null },
                    { 928, 55, 0, 0, 10, null },
                    { 929, 55, 0, 0, 11, null },
                    { 930, 55, 0, 0, 12, null },
                    { 931, 55, 0, 0, 13, null },
                    { 936, 56, 0, 0, 1, null },
                    { 937, 56, 0, 0, 2, null },
                    { 938, 56, 0, 0, 3, null },
                    { 939, 56, 0, 0, 4, null },
                    { 940, 56, 0, 0, 5, null },
                    { 941, 56, 0, 0, 6, null },
                    { 942, 56, 0, 0, 7, null },
                    { 943, 56, 0, 0, 8, null },
                    { 944, 56, 0, 0, 9, null },
                    { 945, 56, 0, 0, 10, null },
                    { 946, 56, 0, 0, 11, null },
                    { 947, 56, 0, 0, 12, null },
                    { 948, 56, 0, 0, 13, null }
                });

            migrationBuilder.UpdateData(
                table: "Specialization",
                keyColumn: "Id",
                keyValue: 7,
                column: "Name",
                value: "ProtectionWarrior");

            migrationBuilder.UpdateData(
                table: "Specialization",
                keyColumn: "Id",
                keyValue: 9,
                column: "SpecializationSpellsId",
                value: "47750,81751,585");

            migrationBuilder.InsertData(
                table: "Specialization",
                columns: new[] { "Id", "Name", "SpecializationSpellsId" },
                values: new object[,]
                {
                    { 14, "HolyPaladin", "82327,85222,25914" },
                    { 15, "ProtectionPaladin", "31935,53600,20271" },
                    { 16, "Elemental", "51505,403,8050" },
                    { 17, "Frost", "116,44614,30455" }
                });

            migrationBuilder.InsertData(
                table: "BestSpecializationScore",
                columns: new[] { "Id", "BossId", "DamageDone", "HealDone", "SpecializationId", "Updated" },
                values: new object[,]
                {
                    { 729, 43, 0, 0, 15, null },
                    { 730, 43, 0, 0, 16, null },
                    { 731, 43, 0, 0, 17, null },
                    { 745, 44, 0, 0, 14, null },
                    { 746, 44, 0, 0, 15, null },
                    { 747, 44, 0, 0, 16, null },
                    { 748, 44, 0, 0, 17, null },
                    { 762, 45, 0, 0, 14, null },
                    { 763, 45, 0, 0, 15, null },
                    { 764, 45, 0, 0, 16, null },
                    { 765, 45, 0, 0, 17, null },
                    { 779, 46, 0, 0, 14, null },
                    { 780, 46, 0, 0, 15, null },
                    { 781, 46, 0, 0, 16, null },
                    { 782, 46, 0, 0, 17, null },
                    { 796, 47, 0, 0, 14, null },
                    { 797, 47, 0, 0, 15, null },
                    { 798, 47, 0, 0, 16, null },
                    { 799, 47, 0, 0, 17, null },
                    { 813, 48, 0, 0, 14, null },
                    { 814, 48, 0, 0, 15, null },
                    { 815, 48, 0, 0, 16, null },
                    { 816, 48, 0, 0, 17, null },
                    { 830, 49, 0, 0, 14, null },
                    { 831, 49, 0, 0, 15, null },
                    { 832, 49, 0, 0, 16, null },
                    { 833, 49, 0, 0, 17, null },
                    { 847, 50, 0, 0, 14, null },
                    { 848, 50, 0, 0, 15, null },
                    { 849, 50, 0, 0, 16, null },
                    { 850, 50, 0, 0, 17, null },
                    { 864, 51, 0, 0, 14, null },
                    { 865, 51, 0, 0, 15, null },
                    { 866, 51, 0, 0, 16, null },
                    { 867, 51, 0, 0, 17, null },
                    { 881, 52, 0, 0, 14, null },
                    { 882, 52, 0, 0, 15, null },
                    { 883, 52, 0, 0, 16, null },
                    { 884, 52, 0, 0, 17, null },
                    { 898, 53, 0, 0, 14, null },
                    { 899, 53, 0, 0, 15, null },
                    { 900, 53, 0, 0, 16, null },
                    { 901, 53, 0, 0, 17, null },
                    { 915, 54, 0, 0, 14, null },
                    { 916, 54, 0, 0, 15, null },
                    { 917, 54, 0, 0, 16, null },
                    { 918, 54, 0, 0, 17, null },
                    { 932, 55, 0, 0, 14, null },
                    { 933, 55, 0, 0, 15, null },
                    { 934, 55, 0, 0, 16, null },
                    { 935, 55, 0, 0, 17, null },
                    { 949, 56, 0, 0, 14, null },
                    { 950, 56, 0, 0, 15, null },
                    { 951, 56, 0, 0, 16, null },
                    { 952, 56, 0, 0, 17, null }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 729);

            migrationBuilder.DeleteData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 730);

            migrationBuilder.DeleteData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 731);

            migrationBuilder.DeleteData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 732);

            migrationBuilder.DeleteData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 733);

            migrationBuilder.DeleteData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 734);

            migrationBuilder.DeleteData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 735);

            migrationBuilder.DeleteData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 736);

            migrationBuilder.DeleteData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 737);

            migrationBuilder.DeleteData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 738);

            migrationBuilder.DeleteData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 739);

            migrationBuilder.DeleteData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 740);

            migrationBuilder.DeleteData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 741);

            migrationBuilder.DeleteData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 742);

            migrationBuilder.DeleteData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 743);

            migrationBuilder.DeleteData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 744);

            migrationBuilder.DeleteData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 745);

            migrationBuilder.DeleteData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 746);

            migrationBuilder.DeleteData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 747);

            migrationBuilder.DeleteData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 748);

            migrationBuilder.DeleteData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 749);

            migrationBuilder.DeleteData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 750);

            migrationBuilder.DeleteData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 751);

            migrationBuilder.DeleteData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 752);

            migrationBuilder.DeleteData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 753);

            migrationBuilder.DeleteData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 754);

            migrationBuilder.DeleteData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 755);

            migrationBuilder.DeleteData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 756);

            migrationBuilder.DeleteData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 757);

            migrationBuilder.DeleteData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 758);

            migrationBuilder.DeleteData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 759);

            migrationBuilder.DeleteData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 760);

            migrationBuilder.DeleteData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 761);

            migrationBuilder.DeleteData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 762);

            migrationBuilder.DeleteData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 763);

            migrationBuilder.DeleteData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 764);

            migrationBuilder.DeleteData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 765);

            migrationBuilder.DeleteData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 766);

            migrationBuilder.DeleteData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 767);

            migrationBuilder.DeleteData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 768);

            migrationBuilder.DeleteData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 769);

            migrationBuilder.DeleteData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 770);

            migrationBuilder.DeleteData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 771);

            migrationBuilder.DeleteData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 772);

            migrationBuilder.DeleteData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 773);

            migrationBuilder.DeleteData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 774);

            migrationBuilder.DeleteData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 775);

            migrationBuilder.DeleteData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 776);

            migrationBuilder.DeleteData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 777);

            migrationBuilder.DeleteData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 778);

            migrationBuilder.DeleteData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 779);

            migrationBuilder.DeleteData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 780);

            migrationBuilder.DeleteData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 781);

            migrationBuilder.DeleteData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 782);

            migrationBuilder.DeleteData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 783);

            migrationBuilder.DeleteData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 784);

            migrationBuilder.DeleteData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 785);

            migrationBuilder.DeleteData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 786);

            migrationBuilder.DeleteData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 787);

            migrationBuilder.DeleteData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 788);

            migrationBuilder.DeleteData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 789);

            migrationBuilder.DeleteData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 790);

            migrationBuilder.DeleteData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 791);

            migrationBuilder.DeleteData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 792);

            migrationBuilder.DeleteData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 793);

            migrationBuilder.DeleteData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 794);

            migrationBuilder.DeleteData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 795);

            migrationBuilder.DeleteData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 796);

            migrationBuilder.DeleteData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 797);

            migrationBuilder.DeleteData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 798);

            migrationBuilder.DeleteData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 799);

            migrationBuilder.DeleteData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 800);

            migrationBuilder.DeleteData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 801);

            migrationBuilder.DeleteData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 802);

            migrationBuilder.DeleteData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 803);

            migrationBuilder.DeleteData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 804);

            migrationBuilder.DeleteData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 805);

            migrationBuilder.DeleteData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 806);

            migrationBuilder.DeleteData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 807);

            migrationBuilder.DeleteData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 808);

            migrationBuilder.DeleteData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 809);

            migrationBuilder.DeleteData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 810);

            migrationBuilder.DeleteData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 811);

            migrationBuilder.DeleteData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 812);

            migrationBuilder.DeleteData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 813);

            migrationBuilder.DeleteData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 814);

            migrationBuilder.DeleteData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 815);

            migrationBuilder.DeleteData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 816);

            migrationBuilder.DeleteData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 817);

            migrationBuilder.DeleteData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 818);

            migrationBuilder.DeleteData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 819);

            migrationBuilder.DeleteData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 820);

            migrationBuilder.DeleteData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 821);

            migrationBuilder.DeleteData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 822);

            migrationBuilder.DeleteData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 823);

            migrationBuilder.DeleteData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 824);

            migrationBuilder.DeleteData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 825);

            migrationBuilder.DeleteData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 826);

            migrationBuilder.DeleteData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 827);

            migrationBuilder.DeleteData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 828);

            migrationBuilder.DeleteData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 829);

            migrationBuilder.DeleteData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 830);

            migrationBuilder.DeleteData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 831);

            migrationBuilder.DeleteData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 832);

            migrationBuilder.DeleteData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 833);

            migrationBuilder.DeleteData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 834);

            migrationBuilder.DeleteData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 835);

            migrationBuilder.DeleteData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 836);

            migrationBuilder.DeleteData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 837);

            migrationBuilder.DeleteData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 838);

            migrationBuilder.DeleteData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 839);

            migrationBuilder.DeleteData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 840);

            migrationBuilder.DeleteData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 841);

            migrationBuilder.DeleteData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 842);

            migrationBuilder.DeleteData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 843);

            migrationBuilder.DeleteData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 844);

            migrationBuilder.DeleteData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 845);

            migrationBuilder.DeleteData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 846);

            migrationBuilder.DeleteData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 847);

            migrationBuilder.DeleteData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 848);

            migrationBuilder.DeleteData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 849);

            migrationBuilder.DeleteData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 850);

            migrationBuilder.DeleteData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 851);

            migrationBuilder.DeleteData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 852);

            migrationBuilder.DeleteData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 853);

            migrationBuilder.DeleteData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 854);

            migrationBuilder.DeleteData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 855);

            migrationBuilder.DeleteData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 856);

            migrationBuilder.DeleteData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 857);

            migrationBuilder.DeleteData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 858);

            migrationBuilder.DeleteData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 859);

            migrationBuilder.DeleteData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 860);

            migrationBuilder.DeleteData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 861);

            migrationBuilder.DeleteData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 862);

            migrationBuilder.DeleteData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 863);

            migrationBuilder.DeleteData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 864);

            migrationBuilder.DeleteData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 865);

            migrationBuilder.DeleteData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 866);

            migrationBuilder.DeleteData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 867);

            migrationBuilder.DeleteData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 868);

            migrationBuilder.DeleteData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 869);

            migrationBuilder.DeleteData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 870);

            migrationBuilder.DeleteData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 871);

            migrationBuilder.DeleteData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 872);

            migrationBuilder.DeleteData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 873);

            migrationBuilder.DeleteData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 874);

            migrationBuilder.DeleteData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 875);

            migrationBuilder.DeleteData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 876);

            migrationBuilder.DeleteData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 877);

            migrationBuilder.DeleteData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 878);

            migrationBuilder.DeleteData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 879);

            migrationBuilder.DeleteData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 880);

            migrationBuilder.DeleteData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 881);

            migrationBuilder.DeleteData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 882);

            migrationBuilder.DeleteData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 883);

            migrationBuilder.DeleteData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 884);

            migrationBuilder.DeleteData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 885);

            migrationBuilder.DeleteData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 886);

            migrationBuilder.DeleteData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 887);

            migrationBuilder.DeleteData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 888);

            migrationBuilder.DeleteData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 889);

            migrationBuilder.DeleteData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 890);

            migrationBuilder.DeleteData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 891);

            migrationBuilder.DeleteData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 892);

            migrationBuilder.DeleteData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 893);

            migrationBuilder.DeleteData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 894);

            migrationBuilder.DeleteData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 895);

            migrationBuilder.DeleteData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 896);

            migrationBuilder.DeleteData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 897);

            migrationBuilder.DeleteData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 898);

            migrationBuilder.DeleteData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 899);

            migrationBuilder.DeleteData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 900);

            migrationBuilder.DeleteData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 901);

            migrationBuilder.DeleteData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 902);

            migrationBuilder.DeleteData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 903);

            migrationBuilder.DeleteData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 904);

            migrationBuilder.DeleteData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 905);

            migrationBuilder.DeleteData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 906);

            migrationBuilder.DeleteData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 907);

            migrationBuilder.DeleteData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 908);

            migrationBuilder.DeleteData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 909);

            migrationBuilder.DeleteData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 910);

            migrationBuilder.DeleteData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 911);

            migrationBuilder.DeleteData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 912);

            migrationBuilder.DeleteData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 913);

            migrationBuilder.DeleteData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 914);

            migrationBuilder.DeleteData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 915);

            migrationBuilder.DeleteData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 916);

            migrationBuilder.DeleteData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 917);

            migrationBuilder.DeleteData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 918);

            migrationBuilder.DeleteData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 919);

            migrationBuilder.DeleteData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 920);

            migrationBuilder.DeleteData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 921);

            migrationBuilder.DeleteData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 922);

            migrationBuilder.DeleteData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 923);

            migrationBuilder.DeleteData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 924);

            migrationBuilder.DeleteData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 925);

            migrationBuilder.DeleteData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 926);

            migrationBuilder.DeleteData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 927);

            migrationBuilder.DeleteData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 928);

            migrationBuilder.DeleteData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 929);

            migrationBuilder.DeleteData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 930);

            migrationBuilder.DeleteData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 931);

            migrationBuilder.DeleteData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 932);

            migrationBuilder.DeleteData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 933);

            migrationBuilder.DeleteData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 934);

            migrationBuilder.DeleteData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 935);

            migrationBuilder.DeleteData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 936);

            migrationBuilder.DeleteData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 937);

            migrationBuilder.DeleteData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 938);

            migrationBuilder.DeleteData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 939);

            migrationBuilder.DeleteData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 940);

            migrationBuilder.DeleteData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 941);

            migrationBuilder.DeleteData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 942);

            migrationBuilder.DeleteData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 943);

            migrationBuilder.DeleteData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 944);

            migrationBuilder.DeleteData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 945);

            migrationBuilder.DeleteData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 946);

            migrationBuilder.DeleteData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 947);

            migrationBuilder.DeleteData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 948);

            migrationBuilder.DeleteData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 949);

            migrationBuilder.DeleteData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 950);

            migrationBuilder.DeleteData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 951);

            migrationBuilder.DeleteData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 952);

            migrationBuilder.DeleteData(
                table: "Specialization",
                keyColumn: "Id",
                keyValue: 14);

            migrationBuilder.DeleteData(
                table: "Specialization",
                keyColumn: "Id",
                keyValue: 15);

            migrationBuilder.DeleteData(
                table: "Specialization",
                keyColumn: "Id",
                keyValue: 16);

            migrationBuilder.DeleteData(
                table: "Specialization",
                keyColumn: "Id",
                keyValue: 17);

            migrationBuilder.AddColumn<bool>(
                name: "IsReady",
                table: "Combat",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.UpdateData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 14,
                columns: new[] { "BossId", "SpecializationId" },
                values: new object[] { 2, 1 });

            migrationBuilder.UpdateData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 15,
                columns: new[] { "BossId", "SpecializationId" },
                values: new object[] { 2, 2 });

            migrationBuilder.UpdateData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 16,
                columns: new[] { "BossId", "SpecializationId" },
                values: new object[] { 2, 3 });

            migrationBuilder.UpdateData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 17,
                columns: new[] { "BossId", "SpecializationId" },
                values: new object[] { 2, 4 });

            migrationBuilder.UpdateData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 18,
                column: "SpecializationId",
                value: 5);

            migrationBuilder.UpdateData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 19,
                column: "SpecializationId",
                value: 6);

            migrationBuilder.UpdateData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 20,
                column: "SpecializationId",
                value: 7);

            migrationBuilder.UpdateData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 21,
                column: "SpecializationId",
                value: 8);

            migrationBuilder.UpdateData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 22,
                column: "SpecializationId",
                value: 9);

            migrationBuilder.UpdateData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 23,
                column: "SpecializationId",
                value: 10);

            migrationBuilder.UpdateData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 24,
                column: "SpecializationId",
                value: 11);

            migrationBuilder.UpdateData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 25,
                column: "SpecializationId",
                value: 12);

            migrationBuilder.UpdateData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 26,
                column: "SpecializationId",
                value: 13);

            migrationBuilder.UpdateData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 27,
                columns: new[] { "BossId", "SpecializationId" },
                values: new object[] { 3, 1 });

            migrationBuilder.UpdateData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 28,
                columns: new[] { "BossId", "SpecializationId" },
                values: new object[] { 3, 2 });

            migrationBuilder.UpdateData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 29,
                columns: new[] { "BossId", "SpecializationId" },
                values: new object[] { 3, 3 });

            migrationBuilder.UpdateData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 30,
                columns: new[] { "BossId", "SpecializationId" },
                values: new object[] { 3, 4 });

            migrationBuilder.UpdateData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 31,
                columns: new[] { "BossId", "SpecializationId" },
                values: new object[] { 3, 5 });

            migrationBuilder.UpdateData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 32,
                columns: new[] { "BossId", "SpecializationId" },
                values: new object[] { 3, 6 });

            migrationBuilder.UpdateData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 33,
                columns: new[] { "BossId", "SpecializationId" },
                values: new object[] { 3, 7 });

            migrationBuilder.UpdateData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 34,
                columns: new[] { "BossId", "SpecializationId" },
                values: new object[] { 3, 8 });

            migrationBuilder.UpdateData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 35,
                column: "SpecializationId",
                value: 9);

            migrationBuilder.UpdateData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 36,
                column: "SpecializationId",
                value: 10);

            migrationBuilder.UpdateData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 37,
                column: "SpecializationId",
                value: 11);

            migrationBuilder.UpdateData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 38,
                column: "SpecializationId",
                value: 12);

            migrationBuilder.UpdateData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 39,
                column: "SpecializationId",
                value: 13);

            migrationBuilder.UpdateData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 40,
                columns: new[] { "BossId", "SpecializationId" },
                values: new object[] { 4, 1 });

            migrationBuilder.UpdateData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 41,
                columns: new[] { "BossId", "SpecializationId" },
                values: new object[] { 4, 2 });

            migrationBuilder.UpdateData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 42,
                columns: new[] { "BossId", "SpecializationId" },
                values: new object[] { 4, 3 });

            migrationBuilder.UpdateData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 43,
                columns: new[] { "BossId", "SpecializationId" },
                values: new object[] { 4, 4 });

            migrationBuilder.UpdateData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 44,
                columns: new[] { "BossId", "SpecializationId" },
                values: new object[] { 4, 5 });

            migrationBuilder.UpdateData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 45,
                columns: new[] { "BossId", "SpecializationId" },
                values: new object[] { 4, 6 });

            migrationBuilder.UpdateData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 46,
                columns: new[] { "BossId", "SpecializationId" },
                values: new object[] { 4, 7 });

            migrationBuilder.UpdateData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 47,
                columns: new[] { "BossId", "SpecializationId" },
                values: new object[] { 4, 8 });

            migrationBuilder.UpdateData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 48,
                columns: new[] { "BossId", "SpecializationId" },
                values: new object[] { 4, 9 });

            migrationBuilder.UpdateData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 49,
                columns: new[] { "BossId", "SpecializationId" },
                values: new object[] { 4, 10 });

            migrationBuilder.UpdateData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 50,
                columns: new[] { "BossId", "SpecializationId" },
                values: new object[] { 4, 11 });

            migrationBuilder.UpdateData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 51,
                columns: new[] { "BossId", "SpecializationId" },
                values: new object[] { 4, 12 });

            migrationBuilder.UpdateData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 52,
                column: "SpecializationId",
                value: 13);

            migrationBuilder.UpdateData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 53,
                columns: new[] { "BossId", "SpecializationId" },
                values: new object[] { 5, 1 });

            migrationBuilder.UpdateData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 54,
                columns: new[] { "BossId", "SpecializationId" },
                values: new object[] { 5, 2 });

            migrationBuilder.UpdateData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 55,
                columns: new[] { "BossId", "SpecializationId" },
                values: new object[] { 5, 3 });

            migrationBuilder.UpdateData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 56,
                columns: new[] { "BossId", "SpecializationId" },
                values: new object[] { 5, 4 });

            migrationBuilder.UpdateData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 57,
                columns: new[] { "BossId", "SpecializationId" },
                values: new object[] { 5, 5 });

            migrationBuilder.UpdateData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 58,
                columns: new[] { "BossId", "SpecializationId" },
                values: new object[] { 5, 6 });

            migrationBuilder.UpdateData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 59,
                columns: new[] { "BossId", "SpecializationId" },
                values: new object[] { 5, 7 });

            migrationBuilder.UpdateData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 60,
                columns: new[] { "BossId", "SpecializationId" },
                values: new object[] { 5, 8 });

            migrationBuilder.UpdateData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 61,
                columns: new[] { "BossId", "SpecializationId" },
                values: new object[] { 5, 9 });

            migrationBuilder.UpdateData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 62,
                columns: new[] { "BossId", "SpecializationId" },
                values: new object[] { 5, 10 });

            migrationBuilder.UpdateData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 63,
                columns: new[] { "BossId", "SpecializationId" },
                values: new object[] { 5, 11 });

            migrationBuilder.UpdateData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 64,
                columns: new[] { "BossId", "SpecializationId" },
                values: new object[] { 5, 12 });

            migrationBuilder.UpdateData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 65,
                columns: new[] { "BossId", "SpecializationId" },
                values: new object[] { 5, 13 });

            migrationBuilder.UpdateData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 66,
                columns: new[] { "BossId", "SpecializationId" },
                values: new object[] { 6, 1 });

            migrationBuilder.UpdateData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 67,
                columns: new[] { "BossId", "SpecializationId" },
                values: new object[] { 6, 2 });

            migrationBuilder.UpdateData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 68,
                columns: new[] { "BossId", "SpecializationId" },
                values: new object[] { 6, 3 });

            migrationBuilder.UpdateData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 69,
                columns: new[] { "BossId", "SpecializationId" },
                values: new object[] { 6, 4 });

            migrationBuilder.UpdateData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 70,
                columns: new[] { "BossId", "SpecializationId" },
                values: new object[] { 6, 5 });

            migrationBuilder.UpdateData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 71,
                columns: new[] { "BossId", "SpecializationId" },
                values: new object[] { 6, 6 });

            migrationBuilder.UpdateData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 72,
                columns: new[] { "BossId", "SpecializationId" },
                values: new object[] { 6, 7 });

            migrationBuilder.UpdateData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 73,
                columns: new[] { "BossId", "SpecializationId" },
                values: new object[] { 6, 8 });

            migrationBuilder.UpdateData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 74,
                columns: new[] { "BossId", "SpecializationId" },
                values: new object[] { 6, 9 });

            migrationBuilder.UpdateData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 75,
                columns: new[] { "BossId", "SpecializationId" },
                values: new object[] { 6, 10 });

            migrationBuilder.UpdateData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 76,
                columns: new[] { "BossId", "SpecializationId" },
                values: new object[] { 6, 11 });

            migrationBuilder.UpdateData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 77,
                columns: new[] { "BossId", "SpecializationId" },
                values: new object[] { 6, 12 });

            migrationBuilder.UpdateData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 78,
                columns: new[] { "BossId", "SpecializationId" },
                values: new object[] { 6, 13 });

            migrationBuilder.UpdateData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 79,
                columns: new[] { "BossId", "SpecializationId" },
                values: new object[] { 7, 1 });

            migrationBuilder.UpdateData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 80,
                columns: new[] { "BossId", "SpecializationId" },
                values: new object[] { 7, 2 });

            migrationBuilder.UpdateData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 81,
                columns: new[] { "BossId", "SpecializationId" },
                values: new object[] { 7, 3 });

            migrationBuilder.UpdateData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 82,
                columns: new[] { "BossId", "SpecializationId" },
                values: new object[] { 7, 4 });

            migrationBuilder.UpdateData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 83,
                columns: new[] { "BossId", "SpecializationId" },
                values: new object[] { 7, 5 });

            migrationBuilder.UpdateData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 84,
                columns: new[] { "BossId", "SpecializationId" },
                values: new object[] { 7, 6 });

            migrationBuilder.UpdateData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 85,
                columns: new[] { "BossId", "SpecializationId" },
                values: new object[] { 7, 7 });

            migrationBuilder.UpdateData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 86,
                columns: new[] { "BossId", "SpecializationId" },
                values: new object[] { 7, 8 });

            migrationBuilder.UpdateData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 87,
                columns: new[] { "BossId", "SpecializationId" },
                values: new object[] { 7, 9 });

            migrationBuilder.UpdateData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 88,
                columns: new[] { "BossId", "SpecializationId" },
                values: new object[] { 7, 10 });

            migrationBuilder.UpdateData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 89,
                columns: new[] { "BossId", "SpecializationId" },
                values: new object[] { 7, 11 });

            migrationBuilder.UpdateData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 90,
                columns: new[] { "BossId", "SpecializationId" },
                values: new object[] { 7, 12 });

            migrationBuilder.UpdateData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 91,
                columns: new[] { "BossId", "SpecializationId" },
                values: new object[] { 7, 13 });

            migrationBuilder.UpdateData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 92,
                columns: new[] { "BossId", "SpecializationId" },
                values: new object[] { 8, 1 });

            migrationBuilder.UpdateData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 93,
                columns: new[] { "BossId", "SpecializationId" },
                values: new object[] { 8, 2 });

            migrationBuilder.UpdateData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 94,
                columns: new[] { "BossId", "SpecializationId" },
                values: new object[] { 8, 3 });

            migrationBuilder.UpdateData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 95,
                columns: new[] { "BossId", "SpecializationId" },
                values: new object[] { 8, 4 });

            migrationBuilder.UpdateData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 96,
                columns: new[] { "BossId", "SpecializationId" },
                values: new object[] { 8, 5 });

            migrationBuilder.UpdateData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 97,
                columns: new[] { "BossId", "SpecializationId" },
                values: new object[] { 8, 6 });

            migrationBuilder.UpdateData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 98,
                columns: new[] { "BossId", "SpecializationId" },
                values: new object[] { 8, 7 });

            migrationBuilder.UpdateData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 99,
                columns: new[] { "BossId", "SpecializationId" },
                values: new object[] { 8, 8 });

            migrationBuilder.UpdateData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 100,
                columns: new[] { "BossId", "SpecializationId" },
                values: new object[] { 8, 9 });

            migrationBuilder.UpdateData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 101,
                columns: new[] { "BossId", "SpecializationId" },
                values: new object[] { 8, 10 });

            migrationBuilder.UpdateData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 102,
                columns: new[] { "BossId", "SpecializationId" },
                values: new object[] { 8, 11 });

            migrationBuilder.UpdateData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 103,
                columns: new[] { "BossId", "SpecializationId" },
                values: new object[] { 8, 12 });

            migrationBuilder.UpdateData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 104,
                columns: new[] { "BossId", "SpecializationId" },
                values: new object[] { 8, 13 });

            migrationBuilder.UpdateData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 105,
                columns: new[] { "BossId", "SpecializationId" },
                values: new object[] { 9, 1 });

            migrationBuilder.UpdateData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 106,
                columns: new[] { "BossId", "SpecializationId" },
                values: new object[] { 9, 2 });

            migrationBuilder.UpdateData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 107,
                columns: new[] { "BossId", "SpecializationId" },
                values: new object[] { 9, 3 });

            migrationBuilder.UpdateData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 108,
                columns: new[] { "BossId", "SpecializationId" },
                values: new object[] { 9, 4 });

            migrationBuilder.UpdateData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 109,
                columns: new[] { "BossId", "SpecializationId" },
                values: new object[] { 9, 5 });

            migrationBuilder.UpdateData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 110,
                columns: new[] { "BossId", "SpecializationId" },
                values: new object[] { 9, 6 });

            migrationBuilder.UpdateData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 111,
                columns: new[] { "BossId", "SpecializationId" },
                values: new object[] { 9, 7 });

            migrationBuilder.UpdateData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 112,
                columns: new[] { "BossId", "SpecializationId" },
                values: new object[] { 9, 8 });

            migrationBuilder.UpdateData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 113,
                columns: new[] { "BossId", "SpecializationId" },
                values: new object[] { 9, 9 });

            migrationBuilder.UpdateData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 114,
                columns: new[] { "BossId", "SpecializationId" },
                values: new object[] { 9, 10 });

            migrationBuilder.UpdateData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 115,
                columns: new[] { "BossId", "SpecializationId" },
                values: new object[] { 9, 11 });

            migrationBuilder.UpdateData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 116,
                columns: new[] { "BossId", "SpecializationId" },
                values: new object[] { 9, 12 });

            migrationBuilder.UpdateData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 117,
                columns: new[] { "BossId", "SpecializationId" },
                values: new object[] { 9, 13 });

            migrationBuilder.UpdateData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 118,
                columns: new[] { "BossId", "SpecializationId" },
                values: new object[] { 10, 1 });

            migrationBuilder.UpdateData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 119,
                columns: new[] { "BossId", "SpecializationId" },
                values: new object[] { 10, 2 });

            migrationBuilder.UpdateData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 120,
                columns: new[] { "BossId", "SpecializationId" },
                values: new object[] { 10, 3 });

            migrationBuilder.UpdateData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 121,
                columns: new[] { "BossId", "SpecializationId" },
                values: new object[] { 10, 4 });

            migrationBuilder.UpdateData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 122,
                columns: new[] { "BossId", "SpecializationId" },
                values: new object[] { 10, 5 });

            migrationBuilder.UpdateData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 123,
                columns: new[] { "BossId", "SpecializationId" },
                values: new object[] { 10, 6 });

            migrationBuilder.UpdateData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 124,
                columns: new[] { "BossId", "SpecializationId" },
                values: new object[] { 10, 7 });

            migrationBuilder.UpdateData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 125,
                columns: new[] { "BossId", "SpecializationId" },
                values: new object[] { 10, 8 });

            migrationBuilder.UpdateData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 126,
                columns: new[] { "BossId", "SpecializationId" },
                values: new object[] { 10, 9 });

            migrationBuilder.UpdateData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 127,
                columns: new[] { "BossId", "SpecializationId" },
                values: new object[] { 10, 10 });

            migrationBuilder.UpdateData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 128,
                columns: new[] { "BossId", "SpecializationId" },
                values: new object[] { 10, 11 });

            migrationBuilder.UpdateData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 129,
                columns: new[] { "BossId", "SpecializationId" },
                values: new object[] { 10, 12 });

            migrationBuilder.UpdateData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 130,
                columns: new[] { "BossId", "SpecializationId" },
                values: new object[] { 10, 13 });

            migrationBuilder.UpdateData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 131,
                columns: new[] { "BossId", "SpecializationId" },
                values: new object[] { 11, 1 });

            migrationBuilder.UpdateData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 132,
                columns: new[] { "BossId", "SpecializationId" },
                values: new object[] { 11, 2 });

            migrationBuilder.UpdateData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 133,
                columns: new[] { "BossId", "SpecializationId" },
                values: new object[] { 11, 3 });

            migrationBuilder.UpdateData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 134,
                columns: new[] { "BossId", "SpecializationId" },
                values: new object[] { 11, 4 });

            migrationBuilder.UpdateData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 135,
                columns: new[] { "BossId", "SpecializationId" },
                values: new object[] { 11, 5 });

            migrationBuilder.UpdateData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 136,
                columns: new[] { "BossId", "SpecializationId" },
                values: new object[] { 11, 6 });

            migrationBuilder.UpdateData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 137,
                columns: new[] { "BossId", "SpecializationId" },
                values: new object[] { 11, 7 });

            migrationBuilder.UpdateData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 138,
                columns: new[] { "BossId", "SpecializationId" },
                values: new object[] { 11, 8 });

            migrationBuilder.UpdateData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 139,
                columns: new[] { "BossId", "SpecializationId" },
                values: new object[] { 11, 9 });

            migrationBuilder.UpdateData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 140,
                columns: new[] { "BossId", "SpecializationId" },
                values: new object[] { 11, 10 });

            migrationBuilder.UpdateData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 141,
                columns: new[] { "BossId", "SpecializationId" },
                values: new object[] { 11, 11 });

            migrationBuilder.UpdateData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 142,
                columns: new[] { "BossId", "SpecializationId" },
                values: new object[] { 11, 12 });

            migrationBuilder.UpdateData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 143,
                columns: new[] { "BossId", "SpecializationId" },
                values: new object[] { 11, 13 });

            migrationBuilder.UpdateData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 144,
                columns: new[] { "BossId", "SpecializationId" },
                values: new object[] { 12, 1 });

            migrationBuilder.UpdateData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 145,
                columns: new[] { "BossId", "SpecializationId" },
                values: new object[] { 12, 2 });

            migrationBuilder.UpdateData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 146,
                columns: new[] { "BossId", "SpecializationId" },
                values: new object[] { 12, 3 });

            migrationBuilder.UpdateData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 147,
                columns: new[] { "BossId", "SpecializationId" },
                values: new object[] { 12, 4 });

            migrationBuilder.UpdateData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 148,
                columns: new[] { "BossId", "SpecializationId" },
                values: new object[] { 12, 5 });

            migrationBuilder.UpdateData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 149,
                columns: new[] { "BossId", "SpecializationId" },
                values: new object[] { 12, 6 });

            migrationBuilder.UpdateData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 150,
                columns: new[] { "BossId", "SpecializationId" },
                values: new object[] { 12, 7 });

            migrationBuilder.UpdateData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 151,
                columns: new[] { "BossId", "SpecializationId" },
                values: new object[] { 12, 8 });

            migrationBuilder.UpdateData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 152,
                columns: new[] { "BossId", "SpecializationId" },
                values: new object[] { 12, 9 });

            migrationBuilder.UpdateData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 153,
                columns: new[] { "BossId", "SpecializationId" },
                values: new object[] { 12, 10 });

            migrationBuilder.UpdateData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 154,
                columns: new[] { "BossId", "SpecializationId" },
                values: new object[] { 12, 11 });

            migrationBuilder.UpdateData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 155,
                columns: new[] { "BossId", "SpecializationId" },
                values: new object[] { 12, 12 });

            migrationBuilder.UpdateData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 156,
                columns: new[] { "BossId", "SpecializationId" },
                values: new object[] { 12, 13 });

            migrationBuilder.UpdateData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 157,
                columns: new[] { "BossId", "SpecializationId" },
                values: new object[] { 13, 1 });

            migrationBuilder.UpdateData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 158,
                columns: new[] { "BossId", "SpecializationId" },
                values: new object[] { 13, 2 });

            migrationBuilder.UpdateData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 159,
                columns: new[] { "BossId", "SpecializationId" },
                values: new object[] { 13, 3 });

            migrationBuilder.UpdateData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 160,
                columns: new[] { "BossId", "SpecializationId" },
                values: new object[] { 13, 4 });

            migrationBuilder.UpdateData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 161,
                columns: new[] { "BossId", "SpecializationId" },
                values: new object[] { 13, 5 });

            migrationBuilder.UpdateData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 162,
                columns: new[] { "BossId", "SpecializationId" },
                values: new object[] { 13, 6 });

            migrationBuilder.UpdateData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 163,
                columns: new[] { "BossId", "SpecializationId" },
                values: new object[] { 13, 7 });

            migrationBuilder.UpdateData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 164,
                columns: new[] { "BossId", "SpecializationId" },
                values: new object[] { 13, 8 });

            migrationBuilder.UpdateData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 165,
                columns: new[] { "BossId", "SpecializationId" },
                values: new object[] { 13, 9 });

            migrationBuilder.UpdateData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 166,
                columns: new[] { "BossId", "SpecializationId" },
                values: new object[] { 13, 10 });

            migrationBuilder.UpdateData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 167,
                columns: new[] { "BossId", "SpecializationId" },
                values: new object[] { 13, 11 });

            migrationBuilder.UpdateData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 168,
                columns: new[] { "BossId", "SpecializationId" },
                values: new object[] { 13, 12 });

            migrationBuilder.UpdateData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 169,
                columns: new[] { "BossId", "SpecializationId" },
                values: new object[] { 13, 13 });

            migrationBuilder.UpdateData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 170,
                columns: new[] { "BossId", "SpecializationId" },
                values: new object[] { 14, 1 });

            migrationBuilder.UpdateData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 171,
                columns: new[] { "BossId", "SpecializationId" },
                values: new object[] { 14, 2 });

            migrationBuilder.UpdateData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 172,
                columns: new[] { "BossId", "SpecializationId" },
                values: new object[] { 14, 3 });

            migrationBuilder.UpdateData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 173,
                columns: new[] { "BossId", "SpecializationId" },
                values: new object[] { 14, 4 });

            migrationBuilder.UpdateData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 174,
                columns: new[] { "BossId", "SpecializationId" },
                values: new object[] { 14, 5 });

            migrationBuilder.UpdateData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 175,
                columns: new[] { "BossId", "SpecializationId" },
                values: new object[] { 14, 6 });

            migrationBuilder.UpdateData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 176,
                columns: new[] { "BossId", "SpecializationId" },
                values: new object[] { 14, 7 });

            migrationBuilder.UpdateData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 177,
                columns: new[] { "BossId", "SpecializationId" },
                values: new object[] { 14, 8 });

            migrationBuilder.UpdateData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 178,
                columns: new[] { "BossId", "SpecializationId" },
                values: new object[] { 14, 9 });

            migrationBuilder.UpdateData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 179,
                columns: new[] { "BossId", "SpecializationId" },
                values: new object[] { 14, 10 });

            migrationBuilder.UpdateData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 180,
                columns: new[] { "BossId", "SpecializationId" },
                values: new object[] { 14, 11 });

            migrationBuilder.UpdateData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 181,
                columns: new[] { "BossId", "SpecializationId" },
                values: new object[] { 14, 12 });

            migrationBuilder.UpdateData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 182,
                columns: new[] { "BossId", "SpecializationId" },
                values: new object[] { 14, 13 });

            migrationBuilder.UpdateData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 183,
                columns: new[] { "BossId", "SpecializationId" },
                values: new object[] { 15, 1 });

            migrationBuilder.UpdateData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 184,
                columns: new[] { "BossId", "SpecializationId" },
                values: new object[] { 15, 2 });

            migrationBuilder.UpdateData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 185,
                columns: new[] { "BossId", "SpecializationId" },
                values: new object[] { 15, 3 });

            migrationBuilder.UpdateData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 186,
                columns: new[] { "BossId", "SpecializationId" },
                values: new object[] { 15, 4 });

            migrationBuilder.UpdateData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 187,
                columns: new[] { "BossId", "SpecializationId" },
                values: new object[] { 15, 5 });

            migrationBuilder.UpdateData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 188,
                columns: new[] { "BossId", "SpecializationId" },
                values: new object[] { 15, 6 });

            migrationBuilder.UpdateData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 189,
                columns: new[] { "BossId", "SpecializationId" },
                values: new object[] { 15, 7 });

            migrationBuilder.UpdateData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 190,
                columns: new[] { "BossId", "SpecializationId" },
                values: new object[] { 15, 8 });

            migrationBuilder.UpdateData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 191,
                columns: new[] { "BossId", "SpecializationId" },
                values: new object[] { 15, 9 });

            migrationBuilder.UpdateData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 192,
                columns: new[] { "BossId", "SpecializationId" },
                values: new object[] { 15, 10 });

            migrationBuilder.UpdateData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 193,
                columns: new[] { "BossId", "SpecializationId" },
                values: new object[] { 15, 11 });

            migrationBuilder.UpdateData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 194,
                columns: new[] { "BossId", "SpecializationId" },
                values: new object[] { 15, 12 });

            migrationBuilder.UpdateData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 195,
                columns: new[] { "BossId", "SpecializationId" },
                values: new object[] { 15, 13 });

            migrationBuilder.UpdateData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 196,
                columns: new[] { "BossId", "SpecializationId" },
                values: new object[] { 16, 1 });

            migrationBuilder.UpdateData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 197,
                columns: new[] { "BossId", "SpecializationId" },
                values: new object[] { 16, 2 });

            migrationBuilder.UpdateData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 198,
                columns: new[] { "BossId", "SpecializationId" },
                values: new object[] { 16, 3 });

            migrationBuilder.UpdateData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 199,
                columns: new[] { "BossId", "SpecializationId" },
                values: new object[] { 16, 4 });

            migrationBuilder.UpdateData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 200,
                columns: new[] { "BossId", "SpecializationId" },
                values: new object[] { 16, 5 });

            migrationBuilder.UpdateData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 201,
                columns: new[] { "BossId", "SpecializationId" },
                values: new object[] { 16, 6 });

            migrationBuilder.UpdateData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 202,
                columns: new[] { "BossId", "SpecializationId" },
                values: new object[] { 16, 7 });

            migrationBuilder.UpdateData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 203,
                columns: new[] { "BossId", "SpecializationId" },
                values: new object[] { 16, 8 });

            migrationBuilder.UpdateData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 204,
                columns: new[] { "BossId", "SpecializationId" },
                values: new object[] { 16, 9 });

            migrationBuilder.UpdateData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 205,
                columns: new[] { "BossId", "SpecializationId" },
                values: new object[] { 16, 10 });

            migrationBuilder.UpdateData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 206,
                columns: new[] { "BossId", "SpecializationId" },
                values: new object[] { 16, 11 });

            migrationBuilder.UpdateData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 207,
                columns: new[] { "BossId", "SpecializationId" },
                values: new object[] { 16, 12 });

            migrationBuilder.UpdateData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 208,
                columns: new[] { "BossId", "SpecializationId" },
                values: new object[] { 16, 13 });

            migrationBuilder.UpdateData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 209,
                columns: new[] { "BossId", "SpecializationId" },
                values: new object[] { 17, 1 });

            migrationBuilder.UpdateData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 210,
                columns: new[] { "BossId", "SpecializationId" },
                values: new object[] { 17, 2 });

            migrationBuilder.UpdateData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 211,
                columns: new[] { "BossId", "SpecializationId" },
                values: new object[] { 17, 3 });

            migrationBuilder.UpdateData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 212,
                columns: new[] { "BossId", "SpecializationId" },
                values: new object[] { 17, 4 });

            migrationBuilder.UpdateData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 213,
                columns: new[] { "BossId", "SpecializationId" },
                values: new object[] { 17, 5 });

            migrationBuilder.UpdateData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 214,
                columns: new[] { "BossId", "SpecializationId" },
                values: new object[] { 17, 6 });

            migrationBuilder.UpdateData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 215,
                columns: new[] { "BossId", "SpecializationId" },
                values: new object[] { 17, 7 });

            migrationBuilder.UpdateData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 216,
                columns: new[] { "BossId", "SpecializationId" },
                values: new object[] { 17, 8 });

            migrationBuilder.UpdateData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 217,
                columns: new[] { "BossId", "SpecializationId" },
                values: new object[] { 17, 9 });

            migrationBuilder.UpdateData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 218,
                columns: new[] { "BossId", "SpecializationId" },
                values: new object[] { 17, 10 });

            migrationBuilder.UpdateData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 219,
                columns: new[] { "BossId", "SpecializationId" },
                values: new object[] { 17, 11 });

            migrationBuilder.UpdateData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 220,
                columns: new[] { "BossId", "SpecializationId" },
                values: new object[] { 17, 12 });

            migrationBuilder.UpdateData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 221,
                columns: new[] { "BossId", "SpecializationId" },
                values: new object[] { 17, 13 });

            migrationBuilder.UpdateData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 222,
                column: "BossId",
                value: 18);

            migrationBuilder.UpdateData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 223,
                column: "BossId",
                value: 18);

            migrationBuilder.UpdateData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 224,
                column: "BossId",
                value: 18);

            migrationBuilder.UpdateData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 225,
                column: "BossId",
                value: 18);

            migrationBuilder.UpdateData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 226,
                column: "BossId",
                value: 18);

            migrationBuilder.UpdateData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 227,
                column: "BossId",
                value: 18);

            migrationBuilder.UpdateData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 228,
                column: "BossId",
                value: 18);

            migrationBuilder.UpdateData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 229,
                column: "BossId",
                value: 18);

            migrationBuilder.UpdateData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 230,
                column: "BossId",
                value: 18);

            migrationBuilder.UpdateData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 231,
                column: "BossId",
                value: 18);

            migrationBuilder.UpdateData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 232,
                column: "BossId",
                value: 18);

            migrationBuilder.UpdateData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 233,
                column: "BossId",
                value: 18);

            migrationBuilder.UpdateData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 234,
                column: "BossId",
                value: 18);

            migrationBuilder.UpdateData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 235,
                columns: new[] { "BossId", "SpecializationId" },
                values: new object[] { 19, 1 });

            migrationBuilder.UpdateData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 236,
                columns: new[] { "BossId", "SpecializationId" },
                values: new object[] { 19, 2 });

            migrationBuilder.UpdateData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 237,
                columns: new[] { "BossId", "SpecializationId" },
                values: new object[] { 19, 3 });

            migrationBuilder.UpdateData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 238,
                columns: new[] { "BossId", "SpecializationId" },
                values: new object[] { 19, 4 });

            migrationBuilder.UpdateData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 239,
                columns: new[] { "BossId", "SpecializationId" },
                values: new object[] { 19, 5 });

            migrationBuilder.UpdateData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 240,
                columns: new[] { "BossId", "SpecializationId" },
                values: new object[] { 19, 6 });

            migrationBuilder.UpdateData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 241,
                columns: new[] { "BossId", "SpecializationId" },
                values: new object[] { 19, 7 });

            migrationBuilder.UpdateData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 242,
                columns: new[] { "BossId", "SpecializationId" },
                values: new object[] { 19, 8 });

            migrationBuilder.UpdateData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 243,
                columns: new[] { "BossId", "SpecializationId" },
                values: new object[] { 19, 9 });

            migrationBuilder.UpdateData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 244,
                columns: new[] { "BossId", "SpecializationId" },
                values: new object[] { 19, 10 });

            migrationBuilder.UpdateData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 245,
                columns: new[] { "BossId", "SpecializationId" },
                values: new object[] { 19, 11 });

            migrationBuilder.UpdateData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 246,
                columns: new[] { "BossId", "SpecializationId" },
                values: new object[] { 19, 12 });

            migrationBuilder.UpdateData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 247,
                columns: new[] { "BossId", "SpecializationId" },
                values: new object[] { 19, 13 });

            migrationBuilder.UpdateData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 248,
                columns: new[] { "BossId", "SpecializationId" },
                values: new object[] { 20, 1 });

            migrationBuilder.UpdateData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 249,
                columns: new[] { "BossId", "SpecializationId" },
                values: new object[] { 20, 2 });

            migrationBuilder.UpdateData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 250,
                columns: new[] { "BossId", "SpecializationId" },
                values: new object[] { 20, 3 });

            migrationBuilder.UpdateData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 251,
                columns: new[] { "BossId", "SpecializationId" },
                values: new object[] { 20, 4 });

            migrationBuilder.UpdateData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 252,
                columns: new[] { "BossId", "SpecializationId" },
                values: new object[] { 20, 5 });

            migrationBuilder.UpdateData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 253,
                columns: new[] { "BossId", "SpecializationId" },
                values: new object[] { 20, 6 });

            migrationBuilder.UpdateData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 254,
                columns: new[] { "BossId", "SpecializationId" },
                values: new object[] { 20, 7 });

            migrationBuilder.UpdateData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 255,
                columns: new[] { "BossId", "SpecializationId" },
                values: new object[] { 20, 8 });

            migrationBuilder.UpdateData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 256,
                columns: new[] { "BossId", "SpecializationId" },
                values: new object[] { 20, 9 });

            migrationBuilder.UpdateData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 257,
                columns: new[] { "BossId", "SpecializationId" },
                values: new object[] { 20, 10 });

            migrationBuilder.UpdateData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 258,
                columns: new[] { "BossId", "SpecializationId" },
                values: new object[] { 20, 11 });

            migrationBuilder.UpdateData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 259,
                columns: new[] { "BossId", "SpecializationId" },
                values: new object[] { 20, 12 });

            migrationBuilder.UpdateData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 260,
                columns: new[] { "BossId", "SpecializationId" },
                values: new object[] { 20, 13 });

            migrationBuilder.UpdateData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 261,
                columns: new[] { "BossId", "SpecializationId" },
                values: new object[] { 21, 1 });

            migrationBuilder.UpdateData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 262,
                columns: new[] { "BossId", "SpecializationId" },
                values: new object[] { 21, 2 });

            migrationBuilder.UpdateData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 263,
                columns: new[] { "BossId", "SpecializationId" },
                values: new object[] { 21, 3 });

            migrationBuilder.UpdateData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 264,
                columns: new[] { "BossId", "SpecializationId" },
                values: new object[] { 21, 4 });

            migrationBuilder.UpdateData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 265,
                columns: new[] { "BossId", "SpecializationId" },
                values: new object[] { 21, 5 });

            migrationBuilder.UpdateData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 266,
                columns: new[] { "BossId", "SpecializationId" },
                values: new object[] { 21, 6 });

            migrationBuilder.UpdateData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 267,
                columns: new[] { "BossId", "SpecializationId" },
                values: new object[] { 21, 7 });

            migrationBuilder.UpdateData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 268,
                columns: new[] { "BossId", "SpecializationId" },
                values: new object[] { 21, 8 });

            migrationBuilder.UpdateData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 269,
                columns: new[] { "BossId", "SpecializationId" },
                values: new object[] { 21, 9 });

            migrationBuilder.UpdateData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 270,
                columns: new[] { "BossId", "SpecializationId" },
                values: new object[] { 21, 10 });

            migrationBuilder.UpdateData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 271,
                columns: new[] { "BossId", "SpecializationId" },
                values: new object[] { 21, 11 });

            migrationBuilder.UpdateData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 272,
                columns: new[] { "BossId", "SpecializationId" },
                values: new object[] { 21, 12 });

            migrationBuilder.UpdateData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 273,
                columns: new[] { "BossId", "SpecializationId" },
                values: new object[] { 21, 13 });

            migrationBuilder.UpdateData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 274,
                columns: new[] { "BossId", "SpecializationId" },
                values: new object[] { 22, 1 });

            migrationBuilder.UpdateData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 275,
                columns: new[] { "BossId", "SpecializationId" },
                values: new object[] { 22, 2 });

            migrationBuilder.UpdateData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 276,
                columns: new[] { "BossId", "SpecializationId" },
                values: new object[] { 22, 3 });

            migrationBuilder.UpdateData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 277,
                columns: new[] { "BossId", "SpecializationId" },
                values: new object[] { 22, 4 });

            migrationBuilder.UpdateData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 278,
                columns: new[] { "BossId", "SpecializationId" },
                values: new object[] { 22, 5 });

            migrationBuilder.UpdateData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 279,
                columns: new[] { "BossId", "SpecializationId" },
                values: new object[] { 22, 6 });

            migrationBuilder.UpdateData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 280,
                columns: new[] { "BossId", "SpecializationId" },
                values: new object[] { 22, 7 });

            migrationBuilder.UpdateData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 281,
                columns: new[] { "BossId", "SpecializationId" },
                values: new object[] { 22, 8 });

            migrationBuilder.UpdateData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 282,
                columns: new[] { "BossId", "SpecializationId" },
                values: new object[] { 22, 9 });

            migrationBuilder.UpdateData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 283,
                columns: new[] { "BossId", "SpecializationId" },
                values: new object[] { 22, 10 });

            migrationBuilder.UpdateData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 284,
                columns: new[] { "BossId", "SpecializationId" },
                values: new object[] { 22, 11 });

            migrationBuilder.UpdateData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 285,
                columns: new[] { "BossId", "SpecializationId" },
                values: new object[] { 22, 12 });

            migrationBuilder.UpdateData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 286,
                columns: new[] { "BossId", "SpecializationId" },
                values: new object[] { 22, 13 });

            migrationBuilder.UpdateData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 287,
                columns: new[] { "BossId", "SpecializationId" },
                values: new object[] { 23, 1 });

            migrationBuilder.UpdateData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 288,
                columns: new[] { "BossId", "SpecializationId" },
                values: new object[] { 23, 2 });

            migrationBuilder.UpdateData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 289,
                columns: new[] { "BossId", "SpecializationId" },
                values: new object[] { 23, 3 });

            migrationBuilder.UpdateData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 290,
                columns: new[] { "BossId", "SpecializationId" },
                values: new object[] { 23, 4 });

            migrationBuilder.UpdateData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 291,
                columns: new[] { "BossId", "SpecializationId" },
                values: new object[] { 23, 5 });

            migrationBuilder.UpdateData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 292,
                columns: new[] { "BossId", "SpecializationId" },
                values: new object[] { 23, 6 });

            migrationBuilder.UpdateData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 293,
                columns: new[] { "BossId", "SpecializationId" },
                values: new object[] { 23, 7 });

            migrationBuilder.UpdateData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 294,
                columns: new[] { "BossId", "SpecializationId" },
                values: new object[] { 23, 8 });

            migrationBuilder.UpdateData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 295,
                columns: new[] { "BossId", "SpecializationId" },
                values: new object[] { 23, 9 });

            migrationBuilder.UpdateData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 296,
                columns: new[] { "BossId", "SpecializationId" },
                values: new object[] { 23, 10 });

            migrationBuilder.UpdateData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 297,
                columns: new[] { "BossId", "SpecializationId" },
                values: new object[] { 23, 11 });

            migrationBuilder.UpdateData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 298,
                columns: new[] { "BossId", "SpecializationId" },
                values: new object[] { 23, 12 });

            migrationBuilder.UpdateData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 299,
                columns: new[] { "BossId", "SpecializationId" },
                values: new object[] { 23, 13 });

            migrationBuilder.UpdateData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 300,
                columns: new[] { "BossId", "SpecializationId" },
                values: new object[] { 24, 1 });

            migrationBuilder.UpdateData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 301,
                columns: new[] { "BossId", "SpecializationId" },
                values: new object[] { 24, 2 });

            migrationBuilder.UpdateData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 302,
                columns: new[] { "BossId", "SpecializationId" },
                values: new object[] { 24, 3 });

            migrationBuilder.UpdateData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 303,
                columns: new[] { "BossId", "SpecializationId" },
                values: new object[] { 24, 4 });

            migrationBuilder.UpdateData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 304,
                columns: new[] { "BossId", "SpecializationId" },
                values: new object[] { 24, 5 });

            migrationBuilder.UpdateData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 305,
                columns: new[] { "BossId", "SpecializationId" },
                values: new object[] { 24, 6 });

            migrationBuilder.UpdateData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 306,
                columns: new[] { "BossId", "SpecializationId" },
                values: new object[] { 24, 7 });

            migrationBuilder.UpdateData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 307,
                columns: new[] { "BossId", "SpecializationId" },
                values: new object[] { 24, 8 });

            migrationBuilder.UpdateData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 308,
                columns: new[] { "BossId", "SpecializationId" },
                values: new object[] { 24, 9 });

            migrationBuilder.UpdateData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 309,
                columns: new[] { "BossId", "SpecializationId" },
                values: new object[] { 24, 10 });

            migrationBuilder.UpdateData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 310,
                columns: new[] { "BossId", "SpecializationId" },
                values: new object[] { 24, 11 });

            migrationBuilder.UpdateData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 311,
                columns: new[] { "BossId", "SpecializationId" },
                values: new object[] { 24, 12 });

            migrationBuilder.UpdateData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 312,
                columns: new[] { "BossId", "SpecializationId" },
                values: new object[] { 24, 13 });

            migrationBuilder.UpdateData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 313,
                columns: new[] { "BossId", "SpecializationId" },
                values: new object[] { 25, 1 });

            migrationBuilder.UpdateData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 314,
                columns: new[] { "BossId", "SpecializationId" },
                values: new object[] { 25, 2 });

            migrationBuilder.UpdateData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 315,
                columns: new[] { "BossId", "SpecializationId" },
                values: new object[] { 25, 3 });

            migrationBuilder.UpdateData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 316,
                columns: new[] { "BossId", "SpecializationId" },
                values: new object[] { 25, 4 });

            migrationBuilder.UpdateData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 317,
                columns: new[] { "BossId", "SpecializationId" },
                values: new object[] { 25, 5 });

            migrationBuilder.UpdateData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 318,
                columns: new[] { "BossId", "SpecializationId" },
                values: new object[] { 25, 6 });

            migrationBuilder.UpdateData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 319,
                columns: new[] { "BossId", "SpecializationId" },
                values: new object[] { 25, 7 });

            migrationBuilder.UpdateData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 320,
                columns: new[] { "BossId", "SpecializationId" },
                values: new object[] { 25, 8 });

            migrationBuilder.UpdateData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 321,
                columns: new[] { "BossId", "SpecializationId" },
                values: new object[] { 25, 9 });

            migrationBuilder.UpdateData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 322,
                columns: new[] { "BossId", "SpecializationId" },
                values: new object[] { 25, 10 });

            migrationBuilder.UpdateData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 323,
                columns: new[] { "BossId", "SpecializationId" },
                values: new object[] { 25, 11 });

            migrationBuilder.UpdateData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 324,
                columns: new[] { "BossId", "SpecializationId" },
                values: new object[] { 25, 12 });

            migrationBuilder.UpdateData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 325,
                columns: new[] { "BossId", "SpecializationId" },
                values: new object[] { 25, 13 });

            migrationBuilder.UpdateData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 326,
                columns: new[] { "BossId", "SpecializationId" },
                values: new object[] { 26, 1 });

            migrationBuilder.UpdateData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 327,
                columns: new[] { "BossId", "SpecializationId" },
                values: new object[] { 26, 2 });

            migrationBuilder.UpdateData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 328,
                columns: new[] { "BossId", "SpecializationId" },
                values: new object[] { 26, 3 });

            migrationBuilder.UpdateData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 329,
                columns: new[] { "BossId", "SpecializationId" },
                values: new object[] { 26, 4 });

            migrationBuilder.UpdateData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 330,
                columns: new[] { "BossId", "SpecializationId" },
                values: new object[] { 26, 5 });

            migrationBuilder.UpdateData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 331,
                columns: new[] { "BossId", "SpecializationId" },
                values: new object[] { 26, 6 });

            migrationBuilder.UpdateData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 332,
                columns: new[] { "BossId", "SpecializationId" },
                values: new object[] { 26, 7 });

            migrationBuilder.UpdateData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 333,
                columns: new[] { "BossId", "SpecializationId" },
                values: new object[] { 26, 8 });

            migrationBuilder.UpdateData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 334,
                columns: new[] { "BossId", "SpecializationId" },
                values: new object[] { 26, 9 });

            migrationBuilder.UpdateData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 335,
                columns: new[] { "BossId", "SpecializationId" },
                values: new object[] { 26, 10 });

            migrationBuilder.UpdateData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 336,
                columns: new[] { "BossId", "SpecializationId" },
                values: new object[] { 26, 11 });

            migrationBuilder.UpdateData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 337,
                columns: new[] { "BossId", "SpecializationId" },
                values: new object[] { 26, 12 });

            migrationBuilder.UpdateData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 338,
                columns: new[] { "BossId", "SpecializationId" },
                values: new object[] { 26, 13 });

            migrationBuilder.UpdateData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 339,
                columns: new[] { "BossId", "SpecializationId" },
                values: new object[] { 27, 1 });

            migrationBuilder.UpdateData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 340,
                columns: new[] { "BossId", "SpecializationId" },
                values: new object[] { 27, 2 });

            migrationBuilder.UpdateData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 341,
                columns: new[] { "BossId", "SpecializationId" },
                values: new object[] { 27, 3 });

            migrationBuilder.UpdateData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 342,
                columns: new[] { "BossId", "SpecializationId" },
                values: new object[] { 27, 4 });

            migrationBuilder.UpdateData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 343,
                columns: new[] { "BossId", "SpecializationId" },
                values: new object[] { 27, 5 });

            migrationBuilder.UpdateData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 344,
                columns: new[] { "BossId", "SpecializationId" },
                values: new object[] { 27, 6 });

            migrationBuilder.UpdateData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 345,
                columns: new[] { "BossId", "SpecializationId" },
                values: new object[] { 27, 7 });

            migrationBuilder.UpdateData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 346,
                columns: new[] { "BossId", "SpecializationId" },
                values: new object[] { 27, 8 });

            migrationBuilder.UpdateData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 347,
                columns: new[] { "BossId", "SpecializationId" },
                values: new object[] { 27, 9 });

            migrationBuilder.UpdateData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 348,
                columns: new[] { "BossId", "SpecializationId" },
                values: new object[] { 27, 10 });

            migrationBuilder.UpdateData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 349,
                columns: new[] { "BossId", "SpecializationId" },
                values: new object[] { 27, 11 });

            migrationBuilder.UpdateData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 350,
                columns: new[] { "BossId", "SpecializationId" },
                values: new object[] { 27, 12 });

            migrationBuilder.UpdateData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 351,
                columns: new[] { "BossId", "SpecializationId" },
                values: new object[] { 27, 13 });

            migrationBuilder.UpdateData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 352,
                columns: new[] { "BossId", "SpecializationId" },
                values: new object[] { 28, 1 });

            migrationBuilder.UpdateData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 353,
                columns: new[] { "BossId", "SpecializationId" },
                values: new object[] { 28, 2 });

            migrationBuilder.UpdateData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 354,
                columns: new[] { "BossId", "SpecializationId" },
                values: new object[] { 28, 3 });

            migrationBuilder.UpdateData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 355,
                columns: new[] { "BossId", "SpecializationId" },
                values: new object[] { 28, 4 });

            migrationBuilder.UpdateData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 356,
                columns: new[] { "BossId", "SpecializationId" },
                values: new object[] { 28, 5 });

            migrationBuilder.UpdateData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 357,
                columns: new[] { "BossId", "SpecializationId" },
                values: new object[] { 28, 6 });

            migrationBuilder.UpdateData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 358,
                columns: new[] { "BossId", "SpecializationId" },
                values: new object[] { 28, 7 });

            migrationBuilder.UpdateData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 359,
                columns: new[] { "BossId", "SpecializationId" },
                values: new object[] { 28, 8 });

            migrationBuilder.UpdateData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 360,
                columns: new[] { "BossId", "SpecializationId" },
                values: new object[] { 28, 9 });

            migrationBuilder.UpdateData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 361,
                columns: new[] { "BossId", "SpecializationId" },
                values: new object[] { 28, 10 });

            migrationBuilder.UpdateData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 362,
                columns: new[] { "BossId", "SpecializationId" },
                values: new object[] { 28, 11 });

            migrationBuilder.UpdateData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 363,
                columns: new[] { "BossId", "SpecializationId" },
                values: new object[] { 28, 12 });

            migrationBuilder.UpdateData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 364,
                columns: new[] { "BossId", "SpecializationId" },
                values: new object[] { 28, 13 });

            migrationBuilder.UpdateData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 365,
                columns: new[] { "BossId", "SpecializationId" },
                values: new object[] { 29, 1 });

            migrationBuilder.UpdateData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 366,
                columns: new[] { "BossId", "SpecializationId" },
                values: new object[] { 29, 2 });

            migrationBuilder.UpdateData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 367,
                columns: new[] { "BossId", "SpecializationId" },
                values: new object[] { 29, 3 });

            migrationBuilder.UpdateData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 368,
                columns: new[] { "BossId", "SpecializationId" },
                values: new object[] { 29, 4 });

            migrationBuilder.UpdateData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 369,
                columns: new[] { "BossId", "SpecializationId" },
                values: new object[] { 29, 5 });

            migrationBuilder.UpdateData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 370,
                columns: new[] { "BossId", "SpecializationId" },
                values: new object[] { 29, 6 });

            migrationBuilder.UpdateData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 371,
                columns: new[] { "BossId", "SpecializationId" },
                values: new object[] { 29, 7 });

            migrationBuilder.UpdateData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 372,
                columns: new[] { "BossId", "SpecializationId" },
                values: new object[] { 29, 8 });

            migrationBuilder.UpdateData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 373,
                columns: new[] { "BossId", "SpecializationId" },
                values: new object[] { 29, 9 });

            migrationBuilder.UpdateData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 374,
                columns: new[] { "BossId", "SpecializationId" },
                values: new object[] { 29, 10 });

            migrationBuilder.UpdateData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 375,
                columns: new[] { "BossId", "SpecializationId" },
                values: new object[] { 29, 11 });

            migrationBuilder.UpdateData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 376,
                columns: new[] { "BossId", "SpecializationId" },
                values: new object[] { 29, 12 });

            migrationBuilder.UpdateData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 377,
                columns: new[] { "BossId", "SpecializationId" },
                values: new object[] { 29, 13 });

            migrationBuilder.UpdateData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 378,
                columns: new[] { "BossId", "SpecializationId" },
                values: new object[] { 30, 1 });

            migrationBuilder.UpdateData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 379,
                columns: new[] { "BossId", "SpecializationId" },
                values: new object[] { 30, 2 });

            migrationBuilder.UpdateData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 380,
                columns: new[] { "BossId", "SpecializationId" },
                values: new object[] { 30, 3 });

            migrationBuilder.UpdateData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 381,
                columns: new[] { "BossId", "SpecializationId" },
                values: new object[] { 30, 4 });

            migrationBuilder.UpdateData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 382,
                columns: new[] { "BossId", "SpecializationId" },
                values: new object[] { 30, 5 });

            migrationBuilder.UpdateData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 383,
                columns: new[] { "BossId", "SpecializationId" },
                values: new object[] { 30, 6 });

            migrationBuilder.UpdateData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 384,
                columns: new[] { "BossId", "SpecializationId" },
                values: new object[] { 30, 7 });

            migrationBuilder.UpdateData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 385,
                columns: new[] { "BossId", "SpecializationId" },
                values: new object[] { 30, 8 });

            migrationBuilder.UpdateData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 386,
                columns: new[] { "BossId", "SpecializationId" },
                values: new object[] { 30, 9 });

            migrationBuilder.UpdateData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 387,
                columns: new[] { "BossId", "SpecializationId" },
                values: new object[] { 30, 10 });

            migrationBuilder.UpdateData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 388,
                columns: new[] { "BossId", "SpecializationId" },
                values: new object[] { 30, 11 });

            migrationBuilder.UpdateData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 389,
                columns: new[] { "BossId", "SpecializationId" },
                values: new object[] { 30, 12 });

            migrationBuilder.UpdateData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 390,
                columns: new[] { "BossId", "SpecializationId" },
                values: new object[] { 30, 13 });

            migrationBuilder.UpdateData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 391,
                columns: new[] { "BossId", "SpecializationId" },
                values: new object[] { 31, 1 });

            migrationBuilder.UpdateData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 392,
                columns: new[] { "BossId", "SpecializationId" },
                values: new object[] { 31, 2 });

            migrationBuilder.UpdateData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 393,
                columns: new[] { "BossId", "SpecializationId" },
                values: new object[] { 31, 3 });

            migrationBuilder.UpdateData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 394,
                columns: new[] { "BossId", "SpecializationId" },
                values: new object[] { 31, 4 });

            migrationBuilder.UpdateData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 395,
                columns: new[] { "BossId", "SpecializationId" },
                values: new object[] { 31, 5 });

            migrationBuilder.UpdateData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 396,
                columns: new[] { "BossId", "SpecializationId" },
                values: new object[] { 31, 6 });

            migrationBuilder.UpdateData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 397,
                columns: new[] { "BossId", "SpecializationId" },
                values: new object[] { 31, 7 });

            migrationBuilder.UpdateData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 398,
                columns: new[] { "BossId", "SpecializationId" },
                values: new object[] { 31, 8 });

            migrationBuilder.UpdateData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 399,
                columns: new[] { "BossId", "SpecializationId" },
                values: new object[] { 31, 9 });

            migrationBuilder.UpdateData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 400,
                columns: new[] { "BossId", "SpecializationId" },
                values: new object[] { 31, 10 });

            migrationBuilder.UpdateData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 401,
                columns: new[] { "BossId", "SpecializationId" },
                values: new object[] { 31, 11 });

            migrationBuilder.UpdateData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 402,
                columns: new[] { "BossId", "SpecializationId" },
                values: new object[] { 31, 12 });

            migrationBuilder.UpdateData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 403,
                columns: new[] { "BossId", "SpecializationId" },
                values: new object[] { 31, 13 });

            migrationBuilder.UpdateData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 404,
                columns: new[] { "BossId", "SpecializationId" },
                values: new object[] { 32, 1 });

            migrationBuilder.UpdateData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 405,
                columns: new[] { "BossId", "SpecializationId" },
                values: new object[] { 32, 2 });

            migrationBuilder.UpdateData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 406,
                columns: new[] { "BossId", "SpecializationId" },
                values: new object[] { 32, 3 });

            migrationBuilder.UpdateData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 407,
                columns: new[] { "BossId", "SpecializationId" },
                values: new object[] { 32, 4 });

            migrationBuilder.UpdateData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 408,
                columns: new[] { "BossId", "SpecializationId" },
                values: new object[] { 32, 5 });

            migrationBuilder.UpdateData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 409,
                columns: new[] { "BossId", "SpecializationId" },
                values: new object[] { 32, 6 });

            migrationBuilder.UpdateData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 410,
                columns: new[] { "BossId", "SpecializationId" },
                values: new object[] { 32, 7 });

            migrationBuilder.UpdateData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 411,
                columns: new[] { "BossId", "SpecializationId" },
                values: new object[] { 32, 8 });

            migrationBuilder.UpdateData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 412,
                columns: new[] { "BossId", "SpecializationId" },
                values: new object[] { 32, 9 });

            migrationBuilder.UpdateData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 413,
                columns: new[] { "BossId", "SpecializationId" },
                values: new object[] { 32, 10 });

            migrationBuilder.UpdateData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 414,
                columns: new[] { "BossId", "SpecializationId" },
                values: new object[] { 32, 11 });

            migrationBuilder.UpdateData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 415,
                columns: new[] { "BossId", "SpecializationId" },
                values: new object[] { 32, 12 });

            migrationBuilder.UpdateData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 416,
                columns: new[] { "BossId", "SpecializationId" },
                values: new object[] { 32, 13 });

            migrationBuilder.UpdateData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 417,
                columns: new[] { "BossId", "SpecializationId" },
                values: new object[] { 33, 1 });

            migrationBuilder.UpdateData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 418,
                columns: new[] { "BossId", "SpecializationId" },
                values: new object[] { 33, 2 });

            migrationBuilder.UpdateData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 419,
                columns: new[] { "BossId", "SpecializationId" },
                values: new object[] { 33, 3 });

            migrationBuilder.UpdateData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 420,
                columns: new[] { "BossId", "SpecializationId" },
                values: new object[] { 33, 4 });

            migrationBuilder.UpdateData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 421,
                columns: new[] { "BossId", "SpecializationId" },
                values: new object[] { 33, 5 });

            migrationBuilder.UpdateData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 422,
                columns: new[] { "BossId", "SpecializationId" },
                values: new object[] { 33, 6 });

            migrationBuilder.UpdateData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 423,
                columns: new[] { "BossId", "SpecializationId" },
                values: new object[] { 33, 7 });

            migrationBuilder.UpdateData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 424,
                columns: new[] { "BossId", "SpecializationId" },
                values: new object[] { 33, 8 });

            migrationBuilder.UpdateData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 425,
                columns: new[] { "BossId", "SpecializationId" },
                values: new object[] { 33, 9 });

            migrationBuilder.UpdateData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 426,
                columns: new[] { "BossId", "SpecializationId" },
                values: new object[] { 33, 10 });

            migrationBuilder.UpdateData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 427,
                columns: new[] { "BossId", "SpecializationId" },
                values: new object[] { 33, 11 });

            migrationBuilder.UpdateData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 428,
                columns: new[] { "BossId", "SpecializationId" },
                values: new object[] { 33, 12 });

            migrationBuilder.UpdateData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 429,
                columns: new[] { "BossId", "SpecializationId" },
                values: new object[] { 33, 13 });

            migrationBuilder.UpdateData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 430,
                columns: new[] { "BossId", "SpecializationId" },
                values: new object[] { 34, 1 });

            migrationBuilder.UpdateData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 431,
                columns: new[] { "BossId", "SpecializationId" },
                values: new object[] { 34, 2 });

            migrationBuilder.UpdateData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 432,
                columns: new[] { "BossId", "SpecializationId" },
                values: new object[] { 34, 3 });

            migrationBuilder.UpdateData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 433,
                columns: new[] { "BossId", "SpecializationId" },
                values: new object[] { 34, 4 });

            migrationBuilder.UpdateData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 434,
                columns: new[] { "BossId", "SpecializationId" },
                values: new object[] { 34, 5 });

            migrationBuilder.UpdateData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 435,
                columns: new[] { "BossId", "SpecializationId" },
                values: new object[] { 34, 6 });

            migrationBuilder.UpdateData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 436,
                columns: new[] { "BossId", "SpecializationId" },
                values: new object[] { 34, 7 });

            migrationBuilder.UpdateData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 437,
                columns: new[] { "BossId", "SpecializationId" },
                values: new object[] { 34, 8 });

            migrationBuilder.UpdateData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 438,
                columns: new[] { "BossId", "SpecializationId" },
                values: new object[] { 34, 9 });

            migrationBuilder.UpdateData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 439,
                columns: new[] { "BossId", "SpecializationId" },
                values: new object[] { 34, 10 });

            migrationBuilder.UpdateData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 440,
                columns: new[] { "BossId", "SpecializationId" },
                values: new object[] { 34, 11 });

            migrationBuilder.UpdateData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 441,
                columns: new[] { "BossId", "SpecializationId" },
                values: new object[] { 34, 12 });

            migrationBuilder.UpdateData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 442,
                columns: new[] { "BossId", "SpecializationId" },
                values: new object[] { 34, 13 });

            migrationBuilder.UpdateData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 443,
                column: "BossId",
                value: 35);

            migrationBuilder.UpdateData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 444,
                column: "BossId",
                value: 35);

            migrationBuilder.UpdateData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 445,
                column: "BossId",
                value: 35);

            migrationBuilder.UpdateData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 446,
                column: "BossId",
                value: 35);

            migrationBuilder.UpdateData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 447,
                column: "BossId",
                value: 35);

            migrationBuilder.UpdateData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 448,
                column: "BossId",
                value: 35);

            migrationBuilder.UpdateData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 449,
                column: "BossId",
                value: 35);

            migrationBuilder.UpdateData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 450,
                column: "BossId",
                value: 35);

            migrationBuilder.UpdateData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 451,
                column: "BossId",
                value: 35);

            migrationBuilder.UpdateData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 452,
                column: "BossId",
                value: 35);

            migrationBuilder.UpdateData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 453,
                column: "BossId",
                value: 35);

            migrationBuilder.UpdateData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 454,
                column: "BossId",
                value: 35);

            migrationBuilder.UpdateData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 455,
                column: "BossId",
                value: 35);

            migrationBuilder.UpdateData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 456,
                columns: new[] { "BossId", "SpecializationId" },
                values: new object[] { 36, 1 });

            migrationBuilder.UpdateData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 457,
                columns: new[] { "BossId", "SpecializationId" },
                values: new object[] { 36, 2 });

            migrationBuilder.UpdateData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 458,
                columns: new[] { "BossId", "SpecializationId" },
                values: new object[] { 36, 3 });

            migrationBuilder.UpdateData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 459,
                columns: new[] { "BossId", "SpecializationId" },
                values: new object[] { 36, 4 });

            migrationBuilder.UpdateData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 460,
                columns: new[] { "BossId", "SpecializationId" },
                values: new object[] { 36, 5 });

            migrationBuilder.UpdateData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 461,
                columns: new[] { "BossId", "SpecializationId" },
                values: new object[] { 36, 6 });

            migrationBuilder.UpdateData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 462,
                columns: new[] { "BossId", "SpecializationId" },
                values: new object[] { 36, 7 });

            migrationBuilder.UpdateData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 463,
                columns: new[] { "BossId", "SpecializationId" },
                values: new object[] { 36, 8 });

            migrationBuilder.UpdateData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 464,
                columns: new[] { "BossId", "SpecializationId" },
                values: new object[] { 36, 9 });

            migrationBuilder.UpdateData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 465,
                columns: new[] { "BossId", "SpecializationId" },
                values: new object[] { 36, 10 });

            migrationBuilder.UpdateData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 466,
                columns: new[] { "BossId", "SpecializationId" },
                values: new object[] { 36, 11 });

            migrationBuilder.UpdateData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 467,
                columns: new[] { "BossId", "SpecializationId" },
                values: new object[] { 36, 12 });

            migrationBuilder.UpdateData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 468,
                columns: new[] { "BossId", "SpecializationId" },
                values: new object[] { 36, 13 });

            migrationBuilder.UpdateData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 469,
                columns: new[] { "BossId", "SpecializationId" },
                values: new object[] { 37, 1 });

            migrationBuilder.UpdateData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 470,
                columns: new[] { "BossId", "SpecializationId" },
                values: new object[] { 37, 2 });

            migrationBuilder.UpdateData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 471,
                columns: new[] { "BossId", "SpecializationId" },
                values: new object[] { 37, 3 });

            migrationBuilder.UpdateData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 472,
                columns: new[] { "BossId", "SpecializationId" },
                values: new object[] { 37, 4 });

            migrationBuilder.UpdateData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 473,
                columns: new[] { "BossId", "SpecializationId" },
                values: new object[] { 37, 5 });

            migrationBuilder.UpdateData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 474,
                columns: new[] { "BossId", "SpecializationId" },
                values: new object[] { 37, 6 });

            migrationBuilder.UpdateData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 475,
                columns: new[] { "BossId", "SpecializationId" },
                values: new object[] { 37, 7 });

            migrationBuilder.UpdateData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 476,
                columns: new[] { "BossId", "SpecializationId" },
                values: new object[] { 37, 8 });

            migrationBuilder.UpdateData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 477,
                columns: new[] { "BossId", "SpecializationId" },
                values: new object[] { 37, 9 });

            migrationBuilder.UpdateData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 478,
                columns: new[] { "BossId", "SpecializationId" },
                values: new object[] { 37, 10 });

            migrationBuilder.UpdateData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 479,
                columns: new[] { "BossId", "SpecializationId" },
                values: new object[] { 37, 11 });

            migrationBuilder.UpdateData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 480,
                columns: new[] { "BossId", "SpecializationId" },
                values: new object[] { 37, 12 });

            migrationBuilder.UpdateData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 481,
                columns: new[] { "BossId", "SpecializationId" },
                values: new object[] { 37, 13 });

            migrationBuilder.UpdateData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 482,
                columns: new[] { "BossId", "SpecializationId" },
                values: new object[] { 38, 1 });

            migrationBuilder.UpdateData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 483,
                columns: new[] { "BossId", "SpecializationId" },
                values: new object[] { 38, 2 });

            migrationBuilder.UpdateData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 484,
                columns: new[] { "BossId", "SpecializationId" },
                values: new object[] { 38, 3 });

            migrationBuilder.UpdateData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 485,
                columns: new[] { "BossId", "SpecializationId" },
                values: new object[] { 38, 4 });

            migrationBuilder.UpdateData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 486,
                columns: new[] { "BossId", "SpecializationId" },
                values: new object[] { 38, 5 });

            migrationBuilder.UpdateData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 487,
                columns: new[] { "BossId", "SpecializationId" },
                values: new object[] { 38, 6 });

            migrationBuilder.UpdateData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 488,
                columns: new[] { "BossId", "SpecializationId" },
                values: new object[] { 38, 7 });

            migrationBuilder.UpdateData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 489,
                columns: new[] { "BossId", "SpecializationId" },
                values: new object[] { 38, 8 });

            migrationBuilder.UpdateData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 490,
                columns: new[] { "BossId", "SpecializationId" },
                values: new object[] { 38, 9 });

            migrationBuilder.UpdateData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 491,
                columns: new[] { "BossId", "SpecializationId" },
                values: new object[] { 38, 10 });

            migrationBuilder.UpdateData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 492,
                columns: new[] { "BossId", "SpecializationId" },
                values: new object[] { 38, 11 });

            migrationBuilder.UpdateData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 493,
                columns: new[] { "BossId", "SpecializationId" },
                values: new object[] { 38, 12 });

            migrationBuilder.UpdateData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 494,
                columns: new[] { "BossId", "SpecializationId" },
                values: new object[] { 38, 13 });

            migrationBuilder.UpdateData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 495,
                columns: new[] { "BossId", "SpecializationId" },
                values: new object[] { 39, 1 });

            migrationBuilder.UpdateData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 496,
                columns: new[] { "BossId", "SpecializationId" },
                values: new object[] { 39, 2 });

            migrationBuilder.UpdateData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 497,
                columns: new[] { "BossId", "SpecializationId" },
                values: new object[] { 39, 3 });

            migrationBuilder.UpdateData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 498,
                columns: new[] { "BossId", "SpecializationId" },
                values: new object[] { 39, 4 });

            migrationBuilder.UpdateData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 499,
                columns: new[] { "BossId", "SpecializationId" },
                values: new object[] { 39, 5 });

            migrationBuilder.UpdateData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 500,
                columns: new[] { "BossId", "SpecializationId" },
                values: new object[] { 39, 6 });

            migrationBuilder.UpdateData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 501,
                columns: new[] { "BossId", "SpecializationId" },
                values: new object[] { 39, 7 });

            migrationBuilder.UpdateData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 502,
                columns: new[] { "BossId", "SpecializationId" },
                values: new object[] { 39, 8 });

            migrationBuilder.UpdateData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 503,
                columns: new[] { "BossId", "SpecializationId" },
                values: new object[] { 39, 9 });

            migrationBuilder.UpdateData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 504,
                columns: new[] { "BossId", "SpecializationId" },
                values: new object[] { 39, 10 });

            migrationBuilder.UpdateData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 505,
                columns: new[] { "BossId", "SpecializationId" },
                values: new object[] { 39, 11 });

            migrationBuilder.UpdateData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 506,
                columns: new[] { "BossId", "SpecializationId" },
                values: new object[] { 39, 12 });

            migrationBuilder.UpdateData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 507,
                columns: new[] { "BossId", "SpecializationId" },
                values: new object[] { 39, 13 });

            migrationBuilder.UpdateData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 508,
                columns: new[] { "BossId", "SpecializationId" },
                values: new object[] { 40, 1 });

            migrationBuilder.UpdateData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 509,
                columns: new[] { "BossId", "SpecializationId" },
                values: new object[] { 40, 2 });

            migrationBuilder.UpdateData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 510,
                columns: new[] { "BossId", "SpecializationId" },
                values: new object[] { 40, 3 });

            migrationBuilder.UpdateData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 511,
                columns: new[] { "BossId", "SpecializationId" },
                values: new object[] { 40, 4 });

            migrationBuilder.UpdateData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 512,
                columns: new[] { "BossId", "SpecializationId" },
                values: new object[] { 40, 5 });

            migrationBuilder.UpdateData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 513,
                columns: new[] { "BossId", "SpecializationId" },
                values: new object[] { 40, 6 });

            migrationBuilder.UpdateData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 514,
                columns: new[] { "BossId", "SpecializationId" },
                values: new object[] { 40, 7 });

            migrationBuilder.UpdateData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 515,
                columns: new[] { "BossId", "SpecializationId" },
                values: new object[] { 40, 8 });

            migrationBuilder.UpdateData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 516,
                columns: new[] { "BossId", "SpecializationId" },
                values: new object[] { 40, 9 });

            migrationBuilder.UpdateData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 517,
                columns: new[] { "BossId", "SpecializationId" },
                values: new object[] { 40, 10 });

            migrationBuilder.UpdateData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 518,
                columns: new[] { "BossId", "SpecializationId" },
                values: new object[] { 40, 11 });

            migrationBuilder.UpdateData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 519,
                columns: new[] { "BossId", "SpecializationId" },
                values: new object[] { 40, 12 });

            migrationBuilder.UpdateData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 520,
                columns: new[] { "BossId", "SpecializationId" },
                values: new object[] { 40, 13 });

            migrationBuilder.UpdateData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 521,
                columns: new[] { "BossId", "SpecializationId" },
                values: new object[] { 41, 1 });

            migrationBuilder.UpdateData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 522,
                columns: new[] { "BossId", "SpecializationId" },
                values: new object[] { 41, 2 });

            migrationBuilder.UpdateData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 523,
                columns: new[] { "BossId", "SpecializationId" },
                values: new object[] { 41, 3 });

            migrationBuilder.UpdateData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 524,
                columns: new[] { "BossId", "SpecializationId" },
                values: new object[] { 41, 4 });

            migrationBuilder.UpdateData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 525,
                columns: new[] { "BossId", "SpecializationId" },
                values: new object[] { 41, 5 });

            migrationBuilder.UpdateData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 526,
                columns: new[] { "BossId", "SpecializationId" },
                values: new object[] { 41, 6 });

            migrationBuilder.UpdateData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 527,
                columns: new[] { "BossId", "SpecializationId" },
                values: new object[] { 41, 7 });

            migrationBuilder.UpdateData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 528,
                columns: new[] { "BossId", "SpecializationId" },
                values: new object[] { 41, 8 });

            migrationBuilder.UpdateData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 529,
                columns: new[] { "BossId", "SpecializationId" },
                values: new object[] { 41, 9 });

            migrationBuilder.UpdateData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 530,
                columns: new[] { "BossId", "SpecializationId" },
                values: new object[] { 41, 10 });

            migrationBuilder.UpdateData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 531,
                columns: new[] { "BossId", "SpecializationId" },
                values: new object[] { 41, 11 });

            migrationBuilder.UpdateData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 532,
                columns: new[] { "BossId", "SpecializationId" },
                values: new object[] { 41, 12 });

            migrationBuilder.UpdateData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 533,
                columns: new[] { "BossId", "SpecializationId" },
                values: new object[] { 41, 13 });

            migrationBuilder.UpdateData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 534,
                columns: new[] { "BossId", "SpecializationId" },
                values: new object[] { 42, 1 });

            migrationBuilder.UpdateData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 535,
                columns: new[] { "BossId", "SpecializationId" },
                values: new object[] { 42, 2 });

            migrationBuilder.UpdateData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 536,
                columns: new[] { "BossId", "SpecializationId" },
                values: new object[] { 42, 3 });

            migrationBuilder.UpdateData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 537,
                columns: new[] { "BossId", "SpecializationId" },
                values: new object[] { 42, 4 });

            migrationBuilder.UpdateData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 538,
                columns: new[] { "BossId", "SpecializationId" },
                values: new object[] { 42, 5 });

            migrationBuilder.UpdateData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 539,
                columns: new[] { "BossId", "SpecializationId" },
                values: new object[] { 42, 6 });

            migrationBuilder.UpdateData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 540,
                columns: new[] { "BossId", "SpecializationId" },
                values: new object[] { 42, 7 });

            migrationBuilder.UpdateData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 541,
                columns: new[] { "BossId", "SpecializationId" },
                values: new object[] { 42, 8 });

            migrationBuilder.UpdateData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 542,
                columns: new[] { "BossId", "SpecializationId" },
                values: new object[] { 42, 9 });

            migrationBuilder.UpdateData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 543,
                columns: new[] { "BossId", "SpecializationId" },
                values: new object[] { 42, 10 });

            migrationBuilder.UpdateData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 544,
                columns: new[] { "BossId", "SpecializationId" },
                values: new object[] { 42, 11 });

            migrationBuilder.UpdateData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 545,
                columns: new[] { "BossId", "SpecializationId" },
                values: new object[] { 42, 12 });

            migrationBuilder.UpdateData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 546,
                columns: new[] { "BossId", "SpecializationId" },
                values: new object[] { 42, 13 });

            migrationBuilder.UpdateData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 547,
                columns: new[] { "BossId", "SpecializationId" },
                values: new object[] { 43, 1 });

            migrationBuilder.UpdateData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 548,
                columns: new[] { "BossId", "SpecializationId" },
                values: new object[] { 43, 2 });

            migrationBuilder.UpdateData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 549,
                columns: new[] { "BossId", "SpecializationId" },
                values: new object[] { 43, 3 });

            migrationBuilder.UpdateData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 550,
                columns: new[] { "BossId", "SpecializationId" },
                values: new object[] { 43, 4 });

            migrationBuilder.UpdateData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 551,
                columns: new[] { "BossId", "SpecializationId" },
                values: new object[] { 43, 5 });

            migrationBuilder.UpdateData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 552,
                columns: new[] { "BossId", "SpecializationId" },
                values: new object[] { 43, 6 });

            migrationBuilder.UpdateData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 553,
                columns: new[] { "BossId", "SpecializationId" },
                values: new object[] { 43, 7 });

            migrationBuilder.UpdateData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 554,
                columns: new[] { "BossId", "SpecializationId" },
                values: new object[] { 43, 8 });

            migrationBuilder.UpdateData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 555,
                columns: new[] { "BossId", "SpecializationId" },
                values: new object[] { 43, 9 });

            migrationBuilder.UpdateData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 556,
                columns: new[] { "BossId", "SpecializationId" },
                values: new object[] { 43, 10 });

            migrationBuilder.UpdateData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 557,
                columns: new[] { "BossId", "SpecializationId" },
                values: new object[] { 43, 11 });

            migrationBuilder.UpdateData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 558,
                columns: new[] { "BossId", "SpecializationId" },
                values: new object[] { 43, 12 });

            migrationBuilder.UpdateData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 559,
                columns: new[] { "BossId", "SpecializationId" },
                values: new object[] { 43, 13 });

            migrationBuilder.UpdateData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 560,
                columns: new[] { "BossId", "SpecializationId" },
                values: new object[] { 44, 1 });

            migrationBuilder.UpdateData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 561,
                columns: new[] { "BossId", "SpecializationId" },
                values: new object[] { 44, 2 });

            migrationBuilder.UpdateData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 562,
                columns: new[] { "BossId", "SpecializationId" },
                values: new object[] { 44, 3 });

            migrationBuilder.UpdateData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 563,
                columns: new[] { "BossId", "SpecializationId" },
                values: new object[] { 44, 4 });

            migrationBuilder.UpdateData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 564,
                columns: new[] { "BossId", "SpecializationId" },
                values: new object[] { 44, 5 });

            migrationBuilder.UpdateData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 565,
                columns: new[] { "BossId", "SpecializationId" },
                values: new object[] { 44, 6 });

            migrationBuilder.UpdateData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 566,
                columns: new[] { "BossId", "SpecializationId" },
                values: new object[] { 44, 7 });

            migrationBuilder.UpdateData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 567,
                columns: new[] { "BossId", "SpecializationId" },
                values: new object[] { 44, 8 });

            migrationBuilder.UpdateData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 568,
                columns: new[] { "BossId", "SpecializationId" },
                values: new object[] { 44, 9 });

            migrationBuilder.UpdateData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 569,
                columns: new[] { "BossId", "SpecializationId" },
                values: new object[] { 44, 10 });

            migrationBuilder.UpdateData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 570,
                columns: new[] { "BossId", "SpecializationId" },
                values: new object[] { 44, 11 });

            migrationBuilder.UpdateData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 571,
                columns: new[] { "BossId", "SpecializationId" },
                values: new object[] { 44, 12 });

            migrationBuilder.UpdateData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 572,
                columns: new[] { "BossId", "SpecializationId" },
                values: new object[] { 44, 13 });

            migrationBuilder.UpdateData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 573,
                columns: new[] { "BossId", "SpecializationId" },
                values: new object[] { 45, 1 });

            migrationBuilder.UpdateData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 574,
                columns: new[] { "BossId", "SpecializationId" },
                values: new object[] { 45, 2 });

            migrationBuilder.UpdateData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 575,
                columns: new[] { "BossId", "SpecializationId" },
                values: new object[] { 45, 3 });

            migrationBuilder.UpdateData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 576,
                columns: new[] { "BossId", "SpecializationId" },
                values: new object[] { 45, 4 });

            migrationBuilder.UpdateData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 577,
                columns: new[] { "BossId", "SpecializationId" },
                values: new object[] { 45, 5 });

            migrationBuilder.UpdateData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 578,
                columns: new[] { "BossId", "SpecializationId" },
                values: new object[] { 45, 6 });

            migrationBuilder.UpdateData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 579,
                columns: new[] { "BossId", "SpecializationId" },
                values: new object[] { 45, 7 });

            migrationBuilder.UpdateData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 580,
                columns: new[] { "BossId", "SpecializationId" },
                values: new object[] { 45, 8 });

            migrationBuilder.UpdateData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 581,
                columns: new[] { "BossId", "SpecializationId" },
                values: new object[] { 45, 9 });

            migrationBuilder.UpdateData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 582,
                columns: new[] { "BossId", "SpecializationId" },
                values: new object[] { 45, 10 });

            migrationBuilder.UpdateData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 583,
                columns: new[] { "BossId", "SpecializationId" },
                values: new object[] { 45, 11 });

            migrationBuilder.UpdateData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 584,
                columns: new[] { "BossId", "SpecializationId" },
                values: new object[] { 45, 12 });

            migrationBuilder.UpdateData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 585,
                columns: new[] { "BossId", "SpecializationId" },
                values: new object[] { 45, 13 });

            migrationBuilder.UpdateData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 586,
                columns: new[] { "BossId", "SpecializationId" },
                values: new object[] { 46, 1 });

            migrationBuilder.UpdateData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 587,
                columns: new[] { "BossId", "SpecializationId" },
                values: new object[] { 46, 2 });

            migrationBuilder.UpdateData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 588,
                columns: new[] { "BossId", "SpecializationId" },
                values: new object[] { 46, 3 });

            migrationBuilder.UpdateData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 589,
                columns: new[] { "BossId", "SpecializationId" },
                values: new object[] { 46, 4 });

            migrationBuilder.UpdateData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 590,
                columns: new[] { "BossId", "SpecializationId" },
                values: new object[] { 46, 5 });

            migrationBuilder.UpdateData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 591,
                columns: new[] { "BossId", "SpecializationId" },
                values: new object[] { 46, 6 });

            migrationBuilder.UpdateData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 592,
                columns: new[] { "BossId", "SpecializationId" },
                values: new object[] { 46, 7 });

            migrationBuilder.UpdateData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 593,
                columns: new[] { "BossId", "SpecializationId" },
                values: new object[] { 46, 8 });

            migrationBuilder.UpdateData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 594,
                columns: new[] { "BossId", "SpecializationId" },
                values: new object[] { 46, 9 });

            migrationBuilder.UpdateData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 595,
                columns: new[] { "BossId", "SpecializationId" },
                values: new object[] { 46, 10 });

            migrationBuilder.UpdateData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 596,
                columns: new[] { "BossId", "SpecializationId" },
                values: new object[] { 46, 11 });

            migrationBuilder.UpdateData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 597,
                columns: new[] { "BossId", "SpecializationId" },
                values: new object[] { 46, 12 });

            migrationBuilder.UpdateData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 598,
                columns: new[] { "BossId", "SpecializationId" },
                values: new object[] { 46, 13 });

            migrationBuilder.UpdateData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 599,
                columns: new[] { "BossId", "SpecializationId" },
                values: new object[] { 47, 1 });

            migrationBuilder.UpdateData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 600,
                columns: new[] { "BossId", "SpecializationId" },
                values: new object[] { 47, 2 });

            migrationBuilder.UpdateData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 601,
                columns: new[] { "BossId", "SpecializationId" },
                values: new object[] { 47, 3 });

            migrationBuilder.UpdateData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 602,
                columns: new[] { "BossId", "SpecializationId" },
                values: new object[] { 47, 4 });

            migrationBuilder.UpdateData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 603,
                columns: new[] { "BossId", "SpecializationId" },
                values: new object[] { 47, 5 });

            migrationBuilder.UpdateData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 604,
                columns: new[] { "BossId", "SpecializationId" },
                values: new object[] { 47, 6 });

            migrationBuilder.UpdateData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 605,
                columns: new[] { "BossId", "SpecializationId" },
                values: new object[] { 47, 7 });

            migrationBuilder.UpdateData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 606,
                columns: new[] { "BossId", "SpecializationId" },
                values: new object[] { 47, 8 });

            migrationBuilder.UpdateData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 607,
                columns: new[] { "BossId", "SpecializationId" },
                values: new object[] { 47, 9 });

            migrationBuilder.UpdateData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 608,
                columns: new[] { "BossId", "SpecializationId" },
                values: new object[] { 47, 10 });

            migrationBuilder.UpdateData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 609,
                columns: new[] { "BossId", "SpecializationId" },
                values: new object[] { 47, 11 });

            migrationBuilder.UpdateData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 610,
                columns: new[] { "BossId", "SpecializationId" },
                values: new object[] { 47, 12 });

            migrationBuilder.UpdateData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 611,
                columns: new[] { "BossId", "SpecializationId" },
                values: new object[] { 47, 13 });

            migrationBuilder.UpdateData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 612,
                columns: new[] { "BossId", "SpecializationId" },
                values: new object[] { 48, 1 });

            migrationBuilder.UpdateData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 613,
                columns: new[] { "BossId", "SpecializationId" },
                values: new object[] { 48, 2 });

            migrationBuilder.UpdateData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 614,
                columns: new[] { "BossId", "SpecializationId" },
                values: new object[] { 48, 3 });

            migrationBuilder.UpdateData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 615,
                columns: new[] { "BossId", "SpecializationId" },
                values: new object[] { 48, 4 });

            migrationBuilder.UpdateData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 616,
                columns: new[] { "BossId", "SpecializationId" },
                values: new object[] { 48, 5 });

            migrationBuilder.UpdateData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 617,
                columns: new[] { "BossId", "SpecializationId" },
                values: new object[] { 48, 6 });

            migrationBuilder.UpdateData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 618,
                columns: new[] { "BossId", "SpecializationId" },
                values: new object[] { 48, 7 });

            migrationBuilder.UpdateData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 619,
                columns: new[] { "BossId", "SpecializationId" },
                values: new object[] { 48, 8 });

            migrationBuilder.UpdateData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 620,
                columns: new[] { "BossId", "SpecializationId" },
                values: new object[] { 48, 9 });

            migrationBuilder.UpdateData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 621,
                columns: new[] { "BossId", "SpecializationId" },
                values: new object[] { 48, 10 });

            migrationBuilder.UpdateData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 622,
                columns: new[] { "BossId", "SpecializationId" },
                values: new object[] { 48, 11 });

            migrationBuilder.UpdateData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 623,
                columns: new[] { "BossId", "SpecializationId" },
                values: new object[] { 48, 12 });

            migrationBuilder.UpdateData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 624,
                columns: new[] { "BossId", "SpecializationId" },
                values: new object[] { 48, 13 });

            migrationBuilder.UpdateData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 625,
                columns: new[] { "BossId", "SpecializationId" },
                values: new object[] { 49, 1 });

            migrationBuilder.UpdateData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 626,
                columns: new[] { "BossId", "SpecializationId" },
                values: new object[] { 49, 2 });

            migrationBuilder.UpdateData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 627,
                columns: new[] { "BossId", "SpecializationId" },
                values: new object[] { 49, 3 });

            migrationBuilder.UpdateData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 628,
                columns: new[] { "BossId", "SpecializationId" },
                values: new object[] { 49, 4 });

            migrationBuilder.UpdateData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 629,
                columns: new[] { "BossId", "SpecializationId" },
                values: new object[] { 49, 5 });

            migrationBuilder.UpdateData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 630,
                columns: new[] { "BossId", "SpecializationId" },
                values: new object[] { 49, 6 });

            migrationBuilder.UpdateData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 631,
                columns: new[] { "BossId", "SpecializationId" },
                values: new object[] { 49, 7 });

            migrationBuilder.UpdateData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 632,
                columns: new[] { "BossId", "SpecializationId" },
                values: new object[] { 49, 8 });

            migrationBuilder.UpdateData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 633,
                columns: new[] { "BossId", "SpecializationId" },
                values: new object[] { 49, 9 });

            migrationBuilder.UpdateData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 634,
                columns: new[] { "BossId", "SpecializationId" },
                values: new object[] { 49, 10 });

            migrationBuilder.UpdateData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 635,
                columns: new[] { "BossId", "SpecializationId" },
                values: new object[] { 49, 11 });

            migrationBuilder.UpdateData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 636,
                columns: new[] { "BossId", "SpecializationId" },
                values: new object[] { 49, 12 });

            migrationBuilder.UpdateData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 637,
                columns: new[] { "BossId", "SpecializationId" },
                values: new object[] { 49, 13 });

            migrationBuilder.UpdateData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 638,
                columns: new[] { "BossId", "SpecializationId" },
                values: new object[] { 50, 1 });

            migrationBuilder.UpdateData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 639,
                columns: new[] { "BossId", "SpecializationId" },
                values: new object[] { 50, 2 });

            migrationBuilder.UpdateData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 640,
                columns: new[] { "BossId", "SpecializationId" },
                values: new object[] { 50, 3 });

            migrationBuilder.UpdateData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 641,
                columns: new[] { "BossId", "SpecializationId" },
                values: new object[] { 50, 4 });

            migrationBuilder.UpdateData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 642,
                columns: new[] { "BossId", "SpecializationId" },
                values: new object[] { 50, 5 });

            migrationBuilder.UpdateData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 643,
                columns: new[] { "BossId", "SpecializationId" },
                values: new object[] { 50, 6 });

            migrationBuilder.UpdateData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 644,
                columns: new[] { "BossId", "SpecializationId" },
                values: new object[] { 50, 7 });

            migrationBuilder.UpdateData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 645,
                columns: new[] { "BossId", "SpecializationId" },
                values: new object[] { 50, 8 });

            migrationBuilder.UpdateData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 646,
                columns: new[] { "BossId", "SpecializationId" },
                values: new object[] { 50, 9 });

            migrationBuilder.UpdateData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 647,
                columns: new[] { "BossId", "SpecializationId" },
                values: new object[] { 50, 10 });

            migrationBuilder.UpdateData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 648,
                columns: new[] { "BossId", "SpecializationId" },
                values: new object[] { 50, 11 });

            migrationBuilder.UpdateData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 649,
                columns: new[] { "BossId", "SpecializationId" },
                values: new object[] { 50, 12 });

            migrationBuilder.UpdateData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 650,
                columns: new[] { "BossId", "SpecializationId" },
                values: new object[] { 50, 13 });

            migrationBuilder.UpdateData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 651,
                columns: new[] { "BossId", "SpecializationId" },
                values: new object[] { 51, 1 });

            migrationBuilder.UpdateData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 652,
                columns: new[] { "BossId", "SpecializationId" },
                values: new object[] { 51, 2 });

            migrationBuilder.UpdateData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 653,
                columns: new[] { "BossId", "SpecializationId" },
                values: new object[] { 51, 3 });

            migrationBuilder.UpdateData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 654,
                columns: new[] { "BossId", "SpecializationId" },
                values: new object[] { 51, 4 });

            migrationBuilder.UpdateData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 655,
                columns: new[] { "BossId", "SpecializationId" },
                values: new object[] { 51, 5 });

            migrationBuilder.UpdateData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 656,
                columns: new[] { "BossId", "SpecializationId" },
                values: new object[] { 51, 6 });

            migrationBuilder.UpdateData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 657,
                columns: new[] { "BossId", "SpecializationId" },
                values: new object[] { 51, 7 });

            migrationBuilder.UpdateData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 658,
                columns: new[] { "BossId", "SpecializationId" },
                values: new object[] { 51, 8 });

            migrationBuilder.UpdateData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 659,
                columns: new[] { "BossId", "SpecializationId" },
                values: new object[] { 51, 9 });

            migrationBuilder.UpdateData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 660,
                columns: new[] { "BossId", "SpecializationId" },
                values: new object[] { 51, 10 });

            migrationBuilder.UpdateData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 661,
                columns: new[] { "BossId", "SpecializationId" },
                values: new object[] { 51, 11 });

            migrationBuilder.UpdateData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 662,
                columns: new[] { "BossId", "SpecializationId" },
                values: new object[] { 51, 12 });

            migrationBuilder.UpdateData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 663,
                columns: new[] { "BossId", "SpecializationId" },
                values: new object[] { 51, 13 });

            migrationBuilder.UpdateData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 664,
                column: "BossId",
                value: 52);

            migrationBuilder.UpdateData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 665,
                column: "BossId",
                value: 52);

            migrationBuilder.UpdateData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 666,
                column: "BossId",
                value: 52);

            migrationBuilder.UpdateData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 667,
                column: "BossId",
                value: 52);

            migrationBuilder.UpdateData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 668,
                column: "BossId",
                value: 52);

            migrationBuilder.UpdateData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 669,
                column: "BossId",
                value: 52);

            migrationBuilder.UpdateData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 670,
                column: "BossId",
                value: 52);

            migrationBuilder.UpdateData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 671,
                column: "BossId",
                value: 52);

            migrationBuilder.UpdateData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 672,
                column: "BossId",
                value: 52);

            migrationBuilder.UpdateData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 673,
                column: "BossId",
                value: 52);

            migrationBuilder.UpdateData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 674,
                column: "BossId",
                value: 52);

            migrationBuilder.UpdateData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 675,
                column: "BossId",
                value: 52);

            migrationBuilder.UpdateData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 676,
                column: "BossId",
                value: 52);

            migrationBuilder.UpdateData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 677,
                columns: new[] { "BossId", "SpecializationId" },
                values: new object[] { 53, 1 });

            migrationBuilder.UpdateData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 678,
                columns: new[] { "BossId", "SpecializationId" },
                values: new object[] { 53, 2 });

            migrationBuilder.UpdateData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 679,
                columns: new[] { "BossId", "SpecializationId" },
                values: new object[] { 53, 3 });

            migrationBuilder.UpdateData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 680,
                columns: new[] { "BossId", "SpecializationId" },
                values: new object[] { 53, 4 });

            migrationBuilder.UpdateData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 681,
                columns: new[] { "BossId", "SpecializationId" },
                values: new object[] { 53, 5 });

            migrationBuilder.UpdateData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 682,
                columns: new[] { "BossId", "SpecializationId" },
                values: new object[] { 53, 6 });

            migrationBuilder.UpdateData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 683,
                columns: new[] { "BossId", "SpecializationId" },
                values: new object[] { 53, 7 });

            migrationBuilder.UpdateData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 684,
                columns: new[] { "BossId", "SpecializationId" },
                values: new object[] { 53, 8 });

            migrationBuilder.UpdateData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 685,
                columns: new[] { "BossId", "SpecializationId" },
                values: new object[] { 53, 9 });

            migrationBuilder.UpdateData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 686,
                columns: new[] { "BossId", "SpecializationId" },
                values: new object[] { 53, 10 });

            migrationBuilder.UpdateData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 687,
                columns: new[] { "BossId", "SpecializationId" },
                values: new object[] { 53, 11 });

            migrationBuilder.UpdateData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 688,
                columns: new[] { "BossId", "SpecializationId" },
                values: new object[] { 53, 12 });

            migrationBuilder.UpdateData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 689,
                columns: new[] { "BossId", "SpecializationId" },
                values: new object[] { 53, 13 });

            migrationBuilder.UpdateData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 690,
                columns: new[] { "BossId", "SpecializationId" },
                values: new object[] { 54, 1 });

            migrationBuilder.UpdateData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 691,
                columns: new[] { "BossId", "SpecializationId" },
                values: new object[] { 54, 2 });

            migrationBuilder.UpdateData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 692,
                columns: new[] { "BossId", "SpecializationId" },
                values: new object[] { 54, 3 });

            migrationBuilder.UpdateData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 693,
                columns: new[] { "BossId", "SpecializationId" },
                values: new object[] { 54, 4 });

            migrationBuilder.UpdateData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 694,
                columns: new[] { "BossId", "SpecializationId" },
                values: new object[] { 54, 5 });

            migrationBuilder.UpdateData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 695,
                columns: new[] { "BossId", "SpecializationId" },
                values: new object[] { 54, 6 });

            migrationBuilder.UpdateData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 696,
                columns: new[] { "BossId", "SpecializationId" },
                values: new object[] { 54, 7 });

            migrationBuilder.UpdateData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 697,
                columns: new[] { "BossId", "SpecializationId" },
                values: new object[] { 54, 8 });

            migrationBuilder.UpdateData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 698,
                columns: new[] { "BossId", "SpecializationId" },
                values: new object[] { 54, 9 });

            migrationBuilder.UpdateData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 699,
                columns: new[] { "BossId", "SpecializationId" },
                values: new object[] { 54, 10 });

            migrationBuilder.UpdateData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 700,
                columns: new[] { "BossId", "SpecializationId" },
                values: new object[] { 54, 11 });

            migrationBuilder.UpdateData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 701,
                columns: new[] { "BossId", "SpecializationId" },
                values: new object[] { 54, 12 });

            migrationBuilder.UpdateData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 702,
                columns: new[] { "BossId", "SpecializationId" },
                values: new object[] { 54, 13 });

            migrationBuilder.UpdateData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 703,
                columns: new[] { "BossId", "SpecializationId" },
                values: new object[] { 55, 1 });

            migrationBuilder.UpdateData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 704,
                columns: new[] { "BossId", "SpecializationId" },
                values: new object[] { 55, 2 });

            migrationBuilder.UpdateData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 705,
                columns: new[] { "BossId", "SpecializationId" },
                values: new object[] { 55, 3 });

            migrationBuilder.UpdateData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 706,
                columns: new[] { "BossId", "SpecializationId" },
                values: new object[] { 55, 4 });

            migrationBuilder.UpdateData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 707,
                columns: new[] { "BossId", "SpecializationId" },
                values: new object[] { 55, 5 });

            migrationBuilder.UpdateData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 708,
                columns: new[] { "BossId", "SpecializationId" },
                values: new object[] { 55, 6 });

            migrationBuilder.UpdateData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 709,
                columns: new[] { "BossId", "SpecializationId" },
                values: new object[] { 55, 7 });

            migrationBuilder.UpdateData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 710,
                columns: new[] { "BossId", "SpecializationId" },
                values: new object[] { 55, 8 });

            migrationBuilder.UpdateData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 711,
                columns: new[] { "BossId", "SpecializationId" },
                values: new object[] { 55, 9 });

            migrationBuilder.UpdateData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 712,
                columns: new[] { "BossId", "SpecializationId" },
                values: new object[] { 55, 10 });

            migrationBuilder.UpdateData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 713,
                columns: new[] { "BossId", "SpecializationId" },
                values: new object[] { 55, 11 });

            migrationBuilder.UpdateData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 714,
                columns: new[] { "BossId", "SpecializationId" },
                values: new object[] { 55, 12 });

            migrationBuilder.UpdateData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 715,
                columns: new[] { "BossId", "SpecializationId" },
                values: new object[] { 55, 13 });

            migrationBuilder.UpdateData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 716,
                columns: new[] { "BossId", "SpecializationId" },
                values: new object[] { 56, 1 });

            migrationBuilder.UpdateData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 717,
                columns: new[] { "BossId", "SpecializationId" },
                values: new object[] { 56, 2 });

            migrationBuilder.UpdateData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 718,
                columns: new[] { "BossId", "SpecializationId" },
                values: new object[] { 56, 3 });

            migrationBuilder.UpdateData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 719,
                columns: new[] { "BossId", "SpecializationId" },
                values: new object[] { 56, 4 });

            migrationBuilder.UpdateData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 720,
                columns: new[] { "BossId", "SpecializationId" },
                values: new object[] { 56, 5 });

            migrationBuilder.UpdateData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 721,
                columns: new[] { "BossId", "SpecializationId" },
                values: new object[] { 56, 6 });

            migrationBuilder.UpdateData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 722,
                columns: new[] { "BossId", "SpecializationId" },
                values: new object[] { 56, 7 });

            migrationBuilder.UpdateData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 723,
                columns: new[] { "BossId", "SpecializationId" },
                values: new object[] { 56, 8 });

            migrationBuilder.UpdateData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 724,
                columns: new[] { "BossId", "SpecializationId" },
                values: new object[] { 56, 9 });

            migrationBuilder.UpdateData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 725,
                columns: new[] { "BossId", "SpecializationId" },
                values: new object[] { 56, 10 });

            migrationBuilder.UpdateData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 726,
                columns: new[] { "BossId", "SpecializationId" },
                values: new object[] { 56, 11 });

            migrationBuilder.UpdateData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 727,
                columns: new[] { "BossId", "SpecializationId" },
                values: new object[] { 56, 12 });

            migrationBuilder.UpdateData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 728,
                columns: new[] { "BossId", "SpecializationId" },
                values: new object[] { 56, 13 });

            migrationBuilder.UpdateData(
                table: "Specialization",
                keyColumn: "Id",
                keyValue: 7,
                column: "Name",
                value: "Protection");

            migrationBuilder.UpdateData(
                table: "Specialization",
                keyColumn: "Id",
                keyValue: 9,
                column: "SpecializationSpellsId",
                value: "47750,81751,47753");
        }
    }
}
