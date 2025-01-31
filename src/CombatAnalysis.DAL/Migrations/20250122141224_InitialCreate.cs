using System;
using CombatAnalysis.DAL.Helpers;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CombatAnalysis.DAL.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Combat",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    LocallyNumber = table.Column<int>(type: "int", nullable: false),
                    DungeonName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Difficulty = table.Column<int>(type: "int", nullable: false),
                    DamageDone = table.Column<int>(type: "int", nullable: false),
                    HealDone = table.Column<int>(type: "int", nullable: false),
                    DamageTaken = table.Column<int>(type: "int", nullable: false),
                    EnergyRecovery = table.Column<int>(type: "int", nullable: false),
                    IsWin = table.Column<bool>(type: "bit", nullable: false),
                    StartDate = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    FinishDate = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    IsReady = table.Column<bool>(type: "bit", nullable: false),
                    CombatLogId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Combat", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "CombatAura",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Creator = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Target = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    AuraCreatorType = table.Column<int>(type: "int", nullable: false),
                    AuraType = table.Column<int>(type: "int", nullable: false),
                    StartTime = table.Column<TimeSpan>(type: "time", nullable: false),
                    FinishTime = table.Column<TimeSpan>(type: "time", nullable: false),
                    Stacks = table.Column<int>(type: "int", nullable: false),
                    CombatId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CombatAura", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "CombatLog",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Date = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    LogType = table.Column<int>(type: "int", nullable: false),
                    NumberReadyCombats = table.Column<int>(type: "int", nullable: false),
                    CombatsInQueue = table.Column<int>(type: "int", nullable: false),
                    IsReady = table.Column<bool>(type: "bit", nullable: false),
                    AppUserId = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CombatLog", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "CombatPlayer",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Username = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    PlayerId = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    AverageItemLevel = table.Column<double>(type: "float", nullable: false),
                    ResourcesRecovery = table.Column<int>(type: "int", nullable: false),
                    DamageDone = table.Column<int>(type: "int", nullable: false),
                    HealDone = table.Column<int>(type: "int", nullable: false),
                    DamageTaken = table.Column<int>(type: "int", nullable: false),
                    CombatId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CombatPlayer", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "CombatPlayerPosition",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PositionX = table.Column<double>(type: "float", nullable: false),
                    PositionY = table.Column<double>(type: "float", nullable: false),
                    Time = table.Column<TimeSpan>(type: "time", nullable: false),
                    CombatPlayerId = table.Column<int>(type: "int", nullable: false),
                    CombatId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CombatPlayerPosition", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "DamageDone",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Spell = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Value = table.Column<int>(type: "int", nullable: false),
                    Time = table.Column<TimeSpan>(type: "time", nullable: false),
                    Creator = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Target = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    DamageType = table.Column<int>(type: "int", nullable: false),
                    IsPeriodicDamage = table.Column<bool>(type: "bit", nullable: false),
                    IsPet = table.Column<bool>(type: "bit", nullable: false),
                    CombatPlayerId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DamageDone", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "DamageDoneGeneral",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Value = table.Column<int>(type: "int", nullable: false),
                    DamagePerSecond = table.Column<double>(type: "float", nullable: false),
                    Spell = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CritNumber = table.Column<int>(type: "int", nullable: false),
                    MissNumber = table.Column<int>(type: "int", nullable: false),
                    CastNumber = table.Column<int>(type: "int", nullable: false),
                    MinValue = table.Column<int>(type: "int", nullable: false),
                    MaxValue = table.Column<int>(type: "int", nullable: false),
                    AverageValue = table.Column<double>(type: "float", nullable: false),
                    IsPet = table.Column<bool>(type: "bit", nullable: false),
                    CombatPlayerId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DamageDoneGeneral", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "DamageTaken",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Spell = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Value = table.Column<int>(type: "int", nullable: false),
                    Time = table.Column<TimeSpan>(type: "time", nullable: false),
                    Creator = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Target = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    DamageTakenType = table.Column<int>(type: "int", nullable: false),
                    ActualValue = table.Column<int>(type: "int", nullable: false),
                    IsPeriodicDamage = table.Column<bool>(type: "bit", nullable: false),
                    Resisted = table.Column<int>(type: "int", nullable: false),
                    Absorbed = table.Column<int>(type: "int", nullable: false),
                    Blocked = table.Column<int>(type: "int", nullable: false),
                    RealDamage = table.Column<int>(type: "int", nullable: false),
                    Mitigated = table.Column<int>(type: "int", nullable: false),
                    CombatPlayerId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DamageTaken", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "DamageTakenGeneral",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Spell = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Value = table.Column<int>(type: "int", nullable: false),
                    ActualValue = table.Column<int>(type: "int", nullable: false),
                    DamageTakenPerSecond = table.Column<double>(type: "float", nullable: false),
                    CritNumber = table.Column<int>(type: "int", nullable: false),
                    MissNumber = table.Column<int>(type: "int", nullable: false),
                    CastNumber = table.Column<int>(type: "int", nullable: false),
                    MinValue = table.Column<int>(type: "int", nullable: false),
                    MaxValue = table.Column<int>(type: "int", nullable: false),
                    AverageValue = table.Column<double>(type: "float", nullable: false),
                    CombatPlayerId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DamageTakenGeneral", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "HealDone",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Spell = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Value = table.Column<int>(type: "int", nullable: false),
                    Time = table.Column<TimeSpan>(type: "time", nullable: false),
                    Creator = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Target = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Overheal = table.Column<int>(type: "int", nullable: false),
                    IsCrit = table.Column<bool>(type: "bit", nullable: false),
                    IsAbsorbed = table.Column<bool>(type: "bit", nullable: false),
                    CombatPlayerId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_HealDone", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "HealDoneGeneral",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Spell = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Value = table.Column<int>(type: "int", nullable: false),
                    HealPerSecond = table.Column<double>(type: "float", nullable: false),
                    CritNumber = table.Column<int>(type: "int", nullable: false),
                    CastNumber = table.Column<int>(type: "int", nullable: false),
                    MinValue = table.Column<int>(type: "int", nullable: false),
                    MaxValue = table.Column<int>(type: "int", nullable: false),
                    AverageValue = table.Column<double>(type: "float", nullable: false),
                    CombatPlayerId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_HealDoneGeneral", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "PlayerDeath",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Username = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    LastHitSpellOrItem = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    LastHitValue = table.Column<int>(type: "int", nullable: false),
                    Time = table.Column<TimeSpan>(type: "time", nullable: false),
                    CombatPlayerId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PlayerDeath", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "PlayerParseInfo",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    SpecId = table.Column<int>(type: "int", nullable: false),
                    ClassId = table.Column<int>(type: "int", nullable: false),
                    BossId = table.Column<int>(type: "int", nullable: false),
                    Difficult = table.Column<int>(type: "int", nullable: false),
                    DamageEfficiency = table.Column<int>(type: "int", nullable: false),
                    HealEfficiency = table.Column<int>(type: "int", nullable: false),
                    CombatPlayerId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PlayerParseInfo", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ResourceRecovery",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Spell = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Value = table.Column<int>(type: "int", nullable: false),
                    Time = table.Column<TimeSpan>(type: "time", nullable: false),
                    Creator = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Target = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CombatPlayerId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ResourceRecovery", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ResourceRecoveryGeneral",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Spell = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Value = table.Column<int>(type: "int", nullable: false),
                    ResourcePerSecond = table.Column<double>(type: "float", nullable: false),
                    CastNumber = table.Column<int>(type: "int", nullable: false),
                    MinValue = table.Column<int>(type: "int", nullable: false),
                    MaxValue = table.Column<int>(type: "int", nullable: false),
                    AverageValue = table.Column<double>(type: "float", nullable: false),
                    CombatPlayerId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ResourceRecoveryGeneral", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "SpecializationScore",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    SpecId = table.Column<int>(type: "int", nullable: false),
                    BossId = table.Column<int>(type: "int", nullable: false),
                    Difficult = table.Column<int>(type: "int", nullable: false),
                    Damage = table.Column<int>(type: "int", nullable: false),
                    Heal = table.Column<int>(type: "int", nullable: false),
                    Updated = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SpecializationScore", x => x.Id);
                });

            MigrationHelper.CreateProcedures(migrationBuilder);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Combat");

            migrationBuilder.DropTable(
                name: "CombatAura");

            migrationBuilder.DropTable(
                name: "CombatLog");

            migrationBuilder.DropTable(
                name: "CombatPlayer");

            migrationBuilder.DropTable(
                name: "CombatPlayerPosition");

            migrationBuilder.DropTable(
                name: "DamageDone");

            migrationBuilder.DropTable(
                name: "DamageDoneGeneral");

            migrationBuilder.DropTable(
                name: "DamageTaken");

            migrationBuilder.DropTable(
                name: "DamageTakenGeneral");

            migrationBuilder.DropTable(
                name: "HealDone");

            migrationBuilder.DropTable(
                name: "HealDoneGeneral");

            migrationBuilder.DropTable(
                name: "PlayerDeath");

            migrationBuilder.DropTable(
                name: "PlayerParseInfo");

            migrationBuilder.DropTable(
                name: "ResourceRecovery");

            migrationBuilder.DropTable(
                name: "ResourceRecoveryGeneral");

            migrationBuilder.DropTable(
                name: "SpecializationScore");

            MigrationHelper.DropProcedures(migrationBuilder);
        }
    }
}
