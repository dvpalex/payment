using System;
using System.Collections.Generic;
using System.Text;
using SuperPag.Data.Messages;
using SuperPag.Data;
using SuperPag.Helper;
using System.Xml;

namespace SuperPag.Agents.ItauShopLine
{
    public class Sonda
    {
        public static void UpdateStatus(DPaymentAttemptItauShopline[] arrAttemptItau)
        {
            foreach (DPaymentAttemptItauShopline attemptItau in arrAttemptItau)
                UpdateStatus(attemptItau);
        }

        public static void UpdateStatus(DPaymentAttemptItauShopline attemptItau)
        {
            string orderId = "", paymentAttemptId = "";
            bool sendPost = false;
            
            try
            {
                paymentAttemptId = attemptItau.paymentAttemptId.ToString();
                DPaymentAttempt attempt = DataFactory.PaymentAttempt().Locate(attemptItau.paymentAttemptId);
                DOrder order = DataFactory.Order().Locate(attempt.orderId);
                orderId = order.orderId.ToString();
                DPaymentAgentSetupItauShopline agentsetup = DataFactory.PaymentAgentSetupItauShopline().Locate(attempt.paymentAgentSetupId);
                Ensure.IsNotNull(agentsetup, "Configuração do agente {0} não encontrada", attempt.paymentAgentSetupId);

                if (String.IsNullOrEmpty(agentsetup.urlItauSonda))
                    return;

                int oldAttemptStatus = attempt.status;

                Crypto cripto = new Crypto();
                string consulta = cripto.GeraConsulta(agentsetup.businessKey, attemptItau.agentOrderReference.ToString(), "1", agentsetup.criptoKey); //0 - HTML , 1 - XML
                
                ServerHttpHtmlRequisition post = new ServerHttpHtmlRequisition();
                post.Method = "POST";
                post.UpperKeys = false;
                post.Url = agentsetup.urlItauSonda;
                post.Parameters.Add("DC", consulta);

                if (post.Send())
                {
                    //Trato o retorno da sonda
                    XmlDocument xmlDoc = new XmlDocument();
                    xmlDoc.LoadXml(post.Response);

                    //Parametros padrao
                    string codEmp = xmlDoc.SelectSingleNode("/consulta/PARAMETER/PARAM[@ID='CodEmp']").Attributes["VALUE"].Value;
                    //Parametros de retorno da Sonda
                    string ped = xmlDoc.SelectSingleNode("/consulta/PARAMETER/PARAM[@ID='Pedido']").Attributes["VALUE"].Value;
                    string tipPag = xmlDoc.SelectSingleNode("/consulta/PARAMETER/PARAM[@ID='tipPag']").Attributes["VALUE"].Value;
                    string sitPag = xmlDoc.SelectSingleNode("/consulta/PARAMETER/PARAM[@ID='sitPag']").Attributes["VALUE"].Value;
                    string valor = xmlDoc.SelectSingleNode("/consulta/PARAMETER/PARAM[@ID='Valor']").Attributes["VALUE"].Value;
                    string dtPag = xmlDoc.SelectSingleNode("/consulta/PARAMETER/PARAM[@ID='dtPag']").Attributes["VALUE"].Value;
                    string codAut = xmlDoc.SelectSingleNode("/consulta/PARAMETER/PARAM[@ID='codAut']").Attributes["VALUE"].Value;
                    string numId = xmlDoc.SelectSingleNode("/consulta/PARAMETER/PARAM[@ID='numId']").Attributes["VALUE"].Value;
                    string compVend = xmlDoc.SelectSingleNode("/consulta/PARAMETER/PARAM[@ID='compVend']").Attributes["VALUE"].Value;
                    string tipCart = xmlDoc.SelectSingleNode("/consulta/PARAMETER/PARAM[@ID='tipCart']").Attributes["VALUE"].Value;

                    attemptItau.tipPag = tipPag;
                    attemptItau.sitPag = sitPag;
                    attemptItau.dtPag = dtPag;
                    attemptItau.codAut = codAut;
                    attemptItau.numId = numId;
                    attemptItau.compVend = compVend;
                    attemptItau.tipCart = tipCart;

                    if (String.IsNullOrEmpty(sitPag))
                        return;

                    //00 = pagamento efetuado;
                    if (sitPag == "00" || sitPag == "05")
                    {
                        attemptItau.itauStatus = (byte)PaymentAttemptItauShoplineStatus.End;
                        attemptItau.msgret = (sitPag == "00" ? "Pagamento efetuado" : "Pagamento efetuado, aguardando compensação");
                        attempt.status = (int)PaymentAttemptStatus.Paid;
                        attempt.lastUpdate = DateTime.Now;
                        attempt.returnMessage = attemptItau.msgret;

                        sendPost = true;
                        
                        GenericHelper.LogFile("EasyPagObject::ItauShopline::Sonda.cs::UpdateStatus storeId=" + order.storeId + " orderId=" + order.orderId + " paymentAttemptId=" + attempt.paymentAttemptId + " a tentativa de pagamento será atualizada para pago", LogFileEntryType.Information);
                    }
                    else
                    {
                        switch (sitPag)
                        {
                            case "01":
                                attemptItau.msgret = "pagamento não finalizado";
                                break;
                            case "02":
                                attemptItau.msgret = "erro no processamento da consulta";
                                break;
                            case "03":
                                attemptItau.msgret = "pagamento não localizado";
                                break;
                            case "04":
                                attemptItau.msgret = "bloqueto emitido com sucesso";
                                break;
                            case "06":
                                attemptItau.msgret = "pagamento não compensado";
                                break;
                            default:
                                attemptItau.msgret = "erro desconhecido";
                                break;
                        }
                    }

                    attemptItau.dataSonda = DateTime.Now;
                    attemptItau.qtdSonda++;
                    attemptItau.sondaOffline = true;
                }
                else
                {
                    attemptItau.sitPag = "-1";
                    attemptItau.msgret = post.Response;
                }

                //atualizo parametros da sonda
                attemptItau.TruncateStringFields();
                DataFactory.PaymentAttemptItauShopline().Update(attemptItau);
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
                GenericHelper.LogFile("EasyPagObject::ItauShopline::Sonda.cs::UpdateStatus orderId=" + orderId + " paymentAttemptId=" + paymentAttemptId + " " + ex.Message, LogFileEntryType.Error);
            }
        }
    }
}
