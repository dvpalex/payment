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
using SuperPag.Data.Messages;
using SuperPag.Data;
using SuperPag.Helper;
using SuperPag;
using System.IO;

public partial class Store_default_fillconsumer : System.Web.UI.UserControl
{
    public DConsumer Consumer;
    public DConsumerAddress BillingAddress;
    public DConsumerAddress DeliveryAddress;
    public List<string> regras;
    int storeId;
    string tipoAcao, linkBotao6, urlBotao6, urlBotao1;

    protected void Page_Load(object sender, EventArgs e)
    {
        UpdateHandshakeVars();
        DStore store = GenericHelper.CheckSessionStore(Context);
        storeId = store.storeId;

        if (Page.IsPostBack)
            return;

        Consumer = DataFactory.Consumer().Locate((long)HttpContext.Current.Session["consumerId"]);
        Ensure.IsNotNullPage(Consumer, "Código de Consumidor inválido.");
        BillingAddress = DataFactory.ConsumerAddress().Locate(Consumer.consumerId, (int)AddressTypes.Billing);
        DeliveryAddress = DataFactory.ConsumerAddress().Locate(Consumer.consumerId, (int)AddressTypes.Delivery);

        if ((CheckConsumer() && CheckBillingAddress() && CheckDeliveryAddress()))
        {
            GenericHelper.SetOrderStatus(Context, WorkflowOrderStatus.ConsumerFilled, null);
            Context.Response.Redirect("~/payment.aspx");
        }

        if (!CheckConsumer())
        {
            if (String.IsNullOrEmpty(Consumer.CNPJ))
                tblFisica.Visible = true;
            else
                tblJuridica.Visible = true;

            FillConsumerDetails();
            consumerDetails.Visible = true;
        } 




        if (!CheckBillingAddress())
        {
            tblEnderecoCobranca.Visible = true;
            
            if(!Ensure.IsNull(BillingAddress))
                FillBillingAddressDetails();
        }
        

        if (!CheckDeliveryAddress())
        {
            tblEnderecoEntrega.Visible = true;

            if (!Ensure.IsNull(DeliveryAddress))
                FillDeliveryAddressDetails();
        }

        
        if (regras.Contains("9"))
        {
            tblEnderecoEntrega.Visible = false;
            tblEnderecoCobranca.Visible = true;
        }

        if (regras.Contains("10"))
        {
            tblEnderecoEntrega.Visible = true;
            tblEnderecoCobranca.Visible = false;
        }

        if (tblEnderecoEntrega.Visible)
        {
            string pathEEI = "~/Store/" + storeId + "/enderecoEntrega.ascx";
            if (File.Exists(Server.MapPath(pathEEI)))
                plhEnderecoEntregaInstruction.Controls.Add(LoadControl(pathEEI));
        }
    }

    protected void ddlPessoa_SelectedIndexChanged(object sender, EventArgs e)
    {
        switch (ddlPessoa.SelectedValue)
        {
            case "1":
                tblFisica.Visible = false;
                tblJuridica.Visible = true;
                break;
            case "":
            case "2":
                tblFisica.Visible = true;
                tblJuridica.Visible = false;
                break;
        }
    }

    protected void btnNext_Click(object sender, ImageClickEventArgs e)
    {
        try
        {
            DConsumer dConsumer = DataFactory.Consumer().Locate((long)HttpContext.Current.Session["consumerId"]);
            DConsumerAddress dBillingAddress = DataFactory.ConsumerAddress().Locate(dConsumer.consumerId, (int)AddressTypes.Billing);
            DConsumerAddress dDeliveryAddress = DataFactory.ConsumerAddress().Locate(dConsumer.consumerId, (int)AddressTypes.Delivery);
            Ensure.IsNotNullPage(dConsumer, "Código de Consumidor inválido.");

            if (consumerDetails.Visible)
            {
                dConsumer.name = txtNome.Text;
                dConsumer.birthDate = GenericHelper.ParseDateddMMyyyy(txtNascimento.Text);
                dConsumer.civilState = ddlEstadoCivil.SelectedItem.Text;
                dConsumer.CPF = txtCPF.Text;
                dConsumer.ger = ddlSexo.SelectedValue.ToString();
                dConsumer.occupation = txtProfissao.Text;
                dConsumer.CNPJ = txtCNPJ.Text;
                dConsumer.IE = txtIE.Text;
                dConsumer.responsibleCPF = txtCPFResp.Text;
                dConsumer.responsibleName = txtNomeResp.Text;
                dConsumer.phone = txtTelefoneDDD.Text + txtTelefone.Text;
                dConsumer.fax = txtFaxDDD.Text + txtFax.Text;

                DataFactory.Consumer().Update(dConsumer);
            }

            if (tblEnderecoCobranca.Visible)
            {
                dBillingAddress.logradouro = ddlLogradouro.SelectedValue;
                dBillingAddress.address = txtEndereco.Text;
                dBillingAddress.addressNumber = txtNumero.Text;
                dBillingAddress.addressComplement = txtComplemento.Text;
                dBillingAddress.district = txtBairro.Text;
                dBillingAddress.city = txtCidade.Text;
                dBillingAddress.cep = txtCEP.Text + txtCEP2.Text;
                dBillingAddress.state = txtEstado.Text;
                dBillingAddress.country = ddlPais.SelectedValue;

                DataFactory.ConsumerAddress().Update(dBillingAddress);
            }
            else if (regras.Contains("10"))
            {
                dBillingAddress.logradouro = ddlLogradouroE.SelectedValue;
                dBillingAddress.address = txtEnderecoE.Text;
                dBillingAddress.addressNumber = txtNumeroE.Text;
                dBillingAddress.addressComplement = txtComplementoE.Text;
                dBillingAddress.district = txtBairroE.Text;
                dBillingAddress.city = txtCidadeE.Text;
                dBillingAddress.cep = txtCEPE.Text + txtCEPE2.Text;
                dBillingAddress.state = txtEstadoE.Text;
                dBillingAddress.country = ddlPaisE.SelectedValue;

                DataFactory.ConsumerAddress().Update(dBillingAddress);
            }

            if (tblEnderecoEntrega.Visible)
            {
                dDeliveryAddress.logradouro = ddlLogradouroE.SelectedValue;
                dDeliveryAddress.address = txtEnderecoE.Text;
                dDeliveryAddress.addressNumber = txtNumeroE.Text;
                dDeliveryAddress.addressComplement = txtComplementoE.Text;
                dDeliveryAddress.district = txtBairroE.Text;
                dDeliveryAddress.city = txtCidadeE.Text;
                dDeliveryAddress.cep = txtCEPE.Text + txtCEPE2.Text;
                dDeliveryAddress.state = txtEstadoE.Text;
                dDeliveryAddress.country = ddlPaisE.SelectedValue;

                DataFactory.ConsumerAddress().Update(dDeliveryAddress);
            }
            else if(regras.Contains("9"))
            {
                dDeliveryAddress.logradouro = ddlLogradouro.SelectedValue;
                dDeliveryAddress.address = txtEndereco.Text;
                dDeliveryAddress.addressNumber = txtNumero.Text;
                dDeliveryAddress.addressComplement = txtComplemento.Text;
                dDeliveryAddress.district = txtBairro.Text;
                dDeliveryAddress.city = txtCidade.Text;
                dDeliveryAddress.cep = txtCEP.Text + txtCEP2.Text;
                dDeliveryAddress.state = txtEstado.Text;
                dDeliveryAddress.country = ddlPais.SelectedValue;

                DataFactory.ConsumerAddress().Update(dDeliveryAddress);
            }
        }
        catch (Exception ex)
        {
            GenericHelper.LogFile("SuperPag::fillconsumer.ascx.cs::Store_default_fillconsumer.btnNext_Click " + ex.Message, LogFileEntryType.Error);
            GenericHelper.RedirectToErrorPage("Não foi possivel completar a informações do consumidor, por favor, volte e tente novamente");
        }

        GenericHelper.SetOrderStatus(Context, WorkflowOrderStatus.ConsumerFilled, null);
        Response.Redirect("~/payment.aspx");
    }

    public void FillConsumerDetails()
    {
        txtNome.Text = Consumer.name;
        txtCPF.Text = Consumer.CPF;
        txtCNPJ.Text = Consumer.CNPJ;
        txtCPFResp.Text = Consumer.responsibleCPF;
        txtNomeResp.Text = Consumer.responsibleName;
        txtRazaoSocial.Text = Consumer.name;
        txtIE.Text = Consumer.IE;
        txtNascimento.Text = Consumer.birthDate.ToString("dd/MM/yyyy");
        txtProfissao.Text = Consumer.occupation;
        if (!String.IsNullOrEmpty(Consumer.phone) && Consumer.phone.Length > 2)
        {
            txtTelefoneDDD.Text = Consumer.phone.Substring(0, 2);
            txtTelefone.Text = Consumer.phone.Substring(2);
        }
        if (!String.IsNullOrEmpty(Consumer.fax) && Consumer.fax.Length > 2)
        {
            txtFax.Text = Consumer.fax.Substring(0, 2);
            txtFaxDDD.Text = Consumer.fax.Substring(0);
        }
        txtEmail.Text = Consumer.email;

        if (!String.IsNullOrEmpty(Consumer.ger))
        {
            if (Consumer.ger == "M") ddlSexo.SelectedIndex = 1;
            else if (Consumer.ger == "F") ddlSexo.SelectedIndex = 2;
        }

        if (String.IsNullOrEmpty(Consumer.CNPJ))
            ddlPessoa.SelectedIndex = 0;
        else
            ddlPessoa.SelectedIndex = 1;
    }
    public void FillBillingAddressDetails()
    {
        ddlLogradouro.SelectedValue = BillingAddress.logradouro;
        txtEndereco.Text = BillingAddress.address;
        txtNumero.Text = BillingAddress.addressNumber;
        txtComplemento.Text = BillingAddress.addressComplement;
        txtBairro.Text = BillingAddress.district;
        if (!String.IsNullOrEmpty(BillingAddress.cep))
        {
            txtCEP.Text = BillingAddress.cep.Substring(0, 5);
            txtCEP2.Text = BillingAddress.cep.Substring(5, 3);
        }
        txtCidade.Text = BillingAddress.city;
        txtEstado.Text = BillingAddress.state.Trim();
        ddlPais.SelectedValue = BillingAddress.country;
    }
    public void FillDeliveryAddressDetails()
    {
        ddlLogradouroE.SelectedValue = DeliveryAddress.logradouro;
        txtEnderecoE.Text = DeliveryAddress.address;
        txtNumeroE.Text = DeliveryAddress.addressNumber;
        txtComplementoE.Text = DeliveryAddress.addressComplement;
        txtBairroE.Text = DeliveryAddress.district;
        if (!String.IsNullOrEmpty(DeliveryAddress.cep))
        {
            txtCEPE.Text = DeliveryAddress.cep.Substring(0, 5);
            txtCEPE2.Text = DeliveryAddress.cep.Substring(5, 3);
        }
        txtCidadeE.Text = DeliveryAddress.city;
        txtEstadoE.Text = DeliveryAddress.state.Trim();
        ddlPaisE.SelectedValue = DeliveryAddress.country;
    }

    public bool CheckConsumer()
    {
        DStore store = GenericHelper.CheckSessionStore(Context);
        DHandshakeConfiguration handshake = DataFactory.HandshakeConfiguration().Locate(store.handshakeConfigurationId);
        Ensure.IsNotNullPage(handshake, "A loja {0} não está configurada", store.storeId);

        if(!Ensure.IsNull(Consumer))
            return (!String.IsNullOrEmpty(Consumer.CNPJ) || !String.IsNullOrEmpty(Consumer.CPF)) && (!handshake.validateEmail || !String.IsNullOrEmpty(Consumer.email)) && !String.IsNullOrEmpty(Consumer.name);
        return false;
    }
    public bool CheckBillingAddress()
    {
        if (!Ensure.IsNull(BillingAddress))
            return !String.IsNullOrEmpty(BillingAddress.address) && !String.IsNullOrEmpty(BillingAddress.cep) && !String.IsNullOrEmpty(BillingAddress.city) && !String.IsNullOrEmpty(BillingAddress.state) && BillingAddress.state.Trim() != "";
        return false;
    }
    public bool CheckDeliveryAddress()
    {
        if (!Ensure.IsNull(DeliveryAddress))
            return !String.IsNullOrEmpty(DeliveryAddress.address) && !String.IsNullOrEmpty(DeliveryAddress.cep) && !String.IsNullOrEmpty(DeliveryAddress.city) && !String.IsNullOrEmpty(DeliveryAddress.state) && DeliveryAddress.state.Trim() != "";
        return false;
    }

    public void UpdateHandshakeVars()
    {
        //Seto as variaveis a serem usadas do handshake
        if (Session["htmlHandshake"] != null)
        {
            tipoAcao = HttpUtility.HtmlDecode(GenericHelper.GetSingleNodeString((string)Session["htmlHandshake"], "//tipo_acao"));
            urlBotao1 = HttpUtility.HtmlDecode(GenericHelper.GetSingleNodeString((string)Session["htmlHandshake"], "//urlbotao1"));
            linkBotao6 = HttpUtility.HtmlDecode(GenericHelper.GetSingleNodeString((string)Session["htmlHandshake"], "//linkbotao6"));
            urlBotao6 = HttpUtility.HtmlDecode(GenericHelper.GetSingleNodeString((string)Session["htmlHandshake"], "//urlbotao6"));
        }
        else
        {
            tipoAcao = HttpUtility.HtmlDecode(GenericHelper.GetSingleNodeString((string)Session["xmlHandshake"], "//tipo_acao"));
            urlBotao1 = HttpUtility.HtmlDecode(GenericHelper.GetSingleNodeString((string)Session["xmlHandshake"], "//urlbotao1"));
            linkBotao6 = HttpUtility.HtmlDecode(GenericHelper.GetSingleNodeString((string)Session["xmlHandshake"], "//link_botao6"));
            urlBotao6 = HttpUtility.HtmlDecode(GenericHelper.GetSingleNodeString((string)Session["xmlHandshake"], "//urlbotao6"));
        }

        //Obtenho as regras enviadas
        regras = new List<string>();
        if (tipoAcao.IndexOf(",") > 0)
        {
            foreach (string regra in tipoAcao.Split(",".ToCharArray()))
                regras.Add(regra);
        }
        else
            regras.Add(tipoAcao);
    }
    public void FillButtons()
    {
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
