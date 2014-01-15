using System;
using System.Data;
using System.Configuration;
using System.Collections;
using System.Collections.Generic;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using SuperPag.Data;
using SuperPag.Data.Messages;
using SuperPag.Agents.Boleto;
using SuperPag;
using SuperPag.Helper;
using System.Xml;

public partial class Agents_Boleto_boleto : System.Web.UI.Page
{

    protected void Page_Load(object sender, EventArgs e)
    {
        Ensure.IsNotNullPage(Session["PaymentAttemptId"], "Sessão inválida iniciando uma transação de boleto");
        
        List<Guid> attemptIdList = (List<Guid>)Session["PaymentAttemptId"];
        List<InfoBoleto> infoBoletoList = new List<InfoBoleto>();

        DPaymentAttempt attempt = DataFactory.PaymentAttempt().Locate(attemptIdList[0]);
        Ensure.IsNotNullPage(attempt, "Tentativa de pagamento {0} não encontrada", attemptIdList[0].ToString());
        DPaymentAgentSetupBoleto dPaymentAgentSetupBoletoBB = DataFactory.PaymentAgentSetupBoleto().Locate(attempt.paymentAgentSetupId);
        Ensure.IsNotNullPage(dPaymentAgentSetupBoletoBB, "A loja não está configurada corretamente para esse meio de pagamento");
        DOrder order = DataFactory.Order().Locate(attempt.orderId);

        //Seto o status do pedido
        GenericHelper.SetOrderStatus(HttpContext.Current, WorkflowOrderStatus.AgentCalled, attempt.paymentFormId + "," + order.installmentQuantity + "," + (int)PaymentAgents.Boleto);

        //setamos a data fora do loop
        DateTime paymentDate = DateTime.Today;
        DateTime expirationPaymentDate;
        if (Session["htmlHandshake"] != null)
            expirationPaymentDate = GenericHelper.ParseDateyyyyMMdd(GenericHelper.GetSingleNodeString(Session["htmlHandshake"].ToString(), "/root/form/data_boleto"));
        else
            expirationPaymentDate = GenericHelper.ParseDateyyyyMMdd(GenericHelper.GetSingleNodeString(Session["xmlHandshake"].ToString(), "/pedido/parametros_opcionais/data_boleto"));

        if (expirationPaymentDate == DateTime.MinValue || expirationPaymentDate < paymentDate)
            expirationPaymentDate = paymentDate.AddDays(dPaymentAgentSetupBoletoBB.expirationDays);

        int storeId = 0;

        foreach (Guid attemptId in attemptIdList)
        {
            attempt = DataFactory.PaymentAttempt().Locate(attemptId);
            DOrder dOrder = DataFactory.Order().Locate(attempt.orderId);
            DOrderInstallment dOrderInstallment = DataFactory.OrderInstallment().Locate(attempt.orderId, attempt.installmentNumber);
            DConsumer dConsumer = DataFactory.Consumer().Locate(dOrder.consumerId);
            DConsumerAddress dConsumerAddressBilling = DataFactory.ConsumerAddress().Locate(dConsumer.consumerId, 1);

            storeId = dOrder.storeId;

            attempt.lastUpdate = DateTime.Now;
            attempt.status = (int)PaymentAttemptStatus.PendingPaid;
            DataFactory.PaymentAttempt().Update(attempt);
            GenericHelper.UpdateOrderStatusByAttemptStatus(dOrder, attempt.status);

            DPaymentAttemptBoleto dPaymentAttemptBoleto = new DPaymentAttemptBoleto();
            dPaymentAttemptBoleto.paymentAttemptId = attemptId;
            dPaymentAttemptBoleto.documentNumber = dOrder.storeReferenceOrder;
            dPaymentAttemptBoleto.withdraw = dConsumer.name;
            dPaymentAttemptBoleto.withdrawDoc = (String.IsNullOrEmpty(dConsumer.CNPJ) ? dConsumer.CPF : dConsumer.CNPJ);
            dPaymentAttemptBoleto.address1 = dConsumerAddressBilling.logradouro + " " + dConsumerAddressBilling.address + ", " + dConsumerAddressBilling.addressNumber + " " + dConsumerAddressBilling.addressComplement;
            dPaymentAttemptBoleto.address2 = dConsumerAddressBilling.district;
            dPaymentAttemptBoleto.address3 = dConsumerAddressBilling.cep + " - " + dConsumerAddressBilling.city + " - " + dConsumerAddressBilling.state;
            dPaymentAttemptBoleto.paymentDate = paymentDate;

            if (Session["htmlHandshake"] != null)
                dPaymentAttemptBoleto.instructions = GenericHelper.GetSingleNodeString(Session["htmlHandshake"].ToString(), "/root/form/instrucao_boleto");
            else
                dPaymentAttemptBoleto.instructions = GenericHelper.GetSingleNodeString(Session["xmlHandshake"].ToString(), "//instrucao_boleto");

            dPaymentAttemptBoleto.expirationPaymentDate = expirationPaymentDate;
            DataFactory.PaymentAttemptBoleto().Insert(dPaymentAttemptBoleto);

            BoletosBancariosInfo boletosBancariosInfo = new BoletosBancariosInfo();
            boletosBancariosInfo.Carteira = dPaymentAgentSetupBoletoBB.wallet;
            boletosBancariosInfo.Convenio = dPaymentAgentSetupBoletoBB.conventionNumber;
            boletosBancariosInfo.CodBanco = dPaymentAgentSetupBoletoBB.bankNumber;
            boletosBancariosInfo.CodMoeda = "9";
            boletosBancariosInfo.DataVencimento = dPaymentAttemptBoleto.expirationPaymentDate;
            boletosBancariosInfo.NossoNumero = dPaymentAttemptBoleto.agentOrderReference.ToString();
            boletosBancariosInfo.ValorBoleto = dOrderInstallment.installmentValue;
            boletosBancariosInfo.CalculaFatorVencimento = true;
            boletosBancariosInfo.ContaCorrente = dPaymentAgentSetupBoletoBB.accountNumber.ToString();
            boletosBancariosInfo.Agencia = dPaymentAgentSetupBoletoBB.agencyNumber.ToString();
            boletosBancariosInfo.CodigoPedidoLoja = dPaymentAttemptBoleto.documentNumber;

            string codigoBarras = "";
            string linhaDigitavel = "";
            string nossoNumero = "";
            switch (attempt.paymentFormId)
            {
                case (int)PaymentForms.BoletoBancoDoBrasil:
                    BoletoBB boletoBB = new BoletoBB(boletosBancariosInfo);
                    nossoNumero = boletoBB.ObtemNossoNumero();
                    codigoBarras = boletoBB.ObtemCodigoBarra();
                    linhaDigitavel = boletoBB.LinhaDigitavel(codigoBarras);
                    break;
                case (int)PaymentForms.BoletoBradesco:
                    BoletoBradesco boletoBradesco = new BoletoBradesco(boletosBancariosInfo);
                    nossoNumero = boletoBradesco.ObtemNossoNumero();
                    codigoBarras = boletoBradesco.ObtemCodigoBarra();
                    linhaDigitavel = boletoBradesco.LinhaDigitavel(codigoBarras);
                    break;
                case (int)PaymentForms.BoletoItau:
                    BoletoItau boletoItau = new BoletoItau(boletosBancariosInfo);
                    nossoNumero = boletoItau.ObtemNossoNumero();
                    codigoBarras = boletoItau.ObtemCodigoBarra();
                    linhaDigitavel = boletoItau.LinhaDigitavel(codigoBarras);
                    break;
                case (int)PaymentForms.BoletoHSBC:
                    BoletoHSBC boletoHSBC = new BoletoHSBC(boletosBancariosInfo);
                    nossoNumero = boletoHSBC.ObtemNossoNumero();
                    codigoBarras = boletoHSBC.ObtemCodigoBarra();
                    linhaDigitavel = boletoHSBC.LinhaDigitavel(codigoBarras);
                    break;
            }

            dPaymentAttemptBoleto.oct = linhaDigitavel;
            dPaymentAttemptBoleto.barCode = codigoBarras;
            dPaymentAttemptBoleto.ourNumber = nossoNumero;

            DataFactory.PaymentAttemptBoleto().Update(dPaymentAttemptBoleto);

            InfoBoleto infoBoleto = new InfoBoleto();
            infoBoleto.SederName = dConsumer.name;
            infoBoleto.InstallmentValue = dOrderInstallment.installmentValue;
            infoBoleto.Billing = linhaDigitavel;
            infoBoleto.PaymentAttemptId = attemptId;
            infoBoleto.InstallmentNumber = dOrderInstallment.installmentNumber;
            infoBoletoList.Add(infoBoleto);

            expirationPaymentDate = expirationPaymentDate.AddMonths(1);
        }

        //seto a primeira attempt para finalizar
        Session["PaymentAttemptId"] = attemptIdList[0];

        ControlInfoBoleto boletoControlInfo = new ControlInfoBoleto();
        boletoControlInfo.StoreId = storeId;
        boletoControlInfo.Path = "~/Controls/boleto.ascx";
        boletoControlInfo.InfoBoletoList = infoBoletoList;
        Session["FinalizationControlInfo"] = boletoControlInfo;

        Response.Redirect("~/finalization.aspx?id=" + attemptIdList[0]);
    }
}
