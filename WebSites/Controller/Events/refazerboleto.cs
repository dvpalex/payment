using System;
using System.Collections;
using SuperPag.Framework.Web.WebController;
using SuperPag.Framework.Web;
using SuperPag.Business.Messages;
using SuperPag.Business;
using Controller.Lib;
using Controller.Lib.Commands;
using SuperPag.Data.Messages;
using SuperPag.Data;
using SuperPag.Agents.Boleto;
using SuperPag;
using System.Web;

namespace Controller.Lib.Views.Ev.Boleto
{
    public class Refazer : BaseEvent
    {
        protected override BaseCommand OnExecute()
        {
            BaseCommand b = null;

            string storeId = "";

            MPaymentAttempt mPaymentAttempt = (MPaymentAttempt)this.GetMessage(typeof(MPaymentAttempt));
            MPaymentAttemptBoleto mPaymentAttemptBoleto = (MPaymentAttemptBoleto)this.GetMessage(typeof(MPaymentAttemptBoleto));

            storeId = mPaymentAttempt.Order.Store.StoreId.ToString();

            DPaymentAgentSetupBoleto dPaymentAgentSetupBoleto = DataFactory.PaymentAgentSetupBoleto().Locate(mPaymentAttempt.PaymentAgentSetupId);
            Ensure.IsNotNull(dPaymentAgentSetupBoleto, "A loja não está configurada corretamente para esse meio de pagamento");

            decimal installmentValue = mPaymentAttempt.Price;
            if (mPaymentAttempt.Price == decimal.MinValue)
            {
                DOrderInstallment dOrderInstallment = DataFactory.OrderInstallment().Locate(mPaymentAttempt.Order.OrderId, mPaymentAttempt.InstallmentNumber);
                Ensure.IsNotNull(dOrderInstallment, "A Parcela {0} do pedido {1} não foi encontrada", mPaymentAttempt.InstallmentNumber, mPaymentAttempt.Order.OrderId);
                installmentValue = dOrderInstallment.installmentValue;
            }

            DPaymentAttempt paymentAttemptNew = new DPaymentAttempt();
            paymentAttemptNew.paymentAttemptId = Guid.NewGuid();
            paymentAttemptNew.orderId = mPaymentAttempt.Order.OrderId;
            paymentAttemptNew.paymentFormId = mPaymentAttempt.PaymentForm.PaymentFormId;
            paymentAttemptNew.paymentAgentSetupId = mPaymentAttempt.PaymentAgentSetupId;
            paymentAttemptNew.price = mPaymentAttempt.Price;
            paymentAttemptNew.startTime = DateTime.Now;
            paymentAttemptNew.lastUpdate = DateTime.Now;
            paymentAttemptNew.step = mPaymentAttempt.Step;
            paymentAttemptNew.installmentNumber = mPaymentAttempt.InstallmentNumber;
            paymentAttemptNew.status = (int)mPaymentAttempt.Status;
            paymentAttemptNew.returnMessage = mPaymentAttempt.ReturnMessage;
            paymentAttemptNew.billingScheduleId = mPaymentAttempt.BillingScheduleId;
            paymentAttemptNew.isSimulation = mPaymentAttempt.IsSimulation;
            DataFactory.PaymentAttempt().Insert(paymentAttemptNew);

            DPaymentAttemptBoleto paymentAttemptBoletoNew = new DPaymentAttemptBoleto();
            paymentAttemptBoletoNew.paymentAttemptId = paymentAttemptNew.paymentAttemptId;
            paymentAttemptBoletoNew.documentNumber = mPaymentAttemptBoleto.DocumentNumber;
            paymentAttemptBoletoNew.withdraw = mPaymentAttemptBoleto.Withdraw;
            paymentAttemptBoletoNew.withdrawDoc = mPaymentAttemptBoleto.WithdrawDoc;
            paymentAttemptBoletoNew.address1 = mPaymentAttemptBoleto.Address1;
            paymentAttemptBoletoNew.address2 = mPaymentAttemptBoleto.Address2;
            paymentAttemptBoletoNew.address3 = mPaymentAttemptBoleto.Address3;
            paymentAttemptBoletoNew.paymentDate = mPaymentAttemptBoleto.PaymentDate;
            paymentAttemptBoletoNew.instructions = mPaymentAttemptBoleto.Instructions;
            paymentAttemptBoletoNew.expirationPaymentDate = mPaymentAttemptBoleto.ExpirationPaymentDate;
            DataFactory.PaymentAttemptBoleto().Insert(paymentAttemptBoletoNew);

            #region Cálculo do código de barras, nosso número e oct
            BoletosBancariosInfo boletosBancariosInfo = new BoletosBancariosInfo();
            boletosBancariosInfo.Carteira = dPaymentAgentSetupBoleto.wallet;
            boletosBancariosInfo.Convenio = dPaymentAgentSetupBoleto.conventionNumber;
            boletosBancariosInfo.CodBanco = dPaymentAgentSetupBoleto.bankNumber;
            boletosBancariosInfo.CodMoeda = "9";
            boletosBancariosInfo.DataVencimento = paymentAttemptBoletoNew.expirationPaymentDate;
            boletosBancariosInfo.NossoNumero = paymentAttemptBoletoNew.agentOrderReference.ToString();
            boletosBancariosInfo.ValorBoleto = installmentValue;
            boletosBancariosInfo.CalculaFatorVencimento = true;
            boletosBancariosInfo.ContaCorrente = dPaymentAgentSetupBoleto.accountNumber.ToString();
            boletosBancariosInfo.Agencia = dPaymentAgentSetupBoleto.agencyNumber.ToString();
            boletosBancariosInfo.CodigoPedidoLoja = paymentAttemptBoletoNew.documentNumber;

            string codigoBarras = "";
            string linhaDigitavel = "";
            string nossoNumero = "";
            switch (mPaymentAttempt.PaymentForm.PaymentFormId)
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
            #endregion

            paymentAttemptBoletoNew.oct = linhaDigitavel;
            paymentAttemptBoletoNew.barCode = codigoBarras;
            paymentAttemptBoletoNew.ourNumber = nossoNumero;
            DataFactory.PaymentAttemptBoleto().Update(paymentAttemptBoletoNew);
            
            b = this.MakeCommand(typeof(ShowOrder));
            b.Parameters["OrderId"] = mPaymentAttempt.Order.OrderId;

            return b;
        }
    }
}
