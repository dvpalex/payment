using System;
using System.Collections.Generic;
using System.Text;
using SuperPag.Helper;
using SuperPag.Helper.ParseHTML;
using SuperPag.Data.Messages;

namespace SuperPag.Agents.VisaMoset
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
        private string _arp;



        public int Cod { get { return _cod; } }
        public int Lr { get { return _lr; } }
        public string Tid { get { return _tid; } }
        public string Free { get { return _free; } }
        public string Ars { get { return _ars; } }
        public string Cap { get { return _cap; } }
        public string Msgret { get { return _msgret; } }
        public string MsgretCapture { get { return _msgretcapture; } }
        public string Arp
        {
            get { return _arp; }
            set { _arp = value; }
        }

        public bool ProcessPayment(string urlComponenteSet, long orderId, string tid, decimal price, string creditCardNumber, DateTime crediCardExpiration, string merchid, string cfg, string paymentAttemptId)
        {
            ServerHttpHtmlRequisition a = new ServerHttpHtmlRequisition();
            a.Url = ((urlComponenteSet.EndsWith("/")) ? urlComponenteSet : urlComponenteSet + "/") + "purchssl.exe";
            a.Method = "POST";
            a.Parameters.Add("order", orderId.ToString());
            a.Parameters.Add("tid", tid);
            a.Parameters.Add("price", Helper.GenericHelper.ParseString(price));
            a.Parameters.Add("ccn", creditCardNumber);
            a.Parameters.Add("exp", crediCardExpiration.ToString("yyyyMM"));
            a.Parameters.Add("merchid", cfg);
            a.Parameters.Add("free", paymentAttemptId);
            
            if (!a.Send())
            {
                _lr = -1;
                _msgret = a.Response;
                return false;
            }

            _msgret = a.Response;

            string page = a.Response;

            StringBuilder resp = new StringBuilder();

            ParseHTML parse = new ParseHTML();
            parse.Source = page;

            while (!parse.Eof())
            {
                char ch = parse.Parse();
                if (ch == 0)
                {
                    AttributeList tag = parse.GetTag();
                    if (tag.Name == "input")
                    {
                        if (tag["name"] != null)
                        {
                            switch (tag["name"].Value)
                            {
                                case "lr":
                                    int val;
                                    if (int.TryParse(tag["value"].Value, out val))
                                        _lr = val;
                                    break;
                                case "tid":
                                    _tid = tag["value"].Value;
                                    break;
                                case "free":
                                    _free = tag["value"].Value;
                                    break;
                                case "arp":
                                    _arp = tag["value"].Value;
                                    break;
                            }
                        }
                    }
                }
            }

            return true;
        }

        public bool Capture(string urlComponenteSet, string tid, string cfg)
        {
            ServerHttpHtmlRequisition a = new ServerHttpHtmlRequisition();
            a.Url = ((urlComponenteSet.EndsWith("/")) ? urlComponenteSet : urlComponenteSet + "/") + "capture.exe";
            a.Method = "POST";
            a.Parameters.Add("tid", tid);
            a.Parameters.Add("merchid", cfg);

            if (!a.Send())
            {
                _cod = -1;
                _msgretcapture = a.Response;
                return false;
            }

            _msgretcapture = a.Response;

            string page = a.Response;

            StringBuilder resp = new StringBuilder();

            ParseHTML parse = new ParseHTML();
            parse.Source = page;

            while (!parse.Eof())
            {
                char ch = parse.Parse();
                if (ch == 0)
                {
                    AttributeList tag = parse.GetTag();
                    if (tag.Name == "input")
                    {
                        if (tag["name"] != null)
                        {
                            switch (tag["name"].Value)
                            {
                                case "cod":
                                    int val;
                                    if (int.TryParse(tag["value"].Value, out val))
                                        _cod = val;
                                    break;
                                case "ars":
                                    _ars = tag["value"].Value;
                                    break;
                                case "free":
                                    _free = tag["value"].Value;
                                    break;
                                case "tid":
                                    _tid = tag["value"].Value;
                                    break;
                                case "cap":
                                    _cap = tag["value"].Value;
                                    break;
                            }
                        }
                    }
                }
            }

            return true;
        }

        public string GetPaymentProcessResponseDescription()
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

        public string GetCaptureResponseDescription()
        {
            string response = "Captura não realizada. Motivo não determinado";
            switch (_cod)
            {
                case 0:
                    response = "Capturado com sucesso";
                    break;
                case 1:
                    response = "Autorização negada";
                    break;
                case 2:
                    response = "Falha na captura";
                    break;
                case 3:
                    response = "Captura já efetuada";
                    break;
                case 98:
                    response = "Time-Out";
                    break;
                case 99:
                    response = "Erro generico entrar em contato com suporteweb@visanet.com.br";
                    break;
                case 108:
                    response = "Tentar novamente - Falha de comunicação entre o seu WebServer e o servidor de POS da VISANET";
                    break;
                case 112:
                    response = "Tid inexistente / Tentativa de Captura excedeu o limite de 5 dias  (dia da compra + 5) / Gateway da VisaNet fora de operação";
                    break;
                case 114:
                    response = "Falha de comunicação";
                    break;

            }

            return response;
        }

        public decimal GetCapturedValue()
        {
            if (_cap == null || _cap == string.Empty)
                return decimal.MinValue;

            string[] values = _cap.Split(',');

            if (values == null || values.Length < 2)
                return int.MinValue;
            
            decimal valor = Helper.GenericHelper.ParseDecimal(values[1]);

            return valor;
        }

        public int GetCapturedCurrency()
        {
            if (_cap == null || _cap == string.Empty)
                return int.MinValue;

            string[] values = _cap.Split(',');

            if (values == null || values.Length == 0)
                return int.MinValue;
            
            int currency = Helper.GenericHelper.ParseInt(values[0]);

            return currency;
        }
    }
}
