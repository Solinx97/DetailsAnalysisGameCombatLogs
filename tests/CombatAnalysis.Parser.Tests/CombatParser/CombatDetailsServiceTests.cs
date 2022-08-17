using CombatAnalysis.CombatParser.Entities;
using CombatAnalysis.CombatParser.Services;
using Microsoft.Extensions.Logging;
using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;

namespace CombatAnalysis.Parser.Tests.CombatParser
{
    [TestFixture]
    internal class CombatDetailsServiceTests
    {
        [Test]
        public void CombatDetailsService_GetDamageDone_Must_Return_Correct_Value_By_Spell_Damage()
        {
            var data = "6/3 21:10:39.957  SPELL_DAMAGE,,\"Волако\",,,,\"Страж племени Амани\",0xa48,0x0,26862,\"Коварный удар\",,,,84842,86172,0,0,0,-1,0,0,0,131.84,1590.25,333,2.0246,71,1330,889,-1,1,0,0,0,1,nil,nil";
            var combat = new Combat
            {
                Name = "Test combat",
                Data = new List<string> { data },
                IsWin = true,
                StartDate = new DateTimeOffset(2022, 06, 05, 20, 15, 05, TimeSpan.Zero),
                FinishDate = new DateTimeOffset(2022, 06, 05, 20, 17, 58, TimeSpan.Zero),
            };

            var mockLogger = new Mock<ILogger>();
            var combatDetails = new CombatDetailsService(mockLogger.Object);
            combatDetails.Initialization(combat, "Волако");
            var damageDone = combatDetails.GetDamageDone();

            var expectedDamageDone = 1330;

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
                StartDate = new DateTimeOffset(2022, 06, 05, 20, 15, 05, TimeSpan.Zero),
                FinishDate = new DateTimeOffset(2022, 06, 05, 20, 17, 58, TimeSpan.Zero),
            };

            var mockLogger = new Mock<ILogger>();
            var combatDetails = new CombatDetailsService(mockLogger.Object);
            combatDetails.Initialization(combat, "Шопи");
            var healDone = combatDetails.GetHealDone();

            var expectedHealDone = 861;

            Assert.AreEqual(expectedHealDone, healDone);
        }

        [Test]
        public void CombatDetailsService_GetDamageTaken_Must_Return_Correct_Value_By_Spell_Damage()
        {
            var data = "6/3 21:10:49.399  SPELL_DAMAGE,Creature-0-4460-568-19650-23889-00019A4C8C,\"Варвар из племени Амани\",0xa48,0x0,Player-4452-0352CBDA,\"Протоппе\",0x40512,0x0,9080,\"Подрезать сухожилия\",0x1,Player-4452-0352CBDA,0000000000000000,99,100,472,722,24548,0,6945,8118,0,122.71,1574.54,333,0.6668,140,19,66,-1,1,0,0,0,nil,nil,nil";
            var combat = new Combat
            {
                Name = "Test combat2",
                Data = new List<string> { data },
                IsWin = true,
                StartDate = new DateTimeOffset(2022, 06, 05, 20, 15, 05, TimeSpan.Zero),
                FinishDate = new DateTimeOffset(2022, 06, 05, 20, 17, 58, TimeSpan.Zero),
            };

            var mockLogger = new Mock<ILogger>();
            var combatDetails = new CombatDetailsService(mockLogger.Object);
            combatDetails.Initialization(combat, "Протоппе");
            var damageTaken = combatDetails.GetDamageTaken();

            var expectedDamageTaken = 19;

            Assert.AreEqual(expectedDamageTaken, damageTaken);
        }

        [Test]
        public void CombatDetailsService_ResourceRecovery_Must_Return_Correct_Value_By_Spell_Periodic_Energize()
        {
            var data = "6/3 21:10:49.499  SPELL_PERIODIC_ENERGIZE,Player-4452-0352CBDA,\"Пратоапуы\",0x40512,0x0,Player-4452-0352CBDA,\"Протопалище - Хроми\",0x40512,0x0,31786,\"Духовное созвучие\",0x1,Player-4452-0352CBDA,0000000000000000,100,100,472,722,24548,0,6973,8118,0,122.39,1574.29,333,0.6668,140,28.0000,0.0000,0,8118";
            var combat = new Combat
            {
                Name = "Test combat2",
                Data = new List<string> { data },
                IsWin = true,
                StartDate = new DateTimeOffset(2022, 06, 05, 20, 15, 05, TimeSpan.Zero),
                FinishDate = new DateTimeOffset(2022, 06, 05, 20, 17, 58, TimeSpan.Zero),
            };

            var mockLogger = new Mock<ILogger>();
            var combatDetails = new CombatDetailsService(mockLogger.Object);
            combatDetails.Initialization(combat, "Пратоапуы");
            var resourceRecovery = combatDetails.GetResourceRecovery();

            var expectedResourceRecovery = 28;

            Assert.AreEqual(expectedResourceRecovery, resourceRecovery);
        }
    }
}
