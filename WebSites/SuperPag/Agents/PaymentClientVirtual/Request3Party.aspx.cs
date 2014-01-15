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
using SuperPag.Data;
using SuperPag;
using SuperPag.Helper;
using SuperPag.Agents.PaymentClientVirtual;

public partial class Agents_PaymentClientVirtual_Request3Party : System.Web.UI.Page
{
    protected string errorNo = "";
    protected string errorMessage = "";
    protected string transactionId = "";

    protected void Page_Load(object sender, System.EventArgs e)
    {
        Ensure.IsNumericPage(Page.Request.QueryString["vpc_MerchTxnRef"], "Retorno da Amex não reconhecido pelo sistema");

        int agentOrderReference = int.Parse(Page.Request.QueryString["vpc_MerchTxnRef"]);

        DPaymentAttemptPaymentClientVirtual dPaymentAttemptPaymentClientVirtual = DataFactory.PaymentAttemptPaymentClientVirtual().Locate(agentOrderReference);
        Ensure.IsNotNullPage(dPaymentAttemptPaymentClientVirtual, "Transação {0} não encontrada no sistema", agentOrderReference);

        DPaymentAttempt attempt = DataFactory.PaymentAttempt().Locate(dPaymentAttemptPaymentClientVirtual.paymentAttemptId);
        DPaymentAgentSetupPaymentClientVirtual agentsetup = DataFactory.PaymentAgentSetupPaymentClientVirtual().Locate(attempt.paymentAgentSetupId);
        DOrder order = DataFactory.Order().Locate(attempt.orderId);
        
        string rawHashData = agentsetup.secureHashSecret;

        if (Page.Request.QueryString["vpc_SecureHash"] != null && Page.Request.QueryString["vpc_SecureHash"].Length > 0)
        {
            dPaymentAttemptPaymentClientVirtual.vpc_SecureHash = Page.Request.QueryString["vpc_SecureHash"];

            // loop through all the data in the Page.Request.Form
            foreach (string item in Page.Request.QueryString)
            {
                // Collect the data required for the MD5 signature if required
                if (!item.Equals("vpc_SecureHash"))
                {
                    rawHashData += Page.Request.QueryString[item];
                }
            }

            // Create the MD5 signature if required
            string signature = PaymentClientVirtual.CreateMD5Signature(rawHashData);

            if (!dPaymentAttemptPaymentClientVirtual.vpc_SecureHash.Equals(signature))
                GenericHelper.RedirectToErrorPage("Chave de autenticação inválida, transação rejeitada");
        }
        else
            GenericHelper.RedirectToErrorPage("Chave de autenticação não disponível, transação rejeitada");

        dPaymentAttemptPaymentClientVirtual.vpc_TxnResponseCode = Page.Request.QueryString["vpc_TxnResponseCode"];
        dPaymentAttemptPaymentClientVirtual.vpc_Card = Page.Request.QueryString["vpc_Card"];
        dPaymentAttemptPaymentClientVirtual.vpc_ReceiptNo = Page.Request.QueryString["vpc_ReceiptNo"];
        dPaymentAttemptPaymentClientVirtual.vpc_AcqResponseCode = Page.Request.QueryString["vpc_AcqResponseCode"];
        dPaymentAttemptPaymentClientVirtual.vpc_Message = Page.Request.QueryString["vpc_Message"];
        dPaymentAttemptPaymentClientVirtual.vpc_AVSResultCode = Page.Request.QueryString["vpc_AVSResultCode"];
        dPaymentAttemptPaymentClientVirtual.vpc_AcqAVSRespCode = Page.Request.QueryString["vpc_AcqAVSRespCode"];
        dPaymentAttemptPaymentClientVirtual.paymentClientVirtualiStatus = (int)PaymentAttemptPaymentClientVirtualStatus.End;

        if (Page.Request.QueryString["vpc_BatchNo"] != null && Page.Request.QueryString["vpc_BatchNo"] != string.Empty)
            dPaymentAttemptPaymentClientVirtual.vpc_BatchNo = int.Parse(Page.Request.QueryString["vpc_BatchNo"]);

        if (Page.Request.QueryString["vpc_AuthorizeId"] != null && Page.Request.QueryString["vpc_AuthorizeId"] != string.Empty)
            dPaymentAttemptPaymentClientVirtual.vpc_AuthorizeId = int.Parse(Page.Request.QueryString["vpc_AuthorizeId"]);        

        if (Page.Request.QueryString["vpc_TransactionNo"] != null && Page.Request.QueryString["vpc_TransactionNo"] != string.Empty)
            dPaymentAttemptPaymentClientVirtual.vpc_TransactionNo = int.Parse(Page.Request.QueryString["vpc_TransactionNo"]);

        //Seto a attempt
        attempt.returnMessage = PaymentClientVirtual.getResponseDescription(dPaymentAttemptPaymentClientVirtual.vpc_TxnResponseCode);
        attempt.lastUpdate = DateTime.Now;

        // only display this data if not an error condition
        if (dPaymentAttemptPaymentClientVirtual.vpc_TxnResponseCode.Equals("0"))
        {
            attempt.status = (int)PaymentAttemptStatus.Paid;
        }
        else
        {
            attempt.status = (int)PaymentAttemptStatus.NotPaid;

            if (errorMessage == string.Empty)
                errorMessage = "Transação não autorizada";
        }

        DataFactory.PaymentAttemptPaymentClientVirtual().Update(dPaymentAttemptPaymentClientVirtual);
        DataFactory.PaymentAttempt().Update(attempt);
        GenericHelper.UpdateOrderStatusByAttemptStatus(order, attempt.status);
        
        transactionId = attempt.paymentAttemptId.ToString();
        errorNo = dPaymentAttemptPaymentClientVirtual.vpc_TxnResponseCode;

        GenericHelper.CloseWindow();
     }
}
