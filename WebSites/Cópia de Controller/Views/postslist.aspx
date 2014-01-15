<%@ Page Language="C#" MasterPageFile="~/Default.master" AutoEventWireup="true" CodeFile="postslist.aspx.cs" Inherits="Views_postslist" %>
<%@ Register TagPrefix="fwc" Namespace="SuperPag.Framework.Web.WebControls" Assembly="SuperPag.Framework" %>

<asp:Content ID="Content1" ContentPlaceHolderID="Conteudo" Runat="Server">
    <div id="SubTitulo">
        <asp:Image ID="imgHome" runat="server" SkinID="imgEmployee" />
        <asp:Label ID="lblNomeUsuario" runat="server" Text="Label"></asp:Label>    
    </div>
	<br />
	<br />
    <!--Painel de Formulario-->
	<div>
      <table id="tblDados" cellspacing="0" cellpadding="0" style="width: 90%; text-align: center; border-width: 3px; border-color: cornflowerblue;">
		<tr>
		  <td style="vertical-align: middle">
		      <table width="95%" align="center" bgColor="lightsteelblue" border="0">
				<tr>
				    <td style="WIDTH: 167px" width="167"><font face="Tahoma" size="2">Configuração de loja não definida pra este usuário</font></td>
			    </tr>
	     	 </table>
          </td>
        </tr>
      </table>
   </div>
</asp:Content>