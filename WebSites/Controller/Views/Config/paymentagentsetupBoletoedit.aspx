<%@ Page Language="C#" MasterPageFile="~/Default.master" AutoEventWireup="true" CodeFile="paymentagentsetupBoletoedit.aspx.cs" Inherits="Views_paymentagentsetupBoletoedit" %>
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
							<asp:label id="Label2" runat="server">Configurações do Boleto</asp:label>
						</td>
					</tr>
                    <tr>
						<td class="tdLabelFRM">
							<asp:label id="Label1" runat="server">Número do banco</asp:label>
						</td>
						<td class="tdCampoFRM" align="left">
							<fwc:MsgTextBox id="MsgTextBox1" runat="server" MsgSource="MPaymentAgentSetupBoleto" MaxLength="50" Width="160px"
								InputMandatory="False" MsgTextField="BankNumber" CustomFormat="NumInteiro" InputFilter="NumInteiro"></fwc:MsgTextBox>
						</td>
					</tr>
					 <tr>
						<td class="tdLabelFRM">
							<asp:label id="Label4" runat="server">Dígito Banco</asp:label>
						</td>
						<td class="tdCampoFRM" align="left">
							<fwc:MsgTextBox id="MsgTextBox2" runat="server" MsgSource="MPaymentAgentSetupBoleto" MaxLength="1" Width="20px"
								InputMandatory="False" MsgTextField="BankDigit" CustomFormat="None" InputFilter="None"></fwc:MsgTextBox>
						</td>
					</tr>
					 <tr>
						<td class="tdLabelFRM">
							<asp:label id="Label5" runat="server">Agência</asp:label>
						</td>
						<td class="tdCampoFRM" align="left">
							<fwc:MsgTextBox id="MsgTextBox3" runat="server" MsgSource="MPaymentAgentSetupBoleto" MaxLength="50" Width="160px"
								InputMandatory="False" MsgTextField="AgencyNumber" CustomFormat="None" InputFilter="None"></fwc:MsgTextBox>
						</td>
					</tr>
					 <tr>
						<td class="tdLabelFRM">
							<asp:label id="Label6" runat="server">Dígito agência</asp:label>
						</td>
						<td class="tdCampoFRM" align="left">
							<fwc:MsgTextBox id="MsgTextBox4" runat="server" MsgSource="MPaymentAgentSetupBoleto" MaxLength="1" Width="20px"
								InputMandatory="False" MsgTextField="AgencyDigit" CustomFormat="None" InputFilter="None"></fwc:MsgTextBox>
						</td>
					</tr>
                    <tr>
						<td class="tdLabelFRM">
							<asp:label id="Label7" runat="server">conta</asp:label>
						</td>
						<td class="tdCampoFRM" align="left">
							<fwc:MsgTextBox id="MsgTextBox6" runat="server" MsgSource="MPaymentAgentSetupBoleto" MaxLength="50" Width="160px"
								InputMandatory="False" MsgTextField="AccountNumber" CustomFormat="None" InputFilter="None"></fwc:MsgTextBox>
						</td>
					</tr>
                    <tr>
						<td class="tdLabelFRM">
							<asp:label id="Label8" runat="server">Digito conta</asp:label>
						</td>
						<td class="tdCampoFRM" align="left">
							<fwc:MsgTextBox id="MsgTextBox7" runat="server" MsgSource="MPaymentAgentSetupBoleto" MaxLength="50" Width="160px"
								InputMandatory="False" MsgTextField="AccountDigit" CustomFormat="None" InputFilter="None"></fwc:MsgTextBox>
						</td>
					</tr>
                    <tr>
						<td class="tdLabelFRM">
							<asp:label id="Label9" runat="server">Código cedente</asp:label>
						</td>
						<td class="tdCampoFRM" align="left">
							<fwc:MsgTextBox id="MsgTextBox8" runat="server" MsgSource="MPaymentAgentSetupBoleto" MaxLength="50" Width="160px"
								InputMandatory="False" MsgTextField="CederCode" CustomFormat="None" InputFilter="None"></fwc:MsgTextBox>
						</td>
					</tr>
                    <tr>
						<td class="tdLabelFRM">
							<asp:label id="Label10" runat="server">Nome cedente</asp:label>
						</td>
						<td class="tdCampoFRM" align="left">
							<fwc:MsgTextBox id="MsgTextBox9" runat="server" MsgSource="MPaymentAgentSetupBoleto" MaxLength="50" Width="400px"
								InputMandatory="False" MsgTextField="CederName" CustomFormat="None" InputFilter="None"></fwc:MsgTextBox>
						</td>
					</tr>
					 <tr>
						<td class="tdLabelFRM">
							<asp:label id="Label3" runat="server">Carteira</asp:label>
						</td>
						<td class="tdCampoFRM" align="left">
							<fwc:MsgTextBox id="MsgTextBox5" runat="server" MsgSource="MPaymentAgentSetupBoleto" MaxLength="50" Width="160px"
								InputMandatory="False" MsgTextField="Wallet" CustomFormat="None" InputFilter="None"></fwc:MsgTextBox>
						</td>
					</tr>
                    <tr>
						<td class="tdLabelFRM">
							<asp:label id="Label11" runat="server">Número convênio</asp:label>
						</td>
						<td class="tdCampoFRM" align="left">
							<fwc:MsgTextBox id="MsgTextBox10" runat="server" MsgSource="MPaymentAgentSetupBoleto" MaxLength="50" Width="160px"
								InputMandatory="False" MsgTextField="ConventionNumber" CustomFormat="None" InputFilter="None"></fwc:MsgTextBox>
						</td>
					</tr>
                     <tr>
						<td class="tdLabelFRM">
							<asp:label id="Label12" runat="server">Dias para cálculo da data vencimento</asp:label>
						</td>
						<td class="tdCampoFRM" align="left">
							<fwc:MsgTextBox id="MsgTextBox11" runat="server" MsgSource="MPaymentAgentSetupBoleto" MaxLength="3" Width="30px"
								InputMandatory="False" MsgTextField="ExpirationDays" CustomFormat="NumInteiro" InputFilter="NumInteiro"></fwc:MsgTextBox>
						</td>
					</tr>
				</table>
			</td>
		</tr>
	</table>
</asp:Content>
