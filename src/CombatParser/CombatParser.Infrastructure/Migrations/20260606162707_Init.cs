using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace CombatParser.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class Init : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Boss",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    GameId = table.Column<int>(type: "int", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(128)", maxLength: 128, nullable: false),
                    Health = table.Column<long>(type: "bigint", nullable: false),
                    Difficult = table.Column<int>(type: "int", nullable: false),
                    Size = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Boss", x => x.Id);
                });

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

            migrationBuilder.CreateTable(
                name: "CombatLog",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(128)", maxLength: 128, nullable: false),
                    Date = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    LogType = table.Column<int>(type: "int", nullable: false),
                    AppUserId = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CombatLog", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Player",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    GameId = table.Column<string>(type: "nvarchar(128)", maxLength: 128, nullable: false),
                    Username = table.Column<string>(type: "nvarchar(128)", maxLength: 128, nullable: false),
                    Faction = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Player", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Specialization",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(128)", maxLength: 128, nullable: false),
                    SpecializationSpellsId = table.Column<string>(type: "nvarchar(128)", maxLength: 128, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Specialization", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Combat",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    DungeonName = table.Column<string>(type: "nvarchar(128)", maxLength: 128, nullable: false),
                    BossHealthPercentage = table.Column<double>(type: "float", nullable: false),
                    DamageDone = table.Column<long>(type: "bigint", nullable: false),
                    HealDone = table.Column<long>(type: "bigint", nullable: false),
                    DamageTaken = table.Column<long>(type: "bigint", nullable: false),
                    ResourcesRecovery = table.Column<long>(type: "bigint", nullable: false),
                    IsWin = table.Column<bool>(type: "bit", nullable: false),
                    StartDate = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    FinishDate = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    BossId = table.Column<int>(type: "int", nullable: false),
                    CombatLogId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Combat", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Combat_Boss_BossId",
                        column: x => x.BossId,
                        principalTable: "Boss",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Combat_CombatLog_CombatLogId",
                        column: x => x.CombatLogId,
                        principalTable: "CombatLog",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "BestSpecializationScore",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    DamageDone = table.Column<int>(type: "int", nullable: false),
                    HealDone = table.Column<int>(type: "int", nullable: false),
                    Updated = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true),
                    SpecializationId = table.Column<int>(type: "int", nullable: false),
                    BossId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BestSpecializationScore", x => x.Id);
                    table.ForeignKey(
                        name: "FK_BestSpecializationScore_Boss_BossId",
                        column: x => x.BossId,
                        principalTable: "Boss",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_BestSpecializationScore_Specialization_SpecializationId",
                        column: x => x.SpecializationId,
                        principalTable: "Specialization",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "CombatPlayer",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    AverageItemLevel = table.Column<double>(type: "float", nullable: false),
                    ResourcesRecovery = table.Column<int>(type: "int", nullable: false),
                    DamageDone = table.Column<int>(type: "int", nullable: false),
                    HealDone = table.Column<int>(type: "int", nullable: false),
                    DamageTaken = table.Column<int>(type: "int", nullable: false),
                    PlayerId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    CombatId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CombatPlayer", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CombatPlayer_Combat_CombatId",
                        column: x => x.CombatId,
                        principalTable: "Combat",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CombatPlayer_Player_PlayerId",
                        column: x => x.PlayerId,
                        principalTable: "Player",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "CombatTarget",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Username = table.Column<string>(type: "nvarchar(128)", maxLength: 128, nullable: false),
                    Target = table.Column<string>(type: "nvarchar(128)", maxLength: 128, nullable: false),
                    Sum = table.Column<int>(type: "int", nullable: false),
                    CombatId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CombatTarget", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CombatTarget_Combat_CombatId",
                        column: x => x.CombatId,
                        principalTable: "Combat",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "CombatPlayerAura",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    GameAuraId = table.Column<int>(type: "int", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(128)", maxLength: 128, nullable: false),
                    Creator = table.Column<string>(type: "nvarchar(128)", maxLength: 128, nullable: false),
                    Target = table.Column<string>(type: "nvarchar(128)", maxLength: 128, nullable: false),
                    AuraCreatorType = table.Column<int>(type: "int", nullable: false),
                    AuraType = table.Column<int>(type: "int", nullable: false),
                    StartTime = table.Column<TimeSpan>(type: "time", nullable: false),
                    FinishTime = table.Column<TimeSpan>(type: "time", nullable: false),
                    Stacks = table.Column<int>(type: "int", nullable: false),
                    CombatPlayerId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CombatPlayerAura", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CombatPlayerAura_CombatPlayer_CombatPlayerId",
                        column: x => x.CombatPlayerId,
                        principalTable: "CombatPlayer",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "CombatPlayerDeath",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Username = table.Column<string>(type: "nvarchar(128)", maxLength: 128, nullable: false),
                    LastHitSpell = table.Column<string>(type: "nvarchar(128)", maxLength: 128, nullable: false),
                    LastHitValue = table.Column<int>(type: "int", nullable: false),
                    Time = table.Column<TimeSpan>(type: "time", nullable: false),
                    CombatPlayerId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CombatPlayerDeath", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CombatPlayerDeath_CombatPlayer_CombatPlayerId",
                        column: x => x.CombatPlayerId,
                        principalTable: "CombatPlayer",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
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
                    CombatPlayerId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CombatPlayerPosition", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CombatPlayerPosition_CombatPlayer_CombatPlayerId",
                        column: x => x.CombatPlayerId,
                        principalTable: "CombatPlayer",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "CombatPlayerPreAura",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CreatorGameId = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    GameId = table.Column<int>(type: "int", nullable: false),
                    Status = table.Column<int>(type: "int", nullable: false),
                    CombatPlayerId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CombatPlayerPreAura", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CombatPlayerPreAura_CombatPlayer_CombatPlayerId",
                        column: x => x.CombatPlayerId,
                        principalTable: "CombatPlayer",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "CombatPlayerStats",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Strength = table.Column<int>(type: "int", nullable: false),
                    Agility = table.Column<int>(type: "int", nullable: false),
                    Intelligence = table.Column<int>(type: "int", nullable: false),
                    Stamina = table.Column<int>(type: "int", nullable: false),
                    Spirit = table.Column<int>(type: "int", nullable: false),
                    Dodge = table.Column<int>(type: "int", nullable: false),
                    Parry = table.Column<int>(type: "int", nullable: false),
                    Crit = table.Column<int>(type: "int", nullable: false),
                    Haste = table.Column<int>(type: "int", nullable: false),
                    Hit = table.Column<int>(type: "int", nullable: false),
                    Expertise = table.Column<int>(type: "int", nullable: false),
                    Armor = table.Column<int>(type: "int", nullable: false),
                    Talents = table.Column<string>(type: "nvarchar(128)", maxLength: 128, nullable: false),
                    CombatPlayerId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CombatPlayerStats", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CombatPlayerStats_CombatPlayer_CombatPlayerId",
                        column: x => x.CombatPlayerId,
                        principalTable: "CombatPlayer",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "DamageDone",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    GameSpellId = table.Column<int>(type: "int", nullable: false),
                    Spell = table.Column<string>(type: "nvarchar(128)", maxLength: 128, nullable: false),
                    Value = table.Column<int>(type: "int", nullable: false),
                    Time = table.Column<TimeSpan>(type: "time", nullable: false),
                    Creator = table.Column<string>(type: "nvarchar(128)", maxLength: 128, nullable: false),
                    Target = table.Column<string>(type: "nvarchar(128)", maxLength: 128, nullable: false),
                    IsTargetBoss = table.Column<bool>(type: "bit", nullable: false),
                    DamageType = table.Column<int>(type: "int", nullable: false),
                    IsPeriodicDamage = table.Column<bool>(type: "bit", nullable: false),
                    IsSingleTarget = table.Column<bool>(type: "bit", nullable: false),
                    IsPet = table.Column<bool>(type: "bit", nullable: false),
                    CombatPlayerId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DamageDone", x => x.Id);
                    table.ForeignKey(
                        name: "FK_DamageDone_CombatPlayer_CombatPlayerId",
                        column: x => x.CombatPlayerId,
                        principalTable: "CombatPlayer",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "DamageDoneGeneral",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    GameSpellId = table.Column<int>(type: "int", nullable: false),
                    Spell = table.Column<string>(type: "nvarchar(128)", maxLength: 128, nullable: false),
                    Value = table.Column<int>(type: "int", nullable: false),
                    DamagePerSecond = table.Column<double>(type: "float", nullable: false),
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
                    table.ForeignKey(
                        name: "FK_DamageDoneGeneral_CombatPlayer_CombatPlayerId",
                        column: x => x.CombatPlayerId,
                        principalTable: "CombatPlayer",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "DamageTaken",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    GameSpellId = table.Column<int>(type: "int", nullable: false),
                    Spell = table.Column<string>(type: "nvarchar(128)", maxLength: 128, nullable: false),
                    Value = table.Column<int>(type: "int", nullable: false),
                    Time = table.Column<TimeSpan>(type: "time", nullable: false),
                    Creator = table.Column<string>(type: "nvarchar(128)", maxLength: 128, nullable: false),
                    Target = table.Column<string>(type: "nvarchar(128)", maxLength: 128, nullable: false),
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
                    table.ForeignKey(
                        name: "FK_DamageTaken_CombatPlayer_CombatPlayerId",
                        column: x => x.CombatPlayerId,
                        principalTable: "CombatPlayer",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "DamageTakenGeneral",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    GameSpellId = table.Column<int>(type: "int", nullable: false),
                    Spell = table.Column<string>(type: "nvarchar(128)", maxLength: 128, nullable: false),
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
                    table.ForeignKey(
                        name: "FK_DamageTakenGeneral_CombatPlayer_CombatPlayerId",
                        column: x => x.CombatPlayerId,
                        principalTable: "CombatPlayer",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "HealDone",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    GameSpellId = table.Column<int>(type: "int", nullable: false),
                    Spell = table.Column<string>(type: "nvarchar(128)", maxLength: 128, nullable: false),
                    Value = table.Column<int>(type: "int", nullable: false),
                    Time = table.Column<TimeSpan>(type: "time", nullable: false),
                    Creator = table.Column<string>(type: "nvarchar(128)", maxLength: 128, nullable: false),
                    Target = table.Column<string>(type: "nvarchar(128)", maxLength: 128, nullable: false),
                    Overheal = table.Column<int>(type: "int", nullable: false),
                    IsCrit = table.Column<bool>(type: "bit", nullable: false),
                    IsAbsorbed = table.Column<bool>(type: "bit", nullable: false),
                    CombatPlayerId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_HealDone", x => x.Id);
                    table.ForeignKey(
                        name: "FK_HealDone_CombatPlayer_CombatPlayerId",
                        column: x => x.CombatPlayerId,
                        principalTable: "CombatPlayer",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "HealDoneGeneral",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    GameSpellId = table.Column<int>(type: "int", nullable: false),
                    Spell = table.Column<string>(type: "nvarchar(128)", maxLength: 128, nullable: false),
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
                    table.ForeignKey(
                        name: "FK_HealDoneGeneral_CombatPlayer_CombatPlayerId",
                        column: x => x.CombatPlayerId,
                        principalTable: "CombatPlayer",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ResourceRecovery",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    GameSpellId = table.Column<int>(type: "int", nullable: false),
                    Spell = table.Column<string>(type: "nvarchar(128)", maxLength: 128, nullable: false),
                    Value = table.Column<int>(type: "int", nullable: false),
                    Time = table.Column<TimeSpan>(type: "time", nullable: false),
                    Creator = table.Column<string>(type: "nvarchar(128)", maxLength: 128, nullable: false),
                    Target = table.Column<string>(type: "nvarchar(128)", maxLength: 128, nullable: false),
                    CombatPlayerId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ResourceRecovery", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ResourceRecovery_CombatPlayer_CombatPlayerId",
                        column: x => x.CombatPlayerId,
                        principalTable: "CombatPlayer",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ResourceRecoveryGeneral",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    GameSpellId = table.Column<int>(type: "int", nullable: false),
                    Spell = table.Column<string>(type: "nvarchar(128)", maxLength: 128, nullable: false),
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
                    table.ForeignKey(
                        name: "FK_ResourceRecoveryGeneral_CombatPlayer_CombatPlayerId",
                        column: x => x.CombatPlayerId,
                        principalTable: "CombatPlayer",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "SpecializationScore",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    DamageScore = table.Column<double>(type: "float", nullable: false),
                    DamageDone = table.Column<int>(type: "int", nullable: false),
                    HealScore = table.Column<double>(type: "float", nullable: false),
                    HealDone = table.Column<int>(type: "int", nullable: false),
                    Updated = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true),
                    SpecializationId = table.Column<int>(type: "int", nullable: false),
                    CombatPlayerId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SpecializationScore", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SpecializationScore_CombatPlayer_CombatPlayerId",
                        column: x => x.CombatPlayerId,
                        principalTable: "CombatPlayer",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_SpecializationScore_Specialization_SpecializationId",
                        column: x => x.SpecializationId,
                        principalTable: "Specialization",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.InsertData(
                table: "Boss",
                columns: new[] { "Id", "Difficult", "GameId", "Health", "Name", "Size" },
                values: new object[,]
                {
                    { 1, 3, 1395, 130841100L, "Каменные стражи", 10 },
                    { 2, 5, 1395, 235513980L, "Каменные стражи", 10 },
                    { 3, 3, 1390, 152647950L, "Фэн Проклятый", 10 },
                    { 4, 5, 1390, 209345760L, "Фэн Проклятый", 10 },
                    { 5, 3, 1434, 117756990L, "Душелов Гара'джал", 10 },
                    { 6, 5, 1434, 179252307L, "Душелов Гара'джал", 10 },
                    { 7, 3, 1436, 174454800L, "Призрачные короли", 10 },
                    { 8, 5, 1436, 261682200L, "Призрачные короли", 10 },
                    { 9, 3, 1500, 294392475L, "Элегон", 10 },
                    { 10, 5, 1500, 339750723L, "Элегон", 10 },
                    { 11, 3, 1407, 314018640L, "Воля императора", 10 },
                    { 12, 5, 1407, 471027960L, "Воля императора", 10 },
                    { 13, 3, 1409, 213968815L, "Вечные защитники", 10 },
                    { 14, 5, 1409, 344082093L, "Вечные защитники", 10 },
                    { 15, 3, 1505, 174454800L, "Цулон", 10 },
                    { 16, 5, 1505, 279127680L, "Цулон", 10 },
                    { 17, 3, 1506, 138168195L, "Лэй Ши", 10 },
                    { 18, 5, 1506, 301457900L, "Лэй Ши", 10 },
                    { 19, 3, 1431, 184704020L, "Ша Страха", 10 },
                    { 20, 5, 1431, 544037304L, "Ша Страха", 10 },
                    { 21, 3, 1507, 174454800L, "Императорский визирь Зор'лок", 10 },
                    { 22, 5, 1507, 218068500L, "Императорский визирь Зор'лок", 10 },
                    { 23, 3, 1504, 150467265L, "Повелитель клинков Та'як", 10 },
                    { 24, 5, 1504, 196261650L, "Повелитель клинков Та'як", 10 },
                    { 25, 3, 1463, 218068500L, "Гаралон", 10 },
                    { 26, 5, 1463, 290759446L, "Гаралон", 10 },
                    { 27, 3, 1498, 270404940L, "Повелитель ветров Мел'джарак", 10 },
                    { 28, 5, 1498, 588784950L, "Повелитель ветров Мел'джарак", 10 },
                    { 29, 3, 1499, 218068500L, "Ваятель янтаря Ун'сок", 10 },
                    { 30, 5, 1499, 340186860L, "Ваятель янтаря Ун'сок", 10 },
                    { 31, 3, 1501, 196261650L, "Великая императрица Шек'зир", 10 },
                    { 32, 5, 1501, 307476585L, "Великая императрица Шек'зир", 10 },
                    { 33, 3, 1577, 207601212L, "Джин'рок Разрушитель", 10 },
                    { 34, 5, 1577, 317507736L, "Джин'рок Разрушитель", 10 },
                    { 35, 3, 1575, 357632340L, "Хорридон", 10 },
                    { 36, 5, 1575, 654205500L, "Хорридон", 10 },
                    { 37, 3, 1570, 299538888L, "Совет старейшин", 10 },
                    { 38, 5, 1570, 470330152L, "Совет старейшин", 10 },
                    { 39, 3, 1565, 179999841L, "Тортос", 10 },
                    { 40, 5, 1565, 319999818L, "Тортос", 10 },
                    { 41, 3, 1578, 263317712L, "Мегера", 10 },
                    { 42, 5, 1578, 342297774L, "Мегера", 10 },
                    { 43, 3, 1573, 244236720L, "Цзи-Кунь", 10 },
                    { 44, 5, 1573, 366355080L, "Цзи-Кунь", 10 },
                    { 45, 3, 1572, 261682200L, "Дуруму Позабытый", 10 },
                    { 46, 5, 1572, 392523300L, "Дуруму Позабытый", 10 },
                    { 47, 3, 1574, 218068500L, "Изначалий", 10 },
                    { 48, 5, 1574, 258193104L, "Изначалий", 10 },
                    { 49, 3, 1576, 80999797L, "Темный Анимус", 10 },
                    { 50, 5, 1576, 288000023L, "Темный Анимус", 10 },
                    { 51, 3, 1559, 119937675L, "Кон Железный", 10 },
                    { 52, 5, 1559, 155700909L, "Кон Железный", 10 },
                    { 53, 3, 1560, 219812670L, "Небесные сестры", 10 },
                    { 54, 5, 1560, 628036200L, "Небесные сестры", 10 },
                    { 55, 3, 1579, 329283435L, "Лэй Шэнь", 10 },
                    { 56, 5, 1579, 580498347L, "Лэй Шэнь", 10 }
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
                    { 12, 7, 120676, "Тотем порыва бури" },
                    { 13, 9, 104272, "Сытость" },
                    { 14, 7, 61316, "Чародейская гениальность Даларана" },
                    { 15, 7, 1126, "Знак дикой природы" },
                    { 16, 7, 109773, "Узы Тьмы" },
                    { 17, 7, 116956, "Легкость воздуха" },
                    { 18, 7, 77747, "Пылающая ярость" },
                    { 19, 7, 113742, "Искусство быстрой битвы" },
                    { 20, 7, 19740, "Благословение могущества" },
                    { 21, 7, 135678, "Бодрящие споры" },
                    { 22, 7, 20217, "Благословение королей" },
                    { 23, 10, 25780, "Праведное неистовство" }
                });

            migrationBuilder.InsertData(
                table: "Specialization",
                columns: new[] { "Id", "Name", "SpecializationSpellsId" },
                values: new object[,]
                {
                    { 1, "Affliction", "48181,30108,1120" },
                    { 2, "Survival", "131900,3674,53301" },
                    { 3, "Unholy", "55078,55090,47632" },
                    { 4, "Balance", "50288,78674,8921" },
                    { 5, "Shadow", "129197,2944,15407" },
                    { 6, "Arms", "12294,86346,7384" },
                    { 7, "ProtectionWarrior", "6572,23922,20243" },
                    { 8, "Brewmaster", "121253,124335,100787" },
                    { 9, "Discipline", "47750,81751,585" },
                    { 10, "Restoration", "61295,52752,51945" },
                    { 11, "Combat", "57841,84617,1752" },
                    { 12, "Subtlety", "53,2098,8676" },
                    { 13, "Destruction", "29722,116858,348" },
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
                    { 1, 1, 0, 0, 1, null },
                    { 2, 1, 0, 0, 2, null },
                    { 3, 1, 0, 0, 3, null },
                    { 4, 1, 0, 0, 4, null },
                    { 5, 1, 0, 0, 5, null },
                    { 6, 1, 0, 0, 6, null },
                    { 7, 1, 0, 0, 7, null },
                    { 8, 1, 0, 0, 8, null },
                    { 9, 1, 0, 0, 9, null },
                    { 10, 1, 0, 0, 10, null },
                    { 11, 1, 0, 0, 11, null },
                    { 12, 1, 0, 0, 12, null },
                    { 13, 1, 0, 0, 13, null },
                    { 14, 1, 0, 0, 14, null },
                    { 15, 1, 0, 0, 15, null },
                    { 16, 1, 0, 0, 16, null },
                    { 17, 1, 0, 0, 17, null },
                    { 18, 2, 0, 0, 1, null },
                    { 19, 2, 0, 0, 2, null },
                    { 20, 2, 0, 0, 3, null },
                    { 21, 2, 0, 0, 4, null },
                    { 22, 2, 0, 0, 5, null },
                    { 23, 2, 0, 0, 6, null },
                    { 24, 2, 0, 0, 7, null },
                    { 25, 2, 0, 0, 8, null },
                    { 26, 2, 0, 0, 9, null },
                    { 27, 2, 0, 0, 10, null },
                    { 28, 2, 0, 0, 11, null },
                    { 29, 2, 0, 0, 12, null },
                    { 30, 2, 0, 0, 13, null },
                    { 31, 2, 0, 0, 14, null },
                    { 32, 2, 0, 0, 15, null },
                    { 33, 2, 0, 0, 16, null },
                    { 34, 2, 0, 0, 17, null },
                    { 35, 3, 0, 0, 1, null },
                    { 36, 3, 0, 0, 2, null },
                    { 37, 3, 0, 0, 3, null },
                    { 38, 3, 0, 0, 4, null },
                    { 39, 3, 0, 0, 5, null },
                    { 40, 3, 0, 0, 6, null },
                    { 41, 3, 0, 0, 7, null },
                    { 42, 3, 0, 0, 8, null },
                    { 43, 3, 0, 0, 9, null },
                    { 44, 3, 0, 0, 10, null },
                    { 45, 3, 0, 0, 11, null },
                    { 46, 3, 0, 0, 12, null },
                    { 47, 3, 0, 0, 13, null },
                    { 48, 3, 0, 0, 14, null },
                    { 49, 3, 0, 0, 15, null },
                    { 50, 3, 0, 0, 16, null },
                    { 51, 3, 0, 0, 17, null },
                    { 52, 4, 0, 0, 1, null },
                    { 53, 4, 0, 0, 2, null },
                    { 54, 4, 0, 0, 3, null },
                    { 55, 4, 0, 0, 4, null },
                    { 56, 4, 0, 0, 5, null },
                    { 57, 4, 0, 0, 6, null },
                    { 58, 4, 0, 0, 7, null },
                    { 59, 4, 0, 0, 8, null },
                    { 60, 4, 0, 0, 9, null },
                    { 61, 4, 0, 0, 10, null },
                    { 62, 4, 0, 0, 11, null },
                    { 63, 4, 0, 0, 12, null },
                    { 64, 4, 0, 0, 13, null },
                    { 65, 4, 0, 0, 14, null },
                    { 66, 4, 0, 0, 15, null },
                    { 67, 4, 0, 0, 16, null },
                    { 68, 4, 0, 0, 17, null },
                    { 69, 5, 0, 0, 1, null },
                    { 70, 5, 0, 0, 2, null },
                    { 71, 5, 0, 0, 3, null },
                    { 72, 5, 0, 0, 4, null },
                    { 73, 5, 0, 0, 5, null },
                    { 74, 5, 0, 0, 6, null },
                    { 75, 5, 0, 0, 7, null },
                    { 76, 5, 0, 0, 8, null },
                    { 77, 5, 0, 0, 9, null },
                    { 78, 5, 0, 0, 10, null },
                    { 79, 5, 0, 0, 11, null },
                    { 80, 5, 0, 0, 12, null },
                    { 81, 5, 0, 0, 13, null },
                    { 82, 5, 0, 0, 14, null },
                    { 83, 5, 0, 0, 15, null },
                    { 84, 5, 0, 0, 16, null },
                    { 85, 5, 0, 0, 17, null },
                    { 86, 6, 0, 0, 1, null },
                    { 87, 6, 0, 0, 2, null },
                    { 88, 6, 0, 0, 3, null },
                    { 89, 6, 0, 0, 4, null },
                    { 90, 6, 0, 0, 5, null },
                    { 91, 6, 0, 0, 6, null },
                    { 92, 6, 0, 0, 7, null },
                    { 93, 6, 0, 0, 8, null },
                    { 94, 6, 0, 0, 9, null },
                    { 95, 6, 0, 0, 10, null },
                    { 96, 6, 0, 0, 11, null },
                    { 97, 6, 0, 0, 12, null },
                    { 98, 6, 0, 0, 13, null },
                    { 99, 6, 0, 0, 14, null },
                    { 100, 6, 0, 0, 15, null },
                    { 101, 6, 0, 0, 16, null },
                    { 102, 6, 0, 0, 17, null },
                    { 103, 7, 0, 0, 1, null },
                    { 104, 7, 0, 0, 2, null },
                    { 105, 7, 0, 0, 3, null },
                    { 106, 7, 0, 0, 4, null },
                    { 107, 7, 0, 0, 5, null },
                    { 108, 7, 0, 0, 6, null },
                    { 109, 7, 0, 0, 7, null },
                    { 110, 7, 0, 0, 8, null },
                    { 111, 7, 0, 0, 9, null },
                    { 112, 7, 0, 0, 10, null },
                    { 113, 7, 0, 0, 11, null },
                    { 114, 7, 0, 0, 12, null },
                    { 115, 7, 0, 0, 13, null },
                    { 116, 7, 0, 0, 14, null },
                    { 117, 7, 0, 0, 15, null },
                    { 118, 7, 0, 0, 16, null },
                    { 119, 7, 0, 0, 17, null },
                    { 120, 8, 0, 0, 1, null },
                    { 121, 8, 0, 0, 2, null },
                    { 122, 8, 0, 0, 3, null },
                    { 123, 8, 0, 0, 4, null },
                    { 124, 8, 0, 0, 5, null },
                    { 125, 8, 0, 0, 6, null },
                    { 126, 8, 0, 0, 7, null },
                    { 127, 8, 0, 0, 8, null },
                    { 128, 8, 0, 0, 9, null },
                    { 129, 8, 0, 0, 10, null },
                    { 130, 8, 0, 0, 11, null },
                    { 131, 8, 0, 0, 12, null },
                    { 132, 8, 0, 0, 13, null },
                    { 133, 8, 0, 0, 14, null },
                    { 134, 8, 0, 0, 15, null },
                    { 135, 8, 0, 0, 16, null },
                    { 136, 8, 0, 0, 17, null },
                    { 137, 9, 0, 0, 1, null },
                    { 138, 9, 0, 0, 2, null },
                    { 139, 9, 0, 0, 3, null },
                    { 140, 9, 0, 0, 4, null },
                    { 141, 9, 0, 0, 5, null },
                    { 142, 9, 0, 0, 6, null },
                    { 143, 9, 0, 0, 7, null },
                    { 144, 9, 0, 0, 8, null },
                    { 145, 9, 0, 0, 9, null },
                    { 146, 9, 0, 0, 10, null },
                    { 147, 9, 0, 0, 11, null },
                    { 148, 9, 0, 0, 12, null },
                    { 149, 9, 0, 0, 13, null },
                    { 150, 9, 0, 0, 14, null },
                    { 151, 9, 0, 0, 15, null },
                    { 152, 9, 0, 0, 16, null },
                    { 153, 9, 0, 0, 17, null },
                    { 154, 10, 0, 0, 1, null },
                    { 155, 10, 0, 0, 2, null },
                    { 156, 10, 0, 0, 3, null },
                    { 157, 10, 0, 0, 4, null },
                    { 158, 10, 0, 0, 5, null },
                    { 159, 10, 0, 0, 6, null },
                    { 160, 10, 0, 0, 7, null },
                    { 161, 10, 0, 0, 8, null },
                    { 162, 10, 0, 0, 9, null },
                    { 163, 10, 0, 0, 10, null },
                    { 164, 10, 0, 0, 11, null },
                    { 165, 10, 0, 0, 12, null },
                    { 166, 10, 0, 0, 13, null },
                    { 167, 10, 0, 0, 14, null },
                    { 168, 10, 0, 0, 15, null },
                    { 169, 10, 0, 0, 16, null },
                    { 170, 10, 0, 0, 17, null },
                    { 171, 11, 0, 0, 1, null },
                    { 172, 11, 0, 0, 2, null },
                    { 173, 11, 0, 0, 3, null },
                    { 174, 11, 0, 0, 4, null },
                    { 175, 11, 0, 0, 5, null },
                    { 176, 11, 0, 0, 6, null },
                    { 177, 11, 0, 0, 7, null },
                    { 178, 11, 0, 0, 8, null },
                    { 179, 11, 0, 0, 9, null },
                    { 180, 11, 0, 0, 10, null },
                    { 181, 11, 0, 0, 11, null },
                    { 182, 11, 0, 0, 12, null },
                    { 183, 11, 0, 0, 13, null },
                    { 184, 11, 0, 0, 14, null },
                    { 185, 11, 0, 0, 15, null },
                    { 186, 11, 0, 0, 16, null },
                    { 187, 11, 0, 0, 17, null },
                    { 188, 12, 0, 0, 1, null },
                    { 189, 12, 0, 0, 2, null },
                    { 190, 12, 0, 0, 3, null },
                    { 191, 12, 0, 0, 4, null },
                    { 192, 12, 0, 0, 5, null },
                    { 193, 12, 0, 0, 6, null },
                    { 194, 12, 0, 0, 7, null },
                    { 195, 12, 0, 0, 8, null },
                    { 196, 12, 0, 0, 9, null },
                    { 197, 12, 0, 0, 10, null },
                    { 198, 12, 0, 0, 11, null },
                    { 199, 12, 0, 0, 12, null },
                    { 200, 12, 0, 0, 13, null },
                    { 201, 12, 0, 0, 14, null },
                    { 202, 12, 0, 0, 15, null },
                    { 203, 12, 0, 0, 16, null },
                    { 204, 12, 0, 0, 17, null },
                    { 205, 13, 0, 0, 1, null },
                    { 206, 13, 0, 0, 2, null },
                    { 207, 13, 0, 0, 3, null },
                    { 208, 13, 0, 0, 4, null },
                    { 209, 13, 0, 0, 5, null },
                    { 210, 13, 0, 0, 6, null },
                    { 211, 13, 0, 0, 7, null },
                    { 212, 13, 0, 0, 8, null },
                    { 213, 13, 0, 0, 9, null },
                    { 214, 13, 0, 0, 10, null },
                    { 215, 13, 0, 0, 11, null },
                    { 216, 13, 0, 0, 12, null },
                    { 217, 13, 0, 0, 13, null },
                    { 218, 13, 0, 0, 14, null },
                    { 219, 13, 0, 0, 15, null },
                    { 220, 13, 0, 0, 16, null },
                    { 221, 13, 0, 0, 17, null },
                    { 222, 14, 0, 0, 1, null },
                    { 223, 14, 0, 0, 2, null },
                    { 224, 14, 0, 0, 3, null },
                    { 225, 14, 0, 0, 4, null },
                    { 226, 14, 0, 0, 5, null },
                    { 227, 14, 0, 0, 6, null },
                    { 228, 14, 0, 0, 7, null },
                    { 229, 14, 0, 0, 8, null },
                    { 230, 14, 0, 0, 9, null },
                    { 231, 14, 0, 0, 10, null },
                    { 232, 14, 0, 0, 11, null },
                    { 233, 14, 0, 0, 12, null },
                    { 234, 14, 0, 0, 13, null },
                    { 235, 14, 0, 0, 14, null },
                    { 236, 14, 0, 0, 15, null },
                    { 237, 14, 0, 0, 16, null },
                    { 238, 14, 0, 0, 17, null },
                    { 239, 15, 0, 0, 1, null },
                    { 240, 15, 0, 0, 2, null },
                    { 241, 15, 0, 0, 3, null },
                    { 242, 15, 0, 0, 4, null },
                    { 243, 15, 0, 0, 5, null },
                    { 244, 15, 0, 0, 6, null },
                    { 245, 15, 0, 0, 7, null },
                    { 246, 15, 0, 0, 8, null },
                    { 247, 15, 0, 0, 9, null },
                    { 248, 15, 0, 0, 10, null },
                    { 249, 15, 0, 0, 11, null },
                    { 250, 15, 0, 0, 12, null },
                    { 251, 15, 0, 0, 13, null },
                    { 252, 15, 0, 0, 14, null },
                    { 253, 15, 0, 0, 15, null },
                    { 254, 15, 0, 0, 16, null },
                    { 255, 15, 0, 0, 17, null },
                    { 256, 16, 0, 0, 1, null },
                    { 257, 16, 0, 0, 2, null },
                    { 258, 16, 0, 0, 3, null },
                    { 259, 16, 0, 0, 4, null },
                    { 260, 16, 0, 0, 5, null },
                    { 261, 16, 0, 0, 6, null },
                    { 262, 16, 0, 0, 7, null },
                    { 263, 16, 0, 0, 8, null },
                    { 264, 16, 0, 0, 9, null },
                    { 265, 16, 0, 0, 10, null },
                    { 266, 16, 0, 0, 11, null },
                    { 267, 16, 0, 0, 12, null },
                    { 268, 16, 0, 0, 13, null },
                    { 269, 16, 0, 0, 14, null },
                    { 270, 16, 0, 0, 15, null },
                    { 271, 16, 0, 0, 16, null },
                    { 272, 16, 0, 0, 17, null },
                    { 273, 17, 0, 0, 1, null },
                    { 274, 17, 0, 0, 2, null },
                    { 275, 17, 0, 0, 3, null },
                    { 276, 17, 0, 0, 4, null },
                    { 277, 17, 0, 0, 5, null },
                    { 278, 17, 0, 0, 6, null },
                    { 279, 17, 0, 0, 7, null },
                    { 280, 17, 0, 0, 8, null },
                    { 281, 17, 0, 0, 9, null },
                    { 282, 17, 0, 0, 10, null },
                    { 283, 17, 0, 0, 11, null },
                    { 284, 17, 0, 0, 12, null },
                    { 285, 17, 0, 0, 13, null },
                    { 286, 17, 0, 0, 14, null },
                    { 287, 17, 0, 0, 15, null },
                    { 288, 17, 0, 0, 16, null },
                    { 289, 17, 0, 0, 17, null },
                    { 290, 18, 0, 0, 1, null },
                    { 291, 18, 0, 0, 2, null },
                    { 292, 18, 0, 0, 3, null },
                    { 293, 18, 0, 0, 4, null },
                    { 294, 18, 0, 0, 5, null },
                    { 295, 18, 0, 0, 6, null },
                    { 296, 18, 0, 0, 7, null },
                    { 297, 18, 0, 0, 8, null },
                    { 298, 18, 0, 0, 9, null },
                    { 299, 18, 0, 0, 10, null },
                    { 300, 18, 0, 0, 11, null },
                    { 301, 18, 0, 0, 12, null },
                    { 302, 18, 0, 0, 13, null },
                    { 303, 18, 0, 0, 14, null },
                    { 304, 18, 0, 0, 15, null },
                    { 305, 18, 0, 0, 16, null },
                    { 306, 18, 0, 0, 17, null },
                    { 307, 19, 0, 0, 1, null },
                    { 308, 19, 0, 0, 2, null },
                    { 309, 19, 0, 0, 3, null },
                    { 310, 19, 0, 0, 4, null },
                    { 311, 19, 0, 0, 5, null },
                    { 312, 19, 0, 0, 6, null },
                    { 313, 19, 0, 0, 7, null },
                    { 314, 19, 0, 0, 8, null },
                    { 315, 19, 0, 0, 9, null },
                    { 316, 19, 0, 0, 10, null },
                    { 317, 19, 0, 0, 11, null },
                    { 318, 19, 0, 0, 12, null },
                    { 319, 19, 0, 0, 13, null },
                    { 320, 19, 0, 0, 14, null },
                    { 321, 19, 0, 0, 15, null },
                    { 322, 19, 0, 0, 16, null },
                    { 323, 19, 0, 0, 17, null },
                    { 324, 20, 0, 0, 1, null },
                    { 325, 20, 0, 0, 2, null },
                    { 326, 20, 0, 0, 3, null },
                    { 327, 20, 0, 0, 4, null },
                    { 328, 20, 0, 0, 5, null },
                    { 329, 20, 0, 0, 6, null },
                    { 330, 20, 0, 0, 7, null },
                    { 331, 20, 0, 0, 8, null },
                    { 332, 20, 0, 0, 9, null },
                    { 333, 20, 0, 0, 10, null },
                    { 334, 20, 0, 0, 11, null },
                    { 335, 20, 0, 0, 12, null },
                    { 336, 20, 0, 0, 13, null },
                    { 337, 20, 0, 0, 14, null },
                    { 338, 20, 0, 0, 15, null },
                    { 339, 20, 0, 0, 16, null },
                    { 340, 20, 0, 0, 17, null },
                    { 341, 21, 0, 0, 1, null },
                    { 342, 21, 0, 0, 2, null },
                    { 343, 21, 0, 0, 3, null },
                    { 344, 21, 0, 0, 4, null },
                    { 345, 21, 0, 0, 5, null },
                    { 346, 21, 0, 0, 6, null },
                    { 347, 21, 0, 0, 7, null },
                    { 348, 21, 0, 0, 8, null },
                    { 349, 21, 0, 0, 9, null },
                    { 350, 21, 0, 0, 10, null },
                    { 351, 21, 0, 0, 11, null },
                    { 352, 21, 0, 0, 12, null },
                    { 353, 21, 0, 0, 13, null },
                    { 354, 21, 0, 0, 14, null },
                    { 355, 21, 0, 0, 15, null },
                    { 356, 21, 0, 0, 16, null },
                    { 357, 21, 0, 0, 17, null },
                    { 358, 22, 0, 0, 1, null },
                    { 359, 22, 0, 0, 2, null },
                    { 360, 22, 0, 0, 3, null },
                    { 361, 22, 0, 0, 4, null },
                    { 362, 22, 0, 0, 5, null },
                    { 363, 22, 0, 0, 6, null },
                    { 364, 22, 0, 0, 7, null },
                    { 365, 22, 0, 0, 8, null },
                    { 366, 22, 0, 0, 9, null },
                    { 367, 22, 0, 0, 10, null },
                    { 368, 22, 0, 0, 11, null },
                    { 369, 22, 0, 0, 12, null },
                    { 370, 22, 0, 0, 13, null },
                    { 371, 22, 0, 0, 14, null },
                    { 372, 22, 0, 0, 15, null },
                    { 373, 22, 0, 0, 16, null },
                    { 374, 22, 0, 0, 17, null },
                    { 375, 23, 0, 0, 1, null },
                    { 376, 23, 0, 0, 2, null },
                    { 377, 23, 0, 0, 3, null },
                    { 378, 23, 0, 0, 4, null },
                    { 379, 23, 0, 0, 5, null },
                    { 380, 23, 0, 0, 6, null },
                    { 381, 23, 0, 0, 7, null },
                    { 382, 23, 0, 0, 8, null },
                    { 383, 23, 0, 0, 9, null },
                    { 384, 23, 0, 0, 10, null },
                    { 385, 23, 0, 0, 11, null },
                    { 386, 23, 0, 0, 12, null },
                    { 387, 23, 0, 0, 13, null },
                    { 388, 23, 0, 0, 14, null },
                    { 389, 23, 0, 0, 15, null },
                    { 390, 23, 0, 0, 16, null },
                    { 391, 23, 0, 0, 17, null },
                    { 392, 24, 0, 0, 1, null },
                    { 393, 24, 0, 0, 2, null },
                    { 394, 24, 0, 0, 3, null },
                    { 395, 24, 0, 0, 4, null },
                    { 396, 24, 0, 0, 5, null },
                    { 397, 24, 0, 0, 6, null },
                    { 398, 24, 0, 0, 7, null },
                    { 399, 24, 0, 0, 8, null },
                    { 400, 24, 0, 0, 9, null },
                    { 401, 24, 0, 0, 10, null },
                    { 402, 24, 0, 0, 11, null },
                    { 403, 24, 0, 0, 12, null },
                    { 404, 24, 0, 0, 13, null },
                    { 405, 24, 0, 0, 14, null },
                    { 406, 24, 0, 0, 15, null },
                    { 407, 24, 0, 0, 16, null },
                    { 408, 24, 0, 0, 17, null },
                    { 409, 25, 0, 0, 1, null },
                    { 410, 25, 0, 0, 2, null },
                    { 411, 25, 0, 0, 3, null },
                    { 412, 25, 0, 0, 4, null },
                    { 413, 25, 0, 0, 5, null },
                    { 414, 25, 0, 0, 6, null },
                    { 415, 25, 0, 0, 7, null },
                    { 416, 25, 0, 0, 8, null },
                    { 417, 25, 0, 0, 9, null },
                    { 418, 25, 0, 0, 10, null },
                    { 419, 25, 0, 0, 11, null },
                    { 420, 25, 0, 0, 12, null },
                    { 421, 25, 0, 0, 13, null },
                    { 422, 25, 0, 0, 14, null },
                    { 423, 25, 0, 0, 15, null },
                    { 424, 25, 0, 0, 16, null },
                    { 425, 25, 0, 0, 17, null },
                    { 426, 26, 0, 0, 1, null },
                    { 427, 26, 0, 0, 2, null },
                    { 428, 26, 0, 0, 3, null },
                    { 429, 26, 0, 0, 4, null },
                    { 430, 26, 0, 0, 5, null },
                    { 431, 26, 0, 0, 6, null },
                    { 432, 26, 0, 0, 7, null },
                    { 433, 26, 0, 0, 8, null },
                    { 434, 26, 0, 0, 9, null },
                    { 435, 26, 0, 0, 10, null },
                    { 436, 26, 0, 0, 11, null },
                    { 437, 26, 0, 0, 12, null },
                    { 438, 26, 0, 0, 13, null },
                    { 439, 26, 0, 0, 14, null },
                    { 440, 26, 0, 0, 15, null },
                    { 441, 26, 0, 0, 16, null },
                    { 442, 26, 0, 0, 17, null },
                    { 443, 27, 0, 0, 1, null },
                    { 444, 27, 0, 0, 2, null },
                    { 445, 27, 0, 0, 3, null },
                    { 446, 27, 0, 0, 4, null },
                    { 447, 27, 0, 0, 5, null },
                    { 448, 27, 0, 0, 6, null },
                    { 449, 27, 0, 0, 7, null },
                    { 450, 27, 0, 0, 8, null },
                    { 451, 27, 0, 0, 9, null },
                    { 452, 27, 0, 0, 10, null },
                    { 453, 27, 0, 0, 11, null },
                    { 454, 27, 0, 0, 12, null },
                    { 455, 27, 0, 0, 13, null },
                    { 456, 27, 0, 0, 14, null },
                    { 457, 27, 0, 0, 15, null },
                    { 458, 27, 0, 0, 16, null },
                    { 459, 27, 0, 0, 17, null },
                    { 460, 28, 0, 0, 1, null },
                    { 461, 28, 0, 0, 2, null },
                    { 462, 28, 0, 0, 3, null },
                    { 463, 28, 0, 0, 4, null },
                    { 464, 28, 0, 0, 5, null },
                    { 465, 28, 0, 0, 6, null },
                    { 466, 28, 0, 0, 7, null },
                    { 467, 28, 0, 0, 8, null },
                    { 468, 28, 0, 0, 9, null },
                    { 469, 28, 0, 0, 10, null },
                    { 470, 28, 0, 0, 11, null },
                    { 471, 28, 0, 0, 12, null },
                    { 472, 28, 0, 0, 13, null },
                    { 473, 28, 0, 0, 14, null },
                    { 474, 28, 0, 0, 15, null },
                    { 475, 28, 0, 0, 16, null },
                    { 476, 28, 0, 0, 17, null },
                    { 477, 29, 0, 0, 1, null },
                    { 478, 29, 0, 0, 2, null },
                    { 479, 29, 0, 0, 3, null },
                    { 480, 29, 0, 0, 4, null },
                    { 481, 29, 0, 0, 5, null },
                    { 482, 29, 0, 0, 6, null },
                    { 483, 29, 0, 0, 7, null },
                    { 484, 29, 0, 0, 8, null },
                    { 485, 29, 0, 0, 9, null },
                    { 486, 29, 0, 0, 10, null },
                    { 487, 29, 0, 0, 11, null },
                    { 488, 29, 0, 0, 12, null },
                    { 489, 29, 0, 0, 13, null },
                    { 490, 29, 0, 0, 14, null },
                    { 491, 29, 0, 0, 15, null },
                    { 492, 29, 0, 0, 16, null },
                    { 493, 29, 0, 0, 17, null },
                    { 494, 30, 0, 0, 1, null },
                    { 495, 30, 0, 0, 2, null },
                    { 496, 30, 0, 0, 3, null },
                    { 497, 30, 0, 0, 4, null },
                    { 498, 30, 0, 0, 5, null },
                    { 499, 30, 0, 0, 6, null },
                    { 500, 30, 0, 0, 7, null },
                    { 501, 30, 0, 0, 8, null },
                    { 502, 30, 0, 0, 9, null },
                    { 503, 30, 0, 0, 10, null },
                    { 504, 30, 0, 0, 11, null },
                    { 505, 30, 0, 0, 12, null },
                    { 506, 30, 0, 0, 13, null },
                    { 507, 30, 0, 0, 14, null },
                    { 508, 30, 0, 0, 15, null },
                    { 509, 30, 0, 0, 16, null },
                    { 510, 30, 0, 0, 17, null },
                    { 511, 31, 0, 0, 1, null },
                    { 512, 31, 0, 0, 2, null },
                    { 513, 31, 0, 0, 3, null },
                    { 514, 31, 0, 0, 4, null },
                    { 515, 31, 0, 0, 5, null },
                    { 516, 31, 0, 0, 6, null },
                    { 517, 31, 0, 0, 7, null },
                    { 518, 31, 0, 0, 8, null },
                    { 519, 31, 0, 0, 9, null },
                    { 520, 31, 0, 0, 10, null },
                    { 521, 31, 0, 0, 11, null },
                    { 522, 31, 0, 0, 12, null },
                    { 523, 31, 0, 0, 13, null },
                    { 524, 31, 0, 0, 14, null },
                    { 525, 31, 0, 0, 15, null },
                    { 526, 31, 0, 0, 16, null },
                    { 527, 31, 0, 0, 17, null },
                    { 528, 32, 0, 0, 1, null },
                    { 529, 32, 0, 0, 2, null },
                    { 530, 32, 0, 0, 3, null },
                    { 531, 32, 0, 0, 4, null },
                    { 532, 32, 0, 0, 5, null },
                    { 533, 32, 0, 0, 6, null },
                    { 534, 32, 0, 0, 7, null },
                    { 535, 32, 0, 0, 8, null },
                    { 536, 32, 0, 0, 9, null },
                    { 537, 32, 0, 0, 10, null },
                    { 538, 32, 0, 0, 11, null },
                    { 539, 32, 0, 0, 12, null },
                    { 540, 32, 0, 0, 13, null },
                    { 541, 32, 0, 0, 14, null },
                    { 542, 32, 0, 0, 15, null },
                    { 543, 32, 0, 0, 16, null },
                    { 544, 32, 0, 0, 17, null },
                    { 545, 33, 0, 0, 1, null },
                    { 546, 33, 0, 0, 2, null },
                    { 547, 33, 0, 0, 3, null },
                    { 548, 33, 0, 0, 4, null },
                    { 549, 33, 0, 0, 5, null },
                    { 550, 33, 0, 0, 6, null },
                    { 551, 33, 0, 0, 7, null },
                    { 552, 33, 0, 0, 8, null },
                    { 553, 33, 0, 0, 9, null },
                    { 554, 33, 0, 0, 10, null },
                    { 555, 33, 0, 0, 11, null },
                    { 556, 33, 0, 0, 12, null },
                    { 557, 33, 0, 0, 13, null },
                    { 558, 33, 0, 0, 14, null },
                    { 559, 33, 0, 0, 15, null },
                    { 560, 33, 0, 0, 16, null },
                    { 561, 33, 0, 0, 17, null },
                    { 562, 34, 0, 0, 1, null },
                    { 563, 34, 0, 0, 2, null },
                    { 564, 34, 0, 0, 3, null },
                    { 565, 34, 0, 0, 4, null },
                    { 566, 34, 0, 0, 5, null },
                    { 567, 34, 0, 0, 6, null },
                    { 568, 34, 0, 0, 7, null },
                    { 569, 34, 0, 0, 8, null },
                    { 570, 34, 0, 0, 9, null },
                    { 571, 34, 0, 0, 10, null },
                    { 572, 34, 0, 0, 11, null },
                    { 573, 34, 0, 0, 12, null },
                    { 574, 34, 0, 0, 13, null },
                    { 575, 34, 0, 0, 14, null },
                    { 576, 34, 0, 0, 15, null },
                    { 577, 34, 0, 0, 16, null },
                    { 578, 34, 0, 0, 17, null },
                    { 579, 35, 0, 0, 1, null },
                    { 580, 35, 0, 0, 2, null },
                    { 581, 35, 0, 0, 3, null },
                    { 582, 35, 0, 0, 4, null },
                    { 583, 35, 0, 0, 5, null },
                    { 584, 35, 0, 0, 6, null },
                    { 585, 35, 0, 0, 7, null },
                    { 586, 35, 0, 0, 8, null },
                    { 587, 35, 0, 0, 9, null },
                    { 588, 35, 0, 0, 10, null },
                    { 589, 35, 0, 0, 11, null },
                    { 590, 35, 0, 0, 12, null },
                    { 591, 35, 0, 0, 13, null },
                    { 592, 35, 0, 0, 14, null },
                    { 593, 35, 0, 0, 15, null },
                    { 594, 35, 0, 0, 16, null },
                    { 595, 35, 0, 0, 17, null },
                    { 596, 36, 0, 0, 1, null },
                    { 597, 36, 0, 0, 2, null },
                    { 598, 36, 0, 0, 3, null },
                    { 599, 36, 0, 0, 4, null },
                    { 600, 36, 0, 0, 5, null },
                    { 601, 36, 0, 0, 6, null },
                    { 602, 36, 0, 0, 7, null },
                    { 603, 36, 0, 0, 8, null },
                    { 604, 36, 0, 0, 9, null },
                    { 605, 36, 0, 0, 10, null },
                    { 606, 36, 0, 0, 11, null },
                    { 607, 36, 0, 0, 12, null },
                    { 608, 36, 0, 0, 13, null },
                    { 609, 36, 0, 0, 14, null },
                    { 610, 36, 0, 0, 15, null },
                    { 611, 36, 0, 0, 16, null },
                    { 612, 36, 0, 0, 17, null },
                    { 613, 37, 0, 0, 1, null },
                    { 614, 37, 0, 0, 2, null },
                    { 615, 37, 0, 0, 3, null },
                    { 616, 37, 0, 0, 4, null },
                    { 617, 37, 0, 0, 5, null },
                    { 618, 37, 0, 0, 6, null },
                    { 619, 37, 0, 0, 7, null },
                    { 620, 37, 0, 0, 8, null },
                    { 621, 37, 0, 0, 9, null },
                    { 622, 37, 0, 0, 10, null },
                    { 623, 37, 0, 0, 11, null },
                    { 624, 37, 0, 0, 12, null },
                    { 625, 37, 0, 0, 13, null },
                    { 626, 37, 0, 0, 14, null },
                    { 627, 37, 0, 0, 15, null },
                    { 628, 37, 0, 0, 16, null },
                    { 629, 37, 0, 0, 17, null },
                    { 630, 38, 0, 0, 1, null },
                    { 631, 38, 0, 0, 2, null },
                    { 632, 38, 0, 0, 3, null },
                    { 633, 38, 0, 0, 4, null },
                    { 634, 38, 0, 0, 5, null },
                    { 635, 38, 0, 0, 6, null },
                    { 636, 38, 0, 0, 7, null },
                    { 637, 38, 0, 0, 8, null },
                    { 638, 38, 0, 0, 9, null },
                    { 639, 38, 0, 0, 10, null },
                    { 640, 38, 0, 0, 11, null },
                    { 641, 38, 0, 0, 12, null },
                    { 642, 38, 0, 0, 13, null },
                    { 643, 38, 0, 0, 14, null },
                    { 644, 38, 0, 0, 15, null },
                    { 645, 38, 0, 0, 16, null },
                    { 646, 38, 0, 0, 17, null },
                    { 647, 39, 0, 0, 1, null },
                    { 648, 39, 0, 0, 2, null },
                    { 649, 39, 0, 0, 3, null },
                    { 650, 39, 0, 0, 4, null },
                    { 651, 39, 0, 0, 5, null },
                    { 652, 39, 0, 0, 6, null },
                    { 653, 39, 0, 0, 7, null },
                    { 654, 39, 0, 0, 8, null },
                    { 655, 39, 0, 0, 9, null },
                    { 656, 39, 0, 0, 10, null },
                    { 657, 39, 0, 0, 11, null },
                    { 658, 39, 0, 0, 12, null },
                    { 659, 39, 0, 0, 13, null },
                    { 660, 39, 0, 0, 14, null },
                    { 661, 39, 0, 0, 15, null },
                    { 662, 39, 0, 0, 16, null },
                    { 663, 39, 0, 0, 17, null },
                    { 664, 40, 0, 0, 1, null },
                    { 665, 40, 0, 0, 2, null },
                    { 666, 40, 0, 0, 3, null },
                    { 667, 40, 0, 0, 4, null },
                    { 668, 40, 0, 0, 5, null },
                    { 669, 40, 0, 0, 6, null },
                    { 670, 40, 0, 0, 7, null },
                    { 671, 40, 0, 0, 8, null },
                    { 672, 40, 0, 0, 9, null },
                    { 673, 40, 0, 0, 10, null },
                    { 674, 40, 0, 0, 11, null },
                    { 675, 40, 0, 0, 12, null },
                    { 676, 40, 0, 0, 13, null },
                    { 677, 40, 0, 0, 14, null },
                    { 678, 40, 0, 0, 15, null },
                    { 679, 40, 0, 0, 16, null },
                    { 680, 40, 0, 0, 17, null },
                    { 681, 41, 0, 0, 1, null },
                    { 682, 41, 0, 0, 2, null },
                    { 683, 41, 0, 0, 3, null },
                    { 684, 41, 0, 0, 4, null },
                    { 685, 41, 0, 0, 5, null },
                    { 686, 41, 0, 0, 6, null },
                    { 687, 41, 0, 0, 7, null },
                    { 688, 41, 0, 0, 8, null },
                    { 689, 41, 0, 0, 9, null },
                    { 690, 41, 0, 0, 10, null },
                    { 691, 41, 0, 0, 11, null },
                    { 692, 41, 0, 0, 12, null },
                    { 693, 41, 0, 0, 13, null },
                    { 694, 41, 0, 0, 14, null },
                    { 695, 41, 0, 0, 15, null },
                    { 696, 41, 0, 0, 16, null },
                    { 697, 41, 0, 0, 17, null },
                    { 698, 42, 0, 0, 1, null },
                    { 699, 42, 0, 0, 2, null },
                    { 700, 42, 0, 0, 3, null },
                    { 701, 42, 0, 0, 4, null },
                    { 702, 42, 0, 0, 5, null },
                    { 703, 42, 0, 0, 6, null },
                    { 704, 42, 0, 0, 7, null },
                    { 705, 42, 0, 0, 8, null },
                    { 706, 42, 0, 0, 9, null },
                    { 707, 42, 0, 0, 10, null },
                    { 708, 42, 0, 0, 11, null },
                    { 709, 42, 0, 0, 12, null },
                    { 710, 42, 0, 0, 13, null },
                    { 711, 42, 0, 0, 14, null },
                    { 712, 42, 0, 0, 15, null },
                    { 713, 42, 0, 0, 16, null },
                    { 714, 42, 0, 0, 17, null },
                    { 715, 43, 0, 0, 1, null },
                    { 716, 43, 0, 0, 2, null },
                    { 717, 43, 0, 0, 3, null },
                    { 718, 43, 0, 0, 4, null },
                    { 719, 43, 0, 0, 5, null },
                    { 720, 43, 0, 0, 6, null },
                    { 721, 43, 0, 0, 7, null },
                    { 722, 43, 0, 0, 8, null },
                    { 723, 43, 0, 0, 9, null },
                    { 724, 43, 0, 0, 10, null },
                    { 725, 43, 0, 0, 11, null },
                    { 726, 43, 0, 0, 12, null },
                    { 727, 43, 0, 0, 13, null },
                    { 728, 43, 0, 0, 14, null },
                    { 729, 43, 0, 0, 15, null },
                    { 730, 43, 0, 0, 16, null },
                    { 731, 43, 0, 0, 17, null },
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
                    { 745, 44, 0, 0, 14, null },
                    { 746, 44, 0, 0, 15, null },
                    { 747, 44, 0, 0, 16, null },
                    { 748, 44, 0, 0, 17, null },
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
                    { 762, 45, 0, 0, 14, null },
                    { 763, 45, 0, 0, 15, null },
                    { 764, 45, 0, 0, 16, null },
                    { 765, 45, 0, 0, 17, null },
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
                    { 779, 46, 0, 0, 14, null },
                    { 780, 46, 0, 0, 15, null },
                    { 781, 46, 0, 0, 16, null },
                    { 782, 46, 0, 0, 17, null },
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
                    { 796, 47, 0, 0, 14, null },
                    { 797, 47, 0, 0, 15, null },
                    { 798, 47, 0, 0, 16, null },
                    { 799, 47, 0, 0, 17, null },
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
                    { 813, 48, 0, 0, 14, null },
                    { 814, 48, 0, 0, 15, null },
                    { 815, 48, 0, 0, 16, null },
                    { 816, 48, 0, 0, 17, null },
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
                    { 830, 49, 0, 0, 14, null },
                    { 831, 49, 0, 0, 15, null },
                    { 832, 49, 0, 0, 16, null },
                    { 833, 49, 0, 0, 17, null },
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
                    { 847, 50, 0, 0, 14, null },
                    { 848, 50, 0, 0, 15, null },
                    { 849, 50, 0, 0, 16, null },
                    { 850, 50, 0, 0, 17, null },
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
                    { 864, 51, 0, 0, 14, null },
                    { 865, 51, 0, 0, 15, null },
                    { 866, 51, 0, 0, 16, null },
                    { 867, 51, 0, 0, 17, null },
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
                    { 881, 52, 0, 0, 14, null },
                    { 882, 52, 0, 0, 15, null },
                    { 883, 52, 0, 0, 16, null },
                    { 884, 52, 0, 0, 17, null },
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
                    { 898, 53, 0, 0, 14, null },
                    { 899, 53, 0, 0, 15, null },
                    { 900, 53, 0, 0, 16, null },
                    { 901, 53, 0, 0, 17, null },
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
                    { 915, 54, 0, 0, 14, null },
                    { 916, 54, 0, 0, 15, null },
                    { 917, 54, 0, 0, 16, null },
                    { 918, 54, 0, 0, 17, null },
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
                    { 932, 55, 0, 0, 14, null },
                    { 933, 55, 0, 0, 15, null },
                    { 934, 55, 0, 0, 16, null },
                    { 935, 55, 0, 0, 17, null },
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
                    { 948, 56, 0, 0, 13, null },
                    { 949, 56, 0, 0, 14, null },
                    { 950, 56, 0, 0, 15, null },
                    { 951, 56, 0, 0, 16, null },
                    { 952, 56, 0, 0, 17, null }
                });

            migrationBuilder.CreateIndex(
                name: "IX_BestSpecializationScore_BossId",
                table: "BestSpecializationScore",
                column: "BossId");

            migrationBuilder.CreateIndex(
                name: "IX_BestSpecializationScore_SpecializationId",
                table: "BestSpecializationScore",
                column: "SpecializationId");

            migrationBuilder.CreateIndex(
                name: "IX_Combat_BossId",
                table: "Combat",
                column: "BossId");

            migrationBuilder.CreateIndex(
                name: "IX_Combat_CombatLogId",
                table: "Combat",
                column: "CombatLogId");

            migrationBuilder.CreateIndex(
                name: "IX_CombatPlayer_CombatId",
                table: "CombatPlayer",
                column: "CombatId");

            migrationBuilder.CreateIndex(
                name: "IX_CombatPlayer_PlayerId",
                table: "CombatPlayer",
                column: "PlayerId");

            migrationBuilder.CreateIndex(
                name: "IX_CombatPlayerAura_CombatPlayerId",
                table: "CombatPlayerAura",
                column: "CombatPlayerId");

            migrationBuilder.CreateIndex(
                name: "IX_CombatPlayerDeath_CombatPlayerId",
                table: "CombatPlayerDeath",
                column: "CombatPlayerId");

            migrationBuilder.CreateIndex(
                name: "IX_CombatPlayerPosition_CombatPlayerId",
                table: "CombatPlayerPosition",
                column: "CombatPlayerId");

            migrationBuilder.CreateIndex(
                name: "IX_CombatPlayerPreAura_CombatPlayerId",
                table: "CombatPlayerPreAura",
                column: "CombatPlayerId");

            migrationBuilder.CreateIndex(
                name: "IX_CombatPlayerStats_CombatPlayerId",
                table: "CombatPlayerStats",
                column: "CombatPlayerId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_CombatTarget_CombatId",
                table: "CombatTarget",
                column: "CombatId");

            migrationBuilder.CreateIndex(
                name: "IX_DamageDone_CombatPlayerId",
                table: "DamageDone",
                column: "CombatPlayerId");

            migrationBuilder.CreateIndex(
                name: "IX_DamageDoneGeneral_CombatPlayerId",
                table: "DamageDoneGeneral",
                column: "CombatPlayerId");

            migrationBuilder.CreateIndex(
                name: "IX_DamageTaken_CombatPlayerId",
                table: "DamageTaken",
                column: "CombatPlayerId");

            migrationBuilder.CreateIndex(
                name: "IX_DamageTakenGeneral_CombatPlayerId",
                table: "DamageTakenGeneral",
                column: "CombatPlayerId");

            migrationBuilder.CreateIndex(
                name: "IX_HealDone_CombatPlayerId",
                table: "HealDone",
                column: "CombatPlayerId");

            migrationBuilder.CreateIndex(
                name: "IX_HealDoneGeneral_CombatPlayerId",
                table: "HealDoneGeneral",
                column: "CombatPlayerId");

            migrationBuilder.CreateIndex(
                name: "IX_ResourceRecovery_CombatPlayerId",
                table: "ResourceRecovery",
                column: "CombatPlayerId");

            migrationBuilder.CreateIndex(
                name: "IX_ResourceRecoveryGeneral_CombatPlayerId",
                table: "ResourceRecoveryGeneral",
                column: "CombatPlayerId");

            migrationBuilder.CreateIndex(
                name: "IX_SpecializationScore_CombatPlayerId",
                table: "SpecializationScore",
                column: "CombatPlayerId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_SpecializationScore_SpecializationId",
                table: "SpecializationScore",
                column: "SpecializationId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "BestSpecializationScore");

            migrationBuilder.DropTable(
                name: "CombatAbility");

            migrationBuilder.DropTable(
                name: "CombatPlayerAura");

            migrationBuilder.DropTable(
                name: "CombatPlayerDeath");

            migrationBuilder.DropTable(
                name: "CombatPlayerPosition");

            migrationBuilder.DropTable(
                name: "CombatPlayerPreAura");

            migrationBuilder.DropTable(
                name: "CombatPlayerStats");

            migrationBuilder.DropTable(
                name: "CombatTarget");

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
                name: "ResourceRecovery");

            migrationBuilder.DropTable(
                name: "ResourceRecoveryGeneral");

            migrationBuilder.DropTable(
                name: "SpecializationScore");

            migrationBuilder.DropTable(
                name: "CombatPlayer");

            migrationBuilder.DropTable(
                name: "Specialization");

            migrationBuilder.DropTable(
                name: "Combat");

            migrationBuilder.DropTable(
                name: "Player");

            migrationBuilder.DropTable(
                name: "Boss");

            migrationBuilder.DropTable(
                name: "CombatLog");
        }
    }
}
