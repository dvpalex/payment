using System;
using System.Data;
using System.Configuration;
using System.Collections;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using SuperPag;
using SuperPag.Data;
using SuperPag.Data.Messages;
using SuperPag.Helper;
using System.Text;

public partial class error : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        StringBuilder logMessage = new StringBuilder();
        logMessage.Append("SuperPag::error.aspx.cs::Page_Load ");

        try
        {
            //Erro inexperado
            if (HttpContext.Current.Items["_error"] != null)
            {
                Exception ex = (Exception)HttpContext.Current.Items["_error"];
                if (ex.InnerException != null)
                    ex = ex.InnerException;

                logMessage.Append("URL: ");
                logMessage.Append(Request.Url.ToString());
                logMessage.Append(" Source: ");
                logMessage.Append(ex.Source);
                logMessage.Append(" Message: ");
                logMessage.Append(ex.Message);
                logMessage.Append(" Stack trace: ");
                logMessage.Append(ex.StackTrace);
                Server.ClearError();

                lblDescription.Text = "Por favor, tente novamente.";
            }

            GenericHelper.LogFile(logMessage.ToString(), LogFileEntryType.Error);
        }
        catch (Exception ex)
        {
            logMessage.Append(ex.Message);
            GenericHelper.LogFile(logMessage.ToString(), LogFileEntryType.Error);
        }
    }
}
