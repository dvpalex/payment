<%@ Page Language="C#" MasterPageFile="~/Default.master" AutoEventWireup="true" CodeFile="paymentagentsetuplist.aspx.cs" Inherits="Views_paymentagentsetuplist" %>
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
			<td class="tdBarraFerramentasSUM"><asp:Label id="Label1" runat="server">Listagem de configurações dos agentes de pagamentos</asp:Label></td>
    		<td class="tdBarraFerramentasAcaoSUM" style="height: 20px"><asp:LinkButton id="lnkIncluir" runat="server" OnClick="lnkIncluir_Click">Incluir configuração</asp:LinkButton></td>			
		</tr>
	</table>
	<fwc:MsgDataGrid runat="server" id="grdPaymentAgentSetup" MsgSource="MCPaymentAgentSetup" OnMessageEvent="grdPaymentAgentSetup_MessageEvent">
		<Columns>
			<fwc:EventColumn CustomFormat="None" Label="../../App_Themes/default/images/icon_edit.gif" ButtonType="Image" Event="ShowTransactionDetail" CustomTotalFormat="None" CausesValidation="False" Totalizar="False" HeaderText="Detalhes"></fwc:EventColumn>
			<fwc:MessageColumn CustomFormat="None" MaxLength="0" id="emcPaymentAgentSetupId" Field="PaymentAgentSetupId" CustomTotalFormat="None" Totalizar="False" HeaderText="código" HelpAction=""></fwc:MessageColumn>
			<fwc:MessageColumn CustomFormat="None" MaxLength="0" id="emcTitle" Field="Title" CustomTotalFormat="None" Totalizar="False" HeaderText="Título" HelpAction=""></fwc:MessageColumn>
			<fwc:MessageColumn CustomFormat="None" MaxLength="0" id="emcPaymentAgentName" Field="PaymentAgent.Name" CustomTotalFormat="None" Totalizar="False" HeaderText="Nome agente de pagamento" HelpAction=""></fwc:MessageColumn>
		</Columns>
	</fwc:MsgDataGrid>
</asp:Content>
