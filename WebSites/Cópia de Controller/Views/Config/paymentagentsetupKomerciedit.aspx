<%@ Page Language="C#" MasterPageFile="~/Default.master" AutoEventWireup="true" CodeFile="paymentagentsetupKomerciedit.aspx.cs" Inherits="Views_paymentagentsetupKormeciedit" %>
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
							<asp:label id="Label2" runat="server">Configurações do Komerci</asp:label>
						</td>
					</tr>
                    <tr>
						<td class="tdLabelFRM">
							<asp:label id="Label1" runat="server">Número convênio</asp:label>
						</td>
						<td class="tdCampoFRM" align="left">
							<fwc:MsgTextBox id="MsgTextBox1" runat="server" MsgSource="MPaymentAgentSetupKomerci" MaxLength="20" Width="100px"
								InputMandatory="False" MsgTextField="BusinessNumber" CustomFormat="None" InputFilter="None"></fwc:MsgTextBox>
						</td>
					</tr>
					 <tr>
						<td class="tdLabelFRM">
							<asp:label id="Label4" runat="server">Tipo parcelamento</asp:label>
						</td>
						<td class="tdCampoFRM" align="left">
							<fwc:MsgTextBox id="MsgTextBox2" runat="server" MsgSource="MPaymentAgentSetupKomerci" MaxLength="2" Width="30px"
								InputMandatory="False" MsgTextField="InstalmentType" CustomFormat="None" InputFilter="None"></fwc:MsgTextBox>
						</td>
					</tr>
					 <tr>
						<td class="tdLabelFRM">
							<asp:label id="Label5" runat="server">Url Komerci</asp:label>
						</td>
						<td class="tdCampoFRM" align="left">
							<fwc:MsgTextBox id="MsgTextBox3" runat="server" MsgSource="MPaymentAgentSetupKomerci" MaxLength="100" Width="400px"
								InputMandatory="False" MsgTextField="UrlKomerci" CustomFormat="None" InputFilter="None"></fwc:MsgTextBox>
						</td>
					</tr>
					 <tr>
						<td class="tdLabelFRM">
							<asp:label id="Label6" runat="server">Url Confirmação Komerci</asp:label>
						</td>
						<td class="tdCampoFRM" align="left">
							<fwc:MsgTextBox id="MsgTextBox4" runat="server" MsgSource="MPaymentAgentSetupKomerci" MaxLength="100" Width="400px"
								InputMandatory="False" MsgTextField="UrlKomerciConfirm" CustomFormat="None" InputFilter="None"></fwc:MsgTextBox>
						</td>
					</tr>
   				    <tr>
						<td class="tdLabelFRM">
							<asp:label id="Label3" runat="server">Url Komerci AVS</asp:label>
						</td>
						<td class="tdCampoFRM" align="left">
							<fwc:MsgTextBox id="MsgTextBox5" runat="server" MsgSource="MPaymentAgentSetupKomerci" MaxLength="100" Width="400px"
								InputMandatory="False" MsgTextField="UrlKomerciAVS" CustomFormat="None" InputFilter="None"></fwc:MsgTextBox>
						</td>
					</tr>
					<tr>
						<td class="tdLabelFRM">
							<asp:label id="Label7" runat="server">Consulta AVS</asp:label>
						</td>
						<td class="tdCampoFRM" align="left">
                            <fwc:MsgCheckBox id="chbConsultaAVS" runat="server" MsgSource="MPaymentAgentSetupKomerci" MsgCheckField="CheckAVS"></fwc:MsgCheckBox>						
						</td>
					</tr>
				</table>
			</td>
		</tr>
	</table>
</asp:Content>
