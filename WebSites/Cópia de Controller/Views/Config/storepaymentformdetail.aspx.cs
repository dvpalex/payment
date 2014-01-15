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
using SuperPag;
using System.Text;


public partial class Views_storedetail : SuperPag.Framework.Web.WebControls.MessagePage 
{
    MOrder mOrder = null;

    protected void Page_Load(object sender, EventArgs e)
    {        
        lblNomeUsuario.Text = User.Identity.Name;
    }

    protected void btnGoBack_Click(object sender, EventArgs e)
    {
        this.RaiseEvent(typeof(Ev.StorePaymentFormDetail.GoBack));
    }
    protected void lnkIncluir_Click(object sender, EventArgs e)
    {
        this.RaiseEvent(typeof(Ev.StorePaymentFormDetail.InsertNewInstallment));
    }
    protected void grdStorePaymentInstallments_MessageEvent(object sender, string eventName, SuperPag.Framework.Message message)
    {
        this.RaiseEvent(typeof(Ev.StorePaymentFormDetail.InstallmentsEdit));
    }
}
