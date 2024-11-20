using CombatAnalysis.CombatParser.Entities;
using CombatAnalysis.CombatParser.Interfaces;
using CombatAnalysis.CombatParser.Services;
using Microsoft.Extensions.Logging;
using Moq;
using System.Text;

namespace CombatAnalysis.Parser.Tests.CombatParser;

[TestFixture]
internal class CombatParserServiceTests
{
    [Test]
    public async Task CombatParserService_FileCheck_Must_Return_True()
    {
        var testCombatLog = "combatLog.txt";

        var correctCombatLogFile = "6/8 20:42:39.739  COMBAT_LOG_VERSION,9,ADVANCED_LOG_ENABLED,1,BUILD_VERSION,2.5.4,PROJECT_ID,5";
        var fakeFileBytes = Encoding.UTF8.GetBytes(correctCombatLogFile);
        var fakeMemoryStream = new MemoryStream(fakeFileBytes);

        var mockFileManager = new Mock<IFileManager>();
        mockFileManager.Setup(fileManager => fileManager.StreamReader(testCombatLog))
                       .Returns(() => new StreamReader(fakeMemoryStream));
        var mockLogger = new Mock<ILogger>();

        var parser = new CombatParserService(mockFileManager.Object, mockLogger.Object);
        var fileIsCorrect = await parser.FileCheckAsync(testCombatLog);
        Assert.IsTrue(fileIsCorrect);
    }

    [Test]
    public async Task CombatParserService_FileCheck_Must_Return_False()
    {
        var testCombatLog = "combatLog.txt";

        var correctCombatLogFile = "6/8 20:42:39.739  COMBAT_LOG_,9,ADVANCED_LOG_ENABLED,1,BUILD_VERSION,2.5.4,PROJECT_ID,5";
        var fakeFileBytes = Encoding.UTF8.GetBytes(correctCombatLogFile);
        var fakeMemoryStream = new MemoryStream(fakeFileBytes);

        var mockFileManager = new Mock<IFileManager>();
        mockFileManager.Setup(fileManager => fileManager.StreamReader(testCombatLog))
                       .Returns(() => new StreamReader(fakeMemoryStream));
        var mockLogger = new Mock<ILogger>();

        var parser = new CombatParserService(mockFileManager.Object, mockLogger.Object);
        var fileIsCorrect = await parser.FileCheckAsync(testCombatLog);
        Assert.IsFalse(fileIsCorrect);
    }

    [Test]
    public async Task CombatParserService_Parse_Must_Fill_Combats_Collection_With_Incorrect_Combat_Name()
    {
        var testCombatLog = "combatLog.txt";

        var fakeFileContents = CreateCorrectlyCombatLogWithIncorrectCombatName();
        var fakeFileBytes = Encoding.UTF8.GetBytes(fakeFileContents);
        var fakeMemoryStream = new MemoryStream(fakeFileBytes);

        var mockFileManager = new Mock<IFileManager>();
        mockFileManager.Setup(fileManager => fileManager.StreamReader(testCombatLog))
                       .Returns(() => new StreamReader(fakeMemoryStream));
        var mockLogger = new Mock<ILogger>();

        var parser = new CombatParserService(mockFileManager.Object, mockLogger.Object);
        await parser.ParseAsync(testCombatLog);

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

        var fakeFileContents = CreateCorrectlyCombatLogWithIncorrectPlayerName();
        var fakeFileBytes = Encoding.UTF8.GetBytes(fakeFileContents);
        var fakeMemoryStream = new MemoryStream(fakeFileBytes);

        var mockFileManager = new Mock<IFileManager>();
        mockFileManager.Setup(fileManager => fileManager.StreamReader(testCombatLog))
                       .Returns(() => new StreamReader(fakeMemoryStream));
        var mockLogger = new Mock<ILogger>();

        var parser = new CombatParserService(mockFileManager.Object, mockLogger.Object);
        await parser.ParseAsync(testCombatLog);

        var combats = parser.Combats;
        Assert.IsNotEmpty(combats);

        var firstCombat = combats[0];

        var expectedCombatPlayerData = new CombatPlayer
        {
            Username = "Oleg - Chrome",
            DamageDone = 10,
            DamageTaken = 15,
            HealDone = 20,
            ResourcesRecovery = 25,
            UsedBuffs = 2,
        };
        var expectedCombat = new Combat
        {
            Name = "Test",
            Data = new List<string> { "data" },
            IsWin = true,
            StartDate = new DateTimeOffset(2022, 06, 03, 21, 15, 05, TimeSpan.Zero),
            FinishDate = new DateTimeOffset(2022, 06, 03, 21, 17, 58, TimeSpan.Zero),
            Players = new List<CombatPlayer> { expectedCombatPlayerData },
        };

        Assert.AreEqual(expectedCombat.Name, firstCombat.Name, "Combat name isn't correct.");

        Assert.IsNotEmpty(firstCombat.Players);

        var firstCombatPlayerData = firstCombat.Players[0];
        Assert.AreNotEqual(expectedCombatPlayerData.Username, firstCombatPlayerData.Username, "Combat players username isn't correct.");
    }

    [Test]
    public async Task CombatParserService_Parse_Must_Not_Fill_Combats_Collection()
    {
        var testCombatLog = "combatLog.txt";

        var fakeFileContents = CreateIncorrectlyCombatLog();
        byte[] fakeFileBytes = Encoding.UTF8.GetBytes(fakeFileContents);
        var fakeMemoryStream = new MemoryStream(fakeFileBytes);

        var mockFileManager = new Mock<IFileManager>();
        mockFileManager.Setup(fileManager => fileManager.StreamReader(testCombatLog))
                       .Returns(() => new StreamReader(fakeMemoryStream));
        var mockLogger = new Mock<ILogger>();

        var parser = new CombatParserService(mockFileManager.Object, mockLogger.Object);
        await parser.ParseAsync(testCombatLog);

        var combats = parser.Combats;
        Assert.IsEmpty(combats);
    }

    [Test]
    public async Task CombatParserService_Parse_Must_Fill_Combats_Collection_Without_Players()
    {
        var testCombatLog = "combatLog.txt";

        var mockFileManager = new Mock<IFileManager>();

        var fakeFileContents = CreateCorrectlyCombatLogWithoutPlayers();
        byte[] fakeFileBytes = Encoding.UTF8.GetBytes(fakeFileContents);
        var fakeMemoryStream = new MemoryStream(fakeFileBytes);

        mockFileManager.Setup(fileManager => fileManager.StreamReader(testCombatLog))
                       .Returns(() => new StreamReader(fakeMemoryStream));
        var mockLogger = new Mock<ILogger>();

        var parser = new CombatParserService(mockFileManager.Object, mockLogger.Object);
        await parser.ParseAsync(testCombatLog);

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
