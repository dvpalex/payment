<%@ Page Language="C#" MasterPageFile="~/Default.master" AutoEventWireup="true" CodeFile="extratofinancial.aspx.cs" Inherits="Views_extratofinancial" %>
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
			<td class="tdBarraDeAcoesBAC" style="height: 17px"><fwc:EventButton id="btnGoBack" runat="server" EventName="GoBack" text="Voltar" CausesValidation="false" OnClick="btnGoBack_Click"></fwc:EventButton></td>
			<td class="tdBarraDeAcoesBAC" style="height: 17px"><fwc:ButtonExportExcel id="ButtonExportExcel1" runat="server" BackColor="LightSteelBlue" BorderColor="CornflowerBlue"
										Text="Exportar para excel" SeparatorType="TAB" ExportType="EXCEL" Width="172px"></fwc:ButtonExportExcel></td>
		</tr>
	</table>
    Saldo em
    <fwc:MsgLabel ID="lblPreviousDate" runat="server" Text="Label" BindValueKey="previousDate" CustomFormat="Data" DataTypeBind="Value"></fwc:MsgLabel>:
    <fwc:MsgLabel ID="MsgLabel1" runat="server" BindValueKey="previousTotal" CustomFormat="NumDecimal_2casa"
        DataTypeBind="Value" Text="Label"></fwc:MsgLabel>&nbsp;<br />
	<!--Painel de Formulario-->
	<table class="tbBarraFerramentasSUM">
		<tr>
			<td class="tdBarraFerramentasSUM"><asp:Label id="Label1" runat="server">Listagem de transações</asp:Label></td>
		</tr>
	</table>
	<fwc:MsgDataGrid runat="server" id="grdTransaction" MsgSource="MCExtrato" OnMessageEvent="grdTransaction_MessageEvent" AllowSorting="False" AllowPaging="false">
		<Columns>
			<fwc:EventColumn CustomFormat="None" Label="../App_Themes/default/images/icon_ver.gif" ButtonType="Image" Event="ShowTransactionDetail" CustomTotalFormat="None" CausesValidation="False" Totalizar="False" HeaderText="Detalhes"></fwc:EventColumn>			
			<fwc:MessageColumn CustomFormat="DataHora" MaxLength="0" id="emcStartDate" Field="Date" SortExpression="Date" CustomTotalFormat="None" Totalizar="False" HeaderText="Data" HelpAction=""></fwc:MessageColumn>
			<fwc:MessageColumn CustomFormat="None" MaxLength="0" id="emcTipoOp" Field="GroupName" CustomTotalFormat="None" Totalizar="False" HeaderText="Tipo Op." SortExpression="Tipo Op." HelpAction=""></fwc:MessageColumn>
			<fwc:MessageColumn CustomFormat="None" MaxLength="0" id="emcDocument" Field="Document" SortExpression="Order.StoreReferenceOrder" CustomTotalFormat="None" Totalizar="False" HeaderText="Pedido" HelpAction=""></fwc:MessageColumn>			
			<fwc:MessageColumn CustomFormat="None" MaxLength="0" id="emcConsumerName" Field="ConsumerName" SortExpression="PaymentForm.Name" CustomTotalFormat="None" Totalizar="False" HeaderText="Nome do Cliente" HelpAction=""></fwc:MessageColumn>
			<fwc:MessageColumn CustomFormat="NumDecimal_2casa" MaxLength="0" id="emcTotalAmount" Field="Value" CustomTotalFormat="None" Totalizar="False" HeaderText="Valor" HelpAction=""></fwc:MessageColumn>			
		</Columns>
	</fwc:MsgDataGrid><br />
    Saldo de
    <fwc:MsgLabel ID="MsgLabel2" runat="server" BindValueKey="startDate" CustomFormat="Data"
        DataTypeBind="Value" Text="Label"></fwc:MsgLabel>
    a
    <fwc:MsgLabel ID="MsgLabel4" runat="server" BindValueKey="endDate" CustomFormat="Data"
        DataTypeBind="Value" Text="Label"></fwc:MsgLabel>:
    <fwc:MsgLabel ID="MsgLabel3" runat="server" BindValueKey="searchValue" CustomFormat="NumDecimal_2casa"
        DataTypeBind="Value" Text="Label"></fwc:MsgLabel>&nbsp;
</asp:Content>