<%@ Page Language="C#" MasterPageFile="~/Default.master" AutoEventWireup="true" CodeFile="ordertransactiondetail.aspx.cs" Inherits="Views_ordertransactiondetail" %>
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
  	<!--Painel de Sumario-->
	<table class="tbBarraFerramentasSUM">
		<tr>
			<td class="tdBarraFerramentasSUM"><asp:Label id="Label4" runat="server"> Informações do Meio de Pagamento</asp:Label></td>
			<td class="tdBarraFerramentasAcaoSUM" style="height: 20px"><asp:Button id="lnkPostConfimacao" runat="server" OnClick="lnkPostConfimacao_Click" Text="Enviar Post Finalização" /></td>
			<td class="tdBarraFerramentasAcaoSUM" style="height: 20px"><asp:Button id="lnkPostPagamento" runat="server" OnClick="lnkPostPagamento_Click" Text="Enviar Post Pagamento" /></td>
		</tr>
	</table>
	<table class="tbPainelSUM">
		<tr>
			<td class="tdLabelSUM"><asp:Label id="Label11" runat="server">Forma de pagamento</asp:Label></td>
			<td class="tdValorSUM" colspan="4"><fwc:MsgLabel id="Msglabel7" runat="server" MsgSource="MPaymentAttempt" MsgTextField="PaymentForm.Name"></fwc:MsgLabel></td>
		</tr>
		<tr>
			<td class="tdLabelSUM"><asp:Label id="Label12" runat="server">Código de identificação da transação</asp:Label></td>
			<td class="tdValorSUM" colspan="4"><fwc:MsgLabel id="Msglabel8" runat="server" MsgSource="MPaymentAttempt" MsgTextField="PaymentAttemptId"></fwc:MsgLabel></td>
		</tr>
		<tr>
			<td class="tdLabelSUM"><asp:Label id="Label29" runat="server">Status</asp:Label></td>
			<td class="tdValorSUM" colspan="4"><fwc:MsgLabel id="Msglabel23" runat="server" MsgSource="MPaymentAttempt" MsgTextField="Status"></fwc:MsgLabel></td>
		</tr>
		<tr>
			<td class="tdLabelSUM"><asp:Label id="Label30" runat="server">Mensagem operadora</asp:Label></td>
			<td class="tdValorSUM" colspan="4"><fwc:MsgLabel id="Msglabel26" runat="server" MsgSource="MPaymentAttempt" MsgTextField="ReturnMessage"></fwc:MsgLabel></td>
		</tr>
	</table>
    <asp:PlaceHolder ID="phTransactionDetail" runat="server"></asp:PlaceHolder>
</asp:Content>
