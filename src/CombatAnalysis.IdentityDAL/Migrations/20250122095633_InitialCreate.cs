using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace CombatAnalysis.IdentityDAL.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "AuthorizationCodeChallenge",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    CodeChallenge = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CodeChallengeMethod = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    RedirectUrl = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CreatedAt = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    ExpiryTime = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    ClientId = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    IsUsed = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AuthorizationCodeChallenge", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Client",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    RedirectUrl = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Scope = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ClientName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ClientType = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CreatedAt = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    UpdatedAt = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Client", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "IdentityUser",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Email = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    PasswordHash = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Salt = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    EmailVerified = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_IdentityUser", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "RefreshToken",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Token = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ExpiryTime = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    ClientId = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    UserId = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RefreshToken", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ResetToken",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Email = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Token = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ExpirationTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    IsUsed = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ResetToken", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "VerifyEmailToken",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Email = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Token = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ExpirationTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    IsUsed = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_VerifyEmailToken", x => x.Id);
                });

            migrationBuilder.InsertData(
                table: "Client",
                columns: new[] { "Id", "ClientName", "ClientType", "CreatedAt", "RedirectUrl", "Scope", "UpdatedAt" },
                values: new object[,]
                {
                    { "client1", "web", "public", new DateTimeOffset(new DateTime(2025, 1, 22, 10, 56, 33, 302, DateTimeKind.Unspecified).AddTicks(6555), new TimeSpan(0, 1, 0, 0, 0)), "encounters.analysis.com/callback", "client1scope", new DateTimeOffset(new DateTime(2025, 1, 22, 10, 56, 33, 304, DateTimeKind.Unspecified).AddTicks(3452), new TimeSpan(0, 1, 0, 0, 0)) },
                    { "client2", "desktop", "public", new DateTimeOffset(new DateTime(2025, 1, 22, 10, 56, 33, 304, DateTimeKind.Unspecified).AddTicks(3652), new TimeSpan(0, 1, 0, 0, 0)), "localhost:45571/callback", "client2scope", new DateTimeOffset(new DateTime(2025, 1, 22, 10, 56, 33, 304, DateTimeKind.Unspecified).AddTicks(3657), new TimeSpan(0, 1, 0, 0, 0)) },
                    { "client3", "devWeb", "public", new DateTimeOffset(new DateTime(2025, 1, 22, 10, 56, 33, 304, DateTimeKind.Unspecified).AddTicks(3659), new TimeSpan(0, 1, 0, 0, 0)), "localhost:44479/callback", "client3scope", new DateTimeOffset(new DateTime(2025, 1, 22, 10, 56, 33, 304, DateTimeKind.Unspecified).AddTicks(3661), new TimeSpan(0, 1, 0, 0, 0)) }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AuthorizationCodeChallenge");

            migrationBuilder.DropTable(
                name: "Client");

            migrationBuilder.DropTable(
                name: "IdentityUser");

            migrationBuilder.DropTable(
                name: "RefreshToken");

            migrationBuilder.DropTable(
                name: "ResetToken");

            migrationBuilder.DropTable(
                name: "VerifyEmailToken");
        }
    }
}
