using CombatParser.Domain.Aggregates;
using CombatParser.Domain.Entities;
using CombatParser.Domain.Entities.CombatPlayerData;
using Microsoft.EntityFrameworkCore.Migrations;
using System.Text;

namespace CombatParser.Infrastructure.Extensions;

internal static class MigrationBuilderExtension
{
    private static readonly Type[] _tableTypes =
    [       
            typeof(DamageDone),
            typeof(DamageDoneGeneral),
            typeof(HealDone),
            typeof(HealDoneGeneral),
            typeof(DamageTaken),
            typeof(DamageTakenGeneral),
            typeof(ResourceRecovery),
            typeof(ResourceRecoveryGeneral),
            typeof(CombatAura),
            typeof(CombatPlayerPosition),
            typeof(CombatPlayerDeath),
    ];

    public static void CreateTableTypes(this MigrationBuilder migrationBuilder)
    {
        foreach (var item in _tableTypes)
        {
            var typeParams = CreateParams(item, false);
            migrationBuilder.Sql($"CREATE TYPE dbo.{item.Name}Type AS TABLE({typeParams.Item1});");

            migrationBuilder.Sql($"""
            EXEC('
                CREATE OR ALTER PROCEDURE dbo.InsertInto{item.Name}Batch
                    @Items dbo.{item.Name}Type READONLY
                AS
                BEGIN
                    SET NOCOUNT ON;

                    INSERT INTO {item.Name} ({typeParams.Item2})
                    SELECT {typeParams.Item2} FROM @Items;
                END
            ');
            """);
        }
    }

    public static void DropTableTypes(this MigrationBuilder migrationBuilder)
    {
        foreach (var item in _tableTypes)
        {
            migrationBuilder.Sql($"DROP TYPE IF EXISTS dbo.{item.Name}Type");
            migrationBuilder.Sql($"DROP PROCEDURE IF EXISTS dbo.InsertInto{item.Name}Batch");
        }
    }

    public static void CreateEnhancedTableTypes(this MigrationBuilder migrationBuilder)
    {
        migrationBuilder.Sql($"""
            CREATE TYPE dbo.CombatPlayerType AS TABLE
            (
                RowId INT NOT NULL,

                -- CombatPlayer
                AverageItemLevel FLOAT NOT NULL,
                ResourcesRecovery INT NOT NULL,
                DamageDone INT NOT NULL,
                HealDone INT NOT NULL,
                DamageTaken INT NOT NULL,
                PlayerId NVARCHAR(MAX) NOT NULL,
                CombatId INT NOT NULL,

                -- Stats
                Strength INT NOT NULL,
                Agility INT NOT NULL,
                Intelligence INT NOT NULL,
                Stamina INT NOT NULL,
                Spirit INT NOT NULL,
                Dodge INT NOT NULL,
                Parry INT NOT NULL,
                Crit INT NOT NULL,
                Haste INT NOT NULL,
                Hit INT NOT NULL,
                Expertise INT NOT NULL,
                Armor INT NOT NULL,
                Talents NVARCHAR(126) NOT NULL,

                -- Score
                DamageScore FLOAT NULL,
                HealScore FLOAT NULL,
                Updated DATETIMEOFFSET NULL,
                SpecializationId INT NULL
            );
            """);

        migrationBuilder.Sql($"""
            EXEC('
                CREATE OR ALTER PROCEDURE dbo.InsertIntoCombatPlayerBatch
                    @Items dbo.CombatPlayerType READONLY
                AS
                BEGIN
                    SET NOCOUNT ON;

                    DECLARE @Inserted TABLE
                    (
                        RowId INT NOT NULL,
                        CombatPlayerId INT NOT NULL
                    );

                    MERGE CombatPlayer AS target
                    USING @Items AS src
                    ON 1 = 0
                    WHEN NOT MATCHED THEN
                        INSERT
                        (
                            AverageItemLevel,
                            ResourcesRecovery,
                            DamageDone,
                            HealDone,
                            DamageTaken,
                            PlayerId,
                            CombatId
                        )
                        VALUES
                        (
                            src.AverageItemLevel,
                            src.ResourcesRecovery,
                            src.DamageDone,
                            src.HealDone,
                            src.DamageTaken,
                            src.PlayerId,
                            src.CombatId
                        )
                    OUTPUT
                        src.RowId,
                        inserted.Id
                    INTO @Inserted (RowId, CombatPlayerId);

                    -- CombatPlayerStats
                    INSERT INTO CombatPlayerStats
                    (
                        CombatPlayerId, Strength, Agility, Intelligence, Stamina,
                        Spirit, Dodge, Parry, Crit, Haste, Hit, Expertise, Armor, Talents
                    )
                    SELECT
                        i.CombatPlayerId,
                        src.Strength, src.Agility, src.Intelligence, src.Stamina,
                        src.Spirit, src.Dodge, src.Parry, src.Crit, src.Haste,
                        src.Hit, src.Expertise, src.Armor, src.Talents
                    FROM @Items src
                    JOIN @Inserted i ON i.RowId = src.RowId;

                    -- SpecializationScore
                    INSERT INTO SpecializationScore
                    (
                        CombatPlayerId, DamageScore, HealScore,
                        DamageDone, HealDone, SpecializationId, Updated
                    )
                    SELECT
                        i.CombatPlayerId,
                        src.DamageScore, src.HealScore,
                        src.DamageDone, src.HealDone,
                        src.SpecializationId, src.Updated
                    FROM @Items src
                    JOIN @Inserted i ON i.RowId = src.RowId
                    WHERE src.DamageScore IS NOT NULL;
                END
            ');
            """);
    }

    public static void DropEnhancedTableTypes(this MigrationBuilder migrationBuilder)
    {
        migrationBuilder.Sql($"DROP TYPE IF EXISTS dbo.CombatPlayerType");
        migrationBuilder.Sql($"DROP PROCEDURE IF EXISTS dbo.InsertCombatPlayerBatch");
    }

    public static Boss[] GenerateBossCollection()
    {
        Boss[] collection =
        [
            // Подземелье Могу'шан
            new() { GameId = 1395, Name = "Каменные стражи", Health = 130841100, Difficult = 3, Size = 10 },
            new() { GameId = 1395, Name = "Каменные стражи", Health = 235513980, Difficult = 5, Size = 10 },
            new() { GameId = 1390, Name = "Фэн Проклятый", Health = 152647950, Difficult = 3, Size = 10 },
            new() { GameId = 1390, Name = "Фэн Проклятый", Health = 209345760, Difficult = 5, Size = 10 },
            new() { GameId = 1434, Name = "Душелов Гара'джал", Health = 117756990, Difficult = 3, Size = 10 },
            new() { GameId = 1434, Name = "Душелов Гара'джал", Health = 179252307, Difficult = 5, Size = 10 },
            new() { GameId = 1436, Name = "Призрачные короли", Health = 174454800, Difficult = 3, Size = 10 },
            new() { GameId = 1436, Name = "Призрачные короли", Health = 261682200, Difficult = 5, Size = 10 },
            new() { GameId = 1500, Name = "Элегон", Health = 294392475, Difficult = 3, Size = 10 },
            new() { GameId = 1500, Name = "Элегон", Health = 339750723, Difficult = 5, Size = 10 },
            new() { GameId = 1407, Name = "Воля императора", Health = 314018640, Difficult = 3, Size = 10 },
            new() { GameId = 1407, Name = "Воля императора", Health = 471027960, Difficult = 5, Size = 10 },

            // Терраса Вечной Весны
            new() { GameId = 1409, Name = "Вечные защитники", Health = 213968815, Difficult = 3, Size = 10 },
            new() { GameId = 1409, Name = "Вечные защитники", Health = 344082093, Difficult = 5, Size = 10 },
            new() { GameId = 1505, Name = "Цулон", Health = 174454800, Difficult = 3 , Size = 10 },
            new() { GameId = 1505, Name = "Цулон", Health = 279127680, Difficult = 5 , Size = 10 },
            new() { GameId = 1506, Name = "Лэй Ши", Health = 138168195, Difficult = 3 , Size = 10 },
            new() { GameId = 1506, Name = "Лэй Ши", Health = 301457900, Difficult = 5 , Size = 10 },
            new() { GameId = 1431, Name = "Ша Страха", Health = 184704020, Difficult = 3 , Size = 10 },
            new() { GameId = 1431, Name = "Ша Страха", Health = 544037304, Difficult = 5 , Size = 10 },

            // Сердце Страха
            new() { GameId = 1507, Name = "Императорский визирь Зор'лок", Health = 174454800, Difficult = 3, Size = 10 },
            new() { GameId = 1507, Name = "Императорский визирь Зор'лок", Health = 218068500, Difficult = 5, Size = 10 },
            new() { GameId = 1504, Name = "Повелитель клинков Та'як", Health = 150467265, Difficult = 3, Size = 10 },
            new() { GameId = 1504, Name = "Повелитель клинков Та'як", Health = 196261650, Difficult = 5, Size = 10 },
            new() { GameId = 1463, Name = "Гаралон", Health = 218068500, Difficult = 3, Size = 10 },
            new() { GameId = 1463, Name = "Гаралон", Health = 290759446, Difficult = 5, Size = 10 },
            new() { GameId = 1498, Name = "Повелитель ветров Мел'джарак", Health = 270404940, Difficult = 3, Size = 10 },
            new() { GameId = 1498, Name = "Повелитель ветров Мел'джарак", Health = 588784950, Difficult = 5, Size = 10 },
            new() { GameId = 1499, Name = "Ваятель янтаря Ун'сок", Health = 218068500, Difficult = 3, Size = 10 },
            new() { GameId = 1499, Name = "Ваятель янтаря Ун'сок", Health = 340186860, Difficult = 5, Size = 10 },
            new() { GameId = 1501, Name = "Великая императрица Шек'зир", Health = 196261650, Difficult = 3, Size = 10 },
            new() { GameId = 1501, Name = "Великая императрица Шек'зир", Health = 307476585, Difficult = 5, Size = 10 },

            // Престол Гроз
            new() { GameId = 1577, Name = "Джин'рок Разрушитель", Health = 207601212, Difficult = 3, Size = 10 },
            new() { GameId = 1577, Name = "Джин'рок Разрушитель", Health = 317507736, Difficult = 5, Size = 10 },
            new() { GameId = 1575, Name = "Хорридон", Health = 357632340, Difficult = 3, Size = 10 },
            new() { GameId = 1575, Name = "Хорридон", Health = 654205500, Difficult = 5, Size = 10 },
            new() { GameId = 1570, Name = "Совет старейшин", Health = 299538888, Difficult = 3, Size = 10 },
            new() { GameId = 1570, Name = "Совет старейшин", Health = 470330152, Difficult = 5, Size = 10 },
            new() { GameId = 1565, Name = "Тортос", Health = 179999841, Difficult = 3, Size = 10 },
            new() { GameId = 1565, Name = "Тортос", Health = 319999818, Difficult = 5, Size = 10 },
            new() { GameId = 1578, Name = "Мегера", Health = 263317712, Difficult = 3, Size = 10 },
            new() { GameId = 1578, Name = "Мегера", Health = 342297774, Difficult = 5, Size = 10 },
            new() { GameId = 1573, Name = "Цзи-Кунь", Health = 244236720, Difficult = 3, Size = 10 },
            new() { GameId = 1573, Name = "Цзи-Кунь", Health = 366355080, Difficult = 5, Size = 10 },
            new() { GameId = 1572, Name = "Дуруму Позабытый", Health = 261682200, Difficult = 3, Size = 10 },
            new() { GameId = 1572, Name = "Дуруму Позабытый", Health = 392523300, Difficult = 5, Size = 10 },
            new() { GameId = 1574, Name = "Изначалий", Health = 218068500, Difficult = 3, Size = 10 },
            new() { GameId = 1574, Name = "Изначалий", Health = 258193104, Difficult = 5, Size = 10 },
            new() { GameId = 1576, Name = "Темный Анимус", Health = 80999797, Difficult = 3, Size = 10 },
            new() { GameId = 1576, Name = "Темный Анимус", Health = 288000023, Difficult = 5, Size = 10 },
            new() { GameId = 1559, Name = "Кон Железный", Health = 119937675, Difficult = 3, Size = 10 },
            new() { GameId = 1559, Name = "Кон Железный", Health = 155700909, Difficult = 5, Size = 10 },
            new() { GameId = 1560, Name = "Небесные сестры", Health = 219812670, Difficult = 3, Size = 10 },
            new() { GameId = 1560, Name = "Небесные сестры", Health = 628036200, Difficult = 5, Size = 10 },
            new() { GameId = 1579, Name = "Лэй Шэнь", Health = 329283435, Difficult = 3, Size = 10 },
            new() { GameId = 1579, Name = "Лэй Шэнь", Health = 580498347, Difficult = 5, Size = 10 }
        ];

        for (int i = 0; i < collection.Length; i++)
        {
            collection[i].Id = i + 1;
        }

        return collection;
    }

    public static Specialization[] GenerateSpecializationCollection()
    {
        Specialization[] collection =
        [
            new() { Name = "Affliction", SpecializationSpellsId = "48181,30108,1120" },
            new() { Name = "Survival", SpecializationSpellsId = "131900,3674,53301" },
            new() { Name = "Unholy", SpecializationSpellsId = "55078,55090,47632" },
            new() { Name = "Balance", SpecializationSpellsId = "50288,78674,8921" },
            new() { Name = "Shadow", SpecializationSpellsId = "129197,2944,15407" },
            new() { Name = "Arms", SpecializationSpellsId = "12294,86346,7384" },
            new() { Name = "Protection", SpecializationSpellsId = "6572,23922,20243" },
            new() { Name = "Brewmaster", SpecializationSpellsId = "121253,124335,100787" },
            new() { Name = "Discipline", SpecializationSpellsId = "47750,81751,47753" },
            new() { Name = "Restoration", SpecializationSpellsId = "61295,52752,51945" },
            new() { Name = "Combat", SpecializationSpellsId = "57841,84617,1752" },
            new() { Name = "Subtlety", SpecializationSpellsId = "53,2098,8676" },
            new() { Name = "Destruction", SpecializationSpellsId = "29722,116858,348" },
        ];

        for (int i = 0; i < collection.Length; i++)
        {
            collection[i].Id = i + 1;
        }

        return collection;
    }
    
    public static BestSpecializationScore[] GenerateBestSpecializationScoreCollection()
    {
        var bosses = GenerateBossCollection();
        var specs = GenerateSpecializationCollection();
        var bestScores = new BestSpecializationScore[bosses.Length * specs.Length];
        var index = 0;

        foreach (var boss in bosses)
        {
            foreach (var spec in specs)
            {
                bestScores[index] = new() { Id = index + 1, BossId = boss.Id, SpecializationId = spec.Id };
                index++;
            }
        }

        return bestScores;
    }

    private static Tuple<string, string> CreateParams(Type type, bool isStoredProcedure = true)
    {
        Type[] navigatorTypes =
        [
            typeof(CombatPlayer),
            typeof(Player),
            typeof(Combat),
            typeof(CombatPlayerStats),
            typeof(SpecializationScore),
            typeof(Specialization),
            typeof(IEnumerable<DamageDone>),
            typeof(IEnumerable<DamageDoneGeneral>),
            typeof(IEnumerable<HealDone>),
            typeof(IEnumerable<HealDoneGeneral>),
            typeof(IEnumerable<DamageTaken>),
            typeof(IEnumerable<DamageTakenGeneral>),
            typeof(IEnumerable<CombatPlayerDeath>),
            typeof(IEnumerable<ResourceRecovery>),
            typeof(IEnumerable<ResourceRecoveryGeneral>),
            typeof(IEnumerable<CombatPlayerPosition>),
        ];

        var properties = type.GetProperties();
        var paramNames = new StringBuilder();
        var paramNamesWithPropertyTypes = new StringBuilder();
        if (type.GetProperty("Id")?.PropertyType != typeof(int))
        {
            paramNamesWithPropertyTypes.Append(isStoredProcedure ? $"@{properties[0].Name} {Converter(properties[0].PropertyType)}," : $"{properties[0].Name} {Converter(properties[0].PropertyType)} NOT NULL,");
            paramNames.Append(isStoredProcedure ? $"@{properties[0].Name}," : $"{properties[0].Name},");
        }

        for (int i = 1; i < properties.Length; i++)
        {
            if (properties[i].CanWrite && !navigatorTypes.Contains(properties[i].PropertyType))
            {
                paramNamesWithPropertyTypes.Append(isStoredProcedure ? $"@{properties[i].Name} {Converter(properties[i].PropertyType)}," : $"{properties[i].Name} {Converter(properties[i].PropertyType)},");
                paramNames.Append(isStoredProcedure ? $"@{properties[i].Name}," : $"{properties[i].Name},");
            }
        }

        paramNamesWithPropertyTypes.Remove(paramNamesWithPropertyTypes.Length - 1, 1);
        paramNames.Remove(paramNames.Length - 1, 1);

        return new Tuple<string, string>(paramNamesWithPropertyTypes.ToString(), paramNames.ToString());
    }

    private static string Converter(Type type)
    {
        if (Nullable.GetUnderlyingType(type) is Type underlying)
        {
            type = underlying;
        }

        return type.Name switch
        {
            "String" => "NVARCHAR (MAX)",
            "Int32" => "INT",
            "Int16" => "INT",
            "Boolean" => "BIT",
            "DateTimeOffset" => "DATETIMEOFFSET (7)",
            "Double" => "FLOAT (53)",
            "TimeSpan" => "TIME (7)",
            _ => "NVARCHAR (256)",
        };
    }
}
