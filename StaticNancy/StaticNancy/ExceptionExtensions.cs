using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StaticNancy
{
    static class ExceptionExtensions
    {
        public static string GetFullMessage(this Exception ex)
        {
            string full = string.Empty;
            GetInnerMostException(ex, out full);
            return full;
        }

        /// <summary>
		/// Returns the inner most exception (the exception that has no InnerException property).
		/// The concatenation of all exception messages is passed out.
		/// </summary>
		/// <param name="ex"></param>
		/// <param name="fullErrorMessage"></param>
		/// <returns></returns>
		public static Exception GetInnerMostException(this Exception ex, out string fullErrorMessage)
        {
            fullErrorMessage = string.Empty;
            Exception next = ex;
            Exception inner = ex;
            while (next != null)
            {
                inner = next;
                fullErrorMessage += "[" + inner.Message + "]";
                next = next.InnerException;
            }
            return inner;
        }
    }
}
