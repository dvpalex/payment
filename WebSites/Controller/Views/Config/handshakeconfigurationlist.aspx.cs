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


public partial class Views_handshakeconfigurationlist : SuperPag.Framework.Web.WebControls.MessagePage 
{
    protected void Page_Load(object sender, EventArgs e)
    {        
        lblNomeUsuario.Text = User.Identity.Name;

    }
    protected void grdHandshakeConfiguration_MessageEvent(object sender, string eventName, SuperPag.Framework.Message message)
    {
        this.RaiseEvent(typeof(Ev.HandshakeConfigurationList.HandshakeConfigurationEdit));
    }
    protected void lnkIncluir_Click(object sender, EventArgs e)
    {
        this.RaiseEvent(typeof(Ev.HandshakeConfigurationList.HandshakeConfigurationInsert));
    }
}
