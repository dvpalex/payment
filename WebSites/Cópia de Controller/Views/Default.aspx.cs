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
using Controller.Lib.Util;


public partial class Views_Default : SuperPag.Framework.Web.WebControls.MessagePage 
{
    protected void Page_Load(object sender, EventArgs e)
    {        
        lblNomeUsuario.Text = User.Identity.Name;

        MCPaymentSummary mcPaymentSummary = (MCPaymentSummary)this.GetMessageArray(typeof(MCPaymentSummary));

        grdSumario.DataSource = MontaTabela(mcPaymentSummary);
        grdSumario.DataBind();
    }

    private DataTable MontaTabela(MCPaymentSummary mcPaymentSummary)
    {
        DataTable oTable = new DataTable();
        DataRow oRow = null;
        oTable.Columns.Add("ProductName", typeof(string));
        oTable.Columns.Add("Qtde_Ok", typeof(int));
        oTable.Columns.Add("Valor_Ok", typeof(decimal));
        oTable.Columns.Add("Qtde_NaoOK", typeof(int));
        oTable.Columns.Add("Valor_NaoOK", typeof(decimal));
        oTable.Columns.Add("Qtde_PayPend", typeof(int));
        oTable.Columns.Add("Valor_PayPend", typeof(decimal));
        oTable.Columns.Add("Qtde_Pend", typeof(int));
        oTable.Columns.Add("Valor_Pend", typeof(decimal));
        oTable.Columns.Add("Qtde_Cancel", typeof(int));
        oTable.Columns.Add("Valor_Cancel", typeof(decimal));

        string paymentFormName = "";
        for (int i = 0; i < mcPaymentSummary.Count; i++)
        {
            MPaymentSummary mPaymentSummary = (MPaymentSummary)mcPaymentSummary[i];
            if (mPaymentSummary.Name != paymentFormName)
            {
                if (oRow != null)
                    oTable.Rows.Add(oRow);
                paymentFormName = mPaymentSummary.Name;
                oRow = oTable.NewRow();
                oRow["ProductName"] = paymentFormName;
                oRow["Qtde_Ok"] = 0;
                oRow["Valor_Ok"] = 0;
                oRow["Qtde_NaoOK"] = 0;
                oRow["Valor_NaoOK"] = 0;
                oRow["Qtde_PayPend"] = 0;
                oRow["Valor_PayPend"] = 0;
                oRow["Qtde_Pend"] = 0;
                oRow["Valor_Pend"] = 0;
                oRow["Qtde_Cancel"] = 0;
                oRow["Valor_Cancel"] = 0;
            }
            switch (mPaymentSummary.Status)
            {
                case 1:
                    oRow["Qtde_Pend"] = mPaymentSummary.Qtde;
                    oRow["Valor_Pend"] = mPaymentSummary.Total.Equals(decimal.MinValue) ? 0m : mPaymentSummary.Total;
                    break;
                case 2:
                    oRow["Qtde_OK"] = mPaymentSummary.Qtde;
                    oRow["Valor_OK"] = mPaymentSummary.Total.Equals(decimal.MinValue) ? 0m : mPaymentSummary.Total;
                    break;
                case 3:
                    oRow["Qtde_NaoOK"] = mPaymentSummary.Qtde;
                    oRow["Valor_NaoOK"] = mPaymentSummary.Total.Equals(decimal.MinValue) ? 0m : mPaymentSummary.Total;
                    break;
                case 4:
                    oRow["Qtde_Cancel"] = mPaymentSummary.Qtde;
                    oRow["Valor_Cancel"] = mPaymentSummary.Total.Equals(decimal.MinValue) ? 0m : mPaymentSummary.Total;
                    break;
                case 5:
                    oRow["Qtde_PayPend"] = mPaymentSummary.Qtde;
                    oRow["Valor_PayPend"] = mPaymentSummary.Total.Equals(decimal.MinValue) ? 0m : mPaymentSummary.Total;
                    break;
            }

            if (i == mcPaymentSummary.Count - 1)
                oTable.Rows.Add(oRow);
        }

        return oTable;
    }

    protected void grdSumario_ItemDataBound(object sender, DataGridItemEventArgs e)
    {
        switch ( e.Item.ItemType )
			{
				case ListItemType.Header:
					e.Item.Cells.RemoveAt(1);
					e.Item.Cells.RemoveAt(2);
					e.Item.Cells.RemoveAt(3);
					e.Item.Cells.RemoveAt(4);
					e.Item.Cells.RemoveAt(5);
					
					e.Item.Cells [ 1 ].Text = "Autorizadas";
					e.Item.Cells [ 1 ].Style.Add("text-align","center");
					e.Item.Cells [ 1 ].Style.Add("color","darkslategray");
					e.Item.Cells [ 1 ].ColumnSpan = 2;

					e.Item.Cells [ 2 ].Text = "Não autorizadas";
					e.Item.Cells [ 2 ].Style.Add("text-align","center");
					e.Item.Cells [ 2 ].Style.Add("color","darkred");
					e.Item.Cells [ 2 ].ColumnSpan = 2;

					e.Item.Cells[3].Text = "Pendentes";
					e.Item.Cells[3].Style.Add("text-align","center");
                    e.Item.Cells[3].Style.Add("color", "darkslategray");
                    e.Item.Cells[3].ColumnSpan = 2;

                    e.Item.Cells[4].Text = "Canceladas";
					e.Item.Cells[4].Style.Add("text-align","center");
                    e.Item.Cells[4].Style.Add("color", "darkred");
                    e.Item.Cells[4].ColumnSpan = 2;

                    e.Item.Cells[5].Text = "Não concluídas";
					e.Item.Cells [ 5 ].Style.Add("text-align","right");
					e.Item.Cells [ 5 ].Style.Add("background-color","lightblue");
					e.Item.Cells [ 5 ].Style.Add("border-left","lightslategray 4px solid");
					e.Item.Cells [ 5 ].Style.Add("color","navy");
					e.Item.Cells [ 5 ].ColumnSpan = 2;

					break;

				case ListItemType.Footer:
					e.Item.Cells [ 0 ].Text = "Total";
					e.Item.Cells [ 0 ].Style.Add("color","navy");
					e.Item.Cells [ 0 ].Style.Add("font-weight","bold");

                    //PreencheTotal ( _summary.totalStatus [ AuthStatus.OK ] , 1 , e);
                    //PreencheTotal ( _summary.totalStatus [ AuthStatus.Cancel ] , 3, e );
                    //PreencheTotal ( _summary.totalStatus [ AuthStatus.NotOK ] , 5 , e);
                    //PreencheTotal ( _summary.totalStatus [ AuthStatus.Error ] , 7 , e);

//					linkDetail1.Text =_summary.totalQuantity.ToString();;
					e.Item.Cells [ 9 ].CssClass = "footerTotal";
					//((LinkButton)e.Item.Cells [ 9 ].Controls [ 1 ]).Text = _summary.totalQuantity.ToString();
				

//					e.Item.Cells [ 10 ].Text = _summary.totalAmount.ToString("N2");
					e.Item.Cells [ 10 ].CssClass = "footerAmountTotal";

					break;

			}
    }
    protected void btnUpdate_Click(object sender, EventArgs e)
    {
        this.RaiseEvent(typeof(Ev.Home.Update));
    }

    protected void grdSumario_ItemCommand(object source, DataGridCommandEventArgs e)
    {
    
        string cardTypeName = e.Item.Cells[0].Text;

        MPaymentAttempt.PaymentAttemptStatus status = MPaymentAttempt.PaymentAttemptStatus.Canceled;
        
        int coluna = int.Parse(e.CommandArgument.ToString());
        switch (coluna)
        {
            case 1:
                status = MPaymentAttempt.PaymentAttemptStatus.Paid;
                break;
            case 3:
                status = MPaymentAttempt.PaymentAttemptStatus.NotPaid;
                break;
            case 5:
                status = MPaymentAttempt.PaymentAttemptStatus.PayPending;
                break;
            case 7:
                status = MPaymentAttempt.PaymentAttemptStatus.Canceled;
                break;
            case 9:
                status = MPaymentAttempt.PaymentAttemptStatus.Pending;
                break;
        }
        Hashtable h = new Hashtable();
        h.Add("Status", status);
        h.Add("Name", cardTypeName);

        this.RaiseEvent(typeof(Ev.Home.List), h);
    }
}
