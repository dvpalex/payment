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


public partial class Views_handshakeconfigurationedit : SuperPag.Framework.Web.WebControls.MessagePage 
{
    protected void Page_Load(object sender, EventArgs e)
    {        
        lblNomeUsuario.Text = User.Identity.Name;

        MHandshakeConfiguration mHandshakeConfiguration = (MHandshakeConfiguration)this.GetMessage(typeof(MHandshakeConfiguration));
        ddlTipoHandshake.Enabled = mHandshakeConfiguration.IsNew;
        txtHandshakeConfigurationId.Enabled = mHandshakeConfiguration.IsNew;
    }

    protected void btnSave_Click(object sender, EventArgs e)
    {
        this.RaiseEvent(typeof(Ev.HandshakeConfigurationEdit.Save));
    }

    protected void btnCancel_Click(object sender, EventArgs e)
    {
        this.RaiseEvent(typeof(Ev.HandshakeConfigurationEdit.Cancel));
    }
}
