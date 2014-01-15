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

public partial class Agents_DepId_depId : ControlBase
{

    protected void Page_Load(object sender, EventArgs e)
    {
        
    }

    public override void ShowControl()
    {
        ControlInfoDepId ctlInfoDepId = (ControlInfoDepId)this.ControlInfo;

        lblBanco.Text = ctlInfoDepId.BankNumber.ToString();
        lblAgencia.Text = ctlInfoDepId.AgencyNumber.ToString() + "-" + ctlInfoDepId.AgencyDigit.ToString();
        lblConta.Text = ctlInfoDepId.AccountNumber.ToString();

        rptDepIdInfo.DataSource = ctlInfoDepId.InfoDepIdList;
        rptDepIdInfo.DataBind();
    }
}
