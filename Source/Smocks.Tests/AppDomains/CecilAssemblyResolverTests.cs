using System;
using NUnit.Framework;
using Smocks.AppDomains;

namespace Smocks.Tests.AppDomains
{
    [TestFixture]
    public class CecilAssemblyResolverTests
    {
        [TestCase]
        public void Constructor_ConfigurationNull_ThrowsArgumentNullException()
        {
            var exception = Assert.Throws<ArgumentNullException>(() => new CecilAssemblyResolver(default(Configuration)));
            Assert.AreEqual("configuration", exception.ParamName);
        }

        [TestCase]
        public void Constructor_ConfigurationWithSearchDirectories_AddsSearchDirectories()
        {
            const string searchPath = @"c:\my\search\path";

            var configuration = new Configuration
            {
                AssemblySearchDirectories = new[] { searchPath }
            };

            var subject = new CecilAssemblyResolver(configuration);

            Assert.Contains(searchPath, subject.GetSearchDirectories());
        }

        [TestCase]
        public void Constructor_ConfigurationWithSearchDirectoriesNull_DoesntThrow()
        {
            var configuration = new Configuration
            {
                AssemblySearchDirectories = null
            };

            Assert.DoesNotThrow(() => new CecilAssemblyResolver(configuration));
        }

        [TestCase]
        public void Constructor_WithConfiguration_AddsAppDomainBaseDirectoryAsSearchPath()
        {
            var subject = new CecilAssemblyResolver(new Configuration());
            Assert.Contains(AppDomain.CurrentDomain.BaseDirectory, subject.GetSearchDirectories());
        }

        [TestCase]
        public void Constructor_WithOutConfiguration_AddsAppDomainBaseDirectoryAsSearchPath()
        {
            var subject = new CecilAssemblyResolver();
            Assert.Contains(AppDomain.CurrentDomain.BaseDirectory, subject.GetSearchDirectories());
        }
    }
}