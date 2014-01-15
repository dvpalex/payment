<%@ Page Language="C#" MasterPageFile="~/Default.master" AutoEventWireup="true" CodeFile="handshakeconfigurationxmledit.aspx.cs" Inherits="Views_handshakeconfigurationxmledit" %>
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
				<fwc:eventbutton id="btnCancel" runat="server" EventName="Cancel" text="Cancelar" CausesValidation="false" OnClick="btnCancel_Click"></fwc:eventbutton>
			</td>
		</tr>
	</table>
	<table id="tbCentral">
		<tr>
			<td id="tdCentral">
				<table class="tbPainelFRM">
					<tr>
						<td class="tdBarraFerramentasSUM" colspan="4">
							<asp:label id="Label2" runat="server">Cadastro do Handshake XML</asp:label>
						</td>
					</tr>
                    <tr>
						<td class="tdLabelFRM">
							<asp:label id="Label1" runat="server">Código handshake</asp:label>
						</td>
						<td class="tdCampoFRM" align="left">
						    <fwc:MsgLabel id="lblPaymentAgentSetupId" runat="server" MsgSource="MHandshakeConfigurationXml" Width="100px"
								MsgTextField="HandshakeConfigurationId"></fwc:MsgLabel>
						</td>
					</tr>					
					 <tr>
						<td class="tdLabelFRM">
							<asp:label id="Label5" runat="server">Url Handshake</asp:label>
						</td>
						<td class="tdCampoFRM" align="left">
							<fwc:MsgTextBox id="MsgTextBox2" runat="server" MsgSource="MHandshakeConfigurationXml" MaxLength="300" Width="400px"
								InputMandatory="false" MsgTextField="UrlHandshake" CustomFormat="None" InputFilter="None"></fwc:MsgTextBox>
						</td>
					</tr>
					 <tr>
						<td class="tdLabelFRM">
							<asp:label id="Label3" runat="server">Url Erro Handshake</asp:label>
						</td>
						<td class="tdCampoFRM" align="left">
							<fwc:MsgTextBox id="MsgTextBox9" runat="server" MsgSource="MHandshakeConfigurationXml" MaxLength="300" Width="400px"
								InputMandatory="false" MsgTextField="UrlHandshakeError" CustomFormat="None" InputFilter="None"></fwc:MsgTextBox>
						</td>
					</tr>
                    <tr>
						<td class="tdLabelFRM">
							<asp:label id="Label4" runat="server">Url Finalização</asp:label>
						</td>
						<td class="tdCampoFRM" align="left">
							<fwc:MsgTextBox id="MsgTextBox1" runat="server" MsgSource="MHandshakeConfigurationXml" MaxLength="300" Width="400px"
								InputMandatory="false" MsgTextField="UrlFinalization" CustomFormat="None" InputFilter="None"></fwc:MsgTextBox>
						</td>
					</tr>
					 <tr>
						<td class="tdLabelFRM">
							<asp:label id="Label6" runat="server">Url Finalização Offline</asp:label>
						</td>
						<td class="tdCampoFRM" align="left">
							<fwc:MsgTextBox id="MsgTextBox3" runat="server" MsgSource="MHandshakeConfigurationXml" MaxLength="300" Width="400px"
								InputMandatory="false" MsgTextField="UrlFinalizationOffline" CustomFormat="None" InputFilter="None"></fwc:MsgTextBox>
						</td>
					</tr>
					 <tr>
						<td class="tdLabelFRM">
							<asp:label id="Label7" runat="server">Url Retorno</asp:label>
						</td>
						<td class="tdCampoFRM" align="left">
							<fwc:MsgTextBox id="MsgTextBox4" runat="server" MsgSource="MHandshakeConfigurationXml" MaxLength="300" Width="400px"
								InputMandatory="false" MsgTextField="UrlReturn" CustomFormat="None" InputFilter="None"></fwc:MsgTextBox>
						</td>
					</tr>
					 <tr>
						<td class="tdLabelFRM">
							<asp:label id="Label8" runat="server">Url confirmação pagamento</asp:label>
						</td>
						<td class="tdCampoFRM" align="left">
							<fwc:MsgTextBox id="MsgTextBox5" runat="server" MsgSource="MHandshakeConfigurationXml" MaxLength="300" Width="400px"
								InputMandatory="false" MsgTextField="UrlPaymentConfirmation" CustomFormat="None" InputFilter="None"></fwc:MsgTextBox>
						</td>
					</tr>
					 <tr>
						<td class="tdLabelFRM">
							<asp:label id="Label9" runat="server">Url confirmação Offline</asp:label>
						</td>
						<td class="tdCampoFRM" align="left">
							<fwc:MsgTextBox id="MsgTextBox6" runat="server" MsgSource="MHandshakeConfigurationXml" MaxLength="300" Width="400px"
								InputMandatory="false" MsgTextField="UrlPaymentConfirmationOffline" CustomFormat="None" InputFilter="None"></fwc:MsgTextBox>
						</td>
					</tr>
					 <tr>
						<td class="tdLabelFRM">
							<asp:label id="Label10" runat="server">Request Encoding</asp:label>
						</td>
						<td class="tdCampoFRM" align="left">
							<fwc:MsgTextBox id="MsgTextBox7" runat="server" MsgSource="MHandshakeConfigurationXml" MaxLength="50" Width="200px"
								InputMandatory="false" MsgTextField="RequestEncoding" CustomFormat="None" InputFilter="None"></fwc:MsgTextBox>
						</td>
					</tr>
					 <tr>
						<td class="tdLabelFRM">
							<asp:label id="Label11" runat="server">Response Encoding</asp:label>
						</td>
						<td class="tdCampoFRM" align="left">
							<fwc:MsgTextBox id="MsgTextBox8" runat="server" MsgSource="MHandshakeConfigurationXml" MaxLength="50" Width="200px"
								InputMandatory="false" MsgTextField="ResponseEncoding" CustomFormat="None" InputFilter="None"></fwc:MsgTextBox>
						</td>
					</tr>


				</table>
			</td>
		</tr>
	</table>
</asp:Content>
