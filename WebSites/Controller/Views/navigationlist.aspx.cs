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


public partial class Views_navigationlist : SuperPag.Framework.Web.WebControls.MessagePage 
{
    protected void Page_Load(object sender, EventArgs e)
    {        
        lblNomeUsuario.Text = User.Identity.Name;

    }

    protected void grdTransaction_MessageEvent(object sender, string eventName, SuperPag.Framework.Message message)
    {
        this.RaiseEvent(typeof(Ev.TransactionList.SelectTransaction));
    }
    protected void btnGoBack_Click(object sender, EventArgs e)
    {
        this.RaiseEvent(typeof(Ev.TransactionList.GoBack));
    }
    protected void grdTransaction_SortCommand(object source, DataGridSortCommandEventArgs e)
    {
        Hashtable h = new Hashtable();
        h.Add("IndexField", e.SortExpression);
        this.RaiseEvent(typeof(Ev.TransactionList.SortList), h);    
    }

}
