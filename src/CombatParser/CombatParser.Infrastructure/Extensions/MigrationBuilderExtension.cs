using CombatParser.Domain.Aggregates;
using CombatParser.Domain.Entities;
using CombatParser.Infrastructure.Enums;

namespace CombatParser.Infrastructure.Extensions;

internal static class MigrationBuilderExtension
{
    public static Boss[] GenerateBossCollection()
    {
        Boss[] collection =
        [
            // Подземелье Могу'шан
            new(1, 1395, "Каменные стражи", 130841100, 3, 10),
            new(2, 1395, "Каменные стражи", 235513980, 5, 10),
            new(3, 1390, "Фэн Проклятый", 152647950, 3, 10),
            new(4, 1390, "Фэн Проклятый", 209345760, 5, 10),
            new(5, 1434, "Душелов Гара'джал", 117756990, 3, 10),
            new(6, 1434, "Душелов Гара'джал", 179252307, 5, 10),
            new(7, 1436, "Призрачные короли", 174454800, 3, 10),
            new(8, 1436, "Призрачные короли", 261682200, 5, 10),
            new(9, 1500, "Элегон", 294392475, 3, 10),
            new(10, 1500, "Элегон", 339750723, 5, 10),
            new(11, 1407, "Воля императора", 314018640, 3, 10),
            new(12, 1407, "Воля императора", 471027960, 5, 10),

            // Терраса Вечной Весны
            new(13, 1409, "Вечные защитники", 213968815, 3, 10),
            new(14, 1409, "Вечные защитники", 344082093, 5, 10),
            new(15, 1505, "Цулон", 174454800, 3 , 10),
            new(16, 1505, "Цулон", 279127680, 5 , 10),
            new(17, 1506, "Лэй Ши", 138168195, 3 , 10),
            new(18, 1506, "Лэй Ши", 301457900, 5 , 10),
            new(19, 1431, "Ша Страха", 184704020, 3 , 10),
            new(20, 1431, "Ша Страха", 544037304, 5 , 10),

            // Сердце Страха
            new(21, 1507, "Императорский визирь Зор'лок", 174454800, 3, 10),
            new(22, 1507, "Императорский визирь Зор'лок", 218068500, 5, 10),
            new(23, 1504, "Повелитель клинков Та'як", 150467265, 3, 10),
            new(24, 1504, "Повелитель клинков Та'як", 196261650, 5, 10),
            new(25, 1463, "Гаралон", 218068500, 3, 10),
            new(26, 1463, "Гаралон", 290759446, 5, 10),
            new(27, 1498, "Повелитель ветров Мел'джарак", 270404940, 3, 10),
            new(28, 1498, "Повелитель ветров Мел'джарак", 588784950, 5, 10),
            new(29, 1499, "Ваятель янтаря Ун'сок", 218068500, 3, 10),
            new(30, 1499, "Ваятель янтаря Ун'сок", 340186860, 5, 10),
            new(31, 1501, "Великая императрица Шек'зир", 196261650, 3, 10),
            new(32, 1501, "Великая императрица Шек'зир", 307476585, 5, 10),

            // Престол Гроз
            new(33, 1577, "Джин'рок Разрушитель", 207601212, 3, 10),
            new(34, 1577, "Джин'рок Разрушитель", 317507736, 5, 10),
            new(35, 1575, "Хорридон", 357632340, 3, 10),
            new(36, 1575, "Хорридон", 654205500, 5, 10),
            new(37, 1570, "Совет старейшин", 299538888, 3, 10),
            new(38, 1570, "Совет старейшин", 470330152, 5, 10),
            new(39, 1565, "Тортос", 179999841, 3, 10),
            new(40, 1565, "Тортос", 319999818, 5, 10),
            new(41, 1578, "Мегера", 263317712, 3, 10),
            new(42, 1578, "Мегера", 342297774, 5, 10),
            new(43, 1573, "Цзи-Кунь", 244236720, 3, 10),
            new(44, 1573, "Цзи-Кунь", 366355080, 5, 10),
            new(45, 1572, "Дуруму Позабытый", 261682200, 3, 10),
            new(46, 1572, "Дуруму Позабытый", 392523300, 5, 10),
            new(47, 1574, "Изначалий", 218068500, 3, 10),
            new(48, 1574, "Изначалий", 258193104, 5, 10),
            new(49, 1576, "Темный Анимус", 80999797, 3, 10),
            new(50, 1576, "Темный Анимус", 288000023, 5, 10),
            new(51, 1559, "Кон Железный", 119937675, 3, 10),
            new(52, 1559, "Кон Железный", 155700909, 5, 10),
            new(53, 1560, "Небесные сестры", 219812670, 3, 10),
            new(54, 1560, "Небесные сестры", 628036200, 5, 10),
            new(55, 1579, "Лэй Шэнь", 329283435, 3, 10),
            new(56, 1579, "Лэй Шэнь", 580498347, 5, 10)
        ];

        return collection;
    }

    public static Specialization[] GenerateSpecializationCollection()
    {
        Specialization[] collection =
        [
            new(1, "Affliction", "48181,30108,1120"),
            new(2, "Survival", "131900,3674,53301"),
            new(3, "Unholy", "55078,55090,47632"),
            new(4, "Balance", "50288,78674,8921"),
            new(5, "Shadow", "129197,2944,15407"),
            new(6, "Arms", "12294,86346,7384"),
            new(7, "ProtectionWarrior", "6572,23922,20243"),
            new(8, "Brewmaster", "121253,124335,100787"),
            new(9, "Discipline", "47750,81751,585"),
            new(10, "Restoration", "61295,52752,51945"),
            new(11, "Combat", "57841,84617,1752"),
            new(12, "Subtlety", "53,2098,8676"),
            new(13, "Destruction", "29722,116858,348"),
            new(14, "HolyPaladin", "82327,85222,25914"),
            new(15, "ProtectionPaladin", "31935,53600,20271"),
            new(16, "Elemental", "51505,403,8050"),
            new(17, "Frost", "116,44614,30455"),
        ];

        return collection;
    }

    public static CombatAbility[] GenerateCombatAbilityCollection()
    {
        CombatAbility[] collection =
        [
            new(1, 105702, "Зелье Нефритовой Змеи", (int)CombatAbilityType.EfficiencyPotion),
            new(2, 105697, "Укус гну-синя", (int)CombatAbilityType.EfficiencyPotion),
            new(3, 105706, "Зелье силы могу", (int)CombatAbilityType.EfficiencyPotion),
            new(4, 125282, "Бодрящая кафа", (int)CombatAbilityType.EfficiencyPotion),
            new(5, 105696, "Настой кусачих морозов", (int)CombatAbilityType.Elixir),
            new(6, 105689, "Настой весенних цветов", (int)CombatAbilityType.Elixir),
            new(7, 105691, "Настой ласкового солнца", (int)CombatAbilityType.Elixir),
            new(8, 104277, "Сытость", (int)CombatAbilityType.Food),
            new(9, 80353, "Искажение времени", (int)CombatAbilityType.PartyEfficiency),
            new(10, 2825, "Жажда крови", (int)CombatAbilityType.PartyEfficiency),
            new(11, 114207, "Знамя с черепом", (int)CombatAbilityType.PartyEfficiency),
            new(12, 120676, "Тотем порыва бури", (int)CombatAbilityType.PartyEfficiency),
            new(13, 104272, "Сытость", (int)CombatAbilityType.Food),
            new(14, 61316, "Чародейская гениальность Даларана", (int)CombatAbilityType.PartyEfficiency),
            new(15, 1126, "Знак дикой природы", (int)CombatAbilityType.PartyEfficiency),
            new(16, 109773, "Узы Тьмы", (int)CombatAbilityType.PartyEfficiency),
            new(17, 116956, "Легкость воздуха", (int)CombatAbilityType.PartyEfficiency),
            new(18, 77747, "Пылающая ярость", (int)CombatAbilityType.PartyEfficiency),
            new(19, 113742, "Искусство быстрой битвы", (int)CombatAbilityType.PartyEfficiency),
            new(20, 19740, "Благословение могущества", (int)CombatAbilityType.PartyEfficiency),
            new(21, 135678, "Бодрящие споры", (int)CombatAbilityType.PartyEfficiency),
            new(22, 20217, "Благословение королей", (int)CombatAbilityType.PartyEfficiency),
            new(23, 25780, "Праведное неистовство", (int)CombatAbilityType.Other),
        ];

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
                bestScores[index] = new(index + 1, 0, 0, null, spec.Id, boss.Id);
                index++;
            }
        }

        return bestScores;
    }
}
