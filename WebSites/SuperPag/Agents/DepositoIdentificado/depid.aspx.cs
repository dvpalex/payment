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

public partial class Agents_DepositoIdentificado_depid : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        Ensure.IsNotNullPage(Session["PaymentAttemptId"], "Sessão inválida iniciando uma transação de boleto");

        List<Guid> attemptIdList = (List<Guid>)Session["PaymentAttemptId"];
        List<InfoDepId> infoDepIdList = new List<InfoDepId>();

        DPaymentAttempt attempt = DataFactory.PaymentAttempt().Locate(attemptIdList[0]);
        Ensure.IsNotNullPage(attempt, "Tentativa de pagamento {0} não encontrada", attemptIdList[0].ToString());
        DPaymentAgentSetupDepId dPaymentAgentSetupDepId = DataFactory.PaymentAgentSetupDepId().Locate(attempt.paymentAgentSetupId);
        Ensure.IsNotNullPage(dPaymentAgentSetupDepId, "A loja não está configurada corretamente para esse meio de pagamento");
        DOrder dOrder = DataFactory.Order().Locate(attempt.orderId);

        //Seto o status do pedido
        GenericHelper.SetOrderStatus(HttpContext.Current, WorkflowOrderStatus.AgentCalled, attempt.paymentFormId + "," + dOrder.installmentQuantity + "," + (int)PaymentAgents.DepositoIdentificado);
        int count = 0;
        foreach (Guid attemptId in attemptIdList)
        {
            attempt = DataFactory.PaymentAttempt().Locate(attemptId);
            DOrderInstallment dOrderInstallment = DataFactory.OrderInstallment().Locate(attempt.orderId, attempt.installmentNumber);
            DConsumer dConsumer = DataFactory.Consumer().Locate(dOrder.consumerId);
            DConsumerAddress dConsumerAddressBilling = DataFactory.ConsumerAddress().Locate(dConsumer.consumerId, 1);

            attempt.lastUpdate = DateTime.Now;
            attempt.status = (int)PaymentAttemptStatus.PendingPaid;
            DataFactory.PaymentAttempt().Update(attempt);
            GenericHelper.UpdateOrderStatusByAttemptStatus(dOrder, attempt.status);

            DPaymentAttemptDepId dPaymentAttemptDepId = new DPaymentAttemptDepId();
            dPaymentAttemptDepId.paymentAttemptId = attemptId;
            dPaymentAttemptDepId.bankNumber = dPaymentAgentSetupDepId.bankNumber;
            dPaymentAttemptDepId.dueDate = DateTime.Now.AddDays(dPaymentAgentSetupDepId.expirationDays).AddMonths(count);
            dPaymentAttemptDepId.paymentStatus = (int)SuperPag.DepIdStatusEnum.ReturnNotFound;
            
            switch (attempt.paymentFormId)
            {
                case (int)PaymentForms.DepositoIdentificadoBradesco:
                    string partialIdNumber = SuperPag.Agents.DepId.DepId.GetIdFromPattern(dOrder.storeReferenceOrder, dPaymentAttemptDepId.agentOrderReference, dPaymentAgentSetupDepId.idPattern, dOrderInstallment.installmentNumber);
                    Ensure.IsNotNullOrEmptyPage(partialIdNumber, "Número do pedido {0} inválido para gerar identificação do depósito.", dOrder.storeReferenceOrder);
                    dPaymentAttemptDepId.idNumber = String.Format("{0}-{1}", partialIdNumber, SuperPag.Agents.DepId.DepIdBradesco.CalculaDigitoModulo7(partialIdNumber));
                    break;
            }

            DataFactory.PaymentAttemptDepId().Insert(dPaymentAttemptDepId);

            InfoDepId infoDepId = new InfoDepId();
            infoDepId.InstallmentValue = dOrderInstallment.installmentValue;
            infoDepId.IdNumber = dPaymentAttemptDepId.idNumber;
            infoDepId.PaymentAttemptId = attemptId;
            infoDepId.InstallmentNumber = dOrderInstallment.installmentNumber;
            infoDepIdList.Add(infoDepId);
        }

        //seto a primeira attempt para finalizar
        Session["PaymentAttemptId"] = attemptIdList[0];

        ControlInfoDepId depIdControlInfo = new ControlInfoDepId();
        depIdControlInfo.StoreId = dOrder.storeId;
        depIdControlInfo.Path = "~/Controls/depId.ascx";
        depIdControlInfo.InfoDepIdList = infoDepIdList;
        depIdControlInfo.BankNumber = dPaymentAgentSetupDepId.bankNumber;
        depIdControlInfo.AgencyNumber = dPaymentAgentSetupDepId.agencyNumber;
        depIdControlInfo.AgencyDigit = dPaymentAgentSetupDepId.agencyDigit;
        depIdControlInfo.AccountNumber = dPaymentAgentSetupDepId.accountNumber;
        Session["FinalizationControlInfo"] = depIdControlInfo;

        Response.Redirect("~/finalization.aspx?id=" + attemptIdList[0]);
    }
}
