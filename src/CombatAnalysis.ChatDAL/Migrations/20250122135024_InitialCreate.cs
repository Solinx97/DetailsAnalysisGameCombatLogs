using CombatAnalysis.ChatDAL.Helpers;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CombatAnalysis.ChatDAL.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "GroupChat",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    AppUserId = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GroupChat", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "GroupChatMessage",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Username = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Message = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Time = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    Status = table.Column<int>(type: "int", nullable: false),
                    Type = table.Column<int>(type: "int", nullable: false),
                    ChatId = table.Column<int>(type: "int", nullable: false),
                    GroupChatUserId = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GroupChatMessage", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "GroupChatMessageCount",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Count = table.Column<int>(type: "int", nullable: false),
                    ChatId = table.Column<int>(type: "int", nullable: false),
                    GroupChatUserId = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GroupChatMessageCount", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "GroupChatRules",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    InvitePeople = table.Column<int>(type: "int", nullable: false),
                    RemovePeople = table.Column<int>(type: "int", nullable: false),
                    PinMessage = table.Column<int>(type: "int", nullable: false),
                    Announcements = table.Column<int>(type: "int", nullable: false),
                    ChatId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GroupChatRules", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "GroupChatUser",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Username = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ChatId = table.Column<int>(type: "int", nullable: false),
                    AppUserId = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GroupChatUser", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "PersonalChat",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    InitiatorId = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CompanionId = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PersonalChat", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "PersonalChatMessage",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Username = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Message = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Time = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    Status = table.Column<int>(type: "int", nullable: false),
                    Type = table.Column<int>(type: "int", nullable: false),
                    ChatId = table.Column<int>(type: "int", nullable: false),
                    AppUserId = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PersonalChatMessage", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "PersonalChatMessageCount",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Count = table.Column<int>(type: "int", nullable: false),
                    ChatId = table.Column<int>(type: "int", nullable: false),
                    AppUserId = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PersonalChatMessageCount", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "UnreadGroupChatMessage",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    GroupChatUserId = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    GroupChatMessageId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UnreadGroupChatMessage", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "VoiceChat",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    AppUserId = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_VoiceChat", x => x.Id);
                });

            MigrationHelper.CreateProcedures(migrationBuilder);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "GroupChat");

            migrationBuilder.DropTable(
                name: "GroupChatMessage");

            migrationBuilder.DropTable(
                name: "GroupChatMessageCount");

            migrationBuilder.DropTable(
                name: "GroupChatRules");

            migrationBuilder.DropTable(
                name: "GroupChatUser");

            migrationBuilder.DropTable(
                name: "PersonalChat");

            migrationBuilder.DropTable(
                name: "PersonalChatMessage");

            migrationBuilder.DropTable(
                name: "PersonalChatMessageCount");

            migrationBuilder.DropTable(
                name: "UnreadGroupChatMessage");

            migrationBuilder.DropTable(
                name: "VoiceChat");

            MigrationHelper.DropProcedures(migrationBuilder);
        }
    }
}
