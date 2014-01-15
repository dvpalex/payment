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
using System.Collections.Specialized;
using System.Text;
using SuperPag.Data.Messages;
using SuperPag.Data;
using SuperPag.Helper;
using SuperPag;
using SuperPag.Handshake.Html;
using K = SuperPag.Agents.Komerci;

public partial class Agents_Komerci_retorno : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        Ensure.IsNotNullPage(Request["NUMPEDIDO"], "Post inválido tentando recuperar o NUMPEDIDO de uma transação Master/Diners");

        string DATA = Request["DATA"];
        string NUMPEDIDO = Request["NUMPEDIDO"];
        string NR_CARTAO = Request["NR_CARTAO"];
        string ORIGEM_BIN = Request["ORIGEM_BIN"];
        string NUMAUTOR = Request["NUMAUTOR"];
        string NUMCV = Request["NUMCV"];
        string NUMAUTENT = Request["NUMAUTENT"];
        string NUMSQN = Request["NUMSQN"];
        string CODRET = Request["CODRET"];
        string MSGRET = Request["MSGRET"];
        string RESPAVS = Request["RESPAVS"];
        string MSGAVS = Request["MSGAVS"];

        DPaymentAttemptKomerci attemptKomerci = DataFactory.PaymentAttemptKomerci().Locate(GenericHelper.ParseInt(NUMPEDIDO));
        Ensure.IsNotNullPage(attemptKomerci, "Número de referência {0} não encontrado", NUMPEDIDO);
        DPaymentAttempt attempt = DataFactory.PaymentAttempt().Locate(attemptKomerci.paymentAttemptId);
        DPaymentAgentSetupKomerci agentsetup = DataFactory.PaymentAgentSetupKomerci().Locate(attempt.paymentAgentSetupId);
        DOrder order = DataFactory.Order().Locate(attempt.orderId);

        // Recupera Session perdida
        Session["PaymentAttemptId"] = attempt.paymentAttemptId;

        attemptKomerci.data = DATA;
        attemptKomerci.nr_cartao = NR_CARTAO;
        attemptKomerci.origem_bin = ORIGEM_BIN;
        attemptKomerci.numautor = NUMAUTOR;
        attemptKomerci.numcv = NUMCV;
        attemptKomerci.numautent = NUMAUTENT;
        attemptKomerci.numsqn = NUMSQN;
        attemptKomerci.codret = CODRET;
        attemptKomerci.msgret = HttpUtility.UrlDecode(MSGRET, Encoding.GetEncoding("iso-8859-1"));
        attemptKomerci.respavs = RESPAVS;
        attemptKomerci.msgavs = HttpUtility.UrlDecode(MSGAVS, Encoding.GetEncoding("iso-8859-1"));

        if (CODRET == null || CODRET == "0" || CODRET == "49")
        {
            if (!agentsetup.checkAVS || (agentsetup.checkAVS && K.Komerci.ValidaAVS(RESPAVS, agentsetup.acceptedAVSReturn)) ||
                  (agentsetup.checkAVS && K.Komerci.BinExcecao(attemptKomerci.nr_cartao, agentsetup.AVSExceptionBINs)))
            {
                ServerHttpHtmlRequisition post = new ServerHttpHtmlRequisition();
                post.Method = "GET";
                post.Url = agentsetup.urlKomerciConfirm;
                post.Parameters.Add("DATA", DATA);
                post.Parameters.Add("TRANSACAO", "203");
                post.Parameters.Add("TRANSORIG", attemptKomerci.transacao);
                post.Parameters.Add("PARCELAS", (order.installmentQuantity == 1 ? "00" : order.installmentQuantity.ToString().PadLeft(2, '0')));
                post.Parameters.Add("FILIACAO", agentsetup.businessNumber.ToString());
                post.Parameters.Add("DISTRIBUIDOR", "");
                post.Parameters.Add("TOTAL", GenericHelper.FormatCurrency(attempt.price));
                post.Parameters.Add("NUMPEDIDO", attemptKomerci.agentOrderReference.ToString());
                post.Parameters.Add("NUMAUTOR", NUMAUTOR);
                post.Parameters.Add("NUMCV", NUMCV);
                post.Parameters.Add("NUMSQN", NUMSQN);

                if(agentsetup.checkAVS && K.Komerci.BinExcecao(attemptKomerci.nr_cartao, agentsetup.AVSExceptionBINs))
                    attemptKomerci.avs = "E";

                attemptKomerci.komerciStatus = (byte)PaymentAttemptKomerciStatus.Capture;
                DataFactory.PaymentAttemptKomerci().Update(attemptKomerci);

                if (post.Send())
                {
                    string[] keys = post.Response.Split(new char[] { '&', '=' });

                    if (keys != null && keys.Length == 4 && keys[0] == "CODRET" && keys[2] == "MSGRET")
                    {
                        if (keys[1] == "0" || keys[1] == "1")
                        {
                            attemptKomerci.komerciStatus = (byte)PaymentAttemptKomerciStatus.End;
                            attemptKomerci.codret = keys[1];
                            attemptKomerci.msgret = HttpUtility.UrlDecode(keys[3], Encoding.GetEncoding("iso-8859-1"));
                            attempt.status = (int)PaymentAttemptStatus.Paid;
                            attempt.lastUpdate = DateTime.Now;
                            attempt.returnMessage = attemptKomerci.msgret;
                            attemptKomerci.TruncateStringFields();
                            attempt.TruncateStringFields();
                            DataFactory.PaymentAttempt().Update(attempt);
                            DataFactory.PaymentAttemptKomerci().Update(attemptKomerci);
                            GenericHelper.UpdateOrderStatusByAttemptStatus(order, attempt.status);

                            GenericHelper.CloseWindow();

                            return;
                        }

                        attemptKomerci.codret = keys[1];
                        attemptKomerci.msgret = HttpUtility.UrlDecode(keys[3], Encoding.GetEncoding("iso-8859-1"));
                        attempt.returnMessage = attemptKomerci.msgret;
                    }
                    else
                    {
                        attemptKomerci.codret = "-1";
                        attemptKomerci.msgret = "retorno não reconhecido: " + post.Response;
                        attempt.returnMessage = "Retorno de autorização não reconhecido";
                    }
                }
                else
                {
                    attemptKomerci.codret = "-1";
                    attemptKomerci.msgret = post.Response;
                    attempt.returnMessage = "Problemas na transferência das informações";
                }
            }
            else
            {
                attempt.returnMessage = "O AVS não foi validado. " + attemptKomerci.msgavs;
            }
        }

        attemptKomerci.komerciStatus = (byte)PaymentAttemptKomerciStatus.End;
        attempt.status = (int)PaymentAttemptStatus.NotPaid;
        attempt.lastUpdate = DateTime.Now;
        
        attemptKomerci.TruncateStringFields();
        attempt.TruncateStringFields();

        DataFactory.PaymentAttempt().Update(attempt);
        DataFactory.PaymentAttemptKomerci().Update(attemptKomerci);
        GenericHelper.UpdateOrderStatusByAttemptStatus(order, attempt.status);

        GenericHelper.CloseWindow();
    }
}
