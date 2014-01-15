<%@ Control Language="C#" AutoEventWireup="true" CodeFile="DepId.ascx.cs" Inherits="Controls_DepId" %>
<%@ Register TagPrefix="fwc" Namespace="SuperPag.Framework.Web.WebControls" Assembly="SuperPag.Framework" %>

<style type="text/css">
.hide
{
    display: none;
}
</style>
<table class="tbBarraFerramentasSUM">
		<tr>
			<td class="tdBarraFerramentasSUM" style="width: 240px; height: 21px;"><asp:Label id="lblDepId" runat="server" Width="252px"> Informações do Depósito Identificado</asp:Label></td>
		</tr>
</table>
<table class="tbPainelSUM">
	<tr>
		<td class="tdLabelSUM"><asp:Label id="lblNumIdentificacao" runat="server">Número Identificação</asp:Label></td>
		<td class="tdValorSUM"><asp:Label id="lblNumIdentificacaoValue" runat="server"></asp:Label></td>
	</tr>
	<tr>
		<td class="tdLabelSUM"><asp:Label id="lblStatus" runat="server">Status</asp:Label></td>
		<td class="tdValorSUM"><asp:Label id="lblStatusName" runat="server"></asp:Label></td>
	</tr>
	<tr>
		<td class="tdLabelSUM"><asp:Label id="Label1" runat="server">Valor Total Confirmado</asp:Label></td>
		<td class="tdValorSUM"><asp:Label id="lblValorTotalConfirmado" runat="server"></asp:Label></td>
	</tr>
</table>
<table id="tblDepReturns" runat="server" class="tbBarraFerramentasSUM">
		<tr>
			<td class="tdBarraFerramentasSUM" style="width: 761px; height: 21px;"><asp:Label id="lblDetalheDepId" runat="server" Width="252px">Detalhamento dos Depósitos</asp:Label></td>
		</tr>
</table>
<asp:GridView ID="grdDeposits" runat="server" AutoGenerateColumns="False" OnRowDataBound="grdDeposits_RowDataBound" OnSelectedIndexChanged="grdDeposits_SelectedIndexChanged" OnDataBound="grdDeposits_DataBound" UseAccessibleHeader="False">
    <Columns>
        <asp:BoundField DataField="CheckId" HeaderText="CheckId" HeaderStyle-CssClass="hide" ItemStyle-CssClass="hide" FooterStyle-CssClass="hide"/>
        <asp:BoundField DataField="Tipo" HeaderText="Tipo do Dep&#243;sito" />
        <asp:BoundField DataField="Valor" HeaderText="Valor" />
        <asp:BoundField DataField="Data" HeaderText="Data" />
        <asp:BoundField DataField="NumDocto" HeaderText="N&#250;mero Documento" />
        <asp:BoundField DataField="Cod_Depositante" HeaderText="C&#243;digo do depositante" />
        <asp:BoundField DataField="Agencia" HeaderText="Ag&#234;ncia acolhedora" />
        <asp:BoundField DataField="Num_Cheque" HeaderText="N&#250;mero Cheque" />
        <asp:BoundField DataField="Status" HeaderText="Status" />
        <asp:ButtonField ButtonType="Button" Text="Confirmar" CommandName="select" InsertVisible="False" />
    </Columns>
</asp:GridView>


