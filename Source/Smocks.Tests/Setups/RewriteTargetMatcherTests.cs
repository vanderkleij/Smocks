#region License
//// The MIT License (MIT)
//// 
//// Copyright (c) 2015 Tom van der Kleij
//// 
//// Permission is hereby granted, free of charge, to any person obtaining a copy of
//// this software and associated documentation files (the "Software"), to deal in
//// the Software without restriction, including without limitation the rights to
//// use, copy, modify, merge, publish, distribute, sublicense, and/or sell copies of
//// the Software, and to permit persons to whom the Software is furnished to do so,
//// subject to the following conditions:
//// 
//// The above copyright notice and this permission notice shall be included in all
//// copies or substantial portions of the Software.
//// 
//// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
//// IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, FITNESS
//// FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR
//// COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER
//// IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN
//// CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
#endregion

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Reflection;
using Moq;
using NUnit.Framework;
using Smocks.IL;
using Smocks.Setups;
using Smocks.Tests.TestUtility;

namespace Smocks.Tests.Setups
{
    [ExcludeFromCodeCoverage]
    [TestFixture]
    public class RewriteTargetMatcherTests
    {
        private Mock<IMethodImporter> _methodImporterMock;

        [TestCase]
        public void Constructor_MethodImporterNull_ThrowsArgumentNullException()
        {
            var exception = Assert.Throws<ArgumentNullException>(
                () => new RewriteTargetMatcher(null, new List<IRewriteTarget>().AsReadOnly()));

            Assert.AreEqual("methodImporter", exception.ParamName);
        }

        [TestCase]
        public void Constructor_TargetsNull_ThrowsArgumentNullException()
        {
            var exception = Assert.Throws<ArgumentNullException>(
                () => new RewriteTargetMatcher(_methodImporterMock.Object, null));

            Assert.AreEqual("targets", exception.ParamName);
        }

        [TestCase]
        public void GetMatchingTargets_ReturnsAllSetupsThatTargetQueriedMethod()
        {
            ReadOnlyCollection<IRewriteTarget> setups = new List<IRewriteTarget>
            {
                TestDataFactory.CreateSetupTarget(() => Console.WriteLine()),
                TestDataFactory.CreateSetupTarget(() => Console.ReadLine()),
                TestDataFactory.CreateSetupTarget(() => Console.WriteLine())
            }.AsReadOnly();

            var subject = new RewriteTargetMatcher(_methodImporterMock.Object, setups);
            var result = subject.GetMatchingTargets(CecilUtility.Import(setups[0].Methods[0]));

            Assert.AreEqual(2, result.Count());
            Assert.IsTrue(result.All(setup => setup.Methods[0] == setups[0].Methods[0]));
        }

        [TestCase(0)]
        [TestCase(1)]
        public void GetMatchingTargets_ReturnsOnlySetupsThatTargetQueriedMethod(int indexToTest)
        {
            ReadOnlyCollection<IRewriteTarget> setups = new List<IRewriteTarget>
            {
                TestDataFactory.CreateSetupTarget(() => Console.WriteLine()),
                TestDataFactory.CreateSetupTarget(() => Console.ReadLine())
            }.AsReadOnly();

            var subject = new RewriteTargetMatcher(_methodImporterMock.Object, setups);
            var result = subject.GetMatchingTargets(CecilUtility.Import(setups[indexToTest].Methods[0]));

            Assert.AreEqual(setups[indexToTest], result.Single());
        }

        [SetUp]
        public void Setup()
        {
            _methodImporterMock = new Mock<IMethodImporter>();
            _methodImporterMock
                .Setup(importer => importer.Import(It.IsAny<MethodBase>()))
                .Returns<MethodBase>(CecilUtility.Import);
        }
    }
}