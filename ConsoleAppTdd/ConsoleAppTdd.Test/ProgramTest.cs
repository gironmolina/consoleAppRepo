using System;
using System.IO;
using System.Linq;
using NUnit.Framework;

namespace ConsoleAppTdd.Test
{
    [TestFixture]
    public class ProgramTest
    {
        [Test]
        public void MainTest()
        {
            // Arrange
            using (var sw = new StringWriter())
            {
                using (var sr = new StringReader("100"))
                {
                    Console.SetOut(sw);
                    Console.SetIn(sr);

                    // Act
                    Program.Main();

                    // Assert
                    string result = sw.ToString();
                    Assert.IsFalse(result.Contains('7'));
                }
            }
        }

        [Test]
        public void MainTest_NullValue()
        {
            // Arrange
            using (var sw = new StringWriter())
            {
                using (var sr = new StringReader(string.Empty))
                {
                    Console.SetOut(sw);
                    Console.SetIn(sr);

                    // Act
                    Program.Main();

                    // Assert
                    string result = sw.ToString();
                    Assert.AreEqual(result, "Enter upper limit: ");
                }
            }
        }
    }
}
