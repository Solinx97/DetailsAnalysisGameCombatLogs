using CombatAnalysis.CombatParser.Entities;
using CombatAnalysis.CombatParser.Interfaces;
using CombatAnalysis.CombatParser.Services;
using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace CombatAnalysis.Parser.Tests.CombatParser
{
    [TestFixture]
    internal class CombatParserServiceTests
    {
        [Test]
        public async Task CombatParserService_FileCheck_Must_Return_True()
        {
            var testCombatLog = "combatLog.txt";

            var mockCombatDetails = new Mock<ICombatDetails>();
            mockCombatDetails.Setup(x => x.GetDamageDone()).Returns(10);
            mockCombatDetails.Setup(x => x.GetHealDone()).Returns(20);
            mockCombatDetails.Setup(x => x.GetDamageTaken()).Returns(15);
            mockCombatDetails.Setup(x => x.GetResourceRecovery()).Returns(25);
            mockCombatDetails.Setup(x => x.GetDeathsNumber()).Returns(2);

            var mockFileManager = new Mock<IFileManager>();

            var correctCombatLogFile = "6/8 20:42:39.739  COMBAT_LOG_VERSION,9,ADVANCED_LOG_ENABLED,1,BUILD_VERSION,2.5.4,PROJECT_ID,5";
            byte[] fakeFileBytes = Encoding.UTF8.GetBytes(correctCombatLogFile);
            var fakeMemoryStream = new MemoryStream(fakeFileBytes);

            mockFileManager.Setup(fileManager => fileManager.StreamReader(testCombatLog))
                           .Returns(() => new StreamReader(fakeMemoryStream));

            var parser = new CombatParserService(mockCombatDetails.Object, mockFileManager.Object);
            var fileIsCorrect = await parser.FileCheck(testCombatLog);

            Assert.IsTrue(fileIsCorrect);
        }

        [Test]
        public async Task CombatParserService_FileCheck_Must_Return_False()
        {
            var testCombatLog = "combatLog.txt";

            var mockCombatDetails = new Mock<ICombatDetails>();
            mockCombatDetails.Setup(x => x.GetDamageDone()).Returns(10);
            mockCombatDetails.Setup(x => x.GetHealDone()).Returns(20);
            mockCombatDetails.Setup(x => x.GetDamageTaken()).Returns(15);
            mockCombatDetails.Setup(x => x.GetResourceRecovery()).Returns(25);
            mockCombatDetails.Setup(x => x.GetDeathsNumber()).Returns(2);

            var mockFileManager = new Mock<IFileManager>();

            var correctCombatLogFile = "6/8 20:42:39.739  COMBAT_LOG_,9,ADVANCED_LOG_ENABLED,1,BUILD_VERSION,2.5.4,PROJECT_ID,5";
            byte[] fakeFileBytes = Encoding.UTF8.GetBytes(correctCombatLogFile);
            var fakeMemoryStream = new MemoryStream(fakeFileBytes);

            mockFileManager.Setup(fileManager => fileManager.StreamReader(testCombatLog))
                           .Returns(() => new StreamReader(fakeMemoryStream));

            var parser = new CombatParserService(mockCombatDetails.Object, mockFileManager.Object);
            var fileIsCorrect = await parser.FileCheck(testCombatLog);

            Assert.IsFalse(fileIsCorrect);
        }

        [Test]
        public async Task CombatParserService_Parse_Must_Fill_Combats_Collection()
        {
            var testCombatLog = "combatLog.txt";

            var combatPlayerData = new CombatPlayerData
            {
                UserName = "Oleg - Chrome",
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
                StartDate = new DateTimeOffset(2022, 06, 03, 21, 15, 05, TimeSpan.Zero),
                FinishDate = new DateTimeOffset(2022, 06, 03, 21, 17, 58, TimeSpan.Zero),
                Players = new List<CombatPlayerData> { combatPlayerData },
            };

            var mockCombatDetails = new Mock<ICombatDetails>();
            mockCombatDetails.Setup(x => x.GetDamageDone()).Returns(10);
            mockCombatDetails.Setup(x => x.GetHealDone()).Returns(20);
            mockCombatDetails.Setup(x => x.GetDamageTaken()).Returns(15);
            mockCombatDetails.Setup(x => x.GetResourceRecovery()).Returns(25);
            mockCombatDetails.Setup(x => x.GetDeathsNumber()).Returns(2);

            var mockFileManager = new Mock<IFileManager>();

            var fakeFileContents = CreateCorrectlyCombatLog();
            byte[] fakeFileBytes = Encoding.UTF8.GetBytes(fakeFileContents);
            var fakeMemoryStream = new MemoryStream(fakeFileBytes);

            mockFileManager.Setup(fileManager => fileManager.StreamReader(testCombatLog))
                           .Returns(() => new StreamReader(fakeMemoryStream));

            var parser = new CombatParserService(mockCombatDetails.Object, mockFileManager.Object);
            await parser.Parse(testCombatLog);

            var combats = parser.Combats;

            Assert.IsNotEmpty(combats);

            var expectedCombat = combats[0];

            Assert.AreEqual(expectedCombat.Name, combat.Name, "Combat name isn't correct.");

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

            Assert.AreEqual(expectedCombatPlayerData.UserName, combatPlayerData.UserName, "Combat players username isn't correct.");

            var expectedCombatPlayerDataHealDone = expectedCombatPlayerData.HealDone;
            var expectedCombatPlayerDataDamageDone = expectedCombatPlayerData.DamageDone;
            var expectedCombatPlayerDataDamageTaken = expectedCombatPlayerData.DamageTaken;
            var expectedCombatPlayerDataEnergyRecovery = expectedCombatPlayerData.EnergyRecovery;

            Assert.AreEqual(expectedCombatPlayerDataHealDone, combatPlayerData.HealDone, "Combat heald one has not expected elements.");
            Assert.AreEqual(expectedCombatPlayerDataDamageDone, combatPlayerData.DamageDone, "Combat damage done has not expected elements.");
            Assert.AreEqual(expectedCombatPlayerDataDamageTaken, combatPlayerData.DamageTaken, "Combat damage taken has not expected elements.");
            Assert.AreEqual(expectedCombatPlayerDataEnergyRecovery, combatPlayerData.EnergyRecovery, "Combat energy recovery has not expected elements.");
        }

        [Test]
        public async Task CombatParserService_Parse_Must_Fill_Combats_Collection_With_Incorrect_Combat_Name()
        {
            var testCombatLog = "combatLog.txt";

            var combatPlayerData = new CombatPlayerData
            {
                UserName = "Oleg - Chrome",
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
                StartDate = new DateTimeOffset(2022, 06, 03, 21, 15, 05, TimeSpan.Zero),
                FinishDate = new DateTimeOffset(2022, 06, 03, 21, 17, 58, TimeSpan.Zero),
                Players = new List<CombatPlayerData> { combatPlayerData },
            };

            var mockCombatDetails = new Mock<ICombatDetails>();
            mockCombatDetails.Setup(x => x.GetDamageDone()).Returns(10);
            mockCombatDetails.Setup(x => x.GetHealDone()).Returns(20);
            mockCombatDetails.Setup(x => x.GetDamageTaken()).Returns(15);
            mockCombatDetails.Setup(x => x.GetResourceRecovery()).Returns(25);
            mockCombatDetails.Setup(x => x.GetDeathsNumber()).Returns(2);

            var mockFileManager = new Mock<IFileManager>();

            var fakeFileContents = CreateCorrectlyCombatLogWithIncorrectCombatName();
            byte[] fakeFileBytes = Encoding.UTF8.GetBytes(fakeFileContents);
            var fakeMemoryStream = new MemoryStream(fakeFileBytes);

            mockFileManager.Setup(fileManager => fileManager.StreamReader(testCombatLog))
                           .Returns(() => new StreamReader(fakeMemoryStream));

            var parser = new CombatParserService(mockCombatDetails.Object, mockFileManager.Object);
            await parser.Parse(testCombatLog);

            var combats = parser.Combats;

            Assert.IsNotEmpty(combats);

            var expectedCombat = combats[0];

            Assert.AreNotEqual(expectedCombat.Name, combat.Name, "Combat name isn't correct.");

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

            Assert.AreEqual(expectedCombatPlayerData.UserName, combatPlayerData.UserName, "Combat players username isn't correct.");

            var expectedCombatPlayerDataHealDone = expectedCombatPlayerData.HealDone;
            var expectedCombatPlayerDataDamageDone = expectedCombatPlayerData.DamageDone;
            var expectedCombatPlayerDataDamageTaken = expectedCombatPlayerData.DamageTaken;
            var expectedCombatPlayerDataEnergyRecovery = expectedCombatPlayerData.EnergyRecovery;

            Assert.AreEqual(expectedCombatPlayerDataHealDone, combatPlayerData.HealDone, "Combat heald one has not expected elements.");
            Assert.AreEqual(expectedCombatPlayerDataDamageDone, combatPlayerData.DamageDone, "Combat damage done has not expected elements.");
            Assert.AreEqual(expectedCombatPlayerDataDamageTaken, combatPlayerData.DamageTaken, "Combat damage taken has not expected elements.");
            Assert.AreEqual(expectedCombatPlayerDataEnergyRecovery, combatPlayerData.EnergyRecovery, "Combat energy recovery has not expected elements.");
        }


        [Test]
        public async Task CombatParserService_Parse_Must_Fill_Combats_Collection_With_Incorrect_Player_Name()
        {
            var testCombatLog = "combatLog.txt";

            var combatPlayerData = new CombatPlayerData
            {
                UserName = "Oleg - Chrome",
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
                StartDate = new DateTimeOffset(2022, 06, 03, 21, 15, 05, TimeSpan.Zero),
                FinishDate = new DateTimeOffset(2022, 06, 03, 21, 17, 58, TimeSpan.Zero),
                Players = new List<CombatPlayerData> { combatPlayerData },
            };

            var mockCombatDetails = new Mock<ICombatDetails>();
            mockCombatDetails.Setup(x => x.GetDamageDone()).Returns(10);
            mockCombatDetails.Setup(x => x.GetHealDone()).Returns(20);
            mockCombatDetails.Setup(x => x.GetDamageTaken()).Returns(15);
            mockCombatDetails.Setup(x => x.GetResourceRecovery()).Returns(25);
            mockCombatDetails.Setup(x => x.GetDeathsNumber()).Returns(2);

            var mockFileManager = new Mock<IFileManager>();

            var fakeFileContents = CreateCorrectlyCombatLogWithIncorrectPlayerName();
            byte[] fakeFileBytes = Encoding.UTF8.GetBytes(fakeFileContents);
            var fakeMemoryStream = new MemoryStream(fakeFileBytes);

            mockFileManager.Setup(fileManager => fileManager.StreamReader(testCombatLog))
                           .Returns(() => new StreamReader(fakeMemoryStream));

            var parser = new CombatParserService(mockCombatDetails.Object, mockFileManager.Object);
            await parser.Parse(testCombatLog);

            var combats = parser.Combats;

            Assert.IsNotEmpty(combats);

            var expectedCombat = combats[0];

            Assert.AreEqual(expectedCombat.Name, combat.Name, "Combat name isn't correct.");

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

            Assert.AreNotEqual(expectedCombatPlayerData.UserName, combatPlayerData.UserName, "Combat players username isn't correct.");

            var expectedCombatPlayerDataHealDone = expectedCombatPlayerData.HealDone;
            var expectedCombatPlayerDataDamageDone = expectedCombatPlayerData.DamageDone;
            var expectedCombatPlayerDataDamageTaken = expectedCombatPlayerData.DamageTaken;
            var expectedCombatPlayerDataEnergyRecovery = expectedCombatPlayerData.EnergyRecovery;

            Assert.AreEqual(expectedCombatPlayerDataHealDone, combatPlayerData.HealDone, "Combat heald one has not expected elements.");
            Assert.AreEqual(expectedCombatPlayerDataDamageDone, combatPlayerData.DamageDone, "Combat damage done has not expected elements.");
            Assert.AreEqual(expectedCombatPlayerDataDamageTaken, combatPlayerData.DamageTaken, "Combat damage taken has not expected elements.");
            Assert.AreEqual(expectedCombatPlayerDataEnergyRecovery, combatPlayerData.EnergyRecovery, "Combat energy recovery has not expected elements.");
        }

        [Test]
        public async Task CombatParserService_Parse_Must_Not_Fill_Combats_Collection()
        {
            var testCombatLog = "combatLog.txt";

            var combatPlayerData = new CombatPlayerData
            {
                UserName = "Oleg - Chrome",
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
                StartDate = new DateTimeOffset(2022, 06, 03, 21, 15, 05, TimeSpan.Zero),
                FinishDate = new DateTimeOffset(2022, 06, 03, 21, 17, 58, TimeSpan.Zero),
                Players = new List<CombatPlayerData> { combatPlayerData },
            };

            var mockCombatDetails = new Mock<ICombatDetails>();
            mockCombatDetails.Setup(x => x.GetDamageDone()).Returns(10);
            mockCombatDetails.Setup(x => x.GetHealDone()).Returns(20);
            mockCombatDetails.Setup(x => x.GetDamageTaken()).Returns(15);
            mockCombatDetails.Setup(x => x.GetResourceRecovery()).Returns(25);
            mockCombatDetails.Setup(x => x.GetDeathsNumber()).Returns(2);

            var mockFileManager = new Mock<IFileManager>();

            var fakeFileContents = CreateIncorrectlyCombatLog();
            byte[] fakeFileBytes = Encoding.UTF8.GetBytes(fakeFileContents);
            var fakeMemoryStream = new MemoryStream(fakeFileBytes);

            mockFileManager.Setup(fileManager => fileManager.StreamReader(testCombatLog))
                           .Returns(() => new StreamReader(fakeMemoryStream));

            var parser = new CombatParserService(mockCombatDetails.Object, mockFileManager.Object);
            await parser.Parse(testCombatLog);

            var combats = parser.Combats;

            Assert.IsEmpty(combats);
        }

        [Test]
        public async Task CombatParserService_Parse_Must_Fill_Combats_Collection_Without_Players()
        {
            var testCombatLog = "combatLog.txt";

            var combat = new Combat
            {
                Name = "Test",
                Data = new List<string> { "data" },
                IsWin = true,
                StartDate = new DateTimeOffset(2022, 06, 03, 21, 15, 05, TimeSpan.Zero),
                FinishDate = new DateTimeOffset(2022, 06, 03, 21, 17, 58, TimeSpan.Zero),
            };

            var mockCombatDetails = new Mock<ICombatDetails>();
            mockCombatDetails.Setup(x => x.GetDamageDone()).Returns(10);
            mockCombatDetails.Setup(x => x.GetHealDone()).Returns(20);
            mockCombatDetails.Setup(x => x.GetDamageTaken()).Returns(15);
            mockCombatDetails.Setup(x => x.GetResourceRecovery()).Returns(25);
            mockCombatDetails.Setup(x => x.GetDeathsNumber()).Returns(2);

            var mockFileManager = new Mock<IFileManager>();

            var fakeFileContents = CreateCorrectlyCombatLogWithoutPlayers();
            byte[] fakeFileBytes = Encoding.UTF8.GetBytes(fakeFileContents);
            var fakeMemoryStream = new MemoryStream(fakeFileBytes);

            mockFileManager.Setup(fileManager => fileManager.StreamReader(testCombatLog))
                           .Returns(() => new StreamReader(fakeMemoryStream));

            var parser = new CombatParserService(mockCombatDetails.Object, mockFileManager.Object);
            await parser.Parse(testCombatLog);

            var combats = parser.Combats;

            Assert.IsNotEmpty(combats);

            var expectedCombat = combats[0];

            Assert.AreEqual(expectedCombat.Name, combat.Name, "Combat name isn't correct.");

            Assert.IsEmpty(combats[0].Players);
        }

        private string CreateCorrectlyCombatLog()
        {
            var fakeFileContents = new StringBuilder();
            fakeFileContents.AppendLine("6/3 21:15:05  ENCOUNTER_START,,\"Test\"");
            fakeFileContents.AppendLine("1  COMBATANT_INFO,Player-4452-02FE19CD");
            fakeFileContents.AppendLine("1  1,Player-4452-02FE19CD,\"Oleg - Chrome\"");
            fakeFileContents.AppendLine("6/3 21:17:58  ENCOUNTER_END,,,,,1");

            return fakeFileContents.ToString();
        }

        private string CreateCorrectlyCombatLogWithoutPlayers()
        {
            var fakeFileContents = new StringBuilder();
            fakeFileContents.AppendLine("6/3 21:15:05  ENCOUNTER_START,,\"Test\"");
            fakeFileContents.AppendLine("1  COMBATANT_IO,Player-4452-02FE19CD");
            fakeFileContents.AppendLine("1  1,Player-4452-02FE19CD,\"Oleg - Chrome\"");
            fakeFileContents.AppendLine("6/3 21:17:58  ENCOUNTER_END,,,,,1");

            return fakeFileContents.ToString();
        }

        private string CreateCorrectlyCombatLogWithIncorrectCombatName()
        {
            var fakeFileContents = new StringBuilder();
            fakeFileContents.AppendLine("6/3 21:15:05  ENCOUNTER_START,,\"Te2st\"");
            fakeFileContents.AppendLine("1  COMBATANT_INFO,Player-4452-02FE19CD");
            fakeFileContents.AppendLine("1  1,Player-4452-02FE19CD,\"Oleg - Chrome\"");
            fakeFileContents.AppendLine("6/3 21:17:58  ENCOUNTER_END,,,,,1");

            return fakeFileContents.ToString();
        }

        private string CreateCorrectlyCombatLogWithIncorrectPlayerName()
        {
            var fakeFileContents = new StringBuilder();
            fakeFileContents.AppendLine("6/3 21:15:05  ENCOUNTER_START,,\"Test\"");
            fakeFileContents.AppendLine("1  COMBATANT_INFO,Player-4452-02FE19CD");
            fakeFileContents.AppendLine("1  1,Player-4452-02FE19CD,\"Ol345eg - Chrome\"");
            fakeFileContents.AppendLine("6/3 21:17:58  ENCOUNTER_END,,,,,1");

            return fakeFileContents.ToString();
        }

        private string CreateIncorrectlyCombatLog()
        {
            var fakeFileContents = new StringBuilder();
            fakeFileContents.AppendLine("6/3 21:15:05  ENCOUNTER_T,,\"Test\"");
            fakeFileContents.AppendLine("1  COMBATANT_INFO,Player-4452-02FE19CD");
            fakeFileContents.AppendLine("1  1,Player-4452-02FE19CD,\"Oleg - Chrome\"");
            fakeFileContents.AppendLine("6/3 21:17:58  ENCOUNTER_END,,,,,1");

            return fakeFileContents.ToString();
        }
    }
}
