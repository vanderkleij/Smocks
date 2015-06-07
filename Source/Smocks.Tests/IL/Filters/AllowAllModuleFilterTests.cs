using System.Diagnostics.CodeAnalysis;
using NUnit.Framework;
using Smocks.IL.Filters;
using Smocks.Tests.TestUtility;

namespace Smocks.Tests.IL.Filters
{
    [ExcludeFromCodeCoverage]
    [TestFixture]
    public class AllowAllModuleFilterTests
    {
        [TestCase]
        public void Accepts_ReturnsTrue()
        {
            var subject = new AllowAllModuleFilter();

            Assert.IsTrue(subject.Accepts(null));
            Assert.IsTrue(subject.Accepts(CecilUtility.Import(typeof(object)).Resolve().Module));
        }
    }
}