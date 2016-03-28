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
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using Moq;
using NUnit.Framework;
using Smocks.Exceptions;
using Smocks.IL;
using Smocks.Injection;
using Smocks.Setups;
using Smocks.Tests.TestUtility;
using Smocks.Utility;

namespace Smocks.Tests.IL
{
    [ExcludeFromCodeCoverage]
    [TestFixture]
    public class SetupExtractorTests
    {
        private static readonly ExpressionHelper ExpressionHelper = new ExpressionHelper();

        [TestCase]
        public void GetSetups_ArrayLength_ThrowsSetupExtractionException()
        {
            var subject = ServiceLocator.Default.Resolve<SetupExtractor>();

            Assert.Throws<SetupExtractionException>(() =>
            {
                subject.GetSetups(ReflectionUtility.GetLambdaMethod<ISmocksContext>(context =>
                {
                    context.Setup(() => new int[0].Length).Returns(10);
                })).ToList();
            });
        }

        [TestCase]
        public void GetSetups_ArrayMethod_ExtractsSetup()
        {
            var subject = ServiceLocator.Default.Resolve<SetupExtractor>();

            var result = subject.GetSetups(ReflectionUtility.GetLambdaMethod<ISmocksContext>(context =>
            {
                context.Setup(() => new int[10].GetLength(0));
            })).ToList();

            Assert.AreEqual(1, result.Count);
        }

        [TestCase]
        public void GetSetups_FloatSetup_ReturnsCorrectSetups()
        {
            var subject = ServiceLocator.Default.Resolve<SetupExtractor>();

            var result = subject.GetSetups(ReflectionUtility.GetLambdaMethod<ISmocksContext>(context =>
            {
                context.Setup(() => 1.23.ToString(CultureInfo.InvariantCulture));
                context.Setup(() => 1.23f.ToString(CultureInfo.InvariantCulture));
            })).ToList();

            Assert.AreEqual(2, result.Count);
        }

        /// <summary>
        /// The setup extractor should extract .Setup(() => ....) calls from
        /// a lamda without invoking any other code in the lambda. This
        /// test confirms that only the code creating expressions is ran
        /// during extraction of setups.
        /// </summary>
        [TestCase]
        public void GetSetups_InterweavedCode_DoesntExecuteNonSetupCode()
        {
            var subject = ServiceLocator.Default.Resolve<SetupExtractor>();

            string test = null;
            var mock = new Mock<IDisposable>();
            ISetup<DateTime> setup = null;

            // Act
            var result = subject.GetSetups(ReflectionUtility.GetLambdaMethod<ISmocksContext>(context =>
            {
                setup = context.Setup(() => DateTime.Now);

                test = "No longer null";

                context.Setup(() => Console.ReadLine());

                mock.Object.Dispose();

                // Prevent the compiler from optimizing away unreachable code after the exception.
                // Reminder: fix this test in the year 1000000 :-)
                if (DateTime.Now.Year < 1000000)
                {
                    throw new Exception("Can we get the next setup without crashing on this exception?");
                }

                context.Setup(() => Enumerable.Range(1, 10).Where(x => x > 5).ToList());
            })).ToList();

            // Assert
            Assert.AreEqual(3, result.Count);
            Assert.Null(test);
            Assert.Null(setup);
            mock.Verify(disposable => disposable.Dispose(), Times.Never);
        }

        [TestCase]
        public void GetSetups_Method_ReturnsExpression()
        {
            var subject = ServiceLocator.Default.Resolve<SetupExtractor>();

            LambdaExpression expectedExpression = ReflectionUtility.GetExpression(() => Console.ReadLine());

            MethodInfo lambdaMethod = ReflectionUtility.GetLambdaMethod<ISmocksContext>(context =>
            {
                context.Setup(() => Console.ReadLine());
            });

            SetupTarget result = subject.GetSetups(lambdaMethod).FirstOrDefault();

            Assert.NotNull(result);
            Assert.AreEqual(ExpressionHelper.GetMethod(expectedExpression),
                ExpressionHelper.GetMethod(result.Expression as LambdaExpression));
        }

        [TestCase]
        public void GetSetups_MultipleSetups_ReturnsCorrectNumberOfSetups()
        {
            var subject = ServiceLocator.Default.Resolve<SetupExtractor>();

            var result = subject.GetSetups(ReflectionUtility.GetLambdaMethod<ISmocksContext>(context =>
            {
                context.Setup(() => DateTime.Now);
                context.Setup(() => Console.ReadLine());
                context.Setup(() => Enumerable.Range(1, 10).Where(x => x > 5).ToList());
            })).ToList();

            Assert.AreEqual(3, result.Count);
        }

        [TestCase]
        public void GetSetups_Property_ReturnsExpression()
        {
            var subject = ServiceLocator.Default.Resolve<SetupExtractor>();

            LambdaExpression expectedExpression = ReflectionUtility.GetExpression(() => DateTime.Now);

            MethodInfo lambdaMethod = ReflectionUtility.GetLambdaMethod<ISmocksContext>(context =>
            {
                context.Setup(() => DateTime.Now);
            });

            SetupTarget result = subject.GetSetups(lambdaMethod).FirstOrDefault();

            Assert.NotNull(result);
            Assert.AreEqual(ExpressionHelper.GetProperty(expectedExpression),
                ExpressionHelper.GetProperty(result.Expression as LambdaExpression));
        }

        [TestCase]
        public void GetSetups_ScanAll()
        {
            // To test robustness of the GetSetups method, we scan the entire
            // Tests assembly for Expressions. It should contain a decent number of
            // challenging expressions.
            var assembly = typeof(SetupExtractorTests).Assembly;

            var subject = ServiceLocator.Default.Resolve<SetupExtractor>();
            var setups = new List<SetupTarget>();

            foreach (var type in assembly.GetTypes())
            {
                var methods = type
                    .GetMethods(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Static)
                    .Where(method => method.GetMethodBody() != null);

                foreach (var method in methods)
                {
                    // Skip tests that are expected to throw.
                    if (!method.Name.Contains("Throws") && !method.DeclaringType.Name.Contains("Throws"))
                    {
                        List<SetupTarget> methodSetups = subject.GetSetups(method).ToList();
                        setups.AddRange(methodSetups);
                    }
                }
            }

            Assert.IsNotEmpty(setups);
        }

        [TestCase]
        public void GetSetups_SwitchInCode_ExtractsSetup()
        {
            var subject = ServiceLocator.Default.Resolve<SetupExtractor>();

            var result = subject.GetSetups(ReflectionUtility.GetLambdaMethod<ISmocksContext>(context =>
            {
                switch (DateTime.Now.Year)
                {
                    case 2015:
                        Console.WriteLine("2015");
                        break;

                    default:
                        Console.WriteLine("Not 2015");
                        break;
                }

                context.Setup(() => new int[10].GetLength(0));
            })).ToList();

            Assert.AreEqual(1, result.Count);
        }
    }
}