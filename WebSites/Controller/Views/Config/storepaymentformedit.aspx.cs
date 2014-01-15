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


public partial class Views_storepaymentformedit : SuperPag.Framework.Web.WebControls.MessagePage 
{
    protected void Page_Load(object sender, EventArgs e)
    {        
        lblNomeUsuario.Text = User.Identity.Name;

        MStorePaymentForm mStorePaymentForm = (MStorePaymentForm)this.GetMessage(typeof(MStorePaymentForm));
        ddlPaymentForm.Visible = (mStorePaymentForm.IsInsert);
        lblPaymentForm.Visible = !mStorePaymentForm.IsInsert;
    }

    protected void btnSave_Click(object sender, EventArgs e)
    {
        this.RaiseEvent(typeof(Ev.EditStorePaymentForm.Save));
    }

    protected void btnCancel_Click(object sender, EventArgs e)
    {
        this.RaiseEvent(typeof(Ev.EditStorePaymentForm.Cancel));
    }

    protected void ddlPaymentForm_SelectedIndexChanged(object sender, EventArgs e)
    {
        Hashtable p = new Hashtable();
        p.Add("PaymentFormId",int.Parse(ddlPaymentForm.SelectedValue));
        this.RaiseEvent(typeof(Ev.EditStorePaymentForm.Reload), p);

    }
    protected void btnDelete_Click(object sender, EventArgs e)
    {
        this.RaiseEvent(typeof(Ev.EditStorePaymentForm.Delete));
    }
}
