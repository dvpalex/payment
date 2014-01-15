<%@ Page Language="C#" MasterPageFile="~/Default.master" AutoEventWireup="true" CodeFile="handshakeconfigurationedit.aspx.cs" Inherits="Views_handshakeconfigurationedit" %>
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
			<td class="tdBarraDeAcoesBAC">
				<fwc:eventbutton id="btnSave" runat="server" EventName="Save" text="Avançar" CausesValidation="true" OnClick="btnSave_Click"></fwc:eventbutton>
			</td>
			<td class="tdBarraDeAcoesBAC">
				<fwc:eventbutton id="btnCancel" runat="server" EventName="Cancel" text="Cancel" CausesValidation="false" OnClick="btnCancel_Click"></fwc:eventbutton>
			</td>
		</tr>
	</table>
	<table id="tbCentral">
		<tr>
			<td id="tdCentral">
				<table class="tbPainelFRM">
					<tr>
						<td class="tdBarraFerramentasSUM" colspan="4">
							<asp:label id="Label2" runat="server">Cadastro do Handshake</asp:label>
						</td>
					</tr>
                    <tr>
						<td class="tdLabelFRM">
							<asp:label id="Label1" runat="server">Código handshake</asp:label>
						</td>
						<td class="tdCampoFRM" align="left">
			   			      <fwc:MsgTextBox id="txtHandshakeConfigurationId" runat="server" MsgSource="MHandshakeConfiguration" MaxLength="20" Width="150px"
								InputMandatory="true" MsgTextField="HandshakeConfigurationId" CustomFormat="NumInteiro" NumInteiro="None"></fwc:MsgTextBox>
			     		</td>
					</tr>
					 <tr>
						<td class="tdLabelFRM">
							<asp:label id="Label3" runat="server">Tipo handshake</asp:label>
						</td>
						<td class="tdCampoFRM" align="left">
							<fwc:MsgDropDownList id="ddlTipoHandshake" runat="server" MsgSource="MHandshakeConfiguration" MsgListSourceType="Enumeration"
										MsgListItemsSource="HandshakeTypeEnum" MsgSourceField="HandshakeType"
										MsgSelectedValueField="HandshakeType" Width="256px" FirstBlank="True" InputMandatory="true"></fwc:MsgDropDownList>
						</td>
					</tr>
					 <tr>
						<td class="tdLabelFRM">
							<asp:label id="Label5" runat="server">Confirmar pagamento automaticamente</asp:label>
						</td>
						<td class="tdCampoFRM" align="left">
                            <fwc:MsgCheckBox id="MsgCheckBox2" runat="server" MsgSource="MHandshakeConfiguration" MsgCheckField="AutoPaymentConfirm"></fwc:MsgCheckBox>						
						</td>
					</tr>
					 <tr>
						<td class="tdLabelFRM">
							<asp:label id="Label6" runat="server">Aceitar pedido duplicado</asp:label>
						</td>
						<td class="tdCampoFRM" align="left">
                            <fwc:MsgCheckBox id="MsgCheckBox3" runat="server" MsgSource="MHandshakeConfiguration" MsgCheckField="AcceptDuplicateOrder"></fwc:MsgCheckBox>						
						</td>
					</tr>
					 <tr>
						<td class="tdLabelFRM">
							<asp:label id="Label7" runat="server">validar e-mail</asp:label>
						</td>
						<td class="tdCampoFRM" align="left">
                            <fwc:MsgCheckBox id="MsgCheckBox4" runat="server" MsgSource="MHandshakeConfiguration" MsgCheckField="ValidateEmail"></fwc:MsgCheckBox>						
						</td>
					</tr>
					 <tr>
						<td class="tdLabelFRM">
							<asp:label id="Label9" runat="server">Enviar e-mail para lojista</asp:label>
						</td>
						<td class="tdCampoFRM" align="left">
                            <fwc:MsgCheckBox id="MsgCheckBox5" runat="server" MsgSource="MHandshakeConfiguration" MsgCheckField="SendEmailStoreKeeper"></fwc:MsgCheckBox>						
						</td>
					</tr>
					 <tr>
						<td class="tdLabelFRM">
							<asp:label id="Label10" runat="server">Enviar e-mail para consumidor</asp:label>
						</td>
						<td class="tdCampoFRM" align="left">
                            <fwc:MsgCheckBox id="MsgCheckBox6" runat="server" MsgSource="MHandshakeConfiguration" MsgCheckField="SendEmailConsumer"></fwc:MsgCheckBox>						
						</td>
					</tr>
					 <tr>
						<td class="tdLabelFRM">
							<asp:label id="Label4" runat="server">Atribuir configuração à loja</asp:label>
						</td>
						<td class="tdCampoFRM" align="left">
							<fwc:MsgDropDownList id="ddlLoja" runat="server" MsgSource="MHandshakeConfiguration" MsgListSourceType="Message"
										MsgListItemsSource="MCStore" MsgListItemTextField="Name" MsgListItemValueField="StoreId" MsgSourceField="StoreId"
										MsgSelectedValueField="StoreId" Width="256px" FirstBlank="True" InputMandatory="true"></fwc:MsgDropDownList>
						</td>
					</tr>
				</table>
			</td>
		</tr>
	</table>
</asp:Content>
