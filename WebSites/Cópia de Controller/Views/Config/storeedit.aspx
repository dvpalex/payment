<%@ Page Language="C#" MasterPageFile="~/Default.master" AutoEventWireup="true" CodeFile="storeedit.aspx.cs" Inherits="Views_storeedit" %>
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
				<fwc:eventbutton id="btnSave" runat="server" EventName="Save" text="Salvar" CausesValidation="true" OnClick="btnSave_Click"></fwc:eventbutton>
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
							<asp:label id="Label2" runat="server">Configurações da loja</asp:label>
						</td>
					</tr>
                    <tr>
						<td class="tdLabelFRM">
							<asp:label id="Label1" runat="server">Código loja</asp:label>
						</td>
						<td class="tdCampoFRM" align="left">
							<fwc:MsgTextBox id="txtStoreId" runat="server" MsgSource="MStore" MaxLength="10" Width="80px"
								InputMandatory="True" MsgTextField="StoreId" CustomFormat="None" InputFilter="None"></fwc:MsgTextBox>
						</td>
					</tr>
					 <tr>
						<td class="tdLabelFRM">
							<asp:label id="Label4" runat="server">Nome</asp:label>
						</td>
						<td class="tdCampoFRM" align="left">
							<fwc:MsgTextBox id="MsgTextBox1" runat="server" MsgSource="MStore" MaxLength="50" Width="400px"
								InputMandatory="True" MsgTextField="Name" CustomFormat="None" InputFilter="None"></fwc:MsgTextBox>
						</td>
					</tr>
					<tr>
						<td class="tdLabelFRM">
							<asp:label id="Label3" runat="server">Url Site</asp:label>
						</td>
						<td class="tdCampoFRM" align="left">
							<fwc:MsgTextBox id="MsgTextBox2" runat="server" MsgSource="MStore" MaxLength="300" Width="400px"
								InputMandatory="False" MsgTextField="UrlSite" CustomFormat="None" InputFilter="None"></fwc:MsgTextBox>
						</td>
					</tr>
					<tr>
						<td class="tdLabelFRM">
							<asp:label id="Label5" runat="server">Chave Loja</asp:label>
						</td>
						<td class="tdCampoFRM" align="left">
							<fwc:MsgTextBox id="MsgTextBox4" runat="server" MsgSource="MStore" MaxLength="100" Width="200px"
								InputMandatory="true" MsgTextField="StoreKey" CustomFormat="None" InputFilter="None"></fwc:MsgTextBox>
						</td>
					</tr>
					<tr>
						<td class="tdLabelFRM">
							<asp:label id="Label6" runat="server">Senha</asp:label>
						</td>
						<td class="tdCampoFRM" align="left">
							<fwc:MsgTextBox id="MsgTextBox5" runat="server" MsgSource="MStore" MaxLength="100" Width="200px"
								InputMandatory="False" MsgTextField="Password" CustomFormat="None" InputFilter="None"></fwc:MsgTextBox>
						</td>
					</tr>
					<tr>
						<td class="tdLabelFRM">
							<asp:label id="Label7" runat="server">Remetente para envio de e-mail</asp:label>
						</td>
						<td class="tdCampoFRM" align="left">
							<fwc:MsgTextBox id="MsgTextBox6" runat="server" MsgSource="MStore" MaxLength="150" Width="400px"
								InputMandatory="False" MsgTextField="MailSenderEmail" CustomFormat="None" InputFilter="None"></fwc:MsgTextBox>
						</td>
					</tr>
					<tr>
						<td class="tdLabelFRM">
							<asp:label id="Label8" runat="server">Configuração de Handshake</asp:label>
						</td>
						<td class="tdCampoFRM" align="left">
							<fwc:MsgDropDownList id="ddlHandshake" runat="server" MsgSource="MStore" MsgListSourceType="Message"
										MsgListItemsSource="MCHandshakeConfiguration" MsgListItemTextField="HandshakeConfigurationId" MsgListItemValueField="HandshakeConfigurationId" MsgSourceField="HandshakeConfigurationId"
										MsgSelectedValueField="HandshakeConfigurationId" Width="256px" FirstBlank="True" InputMandatory="true"></fwc:MsgDropDownList>
						</td>
					</tr>
				</table>
			</td>
		</tr>
	</table>
</asp:Content>
