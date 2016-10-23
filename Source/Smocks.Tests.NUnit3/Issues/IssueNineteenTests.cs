using System;
using JetBrains.dotMemoryUnit.Kernel;
using NUnit.Framework;

namespace Smocks.Tests.NUnit3.Issues
{
    [TestFixture]
    public class IssueNineteenTests
    {
        [TestCase(Description = "The issue was that Smocks was not compatible with NUnit3. This test is an NUnit 3 test that does a basic Smocks operation. This used to fail before Smocks supported NUnit3.")]
        public void Issue19()
        {
            Smock.Run(context =>
            {
                context.Setup(() => DateTime.Now).Returns(new DateTime(2000, 1, 1));

                Assert.AreEqual(2000, DateTime.Now.Year);
            });

            dotMemoryApi.GetSnapshot();
            dotMemoryApi.SaveCollectedData(@"C:\Temp\memory");
        }
    }
}