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
using SuperPag;
using SuperPag.Data;
using SuperPag.Data.Messages;
using System.Collections.Generic;
using System.IO;
using System.Security.Cryptography;

namespace SuperPag.Helper
{
    /// <summary>
    /// Summary description for Helper
    /// </summary>
    public class GenericHelper
    {
        public static void RedirectToErrorPage(string message, params object[] args)
        {
            if (args != null) message = string.Format(message, args);
            System.Web.HttpContext.Current.Server.Transfer(string.Format("~/error.aspx?message={0}", message));
        }

        public static void AppendNodeForCollection(XmlDocument xmlDoc, string rootNode, NameValueCollection collection)
        {
            if (collection == null)
                return;

            XmlNode node = xmlDoc.CreateElement(rootNode);

            AppendNodeForCollection(xmlDoc, node, collection);

            if(xmlDoc.FirstChild != null)
                xmlDoc.FirstChild.AppendChild(node);
            else
                xmlDoc.AppendChild(node);
        }

        public static void AppendNodeForCollection(XmlDocument xmlDoc, XmlNode rootNode, NameValueCollection collection)
        {
            if (collection == null)
                return;

            foreach (string key in collection)
            {
                string encodedKey = System.Web.HttpUtility.HtmlEncode(key);
                if (Char.IsDigit(encodedKey[0]))
                {
                    encodedKey = encodedKey.Insert(0, "N");
                }

                if (encodedKey.Contains("|"))
                    encodedKey = encodedKey.Replace("|", "");

                XmlNode node = xmlDoc.CreateElement(encodedKey.ToLower());
                node.InnerText = collection[key];

                rootNode.AppendChild(node);
            }
        }

        public static void AppendNodeForUrlString(XmlDocument xmlDoc, XmlNode queryString, string urlRedirect)
        {
            NameValueCollection collection = GetUrlQueryString(urlRedirect);
            AppendNodeForCollection(xmlDoc, queryString, collection);
        }

        public static object Parse(string value, System.Type destinationType)
        {
            if (destinationType.Equals(typeof(ulong)))
            {
                ulong ulongResult;
                if (!ulong.TryParse(value, out ulongResult))
                    return (ulong)0;

                return ulongResult;
            }
            else if (destinationType.Equals(typeof(uint)))
            {
                uint uintResult;
                if (!uint.TryParse(value, out uintResult))
                    return (uint)0;

                return uintResult;
            }
            if (destinationType.Equals(typeof(ushort)))
            {
                ushort ushortResult;
                if (!ushort.TryParse(value, out ushortResult))
                    return (ushort)0;

                return ushortResult;
            }
            else
                throw new ArgumentException();
        }

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
        
        public static int ParseInt(string str)
        {
            int result;
            if (!Int32.TryParse(str, out result))
            {
                result = int.MinValue;
            }
            return result;
        }
        public static ulong ParseLong(decimal value)
        {
            return (ulong)(ConvertDecimal(value, 2) * 100);
        }
        public static decimal ParseDecimal(string value)
        {
            value = value.Trim();
            if (value == null || value == "") return Decimal.MinValue;
            System.Globalization.NumberFormatInfo numberInfo = new System.Globalization.NumberFormatInfo();
            numberInfo.NumberDecimalSeparator = ".";
            bool isNegative = (value.Substring(0, 1) == "-");
            if (isNegative) value = value.Substring(1);
            value = value.PadLeft(3, '0');
            value = value.Insert(value.Length - 2, ".");
            if (isNegative) value = "-" + value;
            return Decimal.Parse(value, System.Globalization.NumberStyles.AllowDecimalPoint | System.Globalization.NumberStyles.AllowLeadingSign, numberInfo);
        }
        public static decimal ParseDecimal(ulong value, int decimalPlaces)
        {
            decimal k = (decimal)Math.Pow(10, (double)decimalPlaces);
            return (decimal)value / k;
        }
        public static decimal ParseDecimalLR(decimal lr)
        {
            int a = decimal.ToInt32(lr);

            if (a == int.MinValue)
                return decimal.MinValue;
            else if (a > 9999 || a < -9999)
                throw new Exception("Parâmetro de retorno Visa LR inválido.");
            else
                return lr;
        }
        public static string ParseString(string str)
        {
            if (str == null) return string.Empty;
            else return str;
        }
        public static string ParseString(decimal value)
        {
            if (value == decimal.MinValue) return "";
            if (value == 0) return "000";
            else return (ConvertDecimal(value, 2) * 100).ToString("G0");
        }
        public static decimal ConvertDecimal(decimal val, int decimalPlaces)
        {
            decimal k = (decimal)Math.Pow(10, (double)decimalPlaces);
            return Decimal.Floor(val * k) / k;
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
        public static bool TryParseGuid(string guidString, out Guid guid)
        {
            try
            {
                guid = new Guid(guidString);
            }
            catch
            {
                guid = Guid.Empty;
                return false;
            }
            return true;
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

        public static DStore CheckSessionStore(HttpContext context)
        {
            Ensure.IsNumericPage(context.Session["storeId"], "Loja inválida ou não informada");
            DStore dStore = DataFactory.Store().Locate(Int32.Parse(context.Session["storeId"].ToString()));
            Ensure.IsNotNullPage(dStore, "Loja inválida ou não informada");
            return dStore;
        }
        public static DConsumer CheckSessionConsumer(HttpContext context)
        {
            Ensure.IsNumericPage(context.Session["consumerId"], "Cliente inválido ou não informado");
            long consumerId = Int64.Parse(context.Session["consumerId"].ToString());
            DConsumer dConsumer = DataFactory.Consumer().Locate(consumerId);
            Ensure.IsNotNullPage(dConsumer, "Cliente {0} não encontrado", consumerId);
            return dConsumer;
        }
        public static DOrder CheckSessionOrder(HttpContext context)
        {
            long orderId = GetSessionOrderId(context);
            DOrder dOrder = DataFactory.Order().Locate(orderId);
            Ensure.IsNotNullPage(dOrder, "Pedido {0} não encontrado", orderId);
            return dOrder;
        }
        public static long GetSessionOrderId(HttpContext context)
        {
            Ensure.IsNumericPage(context.Session["orderId"], "Pedido inválido ou não informado");
            return Convert.ToInt64(context.Session["orderId"]);
        }
        public static int GetPaymentFormSession(HttpContext context)
        {
            int paymentFormId;
            if (context.Session["paymentFormId"] == null) return int.MinValue;

            if (Int32.TryParse(context.Session["paymentFormId"].ToString(), out paymentFormId))
            {
                return paymentFormId;
            }
            else
            {
                return int.MinValue;
            }
        }
        public static void SetPaymentFormSession(HttpContext context, int paymentFormId)
        {
            context.Session["paymentFormId"] = paymentFormId;
        }
        public static int GetPaymentGroupSession(HttpContext context)
        {
            int paymentFormGroupId;
            if (context.Session["paymentFormGroupId"] == null) return int.MinValue;

            if (Int32.TryParse(context.Session["paymentFormGroupId"].ToString(), out paymentFormGroupId))
            {
                return paymentFormGroupId;
            }
            else
            {
                return int.MinValue;
            }
        }
        public static void SetPaymentGroupSession(HttpContext context, int paymentFormGroupId)
        {
            context.Session["paymentFormGroupId"] = paymentFormGroupId;
        }
        public static bool GetCanChoosePaymentFormSession(HttpContext context)
        {
            bool canChoose;
            if (context.Session["canChoosePaymentForm"] == null) return false;

            bool.TryParse(context.Session["canChoosePaymentForm"].ToString(), out canChoose);

            return canChoose;
        }
        public static int GetInstallmentNumber(HttpContext context)
        {
            int installmentNumber;
            if (context.Session["installmentNumber"] == null ||
                !int.TryParse(context.Session["installmentNumber"].ToString(), out installmentNumber))
            {
                return int.MinValue;
            }
            return installmentNumber;
        }
        public static int GetHandshakeInstallmentNumber(HttpContext context)
        {
            int installmentNumber;
            if (context.Session["handshakeInstallmentNumber"] == null ||
                !int.TryParse(context.Session["handshakeInstallmentNumber"].ToString(), out installmentNumber))
            {
                return int.MinValue;
            }
            return installmentNumber;
        }
        public static void SetCanChoosePaymentFormSession(HttpContext context, bool canChoose)
        {
            context.Session["canChoosePaymentForm"] = canChoose.ToString();
        }
        public static int GetHandshakePaymentFormSession(HttpContext context, out bool isGroup)
        {
            isGroup = false;
            int paymentFormId;
            if (context.Session["handshakePaymentFormId"] == null) return int.MinValue;

            if (context.Session["handshakePaymentFormId"].ToString() == PaymentGroupsWord.CreditCard)
            {
                isGroup = true;
                return (int)PaymentGroups.CreditCard;
            }

            if (Int32.TryParse(context.Session["handshakePaymentFormId"].ToString(), out paymentFormId))
            {
                return paymentFormId;
            }
            else
            {
                return int.MinValue;
            }
        }
        public static void SetHandshakePaymentFormSession(HttpContext context, string paymentFormId)
        {
            context.Session["handshakePaymentFormId"] = paymentFormId;
        }
        public static void SetInstallmentNumber(HttpContext context, int installmentNumber)
        {
            context.Session["installmentNumber"] = installmentNumber;
        }
        public static void SetHandshakeInstallmentNumber(HttpContext context, int installmentNumber)
        {
            context.Session["handshakeInstallmentNumber"] = installmentNumber;
        }
        public static string GetPaymentAgentWebPage(int paymentAgentId, int storeId)
        {
            DPaymentAgent paymentAgent = DataFactory.PaymentAgent().Locate(paymentAgentId);
            Ensure.IsNotNull(paymentAgent, "A loja {0} não possui o agente de pagamento {1} configurado.", new object[] { storeId, paymentAgentId });
            return paymentAgent.webPage;
        }
        public static int GetPaymentAgentId(int paymentFormId)
        {
            DPaymentForm paymentForm = DataFactory.PaymentForm().Locate(paymentFormId);
            Ensure.IsNotNull(paymentForm, "A forma de pagamento selecionada não possui um agente de pagamento válido.");
            return paymentForm.paymentAgentId;
        }
        public static int GetPaymentAgentSetupId(int storeId, int paymentFormId)
        {
            DStorePaymentForm paymentForm = DataFactory.StorePaymentForm().Locate(storeId, paymentFormId);
            Ensure.IsNotNull(paymentForm, "A loja {0} não possui o meio de pagamento {1} configurado.", new object[] { storeId, paymentFormId });
            return paymentForm.paymentAgentSetupId;
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
        public static void SetPaymentAttemptSession(HttpContext context, List<Guid> guid)
        {
            context.Session["PaymentAttemptId"] = guid;
        }
        public static void SetPaymentAttemptSession(HttpContext context, Guid guid)
        {
            context.Session["PaymentAttemptId"] = guid;
        }
        public static Guid GetPaymentAttemptSession(HttpContext context)
        {
            Guid paymentAttemptId;
            if (context.Session["PaymentAttemptId"] == null) return Guid.Empty;
            if (!GenericHelper.TryParseGuid(context.Session["PaymentAttemptId"].ToString(), out paymentAttemptId))
                return Guid.Empty;
            return paymentAttemptId;
        }
        public static void SetCreditCardXmlSession(HttpContext context, string xml)
        {
            context.Session["CreditCardXml"] = xml;
        }
        public static string GetCreditCardXmlSession(HttpContext context)
        {
            if (context.Session["CreditCardXml"] == null) return string.Empty;
            return context.Session["CreditCardXml"].ToString();
        }
        public static CreditCardInformation GetCreditCardInformation()
        {
            string xml = GenericHelper.GetCreditCardXmlSession(HttpContext.Current);
            Ensure.IsNotNullOrEmptyPage(xml, "Sessão inválida para os dados do cartão de uma transação Moset");
            CreditCardInformation cardinfo = GenericHelper.GetCreditCardXml(xml);
            Ensure.IsNotNullPage(cardinfo, "Dados inválidos do cartão para uma transação Moset");
            return cardinfo;
        }

        public static int GetOrderStatus(HttpContext context)
        {
            DOrder order = CheckSessionOrder(context);
            DWorkflowOrderStatus[] dWorkFlowOrderStatusList = DataFactory.WorkflowOrderStatus().ListSortedByDate(order.orderId);
            Ensure.IsNotNull(dWorkFlowOrderStatusList, "Não foi possível resgatar status do pedido {0}", order.orderId);
            return dWorkFlowOrderStatusList[0].status;
        }
        public static void SetOrderStatus(HttpContext context, WorkflowOrderStatus orderStatus, string text)
        {
            DWorkflowOrderStatus dWorkFlowOrderStatus = new DWorkflowOrderStatus();
            dWorkFlowOrderStatus.creationDate = DateTime.Now;
            dWorkFlowOrderStatus.orderId = GetSessionOrderId(context);
            dWorkFlowOrderStatus.status = (int)orderStatus;
            if(!String.IsNullOrEmpty(text))
                dWorkFlowOrderStatus.text = (text.Length>300?text.Substring(0, 299):text);
            DataFactory.WorkflowOrderStatus().Insert(dWorkFlowOrderStatus);
        }

        public static void RedirectWorkflow(HttpContext context)
        {
            switch ((WorkflowOrderStatus)GetOrderStatus(context))
            {
                case WorkflowOrderStatus.HandshakeFinished:
                    context.Response.Redirect("~/fillconsumer.aspx");
                    break;
                case WorkflowOrderStatus.ConsumerFilled:
                case WorkflowOrderStatus.PaymentFormChoosed:
                case WorkflowOrderStatus.InstallmentChoosed:
                    context.Response.Redirect("~/payment.aspx");
                    break;
                case WorkflowOrderStatus.Finished:
                    RedirectToErrorPage("Este pedido já está finalizado.");
                    break;
            }
        }

        public static bool IsBoleto(int paymentFormId)
        {
            switch(paymentFormId)
            {
                case (int)PaymentForms.BoletoBancoDoBrasil:
                case (int)PaymentForms.BoletoBradesco:
                case (int)PaymentForms.BoletoItau:
                case (int)PaymentForms.BoletoHSBC:
                //TODO:mudança ponto cred
                case (int)PaymentForms.BoletoInvestCred:
                    return true;
            }
            return false;
        }

        public static bool HasInstallmentAttempts(int paymentFormId)
        {
            switch (paymentFormId)
            {
                case (int)PaymentForms.BoletoBancoDoBrasil:
                case (int)PaymentForms.BoletoBradesco:
                case (int)PaymentForms.BoletoItau:
                case (int)PaymentForms.BoletoHSBC:
                case (int)PaymentForms.DepositoIdentificadoBradesco:
                    return true;
            }
            return false;
        }

        public static void RefillSessionByAttempt(Guid paymentAttemptId)
        {
            DPaymentAttempt attempt = DataFactory.PaymentAttempt().Locate(paymentAttemptId);
            Ensure.IsNotNullPage(attempt, "Falha ao resgatar sessão de pagamento.");
            DOrder order = DataFactory.Order().Locate(attempt.orderId);

            HttpContext.Current.Session["PaymentAttemptId"] = paymentAttemptId;
            HttpContext.Current.Session["orderId"] = attempt.orderId;
            HttpContext.Current.Session["storeId"] = order.storeId;
        }

        public static void LogFile(string msg, LogFileEntryType type)
        {
            lock (typeof(ClassForLock))
            {
                StreamWriter sw = null;
                String path = "", name = "", file = "", stype = "";

                try
                {
                    path = System.Configuration.ConfigurationManager.AppSettings["LogPath"];
                    if (path == null || path.Trim() == "")
                        return;

                    name = System.Configuration.ConfigurationManager.AppSettings["LogName"];
                    if (name == null || name.Trim() == "")
                        return;

                    path = (path.EndsWith("\\") ? path : path + "\\");
                    file = name + DateTime.Now.ToString("yyyyMMdd") + ".log";
                    stype = (type==LogFileEntryType.Error?"Error":(type==LogFileEntryType.Information?"Information":"Warning"));
                    
                    if (!Directory.Exists(path))
                        Directory.CreateDirectory(path);

                    sw = File.AppendText(path + file);
                    sw.WriteLine(DateTime.Now.ToString("HH:mm:ss.fff") + "\t" + type + "\t" + (stype.Equals("Error") || stype.Equals("Warning") ? "\t" : "") + msg);
                    sw.Flush();
                }
                catch (Exception e)
                {
                    try
                    {
                        System.Diagnostics.EventLog.WriteEntry("EasyPagObject", "GenericHelper::GenericHelper.GravaLogArquivo Erro tentando gravar arquivo de log: " + e.Message + " mensagem original: " + msg, System.Diagnostics.EventLogEntryType.Error);
                    }
                    catch
                    {
                    }
                }
                finally
                {
                    if (sw != null)
                        sw.Close();
                }
            }
        }

        public static bool UseTestValuesForAgents(int storeId, int paymentFormId)
        {
            //Pego a DStorePaymentForm para checar se usa valor de teste ou nao
            DStorePaymentForm storePaymentForm = DataFactory.StorePaymentForm().Locate(storeId, paymentFormId);
            Ensure.IsNotNull(storePaymentForm, "Não foi possível entrar a configuração do meio de pagamento {0} para a loja {1}", storeId, paymentFormId);

            return storePaymentForm.useTestValues;
        }

        public static decimal GetTestValueForAgent(int paymentFormId, int installmentQuantity)
        {
            switch (paymentFormId)
            {
                case (int)PaymentForms.VisaVBV:
                case (int)PaymentForms.VisaVBVInBox:
                    if (installmentQuantity > 1)
                        return installmentQuantity * 5M;
                    else
                        return 1M;
                default:
                    return 0.01M;
            }
        }

        public static string CalculateHash(string text)
        {
            ASCIIEncoding ascii = new ASCIIEncoding();
            byte[] plain = ascii.GetBytes(text);

            SHA1 sha = new SHA1CryptoServiceProvider();
            byte[] cipher = sha.ComputeHash(plain);

            string cipherText = "";
            foreach (byte b in cipher)
                cipherText += String.Format("{0:X2}", b);

            return cipherText;
        }
        
        public static void CloseWindow()
        {
            if (HttpContext.Current == null)
                return;

            StringBuilder response = new StringBuilder();
            response.Append("<html><head></head><body>\n");
            response.Append("<script language=\"JavaScript\">parent.close();</script>\n");
            response.Append("</body></html>\n");

            HttpContext.Current.Response.Clear();
            HttpContext.Current.Response.Write(response.ToString());
            HttpContext.Current.Response.End();
        }

        public static void RedirectWindow(string url, bool endResponse)
        {
            if (HttpContext.Current == null)
                return;

            StringBuilder response = new StringBuilder();
            response.Append("<html><head></head><body>\n");
            response.Append("<script language=\"JavaScript\">top.window.location.href='" + url + "';</script>\n");
            response.Append("</body></html>\n");

            HttpContext.Current.Response.Clear();
            HttpContext.Current.Response.Write(response.ToString());
            if(endResponse)
                HttpContext.Current.Response.End();
        }
        
        public static string GetCompanyName(long orderId)
        {
            DHandshakeSession[] handshakeSessions = DataFactory.HandshakeSession().List(orderId);
            if (Ensure.IsNull(handshakeSessions))
                return "";

            string xmlData = "";

            DHandshakeSessionLog[] hsLogs = DataFactory.HandshakeSessionLog().List(handshakeSessions[0].handshakeSessionId);

            foreach (DHandshakeSessionLog hsLog in hsLogs)
            {
                if (hsLog.step == 3)
                    xmlData = hsLog.xmlData;
            }

            return GenericHelper.GetSingleNodeString(xmlData, "/root/form/razao_pj");
        }
        
        public static string CreateCreditCardXml(string cardName, string cardNumber, string cardSecurity, DateTime cardExpiration)
        {
            XmlDocument xml = new XmlDocument();
            XmlElement root = xml.CreateElement("CreditCardInfo");
            xml.AppendChild(root);
            XmlElement elem = xml.CreateElement("Name");
            elem.InnerText = cardName;
            root.AppendChild(elem);
            elem = xml.CreateElement("Number");
            elem.InnerText = cardNumber;
            root.AppendChild(elem);
            elem = xml.CreateElement("SecurityNumber");
            elem.InnerText = cardSecurity;
            root.AppendChild(elem);
            elem = xml.CreateElement("ExpirationDate");
            elem.InnerText = cardExpiration.ToString("s");
            root.AppendChild(elem);

            return xml.OuterXml;
        }

        public static CreditCardInformation GetCreditCardXml(string xml)
        {
            CreditCardInformation cardinfo = new CreditCardInformation();
            cardinfo.Name = GetSingleNodeString(xml, "/CreditCardInfo/Name");
            cardinfo.Number = GetSingleNodeString(xml, "/CreditCardInfo/Number");
            cardinfo.SecurityNumber = GetSingleNodeString(xml, "/CreditCardInfo/SecurityNumber");
            DateTime expiration;
            if (DateTime.TryParse(GetSingleNodeString(xml, "/CreditCardInfo/ExpirationDate"), out expiration))
                cardinfo.ExpirationDate = expiration;

            //valida os dados retirados do xml
            if (!Ensure.IsNumeric64(cardinfo.Number))
                return null;
            if (!Ensure.IsNumeric(cardinfo.SecurityNumber))
                return null;
            if (!Ensure.IsNotNullOrEmpty(cardinfo.Name))
                return null;
            if (cardinfo.ExpirationDate == DateTime.MinValue)
                return null;

            return cardinfo;
        }

        public static RedirectInformation GetRedirectXml(string xml)
        {
            RedirectInformation redirectinfo = new RedirectInformation();

            if (String.IsNullOrEmpty(xml))
                return redirectinfo;

            if(xml.ToLower().Contains("<retorno>ok</retorno"))
                redirectinfo.Return = "ok";

            if(xml.ToLower().Contains("<redirect>ok</redirect>"))
            {
                redirectinfo.Redirect = "ok";

                int urlRedirectIniPos = xml.ToLower().IndexOf("<urlredirect>");
                int urlRedirectEndPos = xml.ToLower().IndexOf("</urlredirect>");
                if (urlRedirectIniPos > 0 && urlRedirectEndPos > urlRedirectIniPos)
                    redirectinfo.UrlRedirect = xml.Substring(urlRedirectIniPos + "<urlredirect>".Length, urlRedirectEndPos - (urlRedirectIniPos + "<urlredirect>".Length));
            }

            #region oldCode
            //XmlDocument xmlDoc = new XmlDocument();

            //try
            //{
            //    xmlDoc.LoadXml(xml);
            //}
            //catch (Exception)
            //{
            //    try
            //    {
            //        // se der problema na validação do xml força-se a criação de um elemento root
            //        // pois o xml pode estar formatado corretamente com exceção do elemento da raiz
            //        xml = "<root>" + xml + "</root>";
            //        xmlDoc.LoadXml(xml);
            //    }
            //    catch (Exception)
            //    {
            //        return redirectinfo;
            //    }
            //}

            //redirectinfo.Return = GetSingleNodeString(xmlDoc, "//retorno");
            //redirectinfo.Redirect = GetSingleNodeString(xmlDoc, "//redirect");
            //redirectinfo.UrlRedirect = GetSingleNodeString(xmlDoc, "//urlredirect"); 
            #endregion

            return redirectinfo;
        }

        public static XmlElement CopyElementToName (XmlElement element, string tagName)
        {
            XmlElement newElement = element.OwnerDocument.CreateElement(tagName);
            
            for (int i = 0; i < element.Attributes.Count; i++)
                newElement.SetAttributeNode((XmlAttribute)element.Attributes[i].CloneNode(true));
            
            for (int i = 0; i < element.ChildNodes.Count; i++)
                newElement.AppendChild(element.ChildNodes[i].CloneNode(true));

            return newElement;
        }

        public static DPaymentAttempt ChooseAttemptByStatus(DPaymentAttempt[] arrPaymentAttempt)
        {
            if (Ensure.IsNull(arrPaymentAttempt))
                return null;

            //Regra de negocio: escolhe uma entre varias attempts
            //segundo a precedencia abaixo, da direita para a esquerda
            //(a da direita tem maior precedencia)
            List<int> precedence = new List<int>(new int[] { (int)PaymentAttemptStatus.Pending, (int)PaymentAttemptStatus.NotPaid, (int)PaymentAttemptStatus.Canceled, (int)PaymentAttemptStatus.PendingPaid, (int)PaymentAttemptStatus.Paid });

            DPaymentAttempt attemptCurrent = arrPaymentAttempt[0];
            for (int i = 0; i < arrPaymentAttempt.Length; i++)
                if ((precedence.IndexOf(arrPaymentAttempt[i].status) > precedence.IndexOf(attemptCurrent.status)) ||
                     (arrPaymentAttempt[i].status == attemptCurrent.status && arrPaymentAttempt[i].lastUpdate > attemptCurrent.lastUpdate))
                    attemptCurrent = arrPaymentAttempt[i];

            return attemptCurrent;
        }

        public static string ReplaceStringWithParams(string originalString, NameValueCollection replaceParams, string pattern)
        {
            if (String.IsNullOrEmpty(originalString))
                return originalString;

            foreach (string replaceParam in replaceParams)
                originalString = originalString.Replace(String.Format(pattern, replaceParam), replaceParams[replaceParam]);

            return originalString;
        }
        
        public static void UpdateOrderStatusByAttemptStatus(DOrder order, int attemptStatus)
        {
            //retira o status de nao concluido da Order
            switch ((PaymentAttemptStatus)attemptStatus)
            {
                case PaymentAttemptStatus.Paid:
                    if (order.status != (int)OrderStatus.Approved && order.status != (int)OrderStatus.Cancelled)
                        order.status = (int)OrderStatus.Analysing;
                    break;
                case PaymentAttemptStatus.PendingPaid:
                    if (order.status != (int)OrderStatus.Analysing &&
                        order.status != (int)OrderStatus.Approved & order.status != (int)OrderStatus.Cancelled)
                        order.status = (int)OrderStatus.PendingPaid;
                    break;
                case PaymentAttemptStatus.NotPaid:
                    if (order.status != (int)OrderStatus.Analysing && order.status != (int)OrderStatus.PendingPaid &&
                        order.status != (int)OrderStatus.Approved && order.status != (int)OrderStatus.Cancelled)
                        order.status = (int)OrderStatus.NotPaid;
                    break;
                case PaymentAttemptStatus.Delivered:
                    order.status = (int)OrderStatus.Delivered;
                    break;
            }
            order.lastUpdateDate = DateTime.Now;
            DataFactory.Order().Update(order);
        }
    }

    internal static class ClassForLock
    {
    }
}
