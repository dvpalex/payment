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

public partial class Views_extratofinancial2 : SuperPag.Framework.Web.WebControls.MessagePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        lblNomeUsuario.Text = User.Identity.Name;

        MCExtrato2 mcExtrato = (MCExtrato2)this.GetMessageArray(typeof(MCExtrato2));
        ButtonExportExcel1.DadosExportacao = mcExtrato; 

    }
    protected void btnGoBack_Click(object sender, EventArgs e)
    {
        this.RaiseEvent(typeof(Ev.ExtratoFinancial2.GoBack));
    }
}
