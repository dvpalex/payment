<%@ Page Language="C#" MasterPageFile="~/Default.master" AutoEventWireup="true" CodeFile="transactionlist.aspx.cs" Inherits="Views_transactionlist" %>
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
	<!--Painel de Formulario-->
	<table class="tbBarraFerramentasSUM">
		<tr>
			<td class="tdBarraFerramentasSUM"><asp:Label id="Label1" runat="server">Listagem de transações</asp:Label></td>
		</tr>
	</table>
	<fwc:MsgDataGrid runat="server" id="grdTransaction" MsgSource="MCPaymentAttempt" OnMessageEvent="grdTransaction_MessageEvent" AllowSorting="True" OnSortCommand="grdTransaction_SortCommand">
		<Columns>
			<fwc:EventColumn CustomFormat="None" Label="../App_Themes/default/images/icon_ver.gif" ButtonType="Image" Event="ShowTransactionDetail" CustomTotalFormat="None" CausesValidation="False" Totalizar="False" HeaderText="Detalhes"></fwc:EventColumn>
			<fwc:MessageColumn CustomFormat="None" MaxLength="0" id="emcOrderStatus" Field="Order.Status" CustomTotalFormat="None" Totalizar="False" HeaderText="Status Pedido" HelpAction=""></fwc:MessageColumn>
			<fwc:MessageColumn CustomFormat="None" MaxLength="0" id="emcStoreReferenceOrder" Field="Order.StoreReferenceOrder" CustomTotalFormat="None" Totalizar="False" HeaderText="Pedido Loja" HelpAction=""></fwc:MessageColumn>
			<fwc:MessageColumn CustomFormat="None" MaxLength="0" id="emcPaymentFormName" Field="PaymentForm.Name" CustomTotalFormat="None" Totalizar="False" HeaderText="Forma de pagamento" HelpAction=""></fwc:MessageColumn>
			<fwc:MessageColumn CustomFormat="NumDecimal_2casa" MaxLength="0" id="emcTotalAmount" Field="Order.TotalAmount" CustomTotalFormat="None" Totalizar="False" HeaderText="Valor Pedido" HelpAction=""></fwc:MessageColumn>
			<fwc:MessageColumn CustomFormat="None" MaxLength="0" id="emcInstallmentQuantity" Field="Order.InstallmentQuantity" CustomTotalFormat="None" Totalizar="False" HeaderText="Qtde Parcelas" HelpAction=""></fwc:MessageColumn>
			<fwc:MessageColumn CustomFormat="DataHora" MaxLength="0" id="emcStartDate" Field="StartTime" CustomTotalFormat="None" Totalizar="False" HeaderText="Data Transa&#231;&#227;o" HelpAction=""></fwc:MessageColumn>
			<fwc:MessageColumn CustomFormat="None" MaxLength="0" id="emcStatus" Field="Status" CustomTotalFormat="None" Totalizar="False" HeaderText="Status Cobrança" HelpAction=""></fwc:MessageColumn>	
		</Columns>
	</fwc:MsgDataGrid>
</asp:Content>