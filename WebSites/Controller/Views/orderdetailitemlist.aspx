<%@ Page Language="C#" MasterPageFile="~/Default.master" AutoEventWireup="true" CodeFile="orderdetailitemlist.aspx.cs" Inherits="Views_orderdetailitemlist" %>
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
			<td class="tdBarraFerramentasSUM"><asp:Label id="Label1" runat="server">Listagem de Pedidos</asp:Label></td>
		</tr>
	</table>
	<fwc:MsgDataGrid runat="server" id="grdOrder" MsgSource="MCOrderDetailItem" OnMessageEvent="grdOrder_MessageEvent" AllowSorting="True" OnSortCommand="grdOrder_SortCommand">
		<Columns>
			<fwc:EventColumn CustomFormat="None" Label="../App_Themes/default/images/icon_ver.gif" ButtonType="Image" Event="ShowTransactionDetail" CustomTotalFormat="None" CausesValidation="False" Totalizar="False" HeaderText="Detalhes"></fwc:EventColumn>
			<fwc:MessageColumn CustomFormat="None" MaxLength="0" id="emcOrderStatus" Field="Status" CustomTotalFormat="None" Totalizar="False" HeaderText="Status Pedido" HelpAction=""></fwc:MessageColumn>
			<fwc:MessageColumn CustomFormat="None" MaxLength="0" id="emcStoreReferenceOrder" Field="StoreReferenceOrder" CustomTotalFormat="None" Totalizar="False" HeaderText="Pedido Loja" HelpAction="" ItemStyle-Wrap="false"></fwc:MessageColumn>
			<fwc:MessageColumn CustomFormat="None" MaxLength="0" id="emcItemsDesc" Field="ItemsDesc" CustomTotalFormat="None" Totalizar="False" HeaderText="Itens Loja" HelpAction="" ItemStyle-Wrap="true"></fwc:MessageColumn>
			<fwc:MessageColumn CustomFormat="NumDecimal_2casa" MaxLength="0" id="emcFinalAmount" Field="FinalAmount" CustomTotalFormat="None" Totalizar="False" HeaderText="Valor" HelpAction="" ItemStyle-Wrap="false"></fwc:MessageColumn>
			<fwc:MessageColumn CustomFormat="None" MaxLength="0" id="emcInstallmentQuantity" Field="InstallmentQuantity" CustomTotalFormat="None" Totalizar="False" HeaderText="Qtde Parcelas" HelpAction=""></fwc:MessageColumn>
			<fwc:MessageColumn CustomFormat="DataHora" MaxLength="0" id="emcStartDate" Field="CreationDate" CustomTotalFormat="None" Totalizar="False" HeaderText="Data do pedido" HelpAction="" ItemStyle-Wrap="false"></fwc:MessageColumn>
		</Columns>
	</fwc:MsgDataGrid>
</asp:Content>