<%@ Control Language="C#" AutoEventWireup="true" CodeFile="Komerci.ascx.cs" Inherits="Controls_Komerci" %>
<table class="tbBarraFerramentasSUM">
		<tr>
			<td class="tdBarraFerramentasSUM"><asp:Label id="Label4" runat="server"> Informações do Komerci</asp:Label></td>
		</tr>
</table>
<table class="tbPainelSUM">
		<tr>
			<td class="tdLabelSUM"><asp:Label id="Label11" runat="server">Código de identificação</asp:Label></td>
		    <td class="tdValorSUM"><asp:Label id="lblAuthentic" runat="server"></asp:Label></td>
		</tr>
		<tr>
			<td class="tdLabelSUM"><asp:Label id="Label12" runat="server">Código de autorização</asp:Label></td>
			<td class="tdValorSUM"><asp:Label id="lblAutorizacao" runat="server"></asp:Label></td>
		</tr>
		<tr>
			<td class="tdLabelSUM"><asp:Label id="Label1" runat="server">Identificação da transação (NSU)</asp:Label></td>
			<td class="tdValorSUM"><asp:Label id="lblNSU" runat="server"></asp:Label></td>
		</tr>
	</table>
