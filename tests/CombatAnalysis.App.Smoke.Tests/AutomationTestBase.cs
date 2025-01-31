using FlaUI.Core.Tools;
using NUnit.Framework;
using System;

namespace CombatAnalysis.App.Smoke.Tests
{
    internal class AutomationTestBase
    {
        public const int BigWaitTimeout = 3000;

        public T WaitForElement<T>(Func<T> getter)
        {
            var retry = Retry.WhileNull(
                getter,
                TimeSpan.FromMilliseconds(BigWaitTimeout));

            if (!retry.Success)
            {
                Assert.Fail($"Failed to get an element within a {BigWaitTimeout}ms");
            }

            return retry.Result;
        }
    }
}
