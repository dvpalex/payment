<%@ Page Language="C#" MasterPageFile="~/Default.master" AutoEventWireup="true" CodeFile="paymentagentsetupedit.aspx.cs" Inherits="Views_paymentagentsetupedit" %>
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
				<fwc:eventbutton id="btnSave" runat="server" EventName="Save" text="Continuar" CausesValidation="true" OnClick="btnSave_Click"></fwc:eventbutton>
			</td>
			<td class="tdBarraDeAcoesBAC">
				<fwc:eventbutton id="btnDelete" runat="server" EventName="Delete" text="Excluir" CausesValidation="false" OnClick="btnDelete_Click"></fwc:eventbutton>
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
							<asp:label id="Label2" runat="server">Setup de agente de Pagamento</asp:label>
						</td>
					</tr>
                    <tr>
						<td class="tdLabelFRM">
							<asp:label id="Label3" runat="server">Código</asp:label>
						</td>
						<td class="tdCampoFRM" align="left">
						    <fwc:MsgLabel id="lblPaymentAgentSetupId" runat="server" MsgSource="MPaymentAgentSetup" Width="160px"
								MsgTextField="PaymentAgentSetupId"></fwc:MsgLabel>
						</td>
					</tr>					
                    <tr>
						<td class="tdLabelFRM">
							<asp:label id="Label1" runat="server">Título</asp:label>
						</td>
						<td class="tdCampoFRM" align="left">
							<fwc:MsgTextBox id="MsgTextBox1" runat="server" MsgSource="MPaymentAgentSetup" MaxLength="50" Width="160px"
								InputMandatory="False" MsgTextField="Title" CustomFormat="None" InputFilter="None"></fwc:MsgTextBox>
						</td>
					</tr>
                    <tr>
						<td class="tdLabelFRM">
							<asp:label id="Label4" runat="server">Agente de pagamento</asp:label>
						</td>
						<td class="tdCampoFRM" align="left">
							<fwc:MsgDropDownList id="ddlPaymentAgent" runat="server" MsgSource="MPaymentAgentSetup" MsgListSourceType="Message"
										MsgListItemsSource="MCPaymentAgent" MsgListItemTextField="Name" MsgListItemValueField="PaymentAgentId" MsgSourceField="PaymentAgentId"
										MsgSelectedValueField="PaymentAgentId" Width="256px" FirstBlank="True"></fwc:MsgDropDownList>
						</td>
					</tr>
				</table>
			</td>
		</tr>
	</table>
</asp:Content>
