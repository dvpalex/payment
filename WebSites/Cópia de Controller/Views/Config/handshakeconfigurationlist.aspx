<%@ Page Language="C#" MasterPageFile="~/Default.master" AutoEventWireup="true" CodeFile="handshakeconfigurationlist.aspx.cs" Inherits="Views_handshakeconfigurationlist" %>
<%@ Register TagPrefix="fwc" Namespace="SuperPag.Framework.Web.WebControls" Assembly="SuperPag.Framework" %>

<asp:Content ID="Content1" ContentPlaceHolderID="Conteudo" Runat="Server">
    <div id="SubTitulo">
        <asp:Image ID="imgHome" runat="server" SkinID="imgEmployee" />
        <asp:Label ID="lblNomeUsuario" runat="server" Text="Label"></asp:Label>    
    </div>
	<br />
	<!--Painel de Formulario-->
	<table class="tbBarraFerramentasSUM">
		<tr>
			<td class="tdBarraFerramentasSUM"><asp:Label id="Label1" runat="server">Listagem de transações</asp:Label></td>
    		<td class="tdBarraFerramentasAcaoSUM" style="height: 20px"><asp:LinkButton id="lnkIncluir" runat="server" OnClick="lnkIncluir_Click">Incluir configuração de handshake</asp:LinkButton></td>			
		</tr>
	</table>
	<fwc:MsgDataGrid runat="server" id="grdHandshakeConfiguration" MsgSource="MCHandshakeConfiguration" OnMessageEvent="grdHandshakeConfiguration_MessageEvent">
		<Columns>
			<fwc:EventColumn CustomFormat="None" Label="../../App_Themes/default/images/icon_ver.gif" ButtonType="Image" Event="ShowTransactionDetail" CustomTotalFormat="None" CausesValidation="False" Totalizar="False" HeaderText="Detalhes"></fwc:EventColumn>
			<fwc:MessageColumn CustomFormat="None" MaxLength="0" id="emcHandshakeConfigurationId" Field="HandshakeConfigurationId" CustomTotalFormat="None" Totalizar="False" HeaderText="Código" HelpAction=""></fwc:MessageColumn>
			<fwc:MessageColumn CustomFormat="None" MaxLength="0" id="emcHandshakeType" Field="HandshakeType" CustomTotalFormat="None" Totalizar="False" HeaderText="Tipo" HelpAction=""></fwc:MessageColumn>
			<fwc:CheckBoxColumn id="emcAutoPaymentConfirm" CheckedField="AutoPaymentConfirm" HeaderText="Auto Confirma" Enabled="false" ></fwc:CheckBoxColumn>
			<fwc:CheckBoxColumn id="emcAcceptDuplicateOrder" CheckedField="AcceptDuplicateOrder" HeaderText="Pedido duplicado" Enabled="false" ></fwc:CheckBoxColumn>
			<fwc:CheckBoxColumn id="emcValidateEmail" CheckedField="ValidateEmail" HeaderText="Valida e-mail" Enabled="false" ></fwc:CheckBoxColumn>
			<fwc:CheckBoxColumn id="emcSendEmailStoreKeeper" CheckedField="SendEmailStoreKeeper" HeaderText="Envia e-mail lojista" Enabled="false" ></fwc:CheckBoxColumn>
			<fwc:CheckBoxColumn id="emcSendEmailConsumer" CheckedField="SendEmailConsumer" HeaderText="Envia e-mail consumidor" Enabled="false" ></fwc:CheckBoxColumn>
			<fwc:MessageColumn CustomFormat="DataHora" MaxLength="0" id="emcCreationDate" Field="CreationDate" CustomTotalFormat="None" Totalizar="False" HeaderText="Data cadastro" HelpAction=""></fwc:MessageColumn>
			<fwc:MessageColumn CustomFormat="DataHora" MaxLength="0" id="emcLastUpdate" Field="LastUpdate" CustomTotalFormat="None" Totalizar="False" HeaderText="Data alteração" HelpAction=""></fwc:MessageColumn>
		</Columns>
	</fwc:MsgDataGrid>
</asp:Content>
