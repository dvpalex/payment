using System;
using System.Web.Mail;
using System.Configuration;
using System.Xml;
using System.Xml.Xsl;
using System.Reflection;
using SuperPag.Framework.Helper;

namespace SuperPag.Framework.Web.WebController
{
	public class ErrorMailer
	{
		private StepRecorder _stepRecorder;
		private Exception _ex;
		private static XslTransform _xslTransform;
		private static string _hostName;
		private static string _mailBody;

		static ErrorMailer() {
            //Assembly currentAssembly = Assembly.GetExecutingAssembly();
            //string resourcePath = ResourcesHelper.FindResourcePath(currentAssembly, "ErrorMail.xsl");
			
            ////Carrega o xsl que será usado na formatação do xml de steps
            //using (System.IO.Stream stream = currentAssembly.GetManifestResourceStream(resourcePath)) {
            //    XmlTextReader xmlReader = new XmlTextReader(stream);

            //    try {
            //        XslTransform transform = new XslTransform();
            //        transform.Load(xmlReader, null, null);

            //        _xslTransform = transform;
            //    } finally {
            //        xmlReader.Close();
            //    }
            //}

            ////Carrega o HTML com o corpo do email
            //resourcePath = 
            //    ResourcesHelper.FindResourcePath(currentAssembly, "ErrorMail.htm");

            //System.IO.StreamReader sr = null;
            //try
            //{
            //    sr = new System.IO.StreamReader(
            //        currentAssembly.GetManifestResourceStream(resourcePath));

            //    _mailBody = sr.ReadToEnd();
            //} finally  {
            //    if(sr != null)
            //    {
            //        sr.Close();
            //    }
            //}

            ////Define o hostname
            //_hostName = System.Net.Dns.GetHostName();
		}

		public ErrorMailer(StepRecorder stepRecorder, Exception ex) {
			this._stepRecorder = stepRecorder;
			this._ex = ex;
		}

		public void SendMessage() {	
            ////TODO: ACERTAR CONFIGURADOR
            ////Obtem as configurações de servidor e destinatários do web config
            //string addresses = ""; // Configuration.ConfigurationReader.GetEmailErrorTo();

            //if(addresses == null || addresses.Trim() == "") { return; }

            //string from = ""; //Configuration.ConfigurationReader.GetEmailErrorFrom();
            //string smtpServer = ""; //Configuration.ConfigurationReader.GetEmailErrorSmtpServer();
            //string subject = ""; //Configuration.ConfigurationReader.GetEmailErrorSubject();

            ////Cria e configura a mensagem de e-mail
            //MailMessage mailMessage = new MailMessage();
            //mailMessage.To = addresses;
            //mailMessage.From = from;
            //mailMessage.Subject = subject + " - Host: " + _hostName;

            ////Configura para o formato html
            //mailMessage.BodyFormat = MailFormat.Html;
			
            //string mailBody = _mailBody;
            ////Host Name
            //mailBody = mailBody.Replace("@@HOSTNAME@@", _hostName);
            ////Mensagem
            //mailBody = mailBody.Replace("@@MESSAGE@@", _ex.Message);
            ////Stack trace
            //string stackTrace;
            //if(_ex.StackTrace != null)
            //{
            //    stackTrace = _ex.StackTrace.Replace(" in ", "<br/>in ").Replace(" at ", "<br/>at ");
            //} 
            //else
            //{
            //    stackTrace = "";
            //}
			
            //mailBody = mailBody.Replace("@@TRACE@@", stackTrace);

            ////Obtem o xml com os passos do usuário
            //XmlDocument xmlSteps = _stepRecorder.GetUserXml();
            //System.IO.StringWriter stringWriter = new System.IO.StringWriter();
            ////Formata o xml
            //_xslTransform.Transform(xmlSteps, null, stringWriter, null);
            //mailBody = mailBody.Replace("@@LOG@@", stringWriter.ToString());
			
            ////Preenche a mensagem
            //mailMessage.Body = mailBody;

            ////Configura o servidor SMTP
            //SmtpMail.SmtpServer = ""; //Configuration.ConfigurationReader.GetEmailErrorSmtpServer();

            ////Envia a mensagem
            //SmtpMail.Send(mailMessage);	
		}

	
	}
}
