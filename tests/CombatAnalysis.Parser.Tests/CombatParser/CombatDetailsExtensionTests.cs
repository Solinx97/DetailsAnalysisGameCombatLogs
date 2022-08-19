using CombatAnalysis.CombatParser.Entities;
using CombatAnalysis.CombatParser.Extensions;
using CombatAnalysis.CombatParser.Services;
using Microsoft.Extensions.Logging;
using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;

namespace CombatAnalysis.Parser.Tests.CombatParser
{
    [TestFixture]
    internal class CombatDetailsExtensionTests
    {
        [Test]
        public void CombatDetailsExtension_GetDamageDoneGeneral_Must_Return_Damage_Done_General_Collection()
        {
            var damageDone = new DamageDone
            {
                FromPlayer = "Test player",
                ToEnemy = "Test enemy",
                IsCrit = true,
                SpellOrItem = "TestSpell",
                Value = 2604,
                Time = TimeSpan.Zero
            };
            var damageDone1 = new DamageDone
            {
                FromPlayer = "Test player1",
                ToEnemy = "Test enemy1",
                IsDodge = true,
                SpellOrItem = "TestSpell",
                Value = 3354,
                Time = TimeSpan.Zero
            };
            var damageDone2 = new DamageDone
            {
                FromPlayer = "Test player2",
                ToEnemy = "Test enemy2",
                IsCrit = true,
                IsMiss = true,
                SpellOrItem = "TestSpell",
                Value = 1622,
                Time = TimeSpan.Zero
            };

            var damageDones = new List<DamageDone> { damageDone, damageDone1, damageDone2 };

            var combat = new Combat
            {
                Name = "Test combat 665r",
                StartDate = new DateTimeOffset(2022, 06, 05, 20, 15, 05, TimeSpan.Zero),
                FinishDate = new DateTimeOffset(2022, 06, 05, 20, 17, 58, TimeSpan.Zero),
            };

            var mockLogger = new Mock<ILogger>();
            var combatDetails = new CombatDetailsService(mockLogger.Object);
            var damageDoneGenerals = combatDetails.GetDamageDoneGeneral(damageDones, combat);
            Assert.IsNotNull(damageDoneGenerals);

            var firstDamageDoneGeneral = damageDoneGenerals[0];

            var expectedDamageDoneGeneral = new DamageDoneGeneral
            {
                Value = 7580,
                AverageValue = 2526.7,
                CastNumber = 3,
                DamagePerSecond = 43.8,
                MaxValue = 3354,
                MinValue = 1622,
                SpellOrItem = "TestSpell"
            };
            Assert.AreEqual(expectedDamageDoneGeneral.Value, firstDamageDoneGeneral.Value, "Damage done general value has not expected elements.");
            Assert.AreEqual(expectedDamageDoneGeneral.AverageValue, Math.Round(firstDamageDoneGeneral.AverageValue, 1), "Damage done general average value has not expected elements.");
            Assert.AreEqual(expectedDamageDoneGeneral.CastNumber, firstDamageDoneGeneral.CastNumber, "Damage done general cast number has not expected elements.");
            Assert.AreEqual(expectedDamageDoneGeneral.DamagePerSecond, Math.Round(firstDamageDoneGeneral.DamagePerSecond, 1), "Damage done general damage per second has not expected elements.");
            Assert.AreEqual(expectedDamageDoneGeneral.MaxValue, firstDamageDoneGeneral.MaxValue, "Damage done general max value has not expected elements.");
            Assert.AreEqual(expectedDamageDoneGeneral.MinValue, firstDamageDoneGeneral.MinValue, "Damage done general min value has not expected elements.");
            Assert.AreEqual(expectedDamageDoneGeneral.SpellOrItem, firstDamageDoneGeneral.SpellOrItem, "Damage done general spell or item has not expected elements.");
        }
    }
}
