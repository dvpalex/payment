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
using System.Collections.Generic;
using Controller.Lib.Util;

public partial class Views_navigationfilter : SuperPag.Framework.Web.WebControls.MessagePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        lblNomeUsuario.Text = User.Identity.Name;

    }

    protected void btnSearch_Click(object sender, EventArgs e)
    {
        RaiseEvent(typeof(Controller.Lib.Views.Ev.NavigationFilter.NavigationFilter));
    }
}
