using System;
using System.Web;
using System.Xml;
using System.Text;
using System.Data;
using System.Web.Security;
using System.Configuration;
using System.Globalization;
using System.Collections;
using System.Collections.Specialized;
using System.Net;
using System.Collections.Generic;
using SuperPag.Business.Messages;
using SuperPag.Data.Messages;
using SuperPag.Data;


namespace SuperPag.Business
{
    /// <summary>
    /// Summary description for Helper
    /// </summary>
    public class GenericHelper
    {
        public static DateTime ParseDateddMMyyyy(string date)
        {
            if (date == null || date.Trim() == "") return DateTime.MinValue;

            DateTime d = new DateTime();
            if (!DateTime.TryParseExact(date, "ddMMyyyy", new System.Globalization.DateTimeFormatInfo(), DateTimeStyles.None, out d))
                return DateTime.MinValue;

            return d;
        }
        public static DateTime ParseDateyyyyMMdd(string date)
        {
            if (date == null || date.Trim() == "") return DateTime.MinValue;

            DateTime d = new DateTime();
            if (!DateTime.TryParseExact(date, "yyyyMMdd", new System.Globalization.DateTimeFormatInfo(), DateTimeStyles.None, out d))
                return DateTime.MinValue;

            return d;
        }
        public static DateTime ParseDate(string date, string format)
        {
            if (date == null || date.Trim() == "") return DateTime.MinValue;

            DateTime d = new DateTime();
            if (!DateTime.TryParseExact(date, format, new System.Globalization.DateTimeFormatInfo(), DateTimeStyles.None, out d))
                return DateTime.MinValue;

            return d;
        }

        public static string ParseString(string str)
        {
            if (str == null) return string.Empty;
            else return str;
        }
        public static int ParseInt(string str)
        {
            int result;
            if (!Int32.TryParse(str, out result))
            {
                result = int.MinValue;
            }
            return result;
        }
        public static decimal ParseDecimal(string value)
        {
            if (value == null || value.Trim() == "") return Decimal.MinValue;
            System.Globalization.NumberFormatInfo numberInfo = new System.Globalization.NumberFormatInfo();
            numberInfo.NumberDecimalSeparator = ".";
            value = value.PadLeft(3, '0');
            value = value.Insert(value.Length - 2, ".");
            return Decimal.Parse(value, System.Globalization.NumberStyles.AllowDecimalPoint | NumberStyles.AllowLeadingSign, numberInfo);
        }
        public static string ParseString(decimal value)
        {
            return (value * 100).ToString("G0");
        }
        public static string FormatCurrencyBrasil(decimal value)
        {
            System.Globalization.NumberFormatInfo numberInfo = new System.Globalization.NumberFormatInfo();
            numberInfo.CurrencyDecimalSeparator = ",";
            numberInfo.CurrencyGroupSeparator = ".";
            numberInfo.CurrencyGroupSizes = new int[] { 3 };
            numberInfo.CurrencySymbol = "R$";
            numberInfo.CurrencyNegativePattern = 2;
            numberInfo.CurrencyPositivePattern = 0;
            numberInfo.CurrencyDecimalDigits = 2;
            return value.ToString("C", numberInfo);
        }
        public static NumberFormatInfo GetNumberFormatBrasil()
        {
            NumberFormatInfo numberInfo = new NumberFormatInfo();
            numberInfo.CurrencyDecimalDigits = 2;
            numberInfo.CurrencyDecimalSeparator = ",";
            numberInfo.CurrencyGroupSeparator = ".";
            numberInfo.CurrencyGroupSizes = new int[] { 3 };
            numberInfo.CurrencyNegativePattern = 2;
            numberInfo.CurrencyPositivePattern = 0;
            numberInfo.CurrencySymbol = "R$";
            numberInfo.NegativeSign = "-";
            numberInfo.NumberDecimalDigits = 2;
            numberInfo.NumberDecimalSeparator = ",";
            numberInfo.NumberGroupSeparator = ".";
            numberInfo.NumberGroupSizes = new int[] { 3 };
            numberInfo.NumberNegativePattern = 2;
            numberInfo.PercentDecimalDigits = 2;
            numberInfo.PercentDecimalSeparator = ",";
            numberInfo.PercentGroupSeparator = ".";
            numberInfo.PercentGroupSizes = new int[] { 3 };
            return numberInfo;
        }
        public static string FormatCurrency(decimal value)
        {
            System.Globalization.NumberFormatInfo numberInfo = new System.Globalization.NumberFormatInfo();
            numberInfo.CurrencyDecimalSeparator = ".";
            numberInfo.CurrencyGroupSeparator = "";
            numberInfo.CurrencyGroupSizes = new int[] { 0 };
            numberInfo.CurrencySymbol = "";
            numberInfo.CurrencyNegativePattern = 2;
            numberInfo.CurrencyPositivePattern = 0;
            numberInfo.CurrencyDecimalDigits = 2;
            return value.ToString("C", numberInfo);
        }

        public static NameValueCollection GetUrlQueryString(string url)
        {
            NameValueCollection collection = new NameValueCollection();

            string[] queryStringSplit = url.Split('?');
            if (queryStringSplit.Length >= 2)
            {
                string queryStringPart = queryStringSplit[1];
                queryStringSplit = queryStringPart.Split('&');
                foreach (string queryStringItem in queryStringSplit)
                {
                    collection.Add(
                        queryStringItem.Split('=')[0],
                        queryStringItem.Split('=')[1]);
                }
            }

            return collection;
        }

        public static string GetSingleNodeString(XmlDocument xmlDoc, string xpath)
        {
            XmlNode xNode = xmlDoc.SelectSingleNode(xpath);
            if (xNode == null)
                return string.Empty;

            return xNode.InnerText;
        }
        public static string GetSingleNodeString(string xml, string xpath)
        {
            try
            {
                if (xml == null)
                    return string.Empty;

                XmlDocument xmlDoc = new XmlDocument();
                xmlDoc.LoadXml(xml);

                XmlNode xNode = xmlDoc.SelectSingleNode(xpath);
                if (xNode == null)
                    return string.Empty;

                return xNode.InnerText;
            }
            catch (Exception)
            {
                return string.Empty;
            }
        }
        public static decimal GetInstallmentValue(int installmentNumber, decimal totalAmount, decimal interestPercentage)
        {
            if (interestPercentage > 0)
            {
                //TODO: Calcular valor da parcela com juros.
                return (totalAmount / installmentNumber);
            }
            else if (interestPercentage < 0)
            {
                //TODO: Calcular valor da parcela com desconto.
                return (totalAmount / installmentNumber);
            }
            else
            {
                return (totalAmount / installmentNumber);
            }
        }

        public static void RedirectToErrorPage(string message, params object[] args)
        {
            if (args != null) message = string.Format(message, args);
            System.Web.HttpContext.Current.Response.Redirect(string.Format("~/error.aspx?message={0}", message));
        }
    }
}
