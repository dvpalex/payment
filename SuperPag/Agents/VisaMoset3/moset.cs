using System;
using System.Collections.Generic;
using System.Text;
using SuperPag.Helper;
using SuperPag.Helper.ParseHTML;
using SuperPag.Data.Messages;
using SuperPag.Agents.VisaMoset3.Messages;
using System.Xml.Serialization;
using System.IO;

namespace SuperPag.Agents.VisaMoset3
{
    public class Moset
    {
        private int _cod = int.MinValue;
        private int _lr = int.MinValue;
        private string _tid;
        private string _free;
        private string _ars;
        private string _cap;
        private string _msgret;
        private string _msgretcapture;

        public int Cod { get { return _cod; } }
        public int Lr { get { return _lr; } }
        public string Tid { get { return _tid; } }
        public string Free { get { return _free; } }
        public string Ars { get { return _ars; } }
        public string Cap { get { return _cap; } }
        public string Msgret { get { return _msgret; } }
        public string MsgretCapture { get { return _msgretcapture; } }

        public bool ProcessPayment(string urlComponenteSet, long orderId, string storeReferenceOrder, string tid, decimal price, string creditCardNumber, DateTime crediCardExpiration, string merchid, string cfg, string paymentAttemptId, string cvv2)
        {
            ServerHttpHtmlRequisition a = new ServerHttpHtmlRequisition();
            a.Url = ((urlComponenteSet.EndsWith("/")) ? urlComponenteSet : urlComponenteSet + "/") + "authorize.exe";
            a.Method = "POST";
            a.Parameters.Add("tid", tid);
            a.Parameters.Add("order", "Pedido no. " + storeReferenceOrder + ": Itens: produtos - " + GenericHelper.FormatCurrencyBrasil(price / 100));
            a.Parameters.Add("orderid", orderId.ToString());
            a.Parameters.Add("price", Helper.GenericHelper.ParseString(price));
            a.Parameters.Add("ccn", creditCardNumber);
            a.Parameters.Add("exp", crediCardExpiration.ToString("yyMM"));
            a.Parameters.Add("cvv2", cvv2);            
            a.Parameters.Add("merchid", cfg);                   
            a.Parameters.Add("free", paymentAttemptId);
            
            if (!a.Send("iso-8859-1"))
            {
                _lr = -1;
                _msgret = a.Response;

                return false;
            }

            VBV3AuthorizeReturn authorizeReturn = null;
            try
            {
                XmlSerializer deserializer = new XmlSerializer(typeof(VBV3AuthorizeReturn));
                authorizeReturn = (VBV3AuthorizeReturn)deserializer.Deserialize(new StringReader(a.Response));
                GenericHelper.LogFile(a.Response, LogFileEntryType.Warning);

            }
            catch (Exception ex)
            {
                string message = "SuperPag.Agents.VisaMoset3::Moset::ProcessPayment ";
                message += "tid=" + tid;
                message += " exception=" + ex.Message + " stack=" + ex.StackTrace;
                message += " retorno=" + a.Response;
                GenericHelper.LogFile(message, LogFileEntryType.Error);
                
                _lr = -2;
                _msgret = a.Response;
                return false;
            }

            this._tid = authorizeReturn.tid;
            this._lr = int.Parse(authorizeReturn.lr);
            this._ars = authorizeReturn.ars;
            this._free = authorizeReturn.free;

            return true;
        }

        public static string GetPaymentProcessResponseDescription(int _lr)
        {
            string response = "Venda não autorizada. Motivo não determinado";
            switch (_lr)
            {
                case 0:
                case 11:
                    response = "Aprovado";
                    break;
                case 1:
                case 2:
                case 3:
                case 12:
                case 21:
                case 22:
                case 25:
                case 28:
                case 54:
                case 57:
                case 62:
                case 63:
                case 76:
                case 77:
                case 80:
                case 93:
                    response = "Venda não autorizada";
                    break;
                case 4:
                case 5:
                case 7:
                case 14:
                case 41:
                case 43:
                case 51:
                case 52:
                case 53:
                case 65:
                    response = "Venda não autorizada. Contate o emissor do seu cartão";
                    break;
                case 6:
                case 9:
                case 55:
                case 81:
                case 82:
                case 83:
                case 86:
                    response = "Por favor, refaça a transação";
                    break;
                case 61:
                    response = "Compra não autorizada. O limite do seu cartão VISA foi excedido";
                    break;
                case 15:
                case 91:
                case 98:
                case 99:
                    response = "Sistema do Banco temporariamente fora de operação, por favor tente mais tarde";
                    break;
            }

            return response;
        }

        public static decimal GetCapturedValue(string _cap)
        {
            if (_cap == null || _cap == string.Empty)
                return decimal.MinValue;

            string[] values = _cap.Split(',');

            if (values == null || values.Length < 2)
                return int.MinValue;
            
            decimal valor = Helper.GenericHelper.ParseDecimal(values[1]);

            return valor;
        }

        public static int GetCapturedCurrency(string _cap)
        {
            if (_cap == null || _cap == string.Empty)
                return int.MinValue;

            string[] values = _cap.Split(',');

            if (values == null || values.Length == 0)
                return int.MinValue;
            
            int currency = Helper.GenericHelper.ParseInt(values[0]);

            return currency;
        }

        public static string GenerateTID(string businessNumber, int paymentFormId, int installmentQuantity, byte installmentType)
        {
            string tid = "";
            DateTime tidDate = DateTime.Now;
            tid = businessNumber.ToString().PadLeft(10, '0').Substring(4, 5);
            tid += tidDate.Year.ToString("0000").Substring(3, 1);
            tid += tidDate.DayOfYear.ToString("000");
            tid += tidDate.ToString("hhmmss");
            tid += tidDate.Millisecond.ToString().Substring(0, 1);
            if (paymentFormId == (int)PaymentForms.VisaElectronVBV3)
            {
                if (installmentQuantity != 1)
                    Ensure.IsNotNullPage(null, "Parcelamento inválido para o meio de pagamento Visa Electron.");
                tid += "A001"; // Cartões VISA ELECTRON

            }
            else
            {
                if (installmentQuantity == 1)
                    tid += "1001"; // Cartões VISA à vista
                else
                {
                    //Enviar tipo de pagamento especifico
                    tid += (installmentType == (byte)InstallmentType.Emissor ? "3" : "2") + installmentQuantity.ToString("000");
                }
            }
            return tid;
        }

        public static void BuildRequestXml(string tid, string price)
        {
            System.IO.FileStream fs = new System.IO.FileStream(System.Configuration.ConfigurationManager.AppSettings["VBV3Directory"].ToString() + "\\requests\\" + tid + ".xml", System.IO.FileMode.Create);
            try
            {
                StringBuilder tidfile = new StringBuilder();
                tidfile.Append("<MESSAGE>");
                tidfile.AppendFormat("<PRICE>{0}</PRICE>", price);
                tidfile.Append("<AUTHENTTYPE>0</AUTHENTTYPE>");
                tidfile.Append("</MESSAGE>");
                byte[] XML = System.Text.Encoding.ASCII.GetBytes(tidfile.ToString());
                fs.Write(XML, 0, XML.Length);
            }
            finally
            {
                fs.Close();
            }
        }
    }
}
