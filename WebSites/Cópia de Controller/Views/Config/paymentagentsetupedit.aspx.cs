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
using SuperPag.Framework.Web.WebControls;
using SuperPag.Framework.Web.WebController;
using SuperPag.Business;
using SuperPag.Business.Messages;
using Ev = Controller.Lib.Views.Ev;


public partial class Views_paymentagentsetupedit : SuperPag.Framework.Web.WebControls.MessagePage 
{
    protected void Page_Load(object sender, EventArgs e)
    {        
        lblNomeUsuario.Text = User.Identity.Name;

        btnDelete.Attributes.Add("onclick", "javascript: if (!confirm('Confirmar exclusão?')) return; ");

        MPaymentAgentSetup mPaymentAgentSetup = (MPaymentAgentSetup)this.GetMessage(typeof(MPaymentAgentSetup));
        ddlPaymentAgent.Enabled = (mPaymentAgentSetup.PaymentAgentSetupId == int.MinValue);
    }

    protected void btnSave_Click(object sender, EventArgs e)
    {
        this.RaiseEvent(typeof(Ev.PaymentAgentSetupEdit.Save));
    }

    protected void btnCancel_Click(object sender, EventArgs e)
    {
        this.RaiseEvent(typeof(Ev.PaymentAgentSetupEdit.Cancel));
    }
    protected void btnDelete_Click(object sender, EventArgs e)
    {
        this.RaiseEvent(typeof(Ev.PaymentAgentSetupEdit.Delete));
    }
}
