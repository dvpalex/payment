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
using SuperPag.Agents.VisaMoset3;
using SuperPag.Agents.VBV3.Messages;
using SuperPag.Agents.VBV3;
using SuperPag.Handshake;

public partial class Agents_VisaMoset3_result : System.Web.UI.Page
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
        DPaymentAgentSetupVBV agentsetup = DataFactory.PaymentAgentSetupVBV().Locate(attempt.paymentAgentSetupId);
        Ensure.IsNotNullPage(agentsetup, "A loja não está configurada corretamente para esse meio de pagamento");
        DOrder order = DataFactory.Order().Locate(attempt.orderId);
        DStore store = GenericHelper.CheckSessionStore(Context);
        FillTableTop(store.storeId);
        DStorePaymentInstallment paymentInstallment = DataFactory.StorePaymentInstallment().Locate(store.storeId, attempt.paymentFormId, order.installmentQuantity);

        //Seto o status do pedido
        GenericHelper.SetOrderStatus(HttpContext.Current, WorkflowOrderStatus.AgentCalled, attempt.paymentFormId + "," + order.installmentQuantity + "," + (int)PaymentAgents.VisaMoset3);

        CreditCardInformation cardinfo = GenericHelper.GetCreditCardInformation();
        string urlComponent = ConfigurationManager.AppSettings["VBV3ComponentUrl"];

        DPaymentAttemptMoset attemptMoset = new DPaymentAttemptMoset();
        attemptMoset.paymentAttemptId = attempt.paymentAttemptId;
        attemptMoset.merchantId = agentsetup.businessNumber.ToString();
        attemptMoset.cardInformation = GenericHelper.GetCreditCardXmlSession(HttpContext.Current);
        attemptMoset.tid = Moset.GenerateTID(agentsetup.businessNumber.ToString(), attempt.paymentFormId, order.installmentQuantity, paymentInstallment.installmentType); ;
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

        Moset.BuildRequestXml(attemptMoset.tid, ((int)(attempt.price * 100)).ToString());

        attemptMoset.mosetStatus = (int)PaymentAttemptVisaMosetStatus.Send;
        DataFactory.PaymentAttemptMoset().Update(attemptMoset);
        
        Moset moset = new Moset();
        bool result = moset.ProcessPayment(urlComponent, order.orderId, order.storeReferenceOrder, attemptMoset.tid, attempt.price,
            cardinfo.Number, cardinfo.ExpirationDate, agentsetup.businessNumber.ToString(), "cfg" + agentsetup.paymentAgentSetupId.ToString(), attempt.paymentAttemptId.ToString(), cardinfo.SecurityNumber.PadRight(3, Char.Parse("0")));

        if (result)
        {
            attemptMoset.free = moset.Free;
            attemptMoset.lr = moset.Lr;

            // verifico se o TID retornado é mesmo que está gravado no banco de dados
            if (moset.Tid != attemptMoset.tid)
                GenericHelper.RedirectToErrorPage("TID inconsistente");
            
            attempt.returnMessage = Moset.GetPaymentProcessResponseDescription(moset.Lr);
            attemptMoset.message = attempt.returnMessage;

            if(attemptMoset.lr == 0 || attemptMoset.lr == 11)
            {
                if (agentsetup.autoCapture)
                {
                    attemptMoset.mosetStatus = (int)PaymentAttemptVisaMosetStatus.Capture;
                    attemptMoset.TruncateStringFields();
                    DataFactory.PaymentAttemptMoset().Update(attemptMoset);

                    VBV3CaptureReturn captureReturn = VBV3.Capture(attemptMoset.tid, attemptMoset.free, "cfg" + attempt.paymentAgentSetupId);
                    if (captureReturn != null && captureReturn.tid != null)
                    {
                        attemptMoset.capturedTid = captureReturn.tid;
                        attemptMoset.capturedArs = captureReturn.ars;
                        attemptMoset.capturedCap = captureReturn.cap;
                        attemptMoset.capturedCurrency = Moset.GetCapturedCurrency(captureReturn.cap);
                        attemptMoset.capturedValue = Moset.GetCapturedValue(captureReturn.cap);

                        if (attemptMoset.lr != int.MinValue)
                        {
                            attempt.returnMessage = Moset.GetPaymentProcessResponseDescription((int)captureReturn.lr);
                            attemptMoset.message = attempt.returnMessage;

                            if (attemptMoset.lr == 0)
                            {
                                attempt.status = (int)PaymentAttemptStatus.Paid;
                                attempt.lastUpdate = DateTime.Now;
                                attempt.TruncateStringFields();
                                attemptMoset.mosetStatus = (int)PaymentAttemptVisaMosetStatus.End;
                                attemptMoset.TruncateStringFields();

                                DataFactory.PaymentAttempt().Update(attempt);
                                DataFactory.PaymentAttemptMoset().Update(attemptMoset);
                                GenericHelper.UpdateOrderStatusByAttemptStatus(order, attempt.status);

                                RecurrenceProcess.FinishTransaction(attempt);
                            }
                        }
                        else
                        {
                            attemptMoset.lr = -1;
                            attempt.returnMessage = "Retorno da captura não reconhecido";
                            attemptMoset.message = attempt.returnMessage;
                        }
                    }
                    else
                    {
                        attemptMoset.lr = -1;
                        attempt.returnMessage = "Problemas na captura";
                        attemptMoset.message = attempt.returnMessage;
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

                    RecurrenceProcess.FinishTransaction(attempt);
                }
            }
        }
        else
        {
            if (moset.Lr == -1)
            {
                attemptMoset.lr = -1;
                attemptMoset.message = moset.Msgret;
                attempt.returnMessage = "Problemas na transferência das informações";
            }
            else
            {
                attemptMoset.lr = -2;
                attemptMoset.message = "retorno de autorização não reconhecido: " + moset.Msgret;
                attempt.returnMessage = "Retorno de autorização não reconhecido";
            }
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
