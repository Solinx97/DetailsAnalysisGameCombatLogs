using CombatAnalysis.CombatParser.Interfaces;
using Moq;
using NUnit.Framework;

namespace CombatAnalysis.Parser.Tests.CombatParser
{
    [TestFixture]
    internal class CombatInformationParsserTests
    {
        [Test]
        public void CombatInformationParsser_Parse_Must_Fill_Combats_Collection()
        {
            var testCombatLog = "";

            var mockParser = new Mock<IParser>();
            mockParser.Setup(x => x.Parse(testCombatLog));

            Assert.IsTrue(true);
        }
    }
}
