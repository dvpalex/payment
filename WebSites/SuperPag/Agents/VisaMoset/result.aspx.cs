using System;
using System.Data;
using System.Configuration;
using System.Collections;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using SuperPag.Data.Messages;
using SuperPag.Data;
using SuperPag;
using SuperPag.Helper;
using SuperPag.Agents.VisaMoset;


public partial class Agents_VisaMoset_result : System.Web.UI.Page
{
    private void FillTableTop(int storeId)
    {
        DSPLegacyStore dSPLegay = DataFactory.SPLegacyStore().Locate(storeId);
        if (Ensure.IsNotNull(dSPLegay) && Ensure.IsNotNull(dSPLegay.ucTableTop))
        {
            plhTableTop.Controls.Add(Page.LoadControl(dSPLegay.ucTableTop));
        }
        else
        {
            plhTableTop.Visible = false;
        }
    }
    
    protected void Page_Load(object sender, EventArgs e)
    {
        Ensure.IsNotNullPage(Session["PaymentAttemptId"], "Sessão inválida iniciando uma transação Moset");

        DPaymentAttempt attempt = DataFactory.PaymentAttempt().Locate((Guid)Session["PaymentAttemptId"]);
        Ensure.IsNotNullPage(attempt, "Tentativa de pagamento {0} não encontrada", Session["PaymentAttemptId"].ToString());
        DPaymentAgentSetupMoset agentsetup = DataFactory.PaymentAgentSetupMoset().Locate(attempt.paymentAgentSetupId);
        Ensure.IsNotNullPage(agentsetup, "A loja não está configurada corretamente para esse meio de pagamento");
        DOrder order = DataFactory.Order().Locate(attempt.orderId);
        DStore store = GenericHelper.CheckSessionStore(Context);
        FillTableTop(store.storeId);

        //Seto o status do pedido
        GenericHelper.SetOrderStatus(HttpContext.Current, WorkflowOrderStatus.AgentCalled, attempt.paymentFormId + "," + order.installmentQuantity + "," + (int)PaymentAgents.VisaMoset);

        string xml = GenericHelper.GetCreditCardXmlSession(Context);
        Ensure.IsNotNullOrEmptyPage(xml, "Sessão inválida para os dados do cartão de uma transação Moset");
        CreditCardInformation cardinfo = GenericHelper.GetCreditCardXml(xml);
        Ensure.IsNotNullPage(cardinfo, "Dados inválidos do cartão para uma transação Moset");

        string urlComponent = ConfigurationManager.AppSettings["MosetComponentUrl"];

        string tid = "";
        DateTime tidDate = DateTime.Now;

        tid = agentsetup.merchantId.ToString().PadLeft(10, '0').Substring(4, 5);
        tid += tidDate.Year.ToString("0000").Substring(3, 1);
        tid += tidDate.DayOfYear.ToString("000");
        tid += tidDate.ToString("hhmmss");
        tid += tidDate.Millisecond.ToString().Substring(0, 1);
        if (order.installmentQuantity == 1)
            tid += "1001"; // Cartões VISA à vista
        else
        {
            //TODO: Variar tipo de parcelamento
            tid += "2" + order.installmentQuantity.ToString("000"); // Parcelado Loja
        }
        
        DPaymentAttemptMoset attemptMoset = new DPaymentAttemptMoset();
        attemptMoset.paymentAttemptId = attempt.paymentAttemptId;
        attemptMoset.merchantId = agentsetup.merchantId;
        attemptMoset.cardInformation = xml;
        attemptMoset.tid = tid;
        attemptMoset.mosetStatus = (int)PaymentAttemptVisaMosetStatus.Initial;
        DataFactory.PaymentAttemptMoset().Insert(attemptMoset);

        if (attempt.isSimulation)
        {
            attemptMoset.mosetStatus = (int)PaymentAttemptVisaMosetStatus.End;
            attempt.lastUpdate = DateTime.Now;
            attempt.status = (int)PaymentAttemptStatus.Paid;
            DataFactory.PaymentAttempt().Update(attempt);
            DataFactory.PaymentAttemptMoset().Update(attemptMoset);
            GenericHelper.UpdateOrderStatusByAttemptStatus(order, attempt.status);
            Response.Redirect("~/finalization.aspx?id=" + attempt.paymentAttemptId.ToString());
        }

        attemptMoset.mosetStatus = (int)PaymentAttemptVisaMosetStatus.Send;
        DataFactory.PaymentAttemptMoset().Update(attemptMoset);
        
        Moset moset = new Moset();
        bool result = moset.ProcessPayment(urlComponent, order.orderId, attemptMoset.tid, attempt.price,
            cardinfo.Number, cardinfo.ExpirationDate, agentsetup.merchantId, "cfg" + agentsetup.paymentAgentSetupId.ToString(), attempt.paymentAttemptId.ToString());

        if (result)
        {
            attemptMoset.free = moset.Free;
            attemptMoset.lr = moset.Lr;

            if(attemptMoset.lr != int.MinValue)
            {
                // verifico se o TID retornado é mesmo que está gravado no banco de dados
                if (moset.Tid != attemptMoset.tid)
                    GenericHelper.RedirectToErrorPage("TID inconsistente");
                
                attempt.returnMessage = moset.GetPaymentProcessResponseDescription();
                attemptMoset.message = moset.GetPaymentProcessResponseDescription();

                if(attemptMoset.lr == 0 || attemptMoset.lr == 11)
                {
                    if (agentsetup.autoCapture)
                    {
                        attemptMoset.mosetStatus = (int)PaymentAttemptVisaMosetStatus.Capture;
                        attemptMoset.TruncateStringFields();
                        DataFactory.PaymentAttemptMoset().Update(attemptMoset);

                        bool resultCapture = moset.Capture(urlComponent, attemptMoset.tid, "cfg" + agentsetup.paymentAgentSetupId.ToString());

                        if (resultCapture)
                        {
                            attemptMoset.capturedTid = moset.Tid;
                            attemptMoset.capturedArs = moset.Ars;
                            attemptMoset.capturedCod = moset.Cod;
                            attemptMoset.capturedCap = moset.Cap;
                            attemptMoset.capturedCurrency = moset.GetCapturedCurrency();
                            attemptMoset.capturedValue = moset.GetCapturedValue();

                            if (attemptMoset.capturedCod != int.MinValue)
                            {
                                attempt.returnMessage = moset.GetCaptureResponseDescription();
                                attemptMoset.message = moset.GetCaptureResponseDescription();

                                if (attemptMoset.capturedCod == 0)
                                {
                                    attempt.status = (int)PaymentAttemptStatus.Paid;
                                    attempt.lastUpdate = DateTime.Now;
                                    attempt.TruncateStringFields();
                                    attemptMoset.mosetStatus = (int)PaymentAttemptVisaMosetStatus.End;
                                    attemptMoset.TruncateStringFields();

                                    DataFactory.PaymentAttempt().Update(attempt);
                                    DataFactory.PaymentAttemptMoset().Update(attemptMoset);
                                    GenericHelper.UpdateOrderStatusByAttemptStatus(order, attempt.status);
                                    
                                    Response.Redirect("~/finalization.aspx?id=" + attempt.paymentAttemptId.ToString());
                                }
                            }
                            else
                            {
                                attemptMoset.capturedCod = -1;
                                attemptMoset.message = "retorno da captura não reconhecido: " + moset.MsgretCapture;
                                attempt.returnMessage = "Retorno da captura não reconhecido";
                            }
                        }
                        else
                        {
                            attemptMoset.capturedCod = -1;
                            attemptMoset.message = moset.MsgretCapture;
                            attempt.returnMessage = "Problemas na captura";
                        }
                    }
                    else
                    {
                        attempt.status = (int)PaymentAttemptStatus.Paid;
                        attempt.lastUpdate = DateTime.Now;
                        attempt.TruncateStringFields();
                        attemptMoset.mosetStatus = (int)PaymentAttemptVisaMosetStatus.WaitingCapture;
                        attemptMoset.TruncateStringFields();

                        DataFactory.PaymentAttempt().Update(attempt);
                        DataFactory.PaymentAttemptMoset().Update(attemptMoset);
                        GenericHelper.UpdateOrderStatusByAttemptStatus(order, attempt.status);
                        
                        Response.Redirect("~/finalization.aspx?id=" + attempt.paymentAttemptId.ToString());
                    }
                }
            }
            else
            {
                attemptMoset.lr = -1;
                attemptMoset.message = "retorno de autorização não reconhecido: " + moset.Msgret;
                attempt.returnMessage = "Retorno de autorização não reconhecido";
            }
        }
        else
        {
            attemptMoset.lr = -1;
            attemptMoset.message = moset.Msgret;
            attempt.returnMessage = "Problemas na transferência das informações";
        }

        attempt.status = (int)PaymentAttemptStatus.NotPaid;
        attempt.lastUpdate = DateTime.Now;
        attemptMoset.mosetStatus = (int)PaymentAttemptVisaMosetStatus.End;
        attemptMoset.TruncateStringFields();

        DataFactory.PaymentAttempt().Update(attempt);
        DataFactory.PaymentAttemptMoset().Update(attemptMoset);
        GenericHelper.UpdateOrderStatusByAttemptStatus(order, attempt.status);

        FillButtonImage();
        
        if (attemptMoset.capturedCod != int.MinValue)
            lblCode.Text = attemptMoset.capturedCod.ToString();
        else if (attemptMoset.lr != int.MinValue)
            lblCode.Text = attemptMoset.lr.ToString();
        else
            lblCode.Text = "A transação não foi concluída";
        if (attempt.returnMessage != null)
            lblMessage.Text = attempt.returnMessage;
        else
            lblMessage.Text = "Transação negada";
        lblTid.Text = String.IsNullOrEmpty(attemptMoset.tid) ? "não disponível" : attemptMoset.tid;
        
        GenericHelper.SetPaymentFormSession(Context, int.MinValue);
        GenericHelper.SetPaymentGroupSession(Context, int.MinValue);
        GenericHelper.SetInstallmentNumber(Context, int.MinValue);
    }
    
    private void FillButtonImage()
    {
        //Seta imagem do botao continuar
        string LinkBotao6, UrlBotao6;
        if (Session["htmlHandshake"] != null)
        {
            LinkBotao6 = HttpUtility.HtmlDecode(GenericHelper.GetSingleNodeString((string)Session["htmlHandshake"], "//linkbotao6"));
            UrlBotao6 = HttpUtility.HtmlDecode(GenericHelper.GetSingleNodeString((string)Session["htmlHandshake"], "//urlbotao6"));
        }
        else
        {
            LinkBotao6 = HttpUtility.HtmlDecode(GenericHelper.GetSingleNodeString((string)Session["xmlHandshake"], "//link_botao6"));
            UrlBotao6 = HttpUtility.HtmlDecode(GenericHelper.GetSingleNodeString((string)Session["xmlHandshake"], "//urlbotao6"));
        }

        if (!String.IsNullOrEmpty(UrlBotao6))
            lnkReturn.ImageUrl = UrlBotao6;
        if (!String.IsNullOrEmpty(LinkBotao6))
        {
            lnkReturn.NavigateUrl = LinkBotao6;
            lnkReturn.Visible = true;
        }
    }
}
