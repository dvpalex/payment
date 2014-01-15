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
using SuperPag;
using SuperPag.Data;
using SuperPag.Data.Messages;
using SuperPag.Handshake;
using SuperPag.Helper;
using System.Text;
using SuperPag.Handshake.Html;
using SuperPag.Handshake.Xml;
using System.IO;


public partial class finalization : System.Web.UI.Page
{
    public string InstrucaoFinalizacao;
    int storeId;
    string urlBotao4, linkBotao4, target;

    private void FillTableTop(int storeId)
    {
        DSPLegacyStore dSPLegay = DataFactory.SPLegacyStore().Locate(storeId);
        if (Ensure.IsNotNull(dSPLegay) && Ensure.IsNotNull(dSPLegay.ucTableTop))
        {
            plhTableTop.Controls.Add(Page.LoadControl(dSPLegay.ucTableTop));
        }
        else
        {
            plhTableTop.Visible = false;
        }
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        Ensure.IsNotNullPage(Request["id"], "Post inválido exibindo o resultado de uma transação");

        GenericHelper.RefillSessionByAttempt(new Guid(Request["id"]));
        if (GenericHelper.GetOrderStatus(Context) == (int)WorkflowOrderStatus.Finished)
            GenericHelper.RedirectToErrorPage("Este pedido já está finalizado."); ;

        //preenche o table top
        DStore dStore = GenericHelper.CheckSessionStore(Context);
        FillTableTop(dStore.storeId);
        storeId = dStore.storeId;
        
        UpdateHandshakeVars();
        Guid attemptId = (Guid)Session["PaymentAttemptId"];
        DPaymentAttempt attempt = DataFactory.PaymentAttempt().Locate(attemptId);
        DOrder dOrder = DataFactory.Order().Locate(attempt.orderId);
        DOrderInstallment dOrderInstallment = DataFactory.OrderInstallment().Locate(attempt.orderId, 1);
        DConsumer dConsumer = DataFactory.Consumer().Locate(dOrder.consumerId);
        DPaymentForm dPaymentForm = DataFactory.PaymentForm().Locate(attempt.paymentFormId);

        //Seto o status do pedido
        GenericHelper.SetOrderStatus(HttpContext.Current, WorkflowOrderStatus.Finished, attempt.paymentFormId + "," + dOrder.installmentQuantity + "," + dPaymentForm.paymentAgentId);

        //preenche a instrucao de finalizacao
        if (Session["htmlHandshake"] != null)
            InstrucaoFinalizacao = GenericHelper.GetSingleNodeString((string)Session["htmlHandshake"], "//instrucao_finalizacao");
        else
        {
            InstrucaoFinalizacao = GenericHelper.GetSingleNodeString((string)Session["xmlHandshake"], "/pedido/parametros_opcionais/instrucao_finalizacao").Replace("@numero_pedido", dOrder.storeReferenceOrder).Replace("@data_pedido", dOrder.lastUpdateDate.ToString("dd/MM/yyyy"));
        }

        if (String.IsNullOrEmpty(InstrucaoFinalizacao))
            InstrucaoFinalizacao = "O seu pedido foi recebido.";

        lblPaymentForm.Text = dPaymentForm.name;
        lblPedido.Text = dOrder.storeReferenceOrder;
        lblClientName.Text = dConsumer.name.ToUpper();
        lblInstalmentValue.Text = dOrderInstallment.installmentValue.ToString("C");
        lblInstallmentQuantity.Text = dOrder.installmentQuantity.ToString();

        int[] itensTypes = new int[2];
        itensTypes[0] = (int)ItemTypes.Regular;
        itensTypes[1] = (int)ItemTypes.Discount;
        DOrderItem[] arrDOrderItem = DataFactory.OrderItem().List(dOrder.orderId, itensTypes);
        List<Item> itens = new List<Item>();
        if (arrDOrderItem != null)
        {
            for (int i = 0; i < arrDOrderItem.Length; i++)
            {
                Item item = new Item();
                item.ItemCode = arrDOrderItem[i].itemCode;
                item.ItemDescription = arrDOrderItem[i].itemDescription;
                item.ItemQuantity = arrDOrderItem[i].itemQuantity;
                item.ItemValue = arrDOrderItem[i].itemValue;
                itens.Add(item);
            }
        }

        rptItens.DataSource = itens;
        rptItens.DataBind();

        //calcular o frete total
        DOrderItem[] shippingItens = DataFactory.OrderItem().List(dOrder.orderId, (int)ItemTypes.ShippingRate);
        decimal shippingValue = 0;
        if (shippingItens != null)
        {
            foreach (DOrderItem shippingItem in shippingItens)
                shippingValue += shippingItem.itemValue;
        }
        lblSubTotal.Text = (dOrder.totalAmount - shippingValue).ToString("C");
        lblTotal.Text = dOrder.totalAmount.ToString("C");
        lblFrete.Text = shippingValue.ToString("C");

        //obtem os dados do handshake
        DHandshakeConfiguration dHandshakeConfiguration = DataFactory.HandshakeConfiguration().Locate(dStore.handshakeConfigurationId);
        Ensure.IsNotNullPage(dHandshakeConfiguration, "A loja {0} não está configurada corretamente", dStore.storeId);

        DHandshakeConfigurationXml dHandshakeConfigurationXml = new DHandshakeConfigurationXml();
        DHandshakeConfigurationHtml dHandshakeConfigurationHtml= new DHandshakeConfigurationHtml();
        //verifica se o tipo de handshake é html
        if (dHandshakeConfiguration.handshakeType == (int)HandshakeType.HtmlSPag10)
        {
            //obtem os dados do handshake de html
            dHandshakeConfigurationHtml = DataFactory.HandshakeConfigurationHtml().Locate(dHandshakeConfiguration.handshakeConfigurationId);
            Ensure.IsNotNullPage(dHandshakeConfiguration, "A loja {0} não está configurada para utilizar o handshake HTML", dStore.storeId);
        }
        else if (dHandshakeConfiguration.handshakeType == (int)HandshakeType.XmlSPag10)
        {
            //obtem os dados do handshake de html
            dHandshakeConfigurationXml = DataFactory.HandshakeConfigurationXml().Locate(dHandshakeConfiguration.handshakeConfigurationId);
            Ensure.IsNotNullPage(dHandshakeConfiguration, "A loja {0} não está configurada para utilizar o handshake XML", dStore.storeId);
        }

        //preenche botoes
        if (String.IsNullOrEmpty(linkBotao4))
        {
            if (dHandshakeConfiguration.handshakeType == (int)HandshakeType.HtmlSPag10)
            {
                if (!String.IsNullOrEmpty(dHandshakeConfigurationHtml.urlReturn))
                    linkBotao4 = string.Format(dHandshakeConfigurationHtml.urlReturn, dOrder.orderId, dOrder.storeReferenceOrder, "", "", dConsumer.email);
            }
            else if (dHandshakeConfiguration.handshakeType == (int)HandshakeType.XmlSPag10)
            {
                if (!String.IsNullOrEmpty(dHandshakeConfigurationXml.urlReturn))
                    linkBotao4 = dHandshakeConfigurationXml.urlReturn;
            }
        }

        if (String.IsNullOrEmpty(linkBotao4))
            linkBotao4 = "http://www.superpag.com.br";

        lnkEnd.NavigateUrl = linkBotao4;
        lnkEnd.Target = (String.IsNullOrEmpty(target) ? "_parent" : target);

        //lnkEnd - botao4 (continuar)
        if (!String.IsNullOrEmpty(urlBotao4))
            lnkEnd.ImageUrl = urlBotao4;
        else
        {
            string btnNextPath = "~/Store/" + storeId + "/images/lnkEnd.gif";
            if (File.Exists(Server.MapPath(btnNextPath)))
                lnkEnd.ImageUrl = btnNextPath;
        }

        //obtem dados do consumidor
        DConsumer consumer = DataFactory.Consumer().Locate(dOrder.consumerId);
        Ensure.IsNotNullPage(consumer, "Consumidor do pedido {0} inválido.", dOrder.storeReferenceOrder);

        string controlPath = "";
        switch (dPaymentForm.paymentAgentId)
        {
            case (int)PaymentAgents.Boleto:
                controlPath = "~/Controls/boleto.ascx";
                break;
            case (int)PaymentAgents.BBPag:
                controlPath = "~/Controls/bbpag.ascx";
                break;
            case (int)PaymentAgents.ItauShopLine:
                controlPath = "~/Controls/itaushopline.ascx";
                break;
            case (int)PaymentAgents.Komerci:
            case (int)PaymentAgents.KomerciInBox:
                controlPath = "~/Controls/komerci.ascx";
                break;
            case (int)PaymentAgents.VBV:
            case (int)PaymentAgents.VBVInBox:
            case (int)PaymentAgents.VBV3:
                controlPath = "~/Controls/vbv.ascx";
                break;
            case (int)PaymentAgents.DepositoIdentificado:
                controlPath = "~/Controls/depId.ascx";
                break;
            case (int)PaymentAgents.Bradesco:
                controlPath = "~/Controls/bradesco.ascx";
                break;
        }
        
        //insere controle com dados especificos dos meios de pagamentos:
        if (!string.IsNullOrEmpty(controlPath))
        {
            ControlBase controlBase = (ControlBase)LoadControl(controlPath);
            if (GenericHelper.HasInstallmentAttempts(dPaymentForm.paymentFormId))
                controlBase.ControlInfo = Session["FinalizationControlInfo"];
            controlBase.PaymentAttemptId = attempt.paymentAttemptId;
            controlBase.ShowControl();
            plhFinalization.Controls.Add((Control)controlBase);
        }

        //envia primeiro os emails de confirmacoes e o post de finalizacao
        if (dHandshakeConfiguration.handshakeType == (int)HandshakeType.HtmlSPag10)
        {
            string telaFinalizacao = "";
            string sendConsumerEmail = "";
            string textoFinalizacao = "";
            if (Session["htmlHandshake"] != null)
            {
                telaFinalizacao = GenericHelper.GetSingleNodeString((string)Session["htmlHandshake"], "//show_tela_finalizacao");
                sendConsumerEmail = GenericHelper.GetSingleNodeString((string)Session["htmlHandshake"], "//envia_email_cliente");
                textoFinalizacao = GenericHelper.GetSingleNodeString((string)Session["htmlHandshake"], "//instrucao_finalizacao");
            }
            if (string.IsNullOrEmpty(textoFinalizacao))
                textoFinalizacao = "O seu pedido foi recebido.";

            //email ao cliente/consumidor
            if((String.IsNullOrEmpty(sendConsumerEmail) && dHandshakeConfiguration.sendEmailConsumer) ||
                (!String.IsNullOrEmpty(sendConsumerEmail) && sendConsumerEmail == "1"))
                SuperPag.Handshake.Helper.SendFinalizationConsumerEmail(attemptId, System.Globalization.CultureInfo.CurrentUICulture.ToString().ToLower(), textoFinalizacao, linkBotao4);

            //email ao lojista
            if(dHandshakeConfiguration.sendEmailStoreKeeper)
                SuperPag.Handshake.Helper.SendFinalizationStoreKeeperEmail(attemptId, textoFinalizacao, linkBotao4);

            //post de finalizacao
            if (telaFinalizacao == "0")
            {
                SuperPag.Handshake.Html.FinalizationPost finalization = new SuperPag.Handshake.Html.FinalizationPost(attempt.paymentAttemptId);
                finalization.SendClient();
            }
            else
            {
                SuperPag.Handshake.Html.Handshake hand = new SuperPag.Handshake.Html.Handshake();
                hand.SendFinalizationPost(attempt.paymentAttemptId);
            }
        }
        else if (dHandshakeConfiguration.handshakeType == (int)HandshakeType.XmlSPag10)
        {
            string telaFinalizacao = "";
            string sendConsumerEmail = "";
            string textoFinalizacao = "";
            if (Session["xmlHandshake"] != null)
            {
                telaFinalizacao = GenericHelper.GetSingleNodeString((string)Session["xmlHandshake"], "//show_tela_finalizacao");
                sendConsumerEmail = GenericHelper.GetSingleNodeString((string)Session["xmlHandshake"], "//envia_email_cliente");
                textoFinalizacao = GenericHelper.GetSingleNodeString((string)Session["xmlHandshake"], "//instrucao_finalizacao");
            }
            if (string.IsNullOrEmpty(textoFinalizacao))
                textoFinalizacao = "O seu pedido foi recebido.";

            //email ao cliente
            if ((String.IsNullOrEmpty(sendConsumerEmail) && dHandshakeConfiguration.sendEmailConsumer) ||
                (!String.IsNullOrEmpty(sendConsumerEmail) && sendConsumerEmail == "1"))
                SuperPag.Handshake.Helper.SendFinalizationConsumerEmail(attemptId, System.Globalization.CultureInfo.CurrentUICulture.ToString().ToLower(), textoFinalizacao, linkBotao4);

            //email ao lojista
            if (dHandshakeConfiguration.sendEmailStoreKeeper)
                SuperPag.Handshake.Helper.SendFinalizationStoreKeeperEmail(attemptId, textoFinalizacao, linkBotao4);

            //post de finalizacao
            if (telaFinalizacao == "0")
            {
                SuperPag.Handshake.Xml.FinalizationPost finalization = new SuperPag.Handshake.Xml.FinalizationPost(attempt.paymentAttemptId);
                finalization.ShowFinalization = false;
                finalization.Send();
            }
            else
            {
                SuperPag.Handshake.Xml.Handshake hand = new SuperPag.Handshake.Xml.Handshake();
                hand.SendFinalizationPost(attempt.paymentAttemptId);
            }
        }

        //cria um registro na tabela de post de pagamento caso a attempt tenha sido aprovada
        if (attempt.status == (int)PaymentAttemptStatus.Paid && dHandshakeConfiguration.autoPaymentConfirm)
        {
            if(dHandshakeConfiguration.handshakeType == (int)HandshakeType.HtmlSPag10)
            {
                SuperPag.Handshake.Html.Handshake hand = new SuperPag.Handshake.Html.Handshake();
                hand.SendPaymentPost(attempt.paymentAttemptId);
            }
            else if (dHandshakeConfiguration.handshakeType == (int)HandshakeType.XmlSPag10)
            {
                SuperPag.Handshake.Xml.Handshake hand = new SuperPag.Handshake.Xml.Handshake();
                hand.SendPaymentPost(attempt.paymentAttemptId);
            }
        }
    }

    public void UpdateHandshakeVars()
    {
        //Seto as variaveis a serem usadas do handshake
        if (Session["htmlHandshake"] != null)
        {
            urlBotao4 = HttpUtility.HtmlDecode(GenericHelper.GetSingleNodeString((string)Session["htmlHandshake"], "//urlbotao4"));
            linkBotao4 = HttpUtility.HtmlDecode(GenericHelper.GetSingleNodeString((string)Session["htmlHandshake"], "//urlretornoloja"));
            target = HttpUtility.HtmlDecode(GenericHelper.GetSingleNodeString((string)Session["htmlHandshake"], "//target"));
        }
        else
        {
            urlBotao4 = HttpUtility.HtmlDecode(GenericHelper.GetSingleNodeString((string)Session["xmlHandshake"], "//urlbotao4"));
            linkBotao4 = HttpUtility.HtmlDecode(GenericHelper.GetSingleNodeString((string)Session["xmlHandshake"], "//link_retorno"));
            target = HttpUtility.HtmlDecode(GenericHelper.GetSingleNodeString((string)Session["xmlHandshake"], "/pedido/parametros_opcionais/target"));
        }
    }

    public class Item
    {
        private string _itemCode;
        private string _itemDescription;
        private int _itemQuantity;
        private decimal _itemValue;

        public string ItemCode
        {
            get { return _itemCode; }
            set { _itemCode = value; }
        }
        public string ItemDescription
        {
            get { return _itemDescription; }
            set { _itemDescription = value; }
        }
        public int ItemQuantity
        {
            get { return _itemQuantity; }
            set { _itemQuantity = value; }
        }
        public decimal ItemValue
        {
            get { return _itemValue; }
            set { _itemValue = value; }
        }
        public decimal ItemTotal
        {
            get { return _itemValue * _itemQuantity; }
        }

    }

}
