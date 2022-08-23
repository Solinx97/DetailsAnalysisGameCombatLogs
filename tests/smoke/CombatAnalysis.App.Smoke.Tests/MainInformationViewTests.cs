using FlaUI.Core;
using FlaUI.Core.AutomationElements;
using FlaUI.Core.Input;
using FlaUI.Core.WindowsAPI;
using FlaUI.UIA3;
using NUnit.Framework;
using System;
using System.Diagnostics;

namespace CombatAnalysis.App.Smoke.Tests
{
    [TestFixture]
    internal class MainInformationViewTests
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

            Mouse.Position = new System.Drawing.Point(0, 0);
        }

        [TearDown]
        public void TearDown()
        {
            _stopwatch.Stop();

            Console.WriteLine($"Time elapsed: {_stopwatch.ElapsedMilliseconds}");
        }

        [Test]
        public void MainInformationView_Do_Not_Must_Upload_Combat_Log_But_Show_Combat_Log_After_Parse()
        {
            var window = _app.GetMainWindow(_automation);

            var logUploadButton = _automationTestBase.WaitForElement(() => window.FindFirstDescendant(x => x.ByText("Загрузить")))?.AsButton();
            var analysisButton = _automationTestBase.WaitForElement(() => window.FindFirstDescendant(x => x.ByText("Анализ")))?.AsButton();

            var analysisButtonIsEnabled = analysisButton.IsEnabled;
            if (analysisButtonIsEnabled)
            {
                Assert.Fail("The analysis button must be isn't enabled");
            }

            logUploadButton?.Invoke();

            OpenFileDialog();

            analysisButtonIsEnabled = analysisButton.IsEnabled;
            if (!analysisButtonIsEnabled)
            {
                Assert.Fail("The analysis button must be is enabled");
            }

            var saveCombatLogCheckBox = _automationTestBase.WaitForElement(() => window.FindFirstDescendant(x => x.ByText("Сохранить данные")))?.AsCheckBox();
            saveCombatLogCheckBox.Toggle();
            analysisButton?.Invoke();

            var workWithDataText = _automationTestBase.WaitForElement(() => window.FindFirstDescendant(x => x.ByText("Работа с данными...")));
            var detailsText = _automationTestBase.WaitForElement(() => window.FindFirstDescendant(x => x.ByText("Подробнее")));

            CallbackToMainPage();
        }

        private void OpenFileDialog()
        {
            var window = _app.GetMainWindow(_automation);
            if (window.ModalWindows.Length == 0)
            {
                Assert.Fail("Must be open the file dialog (modal window)");
            }

            var modalWindow = window.ModalWindows[0];
            var directoryTextBox = _automationTestBase.WaitForElement(() => modalWindow.FindFirstDescendant(x => x.ByAutomationId("1001")))?.AsTextBox();
            directoryTextBox.Click();
            directoryTextBox.Enter(@"Адрес: Этот компьютер");

            Keyboard.Press(VirtualKeyShort.ENTER);

            directoryTextBox.Click();
            directoryTextBox.Enter(@"Адрес: D:\Devs\DetalsAnalysisGamesCombatLogs\tests\smoke\CombatAnalysis.App.Smoke.Tests\testsData");

            Keyboard.Press(VirtualKeyShort.ENTER);

            var fileNameTextBox = _automationTestBase.WaitForElement(() => modalWindow.FindFirstDescendant(x => x.ByName("WoWCombatLog")))?.AsTextBox();
            fileNameTextBox.Click();

            var openButton = _automationTestBase.WaitForElement(() => modalWindow.FindFirstDescendant(x => x.ByAutomationId("1")))?.AsButton();
            openButton?.Invoke();
        }

        private void CallbackToMainPage()
        {
            var window = _app.GetMainWindow(_automation);

            var callbackToMainPage = _automationTestBase.WaitForElement(() => window.FindFirstDescendant(x => x.ByText("Загрузка")));
            callbackToMainPage.Click();

            var logUploadButton = _automationTestBase.WaitForElement(() => window.FindFirstDescendant(x => x.ByText("Загрузить")));
        }
    }
}
