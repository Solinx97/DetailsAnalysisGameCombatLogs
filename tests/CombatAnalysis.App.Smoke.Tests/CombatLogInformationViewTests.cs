using CombatAnalysis.App.Smoke.Tests.Core;
using FlaUI.Core;
using FlaUI.Core.AutomationElements;
using FlaUI.Core.Input;
using FlaUI.Core.WindowsAPI;
using FlaUI.UIA3;
using NUnit.Framework;
using System;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Threading;

namespace CombatAnalysis.App.Smoke.Tests
{
    [TestFixture]
    public class CombatLogInformationViewTests
    {
        private Application _app;
        private UIA3Automation _automation;
        private Stopwatch _stopwatch;
        private AutomationTestBase _automationTestBase;
        private Process _process;

        [OneTimeSetUp]
        public void FixtureSetup()
        {
            Deploy.Run();

            var baseDirectory = Directory.GetParent($"{AppContext.BaseDirectory.Split("tests")[0]}");

            var proc = new ProcessStartInfo
            {
                FileName = "dotnet.exe",
                Arguments = $"run --project {baseDirectory}\\src\\API\\CombatAnalysis.CombatParserAPI {nameof(CommandLineArgs.Tests)}",
            };
            _process = Process.Start(proc);

            var processStartInfo = new ProcessStartInfo("CombatAnalysis.App.exe");
            _app = Application.Launch(processStartInfo);
            _automation = new UIA3Automation();
        }

        [OneTimeTearDown]
        public void FixtureTeardown()
        {
            _automation.Dispose();
            _app.Close();

            var processesByName = Process.GetProcessesByName(_process.ProcessName);
            foreach (var item in processesByName)
            {
                item.Kill();
            }

            _process.Dispose();
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
        public void CombatLogInformationView_Do_Not_Must_Upload_Combat_Log_But_Show_Combat_Log_After_Parse()
        {
            var window = _app.GetMainWindow(_automation);

            #region HomePage

            var openCombatLogInformGetter = window.FindFirstDescendant(x => x.ByAutomationId("openCombatLogInform"));
            Assert.That(openCombatLogInformGetter, Is.Not.Null, "Can't be find element by AutomayionId 'openCombatLogInform'");

            var openCombatLogInform = _automationTestBase.WaitForElement(() => openCombatLogInformGetter);

            openCombatLogInform.DrawHighlight(true, Color.Red, TimeSpan.FromMilliseconds(500));
            openCombatLogInform.Click();

            #endregion

            #region CombatLogInformationView

            var selectCmbatLogFileGetter = window.FindFirstDescendant(x => x.ByAutomationId("selectCmbatLogFile"));
            Assert.That(selectCmbatLogFileGetter, Is.Not.Null, "Can't be find element by AutomayionId 'selectCmbatLogFile'");

            var combatLogFileAnalysisGetter = window.FindFirstDescendant(x => x.ByAutomationId("combatLogFileAnalysis"));
            Assert.That(combatLogFileAnalysisGetter, Is.Not.Null, "Can't be find element by AutomayionId 'combatLogFileAnalysis'");

            var selectCmbatLogFile = _automationTestBase.WaitForElement(() => selectCmbatLogFileGetter).AsButton();
            var combatLogFileAnalysis = _automationTestBase.WaitForElement(() => combatLogFileAnalysisGetter).AsButton();

            var analysisButtonIsEnabled = combatLogFileAnalysis.IsEnabled;
            Assert.That(analysisButtonIsEnabled, Is.Not.Null, "The analysis button must be isn't enabled");

            selectCmbatLogFile.DrawHighlight(true, Color.Red, TimeSpan.FromMilliseconds(500));
            selectCmbatLogFile?.Invoke();

            Thread.Sleep(500);

            OpenFileDialog();

            analysisButtonIsEnabled = combatLogFileAnalysis.IsEnabled;
            Assert.That(analysisButtonIsEnabled, Is.True, "The analysis button must be is enabled");

            var isSaveCombatLogGetter = window.FindFirstDescendant(x => x.ByAutomationId("isSaveCombatLog"));
            Assert.That(isSaveCombatLogGetter, Is.Not.Null, "Can't be find element by AutomayionId 'isSaveCombatLog'");

            var saveCombatLogCheckBox = _automationTestBase.WaitForElement(() => isSaveCombatLogGetter).AsCheckBox();

            saveCombatLogCheckBox.DrawHighlight(true, Color.Red, TimeSpan.FromMilliseconds(500));
            saveCombatLogCheckBox.Toggle();

            combatLogFileAnalysis.DrawHighlight(true, Color.Red, TimeSpan.FromMilliseconds(500));
            combatLogFileAnalysis.Invoke();

            #endregion

            #region DetailsSpecificalCombatPage

            var workWithDataTitleGetter = window.FindFirstDescendant(x => x.ByAutomationId("workWithDataTitle"));
            Assert.That(workWithDataTitleGetter, Is.Not.Null, "Can't be find element by AutomayionId 'workWithDataTitle'");

            var workWithDataTitle = _automationTestBase.WaitForElement(() => workWithDataTitleGetter);

            workWithDataTitle.DrawHighlight(true, Color.Red, TimeSpan.FromMilliseconds(500));

            Thread.Sleep(3000);

            var detailsGetter = window.FindFirstDescendant(x => x.ByAutomationId("details"));
            Assert.That(detailsGetter, Is.Not.Null, "Can't be find element by AutomayionId 'details'");

            var detailsText = _automationTestBase.WaitForElement(() => detailsGetter);

            detailsText.DrawHighlight(true, Color.Red, TimeSpan.FromMilliseconds(500));

            #endregion

            CallbackToMainPage();
        }

        private void OpenFileDialog()
        {
            var window = _app.GetMainWindow(_automation);
            Assert.That(window.ModalWindows.Length, Is.Not.EqualTo(0), "Must be open the file dialog (modal window)");

            var modalWindow = window.ModalWindows[0];
            var directoryTextBox = _automationTestBase.WaitForElement(() => modalWindow.FindFirstDescendant(x => x.ByAutomationId("1001")))?.AsTextBox();

            directoryTextBox.DrawHighlight(true, Color.Red, TimeSpan.FromMilliseconds(500));
            directoryTextBox.Click();

            var baseDirectory = AppDomain.CurrentDomain.BaseDirectory + @"TestsData\";
            directoryTextBox.Enter($"Адрес: {baseDirectory}");

            Keyboard.Press(VirtualKeyShort.ENTER);

            var fileNameTextBox = _automationTestBase.WaitForElement(() => modalWindow.FindFirstDescendant(x => x.ByAutomationId("0")))?.AsTextBox();

            fileNameTextBox.DrawHighlight(true, Color.Red, TimeSpan.FromMilliseconds(500));
            fileNameTextBox.Click();

            var openButton = _automationTestBase.WaitForElement(() => modalWindow.FindFirstDescendant(x => x.ByAutomationId("1")))?.AsButton();

            openButton.DrawHighlight(true, Color.Red, TimeSpan.FromMilliseconds(500));
            openButton?.Invoke();
        }

        private void CallbackToMainPage()
        {
            var window = _app.GetMainWindow(_automation);

            var combatLogUploadGetter = window.FindFirstDescendant(x => x.ByAutomationId("combatLogUpload"));
            Assert.That(combatLogUploadGetter, Is.Not.Null, "Can't be find element by AutomayionId 'combatLogUpload'");

            var btnChildren = combatLogUploadGetter.FindFirstChild(x => x.ByAutomationId("btn"));
            Assert.That(btnChildren, Is.Not.Null, "Can't be find element by AutomayionId 'btn'");

            var combatLogUpload = _automationTestBase.WaitForElement(() => btnChildren).AsButton();

            combatLogUpload.DrawHighlight(true, Color.Red, TimeSpan.FromMilliseconds(500));
            combatLogUpload?.Invoke();

            Thread.Sleep(1000);

            var selectCmbatLogFileGetter = window.FindFirstDescendant(x => x.ByAutomationId("selectCmbatLogFile"));
            Assert.That(selectCmbatLogFileGetter, Is.Not.Null, "Can't be find element by AutomayionId 'selectCmbatLogFile'");

            var selectCmbatLogFile = _automationTestBase.WaitForElement(() => selectCmbatLogFileGetter);
            selectCmbatLogFile.DrawHighlight(true, Color.Red, TimeSpan.FromMilliseconds(500));
        }
    }
}
