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
using System.Collections.Specialized;
using SuperPag.Data.Messages;
using SuperPag.Data;
using SuperPag.Helper;
using SuperPag;
using SuperPag.Handshake.Html;
using System.Text;

public partial class Agents_ABN_retorno : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        Ensure.IsNotNullPage(Request["aid"], "Post inválido tentando recuperar o ID de uma transação do Financiamento ABN/AMRO");

        int agentOrderReference;
        if (!int.TryParse(Request["aid"].ToString(), out agentOrderReference))
            Ensure.IsNotNullPage(null, "Id de transação do Financiamento ABN/AMRO inválido");

        DPaymentAttemptABN attemptABN = DataFactory.PaymentAttemptABN().Locate(agentOrderReference);
        Ensure.IsNotNullPage(attemptABN, "Não foi possível resgatar a transação {0} do Financiamento ABN/AMRO", agentOrderReference);
        DPaymentAttempt attempt = DataFactory.PaymentAttempt().Locate(attemptABN.paymentAttemptId);
        DPaymentAgentSetupABN agentsetup = DataFactory.PaymentAgentSetupABN().Locate(attempt.paymentAgentSetupId);
        Ensure.IsNotNullPage(agentsetup, "Financiamento ABN/AMRO não configurado corretamente para esta loja");
        DOrder order = DataFactory.Order().Locate(attempt.orderId);

        if (!int.TryParse(Request["RET01"], out attemptABN.codRet))
        {
            attemptABN.abnStatus = (int)PaymentAttemptABNStatus.End;
            attempt.status = (int)PaymentAttemptStatus.NotPaid;
            attempt.lastUpdate = DateTime.Now;

            attemptABN.codRet = 0;
            attemptABN.msgRet = String.Format("Código de retorno inválido. Código: {0}. Mensagem: {1}.", Request["RET01"], Request["RET02"]);

            DataFactory.PaymentAttempt().Update(attempt);
            DataFactory.PaymentAttemptABN().Update(attemptABN);
            GenericHelper.UpdateOrderStatusByAttemptStatus(order, attempt.status);

            Response.Redirect("~/Agents/ABN/popupclose.aspx?id=" + attempt.paymentAttemptId);
        }

        attemptABN.msgRet = Request["RET02"];
        attemptABN.numProposta = Request["RET03"];

        //TODO: checar campos que vem da base
        if (!String.IsNullOrEmpty(Request["RET04"]))
            attemptABN.qtdPrestacao = int.Parse(Request["RET04"]);
        if (!String.IsNullOrEmpty(Request["RET05"]))
            attemptABN.prestacao = decimal.Parse(Request["RET05"]);
        if (!String.IsNullOrEmpty(Request["RET06"]))
            attemptABN.valorEntrada = decimal.Parse(Request["RET06"]);

        attemptABN.numControle = !String.IsNullOrEmpty(Request["RET07"]) ? decimal.Parse(Request["RET07"]) : decimal.MinValue;

        switch (attemptABN.codRet)
        {
            case 1:
                //Aprovada
            case 2:
                //Em análise

                //Fazemos a verificação com o ABN para evitar fraudes
                ServerHttpHtmlRequisition post = new ServerHttpHtmlRequisition();
                post.Url = agentsetup.urlConsulta.Trim();
                post.Parameters.Add("VAR004", "0");
                post.Parameters.Add("VAR005", Request["RET03"]);
                post.Parameters.Add("VAR115", "P");
                post.Send();

                //TODO: Analisar retorno, se for de sistema indisponível,
                //pegar valores atuais, mas setar sempre como em análise
                //para um checagem posterior quanto na sonda.
                string teste = post.Response;

                attemptABN.statusProposta = "AN";
                attemptABN.abnStatus = (int)PaymentAttemptABNStatus.End;
                attempt.status = (int)PaymentAttemptStatus.PendingPaid;
                attempt.lastUpdate = DateTime.Now;
            
                DataFactory.PaymentAttempt().Update(attempt);
                DataFactory.PaymentAttemptABN().Update(attemptABN);
                GenericHelper.UpdateOrderStatusByAttemptStatus(order, attempt.status);

                Response.Redirect("~/finalization.aspx?id=" + attempt.paymentAttemptId);
                break;
            case 3:
                //Cancelada
            case 0:
                //Erro
            default:
                //Erro
                attempt.status = (int)PaymentAttemptStatus.NotPaid;
                attemptABN.abnStatus = (int)PaymentAttemptABNStatus.End;
                attempt.lastUpdate = DateTime.Now;

                DataFactory.PaymentAttempt().Update(attempt);
                DataFactory.PaymentAttemptABN().Update(attemptABN);
                GenericHelper.UpdateOrderStatusByAttemptStatus(order, attempt.status);

                Response.Redirect("~/Agents/ABN/popupclose.aspx?id=" + attempt.paymentAttemptId);
                break;
        }
    }
}
