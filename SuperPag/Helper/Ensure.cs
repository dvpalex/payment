using System;
using System.Collections.Generic;
using System.Text;
using SuperPag.Helper;

namespace SuperPag.Helper
{
    public class Ensure
    {
        public static void IsNotNull(object o, string message, params object[] args)
        {
            if (args != null) message = string.Format ( message, args );
            
            if (o == null) throw new ApplicationException(message);

            if (o is Array && ((Array)o).Length == 0) throw new ApplicationException(message);
        }

        public static bool IsNotNull(object o)
        {
            if (o == null) return false;

            if (o is Array && ((Array)o).Length == 0) return false;

            return true;
        }
        
        public static bool IsNull(object o)
        {
            return ! IsNotNull(o);
        }

        public static bool IsNotNull(string str)
        {
            return !IsNull(str);
        }

        public static bool IsNull(string str)
        {
            return String.IsNullOrEmpty(str);
        }

        public static void IsNotNullOrEmpty(string str, string message, params object[] args)
        {
            if (args != null) message = string.Format(message, args);

            if (String.IsNullOrEmpty(str)) throw new ApplicationException(message);
        }

        public static void IsNotNullOrEmptyPage(string str, string message, params object[] args)
        {
            if (args != null) message = string.Format(message, args);

            if (String.IsNullOrEmpty(str)) GenericHelper.RedirectToErrorPage(message);
        }

        public static bool IsNotNullOrEmpty(string str)
        {
            return !String.IsNullOrEmpty(str);
        }

        public static bool IsNumeric(object o)
        {
            if (o == null) return false;

            int outResult;
            if (!Int32.TryParse(o.ToString(), out outResult))
            {
                return false;
            }

            return true;
        }

        public static bool IsNumeric64(object o)
        {
            if (o == null) return false;

            Int64 outResult;
            if (!Int64.TryParse(o.ToString(), out outResult))
            {
                return false;
            }

            return true;
        }

        public static void IsNumericPage(object o, string message, params object[] args)
        {
            if (args != null) message = string.Format(message, args);
            
            if (o == null) GenericHelper.RedirectToErrorPage(message);

            int outResult;
            if (! Int32.TryParse(o.ToString(), out outResult ))
            {
                GenericHelper.RedirectToErrorPage(message);
            }            
        }

        public static void IsNumeric64Page(object o, string message, params object[] args)
        {
            if (args != null) message = string.Format(message, args);

            if (o == null) GenericHelper.RedirectToErrorPage(message);

            Int64 outResult;
            if (!Int64.TryParse(o.ToString(), out outResult))
            {
                GenericHelper.RedirectToErrorPage(message);
            }
        }

        public static void IsNotNullPage(object o, string message, params object[] args)
        {
            if (args != null) message = string.Format(message, args);

            if (o == null) GenericHelper.RedirectToErrorPage (message);

            if (o is Array && ((Array)o).Length == 0) GenericHelper.RedirectToErrorPage(message);
        }
    }
}
