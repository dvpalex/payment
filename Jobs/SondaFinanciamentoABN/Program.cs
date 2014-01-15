using System;
using System.Configuration;
using System.Collections.Generic;
using System.Text;
using SuperPag.Helper;
using SuperPag.Data.Messages;
using SuperPag.Data;
using System.Xml;
using SuperPag;
using SuperPag.Business.Messages;

namespace SondaFinanciamentoABN
{
    class Program
    {
        static void Main(string[] args)
        {
            //Logo inicio do Job.
            GenericHelper.LogFile("SondaFinanciamentoABN::Program.cs::Main Início da sonda", LogFileEntryType.Information);
            ServerHttpHtmlRequisition post = new ServerHttpHtmlRequisition();
            try
            {

                string interval = ConfigurationManager.AppSettings["CheckInterval"];
                if (String.IsNullOrEmpty(interval))
                    interval = "15";

                DateTime sinceDate = DateTime.Now.AddDays(-int.Parse(interval));

                //Levantamos todas as Attempts do ABN que constam como AN "Em análise" desde a data especificada na configuracao
                //DPaymentAttemptABN[] attemptsABN = DataFactory.PaymentAttemptABN().ListSonda("AN", sinceDate);

                IList<MPaymentAttemptABN> attemptsABN = SuperPag.Business.PaymentAttemptABN.ListSonda("AN");

                if (attemptsABN != null)
                {
                    foreach (MPaymentAttemptABN attemptABN in attemptsABN)
                    {
                        Ensure.IsNotNull(attemptABN.NumProposta, "Número de proposta inválido.");

                        //Pegamentos o AgentSetup de cada cliente do respectivo pedido.
                        DPaymentAttempt attempt = DataFactory.PaymentAttempt().Locate(attemptABN.PaymentAttemptId);
                        Ensure.IsNotNull(attempt, "Tentativa de pagamento inválida.");
                        DPaymentAgentSetupABN setupABN = DataFactory.PaymentAgentSetupABN().Locate(attempt.paymentAgentSetupId);
                        Ensure.IsNotNull(setupABN, "Configuração ABN inválida.");
                        DOrder order = DataFactory.Order().Locate(attempt.orderId);
                        DConsumer consumer = DataFactory.Consumer().Locate(order.consumerId);
                        if (String.IsNullOrEmpty(consumer.CNPJ) && String.IsNullOrEmpty(consumer.CPF))
                            Ensure.IsNotNull(null, "CPF ou CNPJ inválido.");

                        post.Url = "https://wwws.aymorefinanciamentos.com.br/scripts/flv.dll/Consulta?Pagina=Consulta";
                        post.Parameters.Add("VAR01", setupABN.codigoABN);
                        post.Parameters.Add("VAR02", "04");
                        post.Parameters.Add("VAR03", ConfigurationManager.AppSettings["abnReturnUrl"]);
                        post.Parameters.Add("VAR09", String.IsNullOrEmpty(consumer.CNPJ) ? consumer.CPF : consumer.CNPJ);
                        post.Parameters.Add("VAR29", attemptABN.NumProposta);
                        post.Send();

                        //TODO: Analisar retorno, se for de sistema indisponível, pegar valores atuais, mas setar sempre como em análise para um checagem posterior quanto na sonda.
                        XmlDocument xmlDoc = new XmlDocument();
                        xmlDoc.LoadXml(post.Response.Trim());

                        string statusProposta = xmlDoc.SelectSingleNode("//status").Value;
                        if (statusProposta != attemptABN.StatusProposta)
                        {
                            attemptABN.CodRet = int.Parse(xmlDoc.SelectSingleNode("//ret01").Value);
                            attemptABN.MsgRet = xmlDoc.SelectSingleNode("//message").Value;
                            attemptABN.StatusProposta = statusProposta;
                            GenericHelper.LogFile("SondaFinanciamentoABN::Program.cs::Main Atualizando o status paymentattemptid=" + attempt.paymentAttemptId.ToString() + " proposta=" + attemptABN.NumProposta + " status=" + attemptABN.StatusProposta, LogFileEntryType.Information);
                            SuperPag.Business.PaymentAttemptABN.Update(attemptABN);
                            //DataFactory.PaymentAttemptABN().Update((DPaymentAttemptABN)attemptABN);
                        }
                    }

                }

                //Logo fim do Job.
                GenericHelper.LogFile("SondaFinanciamentoABN::Program.cs::Main Fim da Sonda", LogFileEntryType.Information);
            }
            catch (Exception ex)
            {
                GenericHelper.LogFile("SondaFinanciamentoABN::Program.cs::Main Erro: " + "retorno abn=" + post.Response + " Erro=" + ex.Message, LogFileEntryType.Error);
            }
        }
    }
}
