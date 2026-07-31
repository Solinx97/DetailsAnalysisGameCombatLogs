using CombatParser.Domain.Aggregates;
using CombatParser.Domain.Entities;
using CombatParser.Infrastructure.Enums;

namespace CombatParser.Infrastructure.Extensions;

internal static class MigrationBuilderExtension
{
    public static BossMap[] GenerateMaps()
    {
        BossMap[] collection =
        [
            // Осада Огриммара
            BossMap.Create(1, 556, "Осада Оргриммара", 1000, 550, 1600, 500),
            BossMap.Create(2, 557, "Осада Оргриммара", 1325, 1000, 1450, 150),
            BossMap.Create(3, 558, "Осада Оргриммара", 700, 525, 1350, 600),
            BossMap.Create(4, 559, "Осада Оргриммара", 1250, 900, -4530, -5160),
            BossMap.Create(5, 560, "Осада Оргриммара", 1525, 1300, -3800, -5000),
            BossMap.Create(6, 562, "Осада Оргриммара", 1560, 1500, -4430, -4850),
            BossMap.Create(7, 563, "Осада Оргриммара", 1800, 1575, -4425, -5467),
            BossMap.Create(8, 564, "Осада Оргриммара", 1450, 1060, -4555, -5700),
            BossMap.Create(9, 565, "Осада Оргриммара", 1900, 1720, -5370, -5875),
            BossMap.Create(10, 566, "Осада Оргриммара", 1465, 1200, -5375, -5967),
            BossMap.Create(11, 567, "Осада Оргриммара", 1035, 960, -5400, -5867),
            BossMap.Create(12, 558, "Осада Оргриммара", 665, 500, 1575, 675),
            BossMap.Create(13, 559, "Осада Оргриммара", 1150, 800, -3980, -4900),
            BossMap.Create(14, 564, "Осада Оргриммара", 1200, 1060, -4470, -5700),
        ];

        return collection;
    }

    public static Boss[] GenerateBosses()
    {
        Boss[] collection =
        [
            #region Подземелье Могу'шан

            Boss.Create(1, 1395, "Каменные стражи", 130841100, 3, 10, 1),
            Boss.Create(2, 1395, "Каменные стражи", 235513980, 5, 10, 1),
            Boss.Create(3, 1390, "Фэн Проклятый", 152647950, 3, 10, 1),
            Boss.Create(4, 1390, "Фэн Проклятый", 209345760, 5, 10, 1),
            Boss.Create(5, 1434, "Душелов Гара'джал", 117756990, 3, 10, 1),
            Boss.Create(6, 1434, "Душелов Гара'джал", 179252307, 5, 10, 1),
            Boss.Create(7, 1436, "Призрачные короли", 174454800, 3, 10, 1),
            Boss.Create(8, 1436, "Призрачные короли", 261682200, 5, 10, 1),
            Boss.Create(9, 1500, "Элегон", 294392475, 3, 10, 1),
            Boss.Create(10, 1500, "Элегон", 339750723, 5, 10, 1),
            Boss.Create(11, 1407, "Воля императора", 314018640, 3, 10, 1),
            Boss.Create(12, 1407, "Воля императора", 471027960, 5, 10, 1),

            #endregion

            #region Терраса Вечной Весны

            Boss.Create(13, 1409, "Вечные защитники", 213968815, 3, 10, 1),
            Boss.Create(14, 1409, "Вечные защитники", 344082093, 5, 10, 1),
            Boss.Create(15, 1505, "Цулон", 174454800, 3 , 10, 1),
            Boss.Create(16, 1505, "Цулон", 279127680, 5 , 10, 1),
            Boss.Create(17, 1506, "Лэй Ши", 138168195, 3 , 10, 1),
            Boss.Create(18, 1506, "Лэй Ши", 301457900, 5 , 10, 1),
            Boss.Create(19, 1431, "Ша Страха", 184704020, 3 , 10, 1),
            Boss.Create(20, 1431, "Ша Страха", 544037304, 5 , 10, 1),

            #endregion

            #region Сердце Страха

            Boss.Create(21, 1507, "Императорский визирь Зор'лок", 174454800, 3, 10, 1),
            Boss.Create(22, 1507, "Императорский визирь Зор'лок", 218068500, 5, 10, 1),
            Boss.Create(23, 1504, "Повелитель клинков Та'як", 150467265, 3, 10, 1),
            Boss.Create(24, 1504, "Повелитель клинков Та'як", 196261650, 5, 10, 1),
            Boss.Create(25, 1463, "Гаралон", 218068500, 3, 10, 1),
            Boss.Create(26, 1463, "Гаралон", 290759446, 5, 10, 1),
            Boss.Create(27, 1498, "Повелитель ветров Мел'джарак", 270404940, 3, 10, 1),
            Boss.Create(28, 1498, "Повелитель ветров Мел'джарак", 588784950, 5, 10, 1),
            Boss.Create(29, 1499, "Ваятель янтаря Ун'сок", 218068500, 3, 10, 1),
            Boss.Create(30, 1499, "Ваятель янтаря Ун'сок", 340186860, 5, 10, 1),
            Boss.Create(31, 1501, "Великая императрица Шек'зир", 196261650, 3, 10, 1),
            Boss.Create(32, 1501, "Великая императрица Шек'зир", 307476585, 5, 10, 1),

            #endregion

            #region Престол Гроз

            Boss.Create(33, 1577, "Джин'рок Разрушитель", 207601212, 3, 10, 1),
            Boss.Create(34, 1577, "Джин'рок Разрушитель", 317507736, 5, 10, 1),
            Boss.Create(35, 1575, "Хорридон", 357632340, 3, 10, 1),
            Boss.Create(36, 1575, "Хорридон", 654205500, 5, 10, 1),
            Boss.Create(37, 1570, "Совет старейшин", 299538888, 3, 10, 1),
            Boss.Create(38, 1570, "Совет старейшин", 470330152, 5, 10, 1),
            Boss.Create(39, 1565, "Тортос", 179999841, 3, 10, 1),
            Boss.Create(40, 1565, "Тортос", 319999818, 5, 10, 1),
            Boss.Create(41, 1578, "Мегера", 263317712, 3, 10, 1),
            Boss.Create(42, 1578, "Мегера", 342297774, 5, 10, 1),
            Boss.Create(43, 1573, "Цзи-Кунь", 244236720, 3, 10, 1),
            Boss.Create(44, 1573, "Цзи-Кунь", 366355080, 5, 10, 1),
            Boss.Create(45, 1572, "Дуруму Позабытый", 261682200, 3, 10, 1),
            Boss.Create(46, 1572, "Дуруму Позабытый", 392523300, 5, 10, 1),
            Boss.Create(47, 1574, "Изначалий", 218068500, 3, 10, 1),
            Boss.Create(48, 1574, "Изначалий", 258193104, 5, 10, 1),
            Boss.Create(49, 1576, "Темный Анимус", 80999797, 3, 10, 1),
            Boss.Create(50, 1576, "Темный Анимус", 288000023, 5, 10, 1),
            Boss.Create(51, 1559, "Кон Железный", 119937675, 3, 10, 1),
            Boss.Create(52, 1559, "Кон Железный", 155700909, 5, 10, 1),
            Boss.Create(53, 1560, "Небесные сестры", 219812670, 3, 10, 1),
            Boss.Create(54, 1560, "Небесные сестры", 628036200, 5, 10, 1),
            Boss.Create(55, 1579, "Лэй Шэнь", 329283435, 3, 10, 1),
            Boss.Create(56, 1579, "Лэй Шэнь", 580498347, 5, 10, 1),

            #endregion

            #region Осада Огриммара

            Boss.Create(57, 1602, "Глубиний", 61900000, 3, 10, 2),
            Boss.Create(58, 1602, "Глубиний", 91500000, 5, 10, 2),
            Boss.Create(59, 1598, "Павшие защитники", 114000000, 3, 10, 1),
            Boss.Create(60, 1598, "Павшие защитники", 250000000, 5, 10, 1),
            Boss.Create(61, 1624, "Норусхен", 401000000, 3, 10, 3),
            Boss.Create(62, 1624, "Норусхен", 702000000, 5, 10, 3),
            Boss.Create(63, 1604, "Ша Гордыни", 426000000, 3, 10, 12),
            Boss.Create(64, 1604, "Ша Гордыни", 661000000, 5, 10, 12),
            Boss.Create(65, 1622, "Галакрас", 139000000, 3, 10, 4),
            Boss.Create(66, 1622, "Галакрас", 218000000, 5, 10, 4),
            Boss.Create(67, 1600, "Железный исполин", 451000000, 3, 10, 13),
            Boss.Create(68, 1600, "Железный исполин", 592000000, 5, 10, 13),
            Boss.Create(69, 1606, "Кор'кронские темные шаманы", 349000000, 3, 10, 5),
            Boss.Create(70, 1606, "Кор'кронские темные шаманы", 654000000, 5, 10, 5),
            Boss.Create(71, 1603, "Генерал Назгрим", 349000000, 3, 10, 6),
            Boss.Create(72, 1603, "Генерал Назгрим", 523000000, 5, 10, 6),
            Boss.Create(73, 1595, "Малкорок", 377000000, 3, 10, 7),
            Boss.Create(74, 1595, "Малкорок", 630000000, 5, 10, 7),
            Boss.Create(75, 1594, "Пандарийские трофеи", 621000000, 3, 10, 8),
            Boss.Create(76, 1594, "Пандарийские трофеи", 1190000000, 5, 10, 8),
            Boss.Create(77, 1599, "Ток Кровожадный", 445000000, 3, 10, 14),
            Boss.Create(78, 1599, "Ток Кровожадный", 654000000, 5, 10, 14),
            Boss.Create(79, 1601, "Мастер осады Черноплавс", 298000000, 3, 10, 9),
            Boss.Create(80, 1601, "Мастер осады Черноплавс", 500000000, 5, 10, 9),
            Boss.Create(81, 1593, "Идеалы клакси", 510000000, 3, 10, 10),
            Boss.Create(82, 1593, "Идеалы клакси", 1260000000, 5, 10, 10),
            Boss.Create(83, 1623, "Гаррош Адский Крик", 161000000, 3, 10, 11),
            Boss.Create(84, 1623, "Гаррош Адский Крик", 228000000, 5, 10, 11),

            #endregion
        ];

        return collection;
    }

    public static Specialization[] GenerateSpecializations()
    {
        Specialization[] collection =
        [
            Specialization.Create(1, "Affliction", "48181,30108,1120"),
            Specialization.Create(2, "Survival", "131900,3674,53301"),
            Specialization.Create(3, "Unholy", "55078,55090,47632"),
            Specialization.Create(4, "Balance", "50288,78674,8921"),
            Specialization.Create(5, "Shadow", "129197,2944,15407"),
            Specialization.Create(6, "Arms", "12294,86346,7384"),
            Specialization.Create(7, "ProtectionWarrior", "6572,23922,20243"),
            Specialization.Create(8, "Brewmaster", "121253,124335,100787"),
            Specialization.Create(9, "Discipline", "47750,81751,585"),
            Specialization.Create(10, "RestorationShaman", "61295,52752,51945"),
            Specialization.Create(11, "Combat", "57841,84617,1752"),
            Specialization.Create(12, "Subtlety", "53,2098,8676"),
            Specialization.Create(13, "Destruction", "29722,116858,348"),
            Specialization.Create(14, "HolyPaladin", "82327,85222,25914"),
            Specialization.Create(15, "ProtectionPaladin", "31935,53600,20271"),
            Specialization.Create(16, "Elemental", "51505,403,8050"),
            Specialization.Create(17, "Frost", "116,44614,30455"),
            Specialization.Create(18, "BeastMastery", "83381,121818,17253"),
            Specialization.Create(19, "RestorationDruid", "774,48438,81269"),
            Specialization.Create(20, "HolyPriest", "596,77489,139"),
        ];

        return collection;
    }

    public static CombatAbility[] GenerateCombatAbilities()
    {
        CombatAbility[] collection =
        [
            CombatAbility.Create(1, 105702, "Зелье Нефритовой Змеи", (int)CombatAbilityType.EfficiencyPotion),
            CombatAbility.Create(2, 105697, "Укус гну-синя", (int)CombatAbilityType.EfficiencyPotion),
            CombatAbility.Create(3, 105706, "Зелье силы могу", (int)CombatAbilityType.EfficiencyPotion),
            CombatAbility.Create(4, 125282, "Бодрящая кафа",(int) CombatAbilityType.EfficiencyPotion),
            CombatAbility.Create(5, 105696, "Настой кусачих морозов", (int)CombatAbilityType.Elixir),
            CombatAbility.Create(6, 105689, "Настой весенних цветов",(int) CombatAbilityType.Elixir),
            CombatAbility.Create(7, 105691, "Настой ласкового солнца",(int) CombatAbilityType.Elixir),
            CombatAbility.Create(8, 104277, "Сытость",(int) CombatAbilityType.Food),
            CombatAbility.Create(9, 80353, "Искажение времени",(int) CombatAbilityType.PartyEfficiency),
            CombatAbility.Create(10, 2825, "Жажда крови",(int) CombatAbilityType.PartyEfficiency),
            CombatAbility.Create(11, 114207, "Знамя с черепом",(int) CombatAbilityType.PartyEfficiency),
            CombatAbility.Create(12, 120676, "Тотем порыва бури",(int) CombatAbilityType.PartyEfficiency),
            CombatAbility.Create(13, 104272, "Сытость",(int) CombatAbilityType.Food),
            CombatAbility.Create(14, 61316, "Чародейская гениальность Даларана",(int) CombatAbilityType.PartyEfficiency),
            CombatAbility.Create(15, 1126, "Знак дикой природы",(int) CombatAbilityType.PartyEfficiency),
            CombatAbility.Create(16, 109773, "Узы Тьмы",(int) CombatAbilityType.PartyEfficiency),
            CombatAbility.Create(17, 116956, "Легкость воздуха",(int) CombatAbilityType.PartyEfficiency),
            CombatAbility.Create(18, 77747, "Пылающая ярость",(int) CombatAbilityType.PartyEfficiency),
            CombatAbility.Create(19, 113742, "Искусство быстрой битвы",(int) CombatAbilityType.PartyEfficiency),
            CombatAbility.Create(20, 19740, "Благословение могущества",(int) CombatAbilityType.PartyEfficiency),
            CombatAbility.Create(21, 135678, "Бодрящие споры",(int) CombatAbilityType.PartyEfficiency),
            CombatAbility.Create(22, 20217, "Благословение королей",(int) CombatAbilityType.PartyEfficiency),
            CombatAbility.Create(23, 25780, "Праведное неистовство",(int) CombatAbilityType.Other),
        ];

        return collection;
    }

    public static BestSpecializationScore[] GenerateBestSpecializationScores()
    {
        var bosses = GenerateBosses();
        var specs = GenerateSpecializations();
        var bestScores = new BestSpecializationScore[bosses.Length * specs.Length];
        var index = 0;

        foreach (var boss in bosses)
        {
            foreach (var spec in specs)
            {
                bestScores[index] = BestSpecializationScore.Create(index + 1, 0, 0, null, spec.Id, boss.Id);
                index++;
            }
        }

        return bestScores;
    }
}
