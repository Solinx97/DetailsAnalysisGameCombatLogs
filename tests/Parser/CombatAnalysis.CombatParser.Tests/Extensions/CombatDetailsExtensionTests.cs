using CombatAnalysis.CombatParser.Interfaces;
using CombatAnalysis.CombatParser.Services;
using Microsoft.Extensions.Logging;
using Moq;

namespace CombatAnalysis.CombatParser.Tests.Extensions;

public class CombatDetailsExtensionTests
{
    [Fact]
    public async Task CalculateGeneralData_ShouldCalculateGeneralData()
    {
        // Arrange
        var mockFileManager = new Mock<IFileManager>();
        var mockLogger = new Mock<ILogger<CombatParserService>>();
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
        Assert.NotEmpty(service.CombatDetails.First().DamageDoneGenerals);
        Assert.NotEmpty(service.CombatDetails.First().HealDoneGenerals);
        Assert.NotEmpty(service.CombatDetails.First().DamageTakenGenerals);
        Assert.NotEmpty(service.CombatDetails.First().ResourcesRecoveryGenerals);
    }

    [Fact]
    public async Task CalculateGeneralData_ShouldNotCalculateGeneralData()
    {
        // Arrange
        var mockFileManager = new Mock<IFileManager>();
        var mockLogger = new Mock<ILogger<CombatParserService>>();
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
        Assert.Empty(service.CombatDetails.First().DamageDoneGenerals);
        Assert.Empty(service.CombatDetails.First().HealDoneGenerals);
        Assert.Empty(service.CombatDetails.First().DamageTakenGenerals);
        Assert.Empty(service.CombatDetails.First().ResourcesRecoveryGenerals);
    }
}
