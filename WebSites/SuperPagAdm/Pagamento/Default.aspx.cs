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
using System.Collections.Generic;
using SuperPag;
using System.Xml;
using SuperPag.Business;
using SuperPag.Business.Messages;
using SuperPag.Data;
using Root.Reports;
using SuperPag.Handshake;
using System.IO;
using SuperPag.Agents.Boleto;
using localhost;
using SuperPag.Helper.Xml;
using SuperPag.Helper.Xml.Request;
using SuperPag.Handshake.Service;
using SuperPag.Data.Messages;
using SuperPag.Helper.Xml.Response;
using System.Text;
using System.Text.RegularExpressions;

public partial class Pagamento_Default : System.Web.UI.Page
{
    //Properties da pagina
    public DateTime Vencimento
    {
        get
        {
            DateTime Vencimento = new DateTime(2000, 7, 3);
            return Vencimento.AddDays((Convert.ToInt32(LimpaIPTE(txtIPTE.Text).Substring(33, 4)) - 1000));
        }
    }
    public Decimal Valor
    {
        get
        {
            return Convert.ToDecimal(LimpaIPTE(txtIPTE.Text).Substring(37, 10).Insert(8, ","));
        }
    }
    public string IPTE
    {
        get
        {
            return LimpaIPTE(txtIPTE.Text).Insert(5, " . ").Insert(10 + 3, " ").Insert(15 + 4, " . ").Insert(21 + 7, " ").Insert(26 + 8, " . ").Insert(32 + 11, " ").Insert(33 + 12, " ");
        }
    }
    private string _paymentAttemptId;
    public string PaymentAttemptId
    {
        get { return _paymentAttemptId; }
        set { _paymentAttemptId = value; }
    }
    public Guid UserId
    {
        get
        {
            return (Guid)Membership.GetUser(true).ProviderUserKey;
        }
    }
    public int storeId
    {
        get
        {
            return Store.LocateStore(this.UserId).StoreId;
        }
    }

    /// <summary>
    /// retorna a chave da loja do usuario logado
    /// </summary>
    public string StoreKey
    {
        get
        {
            return Store.LocateStore(this.UserId).StoreKey;
        }
    }
    /// <summary>
    /// Pega o corpo do e-mail na tabela PaymentAgentSetupBoleto
    /// </summary>
    public string BodyMail
    {
        get
        {
            return PaymentAgentSetupBoleto.Locate(SuperPag.Helper.GenericHelper.GetPaymentAgentSetupId(storeId, PaymentForm.Locate(storeId, "Boleto Bancario InvestCred").PaymentFormId)).BodyMail;
        }
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            //Valida o campo IPTE, não permite a digitação de nada, este campo só aceita COPY/PESTE
            this.txtIPTE.Attributes["onkeypress"] = "return ValidaTecla(this, event,'ACEITA_NADA');";
            this.txtCEP.Attributes["onkeypress"] = "return numero(event);";
            this.txtDDD.Attributes["onkeypress"] = "return numero(event);";
            this.txtTel.Attributes["onkeypress"] = "return numero(event);";

            MultiViewGeral.ActiveViewIndex = (int)SuperPag.MultiViewGeral.Entrada;
            MultiViewPagForm.ActiveViewIndex = (int)SuperPag.MultiViewPagForm.FormaPagamentoIPTE;
        }
    }

    protected void RadioEnvio_SelectedIndexChanged(object sender, EventArgs e)
    {
        switch (RadioEnvio.SelectedValue)
        {
            case "01"://E-mail
                MultiViewEnvio.ActiveViewIndex = (int)SuperPag.MultiViewEnvio.Email;
                break;
            case "02"://Fax
                MultiViewEnvio.ActiveViewIndex = (int)SuperPag.MultiViewEnvio.Fax;
                break;
            case "03"://Correio
                MultiViewEnvio.ActiveViewIndex = (int)SuperPag.MultiViewEnvio.Correio;
                break;
            case "04"://impressao
                MultiViewEnvio.ActiveViewIndex = -1;
                break;
        }
    }

    protected void btnConfirmarGeral_Click(object sender, EventArgs e)
    {
        try
        {

            if (this.IPTE.Substring(0, 5) != "24990")
            {
                throw new Exception("IPTE Inválido banco não confere.");
            }

            if (LimpaIPTE(this.IPTE).Length < 47)
            {
                throw new Exception("Número de IPTE inválido.");
            }

            MultiViewGeral.ActiveViewIndex = 1;
            switch (RadioEnvio.SelectedValue)
            {
                case "01"://E-mail
                    MultiViewPagFinal.ActiveViewIndex = (int)SuperPag.MultiViewPagFinal.FinalEmail;
                    this.lblEmailIPTE.Text = IPTE;
                    this.lblEmailSacado.Text = this.txtSacado.Text;
                    this.lblEmailValOp.Text = this.Valor.ToString();
                    this.lblEmailDataVenc.Text = this.Vencimento.Date.ToString("dd/MM/yyyy");
                    this.lblEmailCanalEnv.Text = this.RadioEnvio.SelectedItem.Text;
                    this.lblEmailCanalEmail.Text = this.txtEmail.Text;
                    break;
                case "02"://Fax
                    MultiViewPagFinal.ActiveViewIndex = (int)SuperPag.MultiViewPagFinal.FinalFax;

                    this.lblFaxIPTE.Text = IPTE;
                    this.lblFaxValOp.Text = this.Valor.ToString();
                    this.lblFaxDataVenc.Text = this.Vencimento.Date.ToString("dd/MM/yyyy");
                    this.lblFaxSacado.Text = this.txtSacado.Text;
                    this.lblFaxCanalEnv.Text = this.RadioEnvio.SelectedItem.Text;
                    this.lblFaxTel.Text = string.Concat("(", this.txtDDD.Text, ")", this.txtTel.Text);
                    break;
                case "03"://Correio
                    MultiViewPagFinal.ActiveViewIndex = (int)SuperPag.MultiViewPagFinal.FinalCorreio;

                    this.lblCorreioIPTE.Text = IPTE;
                    this.lblCorreioSacado.Text = this.txtSacado.Text;
                    this.lblCorreioCanalEnv.Text = this.RadioEnvio.SelectedItem.Text;
                    this.lblCorreioEnd.Text = string.Concat(this.txtEnd.Text, ", ", this.txtNum.Text, " - ", this.txtComplemento.Text);
                    this.lblCorreioBairro.Text = this.txtBairro.Text;
                    this.lblCorreioCEP.Text = this.txtCEP.Text;
                    this.lblCorreioEstado.Text = this.ddlEstado.SelectedItem.Text;
                    this.lblCorreioCidade.Text = this.txtCidade.Text;
                    this.lblCorreioValOp.Text = this.Valor.ToString();
                    this.lblCorreioDataVenc.Text = this.Vencimento.Date.ToString("dd/MM/yyyy");

                    break;
                case "04"://impressao
                    MultiViewPagFinal.ActiveViewIndex = (int)SuperPag.MultiViewPagFinal.FinalImpressao;

                    this.lblImpressaoIPTE.Text = IPTE;
                    this.lblImpressaoSacado.Text = this.txtSacado.Text;
                    this.lblImpressaoCanalEnv.Text = this.RadioEnvio.SelectedItem.Text;
                    this.lblImpressaoValOp.Text = this.Valor.ToString();
                    this.lblImpressaoDataVenc.Text = this.Vencimento.Date.ToString("dd/MM/yyyy");
                    break;
            }
        }
        catch (Exception ex)
        {
            CustomIPTEent.IsValid = false;
            CustomIPTEent.ErrorMessage = ex.Message;
        }
    }

    protected void btnFinalCancel_Click(object sender, EventArgs e)
    {
        MultiViewGeral.ActiveViewIndex = (int)SuperPag.MultiViewGeral.Entrada;
    }

    /// <summary>
    /// Salva as informações nas tabelas do superpag
    /// </summary>
    private void Salvar(string xml)
    {
        XmlDocument ObjxmlRet = new XmlDocument();

        DStore store = DataFactory.Store().Locate(StoreKey);

        request req;
        string msgerror = "";
        if ((req = (request)XmlHelper.GetClass(xml, typeof(request), out msgerror)) == null)
            Ensure.IsNotNull(null, msgerror);

        Request requestHelper = new Request();

        response resp = requestHelper.ProcessRequest(store, req, this.UserId);

        string s = XmlHelper.GetXml(typeof(response), resp);

        ObjxmlRet.LoadXml(s);
        PaymentAttemptId = ObjxmlRet.ChildNodes[0].ChildNodes[0].ChildNodes[0].ChildNodes[0].ChildNodes[0].ChildNodes[0].ChildNodes[0].InnerText;

        SuperPag.Helper.GenericHelper.LogFile("pagamento::Salvar " + s, LogFileEntryType.Information);

    }

    protected void btnFinalComfirm_Click(object sender, EventArgs e)
    {
        switch (RadioEnvio.SelectedValue)
        {
            case "01"://E-mail                      
                InsertXML(SuperPag.MultiViewPagFinal.FinalEmail);
                break;
            case "02"://Fax
                InsertXML(SuperPag.MultiViewPagFinal.FinalFax);
                break;
            case "03"://Correio
                InsertXML(SuperPag.MultiViewPagFinal.FinalCorreio);
                break;
            case "04"://impressao
                InsertXML(SuperPag.MultiViewPagFinal.FinalImpressao);
                break;
        }
    }

    //Cria xml de envio
    private void InsertXML(SuperPag.MultiViewPagFinal tipo)
    {
        try
        {
            XmlDocument Objxml = new XmlDocument();

            Objxml.Load(Server.MapPath("xml\\") + "default.xml");

            Objxml.ChildNodes[0].ChildNodes[0].ChildNodes[0].Attributes["reference"].Value = "1";
            Objxml.ChildNodes[0].ChildNodes[0].ChildNodes[0].Attributes["total"].Value = "1";
            Objxml.ChildNodes[0].ChildNodes[0].ChildNodes[0].ChildNodes[0].ChildNodes[0].Attributes["name"].Value = txtSacado.Text;
            Objxml.ChildNodes[0].ChildNodes[0].ChildNodes[0].ChildNodes[2].ChildNodes[0].Attributes["amount"].Value = "1";
            Objxml.ChildNodes[0].ChildNodes[0].ChildNodes[0].ChildNodes[2].ChildNodes[0].Attributes["form"].Value = ddlTipoPag.SelectedValue;
            //IPTE
            Objxml.ChildNodes[0].ChildNodes[0].ChildNodes[0].ChildNodes[2].ChildNodes[0].ChildNodes[0].ChildNodes[0].ChildNodes[0].InnerText = LimpaIPTE(txtIPTE.Text);
            Objxml.ChildNodes[0].ChildNodes[0].ChildNodes[0].ChildNodes[2].ChildNodes[0].ChildNodes[0].ChildNodes[0].ChildNodes[2].InnerText = Vencimento.ToString("yyyy-MM-dd");
            Objxml.ChildNodes[0].ChildNodes[0].ChildNodes[0].ChildNodes[2].ChildNodes[0].ChildNodes[0].ChildNodes[0].ChildNodes[3].InnerText = txtContrato.Text;
            switch (tipo)
            {
                case SuperPag.MultiViewPagFinal.FinalEmail:
                    Objxml.ChildNodes[0].ChildNodes[0].ChildNodes[0].ChildNodes[0].ChildNodes[0].ChildNodes[0].ChildNodes[0].Attributes["address"].Value = txtEmail.Text;

                    Salvar(Objxml.InnerXml);

                    GerarBoleto(false);
                    EnviaMail(txtEmail.Text, "Boleto Ponto Cred : " + this.PaymentAttemptId, BodyMail);

                    break;
                case SuperPag.MultiViewPagFinal.FinalFax:
                    Objxml.ChildNodes[0].ChildNodes[0].ChildNodes[0].ChildNodes[0].ChildNodes[0].ChildNodes[1].ChildNodes[0].Attributes["type"].Value = "fax";
                    Objxml.ChildNodes[0].ChildNodes[0].ChildNodes[0].ChildNodes[0].ChildNodes[0].ChildNodes[1].ChildNodes[0].Attributes["ddd"].Value = txtDDD.Text;
                    Objxml.ChildNodes[0].ChildNodes[0].ChildNodes[0].ChildNodes[0].ChildNodes[0].ChildNodes[1].ChildNodes[0].Attributes["number"].Value = txtTel.Text;

                    Salvar(Objxml.InnerXml);

                    GerarBoleto(false);
                    if (txtDDD.Text == "11")
                    {
                        EnviaMail(ConfigurationManager.AppSettings["EmailFax"].ToString(), txtTel.Text, string.Empty);
                    }
                    else
                    {
                        EnviaMail(ConfigurationManager.AppSettings["EmailFax"].ToString(), ("0" + txtDDD.Text + txtTel.Text), string.Empty);
                    }

                    break;
                case SuperPag.MultiViewPagFinal.FinalCorreio:
                    Objxml.ChildNodes[0].ChildNodes[0].ChildNodes[0].ChildNodes[0].ChildNodes[0].ChildNodes[2].Attributes["location"].Value = txtEnd.Text;
                    Objxml.ChildNodes[0].ChildNodes[0].ChildNodes[0].ChildNodes[0].ChildNodes[0].ChildNodes[2].Attributes["number"].Value = txtNum.Text;
                    Objxml.ChildNodes[0].ChildNodes[0].ChildNodes[0].ChildNodes[0].ChildNodes[0].ChildNodes[2].Attributes["complement"].Value = txtComplemento.Text;
                    Objxml.ChildNodes[0].ChildNodes[0].ChildNodes[0].ChildNodes[0].ChildNodes[0].ChildNodes[2].Attributes["district"].Value = txtBairro.Text;
                    Objxml.ChildNodes[0].ChildNodes[0].ChildNodes[0].ChildNodes[0].ChildNodes[0].ChildNodes[2].Attributes["postalCode"].Value = txtCEP.Text;
                    Objxml.ChildNodes[0].ChildNodes[0].ChildNodes[0].ChildNodes[0].ChildNodes[0].ChildNodes[2].Attributes["city"].Value = txtCidade.Text;
                    Objxml.ChildNodes[0].ChildNodes[0].ChildNodes[0].ChildNodes[0].ChildNodes[0].ChildNodes[2].Attributes["state"].Value = ddlEstado.SelectedValue;

                    Salvar(Objxml.InnerXml);
                    CreateArqCorreio();

                    break;
                case SuperPag.MultiViewPagFinal.FinalImpressao:
                    Salvar(Objxml.InnerXml);
                    GerarBoleto(true);
                    break;
            }
        }
        catch
        {
            CustomIPTE.IsValid = false;
            CustomIPTE.ErrorMessage = "Número de IPTE inválido.";
        }
    }

    private void EnviaMail(string Email, string Subject, string bodyMail)
    {
        //ENVIA O E-MAIL AO CLIENTE
        if (Helper.SendBoletoEmail(Email, ConfigurationManager.AppSettings["PathPDF"] + this.PaymentAttemptId + ".pdf", bodyMail, Subject, null))
        {
            //Exclui os boletos enviados
            foreach (string var in Directory.GetFiles(ConfigurationManager.AppSettings["PathPDF"].ToString()))
            {
                try
                {
                    File.Delete(var);
                }
                catch { }
            }
            Server.Transfer("~/Pagamento/Default.aspx");
        }
        else
        {
            //Atualiza o status do pagamento
            DPaymentAttemptBoleto ObjDPaymentAttemptBoleto = DataFactory.PaymentAttemptBoleto().Locate(new Guid(this.PaymentAttemptId));
            ObjDPaymentAttemptBoleto.Status = false;
            DataFactory.PaymentAttemptBoleto().Update(ObjDPaymentAttemptBoleto);

            CustomIPTE.IsValid = false;
            CustomIPTE.ErrorMessage = "Não foi possível enviar o e-mail.";
        }
    }

    /// <summary>
    /// Cria o arquivo para ser enviado para o carreio
    /// </summary>
    public void CreateArqCorreio()
    {
        StringBuilder ObjStringBuilder = new StringBuilder();
        XmlDocument Objxml = new XmlDocument();

        //Carrega o xml com os dados fixos do boleto
        Objxml.Load(Server.MapPath("xmlBoleto\\") + StoreKey + ".xml");

        for (int i = 0; i < Objxml.ChildNodes[0].ChildNodes.Count; i++)
        {
            ObjStringBuilder.Append("\"").Append(Objxml.ChildNodes[0].ChildNodes[i].Attributes["value"].Value).Append("\"").Append(",");
        }

        ObjStringBuilder.Append("\"").Append(Vencimento.Date.ToString("dd/MM/yyyy")).Append("\"").Append(",");//Dado "Vencimento"
        ObjStringBuilder.Append("\"").Append(DateTime.Now.Date.ToString("dd/MM/yyyy")).Append("\"").Append(",");//Dado "Data do Documento"
        ObjStringBuilder.Append("\"").Append(DateTime.Now.Date.ToString("dd/MM/yyyy")).Append("\"").Append(","); //Dado "Data do Processamento"
        ObjStringBuilder.Append("\"").Append(Valor.ToString()).Append("\"").Append(",");//Dado "Valor do Documento"
        ObjStringBuilder.Append("\"").Append(Valor.ToString()).Append("\"").Append(",");//Dado "Valor Cobrado"
        ObjStringBuilder.Append("\"").Append(this.txtSacado.Text).Append("\"").Append(",");//Dado "Sacado"
        ObjStringBuilder.Append("\"").Append(LimpaIPTE(txtIPTE.Text)).Append("\"").Append(",");//Dado "IPTE"

        //dados do endereço do Destinatário 
        ObjStringBuilder.Append("\"").Append(this.txtSacado.Text).Append("\"").Append(",");
        ObjStringBuilder.Append("\"").Append(txtEnd.Text).Append("\"").Append(",");
        ObjStringBuilder.Append("\"").Append(txtNum.Text).Append("\"").Append(",");
        ObjStringBuilder.Append("\"").Append(txtComplemento.Text).Append("\"").Append(",");
        ObjStringBuilder.Append("\"").Append(txtBairro.Text).Append("\"").Append(",");
        ObjStringBuilder.Append("\"").Append(txtCEP.Text).Append("\"").Append(",");
        ObjStringBuilder.Append("\"").Append(ddlEstado.SelectedItem.Text).Append("\"").Append(",");
        ObjStringBuilder.Append("\"").Append(txtCidade.Text).Append("\"").Append(",");

        //dados do endereço do Remetente 
        ObjStringBuilder.Append("\"").Append("66.129 - AC").Append("\"").Append(",");
        ObjStringBuilder.Append("\"").Append("São Paulo").Append("\"").Append(",");
        ObjStringBuilder.Append("\"").Append("05314-970").Append("\"").Append(",");

        StreamWriter ObjStreamWriter = new StreamWriter(ConfigurationManager.AppSettings["PathCorreio"] + "\\rem_" + DateTime.Now.ToString("dd_MM_yyyy") + ".txt", true, Encoding.Default);

        ObjStreamWriter.WriteLine(ObjStringBuilder.ToString());

        ObjStreamWriter.Close();

        Server.Transfer("~/Pagamento/Default.aspx");
    }

    /// <summary>
    /// Gera o boleto
    /// </summary>
    /// <param name="Imprimir"></param>
    private void GerarBoleto(bool Imprimir)
    {
        Report Objreport = new Report(new PdfFormatter());

        try
        {
            FontDef fd = new FontDef(Objreport, FontDef.StandardFont.Courier);//Fonte - Tipo
            FontProp fp = new FontPropMM(fd, 1.5);//Fonte - Tamanho
            FontProp fp1 = new FontPropMM(fd, 2);//Fonte - Tamanho
            Root.Reports.Page Objpage = new Root.Reports.Page(Objreport);

            System.IO.Stream streamboleto = Helper.GetImage("SuperPag.Image.boleto.JPG");
            Objpage.AddMM(2, 200, new RepImageMM(streamboleto, 203, 200));            

            //cria a imagem do codigo de barras
            Boleto.SaveBarcode(LimpaIPTE(IPTE), Server.MapPath("Barcode\\"));

            Objpage.AddMM(2, 210, new RepImageMM(Server.MapPath("Barcode\\") + LimpaIPTE(IPTE) + ".jpg", 100, 15));            

            //Posicionamento dos Dados no relatório (X, Y)
            //Objpage.AddMM(5, 19, new RepString(fp, "Investcred S/A"));
            Objpage.AddMM(8, 29, new RepString(fp, "PAGAVEL PREFERENCIALMENTE NAS LOJAS PONTO FRIO")); //"Local de Pagamento"
            Objpage.AddMM(163, 29, new RepString(fp, Vencimento.Date.ToString("dd/MM/yyyy"))); //Dado "Vencimento"
            Objpage.AddMM(8, 35, new RepString(fp, "BANCO INVESTCRED S/A")); //Dado "Cedente"
            Objpage.AddMM(163, 35, new RepString(fp, "001/00001.9")); //Dado "Agência/Código Cedente"
            Objpage.AddMM(8, 41, new RepString(fp, DateTime.Now.Date.ToString("dd/MM/yyyy"))); //Dado "Data do Documento"
            Objpage.AddMM(104, 41, new RepString(fp, "COM")); //Dado "Aceite"
            Objpage.AddMM(125, 41, new RepString(fp, DateTime.Now.Date.ToString("dd/MM/yyyy"))); //Dado "Data do Processamento"
            Objpage.AddMM(160, 41, new RepString(fp, "0107 380705 01 5")); //Dado "Cart. / Nº do Banco"
            Objpage.AddMM(66.5, 47.5, new RepString(fp, "REAL")); //Dado "Espécie"
            Objpage.AddMM(165, 47, new RepString(fp, Valor.ToString())); //Dado "Valor do Documento"
            Objpage.AddMM(10, 55, new RepString(fp, "Pagável em Rede Bancária.")); //Dado "Instruções, 1ª Linha"
            Objpage.AddMM(165, 53.5, new RepString(fp, "0,00")); //Dado "Desconto / Abatimento"
            Objpage.AddMM(10, 60, new RepString(fp, "Após vencimento, contatar: ")); //Dado "Instruções, 2ª Linha"
            Objpage.AddMM(10, 63, new RepString(fp, "4004-0199 para Capitais e Regiões Metropolitanas")); //Dado "Instruções, 2ª Linha"
            Objpage.AddMM(10, 66, new RepString(fp, "0800 – 7030199 para demais localidades")); //Dado "Instruções, 3ª Linha"
            Objpage.AddMM(165, 60, new RepString(fp, "0,00")); //Dado "Outras Deduções"
            Objpage.AddMM(165, 66, new RepString(fp, "0,00")); //Dado "Multa Mora"
            Objpage.AddMM(165, 73, new RepString(fp, "0,00")); //Dado "Outros Acrescimos"
            Objpage.AddMM(165, 79.5, new RepString(fp, Valor.ToString())); //Dado "Valor Cobrado"
            Objpage.AddMM(8, 87, new RepString(fp, this.txtSacado.Text)); //Dado "Sacado" 

            //Objpage.AddMM(5, 115, new RepString(fp, "Investcred S/A"));

            RepString obj = new RepString(fp1, this.IPTE);
            //obj.fontProp.bBold = true;
            Objpage.AddMM(69.5, 115.5, obj); //Dado "IPTE"
            Objpage.AddMM(8, 123, new RepString(fp, "PAGAVEL PREFERENCIALMENTE NAS LOJAS PONTO FRIO")); //"Local de Pagamento"
            Objpage.AddMM(163, 123, new RepString(fp, Vencimento.Date.ToString("dd/MM/yyyy"))); //Dado "Vencimento"
            Objpage.AddMM(8, 129, new RepString(fp, "BANCO INVESTCRED S/A")); //Dado "Cedente"
            Objpage.AddMM(163, 129, new RepString(fp, "001/00001.9")); //Dado "Agência/Código Cedente"
            Objpage.AddMM(6.5, 135, new RepString(fp, DateTime.Now.Date.ToString("dd/MM/yyyy"))); //Dado "Data do Documento"
            Objpage.AddMM(102.5, 135, new RepString(fp, "COM")); //Dado "Aceite"
            Objpage.AddMM(122.5, 135, new RepString(fp, DateTime.Now.Date.ToString("dd/MM/yyyy"))); //Dado "Data do Processamento"
            Objpage.AddMM(160, 135, new RepString(fp, "0107 380705 01 5")); //Dado "Cart. / Nº do Banco"
            Objpage.AddMM(65, 141, new RepString(fp, "REAL")); //Dado "Espécie - Valor da Moeda"
            Objpage.AddMM(165, 141, new RepString(fp, Valor.ToString())); //Dado "Valor do Documento"
            Objpage.AddMM(8, 149, new RepString(fp, "Pagável em Rede Bancária.")); //Dado "Instruções, 1ª Linha"
            Objpage.AddMM(8, 154, new RepString(fp, "Após vencimento, contatar: ")); //Dado "Instruções, 2ª Linha"
            Objpage.AddMM(8, 157, new RepString(fp, "4004-0199 para Capitais e Regiões Metropolitanas")); //Dado "Instruções, 2ª Linha"
            Objpage.AddMM(8, 160, new RepString(fp, "0800 – 7030199 para demais localidades")); //Dado "Instruções, 3ª Linha"
            Objpage.AddMM(165, 147.5, new RepString(fp, "0,00")); //Dado "Desconto / Abatimento"            
            Objpage.AddMM(165, 153.5, new RepString(fp, "0,00")); //Dado "Outras Deduções"
            Objpage.AddMM(165, 160, new RepString(fp, "0,00")); //Dado "Multa Mora"
            Objpage.AddMM(165, 167, new RepString(fp, "0,00")); //Dado "Outros Acrescimos"
            Objpage.AddMM(165, 173, new RepString(fp, Valor.ToString())); //Dado "Valor Cobrado"
            Objpage.AddMM(8, 181, new RepString(fp, this.txtSacado.Text)); //Dado "Sacado" 

            string[] strBodyMail = Regex.Split(BodyMail, "<br>");

            if (RadioEnvio.SelectedValue == "02")//adiciona a informação para fax
            {
                int cont = 0;
                foreach (string strMail in strBodyMail)
                {
                    cont += 5;

                    Objpage.AddMM(5, 220 + cont, new RepString(fp, strMail));
                }
            }

            if (Imprimir)
            {
                try
                {
                    RT.ResponsePDF(Objreport, this.Response); //Mostra o Boleto na tela  
                }
                catch { }
            }
            else
            {
                Objreport.Save(ConfigurationManager.AppSettings["PathPDF"] + this.PaymentAttemptId + ".pdf");//salva o arquivo para anexar ao email
            }
            Objreport = null;

        }
        catch (Exception ex)
        {
            SuperPag.Helper.GenericHelper.LogFile("pagamento::GerarBoleto PaymentAttemptId=" + this.PaymentAttemptId + " " + ex.ToString(), LogFileEntryType.Error);
            //Atualiza o status do pagamento para false
            DPaymentAttemptBoleto ObjDPaymentAttemptBoleto = DataFactory.PaymentAttemptBoleto().Locate(new Guid(this.PaymentAttemptId));
            ObjDPaymentAttemptBoleto.Status = false;
            DataFactory.PaymentAttemptBoleto().Update(ObjDPaymentAttemptBoleto);

            CustomIPTE.IsValid = false;
            CustomIPTE.ErrorMessage = "Número de IPTE inválido.";
        }
    }

    protected void btnCancelGeral_Click(object sender, EventArgs e)
    {
        Server.Transfer("../Pagamento/Default.aspx");
    }

    private string LimpaIPTE(string strIPTE)
    {
        strIPTE = strIPTE.Replace(" ", string.Empty);
        strIPTE = strIPTE.Replace(".", string.Empty);
        return strIPTE;
    }
}
