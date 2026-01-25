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
                    Name = table.Column<string>(type: "nvarchar(126)", maxLength: 126, nullable: false),
                    Health = table.Column<long>(type: "bigint", nullable: false),
                    Difficult = table.Column<int>(type: "int", nullable: false),
                    Size = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Boss", x => x.Id);
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
                name: "Player",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    GameId = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Username = table.Column<string>(type: "nvarchar(126)", maxLength: 126, nullable: false),
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
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    SpecializationSpellsId = table.Column<string>(type: "nvarchar(max)", nullable: false)
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
                    DungeonName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    BossHealthPercentage = table.Column<double>(type: "float", nullable: false),
                    DamageDone = table.Column<int>(type: "int", nullable: false),
                    HealDone = table.Column<int>(type: "int", nullable: false),
                    DamageTaken = table.Column<int>(type: "int", nullable: false),
                    ResourcesRecovery = table.Column<int>(type: "int", nullable: false),
                    IsWin = table.Column<bool>(type: "bit", nullable: false),
                    StartDate = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    FinishDate = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    IsReady = table.Column<bool>(type: "bit", nullable: false),
                    BossId = table.Column<int>(type: "int", nullable: false),
                    CombatLogId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Combat", x => x.Id);
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
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_BestSpecializationScore_Specialization_SpecializationId",
                        column: x => x.SpecializationId,
                        principalTable: "Specialization",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "CombatAura",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(126)", maxLength: 126, nullable: false),
                    Creator = table.Column<string>(type: "nvarchar(126)", maxLength: 126, nullable: false),
                    Target = table.Column<string>(type: "nvarchar(126)", maxLength: 126, nullable: false),
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
                    table.ForeignKey(
                        name: "FK_CombatAura_Combat_CombatId",
                        column: x => x.CombatId,
                        principalTable: "Combat",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
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
                    CombatPlayerId = table.Column<int>(type: "int", nullable: false),
                    CombatId = table.Column<int>(type: "int", nullable: false)
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
                name: "CombatPlayerStats",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
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
                    Spell = table.Column<string>(type: "nvarchar(128)", maxLength: 128, nullable: false),
                    Creator = table.Column<string>(type: "nvarchar(128)", maxLength: 128, nullable: false),
                    Target = table.Column<string>(type: "nvarchar(128)", maxLength: 128, nullable: false),
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
                    Spell = table.Column<string>(type: "nvarchar(128)", maxLength: 128, nullable: false),
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
                    Spell = table.Column<string>(type: "nvarchar(128)", maxLength: 128, nullable: false),
                    Creator = table.Column<string>(type: "nvarchar(128)", maxLength: 128, nullable: false),
                    Target = table.Column<string>(type: "nvarchar(128)", maxLength: 128, nullable: false),
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
                    Spell = table.Column<string>(type: "nvarchar(128)", maxLength: 128, nullable: false),
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
                    { 7, "Protection", "6572,23922,20243" },
                    { 8, "Brewmaster", "121253,124335,100787" },
                    { 9, "Discipline", "47750,81751,47753" },
                    { 10, "Restoration", "61295,52752,51945" },
                    { 11, "Combat", "57841,84617,1752" },
                    { 12, "Subtlety", "53,2098,8676" },
                    { 13, "Destruction", "29722,116858,348" }
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
                    { 14, 2, 0, 0, 1, null },
                    { 15, 2, 0, 0, 2, null },
                    { 16, 2, 0, 0, 3, null },
                    { 17, 2, 0, 0, 4, null },
                    { 18, 2, 0, 0, 5, null },
                    { 19, 2, 0, 0, 6, null },
                    { 20, 2, 0, 0, 7, null },
                    { 21, 2, 0, 0, 8, null },
                    { 22, 2, 0, 0, 9, null },
                    { 23, 2, 0, 0, 10, null },
                    { 24, 2, 0, 0, 11, null },
                    { 25, 2, 0, 0, 12, null },
                    { 26, 2, 0, 0, 13, null },
                    { 27, 3, 0, 0, 1, null },
                    { 28, 3, 0, 0, 2, null },
                    { 29, 3, 0, 0, 3, null },
                    { 30, 3, 0, 0, 4, null },
                    { 31, 3, 0, 0, 5, null },
                    { 32, 3, 0, 0, 6, null },
                    { 33, 3, 0, 0, 7, null },
                    { 34, 3, 0, 0, 8, null },
                    { 35, 3, 0, 0, 9, null },
                    { 36, 3, 0, 0, 10, null },
                    { 37, 3, 0, 0, 11, null },
                    { 38, 3, 0, 0, 12, null },
                    { 39, 3, 0, 0, 13, null },
                    { 40, 4, 0, 0, 1, null },
                    { 41, 4, 0, 0, 2, null },
                    { 42, 4, 0, 0, 3, null },
                    { 43, 4, 0, 0, 4, null },
                    { 44, 4, 0, 0, 5, null },
                    { 45, 4, 0, 0, 6, null },
                    { 46, 4, 0, 0, 7, null },
                    { 47, 4, 0, 0, 8, null },
                    { 48, 4, 0, 0, 9, null },
                    { 49, 4, 0, 0, 10, null },
                    { 50, 4, 0, 0, 11, null },
                    { 51, 4, 0, 0, 12, null },
                    { 52, 4, 0, 0, 13, null },
                    { 53, 5, 0, 0, 1, null },
                    { 54, 5, 0, 0, 2, null },
                    { 55, 5, 0, 0, 3, null },
                    { 56, 5, 0, 0, 4, null },
                    { 57, 5, 0, 0, 5, null },
                    { 58, 5, 0, 0, 6, null },
                    { 59, 5, 0, 0, 7, null },
                    { 60, 5, 0, 0, 8, null },
                    { 61, 5, 0, 0, 9, null },
                    { 62, 5, 0, 0, 10, null },
                    { 63, 5, 0, 0, 11, null },
                    { 64, 5, 0, 0, 12, null },
                    { 65, 5, 0, 0, 13, null },
                    { 66, 6, 0, 0, 1, null },
                    { 67, 6, 0, 0, 2, null },
                    { 68, 6, 0, 0, 3, null },
                    { 69, 6, 0, 0, 4, null },
                    { 70, 6, 0, 0, 5, null },
                    { 71, 6, 0, 0, 6, null },
                    { 72, 6, 0, 0, 7, null },
                    { 73, 6, 0, 0, 8, null },
                    { 74, 6, 0, 0, 9, null },
                    { 75, 6, 0, 0, 10, null },
                    { 76, 6, 0, 0, 11, null },
                    { 77, 6, 0, 0, 12, null },
                    { 78, 6, 0, 0, 13, null },
                    { 79, 7, 0, 0, 1, null },
                    { 80, 7, 0, 0, 2, null },
                    { 81, 7, 0, 0, 3, null },
                    { 82, 7, 0, 0, 4, null },
                    { 83, 7, 0, 0, 5, null },
                    { 84, 7, 0, 0, 6, null },
                    { 85, 7, 0, 0, 7, null },
                    { 86, 7, 0, 0, 8, null },
                    { 87, 7, 0, 0, 9, null },
                    { 88, 7, 0, 0, 10, null },
                    { 89, 7, 0, 0, 11, null },
                    { 90, 7, 0, 0, 12, null },
                    { 91, 7, 0, 0, 13, null },
                    { 92, 8, 0, 0, 1, null },
                    { 93, 8, 0, 0, 2, null },
                    { 94, 8, 0, 0, 3, null },
                    { 95, 8, 0, 0, 4, null },
                    { 96, 8, 0, 0, 5, null },
                    { 97, 8, 0, 0, 6, null },
                    { 98, 8, 0, 0, 7, null },
                    { 99, 8, 0, 0, 8, null },
                    { 100, 8, 0, 0, 9, null },
                    { 101, 8, 0, 0, 10, null },
                    { 102, 8, 0, 0, 11, null },
                    { 103, 8, 0, 0, 12, null },
                    { 104, 8, 0, 0, 13, null },
                    { 105, 9, 0, 0, 1, null },
                    { 106, 9, 0, 0, 2, null },
                    { 107, 9, 0, 0, 3, null },
                    { 108, 9, 0, 0, 4, null },
                    { 109, 9, 0, 0, 5, null },
                    { 110, 9, 0, 0, 6, null },
                    { 111, 9, 0, 0, 7, null },
                    { 112, 9, 0, 0, 8, null },
                    { 113, 9, 0, 0, 9, null },
                    { 114, 9, 0, 0, 10, null },
                    { 115, 9, 0, 0, 11, null },
                    { 116, 9, 0, 0, 12, null },
                    { 117, 9, 0, 0, 13, null },
                    { 118, 10, 0, 0, 1, null },
                    { 119, 10, 0, 0, 2, null },
                    { 120, 10, 0, 0, 3, null },
                    { 121, 10, 0, 0, 4, null },
                    { 122, 10, 0, 0, 5, null },
                    { 123, 10, 0, 0, 6, null },
                    { 124, 10, 0, 0, 7, null },
                    { 125, 10, 0, 0, 8, null },
                    { 126, 10, 0, 0, 9, null },
                    { 127, 10, 0, 0, 10, null },
                    { 128, 10, 0, 0, 11, null },
                    { 129, 10, 0, 0, 12, null },
                    { 130, 10, 0, 0, 13, null },
                    { 131, 11, 0, 0, 1, null },
                    { 132, 11, 0, 0, 2, null },
                    { 133, 11, 0, 0, 3, null },
                    { 134, 11, 0, 0, 4, null },
                    { 135, 11, 0, 0, 5, null },
                    { 136, 11, 0, 0, 6, null },
                    { 137, 11, 0, 0, 7, null },
                    { 138, 11, 0, 0, 8, null },
                    { 139, 11, 0, 0, 9, null },
                    { 140, 11, 0, 0, 10, null },
                    { 141, 11, 0, 0, 11, null },
                    { 142, 11, 0, 0, 12, null },
                    { 143, 11, 0, 0, 13, null },
                    { 144, 12, 0, 0, 1, null },
                    { 145, 12, 0, 0, 2, null },
                    { 146, 12, 0, 0, 3, null },
                    { 147, 12, 0, 0, 4, null },
                    { 148, 12, 0, 0, 5, null },
                    { 149, 12, 0, 0, 6, null },
                    { 150, 12, 0, 0, 7, null },
                    { 151, 12, 0, 0, 8, null },
                    { 152, 12, 0, 0, 9, null },
                    { 153, 12, 0, 0, 10, null },
                    { 154, 12, 0, 0, 11, null },
                    { 155, 12, 0, 0, 12, null },
                    { 156, 12, 0, 0, 13, null },
                    { 157, 13, 0, 0, 1, null },
                    { 158, 13, 0, 0, 2, null },
                    { 159, 13, 0, 0, 3, null },
                    { 160, 13, 0, 0, 4, null },
                    { 161, 13, 0, 0, 5, null },
                    { 162, 13, 0, 0, 6, null },
                    { 163, 13, 0, 0, 7, null },
                    { 164, 13, 0, 0, 8, null },
                    { 165, 13, 0, 0, 9, null },
                    { 166, 13, 0, 0, 10, null },
                    { 167, 13, 0, 0, 11, null },
                    { 168, 13, 0, 0, 12, null },
                    { 169, 13, 0, 0, 13, null },
                    { 170, 14, 0, 0, 1, null },
                    { 171, 14, 0, 0, 2, null },
                    { 172, 14, 0, 0, 3, null },
                    { 173, 14, 0, 0, 4, null },
                    { 174, 14, 0, 0, 5, null },
                    { 175, 14, 0, 0, 6, null },
                    { 176, 14, 0, 0, 7, null },
                    { 177, 14, 0, 0, 8, null },
                    { 178, 14, 0, 0, 9, null },
                    { 179, 14, 0, 0, 10, null },
                    { 180, 14, 0, 0, 11, null },
                    { 181, 14, 0, 0, 12, null },
                    { 182, 14, 0, 0, 13, null },
                    { 183, 15, 0, 0, 1, null },
                    { 184, 15, 0, 0, 2, null },
                    { 185, 15, 0, 0, 3, null },
                    { 186, 15, 0, 0, 4, null },
                    { 187, 15, 0, 0, 5, null },
                    { 188, 15, 0, 0, 6, null },
                    { 189, 15, 0, 0, 7, null },
                    { 190, 15, 0, 0, 8, null },
                    { 191, 15, 0, 0, 9, null },
                    { 192, 15, 0, 0, 10, null },
                    { 193, 15, 0, 0, 11, null },
                    { 194, 15, 0, 0, 12, null },
                    { 195, 15, 0, 0, 13, null },
                    { 196, 16, 0, 0, 1, null },
                    { 197, 16, 0, 0, 2, null },
                    { 198, 16, 0, 0, 3, null },
                    { 199, 16, 0, 0, 4, null },
                    { 200, 16, 0, 0, 5, null },
                    { 201, 16, 0, 0, 6, null },
                    { 202, 16, 0, 0, 7, null },
                    { 203, 16, 0, 0, 8, null },
                    { 204, 16, 0, 0, 9, null },
                    { 205, 16, 0, 0, 10, null },
                    { 206, 16, 0, 0, 11, null },
                    { 207, 16, 0, 0, 12, null },
                    { 208, 16, 0, 0, 13, null },
                    { 209, 17, 0, 0, 1, null },
                    { 210, 17, 0, 0, 2, null },
                    { 211, 17, 0, 0, 3, null },
                    { 212, 17, 0, 0, 4, null },
                    { 213, 17, 0, 0, 5, null },
                    { 214, 17, 0, 0, 6, null },
                    { 215, 17, 0, 0, 7, null },
                    { 216, 17, 0, 0, 8, null },
                    { 217, 17, 0, 0, 9, null },
                    { 218, 17, 0, 0, 10, null },
                    { 219, 17, 0, 0, 11, null },
                    { 220, 17, 0, 0, 12, null },
                    { 221, 17, 0, 0, 13, null },
                    { 222, 18, 0, 0, 1, null },
                    { 223, 18, 0, 0, 2, null },
                    { 224, 18, 0, 0, 3, null },
                    { 225, 18, 0, 0, 4, null },
                    { 226, 18, 0, 0, 5, null },
                    { 227, 18, 0, 0, 6, null },
                    { 228, 18, 0, 0, 7, null },
                    { 229, 18, 0, 0, 8, null },
                    { 230, 18, 0, 0, 9, null },
                    { 231, 18, 0, 0, 10, null },
                    { 232, 18, 0, 0, 11, null },
                    { 233, 18, 0, 0, 12, null },
                    { 234, 18, 0, 0, 13, null },
                    { 235, 19, 0, 0, 1, null },
                    { 236, 19, 0, 0, 2, null },
                    { 237, 19, 0, 0, 3, null },
                    { 238, 19, 0, 0, 4, null },
                    { 239, 19, 0, 0, 5, null },
                    { 240, 19, 0, 0, 6, null },
                    { 241, 19, 0, 0, 7, null },
                    { 242, 19, 0, 0, 8, null },
                    { 243, 19, 0, 0, 9, null },
                    { 244, 19, 0, 0, 10, null },
                    { 245, 19, 0, 0, 11, null },
                    { 246, 19, 0, 0, 12, null },
                    { 247, 19, 0, 0, 13, null },
                    { 248, 20, 0, 0, 1, null },
                    { 249, 20, 0, 0, 2, null },
                    { 250, 20, 0, 0, 3, null },
                    { 251, 20, 0, 0, 4, null },
                    { 252, 20, 0, 0, 5, null },
                    { 253, 20, 0, 0, 6, null },
                    { 254, 20, 0, 0, 7, null },
                    { 255, 20, 0, 0, 8, null },
                    { 256, 20, 0, 0, 9, null },
                    { 257, 20, 0, 0, 10, null },
                    { 258, 20, 0, 0, 11, null },
                    { 259, 20, 0, 0, 12, null },
                    { 260, 20, 0, 0, 13, null },
                    { 261, 21, 0, 0, 1, null },
                    { 262, 21, 0, 0, 2, null },
                    { 263, 21, 0, 0, 3, null },
                    { 264, 21, 0, 0, 4, null },
                    { 265, 21, 0, 0, 5, null },
                    { 266, 21, 0, 0, 6, null },
                    { 267, 21, 0, 0, 7, null },
                    { 268, 21, 0, 0, 8, null },
                    { 269, 21, 0, 0, 9, null },
                    { 270, 21, 0, 0, 10, null },
                    { 271, 21, 0, 0, 11, null },
                    { 272, 21, 0, 0, 12, null },
                    { 273, 21, 0, 0, 13, null },
                    { 274, 22, 0, 0, 1, null },
                    { 275, 22, 0, 0, 2, null },
                    { 276, 22, 0, 0, 3, null },
                    { 277, 22, 0, 0, 4, null },
                    { 278, 22, 0, 0, 5, null },
                    { 279, 22, 0, 0, 6, null },
                    { 280, 22, 0, 0, 7, null },
                    { 281, 22, 0, 0, 8, null },
                    { 282, 22, 0, 0, 9, null },
                    { 283, 22, 0, 0, 10, null },
                    { 284, 22, 0, 0, 11, null },
                    { 285, 22, 0, 0, 12, null },
                    { 286, 22, 0, 0, 13, null },
                    { 287, 23, 0, 0, 1, null },
                    { 288, 23, 0, 0, 2, null },
                    { 289, 23, 0, 0, 3, null },
                    { 290, 23, 0, 0, 4, null },
                    { 291, 23, 0, 0, 5, null },
                    { 292, 23, 0, 0, 6, null },
                    { 293, 23, 0, 0, 7, null },
                    { 294, 23, 0, 0, 8, null },
                    { 295, 23, 0, 0, 9, null },
                    { 296, 23, 0, 0, 10, null },
                    { 297, 23, 0, 0, 11, null },
                    { 298, 23, 0, 0, 12, null },
                    { 299, 23, 0, 0, 13, null },
                    { 300, 24, 0, 0, 1, null },
                    { 301, 24, 0, 0, 2, null },
                    { 302, 24, 0, 0, 3, null },
                    { 303, 24, 0, 0, 4, null },
                    { 304, 24, 0, 0, 5, null },
                    { 305, 24, 0, 0, 6, null },
                    { 306, 24, 0, 0, 7, null },
                    { 307, 24, 0, 0, 8, null },
                    { 308, 24, 0, 0, 9, null },
                    { 309, 24, 0, 0, 10, null },
                    { 310, 24, 0, 0, 11, null },
                    { 311, 24, 0, 0, 12, null },
                    { 312, 24, 0, 0, 13, null },
                    { 313, 25, 0, 0, 1, null },
                    { 314, 25, 0, 0, 2, null },
                    { 315, 25, 0, 0, 3, null },
                    { 316, 25, 0, 0, 4, null },
                    { 317, 25, 0, 0, 5, null },
                    { 318, 25, 0, 0, 6, null },
                    { 319, 25, 0, 0, 7, null },
                    { 320, 25, 0, 0, 8, null },
                    { 321, 25, 0, 0, 9, null },
                    { 322, 25, 0, 0, 10, null },
                    { 323, 25, 0, 0, 11, null },
                    { 324, 25, 0, 0, 12, null },
                    { 325, 25, 0, 0, 13, null },
                    { 326, 26, 0, 0, 1, null },
                    { 327, 26, 0, 0, 2, null },
                    { 328, 26, 0, 0, 3, null },
                    { 329, 26, 0, 0, 4, null },
                    { 330, 26, 0, 0, 5, null },
                    { 331, 26, 0, 0, 6, null },
                    { 332, 26, 0, 0, 7, null },
                    { 333, 26, 0, 0, 8, null },
                    { 334, 26, 0, 0, 9, null },
                    { 335, 26, 0, 0, 10, null },
                    { 336, 26, 0, 0, 11, null },
                    { 337, 26, 0, 0, 12, null },
                    { 338, 26, 0, 0, 13, null },
                    { 339, 27, 0, 0, 1, null },
                    { 340, 27, 0, 0, 2, null },
                    { 341, 27, 0, 0, 3, null },
                    { 342, 27, 0, 0, 4, null },
                    { 343, 27, 0, 0, 5, null },
                    { 344, 27, 0, 0, 6, null },
                    { 345, 27, 0, 0, 7, null },
                    { 346, 27, 0, 0, 8, null },
                    { 347, 27, 0, 0, 9, null },
                    { 348, 27, 0, 0, 10, null },
                    { 349, 27, 0, 0, 11, null },
                    { 350, 27, 0, 0, 12, null },
                    { 351, 27, 0, 0, 13, null },
                    { 352, 28, 0, 0, 1, null },
                    { 353, 28, 0, 0, 2, null },
                    { 354, 28, 0, 0, 3, null },
                    { 355, 28, 0, 0, 4, null },
                    { 356, 28, 0, 0, 5, null },
                    { 357, 28, 0, 0, 6, null },
                    { 358, 28, 0, 0, 7, null },
                    { 359, 28, 0, 0, 8, null },
                    { 360, 28, 0, 0, 9, null },
                    { 361, 28, 0, 0, 10, null },
                    { 362, 28, 0, 0, 11, null },
                    { 363, 28, 0, 0, 12, null },
                    { 364, 28, 0, 0, 13, null },
                    { 365, 29, 0, 0, 1, null },
                    { 366, 29, 0, 0, 2, null },
                    { 367, 29, 0, 0, 3, null },
                    { 368, 29, 0, 0, 4, null },
                    { 369, 29, 0, 0, 5, null },
                    { 370, 29, 0, 0, 6, null },
                    { 371, 29, 0, 0, 7, null },
                    { 372, 29, 0, 0, 8, null },
                    { 373, 29, 0, 0, 9, null },
                    { 374, 29, 0, 0, 10, null },
                    { 375, 29, 0, 0, 11, null },
                    { 376, 29, 0, 0, 12, null },
                    { 377, 29, 0, 0, 13, null },
                    { 378, 30, 0, 0, 1, null },
                    { 379, 30, 0, 0, 2, null },
                    { 380, 30, 0, 0, 3, null },
                    { 381, 30, 0, 0, 4, null },
                    { 382, 30, 0, 0, 5, null },
                    { 383, 30, 0, 0, 6, null },
                    { 384, 30, 0, 0, 7, null },
                    { 385, 30, 0, 0, 8, null },
                    { 386, 30, 0, 0, 9, null },
                    { 387, 30, 0, 0, 10, null },
                    { 388, 30, 0, 0, 11, null },
                    { 389, 30, 0, 0, 12, null },
                    { 390, 30, 0, 0, 13, null },
                    { 391, 31, 0, 0, 1, null },
                    { 392, 31, 0, 0, 2, null },
                    { 393, 31, 0, 0, 3, null },
                    { 394, 31, 0, 0, 4, null },
                    { 395, 31, 0, 0, 5, null },
                    { 396, 31, 0, 0, 6, null },
                    { 397, 31, 0, 0, 7, null },
                    { 398, 31, 0, 0, 8, null },
                    { 399, 31, 0, 0, 9, null },
                    { 400, 31, 0, 0, 10, null },
                    { 401, 31, 0, 0, 11, null },
                    { 402, 31, 0, 0, 12, null },
                    { 403, 31, 0, 0, 13, null },
                    { 404, 32, 0, 0, 1, null },
                    { 405, 32, 0, 0, 2, null },
                    { 406, 32, 0, 0, 3, null },
                    { 407, 32, 0, 0, 4, null },
                    { 408, 32, 0, 0, 5, null },
                    { 409, 32, 0, 0, 6, null },
                    { 410, 32, 0, 0, 7, null },
                    { 411, 32, 0, 0, 8, null },
                    { 412, 32, 0, 0, 9, null },
                    { 413, 32, 0, 0, 10, null },
                    { 414, 32, 0, 0, 11, null },
                    { 415, 32, 0, 0, 12, null },
                    { 416, 32, 0, 0, 13, null },
                    { 417, 33, 0, 0, 1, null },
                    { 418, 33, 0, 0, 2, null },
                    { 419, 33, 0, 0, 3, null },
                    { 420, 33, 0, 0, 4, null },
                    { 421, 33, 0, 0, 5, null },
                    { 422, 33, 0, 0, 6, null },
                    { 423, 33, 0, 0, 7, null },
                    { 424, 33, 0, 0, 8, null },
                    { 425, 33, 0, 0, 9, null },
                    { 426, 33, 0, 0, 10, null },
                    { 427, 33, 0, 0, 11, null },
                    { 428, 33, 0, 0, 12, null },
                    { 429, 33, 0, 0, 13, null },
                    { 430, 34, 0, 0, 1, null },
                    { 431, 34, 0, 0, 2, null },
                    { 432, 34, 0, 0, 3, null },
                    { 433, 34, 0, 0, 4, null },
                    { 434, 34, 0, 0, 5, null },
                    { 435, 34, 0, 0, 6, null },
                    { 436, 34, 0, 0, 7, null },
                    { 437, 34, 0, 0, 8, null },
                    { 438, 34, 0, 0, 9, null },
                    { 439, 34, 0, 0, 10, null },
                    { 440, 34, 0, 0, 11, null },
                    { 441, 34, 0, 0, 12, null },
                    { 442, 34, 0, 0, 13, null },
                    { 443, 35, 0, 0, 1, null },
                    { 444, 35, 0, 0, 2, null },
                    { 445, 35, 0, 0, 3, null },
                    { 446, 35, 0, 0, 4, null },
                    { 447, 35, 0, 0, 5, null },
                    { 448, 35, 0, 0, 6, null },
                    { 449, 35, 0, 0, 7, null },
                    { 450, 35, 0, 0, 8, null },
                    { 451, 35, 0, 0, 9, null },
                    { 452, 35, 0, 0, 10, null },
                    { 453, 35, 0, 0, 11, null },
                    { 454, 35, 0, 0, 12, null },
                    { 455, 35, 0, 0, 13, null },
                    { 456, 36, 0, 0, 1, null },
                    { 457, 36, 0, 0, 2, null },
                    { 458, 36, 0, 0, 3, null },
                    { 459, 36, 0, 0, 4, null },
                    { 460, 36, 0, 0, 5, null },
                    { 461, 36, 0, 0, 6, null },
                    { 462, 36, 0, 0, 7, null },
                    { 463, 36, 0, 0, 8, null },
                    { 464, 36, 0, 0, 9, null },
                    { 465, 36, 0, 0, 10, null },
                    { 466, 36, 0, 0, 11, null },
                    { 467, 36, 0, 0, 12, null },
                    { 468, 36, 0, 0, 13, null },
                    { 469, 37, 0, 0, 1, null },
                    { 470, 37, 0, 0, 2, null },
                    { 471, 37, 0, 0, 3, null },
                    { 472, 37, 0, 0, 4, null },
                    { 473, 37, 0, 0, 5, null },
                    { 474, 37, 0, 0, 6, null },
                    { 475, 37, 0, 0, 7, null },
                    { 476, 37, 0, 0, 8, null },
                    { 477, 37, 0, 0, 9, null },
                    { 478, 37, 0, 0, 10, null },
                    { 479, 37, 0, 0, 11, null },
                    { 480, 37, 0, 0, 12, null },
                    { 481, 37, 0, 0, 13, null },
                    { 482, 38, 0, 0, 1, null },
                    { 483, 38, 0, 0, 2, null },
                    { 484, 38, 0, 0, 3, null },
                    { 485, 38, 0, 0, 4, null },
                    { 486, 38, 0, 0, 5, null },
                    { 487, 38, 0, 0, 6, null },
                    { 488, 38, 0, 0, 7, null },
                    { 489, 38, 0, 0, 8, null },
                    { 490, 38, 0, 0, 9, null },
                    { 491, 38, 0, 0, 10, null },
                    { 492, 38, 0, 0, 11, null },
                    { 493, 38, 0, 0, 12, null },
                    { 494, 38, 0, 0, 13, null },
                    { 495, 39, 0, 0, 1, null },
                    { 496, 39, 0, 0, 2, null },
                    { 497, 39, 0, 0, 3, null },
                    { 498, 39, 0, 0, 4, null },
                    { 499, 39, 0, 0, 5, null },
                    { 500, 39, 0, 0, 6, null },
                    { 501, 39, 0, 0, 7, null },
                    { 502, 39, 0, 0, 8, null },
                    { 503, 39, 0, 0, 9, null },
                    { 504, 39, 0, 0, 10, null },
                    { 505, 39, 0, 0, 11, null },
                    { 506, 39, 0, 0, 12, null },
                    { 507, 39, 0, 0, 13, null },
                    { 508, 40, 0, 0, 1, null },
                    { 509, 40, 0, 0, 2, null },
                    { 510, 40, 0, 0, 3, null },
                    { 511, 40, 0, 0, 4, null },
                    { 512, 40, 0, 0, 5, null },
                    { 513, 40, 0, 0, 6, null },
                    { 514, 40, 0, 0, 7, null },
                    { 515, 40, 0, 0, 8, null },
                    { 516, 40, 0, 0, 9, null },
                    { 517, 40, 0, 0, 10, null },
                    { 518, 40, 0, 0, 11, null },
                    { 519, 40, 0, 0, 12, null },
                    { 520, 40, 0, 0, 13, null },
                    { 521, 41, 0, 0, 1, null },
                    { 522, 41, 0, 0, 2, null },
                    { 523, 41, 0, 0, 3, null },
                    { 524, 41, 0, 0, 4, null },
                    { 525, 41, 0, 0, 5, null },
                    { 526, 41, 0, 0, 6, null },
                    { 527, 41, 0, 0, 7, null },
                    { 528, 41, 0, 0, 8, null },
                    { 529, 41, 0, 0, 9, null },
                    { 530, 41, 0, 0, 10, null },
                    { 531, 41, 0, 0, 11, null },
                    { 532, 41, 0, 0, 12, null },
                    { 533, 41, 0, 0, 13, null },
                    { 534, 42, 0, 0, 1, null },
                    { 535, 42, 0, 0, 2, null },
                    { 536, 42, 0, 0, 3, null },
                    { 537, 42, 0, 0, 4, null },
                    { 538, 42, 0, 0, 5, null },
                    { 539, 42, 0, 0, 6, null },
                    { 540, 42, 0, 0, 7, null },
                    { 541, 42, 0, 0, 8, null },
                    { 542, 42, 0, 0, 9, null },
                    { 543, 42, 0, 0, 10, null },
                    { 544, 42, 0, 0, 11, null },
                    { 545, 42, 0, 0, 12, null },
                    { 546, 42, 0, 0, 13, null },
                    { 547, 43, 0, 0, 1, null },
                    { 548, 43, 0, 0, 2, null },
                    { 549, 43, 0, 0, 3, null },
                    { 550, 43, 0, 0, 4, null },
                    { 551, 43, 0, 0, 5, null },
                    { 552, 43, 0, 0, 6, null },
                    { 553, 43, 0, 0, 7, null },
                    { 554, 43, 0, 0, 8, null },
                    { 555, 43, 0, 0, 9, null },
                    { 556, 43, 0, 0, 10, null },
                    { 557, 43, 0, 0, 11, null },
                    { 558, 43, 0, 0, 12, null },
                    { 559, 43, 0, 0, 13, null },
                    { 560, 44, 0, 0, 1, null },
                    { 561, 44, 0, 0, 2, null },
                    { 562, 44, 0, 0, 3, null },
                    { 563, 44, 0, 0, 4, null },
                    { 564, 44, 0, 0, 5, null },
                    { 565, 44, 0, 0, 6, null },
                    { 566, 44, 0, 0, 7, null },
                    { 567, 44, 0, 0, 8, null },
                    { 568, 44, 0, 0, 9, null },
                    { 569, 44, 0, 0, 10, null },
                    { 570, 44, 0, 0, 11, null },
                    { 571, 44, 0, 0, 12, null },
                    { 572, 44, 0, 0, 13, null },
                    { 573, 45, 0, 0, 1, null },
                    { 574, 45, 0, 0, 2, null },
                    { 575, 45, 0, 0, 3, null },
                    { 576, 45, 0, 0, 4, null },
                    { 577, 45, 0, 0, 5, null },
                    { 578, 45, 0, 0, 6, null },
                    { 579, 45, 0, 0, 7, null },
                    { 580, 45, 0, 0, 8, null },
                    { 581, 45, 0, 0, 9, null },
                    { 582, 45, 0, 0, 10, null },
                    { 583, 45, 0, 0, 11, null },
                    { 584, 45, 0, 0, 12, null },
                    { 585, 45, 0, 0, 13, null },
                    { 586, 46, 0, 0, 1, null },
                    { 587, 46, 0, 0, 2, null },
                    { 588, 46, 0, 0, 3, null },
                    { 589, 46, 0, 0, 4, null },
                    { 590, 46, 0, 0, 5, null },
                    { 591, 46, 0, 0, 6, null },
                    { 592, 46, 0, 0, 7, null },
                    { 593, 46, 0, 0, 8, null },
                    { 594, 46, 0, 0, 9, null },
                    { 595, 46, 0, 0, 10, null },
                    { 596, 46, 0, 0, 11, null },
                    { 597, 46, 0, 0, 12, null },
                    { 598, 46, 0, 0, 13, null },
                    { 599, 47, 0, 0, 1, null },
                    { 600, 47, 0, 0, 2, null },
                    { 601, 47, 0, 0, 3, null },
                    { 602, 47, 0, 0, 4, null },
                    { 603, 47, 0, 0, 5, null },
                    { 604, 47, 0, 0, 6, null },
                    { 605, 47, 0, 0, 7, null },
                    { 606, 47, 0, 0, 8, null },
                    { 607, 47, 0, 0, 9, null },
                    { 608, 47, 0, 0, 10, null },
                    { 609, 47, 0, 0, 11, null },
                    { 610, 47, 0, 0, 12, null },
                    { 611, 47, 0, 0, 13, null },
                    { 612, 48, 0, 0, 1, null },
                    { 613, 48, 0, 0, 2, null },
                    { 614, 48, 0, 0, 3, null },
                    { 615, 48, 0, 0, 4, null },
                    { 616, 48, 0, 0, 5, null },
                    { 617, 48, 0, 0, 6, null },
                    { 618, 48, 0, 0, 7, null },
                    { 619, 48, 0, 0, 8, null },
                    { 620, 48, 0, 0, 9, null },
                    { 621, 48, 0, 0, 10, null },
                    { 622, 48, 0, 0, 11, null },
                    { 623, 48, 0, 0, 12, null },
                    { 624, 48, 0, 0, 13, null },
                    { 625, 49, 0, 0, 1, null },
                    { 626, 49, 0, 0, 2, null },
                    { 627, 49, 0, 0, 3, null },
                    { 628, 49, 0, 0, 4, null },
                    { 629, 49, 0, 0, 5, null },
                    { 630, 49, 0, 0, 6, null },
                    { 631, 49, 0, 0, 7, null },
                    { 632, 49, 0, 0, 8, null },
                    { 633, 49, 0, 0, 9, null },
                    { 634, 49, 0, 0, 10, null },
                    { 635, 49, 0, 0, 11, null },
                    { 636, 49, 0, 0, 12, null },
                    { 637, 49, 0, 0, 13, null },
                    { 638, 50, 0, 0, 1, null },
                    { 639, 50, 0, 0, 2, null },
                    { 640, 50, 0, 0, 3, null },
                    { 641, 50, 0, 0, 4, null },
                    { 642, 50, 0, 0, 5, null },
                    { 643, 50, 0, 0, 6, null },
                    { 644, 50, 0, 0, 7, null },
                    { 645, 50, 0, 0, 8, null },
                    { 646, 50, 0, 0, 9, null },
                    { 647, 50, 0, 0, 10, null },
                    { 648, 50, 0, 0, 11, null },
                    { 649, 50, 0, 0, 12, null },
                    { 650, 50, 0, 0, 13, null },
                    { 651, 51, 0, 0, 1, null },
                    { 652, 51, 0, 0, 2, null },
                    { 653, 51, 0, 0, 3, null },
                    { 654, 51, 0, 0, 4, null },
                    { 655, 51, 0, 0, 5, null },
                    { 656, 51, 0, 0, 6, null },
                    { 657, 51, 0, 0, 7, null },
                    { 658, 51, 0, 0, 8, null },
                    { 659, 51, 0, 0, 9, null },
                    { 660, 51, 0, 0, 10, null },
                    { 661, 51, 0, 0, 11, null },
                    { 662, 51, 0, 0, 12, null },
                    { 663, 51, 0, 0, 13, null },
                    { 664, 52, 0, 0, 1, null },
                    { 665, 52, 0, 0, 2, null },
                    { 666, 52, 0, 0, 3, null },
                    { 667, 52, 0, 0, 4, null },
                    { 668, 52, 0, 0, 5, null },
                    { 669, 52, 0, 0, 6, null },
                    { 670, 52, 0, 0, 7, null },
                    { 671, 52, 0, 0, 8, null },
                    { 672, 52, 0, 0, 9, null },
                    { 673, 52, 0, 0, 10, null },
                    { 674, 52, 0, 0, 11, null },
                    { 675, 52, 0, 0, 12, null },
                    { 676, 52, 0, 0, 13, null },
                    { 677, 53, 0, 0, 1, null },
                    { 678, 53, 0, 0, 2, null },
                    { 679, 53, 0, 0, 3, null },
                    { 680, 53, 0, 0, 4, null },
                    { 681, 53, 0, 0, 5, null },
                    { 682, 53, 0, 0, 6, null },
                    { 683, 53, 0, 0, 7, null },
                    { 684, 53, 0, 0, 8, null },
                    { 685, 53, 0, 0, 9, null },
                    { 686, 53, 0, 0, 10, null },
                    { 687, 53, 0, 0, 11, null },
                    { 688, 53, 0, 0, 12, null },
                    { 689, 53, 0, 0, 13, null },
                    { 690, 54, 0, 0, 1, null },
                    { 691, 54, 0, 0, 2, null },
                    { 692, 54, 0, 0, 3, null },
                    { 693, 54, 0, 0, 4, null },
                    { 694, 54, 0, 0, 5, null },
                    { 695, 54, 0, 0, 6, null },
                    { 696, 54, 0, 0, 7, null },
                    { 697, 54, 0, 0, 8, null },
                    { 698, 54, 0, 0, 9, null },
                    { 699, 54, 0, 0, 10, null },
                    { 700, 54, 0, 0, 11, null },
                    { 701, 54, 0, 0, 12, null },
                    { 702, 54, 0, 0, 13, null },
                    { 703, 55, 0, 0, 1, null },
                    { 704, 55, 0, 0, 2, null },
                    { 705, 55, 0, 0, 3, null },
                    { 706, 55, 0, 0, 4, null },
                    { 707, 55, 0, 0, 5, null },
                    { 708, 55, 0, 0, 6, null },
                    { 709, 55, 0, 0, 7, null },
                    { 710, 55, 0, 0, 8, null },
                    { 711, 55, 0, 0, 9, null },
                    { 712, 55, 0, 0, 10, null },
                    { 713, 55, 0, 0, 11, null },
                    { 714, 55, 0, 0, 12, null },
                    { 715, 55, 0, 0, 13, null },
                    { 716, 56, 0, 0, 1, null },
                    { 717, 56, 0, 0, 2, null },
                    { 718, 56, 0, 0, 3, null },
                    { 719, 56, 0, 0, 4, null },
                    { 720, 56, 0, 0, 5, null },
                    { 721, 56, 0, 0, 6, null },
                    { 722, 56, 0, 0, 7, null },
                    { 723, 56, 0, 0, 8, null },
                    { 724, 56, 0, 0, 9, null },
                    { 725, 56, 0, 0, 10, null },
                    { 726, 56, 0, 0, 11, null },
                    { 727, 56, 0, 0, 12, null },
                    { 728, 56, 0, 0, 13, null }
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
                name: "IX_Combat_CombatLogId",
                table: "Combat",
                column: "CombatLogId");

            migrationBuilder.CreateIndex(
                name: "IX_CombatAura_CombatId",
                table: "CombatAura",
                column: "CombatId");

            migrationBuilder.CreateIndex(
                name: "IX_CombatPlayer_CombatId",
                table: "CombatPlayer",
                column: "CombatId");

            migrationBuilder.CreateIndex(
                name: "IX_CombatPlayer_PlayerId",
                table: "CombatPlayer",
                column: "PlayerId");

            migrationBuilder.CreateIndex(
                name: "IX_CombatPlayerDeath_CombatPlayerId",
                table: "CombatPlayerDeath",
                column: "CombatPlayerId");

            migrationBuilder.CreateIndex(
                name: "IX_CombatPlayerPosition_CombatPlayerId",
                table: "CombatPlayerPosition",
                column: "CombatPlayerId");

            migrationBuilder.CreateIndex(
                name: "IX_CombatPlayerStats_CombatPlayerId",
                table: "CombatPlayerStats",
                column: "CombatPlayerId",
                unique: true);

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
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "BestSpecializationScore");

            migrationBuilder.DropTable(
                name: "CombatAura");

            migrationBuilder.DropTable(
                name: "CombatPlayerDeath");

            migrationBuilder.DropTable(
                name: "CombatPlayerPosition");

            migrationBuilder.DropTable(
                name: "CombatPlayerStats");

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
                name: "Boss");

            migrationBuilder.DropTable(
                name: "Specialization");

            migrationBuilder.DropTable(
                name: "CombatPlayer");

            migrationBuilder.DropTable(
                name: "Combat");

            migrationBuilder.DropTable(
                name: "Player");

            migrationBuilder.DropTable(
                name: "CombatLog");
        }
    }
}
