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
using SuperPag.Data.Messages;
using SuperPag.Helper;
using SuperPag.Data;

public partial class Agents_PaymentClientVirtual_Call2Party : System.Web.UI.Page
{
    private void FillTableTop(int storeId)
    {
        DSPLegacyStore dSPLegay = DataFactory.SPLegacyStore().Locate(storeId);
        if (Ensure.IsNotNull(dSPLegay) && Ensure.IsNotNull(dSPLegay.ucTableTop))
        {
            plhTableTop.Controls.Add(Page.LoadControl(dSPLegay.ucTableTop));
        }
        else
        {
            plhTableTop.Visible = false;
        }
    }
    
    protected void Page_Load(object sender, EventArgs e)
    {
        Ensure.IsNotNullPage(Session["PaymentAttemptId"], "Sess�o inv�lida iniciando uma transa��o Amex");
        
        DStore dStore = GenericHelper.CheckSessionStore(Context);
        FillTableTop(dStore.storeId);
    }
}
