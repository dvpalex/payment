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
using SuperPag;
using System.Text;


public partial class Views_orderdetail : SuperPag.Framework.Web.WebControls.MessagePage 
{
    MOrder mOrder = null;

    protected void Page_Load(object sender, EventArgs e)
    {        
        lblNomeUsuario.Text = User.Identity.Name;

        mOrder = (MOrder)this.GetMessage(typeof(MOrder));

        if (mOrder.Consumer != null)
        {
            pnlPessoaFisica.Visible = mOrder.Consumer.CNPJ == null || mOrder.Consumer.CNPJ.Equals(String.Empty);
            pnlPessoaJuridica.Visible = !pnlPessoaFisica.Visible;

            if ((MConsumerAddress)this.GetMessage("1") != null)
                pnlBillingAddress.Visible = true;
            if ((MConsumerAddress)this.GetMessage("2") != null)
                pnlDeliveryAddress.Visible = true;
        }
        else
        {
            pnlConsumer.Visible = false;
        }
        
        pnlRecurrence.Visible = mOrder.Recurrence != null;
        if(mOrder.PaymentAttempts != null)
            pnlAttempts.Visible = mOrder.PaymentAttempts.Count > 0;
        if(mOrder.SchedulePayments != null)
            pnlSchedules.Visible = mOrder.SchedulePayments.Count > 0;

        if (mOrder.Status == MOrder.OrderStatus.Cancelled)
        {
            Label48.Visible = true;
            MsgLabel47.Visible = true;
            Label49.Visible = true;
            MsgLabel48.Visible = true;
            Label59.Visible = true;
            TextBox1.Visible = true;
            TextBox1.Enabled = false;
        }
        else if (mOrder.Status == MOrder.OrderStatus.Approved)
        {
            Label48.Visible = true;
            MsgLabel47.Visible = true;
            Label49.Visible = true;
            MsgLabel48.Visible = true;
        }
        else
        {
            Label58.Visible = true;
            Label59.Visible = true;
            Msgdropdownlist1.Visible = true;
            btnAlterar.Visible = true;
            TextBox1.Visible = true;

            Motivo.Style.Add(HtmlTextWriterStyle.Display, "none");

            Msgdropdownlist1.Attributes.Add("onchange", "javascript: Mostrar();");

            StringBuilder sb = new StringBuilder();
            sb.Append("<script>function Mostrar(){ var combo = document.getElementById('" + Msgdropdownlist1.ClientID + "');");
            sb.Append("if(combo.options[combo.selectedIndex].value == " + (int)OrderStatus.Cancelled + ") { var td = document.getElementById('" + Motivo.ClientID + "'); td.style.display = 'block'; }");
            sb.Append("else { var td = document.getElementById('" + Motivo.ClientID + "'); var ta = document.getElementById('" + TextBox1.ClientID + "');");
            sb.Append("td.style.display = 'none'; ta.value = ''; }; }</script>");
            ClientScript.RegisterClientScriptBlock(this.GetType(), "mostrar", sb.ToString());
        }
    }

    protected void btnGoBack_Click(object sender, EventArgs e)
    {
        this.RaiseEvent(typeof(Ev.OrderDetail.GoBack));
    }

    protected void btnAlterar_Click(object sender, EventArgs e)
    {
        this.RaiseEvent(typeof(Ev.OrderDetail.Alterar));
    }
    protected void msgPaymentAttemp_MessageEvent(object sender, string eventName, SuperPag.Framework.Message message)
    {
        this.RaiseEvent(typeof(Ev.OrderDetail.ShowOrderTransactionDetail));
    }
}
