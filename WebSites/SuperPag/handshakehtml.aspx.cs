using System;
using System.Threading;
using System.Xml;
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
using SuperPag.Data;
using SuperPag.Data.Messages;
using SuperPag.Data.Interfaces;

public partial class handshakehtml : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        Response.AddHeader("p3p", "CP=\"IDC DSP COR ADM DEVi TAIi PSA PSD IVAi IVDi CONi HIS OUR IND CNT\"");

        string sessionCode = Request["36948FFEF212F5E4"];
        string storeKey = Request["5DED746B8F924F2E"];
        string storeReferenceOrder = Request["91D4C3128BF7DA7F"];
        string receiveTransResponse = Request["STS_RECEIVE_TRANS"];
        string keyValidation = Request["VALIDA_KEY"];
        string paymentAttemptId = Request["COD_CONTROLE"];
        string parcela = Request["NUM_PARCELA"];



        SuperPag.Handshake.Html.Handshake htmlHandshakeHelper = new SuperPag.Handshake.Html.Handshake();

        //se STS_RECEIVE_TRANS=OK entao devemos apenas setar o status do pedido
        if (receiveTransResponse != null && receiveTransResponse.ToUpper() == "OK" && !String.IsNullOrEmpty(paymentAttemptId))
        {
            //checar se e de pagamento ou de finalizacao
            if (String.IsNullOrEmpty(parcela))
                //finalizacao
                htmlHandshakeHelper.ReceiveFinalization(paymentAttemptId);
            else
                //pagamento
                htmlHandshakeHelper.ReceivePayment(paymentAttemptId, parcela);
        }
        else
        {
            //se nao recebeu o codigo de sessao, passo 1
            //o passo executa um redirect para o servidor do lojista
            if ((sessionCode == null || sessionCode == string.Empty) && (keyValidation == null || keyValidation != "1"))
            {
                htmlHandshakeHelper.Step1(storeKey, storeReferenceOrder);
            }
            else
            {
                if (keyValidation != null && keyValidation == "1")
                {
                    int storeId;
                    Guid handshakeSessionId = htmlHandshakeHelper.Step1Subpart(storeKey, storeReferenceOrder, out storeId);
                    sessionCode = handshakeSessionId.ToString();
                }

                //o passo obtem os dados do pedido, monta o order e redireciona para
                //a pagina de obter dados do cliente
                htmlHandshakeHelper.Step2(sessionCode, storeReferenceOrder);
            }
        }
    }
    
    protected override void Render(HtmlTextWriter writer)
    {
    }
}
