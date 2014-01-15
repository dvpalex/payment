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
using SuperPag.Business;
using SuperPag.Business.Messages;
using Ev = Controller.Lib.Views.Ev;

public partial class Views_extratofinancial : SuperPag.Framework.Web.WebControls.MessagePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        lblNomeUsuario.Text = User.Identity.Name;

        MCExtrato mcExtrato = (MCExtrato)this.GetMessageArray(typeof(MCExtrato));

        ButtonExportExcel1.DadosExportacao = mcExtrato; 

    }
    protected void btnGoBack_Click(object sender, EventArgs e)
    {
        this.RaiseEvent(typeof(Ev.ExtratoFinancial.GoBack));
    }
    protected void grdTransaction_MessageEvent(object sender, string eventName, SuperPag.Framework.Message message)
    {
        this.RaiseEvent(typeof(Ev.ExtratoFinancial.ShowTransactionDetails));
    }
}
