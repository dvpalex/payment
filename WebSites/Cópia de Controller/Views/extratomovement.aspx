<%@ Page Language="C#" MasterPageFile="~/Default.master" AutoEventWireup="true" CodeFile="extratomovement.aspx.cs"
    Inherits="Views_extratomovement" %>

<%@ Register TagPrefix="fwc" Namespace="SuperPag.Framework.Web.WebControls" Assembly="SuperPag.Framework" %>
<asp:Content ID="Content1" ContentPlaceHolderID="Conteudo" runat="Server">
    <div id="SubTitulo">
        <asp:Image ID="imgHome" runat="server" SkinID="imgEmployee" />
        <asp:Label ID="lblNomeUsuario" runat="server" Text="Label"></asp:Label>
    </div>
    <br />
    <!--Painel de Sumario-->
    <table class="tbBarraDeAcoesBAC">
        <tr>
            <td class="tdBarraDeAcoesBAC">
                <fwc:EventButton ID="btnGoBack" runat="server" EventName="GoBack" Text="Voltar" CausesValidation="false"
                    OnClick="btnGoBack_Click"></fwc:EventButton>                    
            </td>
       		<td class="tdBarraDeAcoesBAC" style="height: 17px"><fwc:ButtonExportExcel id="ButtonExportExcel1" runat="server" BackColor="LightSteelBlue" BorderColor="CornflowerBlue"
										Text="Exportar para excel" SeparatorType="TAB" ExportType="EXCEL" Width="172px"></fwc:ButtonExportExcel></td>	
        </tr>
    </table>
    <!--Painel de Formulario-->
    <table class="tbBarraFerramentasSUM">
        <tr>
            <td class="tdBarraFerramentasSUM">
                <asp:Label ID="Label1" runat="server">Listagem de transações</asp:Label></td>
        </tr>
    </table>
    <fwc:MsgDataGrid runat="server" ID="grdTransaction" MsgSource="MCExtrato" OnMessageEvent="grdTransaction_MessageEvent"
        AllowSorting="False" AllowPaging="false">
        <Columns>
            <fwc:EventColumn CustomFormat="None" Label="../App_Themes/default/images/icon_ver.gif"
                ButtonType="Image" Event="ShowTransactionDetail" CustomTotalFormat="None" CausesValidation="False"
                Totalizar="False" HeaderText="Detalhes">
            </fwc:EventColumn>
            <fwc:MessageColumn CustomFormat="DataHora" MaxLength="0" id="emcStartDate" Field="Date"
                SortExpression="Date" CustomTotalFormat="None" Totalizar="False" HeaderText="Data Venc."
                HelpAction="">
            </fwc:MessageColumn>
            <fwc:MessageColumn CustomFormat="DataHora" MaxLength="0" id="emcPaymentDate" Field="PaymentDate"
                SortExpression="Date" CustomTotalFormat="None" Totalizar="False" HeaderText="Data Pagto"
                HelpAction="">
            </fwc:MessageColumn>
            <fwc:MessageColumn CustomFormat="NumDecimal_2casa" MaxLength="0" id="emcTotalAmount"
                Field="Value" CustomTotalFormat="None" Totalizar="False" HeaderText="Valor" HelpAction="">
            </fwc:MessageColumn>
            <fwc:MessageColumn CustomFormat="None" MaxLength="0" id="emcDocument" Field="Document"
                SortExpression="Order.StoreReferenceOrder" CustomTotalFormat="None" Totalizar="False"
                HeaderText="Pedido" HelpAction="">
            </fwc:MessageColumn>
            <fwc:MessageColumn CustomFormat="None" MaxLength="0" id="emcStatus" Field="StatusName"
                SortExpression="PaymentForm.Name" CustomTotalFormat="None" Totalizar="False"
                HeaderText="Status" HelpAction="">
            </fwc:MessageColumn>
        </Columns>
    </fwc:MsgDataGrid><br />
    Total pago
    <fwc:MsgLabel ID="MsgLabel2" runat="server" BindValueKey="searchPaidValue" CustomFormat="NumDecimal_2casa"
        DataTypeBind="Value" Text="Label"></fwc:MsgLabel>&nbsp;<br />
    Total não pago
    <fwc:MsgLabel ID="MsgLabel4" runat="server" BindValueKey="searchNonPaidValue" CustomFormat="NumDecimal_2casa"
        DataTypeBind="Value" Text="Label"></fwc:MsgLabel>
</asp:Content>