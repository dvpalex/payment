using System;
using System.Data;
using System.Configuration;
using System.Collections;
using System.Collections.Generic;
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
using SuperPag.Agents.DepId.Messages;
using SuperPag.Helper;
using Ev = Controller.Lib.Views.Ev;
using SuperPag.Data.Messages;
using SuperPag.Data;

public partial class Controls_DepId : System.Web.UI.UserControl
{
    MPaymentAttempt mPaymentAttempt;
    MPaymentAttemptDepId mPaymentAttemptDepId;
    decimal valorTotalConfirmado = Decimal.Zero;

    protected void Page_Load(object sender, EventArgs e)
    {
        //TODO: Arrumar style do grid
        mPaymentAttempt = (MPaymentAttempt)((MessagePage)this.Page).GetMessage(typeof(MPaymentAttempt));
        mPaymentAttemptDepId = PaymentAttemptDepId.Locate(mPaymentAttempt.PaymentAttemptId);
        if (!IsPostBack)
            Binding();
    }
    
    private void Binding()
    {
        lblNumIdentificacaoValue.Text = mPaymentAttemptDepId.IdNumber.ToString();
        lblStatusName.Text = mPaymentAttemptDepId.PaymentStatusName;
        PaymentAttemptDepIdReturn paymentAttemptDepIdReturn = new PaymentAttemptDepIdReturn();
        grdDeposits.DataSource = paymentAttemptDepIdReturn.ListDeposits(mPaymentAttemptDepId);
        tblDepReturns.Visible = (grdDeposits.DataSource == null) ? false : true;
        grdDeposits.DataBind();
    }

    protected void grdDeposits_SelectedIndexChanged(Object sender, EventArgs e)
    {
        // ao clicar no button confirmar atualizar o status do cheque no BD
        DPaymentAttemptDepIdReturnChk dPaymentAttemptDepIdReturnChk = DataFactory.PaymentAttemptDepIdReturnChk().Locate(Convert.ToInt32(grdDeposits.Rows[grdDeposits.SelectedIndex].Cells[0].Text));
        dPaymentAttemptDepIdReturnChk.status = (int)MPaymentAttemptDepIdReturnChk.DepositStatusEnum.Confirmado;
        DataFactory.PaymentAttemptDepIdReturnChk().Update(dPaymentAttemptDepIdReturnChk);

        // atualizo o status geral da tentativa
        mPaymentAttemptDepId.PaymentStatus = PaymentAttemptDepId.GetStatus(mPaymentAttemptDepId.PaymentAttemptId);
        PaymentAttemptDepId.Update(mPaymentAttemptDepId);
        
        

        //TODO: Revisar
        if (mPaymentAttemptDepId.PaymentStatus == (int)SuperPag.DepIdStatusEnum.PaymentValueOk
            || mPaymentAttemptDepId.PaymentStatus == (int)SuperPag.DepIdStatusEnum.BiggerPaymentValue)
        {
            // salva o status da attempt como paga
            mPaymentAttempt.Status = MPaymentAttempt.PaymentAttemptStatus.Paid;
            mPaymentAttempt.LastUpdate = DateTime.Now;
            PaymentAttempt.Update(mPaymentAttempt);

            // salva o status da order como paga
            DOrder _dOrder = DataFactory.Order().Locate(mPaymentAttempt.Order.OrderId);
            SuperPag.Helper.GenericHelper.UpdateOrderStatusByAttemptStatus(_dOrder, (int)mPaymentAttempt.Status);

            // envia POST de Pagamento
            SuperPag.Handshake.Helper.SendPaymentPost(mPaymentAttempt.PaymentAttemptId);
        }


        Binding();
    }
    protected void grdDeposits_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        //atualizar o visible do button de liberação de acordo com o status dos cheques
        if (e.Row.DataItem != null)
        {
            if (((MDeposit)e.Row.DataItem).Status.ToUpper().Trim() == "Confirmado".ToUpper().Trim())
            {
                valorTotalConfirmado += ((MDeposit)e.Row.DataItem).Valor;
                e.Row.Cells[9].Controls[0].Visible = false;
            }
            else
                e.Row.Cells[9].Controls[0].Visible = true;
        }
    }

    protected void grdDeposits_DataBound(object sender, EventArgs e)
    {
        lblValorTotalConfirmado.Text = valorTotalConfirmado.ToString("C");
    }

}
