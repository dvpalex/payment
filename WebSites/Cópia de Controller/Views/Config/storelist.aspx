<%@ Page Language="C#" MasterPageFile="~/Default.master" AutoEventWireup="true" CodeFile="storelist.aspx.cs" Inherits="Views_storelist" %>
<%@ Register TagPrefix="fwc" Namespace="SuperPag.Framework.Web.WebControls" Assembly="SuperPag.Framework" %>

<asp:Content ID="Content1" ContentPlaceHolderID="Conteudo" Runat="Server">
    <div id="SubTitulo">
        <asp:Image ID="imgHome" runat="server" SkinID="imgEmployee" />
        <asp:Label ID="lblNomeUsuario" runat="server" Text="Label"></asp:Label>    
    </div>
	<br />
	<!--Painel de Sumario-->
	<!--Painel de Formulario-->
	<table class="tbBarraFerramentasSUM">
		<tr>
			<td class="tdBarraFerramentasSUM"><asp:Label id="Label1" runat="server">Listagem de transações</asp:Label></td>
    		<td class="tdBarraFerramentasAcaoSUM" style="height: 20px"><asp:LinkButton id="lnkIncluir" runat="server" OnClick="lnkIncluir_Click">Nova loja</asp:LinkButton></td>			
		</tr>
	</table>
	<fwc:MsgDataGrid runat="server" id="grdStore" MsgSource="MCStore" OnMessageEvent="grdStore_MessageEvent">
		<Columns>
			<fwc:EventColumn CustomFormat="None" Label="../../App_Themes/default/images/icon_ver.gif" ButtonType="Image" Event="ShowTransactionDetail" CustomTotalFormat="None" CausesValidation="False" Totalizar="False" HeaderText="Detalhes"></fwc:EventColumn>
			<fwc:MessageColumn CustomFormat="None" MaxLength="0" id="emcStoreId" Field="StoreId" CustomTotalFormat="None" Totalizar="False" HeaderText="Código Loja" HelpAction=""></fwc:MessageColumn>
			<fwc:MessageColumn CustomFormat="None" MaxLength="0" id="emcStoreName" Field="Name" CustomTotalFormat="None" Totalizar="False" HeaderText="Name" HelpAction=""></fwc:MessageColumn>
		</Columns>
	</fwc:MsgDataGrid>
</asp:Content>
