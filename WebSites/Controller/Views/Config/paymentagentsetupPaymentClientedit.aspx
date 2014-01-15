<%@ Page Language="C#" MasterPageFile="~/Default.master" AutoEventWireup="true" CodeFile="paymentagentsetupPaymentClientedit.aspx.cs" Inherits="Views_paymentagentsetupPaymentClientedit" %>
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
							<asp:label id="Label2" runat="server">Configurações do Payment Virtual Client (AMEX)</asp:label>
						</td>
					</tr>
                    <tr>
						<td class="tdLabelFRM">
							<asp:label id="Label1" runat="server">Código de acesso</asp:label>
						</td>
						<td class="tdCampoFRM" align="left">
							<fwc:MsgTextBox id="MsgTextBox1" runat="server" MsgSource="MPaymentAgentSetupPaymentClientVirtual" MaxLength="50" Width="160px"
								InputMandatory="False" MsgTextField="AccessCode" CustomFormat="None" InputFilter="None"></fwc:MsgTextBox>
						</td>
					</tr>
					 <tr>
						<td class="tdLabelFRM">
							<asp:label id="Label4" runat="server">Código de segurança</asp:label>
						</td>
						<td class="tdCampoFRM" align="left">
							<fwc:MsgTextBox id="MsgTextBox2" runat="server" MsgSource="MPaymentAgentSetupPaymentClientVirtual" MaxLength="50" Width="160px"
								InputMandatory="False" MsgTextField="SecureHashSecret" CustomFormat="None" InputFilter="None"></fwc:MsgTextBox>
						</td>
					</tr>
					 <tr>
						<td class="tdLabelFRM">
							<asp:label id="Label5" runat="server">Identificação lojista</asp:label>
						</td>
						<td class="tdCampoFRM" align="left">
							<fwc:MsgTextBox id="MsgTextBox3" runat="server" MsgSource="MPaymentAgentSetupPaymentClientVirtual" MaxLength="50" Width="160px"
								InputMandatory="False" MsgTextField="MerchantId" CustomFormat="None" InputFilter="None"></fwc:MsgTextBox>
						</td>
					</tr>
					 <tr>
						<td class="tdLabelFRM">
							<asp:label id="Label6" runat="server">Checar AVS</asp:label>
						</td>
						<td class="tdCampoFRM" align="left">
                            <fwc:MsgCheckBox id="MsgCheckBox1" runat="server" MsgSource="MPaymentAgentSetupPaymentClientVirtual" MsgCheckField="CheckAVS"></fwc:MsgCheckBox>						
                        </td>
					</tr>
					<tr>
						<td class="tdLabelFRM">
							<asp:label id="Label3" runat="server">Versão</asp:label>
						</td>
						<td class="tdCampoFRM" align="left">
							<fwc:MsgTextBox id="MsgTextBox5" runat="server" MsgSource="MPaymentAgentSetupPaymentClientVirtual" MaxLength="50" Width="160px"
								InputMandatory="False" MsgTextField="Version" CustomFormat="None" InputFilter="None"></fwc:MsgTextBox>
						</td>
					</tr>
   				    <tr>
						<td class="tdLabelFRM">
							<asp:label id="Label7" runat="server">Usuário captura</asp:label>
						</td>
						<td class="tdCampoFRM" align="left">
							<fwc:MsgTextBox id="MsgTextBox6" runat="server" MsgSource="MPaymentAgentSetupPaymentClientVirtual" MaxLength="50" Width="160px"
								InputMandatory="False" MsgTextField="CaptureUser" CustomFormat="None" InputFilter="None"></fwc:MsgTextBox>
						</td>
					</tr>
   				    <tr>
						<td class="tdLabelFRM">
							<asp:label id="Label8" runat="server">Senha captura</asp:label>
						</td>
						<td class="tdCampoFRM" align="left">
							<fwc:MsgTextBox id="MsgTextBox7" runat="server" MsgSource="MPaymentAgentSetupPaymentClientVirtual" MaxLength="50" Width="160px"
								InputMandatory="False" MsgTextField="CapturePassword" CustomFormat="None" InputFilter="None"></fwc:MsgTextBox>
						</td>
					</tr>
   				    <tr>
						<td class="tdLabelFRM">
							<asp:label id="Label9" runat="server">AutoCapture</asp:label>
						</td>
						<td class="tdCampoFRM" align="left">
                            <fwc:MsgCheckBox id="chbAutoCapture" runat="server" MsgSource="MPaymentAgentSetupPaymentClientVirtual" MsgCheckField="AutoCapture"></fwc:MsgCheckBox>						
                        </td>
					</tr>
				</table>
			</td>
		</tr>
	</table>
</asp:Content>
