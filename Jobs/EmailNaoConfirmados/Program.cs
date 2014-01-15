using System;
using System.Collections.Generic;
using System.Text;
using Pop3;
using System.Collections;
using SuperPag;
using System.Configuration;
using SuperPag.Data;
using SuperPag.Business;
using SuperPag.Data.Messages;

namespace EmailNaoConfirmados
{
    class Program
    {
        static void Main(string[] args)
        {
            string User = ConfigurationSettings.AppSettings["User"].ToString();
            string Pass = ConfigurationSettings.AppSettings["Pass"].ToString();
            string Server = ConfigurationSettings.AppSettings["server"].ToString();

            Pop3Client email = new Pop3Client(User, Pass, Server);
            try
            {
                Console.WriteLine("conectou");

                email.OpenInbox();

                Console.WriteLine("Abriu caixa");

                while (email.NextEmail())
                {
                    if (email.Subject == "Delivery Status Notification (Failure)" || email.Subject == "Delivery Status Notification (Delay)")
                    {
                        if (email.FromSubject != "Error FromSubject")
                        {
                            try
                            {
                                string[] strData = email.FromSubject.Split(';');
                                if (Convert.ToDateTime(strData[1]) >= DateTime.Now.Date)
                                {
                                    SuperPag.Helper.GenericHelper.LogFile("EmailNaoConfirmados::Dados " + strData[0] + " - " + strData[1] , LogFileEntryType.Information);

                                    DPaymentAttemptBoleto ObjDPaymentAttemptBoleto = DataFactory.PaymentAttemptBoleto().Locate(new Guid(strData[0]));
                                    ObjDPaymentAttemptBoleto.Status = false;
                                    ObjDPaymentAttemptBoleto.ErrorMail = email.Body;
                                    DataFactory.PaymentAttemptBoleto().Update(ObjDPaymentAttemptBoleto);
                                    SuperPag.Helper.GenericHelper.LogFile("EmailNaoConfirmados::Atualizado com sucesso!!!", LogFileEntryType.Information);
                                }
                            }
                            catch (Exception ex) { SuperPag.Helper.GenericHelper.LogFile("EmailNaoConfirmados::Erro " + ex, LogFileEntryType.Error); }
                        }
                    }
                }

                Console.WriteLine("Fim");
                email.CloseConnection();
                //Console.ReadLine();
            }
            catch (Exception ex)
            {
                email.CloseConnection();
                Console.WriteLine(ex.ToString());
                //Console.ReadLine();
                SuperPag.Helper.GenericHelper.LogFile("EmailNaoConfirmados::Erro " + ex, LogFileEntryType.Information);
            }
        }
    }
}
