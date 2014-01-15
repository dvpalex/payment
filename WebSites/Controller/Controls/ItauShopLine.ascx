<%@ Control Language="C#" AutoEventWireup="true" CodeFile="ItauShopLine.ascx.cs" Inherits="Controls_ItauShopLine" %>
<table class="tbBarraFerramentasSUM">
		<tr>
			<td class="tdBarraFerramentasSUM"><asp:Label id="Label4" runat="server"> Informações do Itaú ShopLine</asp:Label></td>
		</tr>
</table>
<table class="tbPainelSUM">
		<tr>
			<td class="tdLabelSUM"><asp:Label id="Label11" runat="server">NSU da transação</asp:Label></td>
		    <td class="tdValorSUM"><asp:Label id="lblNSU" runat="server"></asp:Label></td>
		</tr>
		<tr>
			<td class="tdLabelSUM"><asp:Label id="Label1" runat="server">Número comprovante venda</asp:Label></td>
			<td class="tdValorSUM"><asp:Label id="lblCompVend" runat="server"></asp:Label></td>
		</tr>
		<tr>
			<td class="tdLabelSUM"><asp:Label id="Label12" runat="server">Número de autorização</asp:Label></td>
			<td class="tdValorSUM"><asp:Label id="lblAutorizacao" runat="server"></asp:Label></td>
		</tr>
	</table>
