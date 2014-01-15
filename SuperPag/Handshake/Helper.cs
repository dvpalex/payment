using System;
using System.Xml;
using System.Web;
using System.Text;
using System.Threading;
using System.Collections.Generic;
using System.Collections.Specialized;
using SuperPag.Data;
using SuperPag.Data.Messages;
using SuperPag.Data.Interfaces;
using SuperPag.Helper;
using System.Net.Mail;
using System.Configuration;
using System.IO;
using System.Reflection;

namespace SuperPag.Handshake
{
    public class Helper
    {
        public static bool StoreReferenceOrderIsPaid(int storeId, string storeReferenceOrder)
        {
            bool ret = false;

            DOrder[] arrOrder = DataFactory.Order().List(storeId, storeReferenceOrder);
            if (Ensure.IsNotNull(arrOrder))
                foreach (DOrder order in arrOrder)
                {
                    DPaymentAttempt[] arrAttempt = DataFactory.PaymentAttempt().List(order.orderId);
                    if (Ensure.IsNotNull(arrAttempt))
                        foreach (DPaymentAttempt attempt in arrAttempt)
                            if (attempt.status == (int)PaymentAttemptStatus.Paid)
                            {
                                ret = true;
                                break;
                            }
                }

            return ret;
        }

        public static bool OrderIsPaid(long orderId)
        {
            bool ret = false;

            DPaymentAttempt[] arrAttempt = DataFactory.PaymentAttempt().List(orderId);
            if (Ensure.IsNotNull(arrAttempt))
                foreach (DPaymentAttempt attempt in arrAttempt)
                    if (attempt.status == (int)PaymentAttemptStatus.Paid)
                    {
                        ret = true;
                        break;
                    }

            return ret;
        }

        public static void SendPaymentPost(Guid paymentAttemptId)
        {
            DPaymentAttempt dPaymentAttempt = DataFactory.PaymentAttempt().Locate(paymentAttemptId);
            Ensure.IsNotNull(dPaymentAttempt, "Tentativa de pagamento {0} não encontrada", paymentAttemptId);
            DOrder dOrder = DataFactory.Order().Locate(dPaymentAttempt.orderId);
            DStore dStore = DataFactory.Store().Locate(dOrder.storeId);

            //obtem os dados do handshake
            DHandshakeConfiguration dHandshakeConfiguration = DataFactory.HandshakeConfiguration().Locate(dStore.handshakeConfigurationId);
            Ensure.IsNotNull(dHandshakeConfiguration, "A loja {0} não está configurada", dStore.storeId);

            //verifica se o tipo de handshake é html
            if (dHandshakeConfiguration.handshakeType == (int)HandshakeType.HtmlSPag10)
            {
                SuperPag.Handshake.Html.Handshake hand = new SuperPag.Handshake.Html.Handshake();
                hand.SendPaymentPost(dPaymentAttempt.paymentAttemptId);
            }
            else if (dHandshakeConfiguration.handshakeType == (int)HandshakeType.XmlSPag10)
            {
                SuperPag.Handshake.Xml.Handshake hand = new SuperPag.Handshake.Xml.Handshake();
                hand.SendPaymentPost(dPaymentAttempt.paymentAttemptId);
            }
        }

        public static void SendFinalizationPost(Guid paymentAttemptId)
        {
            DPaymentAttempt dPaymentAttempt = DataFactory.PaymentAttempt().Locate(paymentAttemptId);
            Ensure.IsNotNull(dPaymentAttempt, "Tentativa de pagamento {0} não encontrada", paymentAttemptId);
            DOrder dOrder = DataFactory.Order().Locate(dPaymentAttempt.orderId);
            DStore dStore = DataFactory.Store().Locate(dOrder.storeId);

            //obtem os dados do handshake
            DHandshakeConfiguration dHandshakeConfiguration = DataFactory.HandshakeConfiguration().Locate(dStore.handshakeConfigurationId);
            Ensure.IsNotNull(dHandshakeConfiguration, "A loja {0} não está configurada", dStore.storeId);

            //verifica se o tipo de handshake é html
            if (dHandshakeConfiguration.handshakeType == (int)HandshakeType.HtmlSPag10)
            {
                SuperPag.Handshake.Html.Handshake hand = new SuperPag.Handshake.Html.Handshake();
                hand.SendFinalizationPost(dPaymentAttempt.paymentAttemptId);
            }
            else if (dHandshakeConfiguration.handshakeType == (int)HandshakeType.XmlSPag10)
            {
                SuperPag.Handshake.Xml.Handshake hand = new SuperPag.Handshake.Xml.Handshake();
                hand.SendFinalizationPost(dPaymentAttempt.paymentAttemptId);
            }
        }

        public static bool SendFinalizationConsumerEmail(Guid paymentAttemptId, string idioma, string textoFinalizacao, string linkRetorno)
        {
            try
            {
                DPaymentAttempt attempt = DataFactory.PaymentAttempt().Locate(paymentAttemptId);
                Ensure.IsNotNull(attempt, "Tentativa de pagamento {0} não encontrada", paymentAttemptId);
                DOrder order = DataFactory.Order().Locate(attempt.orderId);

                DServiceEmailPaymentForm serviceEmail;
                if (!String.IsNullOrEmpty(idioma))
                    serviceEmail = DataFactory.ServiceEmailPaymentForm().Locate(order.storeId, (int)EmailTypes.ConsumerFinalization, attempt.paymentFormId, idioma);
                else
                    serviceEmail = DataFactory.ServiceEmailPaymentForm().Locate(order.storeId, (int)EmailTypes.ConsumerFinalization, attempt.paymentFormId, "pt-br");

                Ensure.IsNotNull(serviceEmail, "Configuração para envio do e-mail para a loja {0} e meio de pagamento {1} idioma {2} não encontrada", order.storeId, attempt.paymentFormId, idioma);
                DStore store = DataFactory.Store().Locate(order.storeId);
                DPaymentForm paymentForm = DataFactory.PaymentForm().Locate(attempt.paymentFormId);

                if (order.consumerId == long.MinValue)
                    return false;

                DConsumer consumer = DataFactory.Consumer().Locate(order.consumerId);
                if (consumer == null)
                    return false;

                //verificamos se há um email de consumidor válido
                if (String.IsNullOrEmpty(consumer.email))
                    return false;

                //seto o from
                //TODO: verificar se é possivel a mudança do FROM ou do SENDER
                string fromField = serviceEmail.fromField;
                if (String.IsNullOrEmpty(fromField))
                    fromField = ConfigurationManager.AppSettings["defaultSenderMail"];
                if (String.IsNullOrEmpty(fromField))
                    fromField = "superpag@superpag.com.br";

                MailMessage email = MountFinalizationEmail(attempt, order, paymentForm, consumer, serviceEmail, textoFinalizacao, linkRetorno);
                email.To.Add(consumer.email.Trim());
                email.From = new MailAddress(fromField, store.name);
                email.Sender = new MailAddress(fromField, store.name);
                if (!String.IsNullOrEmpty(serviceEmail.ccField))
                    email.CC.Add(serviceEmail.ccField);
                email.IsBodyHtml = serviceEmail.sendHtml;

                if (!String.IsNullOrEmpty(serviceEmail.encoding))
                {
                    email.BodyEncoding = Encoding.GetEncoding(serviceEmail.encoding);
                    email.SubjectEncoding = Encoding.GetEncoding(serviceEmail.encoding);
                }

                SmtpClient smtp = new SmtpClient();
                smtp.Send(email);

                GenericHelper.LogFile("EasyPagObject::Helper.cs::SendFinalizationConsumerEmail email de finalização enviado ao consumidor storeId=" + order.storeId + " orderId=" + order.orderId + " from=" + fromField + " email=" + consumer.email, LogFileEntryType.Information);

                return true;
            }
            catch (ApplicationException e)
            {
                GenericHelper.LogFile("EasyPagObject::Helper.cs::SendFinalizationConsumerEmail " + e.Message, LogFileEntryType.Warning);
                return false;
            }
            catch (Exception e)
            {
                GenericHelper.LogFile("EasyPagObject::Helper.cs::SendFinalizationConsumerEmail paymentAttemptId=" + paymentAttemptId + " msg error=" + e.Message + " " + e.InnerException, LogFileEntryType.Error);
                return false;
            }
        }

        public static MailMessage MountFinalizationEmail(DPaymentAttempt attempt, DOrder order, DPaymentForm paymentForm, DConsumer consumer, DServiceEmailPaymentForm serviceEmail, string textoFinalizacao, string linkRetorno)
        {
            string serverUrl = "";
            if (HttpContext.Current != null)
            {
                string http = (HttpContext.Current.Request.ServerVariables["HTTPS"] == "off" ? "http" : "https");
                string server = HttpContext.Current.Request.ServerVariables["SERVER_NAME"];
                serverUrl = String.Format("{0}://{1}", http, server);
            }
            else if (ConfigurationManager.AppSettings != null && ConfigurationManager.AppSettings["ServerUrl"] != null)
                serverUrl = ConfigurationManager.AppSettings["ServerUrl"];

            NameValueCollection paramsToReplace = new NameValueCollection();
            NameValueCollection paramsToReplaceItens = new NameValueCollection();
            StringBuilder itens = new StringBuilder();
            NameValueCollection paramsToReplaceInstallment = new NameValueCollection();
            StringBuilder installments = new StringBuilder();

            switch (paymentForm.paymentFormId)
            {
                case (int)PaymentForms.BoletoBradesco:
                case (int)PaymentForms.BoletoBancoDoBrasil:
                case (int)PaymentForms.BoletoItau:
                case (int)PaymentForms.BoletoHSBC:
                    //TODO: detalhe de cada parcela
                    DPaymentAttemptBoleto attemptBoleto = DataFactory.PaymentAttemptBoleto().Locate(attempt.paymentAttemptId);
                    paramsToReplaceInstallment.Add("parcela", attempt.installmentNumber.ToString());
                    paramsToReplaceInstallment.Add("linhaDigitavel", attemptBoleto.oct);
                    paramsToReplaceInstallment.Add("valorBoleto", GenericHelper.FormatCurrencyBrasil(attempt.price));
                    paramsToReplaceInstallment.Add("linkBoleto", String.Format("{0}/Agents/Boleto/showboleto.aspx?id={1}", serverUrl, attempt.paymentAttemptId.ToString()));
                    paramsToReplaceInstallment.Add("vencimentoBoleto", attemptBoleto.expirationPaymentDate.ToString("dd/MM/yyyy"));
                    installments.AppendLine(GenericHelper.ReplaceStringWithParams(serviceEmail.installmentTemplate, paramsToReplaceInstallment, "[@{0}]"));
                    paramsToReplace.Add("installments", installments.ToString());
                    break;

                case (int)PaymentForms.VisaMoset:
                case (int)PaymentForms.VisaMoset3:
                    DPaymentAttemptMoset attemptMoset = DataFactory.PaymentAttemptMoset().Locate(attempt.paymentAttemptId);
                    paramsToReplace.Add("tid", attemptMoset.tid);
                    break;

                case (int)PaymentForms.VisaVBV:
                case (int)PaymentForms.VisaVBVInBox:
                case (int)PaymentForms.VisaVBV3:
                    DPaymentAttemptVBV attemptVBV = DataFactory.PaymentAttemptVBV().Locate(attempt.paymentAttemptId);
                    paramsToReplace.Add("arp", attemptVBV.arp.ToString());
                    paramsToReplace.Add("tid", attemptVBV.tid.ToString());
                    break;

                case (int)PaymentForms.DinersKomerci:
                case (int)PaymentForms.DinersKomerciInBox:
                case (int)PaymentForms.MasterKomerci:
                case (int)PaymentForms.MasterKomerciInBox:
                    DPaymentAttemptKomerci attemptKomerci = DataFactory.PaymentAttemptKomerci().Locate(attempt.paymentAttemptId);
                    paramsToReplace.Add("numcv", attemptKomerci.numcv);
                    paramsToReplace.Add("numautent", attemptKomerci.numautent);
                    paramsToReplace.Add("numautor", attemptKomerci.numautor);
                    break;

                case (int)PaymentForms.DinersWebService:
                case (int)PaymentForms.MasterWebService:
                    DPaymentAttemptKomerciWS attemptKomerciWS = DataFactory.PaymentAttemptKomerciWS().Locate(attempt.paymentAttemptId);
                    paramsToReplace.Add("numcv", attemptKomerciWS.numcv);
                    paramsToReplace.Add("numautent", attemptKomerciWS.numautent);
                    paramsToReplace.Add("numautor", attemptKomerciWS.numautor);
                    break;

                case (int)PaymentForms.Amex3Party:
                case (int)PaymentForms.Amex2Party:
                    DPaymentAttemptPaymentClientVirtual attemptAmex = DataFactory.PaymentAttemptPaymentClientVirtual().Locate(attempt.paymentAttemptId);
                    paramsToReplace.Add("authorizeId", attemptAmex.vpc_AuthorizeId.ToString());
                    break;
            }

            string status = "";
            string statusDesc = "";
            switch (attempt.status)
            {
                case (int)PaymentAttemptStatus.Pending:
                    status = "Não Concluído";
                    statusDesc = "Não foi possível prosseguir com o pagamento, por favor, tente novamente.";
                    break;
                case (int)PaymentAttemptStatus.NotPaid:
                    status = "Não Pago";
                    statusDesc = "A transação realizada não foi aprovada pela instituição financeira, por favor, tente novamente.";
                    break;
                case (int)PaymentAttemptStatus.Paid:
                    status = "Pago";
                    statusDesc = "O pagamento foi realizado com sucesso.";
                    break;
                case (int)PaymentAttemptStatus.PendingPaid:
                    status = "Pendente";
                    statusDesc = "Estamos aguardando a confirmação do pagamento pela instituição financeira.";
                    break;
                case (int)PaymentAttemptStatus.Canceled:
                    status = "Cancelado";
                    statusDesc = "A transação foi cancelada.";
                    break;
            }

            paramsToReplace.Add("nome", consumer.name);
            paramsToReplace.Add("cnpj/cpf", String.IsNullOrEmpty(consumer.CNPJ) ? consumer.CPF : consumer.CNPJ);
            paramsToReplace.Add("numeroPedido", order.storeReferenceOrder);
            paramsToReplace.Add("data", attempt.startTime.ToShortDateString());
            paramsToReplace.Add("horario", attempt.startTime.ToShortTimeString());
            paramsToReplace.Add("formaPagamento", paymentForm.name);
            paramsToReplace.Add("qtdParcelas", order.installmentQuantity.ToString());
            paramsToReplace.Add("statusDesc", statusDesc);
            paramsToReplace.Add("statusCobranca", status);
            paramsToReplace.Add("attemptId", attempt.paymentAttemptId.ToString());
            paramsToReplace.Add("valorTotal", GenericHelper.FormatCurrencyBrasil(order.finalAmount));
            paramsToReplace.Add("valorParcela", GenericHelper.FormatCurrencyBrasil(attempt.price));
            paramsToReplace.Add("textoFinalizacao", textoFinalizacao);
            paramsToReplace.Add("linkRetorno", linkRetorno);

            //lista de Itens
            DOrderItem[] orderItens = DataFactory.OrderItem().List(order.orderId, (int)ItemTypes.ShippingRate);
            if (orderItens != null && orderItens.Length > 0 && orderItens[0] != null)
                paramsToReplace.Add("valorFrete", GenericHelper.FormatCurrencyBrasil(orderItens[0].itemValue));

            orderItens = DataFactory.OrderItem().List(order.orderId, (int)ItemTypes.Regular);
            if (orderItens != null)
            {
                foreach (DOrderItem item in orderItens)
                {
                    paramsToReplaceItens.Add("itemCode", item.itemCode);
                    paramsToReplaceItens.Add("itemDescription", item.itemDescription);
                    paramsToReplaceItens.Add("itemQuantity", item.itemQuantity.ToString());
                    paramsToReplaceItens.Add("itemValue", GenericHelper.FormatCurrencyBrasil(item.itemValue));
                    paramsToReplaceItens.Add("itemTotalValue", GenericHelper.FormatCurrencyBrasil(item.itemValue * item.itemQuantity));

                    itens.AppendLine(GenericHelper.ReplaceStringWithParams(serviceEmail.itensTemplate, paramsToReplaceItens, "[@{0}]"));
                }
            }
            paramsToReplace.Add("itens", itens.ToString());

            MailMessage email = new MailMessage();
            email.Subject = GenericHelper.ReplaceStringWithParams(serviceEmail.subjectTemplate, paramsToReplace, "[@{0}]");
            email.Body = GenericHelper.ReplaceStringWithParams(serviceEmail.bodyTemplate, paramsToReplace, "[@{0}]");

            return email;
        }

        public static bool SendFinalizationStoreKeeperEmail(Guid paymentAttemptId, string textoFinalizacao, string linkRetorno)
        {
            try
            {
                DPaymentAttempt attempt = DataFactory.PaymentAttempt().Locate(paymentAttemptId);
                Ensure.IsNotNull(attempt, "Tentativa de pagamento {0} não encontrada", paymentAttemptId);
                DOrder order = DataFactory.Order().Locate(attempt.orderId);
                DServiceEmailPaymentForm serviceEmail = DataFactory.ServiceEmailPaymentForm().Locate(order.storeId, (int)EmailTypes.StoreFinalization, attempt.paymentFormId, "pt-br");
                Ensure.IsNotNull(serviceEmail, "Configuração para envio do e-mail para a loja {0} e meio de pagamento {1} não encontrada", order.storeId, attempt.paymentFormId);
                DStore store = DataFactory.Store().Locate(order.storeId);
                DPaymentForm paymentForm = DataFactory.PaymentForm().Locate(attempt.paymentFormId);
                DConsumer consumer = DataFactory.Consumer().Locate(order.consumerId);

                //verificamos se há um email de lojista válido
                if (String.IsNullOrEmpty(serviceEmail.toField))
                    return false;

                //seto o from
                //TODO: verificar se é possivel a mudança do FROM ou do SENDER
                string fromField = serviceEmail.fromField;
                if (String.IsNullOrEmpty(fromField))
                    fromField = ConfigurationManager.AppSettings["defaultSenderMail"];
                if (String.IsNullOrEmpty(fromField))
                    fromField = "superpag@superpag.com.br";

                MailMessage email = MountFinalizationEmail(attempt, order, paymentForm, consumer, serviceEmail, textoFinalizacao, linkRetorno);
                email.To.Add(serviceEmail.toField.Trim());
                email.From = new MailAddress(fromField);
                email.Sender = new MailAddress(serviceEmail.fromField);
                email.IsBodyHtml = serviceEmail.sendHtml;

                if (!String.IsNullOrEmpty(serviceEmail.encoding))
                {
                    email.BodyEncoding = Encoding.GetEncoding(serviceEmail.encoding);
                    email.SubjectEncoding = Encoding.GetEncoding(serviceEmail.encoding);
                }

                SmtpClient smtp = new SmtpClient();
                smtp.Send(email);

                GenericHelper.LogFile("EasyPagObject::Helper.cs::SendFinalizationStoreKeeperEmail email de finalização enviado ao lojista storeId=" + order.storeId + " orderId=" + order.orderId + " from=" + fromField + " email=" + serviceEmail.toField, LogFileEntryType.Information);

                return true;
            }
            catch (Exception e)
            {
                GenericHelper.LogFile("EasyPagObject::Helper.cs::SendFinalizationStoreKeeperEmail email de finalização enviado ao lojista paymentAttemptId=" + paymentAttemptId + " " + e.Message + " " + e.InnerException, LogFileEntryType.Error);
                return false;
            }
        }

        public static bool SendFinalizationContigencyEmail(DStore store, string storeReferenceOrder, Guid paymentAttemptId, string emailList)
        {
            try
            {
                MailMessage email = new MailMessage("suporte@novaconexao.com.br", emailList);
                email.Subject = "SuperPag: e-mail de contingência para " + store.name;

                StringBuilder body = new StringBuilder();
                body.AppendLine("O limite de tentativas de envio do post de finalização foi ultrapassado e não recebemos nenhuma confirmação dos posts enviados.");
                body.AppendLine();
                body.AppendLine("Código do pedido: " + storeReferenceOrder);
                body.AppendLine("Código da transação no SuperPag: " + paymentAttemptId.ToString());
                body.AppendLine();
                body.AppendLine("Este e-mail confirma a finalização deste pedido.");

                email.Body = body.ToString();

                SmtpClient smtp = new SmtpClient();
                smtp.Send(email);

                GenericHelper.LogFile("EasyPagObject::Helper.cs::SendFinalizationContigencyEmail email de contingência de finalização enviado paymentAttempId=" + paymentAttemptId + " emailList=" + emailList, LogFileEntryType.Information);

                return true;
            }
            catch (Exception e)
            {
                GenericHelper.LogFile("EasyPagObject::Helper.cs::SendFinalizationContigencyEmail paymentAttempId=" + paymentAttemptId + " " + e.Message + " " + e.InnerException, LogFileEntryType.Error);
                return false;
            }
        }

        public static bool SendPaymentContigencyEmail(DStore store, string storeReferenceOrder, Guid paymentAttemptId, int installmentNumber, string emailList)
        {
            try
            {
                MailMessage email = new MailMessage("suporte@novaconexao.com.br", emailList);
                email.Subject = "SuperPag: e-mail de contingência para " + store.name;

                StringBuilder body = new StringBuilder();
                body.AppendLine("O limite de tentativas de envio do post de pagamento foi ultrapassado e não recebemos nenhuma confirmação dos posts enviados.");
                body.AppendLine();
                body.AppendLine("Código do pedido: " + storeReferenceOrder + " número da parcela: " + installmentNumber.ToString());
                body.AppendLine("Código da transação no SuperPag: " + paymentAttemptId.ToString());
                body.AppendLine();
                body.AppendLine("Este e-mail confirma o pagamento deste pedido.");

                email.Body = body.ToString();

                SmtpClient smtp = new SmtpClient();
                smtp.Send(email);

                GenericHelper.LogFile("EasyPagObject::Helper.cs::SendPaymentContigencyEmail email de contingência de pagamento enviado paymentAttempId=" + paymentAttemptId + " installmentNumber=" + installmentNumber + " emailList=" + emailList, LogFileEntryType.Information);

                return true;
            }
            catch (Exception e)
            {
                GenericHelper.LogFile("EasyPagObject::Helper.cs::SendPaymentContigencyEmail paymentAttempId=" + paymentAttemptId + " " + e.Message + " " + e.InnerException, LogFileEntryType.Error);
                return false;
            }

            #region Exemplo de email de contingência
            //strBody = "O Limite de envio de POST PAGAMENTO foi ultrapassado. - Parceiro: " & rsPedidos("INT_CODIGO_PARCEIRO") & vbCrLf & vbCrLf
            //strBody = strBody & "Código pedido parceiro: " & IIf(IsNull(rsPedidos("STR_NUMERO_COMPRA_PARCEIRO")), "", rsPedidos("STR_NUMERO_COMPRA_PARCEIRO")) & vbCrLf
            //strBody = strBody & "Código smartpag: " & rsPedidos("INT_NUMERO_PEDIDO") & vbCrLf
            //strBody = strBody & vbCrLf & vbCrLf
            //strBody = strBody & "As tentativas de conectar seu servidor para confirmação do pagamento do pedido feito no smartpag se esgotaram." & vbCrLf & vbCrLf
            //strBody = strBody & "Este e-mail confirma o pagamento deste pedido." & vbCrLf
            //strBody = strBody & vbCrLf & vbCrLf
            //strBody = strBody & "Dados para post não enviado: " & strArquivoTexto & vbCrLf
            //strSubject = "[SmartPag - Post Pagamento] Email de contigência para " & rsPedidos("STR_NOME_PARCEIRO")
            #endregion
        }

        public static int GetSmartPagPaymentForm(int paymentFormId)
        {
            switch ((PaymentForms)paymentFormId)
            {
                case PaymentForms.BBPag:
                    return (int)SmartPagLegacy.CodigoMeioPagamento.BBPag;
                case PaymentForms.BoletoBancoDoBrasil:
                    return (int)SmartPagLegacy.CodigoMeioPagamento.BoletoBancoDoBrasil;
                case PaymentForms.BoletoBradesco:
                    return (int)SmartPagLegacy.CodigoMeioPagamento.BoletoBradesco;
                case PaymentForms.ItauShopLine:
                    return (int)SmartPagLegacy.CodigoMeioPagamento.ItauShopLine;
                case PaymentForms.FinanciamentoABN:
                    return (int)SmartPagLegacy.CodigoMeioPagamento.FinanciamentoABNAMROBank;
                case PaymentForms.Amex2Party:
                case PaymentForms.Amex3Party:
                case PaymentForms.DinersKomerci:
                case PaymentForms.MasterKomerci:
                case PaymentForms.DinersKomerciInBox:
                case PaymentForms.MasterKomerciInBox:
                case PaymentForms.VisaSitef:
                case PaymentForms.VisaVBV:
                case PaymentForms.VisaVBVInBox:
                case PaymentForms.VisaVBV3:
                case PaymentForms.VisaMoset:
                case PaymentForms.VisaMoset3:
                case PaymentForms.MasterSitef:
                case PaymentForms.DinersSitef:
                case PaymentForms.AmexSitef:
                case PaymentForms.HipercardSitef:
                case PaymentForms.MasterWebService:
                case PaymentForms.DinersWebService:
                    return (int)SmartPagLegacy.CodigoMeioPagamento.CartaoDeCredito;
                case PaymentForms.BBPagCrediario:
                    return (int)SmartPagLegacy.CodigoMeioPagamento.BBPagCrediario;
                case PaymentForms.PagamentoFacilBradesco:
                    return (int)SmartPagLegacy.CodigoMeioPagamento.PagamentoFacilBradesco;
                default:
                    return paymentFormId;
            }
        }

        public static string GetSmartPagPaymentFormName(int paymentFormId)
        {
            switch ((PaymentForms)paymentFormId)
            {
                case PaymentForms.BoletoBancoDoBrasil:
                    return "BOLETO BANCARIO";
                case PaymentForms.DinersKomerci:
                case PaymentForms.DinersKomerciInBox:
                case PaymentForms.DinersWebService:
                    return "DINNERS";
                case PaymentForms.MasterKomerci:
                case PaymentForms.MasterKomerciInBox:
                case PaymentForms.MasterWebService:
                    return "REDECARD";
                case PaymentForms.VisaSitef:
                    return "VISA";
                case PaymentForms.VisaVBV:
                    return "VISA";
                case PaymentForms.VisaVBVInBox:
                    return "VISA";
                case PaymentForms.VisaVBV3:
                    return "VISA";
                case PaymentForms.VisaMoset:
                    return "VISA";
                case PaymentForms.VisaMoset3:
                    return "VISA";
                case PaymentForms.BBPag:
                    return "BB PAG";
                case PaymentForms.ItauShopLine:
                    return "ITAU SHOPLINE";
                default:
                    DPaymentForm _dPaymentForm = DataFactory.PaymentForm().Locate(paymentFormId);
                    return System.Web.HttpUtility.HtmlEncode(_dPaymentForm.name);
            }
        }

        public static string GetSmartPagPaymentFormNameByGroup(int paymentFormId)
        {
            switch ((PaymentForms)paymentFormId)
            {
                case PaymentForms.BoletoBancoDoBrasil:
                    return "BOLETO BANCARIO";
                case PaymentForms.DinersKomerci:
                case PaymentForms.DinersKomerciInBox:
                case PaymentForms.DinersWebService:
                case PaymentForms.MasterKomerci:
                case PaymentForms.MasterKomerciInBox:
                case PaymentForms.MasterWebService:
                case PaymentForms.VisaSitef:
                case PaymentForms.VisaVBV:
                case PaymentForms.VisaVBVInBox:
                case PaymentForms.VisaVBV3:
                case PaymentForms.VisaMoset:
                case PaymentForms.VisaMoset3:
                    return "Cartão de Crédito";
                case PaymentForms.BBPag:
                    return "BB PAG";
                case PaymentForms.ItauShopLine:
                    return "ITAU SHOPLINE";
                default:
                    DPaymentForm _dPaymentForm = DataFactory.PaymentForm().Locate(paymentFormId);
                    return System.Web.HttpUtility.HtmlEncode(_dPaymentForm.name);
            }
        }

        public static int GetSmartPagCreditCardFlag(int paymentFormId)
        {
            switch ((PaymentForms)paymentFormId)
            {
                case PaymentForms.Amex2Party:
                case PaymentForms.Amex3Party:
                    return (int)SmartPagLegacy.CodigoOpCartao.AmericanExpressOnLine;
                case PaymentForms.AmexSitef:
                    return (int)SmartPagLegacy.CodigoOpCartao.AmericanExpressSitef;
                case PaymentForms.DinersKomerci:
                case PaymentForms.DinersKomerciInBox:
                case PaymentForms.DinersWebService:
                    return (int)SmartPagLegacy.CodigoOpCartao.DinersClubOnLine;
                case PaymentForms.DinersSitef:
                    return (int)SmartPagLegacy.CodigoOpCartao.DinersClubSitef;
                case PaymentForms.MasterKomerci:
                case PaymentForms.MasterKomerciInBox:
                case PaymentForms.MasterWebService:
                    return (int)SmartPagLegacy.CodigoOpCartao.MasterCardOnLine;
                case PaymentForms.MasterSitef:
                    return (int)SmartPagLegacy.CodigoOpCartao.MasterCardSitef;
                case PaymentForms.VisaSitef:
                    return (int)SmartPagLegacy.CodigoOpCartao.VisaSitef;
                case PaymentForms.VisaVBV:
                case PaymentForms.VisaVBVInBox:
                case PaymentForms.VisaVBV3:
                case PaymentForms.VisaMoset:
                case PaymentForms.VisaMoset3:
                    return (int)SmartPagLegacy.CodigoOpCartao.VisaOnLine;
                default:
                    return paymentFormId;
            }
        }

        public static int GetSmartPagOrderStatus(int status)
        {
            switch ((OrderStatus)status)
            {
                case OrderStatus.Analysing:
                    return (int)SmartPagLegacy.StatusPedido.EmAnalise;
                case OrderStatus.Approved:
                    return (int)SmartPagLegacy.StatusPedido.AceitoOuEmProcessamento;
                case OrderStatus.Cancelled:
                    return (int)SmartPagLegacy.StatusPedido.Cancelado;
                case OrderStatus.Delivered:
                    return (int)SmartPagLegacy.StatusPedido.Entregue;
                case OrderStatus.NotPaid:
                    return (int)SmartPagLegacy.StatusPedido.EmAnalise;
                case OrderStatus.PendingPaid:
                    return (int)SmartPagLegacy.StatusPedido.EmAnalise;
                case OrderStatus.Transportation:
                    return (int)SmartPagLegacy.StatusPedido.EmTransito;
                case OrderStatus.Undelivered:
                    return (int)SmartPagLegacy.StatusPedido.NaoEntregue;
                case OrderStatus.Unfinished:
                    return (int)SmartPagLegacy.StatusPedido.NaoConcluido;
                default:
                    return status;
            }
        }

        public static int GetSmartPagBillingStatus(int status)
        {
            switch ((PaymentAttemptStatus)status)
            {
                case PaymentAttemptStatus.Canceled:
                    return (int)SmartPagLegacy.StatusCobranca.Restituido;
                case PaymentAttemptStatus.NotPaid:
                    return (int)SmartPagLegacy.StatusCobranca.NaoPago;
                case PaymentAttemptStatus.Paid:
                    return (int)SmartPagLegacy.StatusCobranca.Pago;
                case PaymentAttemptStatus.Pending:
                    return (int)SmartPagLegacy.StatusCobranca.NaoConcluido;
                case PaymentAttemptStatus.PendingPaid:
                    return (int)SmartPagLegacy.StatusCobranca.NaoPago;
                default:
                    return status;
            }
        }

        public static int SmartPagToSuperpagPaymentForm(int paymentFormId, int cardFlag)
        {
            switch ((SmartPagLegacy.CodigoMeioPagamento)paymentFormId)
            {
                case SmartPagLegacy.CodigoMeioPagamento.BBPag:
                    return (int)PaymentForms.BBPag;
                case SmartPagLegacy.CodigoMeioPagamento.BBPagCrediario:
                    return (int)PaymentForms.BBPagCrediario;
                case SmartPagLegacy.CodigoMeioPagamento.BoletoBancoDoBrasil:
                    return (int)PaymentForms.BoletoBancoDoBrasil;
                case SmartPagLegacy.CodigoMeioPagamento.BoletoBradesco:
                    return (int)PaymentForms.BoletoBradesco;
                case SmartPagLegacy.CodigoMeioPagamento.ItauShopLine:
                    return (int)PaymentForms.ItauShopLine;
                case SmartPagLegacy.CodigoMeioPagamento.FinanciamentoABNAMROBank:
                    return (int)PaymentForms.FinanciamentoABN;
                case SmartPagLegacy.CodigoMeioPagamento.PagamentoFacilBradesco:
                    return (int)PaymentForms.PagamentoFacilBradesco;
                case SmartPagLegacy.CodigoMeioPagamento.CartaoDeCredito:
                    switch ((SmartPagLegacy.CodigoOpCartao)cardFlag)
                    {
                        case SmartPagLegacy.CodigoOpCartao.AmericanExpressOnLine:
                            //TODO: o certo é verificar na configuração
                            //se a loja em questão possui Amex2Party ou Amex3Party
                            return (int)PaymentForms.Amex2Party;
                        case SmartPagLegacy.CodigoOpCartao.DinersClubOnLine:
                            //TODO: o certo é verificar na configuração
                            //se a loja em questão possui DinersKomerci ou DinersKomerciInBox
                            return (int)PaymentForms.DinersKomerci;
                        case SmartPagLegacy.CodigoOpCartao.MasterCardOnLine:
                            //TODO: o certo é verificar na configuração
                            //se a loja em questão possui MasterKomerci ou MasterKomerciInBox
                            return (int)PaymentForms.MasterKomerci;
                        case SmartPagLegacy.CodigoOpCartao.VisaSitef:
                            return (int)PaymentForms.VisaSitef;
                        case SmartPagLegacy.CodigoOpCartao.VisaOnLine:
                            //TODO: o certo é verificar na configuração
                            //se a loja em questão possui VisaVBV, VisaVBVInBox, VisaVBV3, VisaMoset ou VisaMoset3
                            return (int)PaymentForms.VisaVBV;
                        case SmartPagLegacy.CodigoOpCartao.MasterCardSitef:
                            return (int)PaymentForms.MasterSitef;
                        case SmartPagLegacy.CodigoOpCartao.DinersClubSitef:
                            return (int)PaymentForms.DinersSitef;
                        case SmartPagLegacy.CodigoOpCartao.AmericanExpressSitef:
                            return (int)PaymentForms.AmexSitef;
                        default:
                            return paymentFormId;
                    }
                default:
                    return paymentFormId;
            }
        }

        public static bool SendBoletoEmail(string Email, string FilePath, string body, string Subject, string[] CC)
        {
            try
            {
                MailAddress de = new MailAddress("superpag@superpag.com.br", "SuperPag");
                MailAddress para = new MailAddress(Email);

                MailMessage msg = new MailMessage(de, para);

                if (CC != null)
                {
                    foreach (string strCC in CC)
                    {
                        msg.CC.Add(strCC);
                    }
                }
                if (!FilePath.Equals(string.Empty))
                {
                    msg.Attachments.Add(new Attachment(FilePath));
                }
                

                msg.Subject = Subject;

                msg.IsBodyHtml = true;
                msg.Body = body;

                SmtpClient smtp = new SmtpClient();

                smtp.Send(msg);
                GenericHelper.LogFile("EasyPagObject::Helper.cs::SendBoletoEmail email enviado com sucesso!!", LogFileEntryType.Information);
                return true;

            }
            catch (Exception ex)
            {
                GenericHelper.LogFile("EasyPagObject::Helper.cs::SendBoletoEmail email não enviado!!: " + ex.ToString(), LogFileEntryType.Error);
                return false;
            }
        }

        public static Stream GetImage(string name)
        {
            Assembly asm = Assembly.GetExecutingAssembly();
            Stream stream = asm.GetManifestResourceStream(name);
            return stream;
        }
    }
}
