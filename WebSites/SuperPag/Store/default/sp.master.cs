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
using System.IO;
using System.Globalization;
using System.Threading;
using System.Resources;
using SuperPag.Helper;
using SuperPag;

public partial class Store_default_sp : System.Web.UI.MasterPage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            string http = (Request.ServerVariables["HTTPS"] == "off" ? "http" : "https");

            if (Session["storeId"] != null && (int)Session["storeId"] > 0 && File.Exists(Server.MapPath("~/Store/" + (int)Session["storeId"] + "/Style.css")))
            {
                //Alteracao para cuspir o estilo na pagina (webconnector nao pega a tag "link"
                this.styleTag.InnerHtml = File.ReadAllText(Server.MapPath("~/Store/" + (int)Session["storeId"] + "/Style.css"));
                this.linkTag.Href = http + "://" + Request.ServerVariables["SERVER_NAME"] + "/SuperPag/Store/" + (int)Session["storeId"] + "/Style.css";

                //Alteração para atender a loja Loreal adicionando um banner na finalização do pedido       
                try
                {
                    //Para adicionar o contador usar a Session["xmlHandshake"]
                    //Alteração para atender a loja Loreal adicionando um banner na finalização do pedido
                    if (Session["storeId"].ToString().Equals("5") && Request.Path.Equals("/finalization.aspx"))
                    {
                        Literal_LOreal.Visible = true;                        
                    }                    
                }
                catch { GenericHelper.LogFile("SuperPagWS::MasterPage:: Problema no ebit da Loreal" + Request.Path, LogFileEntryType.Error); }                
            }
            else
            {
                this.styleTag.InnerHtml = File.ReadAllText(Server.MapPath("~/Store/default/Style.css"));
                this.linkTag.Href = http + "://" + Request.ServerVariables["SERVER_NAME"] + "/Store/default/Style.css";
            }

            lblCopyright.Text = HttpUtility.HtmlEncode(lblCopyright.Text);
            lblFooter.Text = HttpUtility.HtmlEncode(lblFooter.Text);
        }
    }
}
