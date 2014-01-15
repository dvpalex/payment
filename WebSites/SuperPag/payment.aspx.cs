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
using SuperPag.Helper;
using SuperPag;
using System.Collections.Generic;
using System.Text;
using System.Globalization;
using System.IO;

public partial class payment : System.Web.UI.Page
{
    int storeId;
    string linkBotao6, urlBotao6, urlBotao1;
    int handshakeConfigurationId;

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
        pnlInputCreditCard.Visible = false;

        //preenche o table top
        DStore dStore = GenericHelper.CheckSessionStore(Context);

        FillTableTop(dStore.storeId);
        storeId = dStore.storeId;
        handshakeConfigurationId = dStore.handshakeConfigurationId;

        UpdateHandshakeVars();

        if (!Page.IsPostBack)
        {
            int handshakePaymentFormId;
            int handshakeInstallmentNumber;
            bool isGroup;

            ddlPayments.Visible = GenericHelper.GetCanChoosePaymentFormSession(Context);
            handshakePaymentFormId = GenericHelper.GetHandshakePaymentFormSession(Context, out isGroup);
            handshakeInstallmentNumber = GenericHelper.GetHandshakeInstallmentNumber(Context);
            Profile.InstallmentNumber = handshakeInstallmentNumber.ToString();

            if (isGroup)
            {
                GenericHelper.SetPaymentGroupSession(Context, handshakePaymentFormId);
            }
            else if (handshakePaymentFormId != int.MinValue)
            {
                GenericHelper.SetPaymentGroupSession(Context, int.MinValue);
                GenericHelper.SetPaymentFormSession(Context, handshakePaymentFormId);

                //altero o status da order para pagamento escolhido
                GenericHelper.SetOrderStatus(Context, WorkflowOrderStatus.PaymentFormChoosed, handshakePaymentFormId.ToString());
            }

            if (handshakeInstallmentNumber != int.MinValue)
                GenericHelper.SetInstallmentNumber(Context, handshakeInstallmentNumber);

            DStorePaymentForm[] dStorePayForms = DataFactory.StorePaymentForm().List(dStore.storeId, true);
            Ensure.IsNotNullPage(dStorePayForms, "A loja {0} não possui formas de pagamento configuradas", dStore.storeId);

            FillPaymentFormList(dStorePayForms);
            FillGroupInstruction();
            FillPaymentInstruction();
            FillButtons();
        }

        SetEncoding();
    }

    public void SetEncoding()
    {
        string encoding = "utf-8";
        DHandshakeConfiguration hsConfig = DataFactory.HandshakeConfiguration().Locate(handshakeConfigurationId);
        if (hsConfig != null && hsConfig.handshakeType == (int)HandshakeType.HtmlSPag10)
        {
            DHandshakeConfigurationHtml hsHtml = DataFactory.HandshakeConfigurationHtml().Locate(handshakeConfigurationId);
            if (hsHtml != null && !String.IsNullOrEmpty(hsHtml.responseEncoding))
                encoding = hsHtml.responseEncoding;
        }
        Response.ContentEncoding = System.Text.Encoding.GetEncoding(encoding);
    }

    public void ddlPayments_SelectedIndexChanged(object sender, System.EventArgs e)
    {
        //se foi escolhido o grupo de cartao de credito
        if (ddlPayments.SelectedItem.Value == PaymentGroupsWord.CreditCard)
        {
            //seto a sessao
            GenericHelper.SetPaymentGroupSession(Context, (int)PaymentGroups.CreditCard);
            GenericHelper.SetPaymentFormSession(Context, int.MinValue);
        }
        //se foi escolhido um meio de pagamento
        else if (ddlPayments.SelectedValue != "")
        {
            //seto a sessao
            GenericHelper.SetPaymentGroupSession(Context, Int32.MinValue);
            GenericHelper.SetPaymentFormSession(Context, Int32.Parse(ddlPayments.SelectedValue));

            //altero o status da order para pagamento escolhido
            GenericHelper.SetOrderStatus(Context, WorkflowOrderStatus.PaymentFormChoosed, ddlPayments.SelectedValue.ToString());
        }
        //se nao foi escolhido nada
        else
        {
            //seto a sessao
            GenericHelper.SetPaymentGroupSession(Context, int.MinValue);
            GenericHelper.SetPaymentFormSession(Context, int.MinValue);
        }

        //mostro as instrucoes do grupo se necessario
        FillGroupInstruction();

        //mostro as instrucoes da forma de pagamento se necessario
        FillPaymentInstruction();
    }

    protected void rptCreditCards_ItemCommand(object source, RepeaterCommandEventArgs e)
    {
        //seto a sessao
        GenericHelper.SetPaymentFormSession(Context, Int32.Parse(e.CommandArgument.ToString()));

        //altero o status da order para pagamento escolhido
        GenericHelper.SetOrderStatus(Context, WorkflowOrderStatus.PaymentFormChoosed, e.CommandArgument.ToString());

        FillGroupInstruction();
        FillPaymentInstruction();

    }

    protected void btnNext_Click(object sender, ImageClickEventArgs e)
    {
        int paymentFormId = GenericHelper.GetPaymentFormSession(this.Context);
        DOrder order = GenericHelper.CheckSessionOrder(this.Context);

        int storePaymentInstallmentNumber;
        decimal installmentValue;
        decimal interestPercentage;

        //verifica se existe alguma attempt paga para orderId em questao
        //caso exista nao deixa seguir com um outro pagamento
        if (SuperPag.Handshake.Helper.OrderIsPaid(order.orderId))
            GenericHelper.RedirectToErrorPage("O pedido já consta como pago no sistema. Para evitar qualquer transtorno com o seu pagamento, contate a loja em que efetuou o pedido para maiores esclarecimentos");

        //Se é moset passo os dados do cartão de crédito capturados nessa tela
        if (paymentFormId == (int)PaymentForms.VisaMoset || paymentFormId == (int)PaymentForms.Amex2Party || paymentFormId == (int)PaymentForms.VisaMoset3 || paymentFormId == (int)PaymentForms.MasterWebService || paymentFormId == (int)PaymentForms.DinersWebService)
        {
            Ensure.IsNumeric64Page(txtCardNumber.Text, "As informações do cartão estão inválidas");
            Ensure.IsNumericPage(txtCardSecurity.Text, "As informações do cartão estão inválidas");
            Ensure.IsNotNullOrEmptyPage(txtCardName.Text, "As informações do cartão estão inválidas");

            DateTime cardExpiration = new DateTime(int.Parse(ddlCardValidationYear.SelectedValue), int.Parse(ddlCardValidationMonth.SelectedValue), 1);

            string xml = GenericHelper.CreateCreditCardXml(txtCardName.Text, txtCardNumber.Text, txtCardSecurity.Text, cardExpiration);
            GenericHelper.SetCreditCardXmlSession(Context, xml);
        }

        if (paymentFormId == (int)PaymentForms.FinanciamentoABN)
        {
            //preenche a instrucao de finalizacao
            string xmlData;
            if (Session["htmlHandshake"] != null)
                xmlData = Session["htmlHandshake"].ToString();
            else
                xmlData = Session["xmlHandshake"].ToString();

            Ensure.IsNotNullOrEmptyPage(xmlData, "Parâmetros insuficientes para completar uma transação de Financiamento ABN.");

            string valorEntrada = GenericHelper.GetSingleNodeString(xmlData, "//val_entrada_abn");
            string valorParcela = GenericHelper.GetSingleNodeString(xmlData, "//val_parcela_abn");
            Ensure.IsNotNullOrEmptyPage(valorParcela, "Parâmetros insuficientes para completar uma transação de Financiamento ABN.");

            string dataVencimento = GenericHelper.GetSingleNodeString(xmlData, "//dat_vencimento1_abn");
            Ensure.IsNotNullOrEmptyPage(dataVencimento, "Parâmetros insuficientes para completar uma transação de Financiamento ABN.");

            string qtdPrestacao = GenericHelper.GetSingleNodeString(xmlData, "//qtdparcelas_abn");
            Ensure.IsNotNullOrEmptyPage(valorParcela, "Parâmetros insuficientes para completar uma transação de Financiamento ABN.");

            if (!int.TryParse(qtdPrestacao, out storePaymentInstallmentNumber))
                Ensure.IsNotNullPage(null, "Não foi possível recuperar as informações para o Financiamento ABN/AMRO");

            GenericHelper.SetInstallmentNumber(Context, storePaymentInstallmentNumber);
            installmentValue = GenericHelper.ParseDecimal(valorParcela);
            interestPercentage = 0;
            decimal finalAmount = installmentValue * storePaymentInstallmentNumber;

            //atualizo a order
            order.finalAmount = finalAmount;
            order.installmentQuantity = storePaymentInstallmentNumber;
            DataFactory.Order().Update(order);
        }
        else
        {
            if (GenericHelper.GetInstallmentNumber(Context) >= 1)
            {
                storePaymentInstallmentNumber = GenericHelper.GetInstallmentNumber(Context);
            }
            else
            {
                Ensure.IsNotNullPage(Profile.InstallmentNumber, "Parcela não escolhida.");
                storePaymentInstallmentNumber = Int32.Parse(Profile.InstallmentNumber);
            }

            //pego o parcelamento escolhido
            DStorePaymentInstallment dStorePaymentInstallment = DataFactory.StorePaymentInstallment().Locate(storeId, paymentFormId, storePaymentInstallmentNumber);
            Ensure.IsNotNullPage(dStorePaymentInstallment, "A loja {0} não possui o parcelamento escolhido para esse meio de pagamento.", storeId);

            //calculo valor da parcela e finalAmout da order
            installmentValue = GenericHelper.GetInstallmentValue(dStorePaymentInstallment.installmentNumber, order.totalAmount, dStorePaymentInstallment.interestPercentage);
            decimal finalAmount = installmentValue * dStorePaymentInstallment.installmentNumber;
            interestPercentage = dStorePaymentInstallment.interestPercentage;

            //atualizo a order
            order.finalAmount = finalAmount;
            order.installmentQuantity = storePaymentInstallmentNumber;
            DataFactory.Order().Update(order);
        }

        List<Guid> paymentAttemptIdList = new List<Guid>();
        DPaymentAttempt paymentAttempt = new DPaymentAttempt();
        bool hasInstallmentAttempts = GenericHelper.HasInstallmentAttempts(paymentFormId);
        int loop = (hasInstallmentAttempts ? storePaymentInstallmentNumber : 1);

        int paymentAgentSetupId = GenericHelper.GetPaymentAgentSetupId(storeId, paymentFormId);

        //Cria OrderInstallment
        DataFactory.OrderInstallment().Delete(order.orderId);
        for (int i = 1; i <= storePaymentInstallmentNumber; i++)
        {
            DOrderInstallment orderInstallment = new DOrderInstallment();
            orderInstallment.orderId = order.orderId;
            orderInstallment.installmentNumber = i;
            orderInstallment.paymentFormId = paymentFormId;
            orderInstallment.installmentValue = installmentValue;
            orderInstallment.interestPercentage = interestPercentage;
            orderInstallment.status = (int)OrderInstallmentStatus.Pending;
            DataFactory.OrderInstallment().Insert(orderInstallment);
        }

        //Cria PaymentAttempt
        for (int i = 1; i <= loop; i++)
        {
            paymentAttempt = new DPaymentAttempt();
            paymentAttempt.paymentAttemptId = Guid.NewGuid();

            if (hasInstallmentAttempts)
                paymentAttempt.price = GenericHelper.UseTestValuesForAgents(storeId, paymentFormId) ? GenericHelper.GetTestValueForAgent(paymentFormId, order.installmentQuantity) : installmentValue;
            else
                paymentAttempt.price = GenericHelper.UseTestValuesForAgents(storeId, paymentFormId) ? GenericHelper.GetTestValueForAgent(paymentFormId, order.installmentQuantity) : order.finalAmount;

            paymentAttempt.orderId = order.orderId;
            paymentAttempt.paymentFormId = paymentFormId;
            paymentAttempt.paymentAgentSetupId = paymentAgentSetupId;
            paymentAttempt.startTime = DateTime.Now;
            paymentAttempt.lastUpdate = DateTime.Now;
            paymentAttempt.step = 0;
            paymentAttempt.installmentNumber = (hasInstallmentAttempts ? i : int.MinValue);
            paymentAttempt.status = (int)PaymentAttemptStatus.Pending;
            paymentAttempt.billingScheduleId = int.MinValue;

            if (Session["isSimulation"] != null && (bool)Session["isSimulation"])
                paymentAttempt.isSimulation = true;

            DataFactory.PaymentAttempt().Insert(paymentAttempt);

            paymentAttemptIdList.Add(paymentAttempt.paymentAttemptId);
        }

        if (hasInstallmentAttempts)
            GenericHelper.SetPaymentAttemptSession(Context, paymentAttemptIdList);
        else
            GenericHelper.SetPaymentAttemptSession(Context, paymentAttempt.paymentAttemptId);

        //pego o agent
        int paymentAgentId = GenericHelper.GetPaymentAgentId(paymentFormId);

        //Seto o status do pedido
        GenericHelper.SetOrderStatus(HttpContext.Current, WorkflowOrderStatus.InstallmentChoosed, paymentFormId + "," + storePaymentInstallmentNumber);

        //Redireciona para Agent especifico
        Response.Redirect(GenericHelper.GetPaymentAgentWebPage(paymentAgentId, storeId));
    }

    private void FillPaymentFormList(DStorePaymentForm[] dStorePayForms)
    {
        ddlPayments.Items.Clear();
        if (CultureInfo.CurrentUICulture.Name == "en")
            ddlPayments.Items.Add(new ListItem("Choose Payment Mode ", ""));
        else if (CultureInfo.CurrentUICulture.Name == "es")
            ddlPayments.Items.Add(new ListItem("Elegir el modo del pago ", ""));
        else
            ddlPayments.Items.Add(new ListItem("Escolha um Meio de Pagamento ", ""));


        List<DPaymentForm> dPaymentCreditCards = new List<DPaymentForm>();

        bool creditCardAdded = false;
        foreach (DStorePaymentForm dStorePaymentForm in dStorePayForms)
        {
            DPaymentForm dPaymentForm = DataFactory.PaymentForm().Locate(dStorePaymentForm.paymentFormId);
            if (dPaymentForm.paymentFormGroupId != (int)PaymentGroups.CreditCard)
            {
                ddlPayments.Items.Add(new ListItem(dPaymentForm.name, dPaymentForm.paymentFormId.ToString()));
            }
            else
            {
                if (!creditCardAdded)
                {
                    creditCardAdded = true;
                    if (CultureInfo.CurrentUICulture.Name == "en")
                        ddlPayments.Items.Add(new ListItem("Credit card", PaymentGroupsWord.CreditCard));
                    else if (CultureInfo.CurrentUICulture.Name == "es")
                        ddlPayments.Items.Add(new ListItem("Tarjeta de crédito", PaymentGroupsWord.CreditCard));
                    else
                        ddlPayments.Items.Add(new ListItem("Cartão de Crédito", PaymentGroupsWord.CreditCard));

                }

                dPaymentCreditCards.Add(dPaymentForm);
            }
        }

        if (dPaymentCreditCards.Count > 0)
        {
            rptCreditCards.DataSource = dPaymentCreditCards;
            rptCreditCards.DataBind();
        }
    }

    private void FillGroupInstruction()
    {
        int paymentGroupId = GenericHelper.GetPaymentGroupSession(Context);

        if (paymentGroupId == int.MinValue)
        {
            plhPaymentGroupInstructions.Visible = false;
            rptCreditCards.Visible = false;
            return;
        }

        //obtem a instrucao da forma de pagamento escolhida
        DSPLegacyPaymentGroup dSPLegacyGroup = DataFactory.SPLegacyPaymentGroup().Locate(paymentGroupId, storeId);
        if (Ensure.IsNotNull(dSPLegacyGroup) && Ensure.IsNotNull(dSPLegacyGroup.ucInstructions))
        {
            //carrega o controle de instrucoes
            plhPaymentGroupInstructions.Controls.Add(Page.LoadControl(dSPLegacyGroup.ucInstructions));

            //seta a instrucao do grupo como visivel
            plhPaymentGroupInstructions.Visible = true;
        }
        else
            plhPaymentGroupInstructions.Visible = false;

        rptCreditCards.Visible = true;
    }

    private void FillPaymentInstruction()
    {
        int paymentFormId = GenericHelper.GetPaymentFormSession(Context);
        if (paymentFormId == int.MinValue)
        {
            plhPaymentInstructions.Visible = false;
            gvInstallments.Visible = false;
            btnNext.Visible = false;
            return;
        }

        //preenche parcelas
        DOrder order = GenericHelper.CheckSessionOrder(this.Context);
        DStorePaymentInstallment[] dStorePaymentInstallments = DataFactory.StorePaymentInstallment().List(storeId, paymentFormId, order.totalAmount);
        Ensure.IsNotNullPage(dStorePaymentInstallments, "A loja {0} não possui nenhum parcelamento configurado para esse meio de pagamento e valor.", storeId);

        bool installmentIsOk = false;

        //pula a tela de parcelamento caso tenha vindo o parâmetro no handshake
        if (GenericHelper.GetInstallmentNumber(Context) != int.MinValue && GenericHelper.GetInstallmentNumber(Context) >= 1)
        {
            //checar se a parcela existe na configuração
            foreach (DStorePaymentInstallment installment in dStorePaymentInstallments)
            {
                if (installment.installmentNumber == GenericHelper.GetInstallmentNumber(Context))
                {
                    installmentIsOk = true;
                    break;
                }
            }

            if (installmentIsOk)
            {
                if (paymentFormId != (int)PaymentForms.VisaMoset && paymentFormId != (int)PaymentForms.Amex2Party && paymentFormId != (int)PaymentForms.VisaMoset3 && paymentFormId != (int)PaymentForms.MasterWebService && paymentFormId != (int)PaymentForms.DinersWebService)
                {
                    this.btnNext_Click(null, null);
                    return;
                }
            }
            else
                GenericHelper.SetInstallmentNumber(Context, int.MinValue);
        }

        //obtem a instrucao da forma de pagamento escolhida
        DSPLegacyPaymentForm dSPLegacyForm = DataFactory.SPLegacyPaymentForm().Locate(paymentFormId, storeId);
        if (Ensure.IsNotNull(dSPLegacyForm) && Ensure.IsNotNull(dSPLegacyForm.ucInstructions))
        {
            //carrega o controle de instrucoes
            plhPaymentInstructions.Controls.Add(Page.LoadControl(dSPLegacyForm.ucInstructions));

            //seta a instrucao da forma de pagamento escolhida
            plhPaymentInstructions.Visible = true;
        }
        else
            plhPaymentInstructions.Visible = false;


        List<InstallmentInfo> installmentsInfo = new List<InstallmentInfo>();
        foreach (DStorePaymentInstallment dStorePaymentInstallment in dStorePaymentInstallments)
        {
            string parcela = "";
            if (dStorePaymentInstallment.installmentNumber == 1)
                parcela = "A Vista";
            else
                parcela = dStorePaymentInstallment.installmentNumber + "x";

            decimal installmentValue = GenericHelper.GetInstallmentValue(dStorePaymentInstallment.installmentNumber, order.totalAmount, dStorePaymentInstallment.interestPercentage);
            installmentsInfo.Add(new InstallmentInfo(dStorePaymentInstallment.installmentNumber, parcela, installmentValue.ToString("C", GenericHelper.GetNumberFormatBrasil()), (installmentValue * dStorePaymentInstallment.installmentNumber).ToString("C", GenericHelper.GetNumberFormatBrasil()), dStorePaymentInstallment.interestPercentage.ToString("N2")));
        }

        if (Session["PQTDPARCELAS"] == null)
        {
            gvInstallments.DataSource = installmentsInfo;
            gvInstallments.DataKeyNames = new string[] { "InstallmentNumber" };
            gvInstallments.DataBind();
        }

        if (paymentFormId == (int)PaymentForms.FinanciamentoABN)
            gvInstallments.Visible = false;
        else if (paymentFormId == (int)PaymentForms.VisaMoset || paymentFormId == (int)PaymentForms.VisaMoset3 || paymentFormId == (int)PaymentForms.MasterWebService || paymentFormId == (int)PaymentForms.DinersWebService)
        {
            gvInstallments.Visible = true;
            GenericHelper.SetInstallmentNumber(Context, int.MinValue);
        }
        else if (paymentFormId == (int)PaymentForms.Amex2Party && installmentIsOk)
            gvInstallments.Visible = false;
        else
            gvInstallments.Visible = true;


        if (paymentFormId == (int)PaymentForms.VisaMoset || paymentFormId == (int)PaymentForms.VisaMoset3 || paymentFormId == (int)PaymentForms.MasterWebService || paymentFormId == (int)PaymentForms.DinersWebService)
        {
            lblCVV.Text = "CVV(*)";
            lblCardMessage.Text = "(*) Informar os três últimos dígitos do número em negrito no verso do cartão próximo à tarja de assinatura.";
            pnlInputCreditCard.Visible = true;
        }
        else if (paymentFormId == (int)PaymentForms.Amex2Party)
        {
            lblCVV.Text = "CVV2(*)";
            lblCardMessage.Text = "(*) Informar os quatro dígitos impressos acima do número de seu cartão.";
            pnlInputCreditCard.Visible = true;
        }

        btnNext.Visible = true;
    }

    public string GetImageUrl(string paymentFormId)
    {
        switch (Int32.Parse(paymentFormId))
        {
            case (int)PaymentForms.MasterKomerci:
            case (int)PaymentForms.MasterKomerciInBox:
            case (int)PaymentForms.MasterWebService:
            case (int)PaymentForms.MasterSitef:
                return Page.ResolveUrl("~/Images/Flags/master.gif");
            case (int)PaymentForms.DinersKomerci:
            case (int)PaymentForms.DinersKomerciInBox:
            case (int)PaymentForms.DinersWebService:
            case (int)PaymentForms.DinersSitef:
                return Page.ResolveUrl("~/Images/Flags/diners.gif");
            case (int)PaymentForms.VisaVBVInBox:
            case (int)PaymentForms.VisaVBV:
            case (int)PaymentForms.VisaMoset:
            case (int)PaymentForms.VisaVBV3:
            case (int)PaymentForms.VisaMoset3:
                return Page.ResolveUrl("~/Images/Flags/visa.gif");
            case (int)PaymentForms.Amex2Party:
            case (int)PaymentForms.Amex3Party:
                return Page.ResolveUrl("~/Images/Flags/amex.gif");

        }
        return "";
    }

    public string GetUTF8String(string isoText)
    {
        return Encoding.UTF8.GetString(Encoding.Convert(Encoding.GetEncoding("iso-8859-1"), Encoding.UTF8, Encoding.GetEncoding("iso-8859-1").GetBytes(isoText)));
    }

    protected void gvInstallments_RowCreated(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            int inst = (int)gvInstallments.DataKeys[e.Row.RowIndex].Value;

            string check = ((GenericHelper.GetInstallmentNumber(Context) == int.MinValue && e.Row.RowIndex == 0) || inst == GenericHelper.GetInstallmentNumber(Context)) ? "checked" : "";
            e.Row.Cells[0].Text = string.Format("<input name=\"rdbInstallment\" type=\"radio\" value=\"{0}\" {1}>", inst.ToString(), check);

            if (inst == GenericHelper.GetInstallmentNumber(Context))
            {

                Session["PQTDPARCELAS"] = true;
            }
        }

        if (e.Row.RowType == DataControlRowType.Footer && Session["PQTDPARCELAS"] != null)
        {
            //desabilita o radio se PQTDPARCELAS for passada 
            for (int i = 0; i < gvInstallments.Rows.Count; i++)
            {
                gvInstallments.Rows[i].Cells[0].Enabled = false;
            }
            Session["PQTDPARCELAS"] = null;
        }
    }

    public void UpdateHandshakeVars()
    {
        //Seto as variaveis a serem usadas do handshake
        if (Session["htmlHandshake"] != null)
        {
            urlBotao1 = HttpUtility.HtmlDecode(GenericHelper.GetSingleNodeString((string)Session["htmlHandshake"], "//urlbotao1"));
            linkBotao6 = HttpUtility.HtmlDecode(GenericHelper.GetSingleNodeString((string)Session["htmlHandshake"], "//linkbotao6"));
            urlBotao6 = HttpUtility.HtmlDecode(GenericHelper.GetSingleNodeString((string)Session["htmlHandshake"], "//urlbotao6"));
        }
        else
        {
            urlBotao1 = HttpUtility.HtmlDecode(GenericHelper.GetSingleNodeString((string)Session["xmlHandshake"], "//urlbotao1"));
            linkBotao6 = HttpUtility.HtmlDecode(GenericHelper.GetSingleNodeString((string)Session["xmlHandshake"], "//link_botao6"));
            urlBotao6 = HttpUtility.HtmlDecode(GenericHelper.GetSingleNodeString((string)Session["xmlHandshake"], "//urlbotao6"));
        }
    }

    public void FillButtons()
    {
        int paymentFormId = GenericHelper.GetPaymentFormSession(Context);
        if (paymentFormId != int.MinValue && storeId != int.MinValue && GenericHelper.UseTestValuesForAgents(storeId, paymentFormId))
        {
            lblHomolog.Text = "Atenção, nesta transação os valores utilizados serão fictícios";
            lblHomolog.Visible = true;
        }
        else
            lblHomolog.Visible = false;

        //btnNext - botao1 (continuar)
        if (!String.IsNullOrEmpty(urlBotao1))
            btnNext.ImageUrl = urlBotao1;
        else
        {
            string btnNextPath = "~/Store/" + storeId + "/images/btnNext.gif";
            if (File.Exists(Server.MapPath(btnNextPath)))
                btnNext.ImageUrl = btnNextPath;
        }

        //lnkReturn - botao6 (Retornar a loja)
        //caso a loja tenha passado o endereco
        if (!String.IsNullOrEmpty(urlBotao6))
            lnkReturn.ImageUrl = urlBotao6;
        else
        {
            string lnkReturnPath = "~/Store/" + storeId + "/images/lnkReturn.gif";
            if (File.Exists(Server.MapPath(lnkReturnPath)))
                lnkReturn.ImageUrl = lnkReturnPath;
        }

        if (!String.IsNullOrEmpty(linkBotao6))
        {
            lnkReturn.NavigateUrl = linkBotao6;
            lnkReturn.Visible = true;
        }
    }

}
