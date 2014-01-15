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
using System.Text;
using SuperPag.Data.Messages;
using SuperPag.Helper;
using SuperPag;
using SuperPag.Data;
using SuperPag.br.com.redecard.ecommerce;
using SuperPag.Agents.KomerciWS.Messages;
using SuperPag.Handshake;
using SuperPag.Helper.Xml;

public partial class Agents_KomerciWS_result : System.Web.UI.Page
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
        Ensure.IsNotNullPage(Session["PaymentAttemptId"], "Sessão inválida iniciando uma transação KomerciWS");

        DPaymentAttempt attempt = DataFactory.PaymentAttempt().Locate((Guid)Session["PaymentAttemptId"]);
        Ensure.IsNotNullPage(attempt, "Tentativa de pagamento {0} não encontrada", Session["PaymentAttemptId"].ToString());
        DPaymentAgentSetupKomerci agentsetup = DataFactory.PaymentAgentSetupKomerci().Locate(attempt.paymentAgentSetupId);
        Ensure.IsNotNullPage(agentsetup, "A loja não está configurada corretamente para esse meio de pagamento");
        DOrder order = DataFactory.Order().Locate(attempt.orderId);
        DStore store = GenericHelper.CheckSessionStore(Context);
        FillTableTop(store.storeId);
        DStorePaymentInstallment paymentInstallment = DataFactory.StorePaymentInstallment().Locate(store.storeId, attempt.paymentFormId, order.installmentQuantity);

        //Seto o status do pedido
        GenericHelper.SetOrderStatus(HttpContext.Current, WorkflowOrderStatus.AgentCalled, attempt.paymentFormId + "," + order.installmentQuantity + "," + (int)PaymentAgents.KomerciWS);

        CreditCardInformation cardinfo = GenericHelper.GetCreditCardInformation();

        DPaymentAttemptKomerciWS attemptKomerci = new DPaymentAttemptKomerciWS();
        attemptKomerci.paymentAttemptId = attempt.paymentAttemptId;
        attemptKomerci.komerciStatus = (byte)PaymentAttemptKomerciWSStatus.Initial;
        if (order.installmentQuantity == 1 || attempt.installmentNumber != int.MinValue)
            attemptKomerci.transacao = "04";
        else if (paymentInstallment.installmentType == (byte)InstallmentType.Emissor)
            attemptKomerci.transacao = "06";
        else
            attemptKomerci.transacao = "08";
        attemptKomerci.cardInformation = GenericHelper.GetCreditCardXmlSession(Context);
        attemptKomerci.autoCapture = "S";
        DataFactory.PaymentAttemptKomerciWS().Insert(attemptKomerci);

        if (attempt.isSimulation)
        {
            attemptKomerci.komerciStatus = (int)PaymentAttemptKomerciWSStatus.Captured;
            attempt.lastUpdate = DateTime.Now;
            attempt.status = (int)PaymentAttemptStatus.Paid;
            DataFactory.PaymentAttempt().Update(attempt);
            DataFactory.PaymentAttemptKomerciWS().Update(attemptKomerci);
            GenericHelper.UpdateOrderStatusByAttemptStatus(order, attempt.status);
            Response.Redirect("~/finalization.aspx?id=" + attempt.paymentAttemptId.ToString());
        }

        int code = int.MinValue;

        komerci_capture komerciWS = new komerci_capture();
#if HOMOLOG
            string responseGetAuthorized = komerciWS.GetAuthorizedTst(
                    GenericHelper.FormatCurrency(attempt.price),
                    attemptKomerci.transacao,
                    order.installmentQuantity == 1 || attempt.installmentNumber != int.MinValue ? "00" : order.installmentQuantity.ToString(),
                    agentSetup.businessNumber.ToString(),
                    order.storeReferenceOrder, //TODO: Verificar se poe o storeReferenceOrder ou nao
                    paymentInfo.cardNumber.ToString(),
                    paymentInfo.securityCode.ToString(),
                    paymentInfo.expireDate.Substring(5, 2),
                    paymentInfo.expireDate.Substring(2, 2),
                    paymentInfo.cardHolder,
                    null, null, null, null, null, null, null, null, null, null, null, null, null,
                    attemptKomerci.autoCapture).InnerXml;
#else
        string responseGetAuthorized = komerciWS.GetAuthorized(
                GenericHelper.FormatCurrency(attempt.price),
                attemptKomerci.transacao,
                order.installmentQuantity == 1 || attempt.installmentNumber != int.MinValue ? "00" : order.installmentQuantity.ToString(),
                agentsetup.businessNumber.ToString(),
                order.storeReferenceOrder,
                cardinfo.Number,
                cardinfo.SecurityNumber,
                cardinfo.ExpirationDate.ToString("MM"), //03
                cardinfo.ExpirationDate.ToString("yy"), //83
                cardinfo.Name,
                null, null, null, null, null, null, null, null, null, null, null, null, null,
                attemptKomerci.autoCapture,
                DateTime.Now.ToString("yyyyMMdd")).InnerXml;
#endif

        responseGetAuthorized = "<AUTHORIZATION>" + responseGetAuthorized + "</AUTHORIZATION>";
        
        AUTHORIZATION responseAUTHORIZATION;
        string msgerror = "";
        if ((responseAUTHORIZATION = (AUTHORIZATION)XmlHelper.GetClass(responseGetAuthorized, typeof(AUTHORIZATION), out msgerror)) == null)
            Ensure.IsNotNull(null, "Komerci return is not recognized as a XML");

        attemptKomerci.data = responseAUTHORIZATION.DATA;
        attemptKomerci.codret = responseAUTHORIZATION.CODRET;
        attemptKomerci.msgret = HttpUtility.UrlDecode(responseAUTHORIZATION.MSGRET, Encoding.GetEncoding("iso-8859-1"));
        attemptKomerci.capcodret = responseAUTHORIZATION.CONFCODRET;
        attemptKomerci.capmsgret = HttpUtility.UrlDecode(responseAUTHORIZATION.CONFMSGRET, Encoding.GetEncoding("iso-8859-1"));
        attemptKomerci.numautent = responseAUTHORIZATION.NUMAUTENT;
        attemptKomerci.numautor = responseAUTHORIZATION.NUMAUTOR;
        attemptKomerci.numcv = responseAUTHORIZATION.NUMCV;
        attemptKomerci.numsqn = responseAUTHORIZATION.NUMSQN;
        attemptKomerci.TruncateStringFields();
        
        int authCodRet;
        if (!int.TryParse(responseAUTHORIZATION.CODRET, out authCodRet))
            Ensure.IsNotNull(null, "Komerci CODRET value not recognized as an int");

        code = authCodRet;

        if (authCodRet >= 0 && authCodRet < 50)
        {
            if (attemptKomerci.autoCapture.Equals("S"))
            {
                attemptKomerci.komerciStatus = String.IsNullOrEmpty(responseAUTHORIZATION.CONFCODRET) ?
                    (byte)PaymentAttemptKomerciWSStatus.WaitingCapture :
                    (responseAUTHORIZATION.CONFCODRET.Equals("0") || responseAUTHORIZATION.CONFCODRET.Equals("1") ?
                        attemptKomerci.komerciStatus = (byte)PaymentAttemptKomerciWSStatus.Captured :
                        attemptKomerci.komerciStatus = (byte)PaymentAttemptKomerciWSStatus.WaitingCapture);

                if (attemptKomerci.komerciStatus == (byte)PaymentAttemptKomerciWSStatus.Captured)
                {
                    attempt.returnMessage = attemptKomerci.capmsgret;
                    attempt.status = (int)PaymentAttemptStatus.Paid;
                    attempt.TruncateStringFields();
                    attemptKomerci.TruncateStringFields();

                    DataFactory.PaymentAttempt().Update(attempt);
                    DataFactory.PaymentAttemptKomerciWS().Update(attemptKomerci);
                    GenericHelper.UpdateOrderStatusByAttemptStatus(order, attempt.status);

                    RecurrenceProcess.FinishTransaction(attempt);
                }
                else
                {
                    //Como nao deu certo a tentativa de captura automatica
                    // tentar chamar a captura manualmente
#if HOMOLOG
                        string responseConfirmTxn = komerciWS.ConfirmTxnTst(
                                attemptKomerci.data,
                                attemptKomerci.numsqn,
                                attemptKomerci.numcv,
                                attemptKomerci.numautor,
                                order.installmentQuantity == 1 || attempt.installmentNumber != int.MinValue ? "00" : order.installmentQuantity.ToString(),
                                attemptKomerci.transacao,
                                GenericHelper.FormatCurrency(attempt.price),
                                agentSetup.businessNumber.ToString(),
                                null,
                                order.storeReferenceOrder,
                                null, null, null, null,
                                attemptKomerci.paymentAttemptId.ToString(),
                                null, null, null).InnerXml;
#else
                    string responseConfirmTxn = komerciWS.ConfirmTxn(
                                attemptKomerci.data,
                                attemptKomerci.numsqn,
                                attemptKomerci.numcv,
                                attemptKomerci.numautor,
                                order.installmentQuantity == 1 || attempt.installmentNumber != int.MinValue ? "00" : order.installmentQuantity.ToString(),
                                attemptKomerci.transacao,
                                GenericHelper.FormatCurrency(attempt.price),
                                agentsetup.businessNumber.ToString(),
                                null,
                                order.storeReferenceOrder,
                                null, null, null, null,
                                attemptKomerci.paymentAttemptId.ToString(),
                                null, null, null,
                                DateTime.Now.ToString("yyyyMMdd")).InnerXml;
#endif

                    responseConfirmTxn = "<CONFIRMATION>" + responseConfirmTxn + "</CONFIRMATION>";

                    CONFIRMATION responseCONFIRMATION;
                    if ((responseCONFIRMATION = (CONFIRMATION)XmlHelper.GetClass(responseConfirmTxn, typeof(CONFIRMATION), out msgerror)) == null)
                        Ensure.IsNotNull(null, "Komerci confirmation return is not recognized as a XML");

                    attemptKomerci.capcodret = responseCONFIRMATION.CODRET;
                    attemptKomerci.capmsgret = HttpUtility.UrlDecode(responseCONFIRMATION.MSGRET, Encoding.GetEncoding("iso-8859-1"));

                    int confCodRet;
                    if (!int.TryParse(responseCONFIRMATION.CODRET, out confCodRet)) //TODO: Quando a REDECARD retornar tags em branco, o que fazer?
                        Ensure.IsNotNull(null, "Komerci confirmation CODRET value not recognized as an int");

                    code = confCodRet;

                    if (responseCONFIRMATION.CODRET.Equals("0") || responseCONFIRMATION.CODRET.Equals("1"))
                    {
                        attemptKomerci.komerciStatus = (byte)PaymentAttemptKomerciWSStatus.Captured;
                        attempt.returnMessage = attemptKomerci.capmsgret;
                        attempt.status = (int)PaymentAttemptStatus.Paid;
                        attempt.TruncateStringFields();
                        attemptKomerci.TruncateStringFields();

                        DataFactory.PaymentAttempt().Update(attempt);
                        DataFactory.PaymentAttemptKomerciWS().Update(attemptKomerci);
                        GenericHelper.UpdateOrderStatusByAttemptStatus(order, attempt.status);

                        RecurrenceProcess.FinishTransaction(attempt);
                    }
                    else
                    {
                        attempt.returnMessage = attemptKomerci.capmsgret;
                    }
                }
            }
            else
            {
                attemptKomerci.komerciStatus = (byte)PaymentAttemptKomerciWSStatus.WaitingCapture;
                attempt.returnMessage = attemptKomerci.msgret;
                attempt.status = (int)PaymentAttemptStatus.Paid;
                attempt.TruncateStringFields();
                attemptKomerci.TruncateStringFields();

                DataFactory.PaymentAttempt().Update(attempt);
                DataFactory.PaymentAttemptKomerciWS().Update(attemptKomerci);
                GenericHelper.UpdateOrderStatusByAttemptStatus(order, attempt.status);

                RecurrenceProcess.FinishTransaction(attempt);
            }
        }
        else
        {
            attempt.returnMessage = attemptKomerci.msgret;
        }
        
        attempt.status = (int)PaymentAttemptStatus.NotPaid;
        attempt.lastUpdate = DateTime.Now;
        attempt.TruncateStringFields();
        attemptKomerci.TruncateStringFields();

        DataFactory.PaymentAttempt().Update(attempt);
        DataFactory.PaymentAttemptKomerciWS().Update(attemptKomerci);
        GenericHelper.UpdateOrderStatusByAttemptStatus(order, attempt.status);
        
        FillButtonImage();

        if (code != int.MinValue)
            lblCode.Text = code.ToString();
        else
            lblCode.Text = "A transação não foi concluída";
        if (attempt.returnMessage != null)
            lblMessage.Text = attempt.returnMessage;
        else
            lblMessage.Text = "Transação negada";
        
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
