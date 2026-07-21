using CombatAnalysis.CombatParser.Interfaces;
using CombatAnalysis.CombatParser.Services;
using Microsoft.Extensions.Logging;
using Moq;
using System.Text;

namespace CombatAnalysis.CombatParser.Tests.Services;

public class CombatParserServiceTests
{
    [Fact]
    public async Task FileCheckAsync_ReturnsTrue_WhenFileContainsCombatLogVersion()
    {
        // Arrange
        var fakeFileContent = "6/8 20:42:39.739  COMBAT_LOG_VERSION,9,ADVANCED_LOG_ENABLED,1,BUILD_VERSION,2.5.4,PROJECT_ID,5";
        var mockFileManager = new Mock<IFileManager>();
        var mockLogger = new Mock<ILogger>();
        var mockHttp = new Mock<IHttpClientHelper>();

        var stream = new MemoryStream(Encoding.UTF8.GetBytes(fakeFileContent));
        var reader = new StreamReader(stream);
        mockFileManager.Setup(fm => fm.StreamReader(It.IsAny<string>())).Returns(reader);

        var service = new CombatParserService(mockFileManager.Object, mockLogger.Object, mockHttp.Object);

        // Act
        var result = await service.FileCheckAsync("fakepath.txt");

        // Assert
        Assert.True(result);
    }

    [Fact]
    public async Task FileCheckAsync_ReturnsFalse_WhenFileIsEmpty()
    {
        // Arrange
        var fakeFileContent = string.Empty;
        var mockFileManager = new Mock<IFileManager>();
        var mockLogger = new Mock<ILogger>();
        var mockHttp = new Mock<IHttpClientHelper>();

        var stream = new MemoryStream(Encoding.UTF8.GetBytes(fakeFileContent));
        var reader = new StreamReader(stream);
        mockFileManager.Setup(fm => fm.StreamReader(It.IsAny<string>())).Returns(reader);

        var service = new CombatParserService(mockFileManager.Object, mockLogger.Object, mockHttp.Object);

        // Act
        var result = await service.FileCheckAsync("fakepath.txt");

        // Assert
        Assert.False(result);
    }

    [Fact]
    public async Task ParseAsync_ParseCombatDetailsFromOneFile()
    {
        // Arrange
        var mockFileManager = new Mock<IFileManager>();
        var mockLogger = new Mock<ILogger>();
        var mockHttp = new Mock<IHttpClientHelper>();

        var fakeLines1 = new[] 
        {
            "9/8/2025 21:29:59.4433  ENCOUNTER_START,1501,\"Великая императрица Шек'зир\",3,10,1009,19",
            "9/8/2025 21:31:22.1000  SPELL_DAMAGE,\"Волако\",\"Страж племени Амани\",0xa48,0x0,26862,\"Коварный удар\",84842,86172,0,0,0,-1,0,0,0,131.84,1590.25,333,2.0246,71,1330,889,-1,1,0,0,0,1,nil,nil",
            "9/8/2025 21:34:51.1173  ENCOUNTER_END,1501,\"Великая императрица Шек'зир\",3,10,0",
        };

        mockFileManager
            .Setup(fm => fm.ReadAllLinesAsync("file1.txt", It.IsAny<CancellationToken>()))
            .ReturnsAsync(fakeLines1);

        var service = new CombatParserService(mockFileManager.Object, mockLogger.Object, mockHttp.Object);

        var paths = new List<string> { "file1.txt" };
        var cancellationToken = CancellationToken.None;

        // Act
        await service.ParseAsync(paths, cancellationToken);

        // Assert
        Assert.NotEmpty(service.Combats);
        Assert.NotEmpty(service.CombatDetails);
    }

    [Fact]
    public async Task ParseAsync_EmptyParseCombatDetailsFromOneFileWhenUseIncorrectKeywords()
    {
        // Arrange
        var mockFileManager = new Mock<IFileManager>();
        var mockLogger = new Mock<ILogger>();
        var mockHttp = new Mock<IHttpClientHelper>();

        var fakeLines1 = new[]
        {
            "9/8/2025 21:29:59.4433  START,1501,\"Великая императрица Шек'зир\",3,10,1009,19",
            "9/8/2025 21:31:22.1000  SPELL_DAMAGE,\"Волако\",\"Страж племени Амани\",0xa48,0x0,26862,\"Коварный удар\",84842,86172,0,0,0,-1,0,0,0,131.84,1590.25,333,2.0246,71,1330,889,-1,1,0,0,0,1,nil,nil",
            "9/8/2025 21:34:51.1173  ENCOUNTER_END,1501,\"Великая императрица Шек'зир\",3,10,0",
        };

        mockFileManager
            .Setup(fm => fm.ReadAllLinesAsync("file1.txt", It.IsAny<CancellationToken>()))
            .ReturnsAsync(fakeLines1);

        var service = new CombatParserService(mockFileManager.Object, mockLogger.Object, mockHttp.Object);

        var paths = new List<string> { "file1.txt" };
        var cancellationToken = CancellationToken.None;

        // Act
        await service.ParseAsync(paths, cancellationToken);

        // Assert
        Assert.Empty(service.Combats);
        Assert.Empty(service.CombatDetails);
    }

    [Fact]
    public async Task ParseAsync_ParseCombatDetailsFromFewFiles()
    {
        // Arrange
        var mockFileManager = new Mock<IFileManager>();
        var mockLogger = new Mock<ILogger>();
        var mockHttp = new Mock<IHttpClientHelper>();

        var file1Path = "file1.txt";
        var file2Path = "file2.txt";

        var fakeLines1 = new[]
        {
            "9/8/2025 21:29:59.4433  ENCOUNTER_START,1501,\"Великая императрица Шек'зир\",3,10,1009,19",
            "9/8/2025 21:31:22.1000  SPELL_DAMAGE,\"Волако\",\"Страж племени Амани\",0xa48,0x0,26862,\"Коварный удар\",84842,86172,0,0,0,-1,0,0,0,131.84,1590.25,333,2.0246,71,1330,889,-1,1,0,0,0,1,nil,nil",
            "9/8/2025 21:34:51.1173  ENCOUNTER_END,1501,\"Великая императрица Шек'зир\",3,10,0",
        };

        var fakeLines2 = new[]
{
            "9/8/2025 21:12:23.4433  ENCOUNTER_START,1501,\"Зеквир\",3,10,1009,19",
            "9/8/2025 21:12:49.1000  SPELL_DAMAGE,\"Крито\",\"Страж племени Амани\",0xa48,0x0,26862,\"Коварный удар\",84842,86172,0,0,0,-1,0,0,0,131.84,1590.25,333,2.0246,71,1330,889,-1,1,0,0,0,1,nil,nil",
            "9/8/2025 21:15:50.1173  ENCOUNTER_END,1501,\"Зеквир\",3,10,0",
        };

        mockFileManager
            .Setup(fm => fm.ReadAllLinesAsync(file1Path, It.IsAny<CancellationToken>()))
            .ReturnsAsync(fakeLines1);

        mockFileManager
            .Setup(fm => fm.ReadAllLinesAsync(file2Path, It.IsAny<CancellationToken>()))
            .ReturnsAsync(fakeLines2);

        var service = new CombatParserService(mockFileManager.Object, mockLogger.Object, mockHttp.Object);

        var paths = new List<string> { file1Path, file2Path };
        var cancellationToken = CancellationToken.None;

        // Act
        await service.ParseAsync(paths, cancellationToken);

        // Assert
        Assert.NotEmpty(service.Combats);
        Assert.NotEmpty(service.CombatDetails);
        Assert.Equal(2, service.Combats.Count);
        Assert.Equal(2, service.CombatDetails.Count);
    }

    [Fact]
    public async Task ParseAsync_EmptyParseCombatDetailsFromOneFewFilesWhenUseIncorrectKeywords()
    {
        // Arrange
        var mockFileManager = new Mock<IFileManager>();
        var mockLogger = new Mock<ILogger>();
        var mockHttp = new Mock<IHttpClientHelper>();

        var file1Path = "file1.txt";
        var file2Path = "file2.txt";

        var fakeLines1 = new[]
        {
            "9/8/2025 21:29:59.4433  START,1501,\"Великая императрица Шек'зир\",3,10,1009,19",
            "9/8/2025 21:31:22.1000  SPELL_DAMAGE,\"Волако\",\"Страж племени Амани\",0xa48,0x0,26862,\"Коварный удар\",84842,86172,0,0,0,-1,0,0,0,131.84,1590.25,333,2.0246,71,1330,889,-1,1,0,0,0,1,nil,nil",
            "9/8/2025 21:34:51.1173  ENCOUNTER_END,1501,\"Великая императрица Шек'зир\",3,10,0",
        };

        var fakeLines2 = new[]
{
            "9/8/2025 21:12:23.4433  START,1501,\"Зеквир\",3,10,1009,19",
            "9/8/2025 21:12:49.1000  SPELL_DAMAGE,\"Крито\",\"Страж племени Амани\",0xa48,0x0,26862,\"Коварный удар\",84842,86172,0,0,0,-1,0,0,0,131.84,1590.25,333,2.0246,71,1330,889,-1,1,0,0,0,1,nil,nil",
            "9/8/2025 21:15:50.1173  ENCOUNTER_END,1501,\"Зеквир\",3,10,0",
        };

        mockFileManager
            .Setup(fm => fm.ReadAllLinesAsync(file1Path, It.IsAny<CancellationToken>()))
            .ReturnsAsync(fakeLines1);

        mockFileManager
            .Setup(fm => fm.ReadAllLinesAsync(file2Path, It.IsAny<CancellationToken>()))
            .ReturnsAsync(fakeLines2);

        var service = new CombatParserService(mockFileManager.Object, mockLogger.Object, mockHttp.Object);

        var paths = new List<string> { file1Path, file2Path };
        var cancellationToken = CancellationToken.None;

        // Act
        await service.ParseAsync(paths, cancellationToken);

        // Assert
        Assert.Empty(service.Combats);
        Assert.Empty(service.CombatDetails);
    }

    [Fact]
    public async Task ParseAsync_ParseCombatDetailsFromOnlyOneFile()
    {
        // Arrange
        var mockFileManager = new Mock<IFileManager>();
        var mockLogger = new Mock<ILogger>();
        var mockHttp = new Mock<IHttpClientHelper>();

        var file1Path = "file1.txt";
        var file2Path = "file2.txt";

        var fakeLines1 = new[]
        {
            "9/8/2025 21:29:59.4433  ENCOUNTER_START,1501,\"Великая императрица Шек'зир\",3,10,1009,19",
            "9/8/2025 21:31:22.1000  SPELL_DAMAGE,\"Волако\",\"Страж племени Амани\",0xa48,0x0,26862,\"Коварный удар\",84842,86172,0,0,0,-1,0,0,0,131.84,1590.25,333,2.0246,71,1330,889,-1,1,0,0,0,1,nil,nil",
            "9/8/2025 21:34:51.1173  ENCOUNTER_END,1501,\"Великая императрица Шек'зир\",3,10,0",
        };

        var fakeLines2 = new[]
{
            "9/8/2025 21:12:23.4433  START,1501,\"Зеквир\",3,10,1009,19",
            "9/8/2025 21:12:49.1000  SPELL_DAMAGE,\"Крито\",\"Страж племени Амани\",0xa48,0x0,26862,\"Коварный удар\",84842,86172,0,0,0,-1,0,0,0,131.84,1590.25,333,2.0246,71,1330,889,-1,1,0,0,0,1,nil,nil",
            "9/8/2025 21:15:50.1173  ENCOUNTER_END,1501,\"Зеквир\",3,10,0",
        };

        mockFileManager
            .Setup(fm => fm.ReadAllLinesAsync(file1Path, It.IsAny<CancellationToken>()))
            .ReturnsAsync(fakeLines1);

        mockFileManager
            .Setup(fm => fm.ReadAllLinesAsync(file2Path, It.IsAny<CancellationToken>()))
            .ReturnsAsync(fakeLines2);

        var service = new CombatParserService(mockFileManager.Object, mockLogger.Object, mockHttp.Object);

        var paths = new List<string> { file1Path, file2Path };
        var cancellationToken = CancellationToken.None;

        // Act
        await service.ParseAsync(paths, cancellationToken);

        // Assert
        Assert.NotEmpty(service.Combats);
        Assert.NotEmpty(service.CombatDetails);
        Assert.Equal(1, service.Combats.Count);
        Assert.Equal(1, service.CombatDetails.Count);
    }

    [Fact]
    public async Task ParseAsync_ParseCombatDetailsDataFromOneFile()
    {
        // Arrange
        var mockFileManager = new Mock<IFileManager>();
        var mockLogger = new Mock<ILogger>();
        var mockHttp = new Mock<IHttpClientHelper>();

        var fakeLines1 = new[]
        {
            "9/8/2025 21:29:59.4433  ENCOUNTER_START,1501,\"Великая императрица Шек'зир\",3,10,1009,19",
            "9/8/2025 21:29:59.4433  COMBATANT_INFO,Player-4474-043AF1DD,0,123,132,19099,20601,292,0,0,0,2091,2091,2091,5349,5349,5349,5100,5100,5100,0,0,0,14089,48,(,2,0,0,1,1,1,-1),(),[(90409,496,(),(),(76885,90,76700,90)),(87028,502,(),(),()),(89340,489,(4806,0,0),(),(76700,90)),(41248,1,(),(),()),(82437,476,(4419,0,0),(),(76672,90,76643,90)),(89930,502,(0,0,4223),(),(76700,90,76700,90)),(85376,496,(4895,0,0),(),(76672,90)),(86178,496,(4429,0,0),(),(76700,90)),(86958,509,(4414,0,0),(),()),(89931,502,(0,0,4898),(),(76643,90)),(90859,489,(),(),()),(89072,489,(),(),()),(86792,476,(),(),()),(79331,476,(),(),()),(89971,476,(4892,0,4897),(),()),(86796,476,(4441,0,0),(),()),(79334,476,(),(),()),(69210,1,(),(),())],[Player-4474-043AF1DD,61316,1,Player-4474-043AF1DD,6117,1,Player-4474-04724889,55610,1,Player-4474-043F7371,77747,1,Player-4474-043F7371,116956,1,Player-4474-047B84D4,109773,1,Player-4474-058E8C53,24907,1,Player-4474-03EBE4DE,19506,1,Player-4474-04724889,57330,1,Player-4474-0335E617,19740,1,Player-4474-043AF1DD,105691,1,Player-4474-043AF1DD,104277,1],0,0,(699,314,447,651,0,323)",
            "9/8/2025 21:31:22.1000  SPELL_DAMAGE,\"Волако\",\"Страж племени Амани\",0xa48,0x0,26862,\"Коварный удар\",84842,86172,0,0,0,-1,0,0,0,131.84,1590.25,333,2.0246,71,1330,889,-1,1,0,0,0,1,nil,nil",
            "9/8/2025 21:34:51.1173  ENCOUNTER_END,1501,\"Великая императрица Шек'зир\",3,10,0",
        };

        mockFileManager
            .Setup(fm => fm.ReadAllLinesAsync("file1.txt", It.IsAny<CancellationToken>()))
            .ReturnsAsync(fakeLines1);

        var service = new CombatParserService(mockFileManager.Object, mockLogger.Object, mockHttp.Object);

        var paths = new List<string> { "file1.txt" };
        var cancellationToken = CancellationToken.None;

        // Act
        await service.ParseAsync(paths, cancellationToken);

        // Assert
        Assert.NotEmpty(service.Combats);
        Assert.NotEmpty(service.CombatDetails);
        Assert.NotEmpty(service.CombatDetails.First().DamageDones);
        Assert.NotEmpty(service.CombatDetails.First().HealDones);
        Assert.NotEmpty(service.CombatDetails.First().DamageTakens);
        Assert.NotEmpty(service.CombatDetails.First().ResourcesRecoveries);
    }

    [Fact]
    public async Task ParseAsync_ParseCombatDetailsDataFromFewFiles()
    {
        // Arrange
        var mockFileManager = new Mock<IFileManager>();
        var mockLogger = new Mock<ILogger>();
        var mockHttp = new Mock<IHttpClientHelper>();

        var file1Path = "file1.txt";
        var file2Path = "file2.txt";

        var fakeLines1 = new[]
        {
            "9/8/2025 21:29:59.4433  ENCOUNTER_START,1501,\"Великая императрица Шек'зир\",3,10,1009,19",
            "9/8/2025 21:29:59.4433  COMBATANT_INFO,Player-4474-043AF1DD,0,123,132,19099,20601,292,0,0,0,2091,2091,2091,5349,5349,5349,5100,5100,5100,0,0,0,14089,48,(,2,0,0,1,1,1,-1),(),[(90409,496,(),(),(76885,90,76700,90)),(87028,502,(),(),()),(89340,489,(4806,0,0),(),(76700,90)),(41248,1,(),(),()),(82437,476,(4419,0,0),(),(76672,90,76643,90)),(89930,502,(0,0,4223),(),(76700,90,76700,90)),(85376,496,(4895,0,0),(),(76672,90)),(86178,496,(4429,0,0),(),(76700,90)),(86958,509,(4414,0,0),(),()),(89931,502,(0,0,4898),(),(76643,90)),(90859,489,(),(),()),(89072,489,(),(),()),(86792,476,(),(),()),(79331,476,(),(),()),(89971,476,(4892,0,4897),(),()),(86796,476,(4441,0,0),(),()),(79334,476,(),(),()),(69210,1,(),(),())],[Player-4474-043AF1DD,61316,1,Player-4474-043AF1DD,6117,1,Player-4474-04724889,55610,1,Player-4474-043F7371,77747,1,Player-4474-043F7371,116956,1,Player-4474-047B84D4,109773,1,Player-4474-058E8C53,24907,1,Player-4474-03EBE4DE,19506,1,Player-4474-04724889,57330,1,Player-4474-0335E617,19740,1,Player-4474-043AF1DD,105691,1,Player-4474-043AF1DD,104277,1],0,0,(699,314,447,651,0,323)",
            "9/8/2025 21:31:22.1000  SPELL_DAMAGE,\"Волако\",\"Страж племени Амани\",0xa48,0x0,26862,\"Коварный удар\",84842,86172,0,0,0,-1,0,0,0,131.84,1590.25,333,2.0246,71,1330,889,-1,1,0,0,0,1,nil,nil",
            "9/8/2025 21:34:51.1173  ENCOUNTER_END,1501,\"Великая императрица Шек'зир\",3,10,0",
        };

        var fakeLines2 = new[]
{
            "9/8/2025 21:12:23.4433  ENCOUNTER_START,1501,\"Зеквир\",3,10,1009,19",
            "9/8/2025 21:12:23.4433  COMBATANT_INFO,Player-4234-043AF1DD,0,123,132,19099,20601,292,0,0,0,2091,2091,2091,5349,5349,5349,5100,5100,5100,0,0,0,14089,48,(,2,0,0,1,1,1,-1),(),[(90409,496,(),(),(76885,90,76700,90)),(87028,502,(),(),()),(89340,489,(4806,0,0),(),(76700,90)),(41248,1,(),(),()),(82437,476,(4419,0,0),(),(76672,90,76643,90)),(89930,502,(0,0,4223),(),(76700,90,76700,90)),(85376,496,(4895,0,0),(),(76672,90)),(86178,496,(4429,0,0),(),(76700,90)),(86958,509,(4414,0,0),(),()),(89931,502,(0,0,4898),(),(76643,90)),(90859,489,(),(),()),(89072,489,(),(),()),(86792,476,(),(),()),(79331,476,(),(),()),(89971,476,(4892,0,4897),(),()),(86796,476,(4441,0,0),(),()),(79334,476,(),(),()),(69210,1,(),(),())],[Player-4474-043AF1DD,61316,1,Player-4474-043AF1DD,6117,1,Player-4474-04724889,55610,1,Player-4474-043F7371,77747,1,Player-4474-043F7371,116956,1,Player-4474-047B84D4,109773,1,Player-4474-058E8C53,24907,1,Player-4474-03EBE4DE,19506,1,Player-4474-04724889,57330,1,Player-4474-0335E617,19740,1,Player-4474-043AF1DD,105691,1,Player-4474-043AF1DD,104277,1],0,0,(699,314,447,651,0,323)",
            "9/8/2025 21:12:49.1000  SPELL_DAMAGE,\"Крито\",\"Страж племени Амани\",0xa48,0x0,26862,\"Коварный удар\",84842,86172,0,0,0,-1,0,0,0,131.84,1590.25,333,2.0246,71,1330,889,-1,1,0,0,0,1,nil,nil",
            "9/8/2025 21:15:50.1173  ENCOUNTER_END,1501,\"Зеквир\",3,10,0",
        };

        mockFileManager
            .Setup(fm => fm.ReadAllLinesAsync(file1Path, It.IsAny<CancellationToken>()))
            .ReturnsAsync(fakeLines1);

        mockFileManager
            .Setup(fm => fm.ReadAllLinesAsync(file2Path, It.IsAny<CancellationToken>()))
            .ReturnsAsync(fakeLines2);

        var service = new CombatParserService(mockFileManager.Object, mockLogger.Object, mockHttp.Object);

        var paths = new List<string> { file1Path, file2Path };
        var cancellationToken = CancellationToken.None;

        // Act
        await service.ParseAsync(paths, cancellationToken);

        // Assert
        Assert.NotEmpty(service.Combats);
        Assert.NotEmpty(service.CombatDetails);
        Assert.Equal(2, service.Combats.Count);
        Assert.Equal(2, service.CombatDetails.Count);
        Assert.NotEmpty(service.CombatDetails[0].DamageDones);
        Assert.NotEmpty(service.CombatDetails[0].HealDones);
        Assert.NotEmpty(service.CombatDetails[0].DamageTakens);
        Assert.NotEmpty(service.CombatDetails[0].ResourcesRecoveries);
        Assert.NotEmpty(service.CombatDetails[1].DamageDones);
        Assert.NotEmpty(service.CombatDetails[1].HealDones);
        Assert.NotEmpty(service.CombatDetails[1].DamageTakens);
        Assert.NotEmpty(service.CombatDetails[1].ResourcesRecoveries);
    }
}
