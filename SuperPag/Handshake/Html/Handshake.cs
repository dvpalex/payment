using System;
using System.Web;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Text;
using SuperPag.Handshake;
using SuperPag.Data;
using SuperPag.Data.Messages;
using SuperPag.Data.Interfaces;
using SuperPag.Helper;
using System.Threading;
using System.IO;
using System.Xml;

namespace SuperPag.Handshake.Html
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
            int storeId;

            Guid handshakeSessionId = Step1Subpart(storeKey, storeReferenceOrder, out storeId);

            //obtem os dados do handshake
            DHandshakeConfiguration dHandshakeConfiguration = DataFactory.HandshakeConfiguration().Locate(storeId, (int)HandshakeType.HtmlSPag10);
            Ensure.IsNotNullPage(dHandshakeConfiguration, "A loja {0} não está configurada para utilizar esse tipo Handshake.", storeId);

            //obtem os dados do handshake de html
            DHandshakeConfigurationHtml dHandshakeConfigurationHtml = DataFactory.HandshakeConfigurationHtml().Locate(dHandshakeConfiguration.handshakeConfigurationId);
            Ensure.IsNotNullPage(dHandshakeConfiguration, "A loja {0} não está configurada corretamente para utilizar esse tipo Handshake.", storeId);

            //monto a url de redirect para o servidor da loja
            string urlRedirect = dHandshakeConfigurationHtml.urlHandshake.Trim();
            urlRedirect = string.Format("{0}?36948FFEF212F5E4={1}&91D4C3128BF7DA7F={2}", urlRedirect, handshakeSessionId, storeReferenceOrder);

            //salva os dados de envio no log de handshake
            DHandshakeSessionLog dHandshakeLog = new DHandshakeSessionLog();
            dHandshakeLog.createDate = DateTime.Now;
            dHandshakeLog.handshakeSessionId = handshakeSessionId;
            dHandshakeLog.step = 2;
            dHandshakeLog.url = urlRedirect;
            dHandshakeLog.xmlData = helper.CreateXmlForResponse(urlRedirect);
            DataFactory.HandshakeSessionLog().Insert(dHandshakeLog);

            //coloco a chave de acesso da loja na session
            Context.Session["storeKey"] = storeKey;

            Context.Response.Redirect(urlRedirect);
        }
        public Guid Step1Subpart(string storeKey, string storeReferenceOrder, out int storeId)
        {
            Context.Session.Clear();

            if (storeKey == null || storeKey == string.Empty)
                GenericHelper.RedirectToErrorPage("Codigo de loja inválido");

            //TODO: Verificar se ja existe alguma tentativa de pagamento para esse
            //      pedido de cliente e criar regra de negocio

            //obtem a loja
            DStore dStore = DataFactory.Store().Locate(storeKey);
            Ensure.IsNotNullPage(dStore, "A chave de loja {0} é invalida", storeKey);

            //crio sessao de handshake
            Guid handshakeSessionId = Guid.NewGuid();
            DHandshakeSession dHandshakeSession = new DHandshakeSession();
            dHandshakeSession.handshakeSessionId = handshakeSessionId;
            dHandshakeSession.storeId = dStore.storeId;
            dHandshakeSession.orderId = long.MinValue;
            dHandshakeSession.handshakeType = (int)HandshakeType.HtmlSPag10;
            dHandshakeSession.createDate = DateTime.Now;
            DataFactory.HandshakeSession().Insert(dHandshakeSession);

            //salva os dados recebidos no log de handshake
            DHandshakeSessionLog dHandshakeLog = new DHandshakeSessionLog();
            dHandshakeLog.createDate = DateTime.Now;
            dHandshakeLog.handshakeSessionId = handshakeSessionId;
            dHandshakeLog.step = 1;
            dHandshakeLog.url = Context.Request.RawUrl;
            dHandshakeLog.xmlData = helper.CreateXmlForRequest(null);
            DataFactory.HandshakeSessionLog().Insert(dHandshakeLog);

            storeId = dStore.storeId;

            return handshakeSessionId;
        }
        public void Step2(string handshakeSessionId, string storeReferenceOrder)
        {
            //get store from session code
            if(handshakeSessionId == null)
                GenericHelper.RedirectToErrorPage("Código de sessão nulo.");
            if (handshakeSessionId == String.Empty)
                GenericHelper.RedirectToErrorPage("Código de sessão vazio.");

            Guid hsId;
            if (!GenericHelper.TryParseGuid(handshakeSessionId, out hsId))
                GenericHelper.RedirectToErrorPage("Código de sessão {0} é inválido.", handshakeSessionId);

            DHandshakeSession hsSession = DataFactory.HandshakeSession().Locate(hsId);
            Ensure.IsNotNullPage(hsSession, "Código de sessão {0} é inválido.", handshakeSessionId);

            //obtem a loja
            DStore dStore = DataFactory.Store().Locate(hsSession.storeId);
            if (dStore == null) GenericHelper.RedirectToErrorPage("Codigo de loja inválido");

            //obtem os dados do handshake
            DHandshakeConfiguration dHandshakeConfiguration = DataFactory.HandshakeConfiguration().Locate(dStore.storeId, (int)HandshakeType.HtmlSPag10);
            Ensure.IsNotNullPage(dHandshakeConfiguration, "A loja {0} não está configurada para utilizar esse tipo Handshake.", dStore.storeId);

            //obtem os dados do handshake de html
            DHandshakeConfigurationHtml dHandshakeConfigurationHtml = DataFactory.HandshakeConfigurationHtml().Locate(dHandshakeConfiguration.handshakeConfigurationId);
            Ensure.IsNotNullPage(dHandshakeConfiguration, "A loja {0} não está configurada corretamente para utilizar esse tipo Handshake.", dStore.storeId);

            string encoding = "iso-8859-1";
            if (!String.IsNullOrEmpty(dHandshakeConfigurationHtml.requestEncoding))
                encoding = dHandshakeConfigurationHtml.requestEncoding;

            //Leio o formulario postado de acordo com o encoding configurado.
            Context.Request.InputStream.Position = 0;
            StreamReader reader = new StreamReader(Context.Request.InputStream, Encoding.GetEncoding(encoding));
            string resultISO = reader.ReadToEnd();
            if (resultISO.EndsWith("&")) resultISO.Remove(resultISO.Length - 1);

            //salva o código de sessao na session
            Context.Session["handshakeSessionId"] = handshakeSessionId;

            //salva o storeId na session
            Context.Session["storeId"] = dStore.storeId;

            //salva o codigo do pedido na session
            Context.Session["storeReferenceOrder"] = storeReferenceOrder;

            //salva no log de handshake
            DHandshakeSessionLog dHandshakeLog = new DHandshakeSessionLog();
            dHandshakeLog.createDate = DateTime.Now;
            dHandshakeLog.handshakeSessionId = new Guid(handshakeSessionId);
            dHandshakeLog.step = 3;
            dHandshakeLog.url = Context.Request.RawUrl;
            dHandshakeLog.xmlData = helper.CreateXmlForRequest(HttpUtility.ParseQueryString(resultISO, Encoding.GetEncoding(encoding)));
            DataFactory.HandshakeSessionLog().Insert(dHandshakeLog);

            //salvo o xml do handshake na session
            Context.Session["htmlHandshake"] = dHandshakeLog.xmlData;

            XmlDocument xmlDoc = new XmlDocument();
            try
            {
                xmlDoc.LoadXml(dHandshakeLog.xmlData);
                if (dHandshakeConfiguration.validateItemsTotal && !ValidateItemsTotal(xmlDoc))
                    throw new Exception("A soma dos itens está inconsistente com o valor total do pedido.");
                ValidateRecurrenceParams(xmlDoc);
            }
            catch (Exception ex)
            {
                Ensure.IsNotNullPage(null, "Ocorreu um erro na verificação dos dados do pedido {0}: {1}", storeReferenceOrder, ex.Message);
            }

            //salva a order
            helper.ParseHtml(xmlDoc);

            //Seto o status do pedido
            GenericHelper.SetOrderStatus(HttpContext.Current, WorkflowOrderStatus.HandshakeFinished, "HTML");

            //redireciono para a página do cliente
            Context.Response.Redirect("~/fillconsumer.aspx");
        }

        public void SendFinalizationPost(Guid paymentAttemptId)
        {
            SuperPag.Handshake.Html.FinalizationPost finalization = new SuperPag.Handshake.Html.FinalizationPost(paymentAttemptId);
            Thread thFinalization = new Thread(new ThreadStart(finalization.Send));
            thFinalization.Start();
        }
        public void SendPaymentPost(Guid paymentAttemptId)
        {
            SendPaymentPost(paymentAttemptId, int.MinValue);
        }
        public void SendPaymentPost(Guid paymentAttemptId, int installmentNumber)
        {
            SuperPag.Handshake.Html.PaymentPost payment = new SuperPag.Handshake.Html.PaymentPost(paymentAttemptId, installmentNumber);
            Thread thPayment = new Thread(new ThreadStart(payment.Send));
            thPayment.Start();
        }
        public void ReceiveFinalization(string paymentAttemptId)
        {
            string orderId = "", storeId = "";
            try
            {
                DPaymentAttempt paymentAttempt = DataFactory.PaymentAttempt().Locate(new Guid(paymentAttemptId));
                Ensure.IsNotNull(paymentAttempt, "Código da tentativa de pagamento inválido {0}", paymentAttemptId);

                DOrder dOrder = DataFactory.Order().Locate(paymentAttempt.orderId);
                Ensure.IsNotNull(dOrder, "Pedido não encontrado {0}", paymentAttempt.orderId);

                DHandshakeConfiguration dHandshakeConfigurationH = DataFactory.HandshakeConfiguration().Locate(dOrder.storeId, (int)HandshakeType.HtmlSPag10);
                Ensure.IsNotNull(dOrder, "Configuração não encontrada para a loja {0}", dOrder.storeId);

                orderId = dOrder.orderId.ToString();
                storeId = dOrder.storeId.ToString();

                if (paymentAttempt.status == (int)PaymentAttemptStatus.Paid && !dHandshakeConfigurationH.autoPaymentConfirm)
                    SendPaymentPost(paymentAttempt.paymentAttemptId);

                //atualiza envio de post de finalizacao HTML para confirmado
                DServiceFinalizationPost finalizationPost = DataFactory.ServiceFinalizationPost().Locate(paymentAttempt.paymentAttemptId);
                if (finalizationPost != null)
                {
                    finalizationPost.postStatus = (int)PostStatus.Confirmed;
                    finalizationPost.lastUpdate = DateTime.Now;
                    DataFactory.ServiceFinalizationPost().Update(finalizationPost);
                }
                else
                {
                    DServiceFinalizationPost dServiceFinalizationPost = new DServiceFinalizationPost();
                    dServiceFinalizationPost.paymentAttemptId = paymentAttempt.paymentAttemptId;
                    dServiceFinalizationPost.postStatus = (int)PostStatus.Confirmed;
                    dServiceFinalizationPost.postRetries = 0;
                    dServiceFinalizationPost.lastUpdate = DateTime.Now;
                    DataFactory.ServiceFinalizationPost().Insert(dServiceFinalizationPost);
                }

                GenericHelper.LogFile("EasyPagObject::Handshake.cs::Handshake.ReceiveFinalization Post de finalização atualizado storeId=" + storeId + " orderId=" + orderId + " paymentAttemptId=" + paymentAttemptId, LogFileEntryType.Information);

                //mostro o "OK" na tela
                HttpContext.Current.Response.Write("OK");
            }
            catch (Exception e)
            {
                GenericHelper.LogFile("EasyPagObject::Handshake.cs::Handshake.ReceiveFinalization storeId=" + storeId + " orderId=" + orderId + " paymentAttemptId=" + paymentAttemptId + " msg=" + e.Message, LogFileEntryType.Error);

                HttpContext.Current.Response.Write("NOK");
            }
        }
        public void ReceivePayment(string paymentAttemptId, string parcela)
        {
            try
            {
                int installmentNumber = 0;

                DPaymentAttempt paymentAttempt = DataFactory.PaymentAttempt().Locate(new Guid(paymentAttemptId));
                Ensure.IsNotNull(paymentAttempt, "Código da tentativa de pagamento inválido {0}", paymentAttemptId);

                if(!Int32.TryParse(parcela, out installmentNumber))
                    Ensure.IsNotNull(null, "Parcela {0} é inválida para a tentativa de pagamento {1}", parcela, paymentAttemptId);

                DServicePaymentPost paymentPost = DataFactory.ServicePaymentPost().Locate(paymentAttempt.paymentAttemptId, installmentNumber);
                if (paymentPost != null)
                {
                    paymentPost.postStatus = (int)PostStatus.Confirmed;
                    paymentPost.lastUpdate = DateTime.Now;
                    DataFactory.ServicePaymentPost().Update(paymentPost);
                }
                else
                {
                    DServicePaymentPost dServicePaymentPost = new DServicePaymentPost();
                    dServicePaymentPost.paymentAttemptId = paymentAttempt.paymentAttemptId;
                    dServicePaymentPost.installmentNumber = installmentNumber;
                    dServicePaymentPost.postStatus = (int)PostStatus.Confirmed;
                    dServicePaymentPost.postRetries = 0;
                    dServicePaymentPost.lastUpdate = DateTime.Now;
                    DataFactory.ServicePaymentPost().Insert(dServicePaymentPost);
                }

                GenericHelper.LogFile("EasyPagObject::Handshake.cs::Handshake.ReceivePayment Post de pagamento atualizado paymentAttemptId=" + paymentAttemptId + " parcela=" + parcela, LogFileEntryType.Information);
                
                //mostro o "OK" na tela
                HttpContext.Current.Response.Write("OK");
            }
            catch (Exception e)
            {
                GenericHelper.LogFile("EasyPagObject::Handshake.cs::Handshake.ReceivePayment paymentAttemptId=" + paymentAttemptId + " " + e.Message, LogFileEntryType.Error);

                HttpContext.Current.Response.Write("NOK");
            }
        }

        public bool ValidateItemsTotal(XmlDocument xmlDoc)
        {
            try
            {
                decimal total = GenericHelper.ParseDecimal(GenericHelper.GetSingleNodeString(xmlDoc, "root/form/spv"));
                decimal itemstotal = 0;
                
                List<DOrderItem> dOrderItemList = new List<DOrderItem>();
                #region Extraio os items para a lista
                List<XmlNode> itemList = helper.ExtractItensNode(xmlDoc);
                foreach (XmlNode item in itemList)
                {
                    int number = int.Parse(item.Name.Substring(4));
                    bool founded = false;
                    foreach (DOrderItem dOrderItem in dOrderItemList)
                    {
                        if (dOrderItem.itemNumber == number)
                        {
                            helper.SetItemProperty(item, dOrderItem);
                            founded = true;
                            break;
                        }
                    }
                    if (!founded)
                    {
                        DOrderItem dOrderItem = new DOrderItem();
                        dOrderItem.itemNumber = number;
                        helper.SetItemProperty(item, dOrderItem);
                        dOrderItemList.Add(dOrderItem);
                    }
                }
                #endregion
                foreach (DOrderItem item in dOrderItemList)
                    itemstotal += (item.itemQuantity * item.itemValue);
                
                if (GenericHelper.GetSingleNodeString(xmlDoc, "root/form/sfrete") != null && GenericHelper.GetSingleNodeString(xmlDoc, "root/form/sfrete") != "")
                    itemstotal += GenericHelper.ParseDecimal(GenericHelper.GetSingleNodeString(xmlDoc, "root/form/sfrete"));

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
                string cob_recorrencia = GenericHelper.GetSingleNodeString((string)HttpContext.Current.Session["htmlHandshake"], "//cob_recorrencia");
                if (!String.IsNullOrEmpty(cob_recorrencia))
                {
                    if (!(cob_recorrencia == "1" || cob_recorrencia == "-1"))
                        throw new Exception("O tipo de recorrência inválida.");

                    string cob_liq_1par = GenericHelper.GetSingleNodeString((string)HttpContext.Current.Session["htmlHandshake"], "//cob_liq_1par");

                    int cob_quantidade;
                    if (!Int32.TryParse(GenericHelper.GetSingleNodeString((string)HttpContext.Current.Session["htmlHandshake"], "//cob_quantidade"), out cob_quantidade))
                        throw new Exception("Parâmetro cob_quantidade inválido.");
                    if (cob_quantidade < 1)
                        throw new Exception("Parâmetro cob_quantidade deve ser positivo.");

                    DateTime cob_data_base_agendamento = GenericHelper.ParseDateyyyyMMdd(GenericHelper.GetSingleNodeString((string)HttpContext.Current.Session["htmlHandshake"], "//cob_data_base_agendamento"));
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
