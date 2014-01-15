using System;
using System.Collections;
using SuperPag.Framework.Web.WebController;
using SuperPag.Framework.Web;
using SuperPag.Business.Messages;
using SuperPag.Business;
using Controller.Lib;
using Controller.Lib.Commands;
using SuperPag;

namespace Controller.Lib.Views.Ev.Boleto
{
    public class Reenviar : BaseEvent
    {
        protected override BaseCommand OnExecute()
        {
            BaseCommand b = null;
            MPaymentAttempt mPaymentAttempt = null;
            string storeId = "";

            try
            {
                mPaymentAttempt = (MPaymentAttempt)this.GetMessage(typeof(MPaymentAttempt));
                MPaymentAttemptBoleto mPaymentAttemptBoleto = PaymentAttemptBoleto.Locate(mPaymentAttempt.PaymentAttemptId);

                storeId = mPaymentAttempt.Order.Store.StoreId.ToString();

                Ensure.IsNotNull(System.Configuration.ConfigurationManager.AppSettings["SmtpServer"], "Servidor SMTP não configurado");

                string from = "";
                if (!String.IsNullOrEmpty(mPaymentAttempt.Order.Store.MailSenderEmail))
                {
                    //TODO: validar e-mail
                    from = string.Format("{0} <{1}>", mPaymentAttempt.Order.Store.Name, mPaymentAttempt.Order.Store.MailSenderEmail);
                }
                else
                {
                    Ensure.IsNotNull(System.Configuration.ConfigurationManager.AppSettings["defaultSenderMail"], "Remetente de e-mail não configurado");
                    from = string.Format("{0} <{1}>", mPaymentAttempt.Order.Store.Name, System.Configuration.ConfigurationManager.AppSettings["defaultSenderMail"]);
                }

                System.Net.Mail.MailMessage message = new System.Net.Mail.MailMessage(from, mPaymentAttempt.Order.Consumer.Email);
                message.Subject = "Boleto de cobrança";

                System.Text.StringBuilder body = new System.Text.StringBuilder();
                //TODO: arquivo texto
                body.AppendLine("Acesse o link abaixo");
                body.AppendLine();
                Ensure.IsNotNull(System.Configuration.ConfigurationManager.AppSettings["urlSuperpag"], "Url do superpag não configurada");
                body.AppendLine(string.Format("{0}/agents/boleto/showboleto.aspx?id={1}", System.Configuration.ConfigurationManager.AppSettings["urlSuperpag"], mPaymentAttempt.PaymentAttemptId.ToString()));
                body.AppendLine();

                message.Body = body.ToString();

                System.Net.Mail.SmtpClient smtp = new System.Net.Mail.SmtpClient(System.Configuration.ConfigurationManager.AppSettings["SmtpServer"]);
                smtp.Send(message);

                b = this.MakeCommand(typeof(ShowOrder));
                b.Parameters["OrderId"] = mPaymentAttempt.Order.OrderId;
            }
            catch (Exception e)
            {
                SuperPag.Helper.GenericHelper.LogFile("EasyPagObject::Reenviarboleto.cs::Reenviar.OnExecute storeId=" + storeId + " " + e.Message + " " + (e.InnerException != null ? e.InnerException.Message : ""), LogFileEntryType.Error);
                
                b = this.MakeCommand(typeof(ReenviarBoleto));
                b.Parameters["PaymentAttempt"] = mPaymentAttempt;
                b.Parameters["SendError"] = "Não foi possível enviar o email";
            }            
            
            return b;
        }
    }
}
