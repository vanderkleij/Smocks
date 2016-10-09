using System.IO;
using Aspose.Cells;
using Moq;
using NUnit.Framework;

namespace Smocks.Tests.Issues
{
    [TestFixture]
    public class IssueTwentySixTests
    {
        public interface IEntityWorksheetReaderFactory
        {
            void EntityWorksheetReader(Worksheet worksheet, CellArea cellArea);
        }

        [Test]
        public void Issue26()
        {
            // The issue was that this code was throwing a MethodAccessException. By resolving this issue,
            // it now throws a MockException.
            Assert.Throws<MockException>(() =>
            {
                Smock.Run(context =>
                {
                    // The Aspose.Cells assembly has the assembly attribute AllowPartiallyTrustedCallersAttribute set.
                    // This causes issues when we rewrite the methods in the assembly to call into Smocks to handle
                    // interception. We resolve this by simply removing said attribute, so that the calls into
                    // Smocks are then allowed.
                    var workbook = new Workbook(new MemoryStream());
                    var worksheet = workbook.Worksheets.Add("Wal-Mart Stores");
                    context.Setup(() => workbook.Worksheets[It.IsAny<string>()]).Returns(worksheet);

                    var worksheetReaderFactory = Mock.Of<IEntityWorksheetReaderFactory>();

                    Mock.Get(worksheetReaderFactory).Verify(m => m.EntityWorksheetReader(
                        worksheet,
                        It.Is<CellArea>(
                            ca => ca.StartRow == 1 && ca.EndRow == 5 && ca.StartColumn == 0 && ca.EndColumn == 5)),
                        Times.Once);
                });
            });
        }
    }
}