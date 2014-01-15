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
using SuperPag;
using SuperPag.Data.Messages;
using SuperPag.Data;
using System.Text;

public partial class Controls_bradesco : ControlBase
{
    protected void Page_Load(object sender, EventArgs e)
    {

    }
    
    public override void ShowControl()
    {
        DPaymentAttemptBradesco paymentAttemptBradesco = DataFactory.PaymentAttemptBradesco().Locate(this.PaymentAttemptId);
        lblRefTran.Text = paymentAttemptBradesco.agentOrderReference.ToString();
        
        if (!String.IsNullOrEmpty(paymentAttemptBradesco.assinatura))
        {
            StringBuilder assinaturaTable = new StringBuilder();
            assinaturaTable.Append("<table style='border: 0px solid'><tr>");
            int position = 0;
            int column = 0;
            do
            {
                assinaturaTable.Append("<td class='datatxt2' style='border: 1px solid'>");
                assinaturaTable.Append(paymentAttemptBradesco.assinatura.Substring(position, 4));
                assinaturaTable.Append("</td>");
                position = position + 4;
                column++;

                if (column == 16)
                {
                    column = 0;
                    assinaturaTable.Append("</tr><tr>");
                }

            }
            while (position < paymentAttemptBradesco.assinatura.Length);
            assinaturaTable.Append("</tr></table>");

            lblMessage.Text = assinaturaTable.ToString();
        }
    }
}
