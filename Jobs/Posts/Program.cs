using System;
using System.Collections.Generic;
using System.Text;
using SuperPag;
using SuperPag.Data;
using SuperPag.Data.Messages;
using SuperPag.Helper;
using SuperPag.Handshake;

namespace Posts
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                int sent = 0;

                //Posts de finalizacao
                //Listar attempts que necessitam ser reenviadas
                DServiceFinalizationPost[] listaFinalization = DataFactory.ServiceFinalizationPost().List(new int[] { (int)PostStatus.Sent, (int)PostStatus.Error });
                if (listaFinalization != null)
                    //Para cada attempt, verificar tipo de handshake e chamar
                    foreach (DServiceFinalizationPost finalizationPost in listaFinalization)
                        sent += SendFinalizationPosts(finalizationPost);
                
                if(sent == 0)
                    GenericHelper.LogFile("Posts::Program.cs::Main nenhum post de finalização", LogFileEntryType.Information);

                sent = 0;

                //Posts de pagamento
                //Listar attempts que necessitam ser reenviadas
                DServicePaymentPost[] listaPayment = DataFactory.ServicePaymentPost().List(new int[] { (int)PostStatus.Sent, (int)PostStatus.Error });
                if (listaPayment != null)
                    //Para cada attempt, verificar tipo de handshake e chamar
                    foreach (DServicePaymentPost paymentPost in listaPayment)
                        sent += SendPaymentPosts(paymentPost);
                
                if(sent == 0)
                    GenericHelper.LogFile("Posts::Program.cs::Main nenhum post de pagamento", LogFileEntryType.Information);
            }
            catch (Exception e)
            {
                GenericHelper.LogFile("Posts::Program.cs::Main " + e.Message, LogFileEntryType.Error);
            }
        }

        internal static int SendFinalizationPosts(DServiceFinalizationPost finalizationPost)
        {
            string paymentAttempId = "";

            try
            {
                paymentAttempId = (finalizationPost != null ? finalizationPost.paymentAttemptId.ToString() : "");

                DPaymentAttempt attempt = DataFactory.PaymentAttempt().Locate(finalizationPost.paymentAttemptId);
                DOrder order = DataFactory.Order().Locate(attempt.orderId);
                DStore store = DataFactory.Store().Locate(order.storeId);
                DServicesConfiguration servicesConfiguration = DataFactory.ServicesConfiguration().Locate(order.storeId);
                Ensure.IsNotNull(servicesConfiguration, "Não possível encontrar a configuração do post (ServicesConfiguration) para a loja {0}", order.storeId);

                if (finalizationPost.postRetries >= servicesConfiguration.offLineFinalizationRetries)
                {
                    if (finalizationPost.emailSentDate == DateTime.MinValue && !String.IsNullOrEmpty(servicesConfiguration.contingencyEmails))
                        if (Helper.SendFinalizationContigencyEmail(store, order.storeReferenceOrder, attempt.paymentAttemptId, servicesConfiguration.contingencyEmails))
                        {
                            finalizationPost.emailSentDate = DateTime.Now;
                            DataFactory.ServiceFinalizationPost().Update(finalizationPost);
                        }

                    return 0;
                }
                
                DHandshakeConfiguration handshakeConfiguration = DataFactory.HandshakeConfiguration().Locate(store.handshakeConfigurationId);
                if (handshakeConfiguration.handshakeType == (int)HandshakeType.HtmlSPag10)
                {
                    SuperPag.Handshake.Html.FinalizationPost finalization = new SuperPag.Handshake.Html.FinalizationPost(attempt.paymentAttemptId);
                    finalization.IsOffLine = true;
                    finalization.Send();
                }
                else if (handshakeConfiguration.handshakeType == (int)HandshakeType.XmlSPag10)
                {
                    SuperPag.Handshake.Xml.FinalizationPost finalization = new SuperPag.Handshake.Xml.FinalizationPost(attempt.paymentAttemptId);
                    finalization.IsOffLine = true;
                    finalization.Send();
                }
                
                return 1;
            }
            catch (Exception e)
            {
                GenericHelper.LogFile("Posts::Program.cs::SendFinalizationPosts paymentAttempId=" + paymentAttempId + " " + e.Message, LogFileEntryType.Error);
                return 1;
            }
        }

        internal static int SendPaymentPosts(DServicePaymentPost paymentPost)
        {
            string paymentAttempId = "", installmentNumber = "";

            try
            {
                paymentAttempId = (paymentPost != null ? paymentPost.paymentAttemptId.ToString() : "");
                installmentNumber = (paymentPost != null ? paymentPost.installmentNumber.ToString() : "");

                DPaymentAttempt attempt = DataFactory.PaymentAttempt().Locate(paymentPost.paymentAttemptId);
                DOrder order = DataFactory.Order().Locate(attempt.orderId);
                DStore store = DataFactory.Store().Locate(order.storeId);
                DServicesConfiguration servicesConfiguration = DataFactory.ServicesConfiguration().Locate(order.storeId);
                Ensure.IsNotNull(servicesConfiguration, "Não possível encontrar a configuração do post (ServicesConfiguration) para a loja {0}", order.storeId);

                if (paymentPost.postRetries >= servicesConfiguration.offLinePaymentRetries)
                {
                    if (paymentPost.emailSentDate == DateTime.MinValue && !String.IsNullOrEmpty(servicesConfiguration.contingencyEmails))
                        if (Helper.SendPaymentContigencyEmail(store, order.storeReferenceOrder, attempt.paymentAttemptId, paymentPost.installmentNumber, servicesConfiguration.contingencyEmails))
                        {
                            paymentPost.emailSentDate = DateTime.Now;
                            DataFactory.ServicePaymentPost().Update(paymentPost);
                        }
                    
                    return 0;
                }
                
                DHandshakeConfiguration handshakeConfiguration = DataFactory.HandshakeConfiguration().Locate(store.handshakeConfigurationId);
                if (handshakeConfiguration.handshakeType == (int)HandshakeType.HtmlSPag10)
                {
                    SuperPag.Handshake.Html.PaymentPost payment = new SuperPag.Handshake.Html.PaymentPost(attempt.paymentAttemptId, paymentPost.installmentNumber);
                    payment.IsOffLine = true;
                    payment.Send();
                }
                else if (handshakeConfiguration.handshakeType == (int)HandshakeType.XmlSPag10)
                {
                    SuperPag.Handshake.Xml.PaymentPost payment = new SuperPag.Handshake.Xml.PaymentPost(attempt.paymentAttemptId, paymentPost.installmentNumber);
                    payment.IsOffLine = true;
                    payment.Send();
                }
                
                return 1;
            }
            catch (Exception e)
            {
                GenericHelper.LogFile("Posts::Program.cs::SendPaymentPosts paymentAttempId=" + paymentAttempId + " installmentNumber=" + installmentNumber + " " + e.Message, LogFileEntryType.Error);
                return 1;
            }
        }
    }
}
