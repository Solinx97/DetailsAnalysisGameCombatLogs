using CombatAnalysis.CombatParser.Entities;
using CombatAnalysis.CombatParser.Patterns;
using Microsoft.Extensions.Logging;
using Moq;

namespace CombatAnalysis.Parser.Tests.CombatParser;

[TestFixture]
internal class CombatDetailsServiceTests
{
    [Test]
    public void CombatDetailsService_GetDamageDone_Must_Return_Correct_Value_By_Spell_Damage()
    {
        var data = "6/3 21:10:39.957  SPELL_DAMAGE,,\"Волако\",,,,\"Страж племени Амани\",0xa48,0x0,26862,\"Коварный удар\",,,,84842,86172,0,0,0,-1,0,0,0,131.84,1590.25,333,2.0246,71,1330,889,-1,1,0,0,0,1,nil,nil";
        var combat = new Combat
        {
            Name = "Test combat 3 der",
            Data = new List<string> { data },
            IsWin = true,
            StartDate = new DateTimeOffset(2022, 06, 05, 20, 15, 05, TimeSpan.Zero),
            FinishDate = new DateTimeOffset(2022, 06, 05, 20, 17, 58, TimeSpan.Zero),
        };

        var mockLogger = new Mock<ILogger>();
        var combatDetails = new CombatDetailsDamageDone(mockLogger.Object);
        var damageDone = combatDetails.GetData("Волако", combat.Data);

        var expectedDamageDone = 1330;

        Assert.AreEqual(expectedDamageDone, damageDone);
    }

    [Test]
    public void CombatDetailsService_GetDamageDone_Must_Return_Correct_Value_By_Swing_Damage()
    {
        var data = "6/3 21:10:39.957  SWING_DAMAGE,Player-4452-02DC6CBE,\"Воларн\",0x512,0x0,Creature-0-4460-568-19650-23597-00051A4C8C,\"Страж племени Амани\",0xa48,0x0,Player-4452-02DC6CBE,0000000000000000,100,100,2187,0,4932,3,60,100,0,129.71,1591.36,333,5.4208,141,446,726,-1,1,0,0,0,nil,nil,nil";
        var combat = new Combat
        {
            Name = "Test combat 3gtyh",
            Data = new List<string> { data },
            IsWin = true,
            StartDate = new DateTimeOffset(2022, 06, 05, 19, 15, 05, TimeSpan.Zero),
            FinishDate = new DateTimeOffset(2022, 06, 05, 19, 17, 58, TimeSpan.Zero),
        };

        var mockLogger = new Mock<ILogger>();
        var combatDetails = new CombatDetailsDamageDone(mockLogger.Object);
        var damageDone = combatDetails.GetData("Воларн", combat.Data);

        var expectedDamageDone = 446;

        Assert.AreEqual(expectedDamageDone, damageDone);
    }

    [Test]
    public void CombatDetailsService_GetDamageDone_Must_Return_Correct_Value_By_Spell_Periodic_Damage()
    {
        var data = "6/3 21:10:40.906  SPELL_PERIODIC_DAMAGE,Player-4452-0352CBDA,\"Протоп\",0x40512,0x0,Creature-0-4460-568-19650-23597-00081A4C8C,\"Страж племени Амани\",0xa48,0x0,27173,\"Освящение\",0x2,Creature-0-4460-568-19650-23597-00081A4C8C,0000000000000000,82440,86172,0,0,0,-1,0,0,0,127.13,1573.69,333,5.6290,71,178,169,-1,2,0,0,0,nil,nil,nil";
        var combat = new Combat
        {
            Name = "Test combatiodf",
            Data = new List<string> { data },
            IsWin = true,
            StartDate = new DateTimeOffset(2022, 06, 05, 22, 15, 05, TimeSpan.Zero),
            FinishDate = new DateTimeOffset(2022, 06, 05, 22, 17, 58, TimeSpan.Zero),
        };

        var mockLogger = new Mock<ILogger>();
        var combatDetails = new CombatDetailsDamageDone(mockLogger.Object);
        var damageDone = combatDetails.GetData("Протоп", combat.Data);

        var expectedDamageDone = 178;

        Assert.AreEqual(expectedDamageDone, damageDone);
    }

    [Test]
    public void CombatDetailsService_GetDamageDone_Must_Return_Correct_Value_By_Swing_Missed()
    {
        var data = "6/3 21:10:45.810  SWING_MISSED,Player-4452-01067734,\"Коян\",0x40512,0x0,Creature-0-4460-568-19650-23597-00051A4C8C,\"Страж племени Амани\",0xa48,0x0,DODGE,nil";
        var combat = new Combat
        {
            Name = "Test combat 76 yued 9",
            Data = new List<string> { data },
            IsWin = true,
            StartDate = new DateTimeOffset(2022, 06, 05, 20, 15, 11, TimeSpan.Zero),
            FinishDate = new DateTimeOffset(2022, 06, 05, 20, 17, 25, TimeSpan.Zero),
        };

        var mockLogger = new Mock<ILogger>();
        var combatDetails = new CombatDetailsDamageDone(mockLogger.Object);
        var damageDone = combatDetails.GetData("Коян", combat.Data);

        var expectedDamageDone = 0;

        Assert.AreEqual(expectedDamageDone, damageDone);
    }

    [Test]
    public void CombatDetailsService_GetDamageDone_Must_Return_Correct_Value_By_Damage_Shield_Missed()
    {
        var data = "6/3 21:15:07.864  DAMAGE_SHIELD_MISSED,Player-4452-01067734,\"Форит\",0x40512,0x0,Creature-0-4460-568-19650-23574-00001A4C8C,\"Акил'зон\",0xa48,0x0,26992,\"Шипы\",0x8,RESIST,nil,0";
        var combat = new Combat
        {
            Name = "Test combat 1",
            Data = new List<string> { data },
            IsWin = true,
            StartDate = new DateTimeOffset(2022, 06, 05, 20, 15, 05, TimeSpan.Zero),
            FinishDate = new DateTimeOffset(2022, 06, 05, 20, 17, 58, TimeSpan.Zero),
        };

        var mockLogger = new Mock<ILogger>();
        var combatDetails = new CombatDetailsDamageDone(mockLogger.Object);
        var damageDone = combatDetails.GetData("Форит", combat.Data);

        var expectedDamageDone = 0;

        Assert.AreEqual(expectedDamageDone, damageDone);
    }

    [Test]
    public void CombatDetailsService_GetDamageDone_Must_Return_Correct_Value_By_Range_Damage()
    {
        var data = "6/3 21:15:11.180  RANGE_DAMAGE,Player-4452-03684D5F,\"Хунтер\",0x512,0x0,Creature-0-4460-568-19650-23574-00001A4C8C,\"Акил'зон\",0xa48,0x0,75,\"Автоматическая стрельба\",0x1,Creature-0-4460-568-19650-23574-00001A4C8C,0000000000000000,1329796,1335400,0,0,0,-1,0,0,0,374.16,1409.14,333,6.2717,73,1665,1076,-1,1,0,0,0,1,nil,nil";
        var combat = new Combat
        {
            Name = "Test comdsafw23d",
            Data = new List<string> { data },
            IsWin = true,
            StartDate = new DateTimeOffset(2022, 06, 05, 20, 45, 05, TimeSpan.Zero),
            FinishDate = new DateTimeOffset(2022, 06, 05, 20, 46, 58, TimeSpan.Zero),
        };

        var mockLogger = new Mock<ILogger>();
        var combatDetails = new CombatDetailsDamageDone(mockLogger.Object);
        var damageDone = combatDetails.GetData("Хунтер", combat.Data);

        var expectedDamageDone = 1665;

        Assert.AreEqual(expectedDamageDone, damageDone);
    }

    [Test]
    public void CombatDetailsService_GetDamageDone_Must_Return_Correct_Value_By_Spell_Missed()
    {
        var data = "6/3 21:15:17.799  SPELL_MISSED,Player-4452-01067734,\"Волибир\",0x40512,0x0,Creature-0-4460-568-19650-23574-00001A4C8C,\"Акил'зон\",0xa48,0x0,26996,\"Трепка\",0x1,PARRY,nil";
        var combat = new Combat
        {
            Name = "Test combat 33345d",
            Data = new List<string> { data },
            IsWin = true,
            StartDate = new DateTimeOffset(2022, 06, 05, 11, 15, 05, TimeSpan.Zero),
            FinishDate = new DateTimeOffset(2022, 06, 05, 11, 17, 58, TimeSpan.Zero),
        };

        var mockLogger = new Mock<ILogger>();
        var combatDetails = new CombatDetailsDamageDone(mockLogger.Object);
        var damageDone = combatDetails.GetData("Волибир", combat.Data);

        var expectedDamageDone = 0;

        Assert.AreEqual(expectedDamageDone, damageDone);
    }

    [Test]
    public void CombatDetailsService_GetHealDone_Must_Return_Correct_Value_By_Spell_Heal()
    {
        var data = "6/3 21:07:10.139  SPELL_HEAL,Player-4452-033980F6,\"Шопи\",0x511,0x0,Player-4452-034E2A70,\"Азула - Хроми\",0x514,0x0,25423,\"Цепное исцеление\",0x8,Player-4452-034E2A70,0000000000000000,100,100,55,966,1854,0,9234,10616,0,115.89,1662.72,333,4.4495,120,1405,1405,544,0,1";
        var combat = new Combat
        {
            Name = "Test combat2",
            Data = new List<string> { data },
            IsWin = true,
            StartDate = new DateTimeOffset(2022, 06, 05, 23, 15, 05, TimeSpan.Zero),
            FinishDate = new DateTimeOffset(2022, 06, 05, 23, 17, 58, TimeSpan.Zero),
        };

        var mockLogger = new Mock<ILogger>();
        var combatDetails = new CombatDetailsHealDone(mockLogger.Object);
        var healDone = combatDetails.GetData("Шопи", combat.Data);

        var expectedHealDone = 861;

        Assert.AreEqual(expectedHealDone, healDone);
    }

    [Test]
    public void CombatDetailsService_GetHealDone_Must_Return_Correct_Value_By_Spell_Periodic_Heal()
    {
        var data = "6/3 21:16:35.127  SPELL_PERIODIC_HEAL,Player-4452-02FE19CD,\"Дру\",0x512,0x0,Player-4452-01067734,\"Волибир-Хроми\",0x40512,0x0,26982,\"Омоложение\",0x8,Player-4452-01067734,0000000000000000,100,100,2242,0,35387,1,698,1000,0,379.39,1408.13,333,3.4371,128,1034,1034,218,0,nil";
        var combat = new Combat
        {
            Name = "Test combat2",
            Data = new List<string> { data },
            IsWin = true,
            StartDate = new DateTimeOffset(2022, 06, 05, 23, 15, 05, TimeSpan.Zero),
            FinishDate = new DateTimeOffset(2022, 06, 05, 23, 17, 58, TimeSpan.Zero),
        };

        var mockLogger = new Mock<ILogger>();
        var combatDetails = new CombatDetailsHealDone(mockLogger.Object);
        var healDone = combatDetails.GetData("Дру", combat.Data);

        var expectedHealDone = 816;

        Assert.AreEqual(expectedHealDone, healDone);
    }

    [Test]
    public void CombatDetailsService_GetDamageTaken_Must_Return_Correct_Value_By_Spell_Damage()
    {
        var data = "6/3 21:10:49.399  SPELL_DAMAGE,Creature-0-4460-568-19650-23889-00019A4C8C,\"Варвар из племени Амани\",0xa48,0x0,Player-4452-0352CBDA,\"Протоппе\",0x40512,0x0,9080,\"Подрезать сухожилия\",0x1,Player-4452-0352CBDA,0000000000000000,99,100,472,722,24548,0,6945,8118,0,122.71,1574.54,333,0.6668,140,19,66,-1,1,0,0,0,nil,nil,nil";
        var combat = new Combat
        {
            Name = "Test combat234 rtfed d",
            Data = new List<string> { data },
            IsWin = true,
            StartDate = new DateTimeOffset(2022, 06, 07, 20, 15, 05, TimeSpan.Zero),
            FinishDate = new DateTimeOffset(2022, 06, 07, 20, 17, 58, TimeSpan.Zero),
        };

        var mockLogger = new Mock<ILogger>();
        var combatDetails = new CombatDetailsDamageTaken(mockLogger.Object);
        var damageTaken = combatDetails.GetData("Протоппе", combat.Data);

        var expectedDamageTaken = 19;

        Assert.AreEqual(expectedDamageTaken, damageTaken);
    }

    [Test]
    public void CombatDetailsService_GetDamageTaken_Must_Return_Correct_Value_By_Swing_Damage()
    {
        var data = "6/3 21:16:43.880  SWING_DAMAGE,Creature-0-4460-568-19650-23574-00001A4C8C,\"Акил'зон\",0xa48,0x0,Player-4452-01067734,\"Волирнв\",0x40512,0x0,Creature-0-4460-568-19650-23574-00001A4C8C,0000000000000000,527051,1335400,0,0,0,-1,0,0,0,377.13,1407.70,333,1.3817,73,2114,7132,-1,1,0,0,0,nil,nil,nil";
        var combat = new Combat
        {
            Name = "Test combat 3gtyh",
            Data = new List<string> { data },
            IsWin = true,
            StartDate = new DateTimeOffset(2022, 06, 05, 19, 15, 05, TimeSpan.Zero),
            FinishDate = new DateTimeOffset(2022, 06, 05, 19, 17, 58, TimeSpan.Zero),
        };

        var mockLogger = new Mock<ILogger>();
        var combatDetails = new CombatDetailsDamageTaken(mockLogger.Object);
        var damageTaken = combatDetails.GetData("Волирнв", combat.Data);

        var expectedDamageTaken = 2114;

        Assert.AreEqual(expectedDamageTaken, damageTaken);
    }

    [Test]
    public void CombatDetailsService_GetDamageTaken_Must_Return_Correct_Value_By_Spell_Periodic_Damage()
    {
        var data = "6/3 21:26:06.260  SPELL_PERIODIC_DAMAGE,Creature-0-4460-568-19650-23576-00001A4C8C,\"Налоракк\",0x10a48,0x0,Player-4452-0352CBDA,\"Протопвр\",0x40514,0x0,42395,\"Терзающая рана\",0x1,Player-4452-0352CBDA,0000000000000000,78,100,644,722,18936,0,5077,8118,0,-78.11,1315.71,333,0.9519,140,1624,1727,-1,1,0,0,0,nil,nil,nil";
        var combat = new Combat
        {
            Name = "Test combatiodf",
            Data = new List<string> { data },
            IsWin = true,
            StartDate = new DateTimeOffset(2022, 06, 05, 22, 15, 05, TimeSpan.Zero),
            FinishDate = new DateTimeOffset(2022, 06, 05, 22, 17, 58, TimeSpan.Zero),
        };

        var mockLogger = new Mock<ILogger>();
        var combatDetails = new CombatDetailsDamageTaken(mockLogger.Object);
        var damageTaken = combatDetails.GetData("Протопвр", combat.Data);

        var expectedDamageTaken = 1624;

        Assert.AreEqual(expectedDamageTaken, damageTaken);
    }

    [Test]
    public void CombatDetailsService_GetDamageTaken_Must_Return_Correct_Value_By_Swing_Missed()
    {
        var data = "6/3 21:26:20.179  SWING_MISSED,Creature-0-4460-568-19650-23576-00001A4C8C,\"Налоракк\",0x10a48,0x0,Player-4452-0352CBDA,\"Протопен\",0x40514,0x0,PARRY,nil";
        var combat = new Combat
        {
            Name = "Test combat 76 yued 9",
            Data = new List<string> { data },
            IsWin = true,
            StartDate = new DateTimeOffset(2022, 06, 05, 20, 15, 11, TimeSpan.Zero),
            FinishDate = new DateTimeOffset(2022, 06, 05, 20, 17, 25, TimeSpan.Zero),
        };

        var mockLogger = new Mock<ILogger>();
        var combatDetails = new CombatDetailsDamageTaken(mockLogger.Object);
        var damageTaken = combatDetails.GetData("Протопен", combat.Data);

        var expectedDamageTaken = 0;

        Assert.AreEqual(expectedDamageTaken, damageTaken);
    }

    [Test]
    public void CombatDetailsService_GetDamageTaken_Must_Return_Correct_Value_By_Damage_Shield_Missed()
    {
        var data = "6/3 21:15:07.864  DAMAGE_SHIELD_MISSED,Player-4452-01067734,\"Форит\",0x40512,0x0,Creature-0-4460-568-19650-23574-00001A4C8C,\"Акил'зон\",0xa48,0x0,26992,\"Шипы\",0x8,RESIST,nil,0";
        var combat = new Combat
        {
            Name = "Test combat 1",
            Data = new List<string> { data },
            IsWin = true,
            StartDate = new DateTimeOffset(2022, 06, 05, 20, 15, 05, TimeSpan.Zero),
            FinishDate = new DateTimeOffset(2022, 06, 05, 20, 17, 58, TimeSpan.Zero),
        };

        var mockLogger = new Mock<ILogger>();
        var combatDetails = new CombatDetailsDamageTaken(mockLogger.Object);
        var damageTaken = combatDetails.GetData("Форит", combat.Data);

        var expectedDamageTaken = 0;

        Assert.AreEqual(expectedDamageTaken, damageTaken);
    }

    [Test]
    public void CombatDetailsService_GetDamageTaken_Must_Return_Correct_Value_By_Range_Damage()
    {
        var data = "6/3 21:15:11.180  RANGE_DAMAGE,Player-4452-03684D5F,\"Хунтер\",0x512,0x0,Creature-0-4460-568-19650-23574-00001A4C8C,\"Акил'зон\",0xa48,0x0,75,\"Автоматическая стрельба\",0x1,Creature-0-4460-568-19650-23574-00001A4C8C,0000000000000000,1329796,1335400,0,0,0,-1,0,0,0,374.16,1409.14,333,6.2717,73,1665,1076,-1,1,0,0,0,1,nil,nil";
        var combat = new Combat
        {
            Name = "Test comdsafw23d",
            Data = new List<string> { data },
            IsWin = true,
            StartDate = new DateTimeOffset(2022, 06, 05, 20, 45, 05, TimeSpan.Zero),
            FinishDate = new DateTimeOffset(2022, 06, 05, 20, 46, 58, TimeSpan.Zero),
        };

        var mockLogger = new Mock<ILogger>();
        var combatDetails = new CombatDetailsDamageTaken(mockLogger.Object);
        var damageDone = combatDetails.GetData("Хунтер", combat.Data);

        var expectedDamageDone = 0;

        Assert.AreEqual(expectedDamageDone, damageDone);
    }

    [Test]
    public void CombatDetailsService_GetDamageTaken_Must_Return_Correct_Value_By_Spell_Missed()
    {
        var data = "6/3 21:27:05.773  SPELL_MISSED,Creature-0-4460-568-19650-23576-00001A4C8C,\"Налоракк\",0x10a48,0x0,Player-4452-01067734,\"Волибир\",0x40512,0x0,42389,\"Увечье\",0x1,DODGE,nil";
        var combat = new Combat
        {
            Name = "Test combat 33345d",
            Data = new List<string> { data },
            IsWin = true,
            StartDate = new DateTimeOffset(2022, 06, 05, 11, 15, 05, TimeSpan.Zero),
            FinishDate = new DateTimeOffset(2022, 06, 05, 11, 17, 58, TimeSpan.Zero),
        };

        var mockLogger = new Mock<ILogger>();
        var combatDetails = new CombatDetailsDamageTaken(mockLogger.Object);
        var damageTaken = combatDetails.GetData("Волибир", combat.Data);

        var expectedDamageTaken = 0;

        Assert.AreEqual(expectedDamageTaken, damageTaken);
    }

    [Test]
    public void CombatDetailsService_GetResourceRecovery_Must_Return_Correct_Value_By_Spell_Periodic_Energize()
    {
        var data = "6/3 21:10:49.499  SPELL_PERIODIC_ENERGIZE,Player-4452-0352CBDA,\"Пратоапуы\",0x40512,0x0,Player-4452-0352CBDA,\"Протопалище - Хроми\",0x40512,0x0,31786,\"Духовное созвучие\",0x1,Player-4452-0352CBDA,0000000000000000,100,100,472,722,24548,0,6973,8118,0,122.39,1574.29,333,0.6668,140,28.0000,0.0000,0,8118";
        var combat = new Combat
        {
            Name = "Test combat2111234",
            Data = new List<string> { data },
            IsWin = true,
            StartDate = new DateTimeOffset(2022, 06, 05, 16, 22, 05, TimeSpan.Zero),
            FinishDate = new DateTimeOffset(2022, 06, 05, 16, 24, 58, TimeSpan.Zero),
        };

        var mockLogger = new Mock<ILogger>();
        var combatDetails = new CombatDetailsResourceRecovery(mockLogger.Object);
        var resourceRecovery = combatDetails.GetData("Пратоапуы", combat.Data);

        var expectedResourceRecovery = 28;

        Assert.AreEqual(expectedResourceRecovery, resourceRecovery);
    }

    [Test]
    public void CombatDetailsService_GetDeathsNumber_Must_Return_Correct_Value_By_Unit_Died()
    {
        var data = "6/3 21:16:34.927  UNIT_DIED,0000000000000000,nil,0x80000000,0x80000000,Player-4452-03684D5F,\"Никуапа\",0x512,0x0";
        var combatPlayerData = new CombatPlayer
        {
            Username = "Никуапа",
            DamageDone = 1,
            DamageTaken = 15,
            HealDone = 44,
            EnergyRecovery = 55,
            UsedBuffs = 3,
        };
        var combat = new Combat
        {
            Name = "Test combat2111234 657 hyuj",
            Data = new List<string> { data },
            IsWin = true,
            StartDate = new DateTimeOffset(2022, 06, 05, 16, 22, 05, TimeSpan.Zero),
            FinishDate = new DateTimeOffset(2022, 06, 05, 16, 24, 58, TimeSpan.Zero),
            Players = new List<CombatPlayer> { combatPlayerData }
        };

        var mockLogger = new Mock<ILogger>();
        var combatDetails = new CombatDetailsDeaths(mockLogger.Object, combat.Players);
        var deathNumber = combatDetails.GetData("Никуапа", combat.Data);

        var expectedDeathNumber = 1;

        Assert.AreEqual(expectedDeathNumber, deathNumber);
    }
}
