<%@ Page Language="C#" MasterPageFile="~/Default.master" AutoEventWireup="true" CodeFile="extratofinancial2.aspx.cs" Inherits="Views_extratofinancial2" %>
<%@ Register TagPrefix="fwc" Namespace="SuperPag.Framework.Web.WebControls" Assembly="SuperPag.Framework" %>
<asp:Content ID="Content1" ContentPlaceHolderID="Conteudo" Runat="Server">
    <div id="SubTitulo">
        <asp:Image ID="imgHome" runat="server" SkinID="imgEmployee" />
        <asp:Label ID="lblNomeUsuario" runat="server" Text="Label"></asp:Label>    
    </div>
	<br />
	<table class="tbBarraDeAcoesBAC">
		<tr>
			<td class="tdBarraDeAcoesBAC" style="height: 17px"><fwc:EventButton id="btnGoBack" runat="server" EventName="GoBack" text="Voltar" CausesValidation="false" OnClick="btnGoBack_Click"></fwc:EventButton></td>
			<td class="tdBarraDeAcoesBAC" style="height: 17px">
			    <fwc:ButtonExportExcel id="ButtonExportExcel1" runat="server" BackColor="LightSteelBlue" BorderColor="CornflowerBlue" Text="Download do Extrato" SeparatorType="TAB" ExportType="EXCEL" EncodeType="ISO" Width="172px"></fwc:ButtonExportExcel></td>
		</tr>
	</table>
</asp:Content>