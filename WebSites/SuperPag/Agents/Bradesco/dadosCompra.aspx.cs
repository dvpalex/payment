using System;
using System.Data;
using System.Configuration;
using System.Collections;
using System.Xml;
using System.Text;
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

public partial class Agents_Bradesco_dadosCompra : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        Ensure.IsNotNull(Request["PaymentAttemptId"], "Sessão inválida tentando recuperar o código de transação Bradesco.");
        
        //Recupero a attempt
        DPaymentAttempt attempt = DataFactory.PaymentAttempt().Locate(new Guid(Request["PaymentAttemptId"]));
        Ensure.IsNotNullPage(attempt, "Não foi possível recuperar a tentativa de pagamento do Pagamento Fácil Bradesco.");
        DPaymentAttemptBradesco attemptBradesco = DataFactory.PaymentAttemptBradesco().Locate(attempt.paymentAttemptId);
        Ensure.IsNotNullPage(attemptBradesco, "Não foi possível recuperar a tentativa de pagamento do Pagamento Fácil Bradesco.");
        DPaymentAgentSetupBradesco agentsetup = DataFactory.PaymentAgentSetupBradesco().Locate(attempt.paymentAgentSetupId);
        DOrder order = DataFactory.Order().Locate(attempt.orderId);

        attemptBradesco.bradescoStatus = (int)PaymentAttemptBradescoStatus.OrderNotification;
        attemptBradesco.numOrder = Request["numOrder"];
        attemptBradesco.merchantid = Request["merchantid"];
        string transId = Request["transId"];
        DataFactory.PaymentAttemptBradesco().Update(attemptBradesco);

        StringBuilder dadosCompra = new StringBuilder();

        switch(transId.ToLower())
        {
            case "getorder":
                dadosCompra.Append("<BEGIN_ORDER_DESCRIPTION>");
                dadosCompra.Append(String.Format("<orderid>=({0})", attemptBradesco.numOrder));
                dadosCompra.Append("<descritivo>=(PRODUTOS)");
                dadosCompra.Append("<quantidade>=(1)");
                dadosCompra.Append("<unidade>=(un)");
                dadosCompra.Append(String.Format("<valor>=({0})",GenericHelper.ParseString(attempt.price)));
                dadosCompra.Append("<END_ORDER_DESCRIPTION>");

                //DOrder order = DataFactory.Order().Locate(attempt.orderId);
                //DOrderItem[] normalOrderItens = DataFactory.OrderItem().List(attempt.orderId, (int)ItemTypes.Regular);
                //DOrderItem[] shippingOrderItens = DataFactory.OrderItem().List(attempt.orderId, (int)ItemTypes.ShippingRate);
                //foreach(DOrderItem item in normalOrderItens)
                //{
                //    dadosCompra.Append(String.Format("<descritivo>=({0})",item.itemDescription));
                //    dadosCompra.Append(String.Format("<quantidade>=({0})",item.itemQuantity));
                //    dadosCompra.Append("<unidade>=(un)");
                //    dadosCompra.Append(String.Format("<valor>=({0})",GenericHelper.ParseString(item.itemValue)));
                //}
                break;
            case "putauth":
                attemptBradesco.cod = Request["cod"];
                attemptBradesco.cctype = Request["cctype"];
                attemptBradesco.ccname = Request["ccname"];
                attemptBradesco.ccemail = Request["ccemail"];
                attemptBradesco.numparc = Request["numparc"];
                attemptBradesco.valparc = Request["valparc"];
                attemptBradesco.valtotal = Request["valtotal"];
                attemptBradesco.prazo = Request["prazo"];
                attemptBradesco.tipoPagto = int.Parse(Request["tipopagto"]);
                attemptBradesco.assinatura = Request["assinatura"];
                attemptBradesco.bradescoStatus = (int)PaymentAttemptBradescoStatus.Confirmation;
                DataFactory.PaymentAttemptBradesco().Update(attemptBradesco);
                
                if (Request["if"].ToLower() == "bradesco" && int.Parse(attemptBradesco.cod) == 0)
                {
                    attempt.status = (int)PaymentAttemptStatus.Paid;
                    dadosCompra.Append("<PUT_AUTH_OK>");
                }
                else
                {
                    attempt.returnMessage = "Transação não autorizada pelo Gateway do Bradesco.";
                    attempt.status = (int)PaymentAttemptStatus.NotPaid;
                    dadosCompra.Append("<ERRO>");
                }
                DataFactory.PaymentAttempt().Update(attempt);
                GenericHelper.UpdateOrderStatusByAttemptStatus(order, attempt.status);

                break;
            case "getboleto":
                dadosCompra.Append("<BEGIN_ORDER_DESCRIPTION>");
                //TODO: BRADESCO SPS BOLETO
                dadosCompra.Append("<END_ORDER_DESCRIPTION>");
                break;
        }

        Response.Write(dadosCompra.ToString());
        Response.End();
    }
}
