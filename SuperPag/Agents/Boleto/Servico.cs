using System;
using System.Collections.Generic;
using System.Text;
using SuperPag.Helper.Xml.Response;
using SuperPag.Helper.Xml.Request;
using SuperPag.Helper;
using SuperPag.Data.Messages;
using SuperPag.Data;
using System.Web;
using System.Configuration;

namespace SuperPag.Agents.Boleto
{
    class SystemInterface
    {
        public static responseOrdersOrderPaymentInstallment ProcessPayment(DOrder order, DOrderInstallment orderInstallment, DPaymentAttempt attempt, SuperPag.Helper.Xml.Request.genericPaymentFormDetailBoletoInformation paymentInfo)
        {
            Ensure.IsNotNull(order, "O pedido deve ser informado");
            Ensure.IsNotNull(attempt, "A tentativa de pagamento deve ser informada");
            Ensure.IsNotNull(orderInstallment, "A parcela relativa deve ser informada");
            Ensure.IsNotNull(paymentInfo, "Os dados do boleto devem ser informados");

            DPaymentAgentSetupBoleto dPaymentAgentSetupBoletoBB = DataFactory.PaymentAgentSetupBoleto().Locate(attempt.paymentAgentSetupId);
            Ensure.IsNotNull(dPaymentAgentSetupBoletoBB, "A loja não está configurada corretamente para esse meio de pagamento");
            
            DConsumer dConsumer = DataFactory.Consumer().Locate(order.consumerId);
            DConsumerAddress dConsumerAddressBilling = DataFactory.ConsumerAddress().Locate(dConsumer.consumerId, 1);

            //Inicializo as classes para retorno
            responseOrdersOrderPaymentInstallment installment = new SuperPag.Helper.Xml.Response.responseOrdersOrderPaymentInstallment();
            responseOrdersOrderPaymentInstallmentPaymentFormDetail pfDetail = new SuperPag.Helper.Xml.Response.responseOrdersOrderPaymentInstallmentPaymentFormDetail();
            responseOrdersOrderPaymentInstallmentPaymentFormDetailBoletoInformation boletoInfo = new SuperPag.Helper.Xml.Response.responseOrdersOrderPaymentInstallmentPaymentFormDetailBoletoInformation();

            attempt.lastUpdate = DateTime.Now;
            attempt.status = (int)PaymentAttemptStatus.PendingPaid;
            orderInstallment.status = (int)OrderInstallmentStatus.PendingPaid;
            installment.status = (byte)PaymentAttemptStatus.PendingPaid;
            attempt.TruncateStringFields();
            DataFactory.PaymentAttempt().Update(attempt);
            DataFactory.OrderInstallment().Update(orderInstallment);

            DPaymentAttemptBoleto dPaymentAttemptBoleto = new DPaymentAttemptBoleto();
            dPaymentAttemptBoleto.paymentAttemptId = attempt.paymentAttemptId;
            dPaymentAttemptBoleto.documentNumber = order.storeReferenceOrder;
            dPaymentAttemptBoleto.withdraw = dConsumer.name;
            dPaymentAttemptBoleto.withdrawDoc = (String.IsNullOrEmpty(dConsumer.CNPJ) ? dConsumer.CPF : dConsumer.CNPJ);
            dPaymentAttemptBoleto.address1 = (dConsumerAddressBilling == null ? "" : dConsumerAddressBilling.logradouro + " " + dConsumerAddressBilling.address + ", " + dConsumerAddressBilling.addressNumber + " " + dConsumerAddressBilling.addressComplement);
            dPaymentAttemptBoleto.address2 = (dConsumerAddressBilling == null ? "" : dConsumerAddressBilling.district);
            dPaymentAttemptBoleto.address3 = (dConsumerAddressBilling == null ? "" : dConsumerAddressBilling.cep + " - " + dConsumerAddressBilling.city + " - " + dConsumerAddressBilling.state);
            dPaymentAttemptBoleto.paymentDate = DateTime.Today;
            dPaymentAttemptBoleto.instructions = paymentInfo.instructions;
            dPaymentAttemptBoleto.expirationPaymentDate = paymentInfo.dueDate;
            DataFactory.PaymentAttemptBoleto().Insert(dPaymentAttemptBoleto);

            BoletosBancariosInfo boletosBancariosInfo = new BoletosBancariosInfo();
            boletosBancariosInfo.Carteira = dPaymentAgentSetupBoletoBB.wallet;
            boletosBancariosInfo.Convenio = dPaymentAgentSetupBoletoBB.conventionNumber;
            boletosBancariosInfo.CodBanco = dPaymentAgentSetupBoletoBB.bankNumber;
            boletosBancariosInfo.CodMoeda = "9";
            boletosBancariosInfo.DataVencimento = dPaymentAttemptBoleto.expirationPaymentDate;
            boletosBancariosInfo.NossoNumero = dPaymentAttemptBoleto.agentOrderReference.ToString();
            boletosBancariosInfo.ValorBoleto = attempt.price;
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

            string serverUrl = "";
            if (HttpContext.Current != null)
            {
                string http = (HttpContext.Current.Request.ServerVariables["HTTPS"] == "off" ? "http" : "https");
                string server = HttpContext.Current.Request.ServerVariables["SERVER_NAME"];
                serverUrl = String.Format("{0}://{1}", http, server);
            }
            else if (ConfigurationManager.AppSettings != null && ConfigurationManager.AppSettings["ServerUrl"] != null)
                serverUrl = ConfigurationManager.AppSettings["ServerUrl"];

            boletoInfo.number = nossoNumero;
            boletoInfo.typingLine = linhaDigitavel;
            boletoInfo.url = String.Format("{0}/Agents/Boleto/showboleto.aspx?id={1}", serverUrl, attempt.paymentAttemptId);
            boletoInfo.paidValueSpecified = false;

            pfDetail.Item = boletoInfo;
            installment.paymentFormDetail = pfDetail;
            installment.number = (ulong)orderInstallment.installmentNumber;
            installment.date = DateTime.Today;
            installment.dateSpecified = true;
            installment.paymentDateSpecified = false;

            return installment;
        }
        //TUDO: mudança para a ponto cred
        public static responseOrdersOrderPaymentInstallment ProcessPayment(DOrder order, DOrderInstallment orderInstallment, DPaymentAttempt attempt, SuperPag.Helper.Xml.Request.genericPaymentFormDetailBoletoInformationIPTE paymentInfo,Guid UserId)
        {
            Ensure.IsNotNull(order, "O pedido deve ser informado");
            Ensure.IsNotNull(attempt, "A tentativa de pagamento deve ser informada");
            Ensure.IsNotNull(orderInstallment, "A parcela relativa deve ser informada");
            Ensure.IsNotNull(paymentInfo, "Os dados do boleto devem ser informados");

            DPaymentAgentSetupBoleto dPaymentAgentSetupBoletoBB = DataFactory.PaymentAgentSetupBoleto().Locate(attempt.paymentAgentSetupId);
            Ensure.IsNotNull(dPaymentAgentSetupBoletoBB, "A loja não está configurada corretamente para esse meio de pagamento");

            DConsumer dConsumer = DataFactory.Consumer().Locate(order.consumerId);
            DConsumerAddress dConsumerAddressBilling = DataFactory.ConsumerAddress().Locate(dConsumer.consumerId, 1);

            //Inicializo as classes para retorno
            responseOrdersOrderPaymentInstallment installment = new SuperPag.Helper.Xml.Response.responseOrdersOrderPaymentInstallment();
            responseOrdersOrderPaymentInstallmentPaymentFormDetail pfDetail = new SuperPag.Helper.Xml.Response.responseOrdersOrderPaymentInstallmentPaymentFormDetail();
            responseOrdersOrderPaymentInstallmentPaymentFormDetailBoletoInformationIPTE boletoInfo = new SuperPag.Helper.Xml.Response.responseOrdersOrderPaymentInstallmentPaymentFormDetailBoletoInformationIPTE();

            attempt.lastUpdate = DateTime.Now;
            attempt.status = (int)PaymentAttemptStatus.PendingPaid;
            orderInstallment.status = (int)OrderInstallmentStatus.PendingPaid;
            installment.status = (byte)PaymentAttemptStatus.PendingPaid;
            attempt.TruncateStringFields();
            DataFactory.PaymentAttempt().Update(attempt);
            DataFactory.OrderInstallment().Update(orderInstallment);

            DPaymentAttemptBoleto dPaymentAttemptBoleto = new DPaymentAttemptBoleto();
            dPaymentAttemptBoleto.paymentAttemptId = attempt.paymentAttemptId;
            dPaymentAttemptBoleto.documentNumber = order.storeReferenceOrder;
            dPaymentAttemptBoleto.withdraw = dConsumer.name;
            dPaymentAttemptBoleto.withdrawDoc = (String.IsNullOrEmpty(dConsumer.CNPJ) ? dConsumer.CPF : dConsumer.CNPJ);
            dPaymentAttemptBoleto.address1 = (dConsumerAddressBilling == null ? "" : dConsumerAddressBilling.logradouro + " " + dConsumerAddressBilling.address + ", " + dConsumerAddressBilling.addressNumber + " " + dConsumerAddressBilling.addressComplement);
            dPaymentAttemptBoleto.address2 = (dConsumerAddressBilling == null ? "" : dConsumerAddressBilling.district);
            dPaymentAttemptBoleto.address3 = (dConsumerAddressBilling == null ? "" : dConsumerAddressBilling.cep + " - " + dConsumerAddressBilling.city + " - " + dConsumerAddressBilling.state);
            
            dPaymentAttemptBoleto.paymentDate = DateTime.Now;
            dPaymentAttemptBoleto.instructions = paymentInfo.instructions;

            dPaymentAttemptBoleto.expirationPaymentDate = paymentInfo.dueDate;
            dPaymentAttemptBoleto.UserId = UserId;
            dPaymentAttemptBoleto.Status = true;
            dPaymentAttemptBoleto.Contrato = paymentInfo.Contrato;
            //DataFactory.PaymentAttemptBoleto().Insert(dPaymentAttemptBoleto);
            //SuperPag.Business.PaymentAttemptBoleto.Insert(dPaymentAttemptBoleto);
            DataFactory.PaymentAttemptBoleto().Insert(dPaymentAttemptBoleto);

            dPaymentAttemptBoleto.oct = string.Empty;
            dPaymentAttemptBoleto.barCode = paymentInfo.IPTE;
            dPaymentAttemptBoleto.ourNumber = string.Empty;
            DataFactory.PaymentAttemptBoleto().Update(dPaymentAttemptBoleto);

            boletoInfo.PaymentAttemptId = attempt.paymentAttemptId.ToString();

            pfDetail.Item = boletoInfo;
            installment.paymentFormDetail = pfDetail;
            installment.number = (ulong)orderInstallment.installmentNumber;
            installment.date = DateTime.Today;
            installment.dateSpecified = true;
            installment.paymentDateSpecified = false;

            return installment;
        }
    }
}
