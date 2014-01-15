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

public partial class Views_extratofilter : SuperPag.Framework.Web.WebControls.MessagePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        lblNomeUsuario.Text = User.Identity.Name;


        if (!Page.IsPostBack)
        {
            DStorePaymentForm[] arr = DataFactory.StorePaymentForm().List(ControllerContext.StoreId);

            foreach (DStorePaymentForm d in arr)
            {
                DPaymentForm dForm = DataFactory.PaymentForm().Locate(d.paymentFormId);
                CheckBoxList1.Items.Add(new ListItem(dForm.name, dForm.paymentFormId.ToString()));
            }
        }
    }

    protected void btnSearch_Click(object sender, EventArgs e)
    {
        if ( CheckBoxList1.SelectedValue == null || CheckBoxList1.SelectedValue == string.Empty )
        {
            Label1.Visible = true;
            return;
        }

        DateTime endDate = DateTime.Parse(txtEndDate.Text);

        Hashtable parameters = new Hashtable();
        parameters.Add("startDate", DateTime.Parse(txtStartDate.Text));
        parameters.Add("endDate", DateTime.Parse(txtEndDate.Text + " 23:59:59"));
        List<int> temp = new List<int>();
        foreach(ListItem item in CheckBoxList1.Items )
        {
            if ( item.Selected )
            {
                temp.Add(Int32.Parse(item.Value));
            }
        }
        parameters.Add("payementFormId", temp.ToArray());
        if ( CommandStack.LastCommandIs("ShowExtratoFilterMovement") )
        {
            RaiseEvent(typeof(Controller.Lib.Views.Ev.ExtratoFilter.FilterMovement), parameters);
        }
        else  if ( CommandStack.LastCommandIs("ShowExtratoFilterFinancial") )
        {
            RaiseEvent(typeof(Controller.Lib.Views.Ev.ExtratoFilter.FilterFinancial), parameters);
        }
        else if (CommandStack.LastCommandIs("ShowExtratoFilterFinancial2"))
        {
            RaiseEvent(typeof(Controller.Lib.Views.Ev.ExtratoFilter.FilterFinancial2), parameters);
        }
    }
}
