using System;
using System.Collections.Specialized;
using System.Web;
using System.Xml;
using System.Xml.Schema;
using System.Collections.Generic;
using System.Text;
using SuperPag.Handshake;
using SuperPag.Data;
using SuperPag.Data.Messages;
using SuperPag.Data.Interfaces;
using SuperPag.Helper;
using System.Threading;

namespace SuperPag.Handshake.Xml
{
    public class Handshake
    {
        Helper helper;

        private HttpContext _context;
        public HttpContext Context
        {
            get { return _context; }
            set { _context = value; }
        }

        public static Handshake Instance()
        {
            return new Handshake();
        }
        public Handshake()
        {
            if(HttpContext.Current != null)
                this.Context = HttpContext.Current;

            helper = new Helper();
        }

        public void Step1(string storeKey, string storeReferenceOrder)
        {
            if (storeKey == null || storeKey == string.Empty)
                GenericHelper.RedirectToErrorPage("Codigo de loja inválido");

            //obtem a loja
            DStore dStore = DataFactory.Store().Locate(storeKey);
            Ensure.IsNotNullPage(dStore, "A chave de loja {0} é invalida", storeKey);

            //crio sessao de handshake
            Guid handshakeSessionId = Guid.NewGuid();
            DHandshakeSession dHandshakeSession = new DHandshakeSession();
            dHandshakeSession.handshakeSessionId = handshakeSessionId;
            dHandshakeSession.storeId = dStore.storeId;
            dHandshakeSession.orderId = long.MinValue;
            dHandshakeSession.handshakeType = (int)HandshakeType.XmlSPag10;
            dHandshakeSession.createDate = DateTime.Now;
            DataFactory.HandshakeSession().Insert(dHandshakeSession);

            //salva os dados recebidos no log de handshake
            DHandshakeSessionLog dHandshakeLog = new DHandshakeSessionLog();
            dHandshakeLog.createDate = DateTime.Now;
            dHandshakeLog.handshakeSessionId = handshakeSessionId;
            dHandshakeLog.step = 1;
            dHandshakeLog.url = Context.Request.RawUrl;
            dHandshakeLog.xmlData = helper.CreateXmlForRequest();
            DataFactory.HandshakeSessionLog().Insert(dHandshakeLog);

            //obtem os dados do handshake
            DHandshakeConfiguration dHandshakeConfiguration = DataFactory.HandshakeConfiguration().Locate(dStore.storeId, (int)HandshakeType.XmlSPag10);
            Ensure.IsNotNullPage(dHandshakeConfiguration, "A loja {0} não está configurada para utilizar esse tipo Handshake", dStore.storeId);

            //obtem os dados do handshake de html
            DHandshakeConfigurationXml dHandshakeConfigurationXml = DataFactory.HandshakeConfigurationXml().Locate(dHandshakeConfiguration.handshakeConfigurationId);
            Ensure.IsNotNullPage(dHandshakeConfigurationXml, "A loja {0} não está configurada corretamente para utilizar esse tipo Handshake", dStore.storeId);

            //Regra de negocio: se a configuração do cliente não aceitar pedidos duplicados
            //verifica se ja existe alguma tentativa de pagamento como o STATUS de PAGO para
            //esse pedido de cliente
            if(!dHandshakeConfiguration.acceptDuplicateOrder && SuperPag.Handshake.Helper.StoreReferenceOrderIsPaid(dStore.storeId, storeReferenceOrder))
                GenericHelper.RedirectToErrorPage("O pedido {0} já consta como pago no sistema. Para evitar qualquer transtorno com o seu pagamento, contate a loja em que efetuou o pedido para maiores esclarecimentos", storeReferenceOrder);

            //monto a url de redirect para o servidor da loja
            string urlRedirect = string.Format("{0}?36948FFEF212F5E4={1}&91D4C3128BF7DA7F={2}", dHandshakeConfigurationXml.urlHandshake.Trim(), handshakeSessionId, storeReferenceOrder);

            //salva os dados de envio no log de handshake
            dHandshakeLog = new DHandshakeSessionLog();
            dHandshakeLog.createDate = DateTime.Now;
            dHandshakeLog.handshakeSessionId = handshakeSessionId;
            dHandshakeLog.step = 2;
            dHandshakeLog.url = urlRedirect;
            dHandshakeLog.xmlData = helper.CreateXmlForResponse(urlRedirect);
            DataFactory.HandshakeSessionLog().Insert(dHandshakeLog);

            //coloco a chave de acesso da loja na session
            Context.Session["storeKey"] = storeKey;

            //salva o codigo do pedido na session
            Context.Session["storeReferenceOrder"] = storeReferenceOrder;

            //salva o código de sessao na session
            Context.Session["handshakeSessionId"] = handshakeSessionId;

            //salva o storeId na session
            Context.Session["storeId"] = dStore.storeId;
          
            //Envia Post para a URL de Handshake da Loja e captura XML
            NameValueCollection parameters = new NameValueCollection();
            parameters.Add("36948FFEF212F5E4", handshakeSessionId.ToString());
            parameters.Add("91D4C3128BF7DA7F", storeReferenceOrder);
            if(HttpContext.Current.Session["Language"] != null)
                parameters.Add("idioma", HttpContext.Current.Session["Language"].ToString());
            ServerHttpHtmlRequisition post = new ServerHttpHtmlRequisition(dHandshakeConfigurationXml.urlHandshake, parameters, "GET");
            GenericHelper.LogFile("EasyPagObject::Handshake::XML::Handshake.cs::Step1 capturando XML storeId=" + dStore.storeId + " url=" + post.Url + "?36948FFEF212F5E4=" + parameters["36948FFEF212F5E4"] + "&91D4C3128BF7DA7F=" + parameters["91D4C3128BF7DA7F"] + "&idioma=" + parameters["idioma"], LogFileEntryType.Information);
            if (post.Send(dHandshakeConfigurationXml.requestEncoding))
            {
                //logo o que foi retornado
                dHandshakeLog = new DHandshakeSessionLog();
                dHandshakeLog.createDate = DateTime.Now;
                dHandshakeLog.handshakeSessionId = handshakeSessionId;
                dHandshakeLog.step = 3;
                dHandshakeLog.url = urlRedirect;
                dHandshakeLog.xmlData = post.Response.Trim();
                DataFactory.HandshakeSessionLog().Insert(dHandshakeLog);

                //Verifico se há dados na resposta
                Ensure.IsNotNullPage(post.Response, "A loja não informou corretamente os dados para o pagamento do seu pedido número: {0}.", storeReferenceOrder);
                
                XmlDocument xmlDoc = new XmlDocument();
                try
                {
                    xmlDoc.LoadXml(post.Response.Trim());
                    xmlDoc.Schemas.Add(null, HttpContext.Current.Server.MapPath("handshakexml.xsd"));
                    ValidationEventHandler eventHandler = new ValidationEventHandler(ValidationEventHandler);
                    xmlDoc.Validate(eventHandler);
                    if (dHandshakeConfiguration.validateItemsTotal && !ValidateItemsTotal(xmlDoc))
                        throw new Exception("A soma dos itens está inconsistente com o valor total do pedido.");
                    ValidateRecurrenceParams(xmlDoc);
                }
                catch (Exception ex)
                {
                    Ensure.IsNotNullPage(null, "Ocorreu um erro na verificação do XML do pedido {0}: {1}", storeReferenceOrder, ex.Message);
                }

                //salvo o xml do handshake na session
                Context.Session["xmlHandshake"] = xmlDoc.InnerXml;

                //salva a order e grava xml na session
                helper.ParseXml(xmlDoc);

                //redireciono para a página do cliente
                Context.Response.Redirect("~/fillconsumer.aspx");
            }
            else
            {
                Ensure.IsNotNullPage(null, "Ocorreu um erro ao tentar coletar as informações do pedido {0}: {1}", storeReferenceOrder, post.Response);
            }
		}

        static void ValidationEventHandler(object sender, ValidationEventArgs e)
        {
            switch (e.Severity)
            {
                case XmlSeverityType.Error:
                    Ensure.IsNotNullPage(null, "Erro na validação do XML: {0}", e.Message);
                    break;
            }

        }
        public void SendFinalizationPost(Guid paymentAttemptId)
        {
            SuperPag.Handshake.Xml.FinalizationPost finalization = new SuperPag.Handshake.Xml.FinalizationPost(paymentAttemptId);
            Thread thFinalization = new Thread(new ThreadStart(finalization.Send));
            thFinalization.Start();
        }
        public void SendPaymentPost(Guid paymentAttemptId)
        {
            SendPaymentPost(paymentAttemptId, int.MinValue);
        }
        public void SendPaymentPost(Guid paymentAttemptId, int installmentNumber)
        {
            SuperPag.Handshake.Xml.PaymentPost payment = new SuperPag.Handshake.Xml.PaymentPost(paymentAttemptId, installmentNumber);
            Thread thPayment = new Thread(new ThreadStart(payment.Send));
            thPayment.Start();
        }

        public bool ValidateItemsTotal(XmlDocument xmlDoc)
        {
            try
            {
                decimal total = GenericHelper.ParseDecimal(GenericHelper.GetSingleNodeString(xmlDoc, "/pedido/valor_total_pedido"));
                decimal itemstotal = 0;

                foreach (XmlNode node in xmlDoc.SelectNodes("/pedido/itens/item"))
                {
                    int itemQuantity = node.SelectSingleNode("quantidade_item") != null ? GenericHelper.ParseInt(node.SelectSingleNode("quantidade_item").InnerText) : 0;
                    decimal itemValue = node.SelectSingleNode("valor_unitario_item") != null ? GenericHelper.ParseDecimal(node.SelectSingleNode("valor_unitario_item").InnerText) : 0;
                    itemstotal += (itemQuantity * itemValue);
                }

                if (xmlDoc.SelectSingleNode("/pedido/valor_frete_pedido") != null && xmlDoc.SelectSingleNode("/pedido/valor_frete_pedido").InnerText != "")
                    itemstotal += GenericHelper.ParseDecimal(xmlDoc.SelectSingleNode("/pedido/valor_frete_pedido").InnerText);

                return total == itemstotal;
            }
            catch
            {
                return false;
            }
        }

        public bool ValidateRecurrenceParams(XmlDocument xmlDoc)
        {
            try
            {
                string cob_recorrencia = GenericHelper.GetSingleNodeString((string)HttpContext.Current.Session["xmlHandshake"], "/pedido/parametros_opcionais/cob_recorrencia");
                if(!String.IsNullOrEmpty(cob_recorrencia))
                {
                    if (!(cob_recorrencia == "1" || cob_recorrencia == "-1"))
                        throw new Exception("O tipo de recorrência inválida.");

                    string cob_liq_1par = GenericHelper.GetSingleNodeString((string)HttpContext.Current.Session["xmlHandshake"], "/pedido/parametros_opcionais/cob_liq_1par");

                    int cob_quantidade;
                    if (!Int32.TryParse(GenericHelper.GetSingleNodeString((string)HttpContext.Current.Session["xmlHandshake"], "/pedido/parametros_opcionais/cob_quantidade"), out cob_quantidade))
                        throw new Exception("Parâmetro cob_quantidade inválido.");
                    if (cob_quantidade < 1)
                        throw new Exception("Parâmetro cob_quantidade deve ser positivo.");

                    DateTime cob_data_base_agendamento = GenericHelper.ParseDateyyyyMMdd(GenericHelper.GetSingleNodeString((string)HttpContext.Current.Session["xmlHandshake"], "/pedido/parametros_opcionais/cob_data_base_agendamento"));
                    if (cob_data_base_agendamento == DateTime.MinValue)
                        throw new Exception("Data de início da recorrência inválida.");
                    if (cob_data_base_agendamento < DateTime.Today)
                        throw new Exception("A data de início da recorrência deve ser maior ou igual a atual.");
                }
                return true;
            }
            catch
            {
                return false;
            }
        }
    }
}
