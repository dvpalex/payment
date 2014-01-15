<%@ Page Language="C#" MasterPageFile="~/Default.master" AutoEventWireup="true" CodeFile="storedetail.aspx.cs" Inherits="Views_storedetail" %>
<%@ Register TagPrefix="fwc" Namespace="SuperPag.Framework.Web.WebControls" Assembly="SuperPag.Framework" %>

<asp:Content ID="Content1" ContentPlaceHolderID="Conteudo" Runat="Server">
    <div id="SubTitulo">
        <asp:Image ID="imgHome" runat="server" SkinID="imgEmployee" />
        <asp:Label ID="lblNomeUsuario" runat="server" Text="Label"></asp:Label>    
    </div>
	<br />
	<!--Painel de Sumario-->
	<table class="tbBarraDeAcoesBAC">
		<tr>
			<td class="tdBarraDeAcoesBAC"><fwc:EventButton id="btnGoBack" runat="server" EventName="GoBack" text="Voltar" CausesValidation="false" OnClick="btnGoBack_Click"></fwc:EventButton></td>
		</tr>
	</table>

	<table class="tbBarraFerramentasSUM">
		<tr>
			<td class="tdBarraFerramentasSUM"><asp:Label id="lblTituloSumario_1" runat="server">Informações da pedido</asp:Label></td>
    		<td class="tdBarraFerramentasAcaoSUM" style="height: 20px"><asp:LinkButton id="LinkButton1" runat="server" OnClick="lnkEdit_Click">Editar</asp:LinkButton></td>			
		</tr>
	</table>
	<table class="tbPainelSUM">
		<tr>
			<td class="tdLabelSUM"><asp:Label id="Label2" runat="server">Código Loja</asp:Label></td>
			<td class="tdValorSUM" colspan="2"><fwc:MsgLabel id="Msglabel17" runat="server" MsgSource="MStore" MsgTextField="StoreId"></fwc:MsgLabel></td>
		</tr>
		<tr>
			<td class="tdLabelSUM"><asp:Label id="Label5" runat="server">Nome Loja</asp:Label></td>
			<td class="tdValorSUM" colspan="2"><fwc:MsgLabel id="Msglabel5" runat="server" MsgSource="MStore" MsgTextField="Name"></fwc:MsgLabel></td>
		</tr>
        <tr>
            <td class="tdLabelSUM">
                <asp:Label ID="Label54" runat="server">Url Site</asp:Label></td>
            <td class="tdValorSUM" colspan="2">
                <fwc:MsgLabel ID="MsgLabel54" runat="server" MsgSource="MStore" MsgTextField="UrlSite"></fwc:MsgLabel></td>
        </tr>
		<tr>
			<td class="tdLabelSUM">
                <asp:Label ID="Label1" runat="server">Chave loja</asp:Label></td>
			<td class="tdValorSUM" colspan="2"><fwc:MsgLabel id="Msglabel9" runat="server" MsgSource="MStore" MsgTextField="StoreKey"></fwc:MsgLabel></td>
		</tr>
		<tr>
			<td class="tdLabelSUM"><asp:Label id="Label14" runat="server">Senha</asp:Label></td>
			<td class="tdValorSUM" colspan="2"><fwc:MsgLabel id="Msglabel13" runat="server" MsgSource="MStore" MsgTextField="Password"></fwc:MsgLabel></td>
		</tr>
		<tr>
			<td class="tdLabelSUM"><asp:Label id="Label6" runat="server">E-mail remetente para e-mails</asp:Label></td>
			<td class="tdValorSUM" colspan="2"><fwc:MsgLabel id="Msglabel3" runat="server" MsgSource="MStore" MsgTextField="MailSenderEmail"></fwc:MsgLabel></td>
		</tr>
		<tr>
			<td class="tdLabelSUM"><asp:Label id="Label3" runat="server">Data cadastro</asp:Label></td>
			<td class="tdValorSUM" colspan="2"><fwc:MsgLabel id="Msglabel1" runat="server" MsgSource="MStore" MsgTextField="CreationDate"></fwc:MsgLabel></td>
		</tr>
		<tr>
			<td class="tdLabelSUM"><asp:Label id="Label4" runat="server">Data última alteração</asp:Label></td>
			<td class="tdValorSUM" colspan="2"><fwc:MsgLabel id="Msglabel2" runat="server" MsgSource="MStore" MsgTextField="LastUpdate"></fwc:MsgLabel></td>
		</tr>		
	</table>
	<table class="tbBarraFerramentasSUM">
		<tr>
			<td class="tdBarraFerramentasSUM"><asp:Label id="Label8" runat="server">Handshake utilizado pela loja</asp:Label></td>
		</tr>
	</table>
	<table class="tbPainelSUM">
		<tr>
			<td class="tdLabelSUM"><asp:Label id="Label10" runat="server">Código Handshake</asp:Label></td>
			<td class="tdValorSUM" colspan="2"><fwc:MsgLabel id="Msglabel4" runat="server" MsgSource="MStore" MsgTextField="HandshakeConfiguration.HandshakeConfigurationId"></fwc:MsgLabel></td>
			<td class="tdLabelSUM"><asp:Label id="Label11" runat="server">Tipo</asp:Label></td>
			<td class="tdValorSUM" colspan="2"><fwc:MsgLabel id="Msglabel6" runat="server" MsgSource="MStore" MsgTextField="HandshakeConfiguration.HandshakeType"></fwc:MsgLabel></td>
		</tr>
	</table>
	<!--Painel de Registros de Funcionarios-->
	<table class="tbBarraFerramentasSUM">
		<tr>
			<td class="tdBarraFerramentasSUM"><asp:Label id="Label9" runat="server">Formas de pagamento selecionadas para a loja</asp:Label></td>
    		<td class="tdBarraFerramentasAcaoSUM" style="height: 20px"><asp:LinkButton id="lnkIncluir" runat="server" OnClick="lnkIncluir_Click">Incluir forma de pagamento</asp:LinkButton></td>			
		</tr>
	</table>
    <fwc:MsgDataGrid id="grdStorePaymentForms" ShowFooter="False" runat="server" MsgSource="MStore" MsgSourceField="StorePaymentForms" OnMessageEvent="grdStorePaymentForms_MessageEvent">
		<Columns>
		    <fwc:EventColumn CustomFormat="None" Label="../../App_Themes/default/images/detalhes.gif" ButtonType="Image" Event="ShowStorePaymentInstallments" CustomTotalFormat="None" CausesValidation="False" Totalizar="False" HeaderText="Parcelamentos"></fwc:EventColumn>
		    <fwc:EventColumn CustomFormat="None" Label="../../App_Themes/default/images/icon_edit.gif" ButtonType="Image" Event="EditStorePaymentForm" CustomTotalFormat="None" CausesValidation="False" Totalizar="False" HeaderText="Editar"></fwc:EventColumn>
			<fwc:MessageColumn CustomFormat="None" MaxLength="0" id="emcPaymentFormName" Field="PaymentForm.Name" CustomTotalFormat="None" Totalizar="False" HeaderText="Forma de pagamento" HelpAction=""></fwc:MessageColumn>
			<fwc:MessageColumn CustomFormat="None" MaxLength="0" id="emcPaymentAgentSetupId" Field="PaymentAgentSetup.PaymentAgentSetupId" CustomTotalFormat="None" Totalizar="False" HeaderText="Código config. do agente" HelpAction=""></fwc:MessageColumn>
			<fwc:MessageColumn CustomFormat="None" MaxLength="0" id="emcPaymentAgentSetupTitle" Field="PaymentAgentSetup.Title" CustomTotalFormat="None" Totalizar="False" HeaderText="Título config. do agente" HelpAction=""></fwc:MessageColumn>
			<fwc:CheckBoxColumn id="emcShowInCombo" CheckedField="ShowInCombo" HeaderText="Mostrar no Combo" Enabled="false" ></fwc:CheckBoxColumn>
			<fwc:CheckBoxColumn id="emcUseTestValues" CheckedField="UseTestValues" HeaderText="Usar valores teste" Enabled="false" ></fwc:CheckBoxColumn>
			<fwc:CheckBoxColumn id="emcStatus" CheckedField="IsActive" HeaderText="Ativo" Enabled="false" ></fwc:CheckBoxColumn>
		</Columns>
    </fwc:MsgDataGrid>
  </asp:Content>
