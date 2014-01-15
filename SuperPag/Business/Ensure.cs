using System;
using System.Collections.Generic;
using System.Text;

namespace SuperPag.Business
{
    public class Ensure
    {
        public static void IsNotNull(object o, string message, params object[] args)
        {
            if (args != null) message = string.Format(message, args);

            if (o == null) throw new ApplicationException(message);

            if (o is Array && ((Array)o).Length == 0) throw new ApplicationException(message);
        }

        public static bool IsNull(object o)
        {
            return !IsNotNull(o);
        }

        public static bool IsNull(string str)
        {
            return ((str == null) || (str == string.Empty));
        }

        public static bool IsNotNull(string str)
        {
            return !IsNull(str);
        }

        public static void IsNumericPage(object o, string message, params object[] args)
        {
            if (args != null) message = string.Format(message, args);

            if (o == null) GenericHelper.RedirectToErrorPage(message);

            int outResult;
            if (!Int32.TryParse(o.ToString(), out outResult))
            {
                GenericHelper.RedirectToErrorPage(message);
            }
        }

        public static void IsNotNullPage(object o, string message, params object[] args)
        {
            if (args != null) message = string.Format(message, args);

            if (o == null) GenericHelper.RedirectToErrorPage(message);

            if (o is Array && ((Array)o).Length == 0) GenericHelper.RedirectToErrorPage(message);
        }

        public static bool IsNotNull(object o)
        {
            if (o == null) return false;

            if (o is Array && ((Array)o).Length == 0) return false;

            return true;
        }
    }
}
