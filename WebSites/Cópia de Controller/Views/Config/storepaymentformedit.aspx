<%@ Page Language="C#" MasterPageFile="~/Default.master" AutoEventWireup="true" CodeFile="storepaymentformedit.aspx.cs" Inherits="Views_storepaymentformedit" %>
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
							<asp:label id="Label2" runat="server">Configurações de meios de pagamento para loja</asp:label>
						</td>
					</tr>
                    <tr>
						<td class="tdLabelFRM">
							<asp:label id="Label5" runat="server">Loja</asp:label>
						</td>
						<td class="tdCampoFRM" align="left">
							<fwc:MsgLabel id="Msglabel5" runat="server" MsgSource="MStore" MsgTextField="Name"></fwc:MsgLabel>
						</td>
					</tr>
                    <tr>
						<td class="tdLabelFRM">
							<asp:label id="Label1" runat="server">Selecione o meio de pagamento</asp:label>
						</td>
						<td class="tdCampoFRM" align="left">
        					<fwc:MsgDropDownList id="ddlPaymentForm" runat="server" MsgSource="MStorePaymentForm" MsgListSourceType="Message"
								MsgListItemsSource="MCPaymentForm" MsgListItemTextField="Name" MsgListItemValueField="PaymentFormId" MsgSourceField="PaymentFormId"
								MsgSelectedValueField="PaymentFormId" Width="256px" FirstBlank="True" OnSelectedIndexChanged="ddlPaymentForm_SelectedIndexChanged" AutoPostBack="True" InputMandatory="True"></fwc:MsgDropDownList>
                            <fwc:MsgLabel ID="lblPaymentForm" runat="server" MsgSource="MStorePaymentForm" MsgTextField="PaymentForm.Name"></fwc:MsgLabel></td>
					</tr>
					 <tr>
						<td class="tdLabelFRM">
							<asp:label id="Label4" runat="server">Selecione a configuração do agente a ser utilizada</asp:label>
						</td>
						<td class="tdCampoFRM" align="left">
                        	<fwc:MsgDropDownList id="ddlPaymentAgentSetup" runat="server" MsgSource="MStorePaymentForm" MsgListSourceType="Message"
								MsgListItemsSource="MCPaymentAgentSetup" MsgListItemTextField="Title" MsgListItemValueField="PaymentAgentSetupId" MsgSourceField="PaymentAgentSetupId"
								MsgSelectedValueField="PaymentAgentSetupId" Width="256px" FirstBlank="True" InputMandatory="True"></fwc:MsgDropDownList>
						</td>
					</tr>
					 <tr>
						<td class="tdLabelFRM">
							<asp:label id="Label6" runat="server">Mostrar no combo</asp:label>
						</td>
						<td class="tdCampoFRM" align="left">
                            <fwc:MsgCheckBox id="MsgCheckBox2" runat="server" MsgSource="MStorePaymentForm" MsgCheckField="ShowInCombo" Width="152px"></fwc:MsgCheckBox>						
						</td>
					</tr>
					 <tr>
						<td class="tdLabelFRM">
							<asp:label id="Label7" runat="server">Usar valores teste</asp:label>
						</td>
						<td class="tdCampoFRM" align="left">
                            <fwc:MsgCheckBox id="MsgCheckBox3" runat="server" MsgSource="MStorePaymentForm" MsgCheckField="UseTestValues"></fwc:MsgCheckBox>						
						</td>
					</tr>
					 <tr>
						<td class="tdLabelFRM">
							<asp:label id="Label3" runat="server">Ativo</asp:label>
						</td>
						<td class="tdCampoFRM" align="left">
                            <fwc:MsgCheckBox id="MsgCheckBox1" runat="server" MsgSource="MStorePaymentForm" MsgCheckField="IsActive"></fwc:MsgCheckBox>						
						</td>
					</tr>
				</table>
			</td>
		</tr>
	</table>
</asp:Content>
