<%@ Control Language="C#" AutoEventWireup="true" CodeFile="boletoBB.ascx.cs" Inherits="Controls_boletoBB" %>
<%@ Register TagPrefix="fwc" Namespace="SuperPag.Framework.Web.WebControls" Assembly="SuperPag.Framework" %>
<script type="text/javascript">
function printBoleto(url)
{
    window.open(url, "Boleto","toolbar=0,location=0,directories=0,status=1,menubar=1,scrollbars=1,resizable=1,screenX=0,screenY=0,left=0,top=0,width=700,height=800");
}    
</script>
<table class="tbBarraFerramentasSUM">
		<tr>
			<td class="tdBarraFerramentasSUM"><asp:Label id="Label4" runat="server"> Informações do Boleto Bancário</asp:Label></td>
			<td class="tdBarraFerramentasAcaoSUM" style="height: 20px"><asp:Button id="lnkPaid" runat="server" OnClick="lnkPagar" Text="Alterar para pago"></asp:Button></td>
			<td class="tdBarraFerramentasAcaoSUM" style="height: 20px"><asp:Button id="lnkReenviar" runat="server" OnClick="lnkReenvia" Text="Enviar boleto via e-mail"></asp:Button></td>
			<td class="tdBarraFerramentasAcaoSUM" style="height: 20px"><asp:Button id="lnkRefazer" runat="server" OnClick="lnkRefazerBoleto" Text="Refazer boleto"></asp:Button></td>
		</tr>
</table>
<table class="tbPainelSUM">
		<tr>
			<td class="tdLabelSUM"><asp:Label id="Label11" runat="server">Número Boleto</asp:Label></td>
			<td class="tdValorSUM">
			<a href="#" onclick="javacript:printBoleto('<%=System.Configuration.ConfigurationManager.AppSettings["urlSuperpag"]%>/agents/boleto/showboleto.aspx?id=<%=PaymentAttemptId%>')"><asp:Label id="lblOct" runat="server"></asp:Label></a></td>
		</tr>
		<tr>
			<td class="tdLabelSUM"><asp:Label id="Label12" runat="server">Data Vencimento</asp:Label></td>
			<td class="tdValorSUM"><asp:Label id="lblDataVencimento" runat="server"></asp:Label></td>
		</tr>
	</table>
