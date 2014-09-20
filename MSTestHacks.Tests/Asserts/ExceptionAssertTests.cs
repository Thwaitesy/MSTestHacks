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
            var action = new Action(() => { throw new StackOverflowException(expectedMessage); });

            ExceptionAssert.Throws<StackOverflowException>(action, expectedMessage);
        }

        [TestMethod]
        public void MethodThrowsInheritedException()
        {
            // Arrange
            Action<string> method = (x) => { throw new ArgumentNullException(); };

            // Act & Assert
            ExceptionAssert.Throws<ArgumentException>(() => method("some param"));
        }

        [TestMethod]
        public void MethodThrowsExceptionAndTheValidatorWorks()
        {
            // Arrange
            Action action = () => { throw new Exception("This is silly"); };

            // Act & Assert
            ExceptionAssert.Throws<Exception>(action, validatorForException: x => x.Message == "This is silly");
        }

        [TestMethod]
        public void MethodThrowsExceptionValidatorWorksWhenNotPassing()
        {
            // Arrange
            Action<string> method = (x) => { throw new Exception("This is sillyxxx"); };

            // Act & Assert
            try
            {
                ExceptionAssert.Throws<Exception>(() => method("some param"), x => x.Message == "This is silly");
            }
            catch(Exception ex)
            {
                Assert.IsTrue(ex.Message.Contains("Validator for expected exception failed."));
            }
        }
    }
}
