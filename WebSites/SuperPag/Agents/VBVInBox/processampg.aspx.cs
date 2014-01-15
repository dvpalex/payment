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
using System.Xml;
using System.IO;
using System.Text;
using SuperPag.Data.Messages;
using SuperPag.Data;
using SuperPag;
using SuperPag.Helper;

public partial class Agents_VBVInBox_processampg : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        Ensure.IsNotNullPage(Request["TID"], "Post inválido tentando recuperar o TID de uma transação VISA");

        string Path = System.Configuration.ConfigurationManager.AppSettings["VBVDirectory"] + "\\results\\" + Request["TID"] + ".xml";
        string LR;
        string ARP;
        string PRICE;
        string ORDERID;
        string FREE;
        string PAN;
        string BANK;
        string ARS;
        string AUTHENT;
        string TID;

        System.IO.FileInfo FI = new System.IO.FileInfo(Path);

        // Via LocalHost, o XML não é gerado
        if (FI.Exists == false)
        {
            TID = Request["TID"];
            LR = Request["LR"];
            ARP = Request["ARP"];
            PRICE = Request["PRICE"];
            ORDERID = Request["ORDERID"];
            FREE = Request["FREE"];
            PAN = Request["PAN"];
            BANK = Request["BANK"];
            ARS = Request["ARS"];
            AUTHENT = Request["AUTHENT"];
        }
        else
        {
            XmlDocument xml = new XmlDocument();

            FileStream fs1 = new FileStream(Path, FileMode.Open, FileAccess.Read);
            using (fs1)
            {
                StreamReader sr = new StreamReader(fs1, System.Text.Encoding.GetEncoding("iso-8859-1"));
                using (sr)
                {
                    xml.Load(sr);
                }
            }
            TID = xml.DocumentElement.GetElementsByTagName("TID").Item(0).InnerText;
            LR = xml.DocumentElement.GetElementsByTagName("LR").Item(0).InnerText;
            ARP = xml.DocumentElement.GetElementsByTagName("ARP").Item(0).InnerText;
            PRICE = xml.DocumentElement.GetElementsByTagName("PRICE").Item(0).InnerText;
            ORDERID = xml.DocumentElement.GetElementsByTagName("ORDERID").Item(0).InnerText;
            FREE = xml.DocumentElement.GetElementsByTagName("FREE").Item(0).InnerText;
            PAN = xml.DocumentElement.GetElementsByTagName("PAN").Item(0).InnerText;
            BANK = xml.DocumentElement.GetElementsByTagName("BANK").Item(0).InnerText;
            ARS = xml.DocumentElement.GetElementsByTagName("ARS").Item(0).InnerText;
            AUTHENT = xml.DocumentElement.GetElementsByTagName("AUTHENT").Item(0).InnerText;
        }

        Ensure.IsNotNullPage(FREE, "Arquivo ou post inválido tentando recuperar o FREE de uma transação VISA");
        
        DPaymentAttemptVBV attemptVBV = DataFactory.PaymentAttemptVBV().Locate(new Guid(FREE));
        Ensure.IsNotNullPage(attemptVBV, "Tentativa de pagamento {0} não encontrada", FREE);
        
        // verifico se o TID retornado no posto é mesmo que está gravado
        // no registro recupera pelo paymentAttemptId
        if (TID != attemptVBV.tid)
            GenericHelper.RedirectToErrorPage("TID inconsistente");

        DPaymentAttempt attempt = DataFactory.PaymentAttempt().Locate(attemptVBV.paymentAttemptId);
        DOrder order = DataFactory.Order().Locate(attempt.orderId);

        // Checa preço do retorno com o da ordem para
        // certificar que não foi adulterado
        if (PRICE != null && attempt.price != (int.Parse(PRICE) / 100.0m))
            GenericHelper.RedirectToErrorPage("Arquivo de retorno adulterado");

        // Recupera Session perdida
        Session["PaymentAttemptId"] = attempt.paymentAttemptId;

        attemptVBV.lr = GenericHelper.ParseInt(LR);
        attemptVBV.pan = PAN;
        attemptVBV.bank = GenericHelper.ParseInt(BANK);
        attemptVBV.ars = ARS;
        attemptVBV.authent = GenericHelper.ParseInt(AUTHENT);
        attemptVBV.arp = GenericHelper.ParseInt(ARP);

        if (attemptVBV.lr == 0 || attemptVBV.lr == 11)
        {
            DPaymentAgentSetupVBV agentsetup = DataFactory.PaymentAgentSetupVBV().Locate(attempt.paymentAgentSetupId);
            
            if (agentsetup.autoCapture)
            {
                attemptVBV.vbvStatus = (byte)PaymentAttemptVBVStatus.Capture;
                DataFactory.PaymentAttemptVBV().Update(attemptVBV);

                // Captura
                ClientHttpRequisition post = new ClientHttpRequisition();
                post.FormName = "pay_VerifiedByVisa";
                post.Method = "POST";
                post.Url = System.Configuration.ConfigurationManager.AppSettings["VBVComponentUrl"] + "capture.exe?";
                post.Parameters.Add("tid", TID);
                post.Parameters.Add("merchid", "cfg" + agentsetup.paymentAgentSetupId.ToString());
                post.Parameters.Add("free", attemptVBV.free);
                post.Send();
            }
            else
            {
                attempt.status = (int)PaymentAttemptStatus.Paid;
                attempt.lastUpdate = DateTime.Now;
                if(!String.IsNullOrEmpty(ARS))
                    attempt.returnMessage = ARS;
                else
                    attempt.returnMessage = "mensagem desconhecido";

                attemptVBV.vbvStatus = (byte)PaymentAttemptVBVStatus.WaitingCapture;
                DataFactory.PaymentAttemptVBV().Update(attemptVBV);
                DataFactory.PaymentAttempt().Update(attempt);
                GenericHelper.UpdateOrderStatusByAttemptStatus(order, attempt.status);

                Response.Redirect("~/finalization.aspx?id=" + attempt.paymentAttemptId);
            }
        }

        attemptVBV.vbvStatus = (byte)PaymentAttemptVBVStatus.End;
        attempt.status = (int)PaymentAttemptStatus.NotPaid;
        attempt.lastUpdate = DateTime.Now;
        attempt.returnMessage = ARS;
        
        DataFactory.PaymentAttempt().Update(attempt);
        DataFactory.PaymentAttemptVBV().Update(attemptVBV);
        GenericHelper.UpdateOrderStatusByAttemptStatus(order, attempt.status);

        Response.Redirect("~/Agents/VBVInBox/popupclose.aspx?id=" + attempt.paymentAttemptId);
    }
}
