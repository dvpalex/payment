using System;
using System.Configuration;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Data.Sql;
using System.Data.SqlClient;
using SuperPag;
using SuperPag.Data;
using SuperPag.Data.Messages;
using SuperPag.Helper;
using SuperPag.Handshake;

namespace PostsNaoConfirmados
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                string[] StoreId = ConfigurationManager.AppSettings["storeId"].ToString().Split(',');
                foreach (string var in StoreId)
                {
                    int storeId = int.Parse(var);

                    //Posts de pagamento
                    //Listar attempts que necessitam ser reenviadas
                    DServicePaymentPost[] listaPayment = DataFactory.ServicePaymentPost().List(storeId);
                    if (listaPayment != null)
                        //Para cada attempt, verificar tipo de handshake e chamar
                        foreach (DServicePaymentPost paymentPost in listaPayment)
                        {
                            DStore store = DataFactory.Store().Locate(storeId);
                            DServicesConfiguration servicesConfiguration = DataFactory.ServicesConfiguration().Locate(storeId);
                            Ensure.IsNotNull(servicesConfiguration, "Não possível encontrar a configuração do post (ServicesConfiguration) para a loja {0}", storeId);

                            //if (paymentPost.postRetries >= servicesConfiguration.offLinePaymentRetries)
                            //{
                            //    if (paymentPost.emailSentDate == DateTime.MinValue && !String.IsNullOrEmpty(servicesConfiguration.contingencyEmails))
                            //        if (Helper.SendPaymentContigencyEmail(store, order.storeReferenceOrder, attempt.paymentAttemptId, paymentPost.installmentNumber, servicesConfiguration.contingencyEmails))
                            //        {
                            //            paymentPost.emailSentDate = DateTime.Now;
                            //            DataFactory.ServicePaymentPost().Update(paymentPost);
                            //        }

                            //    return 0;
                            //}

                            DHandshakeConfiguration handshakeConfiguration = DataFactory.HandshakeConfiguration().Locate(store.handshakeConfigurationId);
                            if (handshakeConfiguration.handshakeType == (int)HandshakeType.HtmlSPag10)
                            {
                                SuperPag.Handshake.Html.Handshake html = new SuperPag.Handshake.Html.Handshake();
                                html.SendPaymentPost(paymentPost.paymentAttemptId, paymentPost.installmentNumber);
                            }
                            else if (handshakeConfiguration.handshakeType == (int)HandshakeType.XmlSPag10)
                            {
                                SuperPag.Handshake.Xml.Handshake xml = new SuperPag.Handshake.Xml.Handshake();
                                xml.SendPaymentPost(paymentPost.paymentAttemptId, paymentPost.installmentNumber);



                                try
                                {
                                    DPaymentAttempt ObjDPaymentAttempt = DataFactory.PaymentAttempt().Locate(paymentPost.paymentAttemptId);
                                    DOrder ObjDOrder = DataFactory.Order().Locate(ObjDPaymentAttempt.orderId);
                                    DConsumer ObjDConsumer = DataFactory.Consumer().Locate(ObjDOrder.consumerId);
                                    DPaymentForm ObjDPaymentForm = DataFactory.PaymentForm().Locate(ObjDPaymentAttempt.paymentFormId);

                                    if (paymentPost.postStatus == 3 && paymentPost.postRetries == 3)
                                    {
                                        string Strbody = "Não foi possivel enviar o post de pagamento para os dados abaixo:<br>";
                                        Strbody += " Num pedido=" + ObjDOrder.storeReferenceOrder + " Data do pedido=" + ObjDPaymentAttempt.startTime.ToString();
                                        Strbody += " Valor do pedido=" + ObjDPaymentAttempt.price.ToString() + " order=" + ObjDOrder.orderId.ToString();
                                        Strbody += " Order Date=" + ObjDOrder.creationDate.ToString();
                                        Strbody += " PaymentForm=" + ObjDPaymentForm.paymentFormId.ToString() + " - " + ObjDPaymentForm.name;
                                        Strbody += " TotalOrderAmount=" + ObjDOrder.totalAmount.ToString();
                                        Strbody += " Nome=" + ObjDConsumer.name + " Mail=" + ObjDConsumer.email + " Cpf=" + ObjDConsumer.CPF;

                                        string strmail = servicesConfiguration.contingencyEmails;

                                        if (!SuperPag.Handshake.Helper.SendBoletoEmail(strmail, string.Empty, Strbody, "Posts não confirmados", null))
                                        {
                                            GenericHelper.LogFile("EasyPagObject::HelperXml.cs::PaymentPost Não foi possivel enviar o e-mail " + strmail, LogFileEntryType.Error);
                                        }
                                        else
                                        {
                                            GenericHelper.LogFile("EasyPagObject::HelperXml.cs::PaymentPost E-mail enviado com sucesso!!", LogFileEntryType.Information);
                                        }
                                    }
                                    GenericHelper.LogFile("EasyPagObject::HelperXml.cs::PaymentPost" + " Status=" + paymentPost.postStatus + " Qtdepost=" + paymentPost.postRetries, LogFileEntryType.Information);
                                }
                                catch (Exception ex)
                                {
                                    GenericHelper.LogFile("EasyPagObject::HelperXml.cs::PaymentPost" + ex, LogFileEntryType.Error);
                                }
                            }
                        }
                }
            }
            catch (Exception e)
            {
                GenericHelper.LogFile("PostsNaoConfirmados::Program.cs::Main " + e.Message, LogFileEntryType.Error);
            }
        }
    }
}
