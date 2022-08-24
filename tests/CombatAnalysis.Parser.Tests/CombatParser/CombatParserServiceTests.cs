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

            var correctCombatLogFile = "6/8 20:42:39.739  COMBAT_LOG_VERSION,9,ADVANCED_LOG_ENABLED,1,BUILD_VERSION,2.5.4,PROJECT_ID,5";
            var fakeFileBytes = Encoding.UTF8.GetBytes(correctCombatLogFile);
            var fakeMemoryStream = new MemoryStream(fakeFileBytes);

            var mockFileManager = new Mock<IFileManager>();
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

            var correctCombatLogFile = "6/8 20:42:39.739  COMBAT_LOG_,9,ADVANCED_LOG_ENABLED,1,BUILD_VERSION,2.5.4,PROJECT_ID,5";
            var fakeFileBytes = Encoding.UTF8.GetBytes(correctCombatLogFile);
            var fakeMemoryStream = new MemoryStream(fakeFileBytes);

            var mockFileManager = new Mock<IFileManager>();
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

            var mockCombatDetails = new Mock<ICombatDetails>();
            mockCombatDetails.Setup(x => x.GetDamageDone()).Returns(36);
            mockCombatDetails.Setup(x => x.GetHealDone()).Returns(39);
            mockCombatDetails.Setup(x => x.GetDamageTaken()).Returns(77);
            mockCombatDetails.Setup(x => x.GetResourceRecovery()).Returns(45);
            mockCombatDetails.Setup(x => x.GetDeathsNumber()).Returns(2);

            var fakeFileContents = CreateCorrectlyCombatLog();
            var fakeFileBytes = Encoding.UTF8.GetBytes(fakeFileContents);
            var fakeMemoryStream = new MemoryStream(fakeFileBytes);

            var mockFileManager = new Mock<IFileManager>();
            mockFileManager.Setup(fileManager => fileManager.StreamReader(testCombatLog))
                           .Returns(() => new StreamReader(fakeMemoryStream));

            var parser = new CombatParserService(mockCombatDetails.Object, mockFileManager.Object);
            await parser.Parse(testCombatLog);

            var combats = parser.Combats;
            Assert.IsNotEmpty(combats);

            var firstCombat = combats[0];

            var expectedCombatPlayerData = new CombatPlayerData
            {
                UserName = "Oleg",
                DamageDone = 36,
                DamageTaken = 77,
                HealDone = 39,
                EnergyRecovery = 45,
            };
            var expectedCombatPlayerData1 = new CombatPlayerData
            {
                UserName = "Kiril",
                DamageDone = 36,
                DamageTaken = 77,
                HealDone = 39,
                EnergyRecovery = 45,
            };
            var expectedCombat = new Combat
            {
                Name = "Test",
                Data = new List<string> { "data" },
                IsWin = true,
                StartDate = new DateTimeOffset(2022, 06, 03, 21, 15, 05, TimeSpan.Zero),
                FinishDate = new DateTimeOffset(2022, 06, 03, 21, 17, 58, TimeSpan.Zero),
                Players = new List<CombatPlayerData> { expectedCombatPlayerData, expectedCombatPlayerData1 },
                HealDone = 78,
                DamageDone = 72,
                DamageTaken = 154,
                EnergyRecovery = 90
            };
            Assert.AreEqual(expectedCombat.Name, firstCombat.Name, "Combat name isn't correct.");

            Assert.IsNotEmpty(firstCombat.Players);

            var firstCombatPlayerData = firstCombat.Players[0];
            Assert.AreEqual(expectedCombatPlayerData.UserName, firstCombatPlayerData.UserName, "Combat players username isn't correct.");

            Assert.AreEqual(expectedCombat.HealDone, firstCombat.HealDone, "Combat heal done has not expected elements.");
            Assert.AreEqual(expectedCombat.DamageDone, firstCombat.DamageDone, "Combat damage done has not expected elements.");
            Assert.AreEqual(expectedCombat.DamageTaken, firstCombat.DamageTaken, "Combat damage taken has not expected elements.");
            Assert.AreEqual(expectedCombat.EnergyRecovery, firstCombat.EnergyRecovery, "Combat energy recovery has not expected elements.");

            Assert.AreEqual(expectedCombatPlayerData.HealDone, firstCombatPlayerData.HealDone, "Combat player heal done has not expected elements.");
            Assert.AreEqual(expectedCombatPlayerData.DamageDone, firstCombatPlayerData.DamageDone, "Combat player damage done has not expected elements.");
            Assert.AreEqual(expectedCombatPlayerData.DamageTaken, firstCombatPlayerData.DamageTaken, "Combat player damage taken has not expected elements.");
            Assert.AreEqual(expectedCombatPlayerData.EnergyRecovery, firstCombatPlayerData.EnergyRecovery, "Combat player energy recovery has not expected elements.");
        }

        [Test]
        public async Task CombatParserService_Parse_Must_Fill_Combats_Collection_With_Incorrect_Combat_Name()
        {
            var testCombatLog = "combatLog.txt";

            var mockCombatDetails = new Mock<ICombatDetails>();
            mockCombatDetails.Setup(x => x.GetDamageDone()).Returns(10);
            mockCombatDetails.Setup(x => x.GetHealDone()).Returns(20);
            mockCombatDetails.Setup(x => x.GetDamageTaken()).Returns(15);
            mockCombatDetails.Setup(x => x.GetResourceRecovery()).Returns(25);
            mockCombatDetails.Setup(x => x.GetDeathsNumber()).Returns(2);

            var fakeFileContents = CreateCorrectlyCombatLogWithIncorrectCombatName();
            var fakeFileBytes = Encoding.UTF8.GetBytes(fakeFileContents);
            var fakeMemoryStream = new MemoryStream(fakeFileBytes);

            var mockFileManager = new Mock<IFileManager>();
            mockFileManager.Setup(fileManager => fileManager.StreamReader(testCombatLog))
                           .Returns(() => new StreamReader(fakeMemoryStream));

            var parser = new CombatParserService(mockCombatDetails.Object, mockFileManager.Object);
            await parser.Parse(testCombatLog);

            var combats = parser.Combats;
            Assert.IsNotEmpty(combats);

            var firstCombat = combats[0];

            var expectedCombat = new Combat
            {
                Name = "Test",
                Data = new List<string> { "data" },
                IsWin = true,
                StartDate = new DateTimeOffset(2022, 06, 03, 21, 15, 05, TimeSpan.Zero),
                FinishDate = new DateTimeOffset(2022, 06, 03, 21, 17, 58, TimeSpan.Zero),
            };
            Assert.AreNotEqual(expectedCombat.Name, firstCombat.Name, "Combat name isn't correct.");
        }

        [Test]
        public async Task CombatParserService_Parse_Must_Fill_Combats_Collection_With_Incorrect_Player_Name()
        {
            var testCombatLog = "combatLog.txt";

            var mockCombatDetails = new Mock<ICombatDetails>();
            mockCombatDetails.Setup(x => x.GetDamageDone()).Returns(10);
            mockCombatDetails.Setup(x => x.GetHealDone()).Returns(20);
            mockCombatDetails.Setup(x => x.GetDamageTaken()).Returns(15);
            mockCombatDetails.Setup(x => x.GetResourceRecovery()).Returns(25);
            mockCombatDetails.Setup(x => x.GetDeathsNumber()).Returns(2);

            var fakeFileContents = CreateCorrectlyCombatLogWithIncorrectPlayerName();
            var fakeFileBytes = Encoding.UTF8.GetBytes(fakeFileContents);
            var fakeMemoryStream = new MemoryStream(fakeFileBytes);

            var mockFileManager = new Mock<IFileManager>();
            mockFileManager.Setup(fileManager => fileManager.StreamReader(testCombatLog))
                           .Returns(() => new StreamReader(fakeMemoryStream));

            var parser = new CombatParserService(mockCombatDetails.Object, mockFileManager.Object);
            await parser.Parse(testCombatLog);

            var combats = parser.Combats;
            Assert.IsNotEmpty(combats);

            var firstCombat = combats[0];

            var expectedCombatPlayerData = new CombatPlayerData
            {
                UserName = "Oleg - Chrome",
                DamageDone = 10,
                DamageTaken = 15,
                HealDone = 20,
                EnergyRecovery = 25,
                UsedBuffs = 2,
            };
            var expectedCombat = new Combat
            {
                Name = "Test",
                Data = new List<string> { "data" },
                IsWin = true,
                StartDate = new DateTimeOffset(2022, 06, 03, 21, 15, 05, TimeSpan.Zero),
                FinishDate = new DateTimeOffset(2022, 06, 03, 21, 17, 58, TimeSpan.Zero),
                Players = new List<CombatPlayerData> { expectedCombatPlayerData },
            };

            Assert.AreEqual(expectedCombat.Name, firstCombat.Name, "Combat name isn't correct.");

            Assert.IsNotEmpty(firstCombat.Players);

            var firstCombatPlayerData = firstCombat.Players[0];
            Assert.AreNotEqual(expectedCombatPlayerData.UserName, firstCombatPlayerData.UserName, "Combat players username isn't correct.");
        }

        [Test]
        public async Task CombatParserService_Parse_Must_Not_Fill_Combats_Collection()
        {
            var testCombatLog = "combatLog.txt";

            var mockCombatDetails = new Mock<ICombatDetails>();
            mockCombatDetails.Setup(x => x.GetDamageDone()).Returns(10);
            mockCombatDetails.Setup(x => x.GetHealDone()).Returns(20);
            mockCombatDetails.Setup(x => x.GetDamageTaken()).Returns(15);
            mockCombatDetails.Setup(x => x.GetResourceRecovery()).Returns(25);
            mockCombatDetails.Setup(x => x.GetDeathsNumber()).Returns(2);

            var fakeFileContents = CreateIncorrectlyCombatLog();
            byte[] fakeFileBytes = Encoding.UTF8.GetBytes(fakeFileContents);
            var fakeMemoryStream = new MemoryStream(fakeFileBytes);

            var mockFileManager = new Mock<IFileManager>();
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

            var firstCombat = combats[0];

            var expectedCombat = new Combat
            {
                Name = "Test",
                Data = new List<string> { "data" },
                IsWin = true,
                StartDate = new DateTimeOffset(2022, 06, 03, 21, 15, 05, TimeSpan.Zero),
                FinishDate = new DateTimeOffset(2022, 06, 03, 21, 17, 58, TimeSpan.Zero),
            };

            Assert.AreEqual(expectedCombat.Name, firstCombat.Name, "Combat name isn't correct.");

            Assert.IsEmpty(combats[0].Players);
        }

        private string CreateCorrectlyCombatLog()
        {
            var fakeFileContents = new StringBuilder();
            fakeFileContents.AppendLine("6/3 21:15:05  ENCOUNTER_START,,\"Test\"");
            fakeFileContents.AppendLine("1  COMBATANT_INFO,Player-4452-02FE19CD");
            fakeFileContents.AppendLine("1  COMBATANT_INFO,Player-4452-02FE19TY");
            fakeFileContents.AppendLine("1  1,Player-4452-02FE19CD,\"Oleg\"");
            fakeFileContents.AppendLine("1  1,Player-4452-02FE19TY,\"Kiril\"");
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
