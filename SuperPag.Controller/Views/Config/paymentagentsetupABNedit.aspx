<%@ Page Language="C#" MasterPageFile="~/Default.master" AutoEventWireup="true" CodeFile="paymentagentsetupABNedit.aspx.cs" Inherits="Views_paymentagentsetupABNedit" %>
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
							<asp:label id="Label2" runat="server">Configurações do financiamento ABN</asp:label>
						</td>
					</tr>
                    <tr>
						<td class="tdLabelFRM">
							<asp:label id="Label1" runat="server">Código ABN</asp:label>
						</td>
						<td class="tdCampoFRM" align="left">
							<fwc:MsgTextBox id="MsgTextBox1" runat="server" MsgSource="MPaymentAgentSetupABN" MaxLength="10" Width="100px"
								InputMandatory="False" MsgTextField="CodigoABN" CustomFormat="None" InputFilter="None"></fwc:MsgTextBox>
						</td>
					</tr>
					 <tr>
						<td class="tdLabelFRM">
							<asp:label id="Label4" runat="server">Url Action</asp:label>
						</td>
						<td class="tdCampoFRM" align="left">
							<fwc:MsgTextBox id="MsgTextBox2" runat="server" MsgSource="MPaymentAgentSetupABN" MaxLength="300" Width="600px"
								InputMandatory="False" MsgTextField="UrlAction" CustomFormat="None" InputFilter="None"></fwc:MsgTextBox>
						</td>
					</tr>
					 <tr>
						<td class="tdLabelFRM">
							<asp:label id="Label5" runat="server">Url Consulta</asp:label>
						</td>
						<td class="tdCampoFRM" align="left">
							<fwc:MsgTextBox id="MsgTextBox3" runat="server" MsgSource="MPaymentAgentSetupABN" MaxLength="300" Width="600px"
								InputMandatory="False" MsgTextField="UrlConsulta" CustomFormat="None" InputFilter="None"></fwc:MsgTextBox>
						</td>
					</tr>
					 <tr>
						<td class="tdLabelFRM">
							<asp:label id="Label6" runat="server">Tipo Financiamento</asp:label>
						</td>
						<td class="tdCampoFRM" align="left">
							<fwc:MsgTextBox id="MsgTextBox4" runat="server" MsgSource="MPaymentAgentSetupABN" MaxLength="2" Width="20px"
								InputMandatory="False" MsgTextField="TipoFinanciamento" CustomFormat="None" InputFilter="None"></fwc:MsgTextBox>
						</td>
					</tr>
   				    <tr>
						<td class="tdLabelFRM">
							<asp:label id="Label3" runat="server">Tabela Financiamento</asp:label>
						</td>
						<td class="tdCampoFRM" align="left">
							<fwc:MsgTextBox id="MsgTextBox5" runat="server" MsgSource="MPaymentAgentSetupABN" MaxLength="5" Width="50px"
								InputMandatory="False" MsgTextField="TabelaFinanciamento" CustomFormat="None" InputFilter="None"></fwc:MsgTextBox>
						</td>
					</tr>
   				    <tr>
						<td class="tdLabelFRM">
							<asp:label id="Label7" runat="server">Garantia</asp:label>
						</td>
						<td class="tdCampoFRM" align="left">
							<fwc:MsgTextBox id="MsgTextBox6" runat="server" MsgSource="MPaymentAgentSetupABN" MaxLength="60" Width="160px"
								InputMandatory="False" MsgTextField="Garantia" CustomFormat="None" InputFilter="None"></fwc:MsgTextBox>
						</td>
					</tr>

				</table>
			</td>
		</tr>
	</table>
</asp:Content>
