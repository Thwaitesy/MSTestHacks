using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Microsoft.VisualStudio.TestTools.UnitTesting
{
    /// <summary>
    /// Contains assertion types that are not provided with the standard MSTest assertions.
    /// </summary>
    public static class ExceptionAssert
    {
        /// <summary>
        /// Checks to make sure that the input delegate throws a exception of type TException.
        /// </summary>
        /// <typeparam name="TException">The type of exception expected.</typeparam>
        /// <returns>The exception that was thrown.</returns>
        /// <param name="blockToExecute">The block of code to execute to generate the exception.</param>
        public static TException Throws<TException>(Action blockToExecute) where TException : System.Exception
        {
            try
            {
                blockToExecute();
            }
            catch (Exception ex)
            {
                Assert.IsTrue(ex is TException, "Expected exception of type " + typeof(TException) + " but type of " + ex.GetType() + " was thrown instead.");
                return ex as TException;
            }

            Assert.Fail("Expected exception of type " + typeof(TException) + ", but no exception was thrown.");
            return default(TException);
        }

        /// <summary>
        /// Checks to make sure that the input delegate throws a exception.
        /// </summary>
        /// <returns>The exception that was thrown.</returns>
        /// <param name="blockToExecute">The block of code to execute to generate the exception.</param>
        public static Exception Throws(Action blockToExecute)
        {
            return Throws<Exception>(blockToExecute);
        }

        /// <summary>
        /// Checks to make sure that the input delegate throws a exception of type TException.
        /// </summary>
        /// <param name="blockToExecute">The block of code to execute to generate the exception.</param>
        /// <param name="expectedMessage">The expected message.</param>
        /// <returns>The exception that was thrown.</returns>
        /// <typeparam name="TException">The type of exception expected.</typeparam>
        public static TException Throws<TException>(Action blockToExecute, string expectedMessage) where TException : System.Exception
        {
            try
            {
                blockToExecute();
            }
            catch (Exception ex)
            {
                Assert.IsTrue(ex is TException, "Expected exception of type " + typeof(TException) + " but type of " + ex.GetType() + " was thrown instead.");
                Assert.AreEqual(expectedMessage, ex.Message, "Expected exception with a message of '" + expectedMessage + "', but exception with message of '" + ex.Message + "' was thrown instead.");
                return ex as TException;
            }

            Assert.Fail("Expected exception of type " + typeof(TException) + " but no exception was thrown.");
            return default(TException);
        }

        /// <summary>
        /// Checks to make sure that the input delegate throws a exception.
        /// </summary>
        /// <param name="blockToExecute">The block of code to execute to generate the exception.</param>
        /// <param name="expectedMessage">The expected message.</param>
        /// <returns>The exception that was thrown.</returns>
        public static Exception Throws(Action blockToExecute, string expectedMessage)
        {
            return Throws<Exception>(blockToExecute, expectedMessage);
        }
    }
}