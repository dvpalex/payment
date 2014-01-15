<%@ Page Language="C#" MasterPageFile="~/Store/default/sp.master" AutoEventWireup="true" CodeFile="tryagain.aspx.cs" Inherits="tryagain" Title="Untitled Page" %>
<asp:Content ID="Content2" ContentPlaceHolderID="tableTop" runat="Server">
    <asp:PlaceHolder ID="plhTableTop" runat="server" />
</asp:Content>
<asp:Content ID="Content1" ContentPlaceHolderID="content" runat="Server">
    <asp:PlaceHolder ID="plhTransactionInfo" runat="server" />
    <br />
    <asp:HyperLink id="lnkEnd" runat="server" NavigateUrl="~/payment.aspx" meta:resourcekey="lnkEndResource1">&gt;&gt; Clique aqui para efetuar uma nova tentativa de pagamento</asp:HyperLink>
    <br />
    <div class="buttonlnkReturn"><asp:HyperLink id="lnkReturn" runat="server" ImageUrl="~/Store/default/images/retornarloja.gif" NavigateUrl="#" Visible="false" Target="_parent">Retornar à Loja</asp:HyperLink></div>
</asp:Content>
