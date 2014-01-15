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

public partial class Agents_BoletoBB_boleto : ControlBase
{

    protected void Page_Load(object sender, EventArgs e)
    {
        
    }

    public override void ShowControl()
    {
        rptBoletoInfo.DataSource = ((ControlInfoBoleto)this.ControlInfo).InfoBoletoList;
        rptBoletoInfo.DataBind();
    }
}
