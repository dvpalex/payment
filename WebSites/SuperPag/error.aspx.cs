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
            if (Request.QueryString["message"] != null)
            {
                //Erro esperado
                if (HttpContext.Current.Session["orderId"] != null && Ensure.IsNumeric(HttpContext.Current.Session["orderId"]))
                {
                    //Seto o status do pedido
                    GenericHelper.SetOrderStatus(HttpContext.Current, WorkflowOrderStatus.Error, Request.QueryString["message"]);

                    string storeId = (HttpContext.Current.Session["storeId"] != null ? HttpContext.Current.Session["storeId"].ToString() : "");
                    if (!String.IsNullOrEmpty(storeId))
                        logMessage.Append(" storeId=" + storeId);

                    logMessage.Append(" orderId=" + HttpContext.Current.Session["orderId"].ToString());
                }

                string message = Request.QueryString["message"];
                if (!String.IsNullOrEmpty(message))
                    logMessage.Append(" msg erro=" + message);

                lblDescription.Text = message;
            }
            else
            {
                //Erro inexperado
                Exception ex = Server.GetLastError();
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
