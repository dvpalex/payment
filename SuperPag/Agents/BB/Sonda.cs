using System;
using System.Collections.Generic;
using System.Text;
using SuperPag.Data.Messages;
using SuperPag.Data;
using SuperPag.Helper;
using System.Xml;

namespace SuperPag.Agents.BB
{
    public class Sonda
    {
        public static void UpdateStatus(DPaymentAttemptBB[] attemptsBB)
        {
            foreach (DPaymentAttemptBB attemptBB in attemptsBB)
                UpdateStatus(attemptBB);
        }

        public static void UpdateStatus(DPaymentAttemptBB attemptBB)
        {
            string orderId = "", paymentAttemptId = "";
            bool sendPost = false;

            try
            {
                paymentAttemptId = attemptBB.paymentAttemptId.ToString();
                DPaymentAttempt attempt = DataFactory.PaymentAttempt().Locate(attemptBB.paymentAttemptId);
                DOrder order = DataFactory.Order().Locate(attempt.orderId);
                orderId = order.orderId.ToString();
                DPaymentAgentSetupBB agentsetup = DataFactory.PaymentAgentSetupBB().Locate(attempt.paymentAgentSetupId);
                Ensure.IsNotNull(agentsetup, "Configuração do agent {0} não encontrada", attempt.paymentAgentSetupId);

                if (String.IsNullOrEmpty(agentsetup.urlBBPagSonda))
                    return;

                int oldAttemptStatus = attempt.status;

                ServerHttpHtmlRequisition post = new ServerHttpHtmlRequisition();
                post.Method = "POST";
                post.UpperKeys = false;
                post.Url = agentsetup.urlBBPagSonda;
                post.Parameters.Add("idConv", agentsetup.businessNumber.ToString());
                post.Parameters.Add("refTran", attemptBB.agentOrderReference.ToString());
                post.Parameters.Add("valorSonda", GenericHelper.ParseString(attemptBB.valor));
                post.Parameters.Add("formato", "02"); //01 - HTML , 02 - XML , 03 - String

                if (post.Send())
                {
                    //Trato o retorno da sonda
                    string xml = post.Response.Trim(new char[] { '\r', '\n', '\t', ' ' });
                    xml = xml.Replace("<!DOCTYPE lojavirtual SYSTEM 'lojavirtual.dtd'>", "");

                    XmlDocument xmlDoc = new XmlDocument();
                    xmlDoc.LoadXml(xml);

                    //Parametros de retorno da Sonda
                    string tpPagamento = xmlDoc.SelectSingleNode("/FORMULARIO/ENTRADA[@nome='tpPagamento']").Attributes["valor"].Value;
                    string refTran = xmlDoc.SelectSingleNode("/FORMULARIO/ENTRADA[@nome='reftran']").Attributes["valor"].Value;
                    string idConv = xmlDoc.SelectSingleNode("/FORMULARIO/ENTRADA[@nome='idConv']").Attributes["valor"].Value;
                    string situacao = xmlDoc.SelectSingleNode("/FORMULARIO/ENTRADA[@nome='situacao']").Attributes["valor"].Value;
                    string valor = xmlDoc.SelectSingleNode("/FORMULARIO/ENTRADA[@nome='valor']").Attributes["valor"].Value;
                    string dataPagamento = xmlDoc.SelectSingleNode("/FORMULARIO/ENTRADA[@nome='dataPagamento']").Attributes["valor"].Value;

                    if (String.IsNullOrEmpty(situacao))
                        return;

                    attemptBB.situacao = situacao;
                    
                    //00 = pagamento efetuado;
                    if (situacao == "00")
                    {
                        attemptBB.bbpagStatus = (byte)PaymentAttemptBBPagStatus.End;
                        attemptBB.msgret = "Pagamento efetuado";
                        attempt.status = (int)PaymentAttemptStatus.Paid;
                        attempt.lastUpdate = DateTime.Now;
                        attempt.returnMessage = attemptBB.msgret;

                        sendPost = true;

                        GenericHelper.LogFile("EasyPagObject::BB::Sonda.cs::UpdateStatus storeId=" + order.storeId + " orderId=" + order.orderId + " paymentAttemptId=" + attempt.paymentAttemptId + " a tentativa de pagamento será atualizada para pago", LogFileEntryType.Information);
                    }
                    else
                    {
                        switch (situacao)
                        {
                            case "01":
                                attemptBB.msgret = "pagamento não autorizado";
                                break;
                            case "02":
                                attemptBB.msgret = "erro no processamento da consulta";
                                break;
                            case "03":
                                attemptBB.msgret = "pagamento não localizado";
                                break;
                            case "10":
                                attemptBB.msgret = "campo ”idConv” inválido ou nulo";
                                break;
                            case "11":
                                attemptBB.msgret = "valor informado é inválido, nulo ou não confere com o valor registrado";
                                break;
                            case "12":
                                attemptBB.msgret = "campo “refTran” inválido ou nulo";
                                break;
                            case "99":
                                attemptBB.msgret = "operação cancelada pelo cliente";
                                break;
                            default:
                                attemptBB.msgret = "erro desconhecido";
                                break;
                        }
                    }

                    attemptBB.dataSonda = DateTime.Now;
                    attemptBB.qtdSonda++;
                    attemptBB.sondaOffline = true;
                }
                else
                {
                    attemptBB.situacao = "-1";
                    attemptBB.msgret = post.Response;
                }

                //atualizo parametros da sonda
                attemptBB.TruncateStringFields();
                DataFactory.PaymentAttemptBB().Update(attemptBB);
                if (attempt.status != oldAttemptStatus)
                {
                    attempt.TruncateStringFields();
                    attempt.lastUpdate = DateTime.Now;
                    DataFactory.PaymentAttempt().Update(attempt);
                    GenericHelper.UpdateOrderStatusByAttemptStatus(order, attempt.status);
                }

                if (sendPost)
                {
                    DStore store = DataFactory.Store().Locate(order.storeId);
                    DHandshakeConfiguration handshakeConfiguration = DataFactory.HandshakeConfiguration().Locate(store.handshakeConfigurationId);
                    Ensure.IsNotNull(handshakeConfiguration, "Configuração de handshake {0} não encontrada, o post nao será enviado", store.handshakeConfigurationId);

                    SuperPag.Handshake.Helper.SendFinalizationPost(attempt.paymentAttemptId);

                    if (handshakeConfiguration.autoPaymentConfirm)
                        SuperPag.Handshake.Helper.SendPaymentPost(attempt.paymentAttemptId);
                }
            }
            catch (Exception ex)
            {
                GenericHelper.LogFile("EasyPagObject::BB::Sonda.cs::UpdateStatus orderId=" + orderId + " paymentAttemptId=" + paymentAttemptId + " " + ex.Message, LogFileEntryType.Error);
            }
        }
    }
}
