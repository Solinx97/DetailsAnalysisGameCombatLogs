using FlaUI.Core;
using FlaUI.Core.AutomationElements;
using FlaUI.UIA3;
using NUnit.Framework;
using System;
using System.Diagnostics;
using System.Drawing;
using System.Threading;

namespace CombatAnalysis.App.Smoke.Tests
{
    [TestFixture]
    public class HomeViewTests
    {
        private Application _app;
        private UIA3Automation _automation;
        private Stopwatch _stopwatch;
        private AutomationTestBase _automationTestBase;

        [OneTimeSetUp]
        public void FixtureSetup()
        {
            var processStartInfo = new ProcessStartInfo("CombatAnalysis.App.exe");
            _app = Application.Launch(processStartInfo);
            _automation = new UIA3Automation();
        }

        [OneTimeTearDown]
        public void FixtureTeardown()
        {
            _automation.Dispose();
            _app.Close();
        }

        [SetUp]
        public void Setup()
        {
            _stopwatch = Stopwatch.StartNew();
            _automationTestBase = new AutomationTestBase();
        }

        [TearDown]
        public void TearDown()
        {
            _stopwatch.Stop();

            Console.WriteLine($"Time elapsed: {_stopwatch.ElapsedMilliseconds}");
        }

        [Test]
        public void HomeView_Must_Open_Main_Information_View()
        {
            var window = _app.GetMainWindow(_automation);

            var openCombatLogInformGetter = window.FindFirstDescendant(x => x.ByAutomationId("openCombatLogInform"));
            Assert.IsNotNull(openCombatLogInformGetter, "Can't be find element by AutomayionId 'openCombatLogInform'");

            var openCombatLogInform = _automationTestBase.WaitForElement(() => openCombatLogInformGetter);

            openCombatLogInform.DrawHighlight(true, Color.Red, TimeSpan.FromMilliseconds(500));
            openCombatLogInform.Click();

            Thread.Sleep(500);

            var combatLogInformTitleGetter = window.FindFirstDescendant(x => x.ByAutomationId("combatLogInformTitle"));
            Assert.IsNotNull(combatLogInformTitleGetter, "Can't be find element by AutomayionId 'combatLogInformTitle'");

            var combatLogInformTitle = _automationTestBase.WaitForElement(() => combatLogInformTitleGetter);

            combatLogInformTitle.DrawHighlight(true, Color.Red, TimeSpan.FromMilliseconds(500));
        }
    }
}
