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


public partial class Views_storepaymentinstallmentedit : SuperPag.Framework.Web.WebControls.MessagePage 
{
    MStorePaymentInstallment mStorePaymentInstallment;

    protected void Page_Load(object sender, EventArgs e)
    {        
        lblNomeUsuario.Text = User.Identity.Name;

        mStorePaymentInstallment = (MStorePaymentInstallment)this.GetMessage(typeof(MStorePaymentInstallment));
        txtInstallmentNumber.Enabled = mStorePaymentInstallment.IsNew;
    }

    protected void btnSave_Click(object sender, EventArgs e)
    {
        if (mStorePaymentInstallment.IsNew &&
            (StorePaymentInstallment.Locate(mStorePaymentInstallment.StoreId, mStorePaymentInstallment.PaymentFormId,
                mStorePaymentInstallment.InstallmentNumber) != null))
        {
            RegisterStartupScript("alerta", "<script>alert('Parcela já em uso');</script>");
            return;
        }

        this.RaiseEvent(typeof(Ev.StorePaymentInstallmentsEdit.Save));
    }

    protected void btnCancel_Click(object sender, EventArgs e)
    {
        this.RaiseEvent(typeof(Ev.StorePaymentInstallmentsEdit.Cancel));
    }

    protected void btnDelete_Click(object sender, EventArgs e)
    {
        this.RaiseEvent(typeof(Ev.StorePaymentInstallmentsEdit.Delete));
    }
}
