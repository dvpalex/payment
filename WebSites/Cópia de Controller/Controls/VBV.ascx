<%@ Control Language="C#" AutoEventWireup="true" CodeFile="VBV.ascx.cs" Inherits="Controls_VBV" %>
<table class="tbBarraFerramentasSUM">
		<tr>
			<td class="tdBarraFerramentasSUM"><asp:Label id="Label4" runat="server"> Informa��es do Visa (Verified by VISA)</asp:Label></td>
			<td class="tdBarraFerramentasAcaoSUM" style="height: 20px"><asp:Button id="lnkCapturar" runat="server" OnClick="lnkCaptura_Click" Text="Capturar" Visible="False" /></td>
		</tr>
</table>
<table class="tbPainelSUM">
		<tr>
			<td class="tdLabelSUM"><asp:Label id="Label11" runat="server">TID (Identificador da transa��o)</asp:Label></td>
		    <td class="tdValorSUM"><asp:Label id="lblTID" runat="server"></asp:Label></td>
		</tr>
		<tr>
			<td class="tdLabelSUM"><asp:Label id="Label12" runat="server">C�digo de Autoriza��o</asp:Label></td>
			<td class="tdValorSUM"><asp:Label id="lblAuthentic" runat="server"></asp:Label></td>
		</tr>
		<tr>
			<td class="tdLabelSUM"><asp:Label id="Label1" runat="server">Etapa de comunica��o com a VISA</asp:Label></td>
			<td class="tdValorSUM"><asp:Label id="lblVbvStatus" runat="server"></asp:Label></td>
		</tr>
	</table>
