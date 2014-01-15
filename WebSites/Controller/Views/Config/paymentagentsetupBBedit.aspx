<%@ Page Language="C#" MasterPageFile="~/Default.master" AutoEventWireup="true" CodeFile="paymentagentsetupBBedit.aspx.cs" Inherits="Views_paymentagentsetupBBedit" %>
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
							<asp:label id="Label2" runat="server">Configurações do Pagamento Banco Brasil</asp:label>
						</td>
					</tr>
                    <tr>
						<td class="tdLabelFRM">
							<asp:label id="Label1" runat="server">Contador de referência do agente</asp:label>
						</td>
						<td class="tdCampoFRM" align="left">
							<fwc:MsgTextBox id="MsgTextBox1" runat="server" MsgSource="MPaymentAgentSetupBB" MaxLength="50" Width="160px"
								InputMandatory="False" MsgTextField="AgentOrderReferenceCounter" CustomFormat="None" InputFilter="None"></fwc:MsgTextBox>
						</td>
					</tr>
					 <tr>
						<td class="tdLabelFRM">
							<asp:label id="Label4" runat="server">Número convênio</asp:label>
						</td>
						<td class="tdCampoFRM" align="left">
							<fwc:MsgTextBox id="MsgTextBox2" runat="server" MsgSource="MPaymentAgentSetupBB" MaxLength="50" Width="160px"
								InputMandatory="False" MsgTextField="BusinessNumber" CustomFormat="None" InputFilter="None"></fwc:MsgTextBox>
						</td>
					</tr>
					 <tr>
						<td class="tdLabelFRM">
							<asp:label id="Label5" runat="server">Dias para cálculo da data de expiração</asp:label>
						</td>
						<td class="tdCampoFRM" align="left">
							<fwc:MsgTextBox id="MsgTextBox3" runat="server" MsgSource="MPaymentAgentSetupBB" MaxLength="50" Width="160px"
								InputMandatory="False" MsgTextField="DaysExpirationDate" CustomFormat="NumInteiro" InputFilter="NumInteiro"></fwc:MsgTextBox>
						</td>
					</tr>
					 <tr>
						<td class="tdLabelFRM">
							<asp:label id="Label6" runat="server">Url BBPag</asp:label>
						</td>
						<td class="tdCampoFRM" align="left">
							<fwc:MsgTextBox id="MsgTextBox4" runat="server" MsgSource="MPaymentAgentSetupBB" MaxLength="50" Width="160px"
								InputMandatory="False" MsgTextField="UrlBBPag" CustomFormat="None" InputFilter="None"></fwc:MsgTextBox>
						</td>
					</tr>
					   				    <tr>
						<td class="tdLabelFRM">
							<asp:label id="Label3" runat="server">Url Sonda BBPag</asp:label>
						</td>
						<td class="tdCampoFRM" align="left">
							<fwc:MsgTextBox id="MsgTextBox5" runat="server" MsgSource="MPaymentAgentSetupBB" MaxLength="50" Width="160px"
								InputMandatory="False" MsgTextField="UrlBBPagSonda" CustomFormat="None" InputFilter="None"></fwc:MsgTextBox>
						</td>
					</tr>

				</table>
			</td>
		</tr>
	</table>
</asp:Content>
