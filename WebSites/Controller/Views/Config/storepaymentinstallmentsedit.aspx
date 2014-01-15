<%@ Page Language="C#" MasterPageFile="~/Default.master" AutoEventWireup="true" CodeFile="storepaymentinstallmentsedit.aspx.cs" Inherits="Views_storepaymentinstallmentedit" %>
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
							<asp:label id="Label2" runat="server">Cadastro de parcelamento</asp:label>
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
							<asp:label id="Label1" runat="server">Quantidade parcelas</asp:label>
						</td>
						<td class="tdCampoFRM" align="left">
			            	<fwc:MsgTextBox id="txtInstallmentNumber" runat="server" MsgSource="MStorePaymentInstallment" MaxLength="3" Width="50px"
								InputMandatory="True" MsgTextField="InstallmentNumber" CustomFormat="NumInteiro" InputFilter="NumInteiro"></fwc:MsgTextBox>
			            </td>
					</tr>
					 <tr>
						<td class="tdLabelFRM">
							<asp:label id="Label4" runat="server">Descrição</asp:label>
						</td>
						<td class="tdCampoFRM" align="left">
            				<fwc:MsgTextBox id="MsgTextBox2" runat="server" MsgSource="MStorePaymentInstallment" MaxLength="50" Width="400px"
								InputMandatory="false" MsgTextField="Description" CustomFormat="None" InputFilter="None"></fwc:MsgTextBox>
						</td>
					</tr>
					 <tr>
						<td class="tdLabelFRM">
							<asp:label id="Label3" runat="server">Valor mínimo</asp:label>
						</td>
						<td class="tdCampoFRM" align="left">
            				<fwc:MsgTextBox id="MsgTextBox3" runat="server" MsgSource="MStorePaymentInstallment" MaxLength="30" Width="150px"
								InputMandatory="True" MsgTextField="MinValue" CustomFormat="NumDecimal_2casa" InputFilter="NumDecimal_2casa"></fwc:MsgTextBox>
						</td>
					</tr>
    			    <tr>
						<td class="tdLabelFRM">
							<asp:label id="Label6" runat="server">Valor máximo</asp:label>
						</td>
						<td class="tdCampoFRM" align="left">
            				<fwc:MsgTextBox id="MsgTextBox4" runat="server" MsgSource="MStorePaymentInstallment" MaxLength="30" Width="150px"
								InputMandatory="True" MsgTextField="MaxValue" CustomFormat="NumDecimal_2casa" InputFilter="NumDecimal_2casa"></fwc:MsgTextBox>
						</td>
					</tr>
    			    <tr>
						<td class="tdLabelFRM">
							<asp:label id="Label7" runat="server">Juros</asp:label>
						</td>
						<td class="tdCampoFRM" align="left">
            				<fwc:MsgTextBox id="MsgTextBox5" runat="server" MsgSource="MStorePaymentInstallment" MaxLength="30" Width="150px"
								InputMandatory="False" MsgTextField="InterestPercentage" CustomFormat="none" InputFilter="none"></fwc:MsgTextBox>
						</td>
					</tr>
    			    <tr>
						<td class="tdLabelFRM">
							<asp:label id="Label8" runat="server">Tipo parcelamento</asp:label>
						</td>
						<td class="tdCampoFRM" align="left">
                        	<fwc:MsgDropDownList id="MsgDropDownList4" runat="server" MsgSource="MStorePaymentInstallment" MsgListSourceType="Enumeration"
								MsgListItemsSource="InstallmentTypeEnum" MsgSourceField="InstallmentType" MsgSelectedValueField="InstallmentType" Width="256px" InputMandatory="True"></fwc:MsgDropDownList>
						</td>
					</tr>
    			    <tr>
						<td class="tdLabelFRM">
							<asp:label id="Label9" runat="server">Habilita pagamento parcial</asp:label>
						</td>
						<td class="tdCampoFRM" align="left">
                            <fwc:MsgCheckBox id="MsgCheckBox1" runat="server" MsgSource="MStorePaymentInstallment" MsgCheckField="AllowInParcialPayment" Width="20px"></fwc:MsgCheckBox>						
                        </td>
					</tr>
				</table>
			</td>
		</tr>
	</table>
</asp:Content>
