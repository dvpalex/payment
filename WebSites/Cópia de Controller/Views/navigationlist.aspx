<%@ Page Language="C#" MasterPageFile="~/Default.master" AutoEventWireup="true" CodeFile="navigationlist.aspx.cs" Inherits="Views_navigationlist" %>
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
			<td class="tdBarraFerramentasSUM"><asp:Label id="Label1" runat="server">Listagem de navegações</asp:Label></td>
		</tr>
	</table>
	<fwc:MsgDataGrid runat="server" id="grdTransaction" MsgSource="MCNavigationResult" OnMessageEvent="grdTransaction_MessageEvent" AllowSorting="True" OnSortCommand="grdTransaction_SortCommand">
		<Columns>
			<fwc:MessageColumn CustomFormat="None" MaxLength="0" id="emcCPF" Field="CPF" CustomTotalFormat="None" Totalizar="False" HeaderText="Cód. Cliente" HelpAction=""></fwc:MessageColumn>
			<fwc:MessageColumn CustomFormat="None" MaxLength="0" id="emcStoreReferenceOrder" Field="StoreReferenceOrder" CustomTotalFormat="None" Totalizar="False" HeaderText="Pedido Loja" HelpAction=""></fwc:MessageColumn>
			<fwc:MessageColumn CustomFormat="None" MaxLength="0" id="emcDescription" Field="Description" CustomTotalFormat="None" Totalizar="False" HeaderText="Descrição" HelpAction=""></fwc:MessageColumn>
			<fwc:MessageColumn CustomFormat="DataHora" MaxLength="0" id="emcDate" Field="Date" CustomTotalFormat="None" Totalizar="False" HeaderText="Data Acesso" HelpAction=""></fwc:MessageColumn>
		</Columns>
	</fwc:MsgDataGrid>
</asp:Content>