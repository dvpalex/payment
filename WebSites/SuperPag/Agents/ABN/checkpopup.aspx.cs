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
using SuperPag.Data;
using SuperPag;
using SuperPag.Helper;

public partial class Agents_ABN_checkpopup : System.Web.UI.Page
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

    protected void Page_Init(object sender, EventArgs e)
    {
        ((System.Web.UI.HtmlControls.HtmlGenericControl)this.Master.FindControl("thebody")).Attributes.Add("onmousemove", "javscript: focusPopUpClick();");
        ((System.Web.UI.HtmlControls.HtmlGenericControl)this.Master.FindControl("thebody")).Attributes.Add("onload", "CheckPopUp();");
        ((System.Web.UI.HtmlControls.HtmlGenericControl)this.Master.FindControl("thebody")).Attributes.Add("onfocus", "focusPopUp();");
    }
    
    protected void Page_Load(object sender, EventArgs e)
    {
        DStore dStore = GenericHelper.CheckSessionStore(Context);
        FillTableTop(dStore.storeId);
    }
}