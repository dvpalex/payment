<%@ Page Language="C#" MasterPageFile="~/Default.master" AutoEventWireup="true" CodeFile="storepaymentformdetail.aspx.cs" Inherits="Views_storedetail" %>
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
			<td class="tdBarraDeAcoesBAC"><fwc:EventButton id="btnGoBack" runat="server" EventName="GoBack" text="Voltar" CausesValidation="false" OnClick="btnGoBack_Click"></fwc:EventButton></td>
		</tr>
	</table>

	<table class="tbBarraFerramentasSUM">
		<tr>
			<td class="tdBarraFerramentasSUM"><asp:Label id="lblTituloSumario_1" runat="server">Informações da loja</asp:Label></td>
		</tr>
	</table>
	<table class="tbPainelSUM">
		<tr>
			<td class="tdLabelSUM"><asp:Label id="Label2" runat="server">Código Loja</asp:Label></td>
			<td class="tdValorSUM" colspan="2"><fwc:MsgLabel id="Msglabel17" runat="server" MsgSource="MStore" MsgTextField="StoreId"></fwc:MsgLabel></td>
			<td class="tdLabelSUM"><asp:Label id="Label5" runat="server">Nome Loja</asp:Label></td>
			<td class="tdValorSUM" colspan="2"><fwc:MsgLabel id="Msglabel5" runat="server" MsgSource="MStore" MsgTextField="Name"></fwc:MsgLabel></td>
		</tr>
	</table>
	<table class="tbBarraFerramentasSUM">
		<tr>
			<td class="tdBarraFerramentasSUM"><asp:Label id="Label1" runat="server">Informações da forma de pagamento</asp:Label></td>
		</tr>
	</table>
	<table class="tbPainelSUM">
		<tr>
			<td class="tdLabelSUM"><asp:Label id="Label3" runat="server">Código Forma Pagamento</asp:Label></td>
			<td class="tdValorSUM" colspan="2"><fwc:MsgLabel id="Msglabel1" runat="server" MsgSource="MStorePaymentForm" MsgTextField="PaymentForm.PaymentFormId"></fwc:MsgLabel></td>
			<td class="tdLabelSUM"><asp:Label id="Label4" runat="server">Descrição</asp:Label></td>
			<td class="tdValorSUM" colspan="2"><fwc:MsgLabel id="Msglabel2" runat="server" MsgSource="MStorePaymentForm" MsgTextField="PaymentForm.Name"></fwc:MsgLabel></td>
		</tr>
	</table>
	<!--Painel de Registros de Funcionarios-->
	<table class="tbBarraFerramentasSUM">
		<tr>
			<td class="tdBarraFerramentasSUM"><asp:Label id="Label9" runat="server">Parcelamentos disponíveis</asp:Label></td>
    		<td class="tdBarraFerramentasAcaoSUM" style="height: 20px"><asp:LinkButton id="lnkIncluir" runat="server" OnClick="lnkIncluir_Click">Incluir parcelamento</asp:LinkButton></td>			
		</tr>
	</table>
    <fwc:MsgDataGrid id="grdStorePaymentInstallments" ShowFooter="False" runat="server" MsgSource="MStorePaymentForm" MsgSourceField="StorePaymentInstallments" OnMessageEvent="grdStorePaymentInstallments_MessageEvent">
		<Columns>
		    <fwc:EventColumn CustomFormat="None" Label="../../App_Themes/default/images/icon_edit.gif" ButtonType="Image" Event="EditStorePaymentForm" CustomTotalFormat="None" CausesValidation="False" Totalizar="False" HeaderText="Editar"></fwc:EventColumn>
			<fwc:MessageColumn CustomFormat="None" MaxLength="0" id="emcInstallmentNumber" Field="InstallmentNumber" CustomTotalFormat="None" Totalizar="False" HeaderText="Qtde Parcela" HelpAction=""></fwc:MessageColumn>
			<fwc:MessageColumn CustomFormat="None" MaxLength="0" id="emcDescription" Field="Description" CustomTotalFormat="None" Totalizar="False" HeaderText="Descrição" HelpAction=""></fwc:MessageColumn>
			<fwc:MessageColumn CustomFormat="NumDecimal_2casa" MaxLength="0" id="emcMinValue" Field="MinValue" CustomTotalFormat="None" Totalizar="False" HeaderText="Valor mínio" HelpAction=""></fwc:MessageColumn>
			<fwc:MessageColumn CustomFormat="NumDecimal_2casa" MaxLength="0" id="emcMaxValue" Field="MaxValue" CustomTotalFormat="None" Totalizar="False" HeaderText="Valor máxim" HelpAction=""></fwc:MessageColumn>
			<fwc:MessageColumn CustomFormat="NumDecimal_4casa" MaxLength="0" id="emcInterestPercentage" Field="InterestPercentage" CustomTotalFormat="None" Totalizar="False" HeaderText="Juros" HelpAction=""></fwc:MessageColumn>
			<fwc:MessageColumn CustomFormat="None" MaxLength="0" id="emcInstallmentType" Field="InstallmentType" CustomTotalFormat="None" Totalizar="False" HeaderText="Tipo parcelamento" HelpAction=""></fwc:MessageColumn>
			<fwc:CheckBoxColumn id="emcAllowInParcialPayment" CheckedField="AllowInParcialPayment" HeaderText="Pag. Parcial" Enabled="false" ></fwc:CheckBoxColumn>
		</Columns>
    </fwc:MsgDataGrid>
</asp:Content>
