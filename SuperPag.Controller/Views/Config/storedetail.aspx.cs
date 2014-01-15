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
        this.RaiseEvent(typeof(Ev.StoreDetail.GoBack));
    }

    protected void grdStorePaymentForms_MessageEvent(object sender, string eventName, SuperPag.Framework.Message message)
    {
        switch (eventName)
        {
            case "ShowStorePaymentInstallments":
                this.RaiseEvent(typeof(Ev.StoreDetail.ShowStorePaymentInstallments));
                break;
            case "EditStorePaymentForm":
                this.RaiseEvent(typeof(Ev.StoreDetail.EditStorePaymentForm));
                break;         
        }

    }
    protected void lnkIncluir_Click(object sender, EventArgs e)
    {
        this.RaiseEvent(typeof(Ev.StoreDetail.StorePaymentFormInsert));
    }
    protected void lnkEdit_Click(object sender, EventArgs e)
    {
        this.RaiseEvent(typeof(Ev.StoreDetail.StoreEdit));

    }
}
