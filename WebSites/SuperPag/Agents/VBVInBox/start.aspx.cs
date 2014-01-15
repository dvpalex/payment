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

public partial class Agents_VBVInBox_start : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        Ensure.IsNotNullPage(Request["id"], "Post inválido iniciando uma transação VISA");
        
        DPaymentAttempt attempt = DataFactory.PaymentAttempt().Locate(new Guid(Request["id"]));
        Ensure.IsNotNullPage(attempt, "Tentativa de pagamento {0} não encontrada", Request["id"].ToString());
        DPaymentAttemptVBV attemptVBV = DataFactory.PaymentAttemptVBV().Locate(attempt.paymentAttemptId);

        // verifico se o guid passado na querystring já não foi utilizado anteriormente
        if (attemptVBV.vbvStatus != (byte)PaymentAttemptVBVStatus.Initial)
            GenericHelper.RedirectToErrorPage("Transação executada");
        
        DPaymentAgentSetupVBV agentsetup = DataFactory.PaymentAgentSetupVBV().Locate(attempt.paymentAgentSetupId);
        Ensure.IsNotNullPage(agentsetup, "A loja não está configurada corretamente para esse meio de pagamento");
        DOrder order = DataFactory.Order().Locate(attempt.orderId);

        System.IO.FileStream fs;
        StringBuilder tidfile = new StringBuilder();

        tidfile.Append("<MESSAGE>");
        tidfile.AppendFormat("<PRICE>{0}</PRICE>", attemptVBV.price);
        tidfile.Append("<AUTHENTTYPE>0</AUTHENTTYPE>");
        tidfile.Append("</MESSAGE>");

        byte[] XML = System.Text.Encoding.ASCII.GetBytes(tidfile.ToString());

        fs = new System.IO.FileStream(System.Configuration.ConfigurationManager.AppSettings["VBVDirectory"].ToString() + "\\requests\\" + attemptVBV.tid + ".xml", System.IO.FileMode.Create);
        fs.Write(XML, 0, XML.Length);
        fs.Close();

        ClientHttpRequisition post = new ClientHttpRequisition();
        post.FormName = "pay_VerifiedByVisa";
        post.Method = "POST";
        post.Url = System.Configuration.ConfigurationManager.AppSettings["VBVComponentUrl"] + "mpg.exe?";
        post.Parameters.Add("tid", attemptVBV.tid);
        post.Parameters.Add("order", "Pedido no. " + order.storeReferenceOrder + ": Itens: produto1 - " + GenericHelper.FormatCurrencyBrasil(attemptVBV.price / 100));
        post.Parameters.Add("orderid", attemptVBV.vbvOrderId);
        post.Parameters.Add("merchid", "cfg" + agentsetup.paymentAgentSetupId.ToString());
        post.Parameters.Add("free", attemptVBV.free);
        post.Parameters.Add("damount", GenericHelper.FormatCurrencyBrasil(((decimal)attemptVBV.price) / 100));

        attemptVBV.vbvStatus = (byte)PaymentAttemptVBVStatus.Mpg;
        DataFactory.PaymentAttemptVBV().Update(attemptVBV);

        post.Send();
    }
}
