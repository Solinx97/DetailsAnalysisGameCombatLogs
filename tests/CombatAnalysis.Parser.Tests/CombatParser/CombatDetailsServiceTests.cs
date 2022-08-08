using CombatAnalysis.CombatParser.Entities;
using CombatAnalysis.CombatParser.Interfaces;
using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CombatAnalysis.Parser.Tests.CombatParser
{
    [TestFixture]
    internal class CombatDetailsServiceTests
    {
        [Test]
        public async Task CombatDetailsService_Parse_Must_Fill_Combats_Collection()
        {
            var testCombatLog = string.Empty;

            var combatPlayerData = new CombatPlayerData
            {
                UserName = "Oleg",
                DamageDone = 10,
                DamageTaken = 15,
                HealDone = 20,
                EnergyRecovery = 25,
                UsedBuffs = 2,
            };

            var combat = new Combat
            {
                Name = "Test",
                Data = new List<string> { "data" },
                IsWin = true,
                StartDate = DateTimeOffset.Now,
                FinishDate = DateTimeOffset.Now.AddHours(1),
                Players = new List<CombatPlayerData> { combatPlayerData },
            };

            var mockParser = new Mock<IParser>();
            mockParser.Setup(x => x.Parse(testCombatLog)).Verifiable();
            mockParser.Setup(x => x.Combats).Returns(new List<Combat> { combat });

            var mainInformationViewModelStub = new MainInformationViewModelStub(mockParser.Object);
            var combats = await mainInformationViewModelStub.GetCombatDataDetailsAsync(testCombatLog);

            Assert.IsNotEmpty(combats);

            var expectedCombat = combats[0];
            Assert.AreEqual(expectedCombat, combat, "Combat has not expected elements.");

            var expectedCombatHealDone = combats[0].HealDone;
            var expectedCombatDamageDone = combats[0].DamageDone;
            var expectedCombatDamageTaken = combats[0].DamageTaken;
            var expectedCombatEnergyRecovery = combats[0].EnergyRecovery;
            Assert.AreEqual(expectedCombatHealDone, combatPlayerData.HealDone, "Combat heald one has not expected elements.");
            Assert.AreEqual(expectedCombatDamageDone, combatPlayerData.DamageDone, "Combat damage done has not expected elements.");
            Assert.AreEqual(expectedCombatDamageTaken, combatPlayerData.DamageTaken, "Combat damage taken has not expected elements.");
            Assert.AreEqual(expectedCombatEnergyRecovery, combatPlayerData.EnergyRecovery, "Combat energy recovery has not expected elements.");

            Assert.IsNotEmpty(combats[0].Players);

            var expectedCombatPlayerData = combats[0].Players[0];
            Assert.AreEqual(expectedCombatPlayerData, combatPlayerData, "Combat players has not expected elements.");

            var expectedCombatPlayerDataHealDone = expectedCombatPlayerData.HealDone;
            var expectedCombatPlayerDataDamageDone = expectedCombatPlayerData.DamageDone;
            var expectedCombatPlayerDataDamageTaken = expectedCombatPlayerData.DamageTaken;
            var expectedCombatPlayerDataEnergyRecovery = expectedCombatPlayerData.EnergyRecovery;
            Assert.AreEqual(expectedCombatPlayerDataHealDone, combatPlayerData.HealDone, "Combat heald one has not expected elements.");
            Assert.AreEqual(expectedCombatPlayerDataDamageDone, combatPlayerData.DamageDone, "Combat damage done has not expected elements.");
            Assert.AreEqual(expectedCombatPlayerDataDamageTaken, combatPlayerData.DamageTaken, "Combat damage taken has not expected elements.");
            Assert.AreEqual(expectedCombatPlayerDataEnergyRecovery, combatPlayerData.EnergyRecovery, "Combat energy recovery has not expected elements.");
        }
    }
}
