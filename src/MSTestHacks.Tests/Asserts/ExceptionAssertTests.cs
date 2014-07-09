using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace MSTestHacks.Tests.Asserts
{
    [TestClass]
    public class ExceptionAssertTests
    {
        [TestMethod]
        public void MethodThrowsException()
        {
            ExceptionAssert.Throws(() => { throw new Exception(); });
        }

        [TestMethod]
        public void MethodThrowsSpecificException()
        {
            ExceptionAssert.Throws<StackOverflowException>(() => { throw new StackOverflowException(); });
        }

        [TestMethod]
        public void MethodThrowsExceptionWithExpectedExceptionMessage()
        {
            var expectedMessage = "crap.";

            ExceptionAssert.Throws(() => { throw new Exception(expectedMessage); }, expectedMessage);
        }

        [TestMethod]
        public void MethodThrowsSpecificExceptionWithExpectedExceptionMessage()
        {
            var expectedMessage = "crap.";

            ExceptionAssert.Throws<StackOverflowException>(() => { throw new StackOverflowException(expectedMessage); }, expectedMessage);
        }

        [TestMethod]
        public void MethodThrowsInheritedException()
        {
            // Arrange
            Action<string> method = (x) => { throw new ArgumentNullException(); };

            // Act & Assert
            ExceptionAssert.Throws<ArgumentException>(() => method("some param"));
        }
    }
}
