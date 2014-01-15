using System;
using System.Threading;

namespace SuperPag.Framework.Web.WebController
{
	public class ErrorHandler
	{
		public void Handle(Exception ex)
		{
			StepRecorder stepRecorder = 
				new StepRecorder( null );
			stepRecorder.RecordError(ex.Message);

			stepRecorder.GetUserXml();

			//Envia email
			ErrorMailer errorMail = new ErrorMailer(stepRecorder, ex);

			Thread threadMail = new Thread(new ThreadStart(errorMail.SendMessage));
			threadMail.IsBackground = true;
			threadMail.Start();
		}


	}
}
